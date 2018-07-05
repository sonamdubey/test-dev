using Bikewale.DTO.Campaign;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Bikewale.DTO.PriceQuote.v5
{
    /// <summary>
    /// Created By  : Pratibha Verma on 19 June 2018
    /// Description : New PQOnRoad version for  api/v5/onroadprice
    /// </summary>
    public class PQOnRoad
    {
        [JsonProperty("priceQuote")]
        public Bikewale.DTO.PriceQuote.v3.PQOutput PriceQuote { get; set; }

        [JsonProperty("dealers")]
        public List<Bikewale.DTO.PriceQuote.v3.DPQDealerBase> SecondaryDealers { get; set; }

        [JsonProperty("version")]
        public IEnumerable<Bikewale.DTO.Version.VersionBase> Versions { get; set; }

        [JsonProperty("campaign", NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Ignore)]
        public CampaignBaseDto Campaign { get; set; }

    }
}
