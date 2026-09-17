using NXAI.Shared.Application.Contracts.Dtos;

namespace NXAI.Asset.Application.Contracts.Dtos;

/// <summary>出库单摘要。</summary>
/// <param name="Id">主键。</param>
/// <param name="BillNo">单号。</param>
/// <param name="WarehouseId">仓库。</param>
/// <param name="MemberId">挂接会员。</param>
/// <param name="Status">0 草稿 / 1 已确认。</param>
/// <param name="LineCount">行数。</param>
public record StockOutDto(long Id, string BillNo, long WarehouseId, long? MemberId, int Status, int LineCount) : IDto;
