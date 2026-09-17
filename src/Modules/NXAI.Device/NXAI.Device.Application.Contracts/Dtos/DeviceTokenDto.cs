using NXAI.Shared.Application.Contracts.Dtos;

namespace NXAI.Device.Application.Contracts.Dtos;

/// <summary>设备访问令牌。作 MQTT password 与 X-Device-Token。</summary>
/// <param name="Token">明文令牌，只在激活/刷新时返回。</param>
/// <param name="Sn">设备 SN。</param>
public record DeviceTokenDto(string Token, string Sn) : IDto;
