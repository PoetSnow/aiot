using NXAI.Asset.Application.Contracts.Interfaces;

namespace NXAI.AfterSales.Application.Acl;

/// <summary>转发 Asset 契约。对方 DTO 变化只改这里。</summary>
public sealed class AssetSnGateway(IAssetSnService sns, IDeviceModelService models) : IAssetSnGateway
{
    public async Task<AfterSalesSnInfo?> GetAsync(string sn)
    {
        var dto = await sns.GetBySnAsync(sn);
        return dto is null ? null : new AfterSalesSnInfo(dto.Sn, dto.ModelCode, dto.MemberId, dto.OutboundAt);
    }

    public async Task<bool> IsWarrantyValidAsync(string modelCode, DateTime? outboundAt)
    {
        if (outboundAt is null)
        {
            return false;
        }

        var model = await models.GetByCodeAsync(modelCode);
        var months = model?.WarrantyMonths ?? 12;
        return outboundAt.Value.AddMonths(months) >= DateTime.Now;
    }

    public Task MarkRepairingAsync(string sn) => sns.MarkRepairingAsync(sn);

    public Task MarkReturnedToStockAsync(string sn) => sns.MarkReturnedToStockAsync(sn);
}
