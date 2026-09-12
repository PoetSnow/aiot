using Microsoft.Extensions.Options;

namespace NXAI.Shared.WebApi.Authentication.Basic;

public class BasicPostConfigureOptions : IPostConfigureOptions<BasicSchemeOptions>
{
    public void PostConfigure(string? name, BasicSchemeOptions options)
    {
        // Method intentionally left empty.
    }
}
