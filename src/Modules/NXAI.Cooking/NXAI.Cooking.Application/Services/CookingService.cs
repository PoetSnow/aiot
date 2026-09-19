using System.Net;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using NXAI.Cooking.Application.Acl;
using NXAI.Cooking.Application.Contracts.Dtos;
using NXAI.Cooking.Application.Contracts.Events;
using NXAI.Cooking.Application.Contracts.Interfaces;
using NXAI.Infra.EventBus;
using NXAI.Infra.IdGenerater.Yitter;
using NXAI.Infra.Repository;
using NXAI.Shared.Application.Contracts.ResultModels;
using NXAI.Shared.Pot;
using NXAI.Shared.Pot.Enums;
using NXAI.Shared.Pot.Json;
using NXAI.Shared.Pot.Mqtt;
using CookingCommandEntity = NXAI.Cooking.Repository.Entities.CookingCommand;
using CookingExecutionLogEntity = NXAI.Cooking.Repository.Entities.CookingExecutionLog;
using CookingTaskEntity = NXAI.Cooking.Repository.Entities.CookingTask;
using CookingTaskStatus = NXAI.Cooking.Repository.Entities.CookingTaskStatus;
using CookingTaskStepEntity = NXAI.Cooking.Repository.Entities.CookingTaskStep;

namespace NXAI.Cooking.Application.Services;

