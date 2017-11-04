using Newtonsoft.Json;
using System.Collections.Generic;

namespace Bikewale.DTO.PriceQuote.City.v2
{
    /// <summary>
    /// Price Quote City list
    /// Author  :   Sumit Kate
    /// Date    :   20 Aug 2015
    /// </summary>
    public class PQCityList
    {
        [JsonProperty("cities")]
        public IEnumerable<PQCityBase> Cities { get; set; }
    }
}
