using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Bikewale.DTO.PriceQuote.v3
{
    /// <summary>
    /// Created By : Sushil Kumar
    /// Created on : 17th June 2016
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

        [JsonProperty("isPremium")]
        public bool IsPremiumDealer { get; set; }

        [JsonProperty("version")]
        public List<VersionPriceBase> Versions { get; set; }
    }
}
