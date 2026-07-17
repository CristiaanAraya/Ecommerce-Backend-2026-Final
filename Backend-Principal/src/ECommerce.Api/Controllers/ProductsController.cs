using ECommerce.Api.DTOs;
using ECommerce.Application.Features.Products.Commands;
using ECommerce.Application.Features.Products.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductDto>>> GetAll(CancellationToken ct)
        => Ok(await mediator.Send(new GetAllProductsQuery(), ct));

    [HttpGet("paged")]
    public async Task<ActionResult> GetPaged(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        CancellationToken ct = default)
        => Ok(await mediator.Send(new GetPagedProductsQuery(page, pageSize), ct));

    [HttpGet("search")]
    public async Task<ActionResult<IEnumerable<ProductDto>>> Search([FromQuery] string term, CancellationToken ct)
        => Ok(await mediator.Send(new SearchProductsQuery(term), ct));

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ProductDto>> GetById(Guid id, CancellationToken ct)
        => Ok(await mediator.Send(new GetProductByIdQuery(id), ct));

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<ActionResult<ProductDto>> Create([FromBody] NewProductRequest request, CancellationToken ct)
    {
        var product = await mediator.Send(
            new CreateProductCommand(request.Name, request.Description, request.Price, request.Stock, request.CategoryId), ct);
        return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] EditProductRequest request, CancellationToken ct)
    {
        await mediator.Send(
            new UpdateProductCommand(id, request.Name, request.Description, request.Price, request.Stock, request.CategoryId), ct);
        return NoContent();
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        await mediator.Send(new DeleteProductCommand(id), ct);
        return NoContent();
    }
}
