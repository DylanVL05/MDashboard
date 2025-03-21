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
            //Aqui llega muerto.
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
                                try
                                {
                                    var weatherResponse = JsonConvert.DeserializeObject<ClimaResponse>(result.Value.ToString());

                                    if (weatherResponse != null)
                                    {
                                        var weatherInfo = new MainInfo
                                        {
                                            Temp = weatherResponse.Main?.Temp ?? 0,
                                            Humidity = weatherResponse.Main?.Humidity ?? 0,
                                            Pressure = weatherResponse.Main?.Pressure ?? 0
                                        };

                                        resultados.Add(x.widget.Nombre, weatherInfo);

                                    }
                                    else
                                    {
                                        Console.WriteLine($"Error: No se pudo deserializar el JSON para {x.widget.Nombre}");
                                    }
                                }
                                catch (JsonSerializationException ex)
                                {
                                    Console.WriteLine($"Error de deserialización para {x.widget.Nombre}: {ex.Message}");
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine($"Error inesperado para {x.widget.Nombre}: {ex.Message}");
                                }
                            }
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
