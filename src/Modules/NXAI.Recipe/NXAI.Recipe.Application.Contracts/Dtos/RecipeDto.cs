using NXAI.Shared.Application.Contracts.Dtos;

namespace NXAI.Recipe.Application.Contracts.Dtos;

/// <summary>配方（含步骤）。</summary>
public class RecipeDto : OutputDto
{
    /// <summary>配方编码。</summary>
    public string Code { get; set; } = string.Empty;

    /// <summary>版本号。</summary>
    public int Version { get; set; }

    /// <summary>名称。</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>状态：0 草稿 / 1 已发布 / 2 已取代。</summary>
    public int Status { get; set; }

    /// <summary>场景标签。</summary>
    public string SceneTags { get; set; } = string.Empty;

    /// <summary>适配型号。</summary>
    public string CompatibleModels { get; set; } = string.Empty;

    /// <summary>步骤。</summary>
    public List<RecipeStepDto> Steps { get; set; } = [];
}
