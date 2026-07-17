namespace ECommerce.Api.DTOs;

public record ProductResponse(
    Guid Id,
    string Name,
    string Description,
    decimal Price,
    int Stock,
    Guid CategoryId,
    string CategoryName,
    bool IsActive);

public record NewProductRequest(
    string Name,
    string Description,
    decimal Price,
    int Stock,
    Guid CategoryId);

public record EditProductRequest(
    string Name,
    string Description,
    decimal Price,
    int Stock,
    Guid CategoryId);
