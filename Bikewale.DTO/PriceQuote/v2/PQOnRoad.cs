
using Newtonsoft.Json;
using System.Collections.Generic;
namespace Bikewale.DTO.PriceQuote.v2
{
    public class PQOnRoad
    {
        [JsonProperty("priceQuote")]
        public PQOutput PriceQuote { get; set; }
        [JsonProperty("dealers")]
        public List<DPQDealerBase> SecondaryDealers { get; set; }

        public IEnumerable<Bikewale.DTO.Version.VersionBase> version { get; set; }
    }
}
