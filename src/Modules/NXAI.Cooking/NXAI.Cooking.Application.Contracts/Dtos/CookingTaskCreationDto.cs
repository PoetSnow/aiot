using NXAI.Shared.Application.Contracts.Dtos;

namespace NXAI.Cooking.Application.Contracts.Dtos;

/// <summary>手选配方启动制作。</summary>
public class CookingTaskCreationDto : InputDto
{
    /// <summary>设备 Id。</summary>
    public long DeviceId { get; set; }

    /// <summary>已发布配方编码。</summary>
    public string RecipeCode { get; set; } = string.Empty;
}
