using Newtonsoft.Json;
using System;

namespace Bikewale.DTO.PriceQuote.v2
{
    /// <summary>
    /// Created By : Sumit Kate
    /// Created on : 3rd June 2016
    /// Description : New DPQ_Price version for api/dealerversionprices and api/v2/onroadprice
    /// </summary>
    public class DPQ_Price
    {
        [JsonProperty("categoryName")]
        public string CategoryName { get; set; }
        [JsonProperty("price")]
        public UInt32 Price { get; set; }
    }
}
