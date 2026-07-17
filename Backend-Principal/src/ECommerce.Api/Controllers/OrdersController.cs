using System.Security.Claims;
using ECommerce.Api.DTOs;
using ECommerce.Application.Features.Orders.Commands;
using ECommerce.Application.Features.Orders.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class OrdersController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<PlaceOrderResponse>> Place([FromBody] PlaceOrderRequest request, CancellationToken ct)
    {
        var userId = GetCurrentUserId();
        var lines = request.Items
            .Select(i => new OrderLine(i.ProductId, i.Quantity))
            .ToList();

        var order = await mediator.Send(new PlaceOrderCommand(userId, lines), ct);
        return CreatedAtAction(nameof(GetById), new { id = order.Id }, order);
    }

    [HttpGet("mine")]
    public async Task<ActionResult<IEnumerable<OrderDto>>> GetMine(CancellationToken ct)
    {
        var userId = GetCurrentUserId();
        return Ok(await mediator.Send(new GetOrdersByUserQuery(userId), ct));
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<OrderDto>> GetById(Guid id, CancellationToken ct)
    {
        var userId = GetCurrentUserId();
        var isAdmin = User.IsInRole("Admin");
        return Ok(await mediator.Send(new GetOrderByIdQuery(id, userId, isAdmin), ct));
    }

    private Guid GetCurrentUserId()
    {
        var value = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(value, out var id))
            throw new UnauthorizedAccessException();
        return id;
    }
}
