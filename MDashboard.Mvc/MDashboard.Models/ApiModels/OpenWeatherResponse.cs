using System;
using System.Collections.Generic;

namespace MDashboard.Models.ApiModels
{
    public class OpenWeatherResponse
    {
        public double Lat { get; set; } // Latitud
        public double Lon { get; set; } // Longitud
        public string Timezone { get; set; } // Zona horaria
        public int Timezone_Offset { get; set; } // Desplazamiento horario en segundos
        public List<WeatherData> Data { get; set; } // Lista de datos del clima
    }

    public class WeatherData
    {
        public int Dt { get; set; } // Marca de tiempo en formato UNIX
        public int Sunrise { get; set; } // Hora de salida del sol
        public int Sunset { get; set; } // Hora de puesta del sol
        public double Temp { get; set; } // Temperatura
        public double Feels_Like { get; set; } // Sensación térmica
        public int Pressure { get; set; } // Presión
        public int Humidity { get; set; } // Humedad
        public double Dew_Point { get; set; } // Punto de rocío
        public double Uvi { get; set; } // Índice UV
        public int Clouds { get; set; } // Nubosidad
        public int Visibility { get; set; } // Visibilidad
        public double Wind_Speed { get; set; } // Velocidad del viento
        public int Wind_Deg { get; set; } // Dirección del viento
        public List<WeatherDescription> Weather { get; set; } // Lista de descripciones del clima
    }

    public class WeatherDescription
    {
        public int Id { get; set; } // ID de la condición del clima
        public string Main { get; set; } // Grupo principal del clima (Clear, Rain, etc.)
        public string Description { get; set; } // Descripción del clima (cielo despejado, etc.)
        public string Icon { get; set; } // Icono del clima
    }
}
