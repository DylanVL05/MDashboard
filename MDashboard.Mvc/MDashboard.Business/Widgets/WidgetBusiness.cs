using MDashboard.Data.Models;
using MDashboard.Repository;
using MDashboard.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MDashboard.Business.Widgets
{


    public interface IWidgetBusiness
    {
        Task<List<Widget>> ObtenerWidgetsActivosAsync();
        Task<Widget?> ObtenerWidgetPorIdAsync(int widgetId);
        Task<Widget?> ObtenerWidgetPorNombreAsync(string nombre);
        Task AgregarWidgetAsync(Widget widget);
    }



    public class WidgetBusiness : IWidgetBusiness
    {
        private readonly IWidgetRepository _widgetRepository;

        public WidgetBusiness(IWidgetRepository widgetRepository)
        {
            _widgetRepository = widgetRepository;
        }

        // Obtener todos los widgets activos
        public async Task<List<Widget>> ObtenerWidgetsActivosAsync()
        {
            // Obtener todos los widgets activos y asegurarse de incluir las relaciones necesarias
            var widgets = await _widgetRepository.ObtenerWidgetsActivosAsync();
            return widgets; // Puedes agregar lógica adicional si es necesario
        }

        // Obtener un widget por su ID
        public async Task<Widget?> ObtenerWidgetPorIdAsync(int widgetId)
        {
            // Obtener widget por ID, incluye las relaciones necesarias
            var widget = await _widgetRepository.ObtenerWidgetPorIdAsync(widgetId);
            return widget;
        }

        // Obtener un widget por su nombre
        public async Task<Widget?> ObtenerWidgetPorNombreAsync(string nombre)
        {
            // Obtener widget por nombre, incluye las relaciones necesarias
            var widget = await _widgetRepository.ObtenerWidgetPorNombreAsync(nombre);
            return widget;
        }

        // Agregar un nuevo widget
        public async Task AgregarWidgetAsync(Widget widget)
        {
            // Aquí se puede agregar cualquier lógica de negocio antes de agregar el widget
            await _widgetRepository.AgregarWidgetAsync(widget);
        }
    }
}
