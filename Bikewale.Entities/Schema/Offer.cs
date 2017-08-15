using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Entities.Schema
{
    public class Offer
    {
        [JsonProperty("@type")]
        public string Type { get { return "Offer"; } }
        
        [JsonProperty("availability", NullValueHandling = NullValueHandling.Ignore)]
        public string Availability { get; set; }
        
        [JsonProperty("price", NullValueHandling = NullValueHandling.Ignore)]
        public double Price { get; set; }

        [JsonProperty("priceCurrency")]
        public string PriceCurrency { get { return "INR"; } }
    }
}
