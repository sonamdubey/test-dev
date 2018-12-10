using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.DTOs.PriceQuote
{
    public class PQItemDTO
    {
        [JsonProperty("key")]
        public string Key { get; set; }
        [JsonProperty("value")]
        public long Value { get; set; }
        [JsonProperty("isMetallic")]
        public bool IsMetallic { get; set; }
    }
}
