using MDashboard.Business.Factory;
using MDashboard.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;


namespace MDashboard.Business.Services
{


    public class WidgetService
    {
        private readonly WidgetRepository _widgetRepository;
        private readonly WidgetApiFactory _apiFactory;

        public WidgetService(WidgetRepository widgetRepository, WidgetApiFactory apiFactory)
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