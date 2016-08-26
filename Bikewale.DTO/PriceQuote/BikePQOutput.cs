using Bikewale.DTO.PriceQuote.Area.v2;
using Bikewale.DTO.PriceQuote.City.v2;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel;

namespace Bikewale.DTO.PriceQuote
{
    /// <summary>
    /// 
    /// </summary>
    public class BikePQOutput
    {
        [JsonProperty("priceQuote")]
        public Bikewale.DTO.PriceQuote.v2.PQOutput PriceQuote { get; set; }

        [JsonProperty("pqCities")]
        public IEnumerable<PQCityBase> PQCitites { get; set; }

        [JsonProperty("pqAreas")]
        public IEnumerable<PQAreaBase> PQAreas { get; set; }

        [JsonProperty("action"), DefaultValue(true)] //true  means redirect, false = reload
        public bool Action { get; set; }

        [JsonProperty("qStr")]
        public string QueryString { get; set; }
    }
}
