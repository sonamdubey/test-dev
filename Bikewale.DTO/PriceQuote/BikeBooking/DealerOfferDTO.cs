using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.DTO.PriceQuote.BikeBooking
{
    public class DealerOfferDTO
    {
        [JsonProperty("id")]
        public uint Id { get; set; }
        [JsonProperty("text")]
        public string Text { get; set; }
        [JsonProperty("value")]
        public uint Value { get; set; }
        [JsonProperty("type")]
        public string Type { get; set; }
    }
}
