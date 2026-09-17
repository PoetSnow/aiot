using NXAI.Shared.Application.Contracts.Dtos;

namespace NXAI.Recipe.Application.Contracts.Dtos;

/// <summary>创建物料。</summary>
public class MaterialCreationDto : InputDto
{
    /// <summary>物料编码，与配方 TargetCode 对齐。</summary>
    public string Code { get; set; } = string.Empty;

    /// <summary>名称。</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>默认投料模式，如 PACKAGE。</summary>
    public string DefaultMode { get; set; } = "PACKAGE";
}
