using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDashboard.Models.ApiModels
{
    public class QuotesApiResponse
    {
        [JsonProperty("uuid")]
        public string Uuid { get; set; }

        [JsonProperty("quote")]
        public string Quote { get; set; }

        [JsonProperty("author")]
        public string Author { get; set; }

        [JsonProperty("id")]
        public int Id { get; set; }
    }

        public class QuoteModel
        {
            public string Quote { get; set; }
            public string Author { get; set; }
            public int Id { get; set; }
        }
    }

