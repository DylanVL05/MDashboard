using MDashboard.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace MDashboard.Business.Factory
{
    public class NewsApiClient : IWidgetApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiUrl;
        private readonly string _apiKey;
        public string Name { get; set; }

        public NewsApiClient(HttpClient httpClient, Widget widget)
        {
            if (widget == null)
                throw new ArgumentNullException(nameof(widget), "El widget no puede ser nulo.");

            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _apiUrl = widget.UrlApi ?? throw new ArgumentNullException("URL_API", "La URL de la API no puede ser nula.");
            _apiKey = widget.ApiKey ?? throw new ArgumentNullException("API_Key", "La clave API no puede ser nula.");

            Name = widget.Nombre;
        }

        public async Task<KeyValuePair<string, object>> ObtenerDatosAsync(string name)
        {
            try
            {
                // Construcción de la URL para obtener titulares de EE.UU.
                var url = $"{_apiUrl.TrimEnd('/')}/v2/top-headlines?country=us&apiKey={_apiKey}";
                Console.WriteLine($"URL utilizada: {url}");

                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                return new KeyValuePair<string, object>(name, content);
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Error al realizar la solicitud a NewsAPI: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error inesperado al obtener noticias: {ex.Message}", ex);
            }
        }
    }
}