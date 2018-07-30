using Newtonsoft.Json;
using System.Collections.Generic;

namespace Bikewale.DTO.PriceQuote.Area
{
    /// <summary>
    /// Price Quote Area list
    /// Author  :   Sumit Kate
    /// Date    :   20 Aug 2015
    /// </summary>
    public class PQAreaList
    {
        [JsonProperty("areas")]
        public IEnumerable<PQAreaBase> Areas { get; set; }
    }
}
