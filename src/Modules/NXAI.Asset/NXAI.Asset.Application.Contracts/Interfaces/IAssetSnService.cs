using NXAI.Asset.Application.Contracts.Dtos;
using NXAI.Shared.Application.Contracts.ResultModels;

namespace NXAI.Asset.Application.Contracts.Interfaces;

/// <summary>SN 台账查询。供后台与 Device ACL。</summary>
public interface IAssetSnService
{
    /// <summary>按条件查 SN。</summary>
    /// <param name="warehouseId">仓库，可空。</param>
    /// <param name="modelCode">型号，可空。</param>
    /// <param name="status">状态，可空。</param>
    Task<List<AssetSnDto>> GetListAsync(long? warehouseId, string? modelCode, int? status);

    /// <summary>按 SN 查详情。</summary>
    Task<AssetSnDto?> GetBySnAsync(string sn);

    /// <summary>是否已出库且可被该会员认领。Device 绑定前调用。未出库必须失败。</summary>
    Task<bool> CanMemberClaimAsync(string sn, long memberId);

    /// <summary>绑定成功后 SN → Bound。Device 经 ACL 调用，禁止其他模块直接改表。</summary>
    Task<ServiceResult> MarkBoundAsync(string sn, long memberId);

    /// <summary>售后开维修：SN → Repairing。此后不能再被会员绑定。</summary>
    Task<ServiceResult> MarkRepairingAsync(string sn);

    /// <summary>维修结案回库：SN → InStock，清除会员。</summary>
    Task<ServiceResult> MarkReturnedToStockAsync(string sn);
}
