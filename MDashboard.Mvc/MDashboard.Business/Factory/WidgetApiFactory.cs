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

        public WidgetApiFactory(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public IWidgetApiClient CrearCliente(Widget widget)
        {
            if (widget == null || string.IsNullOrEmpty(widget.UrlApi))
                throw new ArgumentException("Widget inválido o URL de API faltante");

            if (widget.UrlApi.Contains("openweathermap.org"))
            {
                return new OpenWeatherApiClient(_httpClient, widget.UrlApi, widget.ApiKey ?? "");
            }
            else if (widget.UrlApi.Contains("exchangerate-api.com"))
            {
                return new ExchangeRateApiClient(_httpClient, widget);
            }
            else if (widget.UrlApi.Contains("newsapi.org"))
            {
                return new NewsApiClient(_httpClient, widget);
            }

            return new GenericApiClient(_httpClient, widget.UrlApi, widget.ApiKey);
        }
    }
}




