using NXAI.Shared;
using NXAI.Shared.WebApi;

var builder = WebApplication.CreateBuilder(args);
var serviceInfo = ServiceInfo.CreateInstance(typeof(Program).Assembly);
builder.AddConfiguration(serviceInfo);
builder.Services.AddAdnc(serviceInfo, builder.Configuration);

var app = builder.Build();
app.UseNXAI();
await app.RunAsync();
