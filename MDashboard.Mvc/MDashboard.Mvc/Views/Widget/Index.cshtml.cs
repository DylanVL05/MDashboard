using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MDashboard.Business.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MDashboard.Mvc.Pages.Widget
{
    public class IndexModel : PageModel
    {
        private readonly WidgetService _widgetService;

        public IndexModel(WidgetService widgetService)
        {
            _widgetService = widgetService;
        }

        // Propiedad que contendrá los datos de los widgets
        public Dictionary<string, string> WidgetData { get; set; }

        public async Task OnGetAsync()
        {
            // Llamada al servicio para obtener los datos de los widgets
            WidgetData = await _widgetService.ObtenerDatosDeWidgetsAsync();
        }
    }
}
