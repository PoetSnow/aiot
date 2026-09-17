using NXAI.Infra.Repository;

namespace NXAI.Device.Repository.Entities;

/// <summary>设备影子，表 dev_shadow。一行覆盖，只给界面和看门狗，不当投料扳机。</summary>
public class DeviceShadow : EfEntity
{
    /// <summary>工作状态最大长度。</summary>
    public const int WorkState_MaxLength = 16;

    /// <summary>硬件占用仓 JSON 最大长度。</summary>
    public const int SlotsOccupiedJson_MaxLength = 2000;

    /// <summary>所属设备，一对一。</summary>
    public long DeviceId { get; set; }

    /// <summary>影子版本，设备上报递增。</summary>
    public int Version { get; set; }

    /// <summary>是否在线。</summary>
    public bool Online { get; set; }

    /// <summary>当前水温。开火仍看硬件本地探头。</summary>
    public decimal? WaterTemp { get; set; }

    /// <summary>工作状态，默认 IDLE。</summary>
    public string WorkState { get; set; } = "IDLE";

    /// <summary>设备当前任务。</summary>
    public long? CurrentTaskId { get; set; }

    /// <summary>设备当前世代。</summary>
    public int? CurrentEpoch { get; set; }

    /// <summary>设备当前步骤号。</summary>
    public int? CurrentStepNo { get; set; }

    /// <summary>硬件感知的仓位占用 JSON。</summary>
    public string SlotsOccupiedJson { get; set; } = "[]";
}
