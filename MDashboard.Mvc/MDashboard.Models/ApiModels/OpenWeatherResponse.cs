using System;
using System.Collections.Generic;

namespace MDashboard.Models.ApiModels
{
    public class OpenWeatherResponse
    {
        public double Lat { get; set; } // Latitud
        public double Lon { get; set; } // Longitud
        public string Timezone { get; set; } // Zona horaria
        public int Timezone_Offset { get; set; } // Desplazamiento de la zona horaria en segundos
        public CurrentWeather Current { get; set; } // Clima actual
    }

    public class CurrentWeather
    {
        public double Temp { get; set; } // Temperatura
        public int Humidity { get; set; } // Humedad (%)
        public List<WeatherDescription> Weather { get; set; } // Descripción del clima
    }

    public class WeatherDescription
    {
        public string Main { get; set; } // Categoría del clima (Rain, Snow, etc.)
        public string Description { get; set; } // Descripción detallada (lluvia ligera, nublado, etc.)
        public string Icon { get; set; } // ID del icono del clima
    }
}
