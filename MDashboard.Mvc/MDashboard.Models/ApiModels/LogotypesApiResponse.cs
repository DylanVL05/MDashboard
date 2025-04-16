using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDashboard.Models.ApiModels
{
    public class LogotypesApiResponse
    {
        [JsonProperty("example_description")]
        public string Example_description { get; set; }

        [JsonProperty("example_title")]
        public string Example_title { get; set; }

        [JsonProperty("logo")]
        public string Logo { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("variant")]
        public string Variant { get; set; }

        [JsonProperty("version")]
        public string Version { get; set; }

    }
}
