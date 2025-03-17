using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net.Http;

using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;

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
            _apiUrl = apiUrl;
            _apiKey = apiKey;
        }

        // Método para obtener datos sin coordenadas (solo datos generales)
        public async Task<KeyValuePair<string, object>> ObtenerDatosAsync(string name)
        {
            if (string.IsNullOrEmpty(_apiUrl) || string.IsNullOrEmpty(_apiKey))
            {
                throw new ArgumentException("La URL de la API o la clave API no pueden estar vacías.");
            }

            try
            {
                // Construcción de la URL con la API Key
                var url = $"{_apiUrl}?appid={_apiKey}";

                // Realizar la solicitud HTTP GET
                var response = await _httpClient.GetAsync(url);

                // Asegurarse de que la respuesta fue exitosa
                response.EnsureSuccessStatusCode();

                // Retornar el contenido de la respuesta
                var content = await response.Content.ReadAsStringAsync();
                return new KeyValuePair<string, object>(name, content);
            }
            catch (Exception ex)
            {
                // Manejar cualquier error que ocurra en la solicitud HTTP
                throw new Exception("Error al obtener los datos de OpenWeather", ex);
            }
        }
    }
}
