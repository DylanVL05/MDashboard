using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDashboard.Models.ApiModels
{
    public class OpenWeatherResponse
    {
        public string Description { get; set; } // Descripción del clima (soleado, lluvioso, etc.)
        public double Temperature { get; set; } // Temperatura en grados Celsius
        public double Humidity { get; set; } // Humedad en porcentaje
        public double WindSpeed { get; set; } // Velocidad del viento en metros por segundo
                                              // Puedes añadir más propiedades si es necesario, como la presión, las condiciones de lluvia, etc.
    }
}
