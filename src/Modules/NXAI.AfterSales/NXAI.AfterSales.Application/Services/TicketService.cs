using System.Net;
using Microsoft.EntityFrameworkCore;
using NXAI.AfterSales.Application.Contracts.Dtos;
using NXAI.AfterSales.Application.Contracts.Interfaces;
using NXAI.Asset.Application.Contracts.Interfaces;
using NXAI.Device.Application.Contracts.Interfaces;
using NXAI.Infra.IdGenerater.Yitter;
using NXAI.Infra.Repository;
using NXAI.Shared.Application.Contracts.Dtos;
using NXAI.Shared.Application.Contracts.ResultModels;
using TicketEntity = NXAI.AfterSales.Repository.Entities.Ticket;
using TicketLogEntity = NXAI.AfterSales.Repository.Entities.TicketLog;
using TicketStatus = NXAI.AfterSales.Repository.Entities.TicketStatus;
using TicketType = NXAI.AfterSales.Repository.Entities.TicketType;

namespace NXAI.AfterSales.Application.Services;

/// <summary>售后工单。实现见 <see cref="ITicketService"/>。</summary>
public sealed class TicketService(
    IEfRepository<TicketEntity> tickets,
    IEfRepository<TicketLogEntity> logs,
    IAssetSnService assetSns,
    IDeviceModelService models,
    IDeviceService devices) : ITicketService
{
    public Task<ServiceResult<IdDto>> CreateByStaffAsync(long staffId, TicketCreationDto input)
        => CreateAsync(staffId, memberId: null, input);

    public Task<ServiceResult<IdDto>> CreateByMemberAsync(long memberId, TicketCreationDto input)
        => CreateAsync(operatorStaffId: 0, memberId, input);

    public async Task<List<TicketDto>> GetConsoleListAsync()
    {
        var list = await tickets.GetAll().OrderByDescending(x => x.Id).Take(500).ToListAsync();
        return list.Select(Map).ToList();
    }

    public async Task<List<TicketDto>> GetByMemberAsync(long memberId)
    {
        var list = await tickets.GetAll().Where(x => x.MemberId == memberId).OrderByDescending(x => x.Id).ToListAsync();
        return list.Select(Map).ToList();
    }

    public async Task<ServiceResult> AcceptAsync(long staffId, long id)
    {
        var entity = await tickets.FetchAsync(x => x.Id == id, noTracking: false);
        if (entity is null)
        {
            return new ProblemDetails(HttpStatusCode.NotFound, "工单不存在");
        }

        entity.Status = TicketStatus.Accepted;
        await tickets.UpdateAsync(entity);
        await AddLogAsync(id, "accept", string.Empty, staffId);
        return new ServiceResult();
    }

    public async Task<ServiceResult> CloseAsync(long staffId, long id, TicketCloseDto input)
    {
        var entity = await tickets.FetchAsync(x => x.Id == id, noTracking: false);
        if (entity is null)
        {
            return new ProblemDetails(HttpStatusCode.NotFound, "工单不存在");
        }

        var result = string.IsNullOrWhiteSpace(input.Result) ? "return" : input.Result.Trim();
        entity.Status = TicketStatus.Closed;
        entity.CloseResult = result;
        await tickets.UpdateAsync(entity);

        if (string.Equals(result, "return", StringComparison.OrdinalIgnoreCase)
            || string.Equals(result, "scrap", StringComparison.OrdinalIgnoreCase))
        {
            await assetSns.MarkReturnedToStockAsync(entity.Sn);
        }

        await AddLogAsync(id, "close", result, staffId);
        return new ServiceResult();
    }

    private async Task<ServiceResult<IdDto>> CreateAsync(long operatorStaffId, long? memberId, TicketCreationDto input)
    {
        var sn = input.Sn?.Trim() ?? string.Empty;
        if (sn.Length == 0)
        {
            return new ProblemDetails(HttpStatusCode.BadRequest, "SN 不能为空");
        }

        var asset = await assetSns.GetBySnAsync(sn);
        if (asset is null)
        {
            return new ProblemDetails(HttpStatusCode.NotFound, "SN 不存在");
        }

        var bound = await devices.GetBySnAsync(sn);
        var warranty = await IsWarrantyValidAsync(asset.ModelCode, asset.OutboundAt);
        var id = IdGenerater.GetNextId();
        var entity = new TicketEntity
        {
            Id = id,
            TicketNo = $"AFS{id}",
            Sn = sn,
            MemberId = memberId ?? asset.MemberId,
            DeviceId = bound?.Id,
            Type = input.Type,
            Status = TicketStatus.Opened,
            WarrantyValid = warranty,
            Symptom = input.Symptom?.Trim() ?? string.Empty
        };
        await tickets.InsertAsync(entity);
        await AddLogAsync(id, "open", entity.Symptom, operatorStaffId);

        if (input.Type == TicketType.Repair)
        {
            // 经 Asset 契约改 SN 状态，禁止本模块 UPDATE ast_sn
            await assetSns.MarkRepairingAsync(sn);
            await devices.UnbindBySnAsync(sn);
        }

        return new IdDto(id);
    }

    private async Task<bool> IsWarrantyValidAsync(string modelCode, DateTime? outboundAt)
    {
        if (outboundAt is null)
        {
            return false;
        }

        var model = await models.GetByCodeAsync(modelCode);
        var months = model?.WarrantyMonths ?? 12;
        return outboundAt.Value.AddMonths(months) >= DateTime.Now;
    }

    private Task AddLogAsync(long ticketId, string action, string remark, long staffId)
        => logs.InsertAsync(new TicketLogEntity
        {
            Id = IdGenerater.GetNextId(),
            TicketId = ticketId,
            Action = action,
            Remark = remark ?? string.Empty,
            OperatorStaffId = staffId
        });

    private static TicketDto Map(TicketEntity x) => new()
    {
        Id = x.Id,
        TicketNo = x.TicketNo,
        Sn = x.Sn,
        MemberId = x.MemberId,
        Type = x.Type,
        Status = x.Status,
        WarrantyValid = x.WarrantyValid,
        Symptom = x.Symptom,
        CloseResult = x.CloseResult
    };
}
