using Microsoft.Extensions.Options;

namespace NXAI.Shared.WebApi.Authentication.Hybrid;

public class HybridPostConfigureOptions : IPostConfigureOptions<HybridSchemeOptions>
{
    public void PostConfigure(string? name, HybridSchemeOptions options)
    {
        // Method intentionally left empty.
    }
}
