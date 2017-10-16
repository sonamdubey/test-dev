using Bikewale.DTO.Campaign;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Bikewale.DTO.PriceQuote.v4
{
    /// <summary>
    /// Created By : Sushil Kumar
    /// Created on : 17th June 2016
    /// Description : New PQOnRoad version for  api/v3/onroadprice
    /// </summary>
    public class PQOnRoad
    {
        [JsonProperty("priceQuote")]
        public Bikewale.DTO.PriceQuote.v2.PQOutput PriceQuote { get; set; }

        [JsonProperty("dealers")]
        public List<Bikewale.DTO.PriceQuote.v3.DPQDealerBase> SecondaryDealers { get; set; }

        [JsonProperty("version")]
        public IEnumerable<Bikewale.DTO.Version.VersionBase> Versions { get; set; }

        [JsonProperty("campaign", NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Ignore)]
        public CampaignBaseDto Campaign { get; set; }

    }
}
