using Newtonsoft.Json;

namespace MDashboard.Models.ApiModels
{
    public class JokeApiResponse
    {
        [JsonProperty("error")]
        public bool Error { get; set; }

        [JsonProperty("category")]
        public string Category { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("setup")]
        public string Setup { get; set; }

        [JsonProperty("delivery")]
        public string Delivery { get; set; } 

        [JsonProperty("joke")]
        public string Joke { get; set; } 

        [JsonProperty("flags")]
        public Flags Flags { get; set; }

        [JsonProperty("safe")]
        public bool Safe { get; set; }

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("lang")]
        public string Lang { get; set; }
    }

    public class Flags
    {
        [JsonProperty("nsfw")]
        public bool Nsfw { get; set; }

        [JsonProperty("religious")]
        public bool Religious { get; set; }

        [JsonProperty("political")]
        public bool Political { get; set; }

        [JsonProperty("racist")]
        public bool Racist { get; set; }

        [JsonProperty("sexist")]
        public bool Sexist { get; set; }

        [JsonProperty("explicit")]
        public bool Explicit { get; set; }
    }
        public class JokeModel
        {
            public string Categoria { get; set; }
            public string Chiste { get; set; }
            public string Tipo { get; set; }
            public bool Seguro { get; set; }
            public string Setup { get; set; }
            public string Delivery { get; set; }
            public string Idioma { get; set; }
        }
}


