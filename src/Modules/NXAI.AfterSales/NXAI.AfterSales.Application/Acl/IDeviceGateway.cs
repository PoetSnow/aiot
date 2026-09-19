namespace NXAI.AfterSales.Application.Acl;

/// <summary>设备防腐。开维修解绑走这里，不发 MQTT。</summary>
public interface IDeviceGateway
{
    /// <summary>按 SN 取已绑定设备 Id。未绑定则空。</summary>
    Task<long?> GetIdBySnAsync(string sn);

    /// <summary>售后解绑。删除在线会话，不改 ast_sn。</summary>
    Task UnbindBySnAsync(string sn);
}
