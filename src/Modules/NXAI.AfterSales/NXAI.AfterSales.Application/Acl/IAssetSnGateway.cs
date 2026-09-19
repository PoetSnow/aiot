namespace NXAI.AfterSales.Application.Acl;

/// <summary>Asset SN 防腐。质保与改状态只走这里，禁止 UPDATE ast_sn。</summary>
public interface IAssetSnGateway
{
    /// <summary>按 SN 取台账。没有则空。</summary>
    Task<AfterSalesSnInfo?> GetAsync(string sn);

    /// <summary>出库时间加型号保修月数是否仍有效。</summary>
    Task<bool> IsWarrantyValidAsync(string modelCode, DateTime? outboundAt);

    /// <summary>开维修：SN → Repairing，此后不能再绑。</summary>
    Task MarkRepairingAsync(string sn);

    /// <summary>结案回库 / 报废：SN → InStock，清会员。</summary>
    Task MarkReturnedToStockAsync(string sn);
}

/// <summary>售后看到的 SN。</summary>
/// <param name="Sn">设备 SN。</param>
/// <param name="ModelCode">型号。</param>
/// <param name="MemberId">挂接会员。</param>
/// <param name="OutboundAt">出库时间，质保起算。</param>
public sealed record AfterSalesSnInfo(string Sn, string ModelCode, long? MemberId, DateTime? OutboundAt);
