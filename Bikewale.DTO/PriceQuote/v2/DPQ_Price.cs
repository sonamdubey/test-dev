using Newtonsoft.Json;
using System;

namespace Bikewale.DTO.PriceQuote.v2
{
    public class DPQ_Price
    {
        [JsonProperty("categoryName")]
        public string CategoryName { get; set; }
        [JsonProperty("price")]
        public UInt32 Price { get; set; }
    }
}
