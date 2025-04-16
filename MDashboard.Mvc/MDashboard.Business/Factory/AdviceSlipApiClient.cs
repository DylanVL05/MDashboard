// MDashboard.Business.Factory.AdviceSlipApiClient.cs
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using MDashboard.Models;
using MDashboard.Models.ApiModels;
using Newtonsoft.Json;

namespace MDashboard.Business.Factory
{
    public class AdviceSlipApiClient : IWidgetApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiUrl;
        public string Name { get; set; }

        public AdviceSlipApiClient(HttpClient httpClient, Widget widget)
        {
            if (widget == null)
                throw new ArgumentNullException(nameof(widget), "El widget no puede ser nulo.");

            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _apiUrl = widget.UrlApi ?? throw new ArgumentNullException("URL_API", "La URL de la API no puede ser nula.");
            Name = widget.Nombre;
        }

        public async Task<KeyValuePair<string, object>> ObtenerDatosAsync(string name)
        {
            try
            {
                var url = $"{_apiUrl.TrimEnd('/')}";
                Console.WriteLine($"URL utilizada: {url}");

                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                var apiResponse = JsonConvert.DeserializeObject<AdviceSlipResponse>(content);

                return new KeyValuePair<string, object>(name, apiResponse);
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Error al realizar la solicitud a la API de AdviceSlip: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error inesperado al obtener datos de AdviceSlip: {ex.Message}", ex);
            }
        }
    }
}