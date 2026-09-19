using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using NXAI.Device.Application.Acl;
using NXAI.Device.Application.Contracts.Dtos;
using NXAI.Device.Application.Contracts.Events;
using NXAI.Device.Application.Contracts.Interfaces;
using NXAI.Infra.EventBus;
using NXAI.Infra.IdGenerater.Yitter;
using NXAI.Infra.Repository;
using NXAI.Shared.Application.Contracts.Dtos;
using NXAI.Shared.Application.Contracts.ResultModels;
using DeviceEntity = NXAI.Device.Repository.Entities.Device;
using DeviceCapabilityEntity = NXAI.Device.Repository.Entities.DeviceCapability;
using DeviceMqttOutboxEntity = NXAI.Device.Repository.Entities.DeviceMqttOutbox;
using DeviceShadowEntity = NXAI.Device.Repository.Entities.DeviceShadow;
using DeviceSlotEntity = NXAI.Device.Repository.Entities.DeviceSlot;
using MqttOutboxStatus = NXAI.Device.Repository.Entities.MqttOutboxStatus;
using SlotBindingKind = NXAI.Device.Repository.Entities.SlotBindingKind;

namespace NXAI.Device.Application.Services;

/// <summary>已绑定设备。实现见 <see cref="IDeviceService"/>。</summary>
public sealed class DeviceService(
    IEfRepository<DeviceEntity> devices,
    IEfRepository<DeviceSlotEntity> slots,
    IEfRepository<DeviceShadowEntity> shadows,
    IEfRepository<DeviceCapabilityEntity> capabilities,
    IEfRepository<DeviceMqttOutboxEntity> outbox,
    IAssetSnGateway assetSns,
    IMemberGateway members,
    IInventoryGateway inventory,
    IEventPublisher events) : IDeviceService
{
    public async Task<ServiceResult<IdDto>> BindAsync(long memberId, DeviceBindDto input)
    {
        var sn = input.Sn?.Trim() ?? string.Empty;
        if (sn.Length == 0)
        {
            return new ProblemDetails(HttpStatusCode.BadRequest, "SN 不能为空");
        }

        if (!await members.ExistsActiveAsync(memberId))
        {
            return new ProblemDetails(HttpStatusCode.Forbidden, "会员不可用");
        }

        var existing = await devices.FetchAsync(x => x.DeviceSn == sn);
        if (existing is not null)
        {
            return existing.MemberId == memberId
                ? new IdDto(existing.Id)
                : new ProblemDetails(HttpStatusCode.Conflict, "SN 已被其他会员绑定");
        }

        // 未出库 / 在库 / 维修中：Asset 返回 false，绑定必须失败
        if (!await assetSns.CanMemberClaimAsync(sn, memberId))
        {
            return new ProblemDetails(HttpStatusCode.Conflict, "未出库 SN 不能绑定");
        }

        var snInfo = await assetSns.GetAsync(sn);
        if (snInfo is null)
        {
            return new ProblemDetails(HttpStatusCode.NotFound, "SN 不存在");
        }

        var deviceId = IdGenerater.GetNextId();
        await devices.InsertAsync(new DeviceEntity
        {
            Id = deviceId,
            MemberId = memberId,
            DeviceSn = sn,
            ModelCode = snInfo.Value.ModelCode,
            ActiveEpoch = 0,
            DeviceTokenHash = string.Empty
        });

        foreach (var profile in ParseSlotProfile(await assetSns.GetSlotProfileJsonAsync(snInfo.Value.ModelCode)))
        {
            await slots.InsertAsync(new DeviceSlotEntity
            {
                Id = IdGenerater.GetNextId(),
                DeviceId = deviceId,
                SlotCode = profile.SlotCode,
                SlotType = profile.SlotType,
                SupportedModes = profile.SupportedModes,
                BindingKind = SlotBindingKind.None
            });
        }

        await shadows.InsertAsync(new DeviceShadowEntity
        {
            Id = IdGenerater.GetNextId(),
            DeviceId = deviceId,
            Version = 0,
            Online = false,
            WorkState = "IDLE",
            SlotsOccupiedJson = "[]"
        });

        await capabilities.InsertAsync(new DeviceCapabilityEntity
        {
            Id = IdGenerater.GetNextId(),
            DeviceId = deviceId,
            PayloadJson = "{}",
            ReportedAt = null
        });

        if (!await assetSns.MarkBoundAsync(sn, memberId))
        {
            return new ProblemDetails(HttpStatusCode.Conflict, "SN 状态无法标为已绑定");
        }

        return new IdDto(deviceId);
    }

    public async Task<List<DeviceDto>> GetByMemberAsync(long memberId)
    {
        var list = await devices.GetAll().Where(x => x.MemberId == memberId).OrderByDescending(x => x.Id).ToListAsync();
        return list.Select(Map).ToList();
    }

    public async Task<DeviceDto?> GetAsync(long id, long? memberId)
    {
        var entity = await devices.FetchAsync(x => x.Id == id);
        if (entity is null)
        {
            return null;
        }

        if (memberId is > 0 && entity.MemberId != memberId)
        {
            return null;
        }

        return Map(entity);
    }

    public async Task<DeviceShadowDto?> GetShadowAsync(long id, long? memberId)
    {
        if (await GetAsync(id, memberId) is null)
        {
            return null;
        }

        var shadow = await shadows.FetchAsync(x => x.DeviceId == id);
        return shadow is null
            ? null
            : new DeviceShadowDto(
                shadow.DeviceId,
                shadow.Version,
                shadow.Online,
                shadow.WaterTemp,
                shadow.WorkState,
                shadow.CurrentTaskId,
                shadow.CurrentEpoch,
                shadow.CurrentStepNo);
    }

    public async Task<List<DeviceSlotDto>> GetSlotsAsync(long id, long? memberId)
    {
        if (await GetAsync(id, memberId) is null)
        {
            return [];
        }

        var list = await slots.GetAll().Where(x => x.DeviceId == id).OrderBy(x => x.SlotCode).ToListAsync();
        return list.Select(x => new DeviceSlotDto(x.SlotCode, x.SlotType, x.SupportedModes, x.BindingKind, x.MaterialCode, x.ConsumableId)).ToList();
    }

    public async Task<ServiceResult> BindSlotAsync(long id, string slotCode, long memberId, DeviceSlotBindingDto input)
    {
        var device = await devices.FetchAsync(x => x.Id == id);
        if (device is null || device.MemberId != memberId)
        {
            return new ProblemDetails(HttpStatusCode.NotFound, "设备不存在");
        }

        var code = slotCode?.Trim() ?? string.Empty;
        var slot = await slots.FetchAsync(x => x.DeviceId == id && x.SlotCode == code, noTracking: false);
        if (slot is null)
        {
            return new ProblemDetails(HttpStatusCode.NotFound, "仓位不存在");
        }

        if (input.ConsumableId is > 0)
        {
            if (!await inventory.ConsumableBelongsToMemberAsync(input.ConsumableId.Value, memberId))
            {
                return new ProblemDetails(HttpStatusCode.BadRequest, "耗材不属于该会员");
            }

            slot.BindingKind = SlotBindingKind.Consumable;
            slot.ConsumableId = input.ConsumableId;
            slot.MaterialCode = string.IsNullOrWhiteSpace(input.MaterialCode) ? slot.MaterialCode : input.MaterialCode.Trim();
        }
        else if (!string.IsNullOrWhiteSpace(input.MaterialCode))
        {
            slot.BindingKind = SlotBindingKind.Material;
            slot.MaterialCode = input.MaterialCode.Trim();
            slot.ConsumableId = null;
        }
        else
        {
            slot.BindingKind = SlotBindingKind.None;
            slot.MaterialCode = null;
            slot.ConsumableId = null;
        }

        await slots.UpdateAsync(slot);
        return new ServiceResult();
    }

    public async Task<List<DeviceDto>> GetConsoleListAsync()
    {
        var list = await devices.GetAll().OrderByDescending(x => x.Id).Take(500).ToListAsync();
        return list.Select(Map).ToList();
    }

    public async Task<ServiceResult<DeviceTokenDto>> ActivateAsync(DeviceActivateDto input)
    {
        var sn = input.Sn?.Trim() ?? string.Empty;
        var device = await devices.FetchAsync(x => x.DeviceSn == sn, noTracking: false);
        if (device is null)
        {
            return new ProblemDetails(HttpStatusCode.NotFound, "设备未绑定，不能激活");
        }

        var token = IssueToken(device);
        device.ActivatedAt = DateTime.Now;
        await devices.UpdateAsync(device);
        return new DeviceTokenDto(token, device.DeviceSn);
    }

    public async Task<ServiceResult<DeviceTokenDto>> RefreshTokenAsync(string currentToken)
    {
        var hash = HashToken(currentToken);
        var device = await devices.FetchAsync(x => x.DeviceTokenHash == hash, noTracking: false);
        if (device is null)
        {
            return new ProblemDetails(HttpStatusCode.Unauthorized, "设备令牌无效");
        }

        var token = IssueToken(device);
        await devices.UpdateAsync(device);
        return new DeviceTokenDto(token, device.DeviceSn);
    }

    public async Task<bool> MqttAuthAsync(DeviceMqttAuthDto input)
    {
        var sn = input.Username?.Trim() ?? string.Empty;
        if (sn.Length == 0 || !string.Equals(input.ClientId?.Trim(), $"pot-{sn}", StringComparison.Ordinal))
        {
            return false;
        }

        var device = await devices.FetchAsync(x => x.DeviceSn == sn);
        if (device is null || string.IsNullOrWhiteSpace(device.DeviceTokenHash))
        {
            return false;
        }

        return string.Equals(device.DeviceTokenHash, HashToken(input.Password), StringComparison.Ordinal);
    }

    public async Task<ServiceResult> IssueCommandAsync(long deviceId, DeviceCommandIssueDto input)
    {
        if (!await devices.AnyAsync(x => x.Id == deviceId))
        {
            return new ProblemDetails(HttpStatusCode.NotFound, "设备不存在");
        }

        var existing = await outbox.FetchAsync(x => x.CommandId == input.CommandId, noTracking: false);
        if (existing is not null)
        {
            // 已 RECEIVED：禁止当新投放重发；未收到则可重试同一 commandId
            if (existing.Status == MqttOutboxStatus.Received)
            {
                return new ProblemDetails(HttpStatusCode.Conflict, "指令已收到，不能换新投放");
            }

            existing.RetryCount += 1;
            existing.LastAttemptAt = DateTime.Now;
            await outbox.UpdateAsync(existing);
            return new ServiceResult();
        }

        await outbox.InsertAsync(new DeviceMqttOutboxEntity
        {
            Id = IdGenerater.GetNextId(),
            DeviceId = deviceId,
            CommandId = input.CommandId,
            PayloadJson = string.IsNullOrWhiteSpace(input.PayloadJson) ? "{}" : input.PayloadJson,
            Status = MqttOutboxStatus.Pending,
            RetryCount = 0
        });
        return new ServiceResult();
    }

    public async Task<DeviceDto?> GetBySnAsync(string sn)
    {
        var value = sn?.Trim() ?? string.Empty;
        if (value.Length == 0)
        {
            return null;
        }

        var entity = await devices.FetchAsync(x => x.DeviceSn == value);
        return entity is null ? null : Map(entity);
    }

    public async Task<ServiceResult> UnbindBySnAsync(string sn)
    {
        var value = sn?.Trim() ?? string.Empty;
        var device = await devices.FetchAsync(x => x.DeviceSn == value, noTracking: false);
        if (device is null)
        {
            return new ServiceResult();
        }

        var deviceId = device.Id;
        await slots.ExecuteDeleteAsync(x => x.DeviceId == deviceId);
        await shadows.ExecuteDeleteAsync(x => x.DeviceId == deviceId);
        await capabilities.ExecuteDeleteAsync(x => x.DeviceId == deviceId);
        await outbox.ExecuteDeleteAsync(x => x.DeviceId == deviceId);
        await devices.DeleteAsync(deviceId);
        return new ServiceResult();
    }

    public async Task<ServiceResult> TryLockTaskAsync(long deviceId, long taskId, int epoch)
    {
        var device = await devices.FetchAsync(x => x.Id == deviceId, noTracking: false);
        if (device is null)
        {
            return new ProblemDetails(HttpStatusCode.NotFound, "设备不存在");
        }

        // 一壶同时一个 ActiveTask
        if (device.ActiveTaskId is > 0 && device.ActiveTaskId != taskId)
        {
            return new ProblemDetails(HttpStatusCode.Conflict, "设备已有未结束任务");
        }

        device.ActiveTaskId = taskId;
        device.ActiveEpoch = epoch;
        await devices.UpdateAsync(device);
        return new ServiceResult();
    }

    public async Task ReleaseTaskAsync(long deviceId, long taskId)
    {
        var device = await devices.FetchAsync(x => x.Id == deviceId, noTracking: false);
        if (device is null || device.ActiveTaskId != taskId)
        {
            return;
        }

        device.ActiveTaskId = null;
        await devices.UpdateAsync(device);
    }

    public async Task<int> BumpEpochAsync(long deviceId)
    {
        var device = await devices.FetchAsync(x => x.Id == deviceId, noTracking: false);
        if (device is null)
        {
            return 0;
        }

        device.ActiveEpoch += 1;
        await devices.UpdateAsync(device);
        return device.ActiveEpoch;
    }

    public async Task<ServiceResult> SetOnlineBySnAsync(string sn, bool online)
    {
        var device = await devices.FetchAsync(x => x.DeviceSn == sn.Trim());
        if (device is null)
        {
            return new ProblemDetails(HttpStatusCode.NotFound, "设备不存在");
        }

        var shadow = await shadows.FetchAsync(x => x.DeviceId == device.Id, noTracking: false);
        if (shadow is null)
        {
            return new ProblemDetails(HttpStatusCode.NotFound, "影子不存在");
        }

        shadow.Online = online;
        await shadows.UpdateAsync(shadow);
        return new ServiceResult();
    }

    public async Task ApplyCapabilityAsync(string sn, string payloadJson)
    {
        var device = await devices.FetchAsync(x => x.DeviceSn == sn.Trim());
        if (device is null)
        {
            return;
        }

        var cap = await capabilities.FetchAsync(x => x.DeviceId == device.Id, noTracking: false);
        if (cap is null)
        {
            return;
        }

        cap.PayloadJson = string.IsNullOrWhiteSpace(payloadJson) ? "{}" : payloadJson;
        cap.ReportedAt = DateTime.Now;
        await capabilities.UpdateAsync(cap);
    }

    public async Task ApplyShadowAsync(string sn, DeviceShadowReportDto report)
    {
        var device = await devices.FetchAsync(x => x.DeviceSn == sn.Trim());
        if (device is null)
        {
            return;
        }

        var shadow = await shadows.FetchAsync(x => x.DeviceId == device.Id, noTracking: false);
        if (shadow is null)
        {
            return;
        }

        if (report.Version is > 0)
        {
            shadow.Version = report.Version.Value;
        }

        if (report.Online is not null)
        {
            shadow.Online = report.Online.Value;
        }

        if (report.WaterTemp is not null)
        {
            shadow.WaterTemp = report.WaterTemp;
        }

        if (!string.IsNullOrWhiteSpace(report.WorkState))
        {
            shadow.WorkState = report.WorkState.Trim();
        }

        if (report.CurrentTaskId is not null)
        {
            shadow.CurrentTaskId = report.CurrentTaskId;
        }

        if (report.CurrentEpoch is not null)
        {
            shadow.CurrentEpoch = report.CurrentEpoch;
        }

        if (report.CurrentStepNo is not null)
        {
            shadow.CurrentStepNo = report.CurrentStepNo;
        }

        await shadows.UpdateAsync(shadow);
    }

    public async Task ApplyAckAsync(string sn, DeviceAckDto ack)
    {
        var device = await devices.FetchAsync(x => x.DeviceSn == sn.Trim());
        if (device is null)
        {
            return;
        }

        var row = await outbox.FetchAsync(x => x.CommandId == ack.CommandId, noTracking: false);
        if (row is not null && string.Equals(ack.Result, "RECEIVED", StringComparison.OrdinalIgnoreCase))
        {
            row.Status = MqttOutboxStatus.Received;
            await outbox.UpdateAsync(row);
        }

        await events.PublishAsync(DeviceCommandAckedEvent.Topic, new DeviceCommandAckedEvent
        {
            CommandId = ack.CommandId,
            DeviceId = device.Id,
            TaskId = ack.TaskId,
            Epoch = ack.Epoch,
            StepNo = ack.StepNo,
            Result = ack.Result,
            WaterTemp = ack.WaterTemp
        });
    }

    public async Task<List<DeviceMqttPendingDto>> TakePendingOutboxAsync(int take)
    {
        var rows = await outbox.GetAll()
            .Where(x => x.Status == MqttOutboxStatus.Pending)
            .OrderBy(x => x.Id)
            .Take(take)
            .ToListAsync();
        if (rows.Count == 0)
        {
            return [];
        }

        var deviceIds = rows.Select(x => x.DeviceId).Distinct().ToList();
        var snMap = await devices.GetAll().Where(x => deviceIds.Contains(x.Id)).ToDictionaryAsync(x => x.Id, x => x.DeviceSn);
        return rows
            .Where(x => snMap.ContainsKey(x.DeviceId))
            .Select(x => new DeviceMqttPendingDto(x.Id, x.DeviceId, snMap[x.DeviceId], x.CommandId, x.PayloadJson))
            .ToList();
    }

    public async Task MarkOutboxAttemptAsync(long outboxId)
    {
        var row = await outbox.FetchAsync(x => x.Id == outboxId, noTracking: false);
        if (row is null)
        {
            return;
        }

        row.RetryCount += 1;
        row.LastAttemptAt = DateTime.Now;
        await outbox.UpdateAsync(row);
    }

    private static DeviceDto Map(DeviceEntity x) =>
        new(x.Id, x.DeviceSn, x.ModelCode, x.MemberId, x.ActiveTaskId, x.ActiveEpoch);

    private static string IssueToken(DeviceEntity device)
    {
        var token = Guid.NewGuid().ToString("N");
        device.DeviceTokenHash = HashToken(token);
        return token;
    }

    private static string HashToken(string token)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(token ?? string.Empty));
        return Convert.ToHexString(bytes);
    }

    /// <summary>型号 SlotProfileJson；空配置时默认 S1/S2/S3 整包仓，配方「投 1 包」常用 S3。</summary>
    private static List<(string SlotCode, string SlotType, string SupportedModes)> ParseSlotProfile(string? json)
    {
        var defaults = new List<(string, string, string)>
        {
            ("S1", "PACKAGE", "PACKAGE"),
            ("S2", "PACKAGE", "PACKAGE"),
            ("S3", "PACKAGE", "PACKAGE")
        };

        if (string.IsNullOrWhiteSpace(json) || json.Trim() == "[]")
        {
            return defaults;
        }

        try
        {
            using var doc = JsonDocument.Parse(json);
            if (doc.RootElement.ValueKind != JsonValueKind.Array || doc.RootElement.GetArrayLength() == 0)
            {
                return defaults;
            }

            var result = new List<(string, string, string)>();
            foreach (var item in doc.RootElement.EnumerateArray())
            {
                var code = item.TryGetProperty("slotCode", out var c) ? c.GetString()?.Trim() : null;
                if (string.IsNullOrWhiteSpace(code))
                {
                    continue;
                }

                var type = item.TryGetProperty("slotType", out var t) ? t.GetString()?.Trim() : "PACKAGE";
                var modes = "PACKAGE";
                if (item.TryGetProperty("supportedModes", out var m))
                {
                    modes = m.ValueKind == JsonValueKind.Array
                        ? string.Join(',', m.EnumerateArray().Select(x => x.GetString()).Where(x => !string.IsNullOrWhiteSpace(x)))
                        : m.GetString() ?? "PACKAGE";
                }

                result.Add((code, string.IsNullOrWhiteSpace(type) ? "PACKAGE" : type!, string.IsNullOrWhiteSpace(modes) ? "PACKAGE" : modes));
            }

            return result.Count == 0 ? defaults : result;
        }
        catch (JsonException)
        {
            return defaults;
        }
    }
}
