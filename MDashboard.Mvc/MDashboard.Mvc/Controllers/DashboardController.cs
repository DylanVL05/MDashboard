using MDashboard.Models;
using MDashboard.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;  // Para autorización
using System.Threading.Tasks;
using MDashboard.Business.Services;
using Newtonsoft.Json;

namespace MDashboard.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly IWidgetRepository _widgetRepository;
        private readonly WidgetService _widgetService;

        public DashboardController(IWidgetRepository widgetRepository, WidgetService widgetService)
        {
            _widgetRepository = widgetRepository;
            _widgetService = widgetService;
        }

        // Acción para mostrar el dashboard con los widgets existentes
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                // Obtener widgets almacenados en la base de datos
                var widgets = await _widgetRepository.ObtenerWidgetsActivosAsync();

                // Obtener los datos de los widgets
                var dynamicData = await _widgetService.ObtenerDatosDeWidgetsAsync();

                // Debugging: Ver los datos de los widgets
                Console.WriteLine("Datos de los widgets:");
                foreach (var item in dynamicData)
                {
                    Console.WriteLine($"{item.Key}: {JsonConvert.SerializeObject(item.Value)}");
                }

                ViewBag.DynamicData = dynamicData ?? new Dictionary<string, object>();

                return View(widgets);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al cargar el dashboard: {ex.Message}");
            }
        }


        [HttpPost]
        public async Task<IActionResult> UpdateWidgetVisibility(int widgetId, bool isVisible)
        {
            try
            {
                // Aquí puedes guardar el estado de visibilidad en tu base de datos si lo necesitas
                // Por ejemplo:
                //var widget = await _widgetRepository.ObtenerWidgetPorIdAsync(widgetId);
                // if (widget != null) {
                //     widget.IsVisible = isVisible;
                //     await _widgetRepository.ActualizarWidgetAsync(widget);
                // }

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerWidgetsParciales()
        {
            try
            {
                var widgets = await _widgetRepository.ObtenerWidgetsActivosAsync();
                var dynamicData = await _widgetService.ObtenerDatosDeWidgetsAsync();
                ViewBag.DynamicData = dynamicData ?? new Dictionary<string, object>();

                return PartialView("_WidgetPartial", widgets); // nueva vista parcial
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al actualizar los widgets: {ex.Message}");
            }
        }



        // Acción para agregar un widget preexistente al dashboard
        [HttpPost]
        public async Task<IActionResult> AgregarWidget(int widgetId)
        {
            // Obtener el widget por su ID
            var widget = await _widgetRepository.ObtenerWidgetPorIdAsync(widgetId);

            if (widget != null)
            {
                // Lógica para agregar el widget al dashboard
                // Aquí guardamos el widget en una tabla que asocia los widgets con el usuario, si se desea

                // Para este ejemplo, simplemente podemos actualizar el estado o realizar alguna otra acción que refleje el agregado del widget
                // Ejemplo: actualizar el estado del widget a "agregado" o asociarlo a un usuario, dependiendo de tu lógica

                // Redirigir al dashboard para mostrar el widget agregado
                return RedirectToAction("Index");
            }

            return NotFound();  // Si no se encuentra el widget
        }
    }
}
