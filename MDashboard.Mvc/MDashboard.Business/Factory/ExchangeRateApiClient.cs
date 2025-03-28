using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using MDashboard.Models;
using MDashboard.Models.ApiModels;
using Newtonsoft.Json;

namespace MDashboard.Business.Factory
{
    public class ExchangeRateApiClient : IWidgetApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiUrl;
        private readonly string _apiKey;
        private readonly string _baseCurrency;
        public string Name { get; set; }

        public ExchangeRateApiClient(HttpClient httpClient, Widget widget)
        {
            if (widget == null)
                throw new ArgumentNullException(nameof(widget), "El widget no puede ser nulo.");

            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _apiUrl = widget.UrlApi ?? throw new ArgumentNullException("URL_API", "La URL de la API no puede ser nula.");
            _apiKey = widget.ApiKey ?? throw new ArgumentNullException("API_Key", "La clave API no puede ser nula.");

            // Valor por defecto para la moneda base
            _baseCurrency = "USD";
            Name = widget.Nombre;
        }

        public async Task<KeyValuePair<string, object>> ObtenerDatosAsync(string name)
        {
            try
            {
                // Construcción de la URL según el formato requerido por la API
                // Formato esperado: https://v6.exchangerate-api.com/v6/YOUR-API-KEY/latest/USD
                var url = $"{_apiUrl.TrimEnd('/')}/v6/{_apiKey}/latest/{_baseCurrency}";
                Console.WriteLine($"URL utilizada: {url}");

                // Realizar solicitud HTTP GET
                var response = await _httpClient.GetAsync(url);

                // Asegurarse de que la solicitud fue exitosa
                response.EnsureSuccessStatusCode();

                // Leer el contenido de la respuesta
                var content = await response.Content.ReadAsStringAsync();

                // Devolver los datos obtenidos como un KeyValuePair
                return new KeyValuePair<string, object>(name, content);
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Error al realizar la solicitud a la API de ExchangeRate: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error inesperado al obtener datos de ExchangeRate: {ex.Message}", ex);
            }
        }
    }
}