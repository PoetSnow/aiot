using NXAI.Shared.Application.Contracts.Dtos;

namespace NXAI.Device.Application.Contracts.Dtos;

/// <summary>设备激活。SN 须已绑定。</summary>
public class DeviceActivateDto : InputDto
{
    /// <summary>设备 SN。</summary>
    public string Sn { get; set; } = string.Empty;
}
