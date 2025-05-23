﻿using MDashboard.Models;
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
using MDashboard.Helpers;
using MDashboard.Business.Widgets;


namespace MDashboard.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly IWidgetRepository _widgetRepository;
        private readonly IWidgetBusiness _widgetBusiness;
        private readonly WidgetService _widgetService;
        private readonly MediaDashboardContext _context;

        public DashboardController(IWidgetRepository widgetRepository, WidgetService widgetService, WidgetBusiness widgetBusiness, MediaDashboardContext context)
        {
            _widgetRepository = widgetRepository;
            _widgetBusiness = widgetBusiness;
            _widgetService = widgetService;
            _context = context;
        }

        // Acción para mostrar el dashboard con los widgets existentes
        [HttpGet]
        public async Task<IActionResult> Index(bool partial = false)
        {
            try
            {
                int usuarioId = ObtenerUsuarioActual();

                var todosLosWidgets = await _widgetRepository.ObtenerWidgetsActivosAsync();

                var configuracionesUsuario = await _context.ConfiguracionWidgets
                    .Where(c => c.UsuarioId == usuarioId)
                    .ToDictionaryAsync(c => c.WidgetId, c => c);

                var widgetsOcultos = new List<Widget>();
                var favoritos = new List<Widget>();
                var noFavoritos = new List<Widget>();

                foreach (var widget in todosLosWidgets)
                {
                    if (configuracionesUsuario.TryGetValue(widget.Id, out var config))
                    {
                        if (!config.EsVisible)
                        {
                            widgetsOcultos.Add(widget); 
                        }
                        else
                        {
                            if (config.EsFavorito)
                                favoritos.Add(widget);
                            else
                                noFavoritos.Add(widget);
                        }
                    }
                    else
                    {
                        noFavoritos.Add(widget);
                    }
                }

                var widgetsVisibles = favoritos.Concat(noFavoritos).ToList();

                var dynamicData = await _widgetService.ObtenerDatosDeWidgetsAsync();
                ViewBag.DynamicData = dynamicData ?? new Dictionary<string, object>();
                ViewBag.WidgetsOcultos = widgetsOcultos;
                ViewBag.Configuraciones = configuracionesUsuario;

                if (partial && Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    var html = await this.RenderViewAsync("Index", widgetsVisibles, true);
                    return Json(new { html });
                }

                return View(widgetsVisibles);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al cargar el dashboard: {ex.Message}");
                return StatusCode(500, $"Error al cargar el dashboard: {ex.Message}");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleFavorite(int widgetId)
        {
            try
            {
                int usuarioId = ObtenerUsuarioActual();

                var configuracion = await _context.ConfiguracionWidgets
                    .FirstOrDefaultAsync(c => c.UsuarioId == usuarioId && c.WidgetId == widgetId);

                if (configuracion != null)
                {
                    configuracion.EsFavorito = !configuracion.EsFavorito;
                    _context.ConfiguracionWidgets.Update(configuracion);
                }
                else
                {
                    configuracion = new ConfiguracionWidget
                    {
                        UsuarioId = usuarioId,
                        WidgetId = widgetId,
                        EsVisible = true, 
                        EsFavorito = true
                    };
                    _context.ConfiguracionWidgets.Add(configuracion);
                }

                await _context.SaveChangesAsync();

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al cambiar el estado de favorito: {ex.Message}");
                TempData["ErrorMessage"] = $"Error al cambiar el estado de favorito: {ex.Message}";
                return RedirectToAction("Index");
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
                    configuracion.EsVisible = esVisible;
                }
                else
                {
                    _context.ConfiguracionWidgets.Add(new ConfiguracionWidget
                    {
                        UsuarioId = usuarioId,
                        WidgetId = widgetId,
                        EsVisible = esVisible,
                        Width = 1,
                        Height = 1,
                        EsFavorito = false
                    });
                }

                _context.SaveChanges();

                return Json(new { success = true, widgetId = widgetId, esVisible = esVisible });
            }
            catch (Exception ex)
            {
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

                return PartialView("_WidgetPartial", widgets); 
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al actualizar los widgets: {ex.Message}");
            }
        }



        [HttpPost]
        public IActionResult UpdateWidget(Widget updatedWidget)
        {
            var widget = _context.Widgets.FirstOrDefault(w => w.Id == updatedWidget.Id);
            if (widget == null) return NotFound();

            widget.Nombre = updatedWidget.Nombre;
            widget.ComponentId = updatedWidget.ComponentId;
            widget.UrlApi = updatedWidget.UrlApi;

            _context.SaveChanges();
            return Ok();
        }

        public async Task<IActionResult> ConfigWidget(int id)
        {
            var widget = await _widgetBusiness.ObtenerWidgetPorIdAsync(id);

            if (widget == null)
            {
                return NotFound();
            }

            return PartialView("_ConfigWidgetPartial", widget); 
        }


        // Acción para obtener todos los widgets disponibles (visible o no)
        [HttpGet]
        public async Task<IActionResult> ObtenerTodosLosWidgets()
        {
            try
            {
                int usuarioId = ObtenerUsuarioActual();

                var widgets = await _widgetRepository.ObtenerWidgetsActivosAsync();

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
        [HttpPost]
        public async Task<IActionResult> GuardarComponentId(Widget request)
        {
            try
            {
                var widget = await _context.Widgets.FirstOrDefaultAsync(w => w.Id == request.Id);

                if (widget != null)
                {
                    widget.ComponentId = request.ComponentId;
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index"); 
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al actualizar ComponentId: {ex.Message}");
                return StatusCode(500, "Error al actualizar ComponentId");
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ActualizarDimensionesWidget(int widgetId, int width, int height)
        {
            try
            {
                int usuarioId = ObtenerUsuarioActual();

                width = Math.Clamp(width, 300, 1200);
                height = Math.Clamp(height, 300, 800);

                var configuracion = await _context.ConfiguracionWidgets
                    .FirstOrDefaultAsync(c => c.UsuarioId == usuarioId && c.WidgetId == widgetId);

                if (configuracion != null)
                {
                    configuracion.Width = width;
                    configuracion.Height = height;
                    _context.ConfiguracionWidgets.Update(configuracion);
                }
                else
                {
                    configuracion = new ConfiguracionWidget
                    {
                        UsuarioId = usuarioId,
                        WidgetId = widgetId,
                        Width = width,
                        Height = height,
                        EsVisible = true,
                        EsFavorito = false
                    };
                    await _context.ConfiguracionWidgets.AddAsync(configuracion);
                }

                await _context.SaveChangesAsync();

                return Json(new
                {
                    success = true,
                    widgetId = widgetId,
                    width = width,
                    height = height
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al actualizar dimensiones: {ex.Message}");
                return Json(new
                {
                    success = false,
                    error = "Error al actualizar dimensiones del widget"
                });
            }
        }

    }
}