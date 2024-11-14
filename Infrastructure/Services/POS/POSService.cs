using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Application.Common;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Repositories;
using Newtonsoft.Json;

namespace Infrastructure.Services.POS
{
  public class POSService : IPOSService
  {
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IProductRepository _productRepository;

    public POSService(IHttpClientFactory httpClientFactory, IProductRepository productRepository)
    {
      _httpClientFactory = httpClientFactory;
      _productRepository = productRepository;
    }

    public async Task<IReadOnlyList<KoronaProductDto>> FetchProductsFromPOSAsync()
    {
      var client = _httpClientFactory.CreateClient();

      var request = new HttpRequestMessage(HttpMethod.Get, "https://435.koronacloud.com/web/api/v3/accounts/afa3c84a-6728-4acb-aa60-4a40ce87956c/products");
      var token = AuthHelper.GetBasicAuthToken("test", "test123");
      request.Headers.Authorization = new AuthenticationHeaderValue(token);

      var response = await client.SendAsync(request);

      response.EnsureSuccessStatusCode();

      var content = await response.Content.ReadAsStringAsync();
      var products = JsonConvert.DeserializeObject<IReadOnlyList<KoronaProductDto>>(content);

      return products;
    }

    public async Task UpsertProductsAsync(IReadOnlyList<KoronaProductDto> products)
    {
      await _productRepository.UpsertAsync(products);
    }
  }

}
