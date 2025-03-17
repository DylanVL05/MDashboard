using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace MDashboard.Business.Factory
{
    public class OpenWeatherApiClient : IWidgetApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiUrl;
        private readonly string _apiKey;
        public string Name { get; set; }

        public OpenWeatherApiClient(HttpClient httpClient, string apiUrl, string apiKey)
        {
            _httpClient = httpClient;
            _apiUrl = apiUrl ?? throw new ArgumentNullException(nameof(apiUrl), "La URL de la API no puede ser nula.");
            _apiKey = apiKey ?? throw new ArgumentNullException(nameof(apiKey), "La clave API no puede ser nula.");
        }

        // Método para obtener datos dinámicos desde OpenWeather API
        public async Task<KeyValuePair<string, object>> ObtenerDatosAsync(string name)
        {
            try
            {
                // Coordenadas de ejemplo (Nueva York, modificables)
                double lat = 40.7128;  // Latitud
                double lon = -74.0060; // Longitud
                string units = "metric"; // Unidades (metric, imperial o standard)
                string lang = "es"; // Idioma de la respuesta

                // Construcción de la URL
                var url = $"{_apiUrl}?lat={lat}&lon={lon}&units={units}&lang={lang}&appid={_apiKey}";

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
                throw new Exception($"Error al realizar la solicitud a la API de OpenWeather: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error inesperado al obtener datos de OpenWeather: {ex.Message}", ex);
            }
        }
    }
}
