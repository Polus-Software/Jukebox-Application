using Application.Common.JukeBox;
using Domain.Interfaces.JukeBox;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
namespace Infrastructure.Services.JukeBox
{
    public class HttpClientService : IHttpClientService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly AppSettings _appSettings;

        public HttpClientService(IHttpClientFactory httpClientFactory, IOptions<AppSettings> appSettings)
        {
            _httpClientFactory = httpClientFactory;
            _appSettings = appSettings.Value;
        }

        public async Task<string> GetData(StringContent content)
        {
            var httpClient = _httpClientFactory.CreateClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", _appSettings.APIKey);
            var response = await httpClient.PostAsync(_appSettings.SoundHornEndpoint, content);
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();
            return responseBody;
        }
    }
}
