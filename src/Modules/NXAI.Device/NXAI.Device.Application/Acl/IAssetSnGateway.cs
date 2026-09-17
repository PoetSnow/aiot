namespace NXAI.Device.Application.Acl;

/// <summary>Asset SN 防腐。绑定只问「已出库且可认领」，不直接碰 ast_sn。</summary>
public interface IAssetSnGateway
{
    /// <summary>未出库、维修中、非该会员均失败。</summary>
    Task<bool> CanMemberClaimAsync(string sn, long memberId);

    /// <summary>读取 SN 与型号，供建仓。</summary>
    Task<(string Sn, string ModelCode)?> GetAsync(string sn);

    /// <summary>绑定成功后标记 Bound。</summary>
    Task<bool> MarkBoundAsync(string sn, long memberId);

    /// <summary>型号仓位配置 JSON。</summary>
    Task<string?> GetSlotProfileJsonAsync(string modelCode);
}
