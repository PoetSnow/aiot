using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace NXAI.Member.API.Controllers;

/// <summary>会员模块探活，证明小程序文档里有接口。</summary>
public sealed class MemberPingController : PortalApiController
{
    /// <summary>探活。</summary>
    [AllowAnonymous]
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult Get() => Ok(new { module = "Member", surface = "portal" });
}
