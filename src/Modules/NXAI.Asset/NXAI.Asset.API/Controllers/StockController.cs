using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NXAI.Asset.Application.Contracts.Dtos;
using NXAI.Asset.Application.Contracts.Interfaces;
using NXAI.Shared.Application.Contracts.Dtos;
using NXAI.Shared.WebApi.Routing;

namespace NXAI.Asset.API.Controllers;

/// <summary>入出库与库存汇总（后台）。</summary>
[Authorize]
[Route($"{ApiSurfaces.ConsoleRoutePrefix}")]
public sealed class StockController(IStockService stocks) : ConsoleApiController
{
    /// <summary>创建入库草稿。</summary>
    [HttpPost("stock-ins")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult<IdDto>> CreateStockInAsync([FromBody] StockInCreationDto input)
        => CreatedResult(await stocks.CreateStockInAsync(input));

    /// <summary>确认入库。SN 未占用 → InStock。</summary>
    [HttpPost("stock-ins/{id:long}/confirm")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> ConfirmStockInAsync([FromRoute] long id)
        => Result(await stocks.ConfirmStockInAsync(id));

    /// <summary>创建出库草稿。</summary>
    [HttpPost("stock-outs")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult<IdDto>> CreateStockOutAsync([FromBody] StockOutCreationDto input)
        => CreatedResult(await stocks.CreateStockOutAsync(input));

    /// <summary>确认出库。必须 InStock → Outbound。</summary>
    [HttpPost("stock-outs/{id:long}/confirm")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> ConfirmStockOutAsync([FromRoute] long id)
        => Result(await stocks.ConfirmStockOutAsync(id));

    /// <summary>在库库存按仓+型号汇总（数 SN）。</summary>
    [HttpGet("stock-summaries")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<StockSummaryDto>>> GetStockSummariesAsync()
        => await stocks.GetStockSummariesAsync();
}
