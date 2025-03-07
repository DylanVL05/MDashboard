using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;

namespace MDashboard.Business.Factory
{
    public class GenericApiClient : IWidgetApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly string _url;
        private readonly string? _apiKey;

        public GenericApiClient(HttpClient httpClient, string url, string? apiKey = null)
        {
            _httpClient = httpClient;
            _url = url;
            _apiKey = apiKey;
        }

        public async Task<string> ObtenerDatosAsync()
        {
            var requestUrl = string.IsNullOrEmpty(_apiKey) ? _url : $"{_url}?apiKey={_apiKey}";
            var response = await _httpClient.GetAsync(requestUrl);
            return await response.Content.ReadAsStringAsync();
        }
    }
}
