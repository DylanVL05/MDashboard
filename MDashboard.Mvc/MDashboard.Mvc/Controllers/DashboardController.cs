using MDashboard.Models;
using MDashboard.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MDashboard.Business.Services;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using MDashboard.Data.Models;

namespace MDashboard.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly IWidgetRepository _widgetRepository;
        private readonly WidgetService _widgetService;
        private readonly MediaDashboardContext _context;

        public DashboardController(IWidgetRepository widgetRepository, WidgetService widgetService, MediaDashboardContext context)
        {
            _widgetRepository = widgetRepository;
            _widgetService = widgetService;
            _context = context;
        }

        // Acción para mostrar el dashboard con los widgets existentes
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                int usuarioId = ObtenerUsuarioActual();

                // Obtener todos los widgets activos
                var todosLosWidgets = await _widgetRepository.ObtenerWidgetsActivosAsync();

                // Obtener configuraciones del usuario actual
                var configuracionesUsuario = await _context.ConfiguracionWidgets
                    .Where(c => c.UsuarioId == usuarioId)
                    .ToDictionaryAsync(c => c.WidgetId, c => c);

                // Separar widgets en visibles y ocultos según la configuración guardada
                var widgetsVisibles = new List<Widget>();
                var widgetsOcultos = new List<Widget>();

                foreach (var widget in todosLosWidgets)
                {
                    // Si existe configuración y está marcado como no visible, lo ponemos en ocultos
                    if (configuracionesUsuario.TryGetValue(widget.Id, out var config) && !config.EsVisible)
                    {
                        widgetsOcultos.Add(widget);
                    }
                    else
                    {
                        // Si no hay configuración o está marcado como visible
                        widgetsVisibles.Add(widget);
                    }
                }

                // Obtener los datos dinámicos de los widgets
                var dynamicData = await _widgetService.ObtenerDatosDeWidgetsAsync();

                // Pasar los datos a la vista
                ViewBag.DynamicData = dynamicData ?? new Dictionary<string, object>();
                ViewBag.WidgetsOcultos = widgetsOcultos;

                return View(widgetsVisibles);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al cargar el dashboard: {ex.Message}");
                return StatusCode(500, $"Error al cargar el dashboard: {ex.Message}");
            }
        }

        // Acción para agregar un widget preexistente al dashboard
        [HttpPost]
        public async Task<IActionResult> AgregarWidget(int widgetId)
        {
            // Obtener el widget por su ID
            var widget = await _widgetRepository.ObtenerWidgetPorIdAsync(widgetId);

            if (widget != null)
            {
                // Lógica para agregar el widget al dashboard
                // Aquí guardamos el widget en una tabla que asocia los widgets con el usuario, si se desea

                // Redirigir al dashboard para mostrar el widget agregado
                return RedirectToAction("Index");
            }

            return NotFound();  // Si no se encuentra el widget
        }

        // Acción para ocultar un widget (actualizar a no visible)
        [HttpPost]
        public IActionResult ActualizarVisibilidadWidget(int widgetId, bool esVisible)
        {
            try
            {
                int usuarioId = ObtenerUsuarioActual();

                var configuracion = _context.ConfiguracionWidgets
                    .FirstOrDefault(c => c.UsuarioId == usuarioId && c.WidgetId == widgetId);

                if (configuracion != null)
                {
                    // Si existe, actualizamos la visibilidad
                    configuracion.EsVisible = esVisible;
                }
                else
                {
                    // Si no existe, creamos una nueva entrada
                    _context.ConfiguracionWidgets.Add(new ConfiguracionWidget
                    {
                        UsuarioId = usuarioId,
                        WidgetId = widgetId,
                        EsVisible = esVisible,
                        // Valores predeterminados para otras propiedades
                        Width = 1,
                        Height = 1,
                        EsFavorito = false
                    });
                }

                // Guardar cambios en la base de datos
                _context.SaveChanges();

                // Retornar una respuesta de éxito con información adicional
                return Json(new { success = true, widgetId = widgetId, esVisible = esVisible });
            }
            catch (Exception ex)
            {
                // Manejo de excepciones, loguear si es necesario
                Console.WriteLine($"Error al actualizar la visibilidad del widget: {ex.Message}");
                return StatusCode(500, "Error al actualizar la visibilidad del widget");
            }
        }

        // Acción para mostrar un widget oculto (actualizar a visible)
        [HttpPost]
        public IActionResult MostrarWidget(int widgetId)
        {
            try
            {
                // Reutilizamos el método anterior pero estableciendo esVisible a true
                return ActualizarVisibilidadWidget(widgetId, true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al mostrar el widget: {ex.Message}");
                return StatusCode(500, "Error al mostrar el widget");
            }
        }

        // Acción para ocultar un widget (actualizar a no visible)
        [HttpPost]
        public IActionResult OcultarWidget(int widgetId)
        {
            try
            {
                // Reutilizamos el método anterior pero estableciendo esVisible a false
                return ActualizarVisibilidadWidget(widgetId, false);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al ocultar el widget: {ex.Message}");
                return StatusCode(500, "Error al ocultar el widget");
            }
        }

        // Método auxiliar para obtener el ID del usuario actual
        private int ObtenerUsuarioActual()
        {
            var claim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (claim == null)
            {
                throw new InvalidOperationException("No se pudo obtener el ID del usuario autenticado");
            }
            return int.Parse(claim.Value);
        }
        [HttpGet]
        public async Task<IActionResult> ObtenerWidgetsParciales()
        {
            try
            {
                var widgets = await _widgetRepository.ObtenerWidgetsActivosAsync();
                var dynamicData = await _widgetService.ObtenerDatosDeWidgetsAsync();
                ViewBag.DynamicData = dynamicData ?? new Dictionary<string, object>();

                return PartialView("_WidgetPartial", widgets); // nueva vista parcial
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al actualizar los widgets: {ex.Message}");
            }
        }

        // Acción para obtener todos los widgets disponibles (visible o no)
        [HttpGet]
        public async Task<IActionResult> ObtenerTodosLosWidgets()
        {
            try
            {
                int usuarioId = ObtenerUsuarioActual();

                // Obtener todos los widgets
                var widgets = await _widgetRepository.ObtenerWidgetsActivosAsync();

                // Obtener configuraciones del usuario
                var configuraciones = await _context.ConfiguracionWidgets
                    .Where(c => c.UsuarioId == usuarioId)
                    .ToDictionaryAsync(c => c.WidgetId, c => c);

                var resultado = widgets.Select(w => new {
                    Id = w.Id,
                    Nombre = w.Nombre,
                    Descripcion = w.Descripcion,
                    EsVisible = !configuraciones.TryGetValue(w.Id, out var config) || config.EsVisible
                }).ToList();

                return Json(resultado);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener widgets: {ex.Message}");
            }
        }
    }
}