using NXAI.Recipe.Application.Contracts.Interfaces;

namespace NXAI.Cooking.Application.Acl;

/// <summary>转发 Recipe 契约。对方 DTO 变化只改这里。</summary>
public sealed class RecipeGateway(IRecipeService recipes) : IRecipeGateway
{
    public async Task<CookingRecipeSnapshot?> GetPublishedByCodeAsync(string code)
    {
        var dto = await recipes.GetPublishedByCodeAsync(code);
        if (dto is null)
        {
            return null;
        }

        return new CookingRecipeSnapshot
        {
            Id = dto.Id,
            Code = dto.Code,
            Version = dto.Version,
            CompatibleModels = dto.CompatibleModels,
            Steps = dto.Steps.Select(s => new CookingRecipeStepSnapshot
            {
                StepNo = s.StepNo,
                Action = s.Action,
                TriggerType = s.TriggerType,
                TempCelsius = s.TempCelsius,
                TargetCode = s.TargetCode,
                Mode = s.Mode,
                AmountValue = s.AmountValue,
                AmountUnit = s.AmountUnit
            }).ToList()
        };
    }
}
