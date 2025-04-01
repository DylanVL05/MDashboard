using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDashboard.Models.ApiModels
{
    public class MeowFactsAPIResponse
    {
        [JsonProperty("data")]
        public List<string> Data { get; set; } 
    }
}
