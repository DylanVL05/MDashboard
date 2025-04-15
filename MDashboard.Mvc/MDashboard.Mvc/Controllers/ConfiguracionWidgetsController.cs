using MDashboard.Business;
using MDashboard.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace MDashboard.Controllers
{
    public class ConfiguracionWidgetsController : Controller
    {
        private readonly IConfiguracionWidgetsBusiness _configBusiness;

        public ConfiguracionWidgetsController(IConfiguracionWidgetsBusiness configBusiness)
        {
            _configBusiness = configBusiness;
        }

        // GET: ConfiguracionWidgets
        public async Task<IActionResult> Index()
        {
            var configuraciones = await _configBusiness.GetAllConfiguracionesAsync();
            return View(configuraciones);
        }

        // Acción GET para mostrar la vista Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ConfiguracionWidgets/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ConfiguracionWidget configuracion)
        {
            await _configBusiness.SaveConfiguracionAsync(configuracion);
            return RedirectToAction(nameof(Index));
        }




        // GET: ConfiguracionWidgets/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var configuracion = await _configBusiness.GetConfiguracionByIdAsync(id);
            if (configuracion == null)
            {
                return NotFound();
            }
            return View(configuracion);
        }

        // POST: ConfiguracionWidgets/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ConfiguracionWidget configuracion)
        {
            await _configBusiness.UpdateConfiguracionAsync(id, configuracion);
            return RedirectToAction(nameof(Index));
        }


        // GET: ConfiguracionWidgets/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var configuracion = await _configBusiness.GetConfiguracionByIdAsync(id);
            if (configuracion == null)
            {
                return NotFound();
            }
            return View(configuracion);
        }

        // POST: ConfiguracionWidgets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _configBusiness.DeleteConfiguracionAsync(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                // En caso de error se puede redireccionar a una vista de error o mostrar el mensaje
                return BadRequest(ex.Message);
            }
        }
    }
}
