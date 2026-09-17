using System.Net;
using Microsoft.EntityFrameworkCore;
using NXAI.Infra.IdGenerater.Yitter;
using NXAI.Infra.Repository;
using NXAI.Recipe.Application.Contracts.Dtos;
using NXAI.Recipe.Application.Contracts.Interfaces;
using NXAI.Shared.Application.Contracts.Dtos;
using NXAI.Shared.Application.Contracts.ResultModels;
using MaterialEntity = NXAI.Recipe.Repository.Entities.Material;
using RecipeEntity = NXAI.Recipe.Repository.Entities.Recipe;
using RecipeStatus = NXAI.Recipe.Repository.Entities.RecipeStatus;
using RecipeStepEntity = NXAI.Recipe.Repository.Entities.RecipeStep;

namespace NXAI.Recipe.Application.Services;

/// <summary>物料与配方。实现见 <see cref="IRecipeService"/>。</summary>
public sealed class RecipeService(
    IEfRepository<MaterialEntity> materials,
    IEfRepository<RecipeEntity> recipes,
    IEfRepository<RecipeStepEntity> steps) : IRecipeService
{
    public async Task<ServiceResult<IdDto>> CreateMaterialAsync(MaterialCreationDto input)
    {
        var code = input.Code?.Trim() ?? string.Empty;
        if (code.Length == 0)
        {
            return new ProblemDetails(HttpStatusCode.BadRequest, "物料编码不能为空");
        }

        if (await materials.AnyAsync(x => x.Code == code))
        {
            return new ProblemDetails(HttpStatusCode.Conflict, "物料编码已存在");
        }

        var entity = new MaterialEntity
        {
            Id = IdGenerater.GetNextId(),
            Code = code,
            Name = string.IsNullOrWhiteSpace(input.Name) ? code : input.Name.Trim(),
            DefaultMode = string.IsNullOrWhiteSpace(input.DefaultMode) ? "PACKAGE" : input.DefaultMode.Trim()
        };
        await materials.InsertAsync(entity);
        return new IdDto(entity.Id);
    }

    public async Task<ServiceResult> UpdateMaterialAsync(long id, MaterialCreationDto input)
    {
        var entity = await materials.FetchAsync(x => x.Id == id, noTracking: false);
        if (entity is null)
        {
            return new ProblemDetails(HttpStatusCode.NotFound, "物料不存在");
        }

        entity.Name = string.IsNullOrWhiteSpace(input.Name) ? entity.Name : input.Name.Trim();
        entity.DefaultMode = string.IsNullOrWhiteSpace(input.DefaultMode) ? entity.DefaultMode : input.DefaultMode.Trim();
        await materials.UpdateAsync(entity);
        return new ServiceResult();
    }

    public async Task<List<MaterialDto>> GetMaterialsAsync()
    {
        var list = await materials.GetAll().OrderBy(x => x.Code).ToListAsync();
        return list.Select(x => new MaterialDto { Id = x.Id, Code = x.Code, Name = x.Name, DefaultMode = x.DefaultMode }).ToList();
    }

    public async Task<ServiceResult<IdDto>> CreateRecipeAsync(RecipeCreationDto input)
    {
        var error = ValidateSteps(input.Steps);
        if (error is not null)
        {
            return error;
        }

        var code = input.Code?.Trim() ?? string.Empty;
        if (code.Length == 0)
        {
            return new ProblemDetails(HttpStatusCode.BadRequest, "配方编码不能为空");
        }

        var maxVersion = await recipes.GetAll().Where(x => x.Code == code).Select(x => (int?)x.Version).MaxAsync() ?? 0;
        var entity = new RecipeEntity
        {
            Id = IdGenerater.GetNextId(),
            Code = code,
            Version = maxVersion + 1,
            Name = string.IsNullOrWhiteSpace(input.Name) ? code : input.Name.Trim(),
            Status = RecipeStatus.Draft,
            SceneTags = input.SceneTags?.Trim() ?? string.Empty,
            CompatibleModels = input.CompatibleModels?.Trim() ?? string.Empty
        };
        await recipes.InsertAsync(entity);
        await InsertStepsAsync(entity.Id, input.Steps);
        return new IdDto(entity.Id);
    }

    public async Task<ServiceResult> UpdateRecipeAsync(long id, RecipeCreationDto input)
    {
        var error = ValidateSteps(input.Steps);
        if (error is not null)
        {
            return error;
        }

        var entity = await recipes.FetchAsync(x => x.Id == id, noTracking: false);
        if (entity is null)
        {
            return new ProblemDetails(HttpStatusCode.NotFound, "配方不存在");
        }

        if (entity.Status != RecipeStatus.Draft)
        {
            return new ProblemDetails(HttpStatusCode.Conflict, "只能改草稿");
        }

        entity.Name = string.IsNullOrWhiteSpace(input.Name) ? entity.Name : input.Name.Trim();
        entity.SceneTags = input.SceneTags?.Trim() ?? entity.SceneTags;
        entity.CompatibleModels = input.CompatibleModels?.Trim() ?? entity.CompatibleModels;
        await recipes.UpdateAsync(entity);

        await steps.ExecuteDeleteAsync(x => x.RecipeId == id);
        await InsertStepsAsync(id, input.Steps);
        return new ServiceResult();
    }

    public async Task<List<RecipeDto>> GetConsoleListAsync()
    {
        var list = await recipes.GetAll().OrderBy(x => x.Code).ThenByDescending(x => x.Version).ToListAsync();
        return await MapManyAsync(list);
    }

    public async Task<RecipeDto?> GetByIdAsync(long id)
    {
        var entity = await recipes.FetchAsync(x => x.Id == id);
        return entity is null ? null : (await MapManyAsync([entity])).FirstOrDefault();
    }

    public async Task<ServiceResult> PublishAsync(long id)
    {
        var entity = await recipes.FetchAsync(x => x.Id == id, noTracking: false);
        if (entity is null)
        {
            return new ProblemDetails(HttpStatusCode.NotFound, "配方不存在");
        }

        if (entity.Status != RecipeStatus.Draft)
        {
            return new ProblemDetails(HttpStatusCode.Conflict, "只能发布草稿");
        }

        var published = await recipes.GetAll(noTracking: false)
            .Where(x => x.Code == entity.Code && x.Status == RecipeStatus.Published)
            .ToListAsync();
        foreach (var old in published)
        {
            old.Status = RecipeStatus.Superseded;
            await recipes.UpdateAsync(old);
        }

        entity.Status = RecipeStatus.Published;
        await recipes.UpdateAsync(entity);
        return new ServiceResult();
    }

    public async Task<List<RecipeDto>> GetPublishedListAsync()
    {
        var list = await recipes.GetAll().Where(x => x.Status == RecipeStatus.Published).OrderBy(x => x.Code).ToListAsync();
        return await MapManyAsync(list);
    }

    public async Task<RecipeDto?> GetPublishedByCodeAsync(string code)
    {
        var value = code?.Trim() ?? string.Empty;
        if (value.Length == 0)
        {
            return null;
        }

        var entity = await recipes.FetchAsync(x => x.Code == value && x.Status == RecipeStatus.Published);
        return entity is null ? null : (await MapManyAsync([entity])).FirstOrDefault();
    }

    private async Task InsertStepsAsync(long recipeId, List<RecipeStepInputDto> inputs)
    {
        foreach (var step in inputs.OrderBy(x => x.StepNo))
        {
            await steps.InsertAsync(new RecipeStepEntity
            {
                Id = IdGenerater.GetNextId(),
                RecipeId = recipeId,
                StepNo = step.StepNo,
                Action = step.Action.Trim(),
                TriggerType = string.IsNullOrWhiteSpace(step.TriggerType) ? "IMMEDIATE" : step.TriggerType.Trim(),
                TempCelsius = step.TempCelsius,
                DelaySeconds = step.DelaySeconds,
                TargetKind = string.IsNullOrWhiteSpace(step.TargetKind) ? null : step.TargetKind.Trim(),
                TargetCode = string.IsNullOrWhiteSpace(step.TargetCode) ? null : step.TargetCode.Trim(),
                Mode = string.IsNullOrWhiteSpace(step.Mode) ? null : step.Mode.Trim(),
                AmountValue = step.AmountValue,
                AmountUnit = string.IsNullOrWhiteSpace(step.AmountUnit) ? null : step.AmountUnit.Trim()
            });
        }
    }

    private async Task<List<RecipeDto>> MapManyAsync(List<RecipeEntity> list)
    {
        if (list.Count == 0)
        {
            return [];
        }

        var ids = list.Select(x => x.Id).ToList();
        var allSteps = await steps.GetAll().Where(x => ids.Contains(x.RecipeId)).OrderBy(x => x.StepNo).ToListAsync();
        return list.Select(recipe =>
        {
            var dto = new RecipeDto
            {
                Id = recipe.Id,
                Code = recipe.Code,
                Version = recipe.Version,
                Name = recipe.Name,
                Status = recipe.Status,
                SceneTags = recipe.SceneTags,
                CompatibleModels = recipe.CompatibleModels,
                Steps = allSteps.Where(s => s.RecipeId == recipe.Id).Select(s => new RecipeStepDto
                {
                    Id = s.Id,
                    StepNo = s.StepNo,
                    Action = s.Action,
                    TriggerType = s.TriggerType,
                    TempCelsius = s.TempCelsius,
                    DelaySeconds = s.DelaySeconds,
                    TargetKind = s.TargetKind,
                    TargetCode = s.TargetCode,
                    Mode = s.Mode,
                    AmountValue = s.AmountValue,
                    AmountUnit = s.AmountUnit
                }).ToList()
            };
            return dto;
        }).ToList();
    }

    private static ProblemDetails? ValidateSteps(List<RecipeStepInputDto>? inputs)
    {
        if (inputs is null || inputs.Count == 0)
        {
            return new ProblemDetails(HttpStatusCode.BadRequest, "至少一步");
        }

        foreach (var step in inputs)
        {
            var action = step.Action?.Trim() ?? string.Empty;
            if (action.Length == 0)
            {
                return new ProblemDetails(HttpStatusCode.BadRequest, "步骤动作不能为空");
            }

            // 模板禁止写死仓位，Cooking 才解析 TargetCode → slotCode
            if (!string.IsNullOrWhiteSpace(step.TargetKind) &&
                string.Equals(step.TargetKind, "SLOT", StringComparison.OrdinalIgnoreCase))
            {
                return new ProblemDetails(HttpStatusCode.BadRequest, "步骤禁止写仓位号");
            }
        }

        return null;
    }
}
