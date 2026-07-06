// src/OrderService/Infrastructure/ProductServiceClient.cs
namespace OrderService.Infrastructure;

public class ProductServiceClient
{
    private readonly HttpClient _httpClient;

    public ProductServiceClient(HttpClient httpClient) => _httpClient = httpClient;

    public async Task<bool> ProductExistsAsync(Guid productId, string bearerToken)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, $"/api/products/{productId}");
        request.Headers.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", bearerToken);

        var response = await _httpClient.SendAsync(request);
        return response.IsSuccessStatusCode;
    }
}