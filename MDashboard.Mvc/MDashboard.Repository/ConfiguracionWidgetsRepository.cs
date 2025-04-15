using MDashboard.Models;
using MDashboard.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MDashboard.Repository
{
    public interface IConfiguracionWidgetsRepository : IRepositoryBase<ConfiguracionWidget>
    {
        Task<IEnumerable<ConfiguracionWidget>> GetAllConfiguracionesAsync();
        Task<ConfiguracionWidget> GetConfiguracionByIdAsync(int id);
        Task<ConfiguracionWidget> SaveConfiguracionAsync(ConfiguracionWidget configuracion);
        Task<bool> DeleteConfiguracionAsync(int id);
        Task<ConfiguracionWidget> UpdateConfiguracionAsync(int id, ConfiguracionWidget updatedConfiguracion);
        Task<IEnumerable<ConfiguracionWidget>> GetConfiguracionesByUsuarioAsync(int usuarioId);
        Task<IEnumerable<ConfiguracionWidget>> GetConfiguracionesByWidgetAsync(int widgetId);
    }

    public class ConfiguracionWidgetsRepository : RepositoryBase<ConfiguracionWidget>, IConfiguracionWidgetsRepository
    {
        // Obtener todas las configuraciones
        public async Task<IEnumerable<ConfiguracionWidget>> GetAllConfiguracionesAsync()
        {
            return await ReadAsync();
        }

        // Obtener configuraci贸n por ID
        public async Task<ConfiguracionWidget> GetConfiguracionByIdAsync(int id)
        {
            var configuraciones = await ReadAsync();
            return configuraciones.SingleOrDefault(c => c.Id == id);
        }

        // Guardar configuraci贸n (crear o actualizar)
        public async Task<ConfiguracionWidget> SaveConfiguracionAsync(ConfiguracionWidget configuracion)
        {
            var exists = configuracion.Id > 0;
            if (exists)
            {
                await UpdateAsync(configuracion);
            }
            else
            {
                await CreateAsync(configuracion);
            }

            var updatedConfigs = await ReadAsync();
            return updatedConfigs.SingleOrDefault(c => c.Id == configuracion.Id);
        }

        // Eliminar configuraci贸n
        public async Task<bool> DeleteConfiguracionAsync(int id)
        {
            var configuracion = await GetConfiguracionByIdAsync(id);
            if (configuracion != null)
            {
                return await DeleteAsync(configuracion);
            }
            return false;
        }

        // Actualizar configuraci贸n
        public async Task<ConfiguracionWidget> UpdateConfiguracionAsync(int id, ConfiguracionWidget updatedConfiguracion)
        {
            var existingConfig = await GetConfiguracionByIdAsync(id);

            existingConfig.UsuarioId = updatedConfiguracion.UsuarioId;
            existingConfig.WidgetId = updatedConfiguracion.WidgetId;
            existingConfig.Height = updatedConfiguracion.Height;
            existingConfig.Width = updatedConfiguracion.Width;
            existingConfig.EsFavorito = updatedConfiguracion.EsFavorito;
            existingConfig.EsVisible = updatedConfiguracion.EsVisible;

            await UpdateAsync(existingConfig);
            return existingConfig;
        }

        // Obtener configuraciones por usuario
        public async Task<IEnumerable<ConfiguracionWidget>> GetConfiguracionesByUsuarioAsync(int usuarioId)
        {
            var configuraciones = await ReadAsync();
            return configuraciones.Where(c => c.UsuarioId == usuarioId).ToList();
        }

        // Obtener configuraciones por widget
        public async Task<IEnumerable<ConfiguracionWidget>> GetConfiguracionesByWidgetAsync(int widgetId)
        {
            var configuraciones = await ReadAsync();
            return configuraciones.Where(c => c.WidgetId == widgetId).ToList();
        }
    }
}