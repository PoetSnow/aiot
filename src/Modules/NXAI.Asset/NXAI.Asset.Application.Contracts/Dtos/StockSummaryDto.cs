using NXAI.Shared.Application.Contracts.Dtos;

namespace NXAI.Asset.Application.Contracts.Dtos;

/// <summary>在库库存汇总。Qty = 该仓该型号 Status=InStock 的 SN 数。</summary>
/// <param name="WarehouseId">仓库 Id。</param>
/// <param name="WarehouseCode">仓库编码。</param>
/// <param name="ModelCode">型号编码。</param>
/// <param name="Qty">在库数量。</param>
public record StockSummaryDto(long WarehouseId, string WarehouseCode, string ModelCode, int Qty) : IDto;
