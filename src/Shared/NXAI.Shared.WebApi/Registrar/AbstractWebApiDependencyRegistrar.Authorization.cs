using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using NXAI.Shared.WebApi.Authorization;
using NXAI.Shared.WebApi.Authorization.Handlers;

namespace NXAI.Shared.WebApi.Registrar;

public abstract partial class AbstractWebApiDependencyRegistrar
{
    /// <summary>
    /// Registers authorization components.
    /// </summary>
    /// <typeparam name="TAuthorizationHandler"></typeparam>
    protected virtual void AddAuthorization<TAuthorizationHandler>() where TAuthorizationHandler : AbstractPermissionHandler
    {
        var policyName = AuthorizePolicy.Default;
        Services
            .AddScoped<IAuthorizationHandler, TAuthorizationHandler>()
            .AddAuthorization(options =>
            {
                options.AddPolicy(policyName, policy =>
                {
                    policy.Requirements.Add(new PermissionRequirement());
                });
            });
    }
}
