using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.DTOs.Dealer
{
    public class DealerDTO
    {
        [JsonProperty("id")]
        public int DealerId { get; set; }
        [JsonProperty("name")]
        public string DealerName { get; set; }
        [JsonProperty("area")]
        public string DealerArea { get; set; }
        [JsonProperty("distance")]
        public int Distance { get; set; }
    }
}
