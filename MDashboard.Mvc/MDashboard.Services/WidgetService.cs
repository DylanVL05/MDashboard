using MDashboard.Repository;
using MDashboard.Models.ApiModels;
using MDashboard.Data.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
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

            // Procesar los widgets en paralelo
            await Parallel.ForEachAsync(
                widgets.Select(widget => new
                {
                    client = _apiFactory.CrearCliente(widget),
                    widget
                }), async (x, _) =>
                {
                    try
                    {
                        // Obtener los datos desde la API correspondiente
                        var result = await x.client.ObtenerDatosAsync(x.widget.Nombre);

                        if (result.Value != null)
                        {
                            Console.WriteLine($"Datos obtenidos para {x.widget.Nombre}: {result.Value}");

                            // Deserialización específica para OpenWeather API
                            if (x.widget.UrlApi.Contains("openweathermap.org"))
                            {
                                var weatherResponse = JsonConvert.DeserializeObject<OpenWeatherResponse>(result.Value.ToString());

                                if (weatherResponse != null && weatherResponse.Data != null && weatherResponse.Data.Any())
                                {
                                    // Tomar el primer elemento de la lista "data"
                                    var weatherData = weatherResponse.Data.FirstOrDefault();
                                    resultados.Add(result.Key, weatherData);
                                }
                                else
                                {
                                    Console.WriteLine($"No se encontraron datos válidos en la respuesta para {x.widget.Nombre}");
                                }
                            }
                            else
                            {
                                // Otros casos (datos genéricos)
                                resultados.Add(result.Key, result.Value);
                            }
                        }
                        else
                        {
                            Console.WriteLine($"No se obtuvieron datos para {x.widget.Nombre}");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error procesando widget {x.widget.Nombre}: {ex.Message}");
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
