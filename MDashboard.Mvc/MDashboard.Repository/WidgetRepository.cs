using MDashboard.Data.Models;
using MDashboard.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
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

        Task AgregarWidgetAsync(Widget widget);


    }
    public class WidgetRepository : IWidgetRepository
    {
        private readonly MediaDashboardContext _context;
        private readonly IHostEnvironment _hostEnvironment;

        public WidgetRepository(MediaDashboardContext context, IHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
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


        public async Task AgregarWidgetAsync(Widget widget)
        {
            _context.Widgets.Add(widget);
            await _context.SaveChangesAsync();
        }




        //---------------------------------Posiblemente mejor manejar aparte----------------------------------------------------------------






        public async Task<string> VisualizarWidgets(int widgetId, object widgetData)
        {
            // Obtener el widget desde la base de datos
            var widget = await _context.Widgets.Include(w => w.Component).FirstOrDefaultAsync(w => w.Id == widgetId);

            if (widget == null)
            {
                return "Widget no encontrado";
            }

            // Según el tipo de componente, determinar el template a usar
            var componentType = widget.Component.Tipo;  // Tipo de componente, por ejemplo 'chart_v1', 'table_1', 'card_v1'

            // Obtener los datos del widget (esto puede involucrar llamadas a APIs externas) 
            // El widgetData ya ha sido obtenido en el servicio y es pasado a este método

            // Obtener la ruta de la carpeta Templates usando IHostEnvironment
            string templatesFolder = Path.Combine(_hostEnvironment.ContentRootPath, "Templates");

            // Determinar el path del template según el tipo de componente
            string templatePath = string.Empty;

            switch (componentType)
            {
                case "chart_v1":
                    templatePath = Path.Combine(templatesFolder, "chart_v1.html");
                    break;
                case "table_1":
                    templatePath = Path.Combine(templatesFolder, "table_1.html");
                    break;
                case "card_v1":
                    templatePath = Path.Combine(templatesFolder, "card_v1.html");
                    break;
                default:
                    return "Tipo de componente no soportado";
            }

            // Leer el template y renderizarlo con los datos del widget
            var template = await File.ReadAllTextAsync(templatePath);

            // Aquí, renderizamos el template con los datos del widget. Esto depende de cómo quieras estructurarlo.
            // Podrías usar un motor de plantillas como Razor o simplemente reemplazar placeholders.

            var renderedWidget = RenderizarTemplate(template, widgetData);

            return renderedWidget; // Devuelve el HTML renderizado
        }




        // Método auxiliar para obtener el tipo del componente desde la base de datos (según su ID)
        private async Task<string> ObtenerTemplateHtmlAsync(string templateName)
        {
            var path = Path.Combine(_hostEnvironment.ContentRootPath, "Templates", "Widgets", templateName);

            if (!File.Exists(path))
            {
                return "<p>Template no encontrado.</p>";
            }

            return await File.ReadAllTextAsync(path);
        }


        // Método para renderizar el template con los datos obtenidos
        private string RenderizarTemplate(string template, object widgetData)
        {
            // Aquí puedes tener una lógica de reemplazo de placeholders en el template
            // por ejemplo, si el template tiene placeholders como {{title}}, {{data}}, etc.

            // Supón que widgetData es un diccionario o un objeto que contiene los datos para reemplazar
            var json = JsonConvert.SerializeObject(widgetData);
            var data = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);

            // Reemplazar los placeholders en el template con los valores del widgetData
            foreach (var item in data)
            {
                var placeholder = $"{{{{ {item.Key} }}}}";
                template = template.Replace(placeholder, item.Value);
            }

            return template;
        }









    }





}


