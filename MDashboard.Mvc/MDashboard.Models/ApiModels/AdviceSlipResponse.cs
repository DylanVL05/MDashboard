using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System;

namespace MDashboard.Models.ApiModels
{
    public class AdviceSlipResponse
    {
        [JsonProperty("slip")]
        public Slip Slip { get; set; }
    }

    public class Slip
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("advice")]
        public string Advice { get; set; }
    }
}