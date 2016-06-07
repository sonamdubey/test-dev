using Newtonsoft.Json;
using System;

namespace Bikewale.DTO.PriceQuote.v2
{
    /// <summary>
    /// Created By : Sumit Kate
    /// Created on : 3rd June 2016
    /// Description : New DPQDealerBase version for api/dealerversionprices and api/v2/onroadprice
    /// </summary>
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
