using NXAI.Shared.Application.Contracts.Dtos;

namespace NXAI.Device.Application.Contracts.Dtos;

/// <summary>Cooking 下发指令入参。本模块只写 Outbox，不直连 MQTT。</summary>
public class DeviceCommandIssueDto : InputDto
{
    /// <summary>指令 Id。见过的只重试同一条。</summary>
    public long CommandId { get; set; }

    /// <summary>COMMAND 信封 JSON。</summary>
    public string PayloadJson { get; set; } = "{}";
}
