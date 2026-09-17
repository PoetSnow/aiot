using NXAI.Shared.Application.Contracts.Dtos;

namespace NXAI.Asset.Application.Contracts.Dtos;

/// <summary>SN 台账。</summary>
/// <param name="Id">主键。</param>
/// <param name="Sn">设备 SN。</param>
/// <param name="ModelCode">型号。</param>
/// <param name="WarehouseId">仓库。</param>
/// <param name="Status">见资产 SN 状态常量。</param>
/// <param name="MemberId">挂接会员。</param>
/// <param name="InboundAt">入库时间。</param>
/// <param name="OutboundAt">出库时间。</param>
public record AssetSnDto(
    long Id,
    string Sn,
    string ModelCode,
    long WarehouseId,
    int Status,
    long? MemberId,
    DateTime? InboundAt,
    DateTime? OutboundAt) : IDto;
