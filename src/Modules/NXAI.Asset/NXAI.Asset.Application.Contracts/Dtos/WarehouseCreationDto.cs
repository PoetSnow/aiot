using NXAI.Shared.Application.Contracts.Dtos;

namespace NXAI.Asset.Application.Contracts.Dtos;

/// <summary>创建仓库。</summary>
public class WarehouseCreationDto : InputDto
{
    /// <summary>仓库编码，唯一。</summary>
    public string Code { get; set; } = string.Empty;

    /// <summary>仓库名称。</summary>
    public string Name { get; set; } = string.Empty;
}
