using NXAI.Shared.Application.Contracts.Dtos;

namespace NXAI.Device.Application.Contracts.Dtos;

/// <summary>待下发 MQTT 指令。</summary>
/// <param name="OutboxId">Outbox 主键。</param>
/// <param name="DeviceId">设备 Id。</param>
/// <param name="DeviceSn">SN，拼 topic。</param>
/// <param name="CommandId">指令 Id。</param>
/// <param name="PayloadJson">COMMAND 信封。</param>
public record DeviceMqttPendingDto(
    long OutboxId,
    long DeviceId,
    string DeviceSn,
    long CommandId,
    string PayloadJson) : IDto;
