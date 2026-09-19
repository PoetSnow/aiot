using NXAI.Device.Application.Contracts.Interfaces;

namespace NXAI.AfterSales.Application.Acl;

/// <summary>转发 Device 契约。</summary>
public sealed class DeviceGateway(IDeviceService devices) : IDeviceGateway
{
    public async Task<long?> GetIdBySnAsync(string sn)
    {
        var dto = await devices.GetBySnAsync(sn);
        return dto?.Id;
    }

    public Task UnbindBySnAsync(string sn) => devices.UnbindBySnAsync(sn);
}
