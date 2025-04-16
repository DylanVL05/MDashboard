using MDashboard.Models;
using MDashboard.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MDashboard.Business
{
    public interface IConfiguracionWidgetsBusiness
    {
        Task<IEnumerable<ConfiguracionWidget>> GetAllConfiguracionesAsync();
        Task<ConfiguracionWidget> GetConfiguracionByIdAsync(int id);
        Task<ConfiguracionWidget> SaveConfiguracionAsync(ConfiguracionWidget configuracion);
        Task<bool> DeleteConfiguracionAsync(int id);
        Task<ConfiguracionWidget> UpdateConfiguracionAsync(int id, ConfiguracionWidget configuracion);
        Task<IEnumerable<ConfiguracionWidget>> GetConfiguracionesByUsuarioAsync(int usuarioId);
        Task<IEnumerable<ConfiguracionWidget>> GetConfiguracionesByWidgetAsync(int widgetId);
    }

    public class ConfiguracionWidgetsBusiness : IConfiguracionWidgetsBusiness
    {
        private readonly IConfiguracionWidgetsRepository _configuracionRepository;

        public ConfiguracionWidgetsBusiness(IConfiguracionWidgetsRepository configuracionRepository)
        {
            _configuracionRepository = configuracionRepository;
        }

        public async Task<IEnumerable<ConfiguracionWidget>> GetAllConfiguracionesAsync()
        {
            return await _configuracionRepository.GetAllConfiguracionesAsync();
        }

        public async Task<ConfiguracionWidget> GetConfiguracionByIdAsync(int id)
        {
            return await _configuracionRepository.GetConfiguracionByIdAsync(id);
        }

        public async Task<ConfiguracionWidget> SaveConfiguracionAsync(ConfiguracionWidget configuracion)
        {
            // Validación básica de negocio
            if (configuracion.Height <= 0 || configuracion.Width <= 0)
            {
                throw new ArgumentException("Las dimensiones del widget deben ser mayores a cero");
            }

            return await _configuracionRepository.SaveConfiguracionAsync(configuracion);
        }

        public async Task<bool> DeleteConfiguracionAsync(int id)
        {
            return await _configuracionRepository.DeleteConfiguracionAsync(id);
        }

        public async Task<ConfiguracionWidget> UpdateConfiguracionAsync(int id, ConfiguracionWidget configuracion)
        {
            // Validación de existencia
            var existingConfig = await _configuracionRepository.GetConfiguracionByIdAsync(id);
            if (existingConfig == null)
            {
                throw new KeyNotFoundException("Configuración no encontrada");
            }

            return await _configuracionRepository.UpdateConfiguracionAsync(id, configuracion);
        }

        public async Task<IEnumerable<ConfiguracionWidget>> GetConfiguracionesByUsuarioAsync(int usuarioId)
        {
            return await _configuracionRepository.GetConfiguracionesByUsuarioAsync(usuarioId);
        }

        public async Task<IEnumerable<ConfiguracionWidget>> GetConfiguracionesByWidgetAsync(int widgetId)
        {
            return await _configuracionRepository.GetConfiguracionesByWidgetAsync(widgetId);
        }
    }
}