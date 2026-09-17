using NXAI.Shared.Application.Contracts.Dtos;

namespace NXAI.Device.Application.Contracts.Dtos;

/// <summary>EMQX HTTP 认证回调。</summary>
public class DeviceMqttAuthDto : InputDto
{
    /// <summary>用户名，必须等于 SN。</summary>
    public string Username { get; set; } = string.Empty;

    /// <summary>密码，即设备 Token。</summary>
    public string Password { get; set; } = string.Empty;

    /// <summary>客户端 Id，必须是 pot-{sn}。</summary>
    public string ClientId { get; set; } = string.Empty;
}
