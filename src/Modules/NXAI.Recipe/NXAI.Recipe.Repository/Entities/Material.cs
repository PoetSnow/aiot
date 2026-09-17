using NXAI.Infra.Repository;

namespace NXAI.Recipe.Repository.Entities;

/// <summary>物料主数据，表 rcp_material。</summary>
public class Material : EfEntity
{
    public const int Code_MaxLength = 32;
    public const int Name_MaxLength = 64;
    public const int DefaultMode_MaxLength = 16;

    /// <summary>物料编码，与配方 TargetCode、耗材类型对齐。</summary>
    public string Code { get; set; } = string.Empty;

    /// <summary>名称。</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>默认投料模式，如 PACKAGE。</summary>
    public string DefaultMode { get; set; } = "PACKAGE";
}
