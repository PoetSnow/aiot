$ErrorActionPreference = 'Stop'
$utf8 = New-Object System.Text.UTF8Encoding $false
$root = Split-Path $PSScriptRoot -Parent
$modules = @(
    @{ Name = 'Member'; Prefix = 'mb'; Surface = 'Portal' }
    @{ Name = 'Asset'; Prefix = 'ast'; Surface = 'Console' }
    @{ Name = 'AfterSales'; Prefix = 'afs'; Surface = 'Console' }
    @{ Name = 'Device'; Prefix = 'dev'; Surface = 'Device' }
    @{ Name = 'Recipe'; Prefix = 'rcp'; Surface = 'Console' }
    @{ Name = 'Cooking'; Prefix = 'ckg'; Surface = 'Portal' }
    @{ Name = 'Inventory'; Prefix = 'inv'; Surface = 'Portal' }
    @{ Name = 'Catalog'; Prefix = 'cat'; Surface = 'Console' }
)

function Write-Utf8([string]$Path, [string]$Content) {
    $dir = Split-Path $Path -Parent
    if (-not (Test-Path $dir)) { New-Item -ItemType Directory -Path $dir | Out-Null }
    [System.IO.File]::WriteAllText($Path, $Content.TrimStart("`r", "`n") + "`n", $utf8)
}

foreach ($m in $modules) {
    $name = $m.Name
    $prefix = $m.Prefix
    $surface = $m.Surface
    $surfaceLower = $surface.ToLowerInvariant()
    $base = Join-Path $root "src\Modules\NXAI.$name"
    $ns = "NXAI.$name"

    Write-Utf8 (Join-Path $base "$ns.Application.Contracts\$ns.Application.Contracts.csproj") @"
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFramework>net8.0</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
  </PropertyGroup>
  <ItemGroup>
    <ProjectReference Include="..\..\..\Shared\NXAI.Shared.Application.Contracts\NXAI.Shared.Application.Contracts.csproj" />
  </ItemGroup>
</Project>
"@

    Write-Utf8 (Join-Path $base "$ns.Application.Contracts\AssemblyMarker.cs") @"
namespace $ns.Application.Contracts;

public static class AssemblyMarker
{
}
"@

    Write-Utf8 (Join-Path $base "$ns.Repository\$ns.Repository.csproj") @"
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFramework>net8.0</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
  </PropertyGroup>
  <ItemGroup>
    <ProjectReference Include="..\..\..\Infrastructures\NXAI.Infra.Repository.EfCore.MySql\NXAI.Infra.Repository.EfCore.MySql.csproj" />
    <ProjectReference Include="..\..\..\Infrastructures\NXAI.Infra.Repository.EfCore\NXAI.Infra.Repository.EfCore.csproj" />
    <ProjectReference Include="..\..\..\Infrastructures\NXAI.Infra.Repository\NXAI.Infra.Repository.csproj" />
    <ProjectReference Include="..\..\..\Shared\NXAI.Shared.Repository\NXAI.Shared.Repository.csproj" />
  </ItemGroup>
</Project>
"@

    Write-Utf8 (Join-Path $base "$ns.Repository\EntityInfo.cs") @"
using System.Reflection;
using NXAI.Infra.Repository.EfCore;

namespace $ns.Repository;

public class EntityInfo : AbstractEntityInfo
{
    protected override List<Assembly> GetEntityAssemblies() => [GetType().Assembly];

    protected override void SetTableName(Microsoft.EntityFrameworkCore.ModelBuilder modelBuilder)
    {
        // Table prefix ${prefix}_ . Entities are added in a later step.
    }
}
"@

    Write-Utf8 (Join-Path $base "$ns.Repository\${name}DbContext.cs") @"
using Microsoft.EntityFrameworkCore;
using NXAI.Infra.Repository;
using NXAI.Infra.Repository.EfCore.MySql;

namespace $ns.Repository;

public class ${name}DbContext(DbContextOptions<${name}DbContext> options, EntityInfo entityInfo, Operater operater)
    : MySqlDbContext(options, entityInfo, operater);
"@

    Write-Utf8 (Join-Path $base "$ns.Application\$ns.Application.csproj") @"
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFramework>net8.0</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
  </PropertyGroup>
  <ItemGroup>
    <ProjectReference Include="..\..\..\Shared\NXAI.Shared.Application\NXAI.Shared.Application.csproj" />
    <ProjectReference Include="..\$ns.Application.Contracts\$ns.Application.Contracts.csproj" />
    <ProjectReference Include="..\$ns.Repository\$ns.Repository.csproj" />
  </ItemGroup>
</Project>
"@

    Write-Utf8 (Join-Path $base "$ns.Application\DependencyRegistrar.cs") @"
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NXAI.Shared;
using NXAI.Shared.Application.Registrar;
using $ns.Repository;

namespace $ns.Application;

public sealed class DependencyRegistrar(IServiceCollection services, IServiceInfo serviceInfo, IConfiguration configuration, ServiceLifetime lifetime = ServiceLifetime.Scoped)
    : AbstractApplicationDependencyRegistrar(services, serviceInfo, configuration, lifetime)
{
    protected override Assembly ApplicationLayerAssembly => Assembly.GetExecutingAssembly();

    protected override Assembly RepositoryOrDomainLayerAssembly => typeof(EntityInfo).Assembly;

    public override void AddApplicationServices()
    {
        // Step 1 shell: no DbContext so Host can start without MySQL.
    }
}
"@

    $baseClass = switch ($surface) {
        'Console' { 'ConsoleApiController' }
        'Portal' { 'PortalApiController' }
        'Device' { 'DeviceApiController' }
        default { 'ConsoleApiController' }
    }

    $allowAnon = if ($surface -eq 'Device') { '' } else { "[AllowAnonymous]`r`n    " }

    Write-Utf8 (Join-Path $base "$ns.API\$ns.API.csproj") @"
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFramework>net8.0</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
  </PropertyGroup>
  <ItemGroup>
    <FrameworkReference Include="Microsoft.AspNetCore.App" />
    <ProjectReference Include="..\..\..\Shared\NXAI.Shared.WebApi\NXAI.Shared.WebApi.csproj" />
    <ProjectReference Include="..\$ns.Application.Contracts\$ns.Application.Contracts.csproj" />
  </ItemGroup>
</Project>
"@

    $ping = @"
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace $ns.API.Controllers;

public sealed class ${name}PingController : $baseClass
{
    $($allowAnon)[HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult Get() => Ok(new { module = "$name", surface = "$surfaceLower" });
}
"@
    Write-Utf8 (Join-Path $base "$ns.API\Controllers\${name}PingController.cs") $ping

    Write-Host "created $name"
}
