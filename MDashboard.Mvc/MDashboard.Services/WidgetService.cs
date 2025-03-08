using MDashboard.Business.Factory;
using MDashboard.Repository;
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
    }
}