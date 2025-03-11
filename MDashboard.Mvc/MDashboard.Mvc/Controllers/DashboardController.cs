using MDashboard.Business.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MDashboard.Controllers
{
    public class DashboardController : Controller
    {
        private readonly WidgetService _widgetService;

        public DashboardController(WidgetService widgetService)
        {
            _widgetService = widgetService;
        }

      
    }
}