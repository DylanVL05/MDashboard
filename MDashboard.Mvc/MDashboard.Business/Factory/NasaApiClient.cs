using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using MDashboard.Models;
using MDashboard.Models.ApiModels;
using Newtonsoft.Json;


namespace MDashboard.Business.Factory
{
    public class NasaApiClient : IWidgetApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiUrl;
        private readonly string _apiKey;
        public string Name { get; set; }

        public NasaApiClient(HttpClient httpClient, Widget widget)
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
                var url = $"{_apiUrl.TrimEnd('/')}?api_key={_apiKey}";
                Console.WriteLine($"URL utilizada: {url}");

                var response = await _httpClient.GetAsync(url);

                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();

                var nasaResponse = JsonConvert.DeserializeObject<NasaApodResponse>(content);

                return new KeyValuePair<string, object>(name, nasaResponse);
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Error al realizar la solicitud a la API de NASA APOD: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error inesperado al obtener datos de NASA APOD: {ex.Message}", ex);
            }
        }
    }
}
