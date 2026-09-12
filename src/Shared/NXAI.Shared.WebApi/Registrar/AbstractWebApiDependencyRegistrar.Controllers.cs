using System.Reflection;
using System.Net;
using System.Text.Json;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;
using NXAI.Infra.Core.Json;
using NXAI.Shared.WebApi.Filters;
using ProblemDetails = Microsoft.AspNetCore.Mvc.ProblemDetails;

namespace NXAI.Shared.WebApi.Registrar;

public abstract partial class AbstractWebApiDependencyRegistrar
{
    /// <summary>
    /// Registers controllers
    /// Configures System.Text.Json
    /// Registers FluentValidation
    /// Configures ApiBehaviorOptions
    /// </summary>
    protected virtual void AddControllers()
    {
        var mvcBuilder =         Services
            .AddControllers(options => options.Filters.Add(typeof(CustomExceptionFilterAttribute)));

        Services.AddScoped<DeviceApiEnvelopeResultFilter>();

        foreach (var assembly in GetModuleApiAssemblies())
        {
            mvcBuilder.AddApplicationPart(assembly);
        }

        mvcBuilder.AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new Int64JsonConverter());
                options.JsonSerializerOptions.Converters.Add(new Int64NullableJsonConverter());
                options.JsonSerializerOptions.Converters.Add(new DateTimeConverter());
                options.JsonSerializerOptions.Converters.Add(new DateTimeNullableConverter());
                options.JsonSerializerOptions.Encoder = SystemTextJson.GetNXAIDefaultEncoder();
                // Indicates whether comments are allowed, disallowed, or skipped.
                options.JsonSerializerOptions.ReadCommentHandling = JsonCommentHandling.Skip;
                // Serialization settings for dynamic and anonymous types
                options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                //dynamic
                options.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
                // Anonymous types
                options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            });

        Services
            .AddFluentValidationAutoValidation(cfg =>
            {
                // Continue validating the remaining items after a validation failure
                ValidatorOptions.Global.DefaultClassLevelCascadeMode = CascadeMode.Continue;
                //cfg.ValidatorOptions.DefaultClassLevelCascadeMode = FluentValidation.CascadeMode.Continue;
                // Optionally set validator factory if you have problems with scope resolve inside validators.
                // cfg.ValidatorFactoryType = typeof(HttpContextServiceProviderValidatorFactory);
            });

        Services
            .Configure<ApiBehaviorOptions>(options =>
            {
                // Adjust the response format for parameter validation failures
                // Disable automatic validation
                //options.SuppressModelStateInvalidFilter = true;
                // Format validation messages
                options.InvalidModelStateResponseFactory = (context) =>
                {
                    var problemDetails = new ProblemDetails
                    {
                        Detail = context.ModelState.GetValidationSummary("<br>"),
                        Title = "Invalid parameters",
                        Status = (int)HttpStatusCode.BadRequest,
                        Type = "https://httpstatuses.com/400",
                        Instance = context.HttpContext.Request.Path
                    };

                    return new ObjectResult(problemDetails)
                    {
                        StatusCode = problemDetails.Status
                    };
                };
            });
    }
}
