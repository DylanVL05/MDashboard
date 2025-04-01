using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDashboard.Models.ApiModels
{
    public class ExchangeRateInfo
    {
        public string BaseCurrency { get; set; }
        public string LastUpdate { get; set; }
        public Dictionary<string, decimal> FeaturedRates { get; set; } 
        public Dictionary<string, decimal> AllRates { get; set; }

    }
}