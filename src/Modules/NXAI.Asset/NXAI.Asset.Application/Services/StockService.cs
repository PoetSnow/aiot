using System.Net;
using Microsoft.EntityFrameworkCore;
using NXAI.Asset.Application.Contracts.Dtos;
using NXAI.Asset.Application.Contracts.Interfaces;
using NXAI.Infra.IdGenerater.Yitter;
using NXAI.Infra.Repository;
using NXAI.Shared.Application.Contracts.Dtos;
using NXAI.Shared.Application.Contracts.ResultModels;
using AssetSnEntity = NXAI.Asset.Repository.Entities.AssetSn;
using AssetSnStatus = NXAI.Asset.Repository.Entities.AssetSnStatus;
using DeviceModelEntity = NXAI.Asset.Repository.Entities.DeviceModel;
using StockDocStatus = NXAI.Asset.Repository.Entities.StockDocStatus;
using StockInEntity = NXAI.Asset.Repository.Entities.StockIn;
using StockInLineEntity = NXAI.Asset.Repository.Entities.StockInLine;
using StockOutEntity = NXAI.Asset.Repository.Entities.StockOut;
using StockOutLineEntity = NXAI.Asset.Repository.Entities.StockOutLine;
using WarehouseEntity = NXAI.Asset.Repository.Entities.Warehouse;

namespace NXAI.Asset.Application.Services;

