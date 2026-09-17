using NXAI.Shared.Application.Contracts.Dtos;

namespace NXAI.Recipe.Application.Contracts.Dtos;

/// <summary>物料。</summary>
public class MaterialDto : OutputDto
{
    /// <summary>物料编码。</summary>
    public string Code { get; set; } = string.Empty;

    /// <summary>名称。</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>默认投料模式。</summary>
    public string DefaultMode { get; set; } = "PACKAGE";
}
