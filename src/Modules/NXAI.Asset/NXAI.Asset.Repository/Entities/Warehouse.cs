using NXAI.Infra.Repository;

namespace NXAI.Asset.Repository.Entities;

/// <summary>仓库，表 ast_warehouse。</summary>
public class Warehouse : EfEntity
{
    /// <summary>仓库编码最大长度。</summary>
    public const int Code_MaxLength = 32;

    /// <summary>仓库名称最大长度。</summary>
    public const int Name_MaxLength = 64;

    /// <summary>仓库编码，唯一。</summary>
    public string Code { get; set; } = string.Empty;

    /// <summary>仓库名称。</summary>
    public string Name { get; set; } = string.Empty;
}
