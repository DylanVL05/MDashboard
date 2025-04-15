using MDashboard.Business;
using MDashboard.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MDashboard.Repository;
namespace MDashboard.Services
{
    public interface IConfiguracionWidgetService
    {
        Task<IEnumerable<ConfiguracionWidget>> GetAllConfiguracionesAsync();
        Task<ConfiguracionWidget> GetConfiguracionByIdAsync(int id);
        Task<ConfiguracionWidget> SaveConfiguracionAsync(ConfiguracionWidget configuracion);
        Task<bool> DeleteConfiguracionAsync(int id);
        Task<ConfiguracionWidget> UpdateConfiguracionAsync(int id, ConfiguracionWidget configuracion);
        Task<IEnumerable<ConfiguracionWidget>> GetConfiguracionesByUsuarioAsync(int usuarioId);
        Task<IEnumerable<ConfiguracionWidget>> GetConfiguracionesByWidgetAsync(int widgetId);
    }

    public class ConfiguracionWidgetService : IConfiguracionWidgetService
    {
        private readonly IConfiguracionWidgetsBusiness _configBusiness;
        private readonly ILogger<ConfiguracionWidgetService> _logger;
        private readonly IConfiguracionWidgetsRepository _configuracionRepository;

        public ConfiguracionWidgetService(
            IConfiguracionWidgetsBusiness configBusiness,
            ILogger<ConfiguracionWidgetService> logger,
            IConfiguracionWidgetsRepository configuracionWidgetsRepository)
        {
            _configBusiness = configBusiness;
            _logger = logger;
            _configuracionRepository = configuracionWidgetsRepository;
        }

        public async Task<IEnumerable<ConfiguracionWidget>> GetAllConfiguracionesAsync()
        {
            try
            {
                _logger.LogInformation("Obteniendo todas las configuraciones de widgets");
                return await _configBusiness.GetAllConfiguracionesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todas las configuraciones");
                throw;
            }
        }

        public async Task<ConfiguracionWidget> GetConfiguracionByIdAsync(int id)
        {
            try
            {
                _logger.LogInformation($"Buscando configuración con ID: {id}");
                return await _configBusiness.GetConfiguracionByIdAsync(id);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, $"Configuración no encontrada con ID: {id}");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al buscar configuración con ID: {id}");
                throw;
            }
        }

        public async Task<ConfiguracionWidget> SaveConfiguracionAsync(ConfiguracionWidget configuracion)
        {
            try
            {
                // Validaciones adicionales
                if (configuracion.UsuarioId <= 0 || configuracion.WidgetId <= 0)
                {
                    throw new ArgumentException("Debe seleccionar un usuario y widget válidos");
                }

                return await _configuracionRepository.SaveConfiguracionAsync(configuracion);
            }
            catch (Exception ex)
            {
                // Loggear error
                throw new ApplicationException("Error al guardar la configuración", ex);
            }
        }

        public async Task<bool> DeleteConfiguracionAsync(int id)
        {
            try
            {
                _logger.LogInformation($"Eliminando configuración con ID: {id}");
                return await _configBusiness.DeleteConfiguracionAsync(id);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, $"Intento de eliminar configuración no existente ID: {id}");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al eliminar configuración con ID: {id}");
                throw;
            }
        }

        public async Task<ConfiguracionWidget> UpdateConfiguracionAsync(int id, ConfiguracionWidget configuracion)
        {
            try
            {
                _logger.LogInformation($"Actualizando configuración con ID: {id}");
                return await _configBusiness.UpdateConfiguracionAsync(id, configuracion);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, $"Intento de actualizar configuración no existente ID: {id}");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al actualizar configuración con ID: {id}");
                throw;
            }
        }

        public async Task<IEnumerable<ConfiguracionWidget>> GetConfiguracionesByUsuarioAsync(int usuarioId)
        {
            try
            {
                _logger.LogInformation($"Obteniendo configuraciones para usuario ID: {usuarioId}");
                return await _configBusiness.GetConfiguracionesByUsuarioAsync(usuarioId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al obtener configuraciones para usuario ID: {usuarioId}");
                throw;
            }
        }

        public async Task<IEnumerable<ConfiguracionWidget>> GetConfiguracionesByWidgetAsync(int widgetId)
        {
            try
            {
                _logger.LogInformation($"Obteniendo configuraciones para widget ID: {widgetId}");
                return await _configBusiness.GetConfiguracionesByWidgetAsync(widgetId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al obtener configuraciones para widget ID: {widgetId}");
                throw;
            }
        }
    }
}