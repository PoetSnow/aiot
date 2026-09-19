namespace NXAI.Cooking.Application.Acl;

/// <summary>配方防腐。只取已发布快照，不碰 rcp_ 表。</summary>
public interface IRecipeGateway
{
    /// <summary>按编码取当前已发布版。没有则空。</summary>
    Task<CookingRecipeSnapshot?> GetPublishedByCodeAsync(string code);
}

/// <summary>Cooking 自己的配方快照。对方 DTO 变化只改 Gateway。</summary>
public sealed class CookingRecipeSnapshot
{
    /// <summary>配方行 Id。</summary>
    public long Id { get; set; }

    /// <summary>配方编码。</summary>
    public string Code { get; set; } = string.Empty;

    /// <summary>版本号。</summary>
    public int Version { get; set; }

    /// <summary>适配型号，逗号分隔。</summary>
    public string CompatibleModels { get; set; } = string.Empty;

    /// <summary>步骤。目标只写 TargetCode，不写仓位号。</summary>
    public List<CookingRecipeStepSnapshot> Steps { get; set; } = [];
}

/// <summary>配方步骤快照。</summary>
public sealed class CookingRecipeStepSnapshot
{
    /// <summary>步骤号。</summary>
    public int StepNo { get; set; }

    /// <summary>动作，如 HEAT / DISPENSE。</summary>
    public string Action { get; set; } = string.Empty;

    /// <summary>触发类型。</summary>
    public string TriggerType { get; set; } = string.Empty;

    /// <summary>触发温度。</summary>
    public decimal? TempCelsius { get; set; }

    /// <summary>目标物料编码，不是仓位号。</summary>
    public string? TargetCode { get; set; }

    /// <summary>投料模式。</summary>
    public string? Mode { get; set; }

    /// <summary>数量。</summary>
    public decimal? AmountValue { get; set; }

    /// <summary>单位。</summary>
    public string? AmountUnit { get; set; }
}
