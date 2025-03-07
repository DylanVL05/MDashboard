using System.Collections.Generic;
using System.Threading.Tasks;
using MDashboard.Business.Services;
using Microsoft.AspNetCore.Mvc;

namespace MDashboard.Mvc.Controllers
{
    [Route("widgets")]
    public class WidgetController : Controller
    {
        private readonly WidgetService _widgetService;

        public WidgetController(WidgetService widgetService)
        {
            _widgetService = widgetService;
        }

        // 📌 Obtener datos de todos los widgets activos y mostrarlos en la vista
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            // Obtén los datos de los widgets activos
            var resultados = await _widgetService.ObtenerDatosDeWidgetsAsync();

            // Devuelve los resultados a la vista Index
            return View(resultados);
        }

        // 📌 Obtener datos de un widget específico por su nombre y mostrarlos en la vista
        [HttpGet("{nombre}")]
        public async Task<IActionResult> Detalles(string nombre)
        {
            var resultados = await _widgetService.ObtenerDatosDeWidgetsAsync();

            if (resultados.TryGetValue(nombre, out var datos))
            {
                // Si el widget existe, lo pasa a la vista Detalles
                return View("Detalles", new { nombre, datos });
            }

            // Si no se encuentra el widget, redirige a la vista Index
            return RedirectToAction("Index");
        }
    }
}
