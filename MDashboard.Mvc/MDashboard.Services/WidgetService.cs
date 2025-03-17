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

            await Parallel.ForEachAsync(
                widgets.Select(any => new {
                    client = _apiFactory.CrearCliente(any),
                    widget = any,
                }), async (x, _) => {
                var result = await x.client.ObtenerDatosAsync(x.widget.Nombre);
                resultados.Add(result.Key, result.Value); 
            });
            /*
            foreach (var widget in widgets)
            {
                var clienteApi = _apiFactory.CrearCliente(widget);
                var datos = await clienteApi.ObtenerDatosAsync();

                if (widget.UrlApi.Contains("openweathermap.org"))
                {
                    var weatherData = JsonConvert.DeserializeObject<OpenWeatherResponse>(datos);
                    resultados.Add(widget.Nombre, weatherData);
                }
                else
                {
                    resultados.Add(widget.Nombre, datos);
                }
            }*/

            return resultados;
        }

        public async Task AgregarWidgetAsync(Widget widget)
        {
            await _widgetRepository.AgregarWidgetAsync(widget);
        }



    }
}







    
