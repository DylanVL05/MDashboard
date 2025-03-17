using MDashboard.Repository;
using MDashboard.Models.ApiModels;
using MDashboard.Data.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using MDashboard.Business.Factory;
using MDashboard.Models;

namespace MDashboard.Business.Services
{
    public class WidgetService
    {
        private readonly IWidgetRepository _widgetRepository;
        private readonly WidgetApiFactory _apiFactory;

        public WidgetService(IWidgetRepository widgetRepository, WidgetApiFactory apiFactory)
        {
            _widgetRepository = widgetRepository;
            _apiFactory = apiFactory;
        }

        public async Task<Dictionary<string, object>> ObtenerDatosDeWidgetsAsync()
        {
            var widgets = await _widgetRepository.ObtenerWidgetsActivosAsync();
            var resultados = new Dictionary<string, object>();

            // Usamos Parallel.ForEachAsync para recorrer los widgets en paralelo
            await Parallel.ForEachAsync(
                widgets.Select(any => new {
                    client = _apiFactory.CrearCliente(any),
                    widget = any,
                }), async (x, _) => {
                    // Obtener los datos desde el cliente de la API
                    var result = await x.client.ObtenerDatosAsync(x.widget.Nombre);

                    // Si la URL de la API contiene "openweathermap.org", deserializamos los datos
                    if (x.widget.UrlApi.Contains("openweathermap.org"))
                    {
                        // Deserializamos la respuesta a un objeto de tipo OpenWeatherResponse
                        var weatherData = JsonConvert.DeserializeObject<OpenWeatherResponse>(result.Value.ToString());
                        // Agregar la respuesta deserializada al diccionario
                        resultados.Add(result.Key, weatherData);
                    }
                    else
                    {
                        // Si no es OpenWeather, agregamos los datos tal cual están
                        resultados.Add(result.Key, result.Value);
                    }
                });

            return resultados;
        }

        public async Task AgregarWidgetAsync(Widget widget)
        {
            await _widgetRepository.AgregarWidgetAsync(widget);
        }



    }
}







    
