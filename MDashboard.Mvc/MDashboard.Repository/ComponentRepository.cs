using MDashboard.Data.Models;
using MDashboard.Models;
using MDashboard.Repository;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MDashboard.Repository
{


    public interface IComponentRepository
    {
       
        Task<List<Component>> ObtenerComponentesAsync();
    }

public class ComponentRepository : IComponentRepository
    {
        private readonly MediaDashboardContext _context;

        public ComponentRepository(MediaDashboardContext context)
        {
            _context = context;
        }

        // Implementación del método async
        public async Task<List<Component>> ObtenerComponentesAsync()
        {
            // Obtener todos los componentes de la base de datos de manera asíncrona
            return await _context.Components.ToListAsync();
        }
    }
}
