
using Newtonsoft.Json;
using System.Collections.Generic;
namespace Bikewale.DTO.PriceQuote.v2
{
    /// <summary>
    /// Created By : Sumit Kate
    /// Created on : 3rd June 2016
    /// Description : New PQOnRoad version for api/dealerversionprices and api/v2/onroadprice
    /// </summary>
    public class PQOnRoad
    {
        [JsonProperty("priceQuote")]
        public PQOutput PriceQuote { get; set; }
        [JsonProperty("dealers")]
        public List<DPQDealerBase> SecondaryDealers { get; set; }

        public IEnumerable<Bikewale.DTO.Version.VersionBase> version { get; set; }
    }
}
