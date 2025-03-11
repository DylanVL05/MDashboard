using MDashboard.Business.Factory;
using MDashboard.Models.ApiModels;
using MDashboard.Repository;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MDashboard.Business.Services
{
    public class WidgetService
    {

        private readonly IWidgetRepository _widgetRepository; // Usa la interfaz
        private readonly WidgetApiFactory _apiFactory;

        public WidgetService(IWidgetRepository widgetRepository, WidgetApiFactory apiFactory) // Usa la interfaz
        {
            _widgetRepository = widgetRepository;
            _apiFactory = apiFactory;
        }

        /**
        public async Task<Dictionary<string, string>> ObtenerDatosDeWidgetsAsync()
        {
            var widgets = _widgetRepository.ObtenerWidgetsActivosAsync();
            var resultados = new Dictionary<string, string>();

            foreach (var widget in await widgets)
            {
                var clienteApi = _apiFactory.CrearCliente(widget);
                var datos = await clienteApi.ObtenerDatosAsync();
                resultados.Add(widget.Nombre, datos);
            }

            return resultados;
        }
        **/



        public async Task<Dictionary<string, object>> ObtenerDatosDeWidgetsAsync()
        {
            var widgets = _widgetRepository.ObtenerWidgetsActivosAsync();
            var resultados = new Dictionary<string, object>();

            foreach (var widget in await widgets)
            {
                var clienteApi = _apiFactory.CrearCliente(widget);
                var datos = await clienteApi.ObtenerDatosAsync();

                // En este caso, si es de OpenWeather, deserializas a un tipo específico (Se manejan los datos por la clase OpenWeatherResponse)
                if (widget.UrlApi.Contains("openweathermap.org"))
                {
                    var weatherData = JsonConvert.DeserializeObject<OpenWeatherResponse>(datos);
                    resultados.Add(widget.Nombre, weatherData);
                }
                else
                {
                    resultados.Add(widget.Nombre, datos);  // Caso genérico (Probablemente se obtenga todo los datos)
                }
            }

            return resultados;
        }










    }
}