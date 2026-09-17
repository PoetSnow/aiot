using NXAI.Asset.Application.Contracts.Dtos;
using NXAI.Shared.Application.Contracts.Dtos;
using NXAI.Shared.Application.Contracts.ResultModels;

namespace NXAI.Asset.Application.Contracts.Interfaces;

/// <summary>入出库单据。确认后才改 SN 台账。</summary>
public interface IStockService
{
    /// <summary>创建入库草稿。</summary>
    Task<ServiceResult<IdDto>> CreateStockInAsync(StockInCreationDto input);

    /// <summary>确认入库：SN 未占用 → InStock。</summary>
    Task<ServiceResult> ConfirmStockInAsync(long id);

    /// <summary>创建出库草稿。</summary>
    Task<ServiceResult<IdDto>> CreateStockOutAsync(StockOutCreationDto input);

    /// <summary>确认出库：必须 InStock → Outbound。</summary>
    Task<ServiceResult> ConfirmStockOutAsync(long id);

    /// <summary>按仓+型号汇总在库 SN 数。</summary>
    Task<List<StockSummaryDto>> GetStockSummariesAsync();
}
