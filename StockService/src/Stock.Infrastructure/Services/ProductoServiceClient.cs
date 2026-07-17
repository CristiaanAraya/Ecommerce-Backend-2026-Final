using System.Net.Http.Json;
using Stock.Application.Contracts.Infrastructure;

namespace Stock.Infrastructure.Services;

public class ProductoServiceClient(HttpClient httpClient) : IProductoServiceClient
{
    public async Task<ProductInfo?> GetProductByIdAsync(Guid productId, CancellationToken ct = default)
    {
        try
        {
            var response = await httpClient.GetAsync($"/api/products/{productId}", ct);
            if (!response.IsSuccessStatusCode)
                return null;

            return await response.Content.ReadFromJsonAsync<ProductInfo>(cancellationToken: ct);
        }
        catch
        {
            return null;
        }
    }
}
