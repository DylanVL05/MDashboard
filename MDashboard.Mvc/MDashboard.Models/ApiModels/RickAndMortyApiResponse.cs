using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MDashboard.Models.ApiModels
{
    public class RickAndMortyApiResponse
    {
        [JsonProperty("info")]
        public Info Info { get; set; }

        [JsonProperty("results")]
        public List<Character> Results { get; set; }
    }

    public class Info
    {
        [JsonProperty("count")]
        public int Count { get; set; }

        [JsonProperty("pages")]
        public int Pages { get; set; }

        [JsonProperty("next")]
        public string Next { get; set; }

        [JsonProperty("prev")]
        public string Prev { get; set; }
    }

    public class Character
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("species")]
        public string Species { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("gender")]
        public string Gender { get; set; }

        [JsonProperty("origin")]
        public Origin Origin { get; set; }

        [JsonProperty("location")]
        public Location Location { get; set; }

        [JsonProperty("image")]
        public string Image { get; set; }

        [JsonProperty("episode")]
        public List<string> Episode { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("created")]
        public DateTime Created { get; set; }
    }

    public class Origin
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }
    }

    public class Location
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }
    }
}

