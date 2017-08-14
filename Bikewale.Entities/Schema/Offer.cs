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
        public string Availability { get; set; }
        public double Price { get; set; }
        public string PriceCurrency { get { return "INR"; } }
    }
}
