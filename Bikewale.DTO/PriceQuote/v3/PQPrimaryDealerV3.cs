using Bikewale.DTO.PriceQuote.v2;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Bikewale.DTO.PriceQuote.v3
{
    /// <summary>
    /// Created By : Sumit Kate
    /// Created on : 3rd June 2016
    /// Description : New PQPrimaryDealer version for api/dealerversionprices and api/v2/onroadprice
    /// </summary>
    public class PQPrimaryDealerV3
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("area")]
        public string Area { get; set; }

        [JsonProperty("maskingNumber")]
        public string MaskingNumber { get; set; }

        [JsonProperty("offers")]
        public IEnumerable<DPQOffer> Offers { get; set; }

    }
}
