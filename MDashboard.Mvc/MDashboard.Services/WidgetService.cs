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

                            // Si el widget pertenece a OpenWeather, procesar y deserializar los datos
                            if (x.widget.UrlApi.Contains("openweathermap.org"))
                            {
                                var weatherResponse = JsonConvert.DeserializeObject<OpenWeatherResponse>(result.Value.ToString());

                                // Validar si la lista `Data` existe y contiene elementos
                                if (weatherResponse != null && weatherResponse.Data != null && weatherResponse.Data.Any())
                                {
                                    // Extraer el primer elemento de `Data`
                                    var weatherData = weatherResponse.Data.FirstOrDefault();

                                    if (weatherData != null)
                                    {
                                        // Preparar los datos relevantes para el diccionario
                                        var weatherInfo = new
                                        {
                                            Temperature = weatherData.Temp, // Temperatura
                                            Humidity = weatherData.Humidity, // Humedad
                                            WeatherCondition = weatherData.Weather.FirstOrDefault()?.Description // Descripción del clima
                                        };

                                        // Agregar los datos procesados al diccionario
                                        resultados.Add(x.widget.Nombre, weatherInfo);
                                    }
                                    else
                                    {
                                        Console.WriteLine($"No se encontraron datos válidos en 'Data' para {x.widget.Nombre}");
                                    }
                                }
                                else
                                {
                                    Console.WriteLine($"La respuesta de OpenWeather para {x.widget.Nombre} no contiene información válida");
                                }
                            }
                            else
                            {
                                // Si el widget no pertenece a OpenWeather, agregar los datos sin procesar
                                resultados.Add(x.widget.Nombre, result.Value);
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
