using System.Net;
using Microsoft.EntityFrameworkCore;
using NXAI.Asset.Application.Contracts.Dtos;
using NXAI.Asset.Application.Contracts.Interfaces;
using NXAI.Infra.Repository;
using NXAI.Shared.Application.Contracts.ResultModels;
using AssetSnEntity = NXAI.Asset.Repository.Entities.AssetSn;
using AssetSnStatus = NXAI.Asset.Repository.Entities.AssetSnStatus;

namespace NXAI.Asset.Application.Services;

/// <summary>SN 台账。实现见 <see cref="IAssetSnService"/>。</summary>
public sealed class AssetSnService(IEfRepository<AssetSnEntity> sns) : IAssetSnService
{
    public async Task<List<AssetSnDto>> GetListAsync(long? warehouseId, string? modelCode, int? status)
    {
        var query = sns.GetAll().AsQueryable();
        if (warehouseId is > 0)
        {
            query = query.Where(x => x.WarehouseId == warehouseId.Value);
        }

        if (!string.IsNullOrWhiteSpace(modelCode))
        {
            var code = modelCode.Trim();
            query = query.Where(x => x.ModelCode == code);
        }

        if (status is not null)
        {
            query = query.Where(x => x.Status == status.Value);
        }

        var list = await query.OrderByDescending(x => x.Id).Take(500).ToListAsync();
        return list.Select(Map).ToList();
    }

    public async Task<AssetSnDto?> GetBySnAsync(string sn)
    {
        var value = sn?.Trim() ?? string.Empty;
        if (value.Length == 0)
        {
            return null;
        }

        var entity = await sns.FetchAsync(x => x.Sn == value);
        return entity is null ? null : Map(entity);
    }

    public async Task<bool> CanMemberClaimAsync(string sn, long memberId)
    {
        var entity = await sns.FetchAsync(x => x.Sn == sn);
        if (entity is null || entity.Status != AssetSnStatus.Outbound)
        {
            return false;
        }

        // 出库时未挂会员，或挂的正是该会员，才可认领
        return entity.MemberId is null || entity.MemberId == memberId;
    }

    public async Task<ServiceResult> MarkBoundAsync(string sn, long memberId)
    {
        var entity = await sns.FetchAsync(x => x.Sn == sn, noTracking: false);
        if (entity is null || entity.Status != AssetSnStatus.Outbound)
        {
            return new ProblemDetails(HttpStatusCode.Conflict, "SN 未出库，不能绑定");
        }

        if (entity.MemberId is not null && entity.MemberId != memberId)
        {
            return new ProblemDetails(HttpStatusCode.Forbidden, "SN 不属于该会员");
        }

        entity.Status = AssetSnStatus.Bound;
        entity.MemberId = memberId;
        await sns.UpdateAsync(entity);
        return new ServiceResult();
    }

    public async Task<ServiceResult> MarkRepairingAsync(string sn)
    {
        var entity = await sns.FetchAsync(x => x.Sn == sn.Trim(), noTracking: false);
        if (entity is null)
        {
            return new ProblemDetails(HttpStatusCode.NotFound, "SN 不存在");
        }

        entity.Status = AssetSnStatus.Repairing;
        await sns.UpdateAsync(entity);
        return new ServiceResult();
    }

    public async Task<ServiceResult> MarkReturnedToStockAsync(string sn)
    {
        var entity = await sns.FetchAsync(x => x.Sn == sn.Trim(), noTracking: false);
        if (entity is null)
        {
            return new ProblemDetails(HttpStatusCode.NotFound, "SN 不存在");
        }

        entity.Status = AssetSnStatus.InStock;
        entity.MemberId = null;
        await sns.UpdateAsync(entity);
        return new ServiceResult();
    }

    private static AssetSnDto Map(AssetSnEntity x) =>
        new(x.Id, x.Sn, x.ModelCode, x.WarehouseId, x.Status, x.MemberId, x.InboundAt, x.OutboundAt);
}
