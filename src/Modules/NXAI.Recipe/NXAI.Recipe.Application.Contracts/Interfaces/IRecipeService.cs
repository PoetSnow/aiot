using NXAI.Recipe.Application.Contracts.Dtos;
using NXAI.Shared.Application.Contracts.Dtos;
using NXAI.Shared.Application.Contracts.ResultModels;

namespace NXAI.Recipe.Application.Contracts.Interfaces;

/// <summary>物料与配方。方案 A：Code+Version，发布后旧版只读。</summary>
public interface IRecipeService
{
    /// <summary>创建物料。</summary>
    Task<ServiceResult<IdDto>> CreateMaterialAsync(MaterialCreationDto input);

    /// <summary>更新物料。</summary>
    Task<ServiceResult> UpdateMaterialAsync(long id, MaterialCreationDto input);

    /// <summary>物料列表。</summary>
    Task<List<MaterialDto>> GetMaterialsAsync();

    /// <summary>创建草稿配方。</summary>
    Task<ServiceResult<IdDto>> CreateRecipeAsync(RecipeCreationDto input);

    /// <summary>只改 Draft。已发布行禁止改。</summary>
    Task<ServiceResult> UpdateRecipeAsync(long id, RecipeCreationDto input);

    /// <summary>后台配方列表。</summary>
    Task<List<RecipeDto>> GetConsoleListAsync();

    /// <summary>后台按 Id 取一版（含步骤）。</summary>
    Task<RecipeDto?> GetByIdAsync(long id);

    /// <summary>发布：旧 Published 变 Superseded，本行变 Published。</summary>
    Task<ServiceResult> PublishAsync(long id);

    /// <summary>小程序：当前已发布列表。</summary>
    Task<List<RecipeDto>> GetPublishedListAsync();

    /// <summary>按 Code 取当前 Published 快照。Cooking 创建任务时调用。</summary>
    Task<RecipeDto?> GetPublishedByCodeAsync(string code);
}
