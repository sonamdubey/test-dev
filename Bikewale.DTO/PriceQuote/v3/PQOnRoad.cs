﻿
using Newtonsoft.Json;
using System.Collections.Generic;
namespace Bikewale.DTO.PriceQuote.v3
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

        [JsonProperty("versions")]
        public IEnumerable<Bikewale.DTO.Version.VersionBase> Versions { get; set; }
    }
}