/// <summary>制作任务状态机。实现见 <see cref="ICookingService"/>。</summary>
public sealed class CookingService(
    IEfRepository<CookingTaskEntity> tasks,
    IEfRepository<CookingTaskStepEntity> steps,
    IEfRepository<CookingCommandEntity> commands,
    IEfRepository<CookingExecutionLogEntity> logs,
    IRecipeGateway recipes,
    IDeviceGateway devices,
    IInventoryGateway inventory,
    IEventPublisher events) : ICookingService
{
    private static readonly HashSet<string> TerminalOk = ["SUCCESS"];
    private static readonly HashSet<string> TerminalFail = ["FAILED", "MISSING", "EXCEPTION", "REJECTED"];

    public async Task<ServiceResult<CookingTaskDto>> CreateAsync(long memberId, CookingTaskCreationDto input)
    {
        var recipe = await recipes.GetPublishedByCodeAsync(input.RecipeCode);
        if (recipe is null)
        {
            return new ProblemDetails(HttpStatusCode.NotFound, "没有已发布配方");
        }

        var device = await devices.GetAsync(input.DeviceId, memberId);
        if (device is null)
        {
            return new ProblemDetails(HttpStatusCode.NotFound, "设备不存在");
        }

        if (device.ActiveTaskId is > 0)
        {
            return new ProblemDetails(HttpStatusCode.Conflict, "设备已有未结束任务");
        }

        if (!await devices.IsOnlineAsync(device.Id, memberId))
        {
            return new ProblemDetails(HttpStatusCode.Conflict, "设备离线，不能启动");
        }

        if (!IsModelCompatible(recipe.CompatibleModels, device.ModelCode))
        {
            return await RejectAsync(memberId, device.Id, recipe, "型号不兼容");
        }

        var slotList = await devices.GetSlotsAsync(device.Id, memberId);
        var resolved = new List<CookingTaskStepEntity>();
        foreach (var step in recipe.Steps.OrderBy(x => x.StepNo))
        {
            var row = new CookingTaskStepEntity
            {
                Id = IdGenerater.GetNextId(),
                StepNo = step.StepNo,
                Action = step.Action.Trim(),
                Mode = step.Mode,
                AmountValue = step.AmountValue,
                AmountUnit = step.AmountUnit,
                TriggerType = string.IsNullOrWhiteSpace(step.TriggerType) ? "IMMEDIATE" : step.TriggerType.Trim(),
                TempCelsius = step.TempCelsius
            };

            if (string.Equals(row.Action, "DISPENSE", StringComparison.OrdinalIgnoreCase))
            {
                var target = step.TargetCode?.Trim() ?? string.Empty;
                var matches = slotList.Where(s => string.Equals(s.MaterialCode, target, StringComparison.OrdinalIgnoreCase)).ToList();
                if (matches.Count != 1)
                {
                    return await RejectAsync(memberId, device.Id, recipe, "投料目标不能解析到唯一仓位");
                }

                var slot = matches[0];
                if (!string.IsNullOrWhiteSpace(row.Mode) &&
                    !slot.SupportedModes.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                        .Contains(row.Mode, StringComparer.OrdinalIgnoreCase))
                {
                    return await RejectAsync(memberId, device.Id, recipe, "仓位模式不匹配");
                }

                row.SlotCode = slot.SlotCode;
                row.ConsumableId = slot.ConsumableId;

                if (string.Equals(row.Mode, "PACKAGE", StringComparison.OrdinalIgnoreCase))
                {
                    var qty = row.AmountValue ?? 1;
                    if (!await inventory.HasEnoughAsync(memberId, target, qty))
                    {
                        return await RejectAsync(memberId, device.Id, recipe, "PACKAGE 余量不足");
                    }
                }
            }

            resolved.Add(row);
        }

        var epoch = await devices.BumpEpochAsync(device.Id);
        var taskId = IdGenerater.GetNextId();
        var lockResult = await devices.TryLockTaskAsync(device.Id, taskId, epoch);
        if (!lockResult.IsSuccess)
        {
            return lockResult;
        }

        var entity = new CookingTaskEntity
        {
            Id = taskId,
            MemberId = memberId,
            DeviceId = device.Id,
            RecipeId = recipe.Id,
            RecipeCode = recipe.Code,
            RecipeVersion = recipe.Version,
            SnapshotJson = JsonSerializer.Serialize(recipe),
            Status = CookingTaskStatus.Running,
            CurrentStepNo = resolved[0].StepNo,
            Epoch = epoch
        };
        await tasks.InsertAsync(entity);
        foreach (var step in resolved)
        {
            step.TaskId = taskId;
            await steps.InsertAsync(step);
        }

        await IssueStepAsync(entity, resolved[0], device);
        var created = await GetAsync(memberId, taskId);
        return created is null
            ? new ProblemDetails(HttpStatusCode.InternalServerError, "任务读取失败")
            : created;
    }

    public async Task<CookingTaskDto?> GetAsync(long memberId, long id)
    {
        var entity = await tasks.FetchAsync(x => x.Id == id);
        if (entity is null || entity.MemberId != memberId)
        {
            return null;
        }

        var stepList = await steps.GetAll().Where(x => x.TaskId == id).OrderBy(x => x.StepNo).ToListAsync();
        return Map(entity, stepList);
    }

    public async Task<ServiceResult> CancelAsync(long memberId, long id)
    {
        var entity = await tasks.FetchAsync(x => x.Id == id, noTracking: false);
        if (entity is null || entity.MemberId != memberId)
        {
            return new ProblemDetails(HttpStatusCode.NotFound, "任务不存在");
        }

        if (entity.Status is CookingTaskStatus.Completed or CookingTaskStatus.Failed or CookingTaskStatus.Rejected or CookingTaskStatus.Cancelled)
        {
            return new ServiceResult();
        }

        entity.Status = CookingTaskStatus.Cancelled;
        await tasks.UpdateAsync(entity);
        await devices.ReleaseTaskAsync(entity.DeviceId, entity.Id);
        var device = await devices.GetAsync(entity.DeviceId, memberId);
        if (device is not null)
        {
            await IssueStopAsync(entity, device);
        }

        return new ServiceResult();
    }

    public async Task HandleAckAsync(long commandId, string result, int? epoch)
    {
        var command = await commands.FetchAsync(x => x.Id == commandId, noTracking: false);
        if (command is null)
        {
            return;
        }

        command.Result = result;
        await commands.UpdateAsync(command);

        var task = await tasks.FetchAsync(x => x.Id == command.TaskId, noTracking: false);
        if (task is null)
        {
            return;
        }

        if (task.Status is CookingTaskStatus.Completed or CookingTaskStatus.Failed or CookingTaskStatus.Rejected or CookingTaskStatus.Cancelled)
        {
            return;
        }

        await logs.InsertAsync(new CookingExecutionLogEntity
        {
            Id = IdGenerater.GetNextId(),
            TaskId = task.Id,
            CommandId = commandId,
            Message = result
        });

        if (string.Equals(result, "RECEIVED", StringComparison.OrdinalIgnoreCase)
            || string.Equals(result, "RUNNING", StringComparison.OrdinalIgnoreCase))
        {
            return;
        }

        if (epoch is not null && epoch != task.Epoch)
        {
            await FailAsync(task, "旧 epoch 被拒");
            return;
        }

        var step = await steps.FetchAsync(x => x.Id == command.StepId, noTracking: false);
        if (step is not null && step.Result is not null && (TerminalOk.Contains(step.Result) || TerminalFail.Contains(step.Result)))
        {
            return;
        }
        if (step is not null)
        {
            step.Result = result;
            await steps.UpdateAsync(step);
        }

        if (TerminalFail.Contains(result))
        {
            await FailAsync(task, result);
            return;
        }

        if (!TerminalOk.Contains(result))
        {
            return;
        }

        var next = await steps.GetAll()
            .Where(x => x.TaskId == task.Id && x.StepNo > task.CurrentStepNo)
            .OrderBy(x => x.StepNo)
            .FirstOrDefaultAsync();
        if (next is null)
        {
            task.Status = CookingTaskStatus.Completed;
            await tasks.UpdateAsync(task);
            await devices.ReleaseTaskAsync(task.DeviceId, task.Id);
            var used = await steps.GetAll().Where(x => x.TaskId == task.Id && x.ConsumableId != null).Select(x => x.ConsumableId!.Value).ToListAsync();
            await events.PublishAsync(CookingTaskCompletedEvent.Topic, new CookingTaskCompletedEvent
            {
                TaskId = task.Id,
                DeviceId = task.DeviceId,
                MemberId = task.MemberId,
                ConsumableIds = used
            });
            return;
        }

        // HEAT SUCCESS 之后才发 TEMP_GTE 的 DISPENSE，禁止用 IMMEDIATE 代替
        task.CurrentStepNo = next.StepNo;
        await tasks.UpdateAsync(task);
        var device = await devices.GetAsync(task.DeviceId, task.MemberId);
        if (device is null)
        {
            await FailAsync(task, "设备已解绑");
            return;
        }

        await IssueStepAsync(task, next, device);
    }

    public async Task FailTimedOutAsync()
    {
        var deadline = DateTime.Now.AddSeconds(-120);
        var running = await tasks.GetAll(noTracking: false)
            .Where(x => x.Status == CookingTaskStatus.Running || x.Status == CookingTaskStatus.Steeping)
            .ToListAsync();
        foreach (var task in running)
        {
            var pending = await commands.GetAll()
                .Where(x => x.TaskId == task.Id && (x.Result == null || x.Result == "RECEIVED" || x.Result == "RUNNING"))
                .OrderByDescending(x => x.Id)
                .FirstOrDefaultAsync();
            if (pending is null || pending.IssuedAt > deadline)
            {
                continue;
            }

            await FailAsync(task, "指令超时");
        }
    }

    private async Task FailAsync(CookingTaskEntity task, string reason)
    {
        if (task.Status is CookingTaskStatus.Completed or CookingTaskStatus.Failed or CookingTaskStatus.Rejected or CookingTaskStatus.Cancelled)
        {
            return;
        }

        task.Status = CookingTaskStatus.Failed;
        task.RejectReason = reason;
        await tasks.UpdateAsync(task);
        await devices.ReleaseTaskAsync(task.DeviceId, task.Id);
        var device = await devices.GetAsync(task.DeviceId, null);
        if (device is not null)
        {
            await IssueStopAsync(task, device);
        }

        await events.PublishAsync(CookingTaskFailedEvent.Topic, new CookingTaskFailedEvent { TaskId = task.Id, Reason = reason });
    }

    private async Task<ServiceResult<CookingTaskDto>> RejectAsync(long memberId, long deviceId, CookingRecipeSnapshot recipe, string reason)
    {
        var entity = new CookingTaskEntity
        {
            Id = IdGenerater.GetNextId(),
            MemberId = memberId,
            DeviceId = deviceId,
            RecipeId = recipe.Id,
            RecipeCode = recipe.Code,
            RecipeVersion = recipe.Version,
            SnapshotJson = JsonSerializer.Serialize(recipe),
            Status = CookingTaskStatus.Rejected,
            RejectReason = reason
        };
        await tasks.InsertAsync(entity);
        return new CookingTaskDto
        {
            Id = entity.Id,
            DeviceId = deviceId,
            RecipeCode = recipe.Code,
            RecipeVersion = recipe.Version,
            Status = entity.Status,
            RejectReason = reason
        };
    }

    private async Task IssueStepAsync(CookingTaskEntity task, CookingTaskStepEntity step, CookingDeviceInfo device)
    {
        var commandId = IdGenerater.GetNextId();
        var seq = 1;
        var key = $"{task.Id}:{step.Id}:{seq}";
        var action = Enum.Parse<CommandAction>(step.Action, ignoreCase: true);
        var trigger = new PotTrigger
        {
            Type = Enum.TryParse<TriggerType>(step.TriggerType, true, out var tt) ? tt : TriggerType.IMMEDIATE,
            TempCelsius = step.TempCelsius
        };

        // DISPENSE 必须带 TEMP_GTE，禁止无 Trigger 立即投
        if (action == CommandAction.DISPENSE && trigger.Type != TriggerType.TEMP_GTE)
        {
            trigger.Type = TriggerType.TEMP_GTE;
            trigger.TempCelsius ??= 80;
        }

        var envelope = new PotMqttEnvelope<PotCommandPayload>
        {
            Schema = PotMqttSchemas.CmdV1,
            MsgId = IdGenerater.GetNextId(),
            Ts = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
            Sn = device.DeviceSn,
            Type = PotMessageType.COMMAND,
            Payload = new PotCommandPayload
            {
                CommandId = commandId,
                TaskId = task.Id,
                StepId = step.Id,
                Epoch = task.Epoch,
                StepNo = step.StepNo,
                Seq = seq,
                IdempotencyKey = key,
                Action = action,
                SlotCode = step.SlotCode,
                Mode = string.IsNullOrWhiteSpace(step.Mode) ? null : Enum.Parse<DispenseMode>(step.Mode, true),
                Amount = step.AmountValue is null ? null : new PotAmount
                {
                    Value = step.AmountValue.Value,
                    Unit = Enum.TryParse<AmountUnit>(step.AmountUnit, true, out var unit) ? unit : AmountUnit.PACK
                },
                Trigger = trigger,
                ExpireAt = DateTimeOffset.UtcNow.AddSeconds(120).ToUnixTimeSeconds()
            }
        };

        await commands.InsertAsync(new CookingCommandEntity
        {
            Id = commandId,
            TaskId = task.Id,
            StepId = step.Id,
            DeviceId = task.DeviceId,
            Seq = seq,
            IdempotencyKey = key,
            Action = step.Action,
            IssuePolicy = action == CommandAction.DISPENSE ? "NO_NEW_ID" : "RETRY_SAME",
            IssuedAt = DateTime.Now
        });
        step.CommandId = commandId;
        await steps.UpdateAsync(step);

        await devices.IssueCommandAsync(task.DeviceId, commandId, PotJson.Serialize(envelope));
    }

    private async Task IssueStopAsync(CookingTaskEntity task, CookingDeviceInfo device)
    {
        var commandId = IdGenerater.GetNextId();
        var envelope = new PotMqttEnvelope<PotCommandPayload>
        {
            Schema = PotMqttSchemas.CmdV1,
            MsgId = IdGenerater.GetNextId(),
            Ts = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
            Sn = device.DeviceSn,
            Type = PotMessageType.COMMAND,
            Payload = new PotCommandPayload
            {
                CommandId = commandId,
                TaskId = task.Id,
                StepId = 0,
                Epoch = task.Epoch,
                StepNo = task.CurrentStepNo,
                Seq = 1,
                IdempotencyKey = $"{task.Id}:stop:1",
                Action = CommandAction.STOP,
                ExpireAt = DateTimeOffset.UtcNow.AddSeconds(60).ToUnixTimeSeconds()
            }
        };
        await devices.IssueCommandAsync(task.DeviceId, commandId, PotJson.Serialize(envelope));
    }

    private static bool IsModelCompatible(string compatible, string modelCode)
    {
        if (string.IsNullOrWhiteSpace(compatible))
        {
            return true;
        }

        return compatible.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Contains(modelCode, StringComparer.OrdinalIgnoreCase);
    }

    private static CookingTaskDto Map(CookingTaskEntity entity, List<CookingTaskStepEntity> stepList) => new()
    {
        Id = entity.Id,
        DeviceId = entity.DeviceId,
        RecipeCode = entity.RecipeCode,
        RecipeVersion = entity.RecipeVersion,
        Status = entity.Status,
        CurrentStepNo = entity.CurrentStepNo,
        Epoch = entity.Epoch,
        RejectReason = entity.RejectReason,
        Steps = stepList.Select(s => new CookingTaskStepDto
        {
            Id = s.Id,
            StepNo = s.StepNo,
            Action = s.Action,
            SlotCode = s.SlotCode,
            CommandId = s.CommandId,
            Result = s.Result
        }).ToList()
    };
}
