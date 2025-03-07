using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net.Http;


namespace MDashboard.Business.Factory
{
    
    public class OpenWeatherApiClient : IWidgetApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiUrl;
        private readonly string _apiKey;

        public OpenWeatherApiClient(HttpClient httpClient, string apiUrl, string apiKey)
        {
            _httpClient = httpClient;
            _apiUrl = apiUrl;
            _apiKey = apiKey;
        }

        public async Task<string> ObtenerDatosAsync()
        {
            var response = await _httpClient.GetAsync($"{_apiUrl}?appid={_apiKey}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
    }
   


}
