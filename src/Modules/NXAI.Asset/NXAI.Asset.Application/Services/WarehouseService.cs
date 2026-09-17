using System.Net;
using Microsoft.EntityFrameworkCore;
using NXAI.Asset.Application.Contracts.Dtos;
using NXAI.Asset.Application.Contracts.Interfaces;
using NXAI.Infra.IdGenerater.Yitter;
using NXAI.Infra.Repository;
using NXAI.Shared.Application.Contracts.Dtos;
using NXAI.Shared.Application.Contracts.ResultModels;
using WarehouseEntity = NXAI.Asset.Repository.Entities.Warehouse;

namespace NXAI.Asset.Application.Services;

/// <summary>仓库。实现见 <see cref="IWarehouseService"/>。</summary>
public sealed class WarehouseService(IEfRepository<WarehouseEntity> warehouses) : IWarehouseService
{
    public async Task<ServiceResult<IdDto>> CreateAsync(WarehouseCreationDto input)
    {
        var code = input.Code?.Trim() ?? string.Empty;
        if (string.IsNullOrWhiteSpace(code))
        {
            return new ProblemDetails(HttpStatusCode.BadRequest, "仓库编码不能为空");
        }

        if (await warehouses.AnyAsync(x => x.Code == code))
        {
            return new ProblemDetails(HttpStatusCode.Conflict, "仓库编码已存在");
        }

        var entity = new WarehouseEntity
        {
            Id = IdGenerater.GetNextId(),
            Code = code,
            Name = string.IsNullOrWhiteSpace(input.Name) ? code : input.Name.Trim()
        };
        await warehouses.InsertAsync(entity);
        return new IdDto(entity.Id);
    }

    public async Task<List<WarehouseDto>> GetListAsync()
    {
        var list = await warehouses.GetAll().OrderBy(x => x.Code).ToListAsync();
        return list.Select(x => new WarehouseDto(x.Id, x.Code, x.Name)).ToList();
    }
}
