using MDashboard.Data.Models;
using MDashboard.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks;



namespace MDashboard.Repository
{


    public interface IWidgetRepository
    {
        Task<List<Widget>> ObtenerWidgetsActivosAsync();
        Task<Widget?> ObtenerWidgetPorIdAsync(int widgetId);
        Task<Widget?> ObtenerWidgetPorNombreAsync(string nombre);

    }
    public class WidgetRepository : IWidgetRepository
    {
        private readonly MediaDashboardContext _context;

        public WidgetRepository(MediaDashboardContext context)
        {
            _context = context;
        }

        // 📌 Obtener todos los widgets activos
        public async Task<List<Widget>> ObtenerWidgetsActivosAsync()
        {
            return await _context.Widgets
                .Where(w => w.Estado == true) // Solo los widgets activos
                .Include(w => w.Component)   // Incluir el tipo de widget
                .ToListAsync();
        }

        // 📌 Obtener un widget por su ID
        public async Task<Widget?> ObtenerWidgetPorIdAsync(int widgetId)
        {
            return await _context.Widgets
                .Include(w => w.Component)
                .FirstOrDefaultAsync(w => w.Id == widgetId);
        }

        // 📌 Obtener un widget por su nombre
        public async Task<Widget?> ObtenerWidgetPorNombreAsync(string nombre)
        {
            return await _context.Widgets
                .Include(w => w.Component)
                .FirstOrDefaultAsync(w => w.Nombre == nombre);
        }
    }

   
}


