using System.Net;
using Microsoft.EntityFrameworkCore;
using NXAI.Asset.Application.Contracts.Dtos;
using NXAI.Asset.Application.Contracts.Interfaces;
using NXAI.Infra.IdGenerater.Yitter;
using NXAI.Infra.Repository;
using NXAI.Shared.Application.Contracts.Dtos;
using NXAI.Shared.Application.Contracts.ResultModels;
using DeviceModelEntity = NXAI.Asset.Repository.Entities.DeviceModel;

namespace NXAI.Asset.Application.Services;

/// <summary>设备型号。实现见 <see cref="IDeviceModelService"/>。</summary>
public sealed class DeviceModelService(IEfRepository<DeviceModelEntity> models) : IDeviceModelService
{
    public async Task<ServiceResult<IdDto>> CreateAsync(DeviceModelCreationDto input)
    {
        var code = input.ModelCode?.Trim() ?? string.Empty;
        if (string.IsNullOrWhiteSpace(code))
        {
            return new ProblemDetails(HttpStatusCode.BadRequest, "型号编码不能为空");
        }

        if (await models.AnyAsync(x => x.ModelCode == code))
        {
            return new ProblemDetails(HttpStatusCode.Conflict, "型号编码已存在");
        }

        var entity = new DeviceModelEntity
        {
            Id = IdGenerater.GetNextId(),
            ModelCode = code,
            Name = string.IsNullOrWhiteSpace(input.Name) ? code : input.Name.Trim(),
            WarrantyMonths = input.WarrantyMonths < 0 ? 0 : input.WarrantyMonths,
            SlotProfileJson = string.IsNullOrWhiteSpace(input.SlotProfileJson) ? "[]" : input.SlotProfileJson.Trim()
        };
        await models.InsertAsync(entity);
        return new IdDto(entity.Id);
    }

    public async Task<List<DeviceModelDto>> GetListAsync()
    {
        var list = await models.GetAll().OrderBy(x => x.ModelCode).ToListAsync();
        return list.Select(x => new DeviceModelDto(x.Id, x.ModelCode, x.Name, x.WarrantyMonths, x.SlotProfileJson)).ToList();
    }

    public async Task<DeviceModelDto?> GetByCodeAsync(string modelCode)
    {
        var code = modelCode?.Trim() ?? string.Empty;
        if (code.Length == 0)
        {
            return null;
        }

        var entity = await models.FetchAsync(x => x.ModelCode == code);
        return entity is null
            ? null
            : new DeviceModelDto(entity.Id, entity.ModelCode, entity.Name, entity.WarrantyMonths, entity.SlotProfileJson);
    }
}
