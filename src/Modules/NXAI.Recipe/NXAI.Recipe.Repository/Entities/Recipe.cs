using NXAI.Infra.Repository;

namespace NXAI.Recipe.Repository.Entities;

/// <summary>配方模板，表 rcp_recipe。方案 A：Code+Version 多行。</summary>
public class Recipe : EfEntity
{
    public const int Code_MaxLength = 32;
    public const int Name_MaxLength = 64;
    public const int SceneTags_MaxLength = 128;
    public const int CompatibleModels_MaxLength = 256;

    /// <summary>配方编码。</summary>
    public string Code { get; set; } = string.Empty;

    /// <summary>版本号，从 1 起。</summary>
    public int Version { get; set; } = 1;

    /// <summary>名称。</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>状态，见 <see cref="RecipeStatus"/>。</summary>
    public int Status { get; set; } = RecipeStatus.Draft;

    /// <summary>场景标签，逗号分隔。</summary>
    public string SceneTags { get; set; } = string.Empty;

    /// <summary>适配型号，逗号分隔。空表示不限。</summary>
    public string CompatibleModels { get; set; } = string.Empty;
}

/// <summary>配方状态。</summary>
public static class RecipeStatus
{
    /// <summary>草稿，可改。</summary>
    public const int Draft = 0;

    /// <summary>已发布。同一 Code 最多一条。</summary>
    public const int Published = 1;

    /// <summary>被新版本取代，只读。</summary>
    public const int Superseded = 2;
}
