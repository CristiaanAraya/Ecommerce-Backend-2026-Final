using System.Net.Http.Json;
using ECommerce.Application.Contracts.Infrastructure;

namespace ECommerce.Infrastructure.Services;

public class StockServiceClient(HttpClient httpClient) : IStockServiceClient
{
    public async Task<List<StockItemDto>> GetAllStockAsync(CancellationToken ct = default)
    {
        try
        {
            var response = await httpClient.GetAsync("/api/stock", ct);
            if (!response.IsSuccessStatusCode)
                return [];

            return await response.Content.ReadFromJsonAsync<List<StockItemDto>>(cancellationToken: ct)
                   ?? [];
        }
        catch
        {
            return [];
        }
    }

    public async Task<StockItemDto?> GetStockAsync(Guid productId, CancellationToken ct = default)
    {
        try
        {
            var response = await httpClient.GetAsync($"/api/stock/{productId}", ct);
            if (!response.IsSuccessStatusCode)
                return null;

            return await response.Content.ReadFromJsonAsync<StockItemDto>(cancellationToken: ct);
        }
        catch
        {
            return null;
        }
    }

    public async Task<ReserveResponseDto> ReserveAsync(List<ReserveItemDto> items, CancellationToken ct = default)
    {
        try
        {
            var request = new { Items = items.Select(i => new { i.ProductId, i.Quantity }).ToList() };
            var response = await httpClient.PostAsJsonAsync("/api/stock/reserve", request, ct);

            if (!response.IsSuccessStatusCode)
                return new ReserveResponseDto { Reserved = false };

            return await response.Content.ReadFromJsonAsync<ReserveResponseDto>(cancellationToken: ct)
                   ?? new ReserveResponseDto { Reserved = false };
        }
        catch
        {
            return new ReserveResponseDto { Reserved = false };
        }
    }
}
