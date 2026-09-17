using NXAI.Asset.Application.Contracts.Dtos;
using NXAI.Shared.Application.Contracts.Dtos;
using NXAI.Shared.Application.Contracts.ResultModels;

namespace NXAI.Asset.Application.Contracts.Interfaces;

/// <summary>设备型号维护。</summary>
public interface IDeviceModelService
{
    /// <summary>创建型号。</summary>
    Task<ServiceResult<IdDto>> CreateAsync(DeviceModelCreationDto input);

    /// <summary>型号列表。</summary>
    Task<List<DeviceModelDto>> GetListAsync();

    /// <summary>按型号编码读取。绑定设备时取仓位配置。</summary>
    Task<DeviceModelDto?> GetByCodeAsync(string modelCode);
}
