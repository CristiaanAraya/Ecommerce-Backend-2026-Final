using Stock.Api.DTOs;
using Stock.Application.Features.Stock;
using Stock.Application.Features.Stock.Commands;
using Stock.Application.Features.Stock.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Stock.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StockController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<StockDto>>> ObtenerTodos(CancellationToken ct)
        => Ok(await mediator.Send(new GetAllStockQuery(), ct));

    [HttpGet("{productId:guid}")]
    public async Task<ActionResult<StockDto>> ObtenerPorProducto(Guid productId, CancellationToken ct)
        => Ok(await mediator.Send(new GetStockByProductIdQuery(productId), ct));

    [HttpPost]
    public async Task<ActionResult<StockDto>> AgregarStock([FromBody] AddStockCommand command, CancellationToken ct)
    {
        var result = await mediator.Send(command, ct);
        return CreatedAtAction(nameof(ObtenerPorProducto), new { productId = result.ProductId }, result);
    }

    [HttpPost("reserve")]
    public async Task<ActionResult<ReserveStockResponse>> Reservar([FromBody] ReserveStockRequest request, CancellationToken ct)
    {
        var command = new ReserveStockCommand
        {
            Items = request.Items.Select(i => new ReserveItem
            {
                ProductId = i.ProductId,
                Quantity = i.Quantity
            }).ToList()
        };

        var result = await mediator.Send(command, ct);
        return Ok(result);
    }

    [HttpPost("release")]
    public async Task<ActionResult<ReleaseStockResponse>> Liberar([FromBody] ReleaseStockRequest request, CancellationToken ct)
    {
        var command = new ReleaseStockCommand
        {
            Items = request.Items.Select(i => new ReleaseItem
            {
                ProductId = i.ProductId,
                Quantity = i.Quantity
            }).ToList()
        };

        var result = await mediator.Send(command, ct);
        return Ok(result);
    }
}
