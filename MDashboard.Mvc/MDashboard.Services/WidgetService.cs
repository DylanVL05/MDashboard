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
                            // Procesamiento para News API
                            else if (x.widget.UrlApi.Contains("newsapi.org"))
                            {
                                ProcesarNewsApiResponse(x.widget.Nombre, result.Value.ToString(), resultados);
                            }
                            // Procesamiento para Nasa API
                            else if (x.widget.UrlApi.Contains("api.nasa.gov"))
                            {
                                ProcesarNasaApiResponse(x.widget.Nombre, result.Value as NasaApodResponse, resultados);
                            }
                            else if (x.widget.UrlApi.Contains("rickandmortyapi"))
                            {
                                ProcesarRickAndMortyApiResponse(x.widget.Nombre, result.Value as RickAndMortyApiResponse, resultados);
                            }
                            else if (x.widget.UrlApi.Contains("v2.jokeapi.dev"))
                            {
                                ProcesarJokeApiResponse(x.widget.Nombre, result.Value as JokeApiResponse, resultados);
                            }
                            else if (x.widget.UrlApi.Contains("meowfacts"))
                            {
                                ProcesarMeowFactsApiResponse(x.widget.Nombre, result.Value as MeowFactsAPIResponse, resultados);
                            }
                            else if (x.widget.UrlApi.Contains("dog.ceo"))
                            {
                                ProcesarDogMeowFactsApiResponse(x.widget.Nombre, result.Value as DogApiResponse, resultados);
                            }
                            else if (x.widget.UrlApi.Contains("logotypes.dev"))
                            {
                                ProcesarLogotypeApiResponse(x.widget.Nombre, result.Value as LogotypesApiResponse, resultados);
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

        private void ProcesarNasaApiResponse(string widgetNombre, NasaApodResponse nasaResponse, Dictionary<string, object> resultados)
        {
            if (nasaResponse != null)
            {
                resultados.Add(widgetNombre, new NasaApodResponse
                {
                    Title = nasaResponse.Title,
                    Date = nasaResponse.Date,
                    Explanation = nasaResponse.Explanation,
                    MediaType = nasaResponse.MediaType,
                    Url = nasaResponse.Url
                });
            }
            else
            {
                Console.WriteLine($"Error: El modelo NasaApodResponse es nulo para {widgetNombre}");
            }
        }


        private void ProcesarRickAndMortyApiResponse(string widgetNombre, RickAndMortyApiResponse rickResponse, Dictionary<string, object> resultados)
        {
            if (rickResponse != null && rickResponse.Results != null)
            {
                resultados.Add(widgetNombre, rickResponse.Results);
            }
            else
            {
                Console.WriteLine($"Error: Respuesta inválida para {widgetNombre}");
            }
        }

        private void ProcesarJokeApiResponse(string widgetNombre, JokeApiResponse jokeResponse, Dictionary<string, object> resultados)
        {
            try
            {
                if (jokeResponse != null && !jokeResponse.Error)
                {
                    string chiste;

                    if (jokeResponse.Type == "single")
                    {
                        chiste = jokeResponse.Joke;
                    }
                    else if (jokeResponse.Type == "twopart")
                    {
                        chiste = $"{jokeResponse.Setup} {jokeResponse.Delivery}";
                    }
                    else
                    {
                        chiste = "Tipo de chiste desconocido.";
                    }

                    var jokeInfo = new
                    {
                        Categoria = jokeResponse.Category,
                        Chiste = chiste,
                        type = jokeResponse.Type,
                        Seguro = jokeResponse.Safe,
                        Setup = jokeResponse.Setup ?? "N/A",
                        Delivery = jokeResponse.Delivery ?? "N/A",
                        Idioma = jokeResponse.Lang
                    };

                    resultados.Add(widgetNombre, jokeInfo);
                }
                else
                {
                    Console.WriteLine($"Error: No se encontró un chiste válido para {widgetNombre}.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error inesperado para {widgetNombre}: {ex.Message}");
            }
        }

        private void ProcesarMeowFactsApiResponse(string widgetNombre, MeowFactsAPIResponse meowResponse, Dictionary<string, object> resultados)
        {
            if (meowResponse != null)
            {
                resultados.Add(widgetNombre, new MeowFactsAPIResponse
                {
                    Data = meowResponse.Data
                });
            }
            else
            {
                Console.WriteLine($"Error: El modelo NasaApodResponse es nulo para {widgetNombre}");
            }
        }

        private void ProcesarDogMeowFactsApiResponse(string widgetNombre, DogApiResponse dogResponse, Dictionary<string, object> resultados)
        {
            if (dogResponse != null)
            {
                resultados.Add(widgetNombre, new DogApiResponse
                {
                    Message = dogResponse.Message,
                    Status = dogResponse.Status
                });
            }
            else
            {
                Console.WriteLine($"Error: El modelo NasaApodResponse es nulo para {widgetNombre}");
            }
        }

        private void ProcesarLogotypeApiResponse(string widgetNombre, LogotypesApiResponse logotypeResponse, Dictionary<string, object> resultados)
        {
            if (logotypeResponse != null)
            {
                resultados.Add(widgetNombre, new LogotypesApiResponse
                {
                    Example_description = logotypeResponse.Example_description,
                    Example_title = logotypeResponse.Example_title,
                    Logo = logotypeResponse.Logo,
                    Name = logotypeResponse.Name,
                    Variant = logotypeResponse.Variant,
                    Version = logotypeResponse.Version
                });
            }
            else
            {
                Console.WriteLine($"Error: El modelo NasaApodResponse es nulo para {widgetNombre}");
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