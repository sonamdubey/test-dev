using Bikewale.DTO.PriceQuote.Area;
using Newtonsoft.Json;
using System;

namespace Bikewale.DTO.DealerLocator
{
    /// <summary>
    /// Created By : Lucky Rathore on 21 March 2016
    /// Description : for Dealer Details. 
    /// </summary>
    public class DealerDetail : DealerBase
    {
        [JsonProperty("Area")]
        public PQAreaBase Area { get; set; }

        [JsonProperty("cityName")]
        public UInt16 DealerType { get; set; }

        [JsonProperty("city")]
        public string City { get; set; }

        [JsonProperty("email")]
        public string EMail { get; set; }

        [JsonProperty("address")]
        public string Address { get; set; }
        
        [JsonProperty("workingHours")]
        public string WorkingHours { get; set; }
    }
}
