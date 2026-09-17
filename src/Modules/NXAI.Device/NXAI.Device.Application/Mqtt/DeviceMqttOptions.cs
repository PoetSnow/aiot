namespace NXAI.Device.Application.Mqtt;

/// <summary>Device 连接 EMQX 的配置。Broker 不可用时 Host 仍启动。</summary>
public sealed class DeviceMqttOptions
{
    /// <summary>配置节名。</summary>
    public const string SectionKey = "Mqtt";

    /// <summary>是否启用 MQTT 工作器。</summary>
    public bool Enabled { get; set; } = true;

    /// <summary>Broker 地址。</summary>
    public string Host { get; set; } = "127.0.0.1";

    /// <summary>MQTT 端口。</summary>
    public int Port { get; set; } = 1883;

    /// <summary>云端服务账号 clientId。</summary>
    public string ClientId { get; set; } = "nxai-device-svc";

    /// <summary>用户名。</summary>
    public string Username { get; set; } = "nxai-device-svc";

    /// <summary>密码，只放 Host 配置。</summary>
    public string Password { get; set; } = "nxai-device-svc";
}