/// <summary>入出库。实现见 <see cref="IStockService"/>。</summary>
public sealed class StockService(
    IEfRepository<StockInEntity> stockIns,
    IEfRepository<StockInLineEntity> stockInLines,
    IEfRepository<StockOutEntity> stockOuts,
    IEfRepository<StockOutLineEntity> stockOutLines,
    IEfRepository<AssetSnEntity> sns,
    IEfRepository<DeviceModelEntity> models,
    IEfRepository<WarehouseEntity> warehouses,
    Acl.IMemberGateway members) : IStockService
{
    public async Task<ServiceResult<IdDto>> CreateStockInAsync(StockInCreationDto input)
    {
        if (!await warehouses.AnyAsync(x => x.Id == input.WarehouseId))
        {
            return new ProblemDetails(HttpStatusCode.BadRequest, "仓库不存在");
        }

        var lines = NormalizeInLines(input.Lines);
        if (lines.Count == 0)
        {
            return new ProblemDetails(HttpStatusCode.BadRequest, "入库明细不能为空");
        }

        var modelCodes = lines.Select(x => x.ModelCode).Distinct(StringComparer.Ordinal).ToList();
        var existingModels = await models.GetAll().Where(x => modelCodes.Contains(x.ModelCode)).Select(x => x.ModelCode).ToListAsync();
        var missingModel = modelCodes.FirstOrDefault(x => !existingModels.Contains(x, StringComparer.Ordinal));
        if (missingModel is not null)
        {
            return new ProblemDetails(HttpStatusCode.BadRequest, $"型号不存在：{missingModel}");
        }

        var billId = IdGenerater.GetNextId();
        await stockIns.InsertAsync(new StockInEntity
        {
            Id = billId,
            BillNo = $"SI{billId}",
            WarehouseId = input.WarehouseId,
            Status = StockDocStatus.Draft,
            Remark = input.Remark?.Trim() ?? string.Empty
        });

        foreach (var line in lines)
        {
            await stockInLines.InsertAsync(new StockInLineEntity
            {
                Id = IdGenerater.GetNextId(),
                StockInId = billId,
                Sn = line.Sn,
                ModelCode = line.ModelCode
            });
        }

        return new IdDto(billId);
    }

    public async Task<ServiceResult> ConfirmStockInAsync(long id)
    {
        var bill = await stockIns.FetchAsync(x => x.Id == id, noTracking: false);
        if (bill is null)
        {
            return new ProblemDetails(HttpStatusCode.NotFound, "入库单不存在");
        }

        if (bill.Status != StockDocStatus.Draft)
        {
            return new ProblemDetails(HttpStatusCode.Conflict, "入库单已确认");
        }

        var lines = await stockInLines.GetAll(writeDb: true, noTracking: true).Where(x => x.StockInId == id).ToListAsync();
        if (lines.Count == 0)
        {
            return new ProblemDetails(HttpStatusCode.BadRequest, "入库单无明细");
        }

        var snValues = lines.Select(x => x.Sn).ToList();
        var occupied = await sns.GetAll().Where(x => snValues.Contains(x.Sn)).Select(x => x.Sn).ToListAsync();
        if (occupied.Count > 0)
        {
            return new ProblemDetails(HttpStatusCode.Conflict, $"SN 已被占用：{string.Join(',', occupied)}");
        }

        var now = DateTime.Now;
        foreach (var line in lines)
        {
            await sns.InsertAsync(new AssetSnEntity
            {
                Id = IdGenerater.GetNextId(),
                Sn = line.Sn,
                ModelCode = line.ModelCode,
                WarehouseId = bill.WarehouseId,
                Status = AssetSnStatus.InStock,
                StockInId = bill.Id,
                InboundAt = now
            });
        }

        bill.Status = StockDocStatus.Confirmed;
        bill.ConfirmedAt = now;
        await stockIns.UpdateAsync(bill);
        return new ServiceResult();
    }

    public async Task<ServiceResult<IdDto>> CreateStockOutAsync(StockOutCreationDto input)
    {
        if (!await warehouses.AnyAsync(x => x.Id == input.WarehouseId))
        {
            return new ProblemDetails(HttpStatusCode.BadRequest, "仓库不存在");
        }

        if (input.MemberId is > 0 && !await members.ExistsActiveAsync(input.MemberId.Value))
        {
            return new ProblemDetails(HttpStatusCode.BadRequest, "会员不存在或已注销");
        }

        var snValues = (input.Sns ?? [])
            .Select(x => x?.Trim() ?? string.Empty)
            .Where(x => x.Length > 0)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();
        if (snValues.Count == 0)
        {
            return new ProblemDetails(HttpStatusCode.BadRequest, "出库 SN 不能为空");
        }

        var billId = IdGenerater.GetNextId();
        await stockOuts.InsertAsync(new StockOutEntity
        {
            Id = billId,
            BillNo = $"SO{billId}",
            WarehouseId = input.WarehouseId,
            MemberId = input.MemberId is > 0 ? input.MemberId : null,
            Status = StockDocStatus.Draft,
            Remark = input.Remark?.Trim() ?? string.Empty
        });

        foreach (var sn in snValues)
        {
            await stockOutLines.InsertAsync(new StockOutLineEntity
            {
                Id = IdGenerater.GetNextId(),
                StockOutId = billId,
                Sn = sn
            });
        }

        return new IdDto(billId);
    }

    public async Task<ServiceResult> ConfirmStockOutAsync(long id)
    {
        var bill = await stockOuts.FetchAsync(x => x.Id == id, noTracking: false);
        if (bill is null)
        {
            return new ProblemDetails(HttpStatusCode.NotFound, "出库单不存在");
        }

        if (bill.Status != StockDocStatus.Draft)
        {
            return new ProblemDetails(HttpStatusCode.Conflict, "出库单已确认");
        }

        var lines = await stockOutLines.GetAll().Where(x => x.StockOutId == id).ToListAsync();
        if (lines.Count == 0)
        {
            return new ProblemDetails(HttpStatusCode.BadRequest, "出库单无明细");
        }

        var now = DateTime.Now;
        foreach (var line in lines)
        {
            var sn = await sns.FetchAsync(x => x.Sn == line.Sn, noTracking: false);
            if (sn is null)
            {
                return new ProblemDetails(HttpStatusCode.BadRequest, $"SN 不存在：{line.Sn}");
            }

            if (sn.WarehouseId != bill.WarehouseId)
            {
                return new ProblemDetails(HttpStatusCode.BadRequest, $"SN 不在该仓库：{line.Sn}");
            }

            // 未出库（InStock）才能出库；未入库不能走到这里出成功
            if (sn.Status != AssetSnStatus.InStock)
            {
                return new ProblemDetails(HttpStatusCode.Conflict, $"SN 非在库状态，不能出库：{line.Sn}");
            }

            sn.Status = AssetSnStatus.Outbound;
            sn.MemberId = bill.MemberId;
            sn.StockOutId = bill.Id;
            sn.OutboundAt = now;
            await sns.UpdateAsync(sn);
        }

        bill.Status = StockDocStatus.Confirmed;
        bill.ConfirmedAt = now;
        await stockOuts.UpdateAsync(bill);
        return new ServiceResult();
    }

    public async Task<List<StockSummaryDto>> GetStockSummariesAsync()
    {
        var rows = await (
            from sn in sns.GetAll()
            where sn.Status == AssetSnStatus.InStock
            join wh in warehouses.GetAll() on sn.WarehouseId equals wh.Id
            group sn by new { sn.WarehouseId, wh.Code, sn.ModelCode } into g
            select new StockSummaryDto(g.Key.WarehouseId, g.Key.Code, g.Key.ModelCode, g.Count())
        ).ToListAsync();

        return rows.OrderBy(x => x.WarehouseCode).ThenBy(x => x.ModelCode).ToList();
    }

    private static List<(string Sn, string ModelCode)> NormalizeInLines(List<StockInLineDto>? lines)
    {
        var result = new List<(string Sn, string ModelCode)>();
        var seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        foreach (var line in lines ?? [])
        {
            var sn = line.Sn?.Trim() ?? string.Empty;
            var model = line.ModelCode?.Trim() ?? string.Empty;
            if (sn.Length == 0 || model.Length == 0)
            {
                continue;
            }

            if (!seen.Add(sn))
            {
                continue;
            }

            result.Add((sn, model));
        }

        return result;
    }
}
