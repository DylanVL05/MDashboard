using MDashboard.Models;
using MDashboard.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace MDashboard.Controllers
{
    public class WidgetController : Controller
    {
        private readonly IWidgetRepository _widgetRepository;

        public WidgetController(IWidgetRepository widgetRepository)
        {
            _widgetRepository = widgetRepository;
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }


        // 📌 Acción para procesar el formulario y guardar el widget
        [HttpPost]
        public async Task<IActionResult> Create(Widget widget)
        {
            if (ModelState.IsValid)
            {
                await _widgetRepository.AgregarWidgetAsync(widget);
                return RedirectToAction("Index");
            }
            return View(widget);
        }

        // 📌 Acción para listar los widgets
        public async Task<IActionResult> Index()
        {
            var widgets = await _widgetRepository.ObtenerWidgetsActivosAsync();
            return View(widgets);
        }
    }
}
