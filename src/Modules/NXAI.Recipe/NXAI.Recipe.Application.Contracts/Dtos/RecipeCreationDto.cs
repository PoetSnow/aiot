using NXAI.Shared.Application.Contracts.Dtos;

namespace NXAI.Recipe.Application.Contracts.Dtos;

/// <summary>创建配方草稿。步骤禁止写仓位号。</summary>
public class RecipeCreationDto : InputDto
{
    /// <summary>配方编码。</summary>
    public string Code { get; set; } = string.Empty;

    /// <summary>名称。</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>场景标签，逗号分隔。</summary>
    public string SceneTags { get; set; } = string.Empty;

    /// <summary>适配型号，逗号分隔。空表示不限。</summary>
    public string CompatibleModels { get; set; } = string.Empty;

    /// <summary>步骤。投料只写 TargetCode，不写 slotCode。</summary>
    public List<RecipeStepInputDto> Steps { get; set; } = [];
}
