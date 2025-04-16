using MDashboard.Models;
using MDashboard.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace MDashboard.Controllers
{
    [Authorize]
    public class WidgetController : Controller
    {
        private readonly IWidgetRepository _widgetRepository;
        private readonly IComponentRepository _componentRepository;

        public WidgetController(IWidgetRepository widgetRepository, IComponentRepository componentRepository)
        {
            _widgetRepository = widgetRepository;
            _componentRepository = componentRepository;
        }

        // Acción para cargar el formulario de creación de widget
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            // Obtener los componentes disponibles para la lista desplegable
            var components = await _componentRepository.ObtenerComponentesAsync();
            ViewBag.Components = components; // Pasar la lista de componentes a la vista
            return View();
        }


        // Acción para procesar el formulario y guardar el widget
        [HttpPost]
        public async Task<IActionResult> Create(Widget widget)
        {
            await _widgetRepository.AgregarWidgetAsync(widget);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var widget = await _widgetRepository.ObtenerWidgetPorIdAsync(id);
            if (widget != null)
            {
                await _widgetRepository.EliminarWidgetAsync(widget);
            }
            return RedirectToAction("Index");
        }







        // 📌 Acción para listar los widgets
        public async Task<IActionResult> Index()
        {
            var widgets = await _widgetRepository.ObtenerWidgetsActivosAsync();
            return View(widgets);
        }
    }
}
