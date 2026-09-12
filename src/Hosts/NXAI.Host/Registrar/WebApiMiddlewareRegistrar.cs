using NXAI.Shared.WebApi.Registrar;

namespace NXAI.Host.Registrar;

public sealed class WebApiMiddlewareRegistrar(WebApplication app) : AbstractWebApiMiddlewareRegistrar(app)
{
    public override void UseNXAI() => UseWebApiDefault();
}
