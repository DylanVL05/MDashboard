using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using MDashboard.Models.ApiModels;
using Newtonsoft.Json;
using MDashboard.Models;

namespace MDashboard.Business.Factory
{
    public class ChuckNorrisApiClient : IWidgetApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiUrl;
        public string Name { get; set; }

        public ChuckNorrisApiClient(HttpClient httpClient, Widget widget)
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
                Console.WriteLine($"Respuesta JSON: {content}");

                var apiResponse = JsonConvert.DeserializeObject<ChuckNorrisApiResponse>(content);

                return new KeyValuePair<string, object>(name, apiResponse);
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Error al realizar la solicitud a la API de Chuck Norris: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error inesperado al obtener datos de Chuck Norris: {ex.Message}", ex);
            }
        }
    }
}