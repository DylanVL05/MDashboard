using MDashboard.Repository;
using MDashboard.Models.ApiModels;
using MDashboard.Data.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MDashboard.Business.Factory;
using MDashboard.Models;
using System;

namespace MDashboard.Business.Services
{
    public class WidgetService
    {
        private readonly IWidgetRepository _widgetRepository;
        private readonly WidgetApiFactory _apiFactory;

        public WidgetService(IWidgetRepository widgetRepository, WidgetApiFactory apiFactory)
        {
            _widgetRepository = widgetRepository;
            _apiFactory = apiFactory;
        }

        public async Task<Dictionary<string, object>> ObtenerDatosDeWidgetsAsync()
        {
            var widgets = await _widgetRepository.ObtenerWidgetsActivosAsync();
            var resultados = new Dictionary<string, object>();

            await Parallel.ForEachAsync(
                widgets.Select(widget => new
                {
                    client = _apiFactory.CrearCliente(widget),
                    widget
                }), async (x, _) =>
                {
                    try
                    {
                        // Obtener los datos desde la API correspondiente
                        var result = await x.client.ObtenerDatosAsync(x.widget.Nombre);

                        if (result.Value != null)
                        {
                            Console.WriteLine($"Datos obtenidos para {x.widget.Nombre}: {result.Value}");

                            // Procesamiento para OpenWeather API
                            if (x.widget.UrlApi.Contains("openweathermap.org"))
                            {
                                ProcesarOpenWeatherResponse(x.widget.Nombre, result.Value.ToString(), resultados);
                            }
                            // Procesamiento para ExchangeRate API
                            else if (x.widget.UrlApi.Contains("exchangerate-api.com"))
                            {
                                ProcesarExchangeRateResponse(x.widget.Nombre, result.Value.ToString(), resultados);
                            }

                            else if (x.widget.UrlApi.Contains("newsapi.org"))
                            {
                                ProcesarNewsApiResponse(x.widget.Nombre, result.Value.ToString(), resultados);
                            }
                            // Caso genérico para otras APIs
                            else
                            {
                                resultados.Add(x.widget.Nombre, result.Value);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error procesando widget {x.widget.Nombre}: {ex.Message}");
                    }
                });

            return resultados;
        }

        private void ProcesarOpenWeatherResponse(string widgetNombre, string jsonResponse, Dictionary<string, object> resultados)
        {
            try
            {
                var weatherResponse = JsonConvert.DeserializeObject<ClimaResponse>(jsonResponse);

                if (weatherResponse != null)
                {
                    var weatherInfo = new MainInfo
                    {
                        Temp = weatherResponse.Main?.Temp ?? 0,
                        Humidity = weatherResponse.Main?.Humidity ?? 0,
                        Pressure = weatherResponse.Main?.Pressure ?? 0
                    };

                    resultados.Add(widgetNombre, weatherInfo);
                }
                else
                {
                    Console.WriteLine($"Error: No se pudo deserializar el JSON para {widgetNombre}");
                }
            }
            catch (JsonSerializationException ex)
            {
                Console.WriteLine($"Error de deserialización para {widgetNombre}: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error inesperado para {widgetNombre}: {ex.Message}");
            }
        }

        private void ProcesarExchangeRateResponse(string widgetNombre, string jsonResponse, Dictionary<string, object> resultados)
        {
            try
            {
                var exchangeResponse = JsonConvert.DeserializeObject<ExchangeRateResponse>(jsonResponse);

                if (exchangeResponse != null)
                {
                    // Seleccionamos algunas monedas importantes para mostrar
                    var featuredRates = new Dictionary<string, decimal>
            {
                { "EUR", exchangeResponse.ConversionRates["EUR"] },
                { "GBP", exchangeResponse.ConversionRates["GBP"] },
                { "JPY", exchangeResponse.ConversionRates["JPY"] },
                { "MXN", exchangeResponse.ConversionRates["MXN"] }
            };

                    var exchangeInfo = new ExchangeRateInfo
                    {
                        BaseCurrency = exchangeResponse.BaseCode,
                        LastUpdate = exchangeResponse.TimeLastUpdateUtc,
                        FeaturedRates = featuredRates,
                        AllRates = exchangeResponse.ConversionRates
                    };

                    resultados.Add(widgetNombre, exchangeInfo);
                }
                else
                {
                    Console.WriteLine($"Error: No se pudo deserializar el JSON para {widgetNombre}");
                }
            }
            catch (JsonSerializationException ex)
            {
                Console.WriteLine($"Error de deserialización para {widgetNombre}: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error inesperado para {widgetNombre}: {ex.Message}");
            }






        }

        private void ProcesarNewsApiResponse(string widgetNombre, string jsonResponse, Dictionary<string, object> resultados)
        {
            try
            {
                var newsResponse = JsonConvert.DeserializeObject<NewsApiResponse>(jsonResponse);

                if (newsResponse != null && newsResponse.Status == "ok")
                {
                    var newsInfo = new
                    {
                        TotalNoticias = newsResponse.TotalResults,
                        UltimasNoticias = newsResponse.Articles.Take(3).Select(a => new
                        {
                            Fuente = a.Source?.Name,
                            Titulo = a.Title,
                            Imagen = a.UrlToImage,
                            Fecha = a.PublishedAt.ToString("g"),
                            Resumen = a.Description
                        }).ToList()
                    };

                    resultados.Add(widgetNombre, newsInfo);
                }
                else
                {
                    Console.WriteLine($"Error: Respuesta inválida de NewsAPI para {widgetNombre}");
                }
            }
            catch (JsonSerializationException ex)
            {
                Console.WriteLine($"Error de deserialización para {widgetNombre}: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error inesperado para {widgetNombre}: {ex.Message}");
            }
        }




        public async Task AgregarWidgetAsync(Widget widget)
        {
            await _widgetRepository.AgregarWidgetAsync(widget);
        }
    }
}