using NXAI.Shared.Application.Contracts.Dtos;

namespace NXAI.Device.Application.Contracts.Dtos;

/// <summary>小程序绑定设备。</summary>
public class DeviceBindDto : InputDto
{
    /// <summary>已出库 SN。未出库必须失败。</summary>
    public string Sn { get; set; } = string.Empty;
}
