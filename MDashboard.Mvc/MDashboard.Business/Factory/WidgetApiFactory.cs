using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Net.Http;
using System;
using System.Net.Http;
using MDashboard.Models;


namespace MDashboard.Business.Factory
{
    public class WidgetApiFactory
    {
        private readonly HttpClient _httpClient;
        private readonly List<(Func<string, bool> Match, Func<Widget, IWidgetApiClient> Create)> _strategies;

        public WidgetApiFactory(HttpClient httpClient)
        {
            _httpClient = httpClient;

            _strategies = new List<(Func<string, bool>, Func<Widget, IWidgetApiClient>)>
            {
                (url => url.Contains("openweathermap.org"), w => new OpenWeatherApiClient(_httpClient, w.UrlApi, w.ApiKey ?? "")),
                (url => url.Contains("exchangerate-api.com"), w => new ExchangeRateApiClient(_httpClient, w)),
                (url => url.Contains("newsapi.org"), w => new NewsApiClient(_httpClient, w)),
                (url => url.Contains("api.nasa.gov"), w => new NasaApiClient(_httpClient, w)),
                (url => url.Contains("rickandmortyapi"), w => new RickAndMortyApiClient(_httpClient, w)),
                (url => url.Contains("v2.jokeapi.dev"), w => new JokeApiClient(_httpClient, w)),
                (url => url.Contains("meowfacts"), w => new MeowFactsAPIClient(_httpClient, w)),
                (url => url.Contains("thequotesapi.onrender.com"), w => new QuotesApiClient(_httpClient, w)),
                (url => url.Contains("adviceslip.com"), w => new AdviceSlipApiClient(_httpClient, w)),
                (url => url.Contains("dog.ceo"), w => new DogApiClient(_httpClient, w)),
                (url => url.Contains("logotypes.dev"), w => new LogotypesApiClient(_httpClient, w)),
                (url => url.Contains("api.chucknorris.io"), w => new ChuckNorrisApiClient(_httpClient, w)),
            };
        }

        public IWidgetApiClient CrearCliente(Widget widget)
        {
            if (widget == null || string.IsNullOrEmpty(widget.UrlApi))
                throw new ArgumentException("Widget inválido o URL de API faltante");

            foreach (var (match, create) in _strategies)
            {
                if (match(widget.UrlApi))
                {
                    return create(widget);
                }
            }

            return new GenericApiClient(_httpClient, widget.UrlApi, widget.ApiKey);
        }
    }
}
