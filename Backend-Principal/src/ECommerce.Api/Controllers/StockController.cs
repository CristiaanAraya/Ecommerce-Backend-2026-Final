using ECommerce.Application.Contracts.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StockController(IStockServiceClient stockService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<StockItemDto>>> ListarTodos(CancellationToken ct)
        => Ok(await stockService.GetAllStockAsync(ct));

    [HttpGet("{productId:guid}")]
    public async Task<ActionResult<StockItemDto>> Consultar(Guid productId, CancellationToken ct)
    {
        var stock = await stockService.GetStockAsync(productId, ct);
        if (stock is null)
            return NotFound(new { Title = $"Producto con id '{productId}' no tiene stock registrado." });

        return Ok(stock);
    }
}
