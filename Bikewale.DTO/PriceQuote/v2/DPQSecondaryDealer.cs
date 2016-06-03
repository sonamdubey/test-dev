using Newtonsoft.Json;
using System;

namespace Bikewale.DTO.PriceQuote.v2
{
    public class DPQDealerBase
    {
        [JsonProperty("id")]
        public UInt32 DealerId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("area")]
        public string Area { get; set; }

        [JsonProperty("maskingNumber")]
        public string MaskingNumber { get; set; }
    }
}
