using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;
using MDashboard.Models.ApiModels;
using Newtonsoft.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MDashboard.Business.Factory
{
    public class GenericApiClient : IWidgetApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly string _url;
        private readonly string? _apiKey;
        public string Name { get; set; }

        public GenericApiClient(HttpClient httpClient, string url, string? apiKey = null)
        {
            _httpClient = httpClient;
            _url = url;
            _apiKey = apiKey;
        }

        public async Task<KeyValuePair<string, object>> ObtenerDatosAsync(string name)
        {
            var requestUrl = string.IsNullOrEmpty(_apiKey) ? _url : $"{_url}?apiKey={_apiKey}";
            var response = await _httpClient.GetAsync(requestUrl);
            var data = await response.Content.ReadAsStringAsync();
            var deserialized = JsonConvert.DeserializeObject<OpenWeatherResponse>(data);
            return new KeyValuePair<string, object>(name, deserialized);
        }

        public Task Set()
        {
            throw new NotImplementedException();
        }
    }
}
