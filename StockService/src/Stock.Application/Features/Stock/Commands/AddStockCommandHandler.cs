using Stock.Application.Contracts.Infrastructure;
using Stock.Domain.Contracts.Persistence;
using Stock.Domain.Entities;
using Stock.Domain.Exceptions;
using MediatR;

namespace Stock.Application.Features.Stock.Commands;

public class AddStockCommandHandler(
    IProductStockRepository productStockRepository,
    IProductoServiceClient productoServicesClient,
    IUnitOfWork unitOfWork)
    : IRequestHandler<AddStockCommand, StockDto>
{
    public async Task<StockDto> Handle(AddStockCommand request, CancellationToken ct)
    {
        var existing = await productStockRepository.GetByProductIdAsync(request.ProductId, ct);

        if (existing is not null)
        {
            existing.AddStock(request.Quantity);
            await productStockRepository.UpdateAsync(existing, ct);
            await unitOfWork.SaveChangesAsync(ct);

            return MapToDto(existing);
        }

        var productInfo = await productoServicesClient.GetProductByIdAsync(request.ProductId, ct);
        if (productInfo is null)
            throw new NotFoundException("Producto", request.ProductId);

        var stock = new ProductStock(request.ProductId, productInfo.Name, request.Quantity);
        await productStockRepository.AddAsync(stock, ct);
        await unitOfWork.SaveChangesAsync(ct);

        return MapToDto(stock);
    }

    private static StockDto MapToDto(ProductStock s) =>
        new(s.ProductId, s.ProductName, s.TotalQuantity, s.ReservedQuantity, s.AvailableQuantity);
}
