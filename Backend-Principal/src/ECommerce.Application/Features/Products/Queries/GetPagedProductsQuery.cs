using ECommerce.Domain.Common;
using MediatR;

namespace ECommerce.Application.Features.Products.Queries;

public record GetPagedProductsQuery(int Page, int PageSize) : IRequest<PagedData<ProductDto>>;
