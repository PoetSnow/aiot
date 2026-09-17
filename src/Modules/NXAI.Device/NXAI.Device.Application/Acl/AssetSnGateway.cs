using NXAI.Asset.Application.Contracts.Interfaces;

namespace NXAI.Device.Application.Acl;

/// <summary>转发 Asset 契约。对方 DTO 变化只改这里。</summary>
public sealed class AssetSnGateway(IAssetSnService sns, IDeviceModelService models) : IAssetSnGateway
{
    public Task<bool> CanMemberClaimAsync(string sn, long memberId) => sns.CanMemberClaimAsync(sn, memberId);

    public async Task<(string Sn, string ModelCode)?> GetAsync(string sn)
    {
        var dto = await sns.GetBySnAsync(sn);
        return dto is null ? null : (dto.Sn, dto.ModelCode);
    }

    public async Task<bool> MarkBoundAsync(string sn, long memberId)
    {
        var result = await sns.MarkBoundAsync(sn, memberId);
        return result.IsSuccess;
    }

    public async Task<string?> GetSlotProfileJsonAsync(string modelCode)
    {
        var model = await models.GetByCodeAsync(modelCode);
        return model?.SlotProfileJson;
    }
}
