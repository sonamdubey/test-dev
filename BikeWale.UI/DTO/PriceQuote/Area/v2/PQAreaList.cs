using Newtonsoft.Json;
using System.Collections.Generic;

namespace Bikewale.DTO.PriceQuote.Area.v2
{
    /// <summary>
    /// Price Quote Area list new version
    /// Author  :   Sushil Kumar
    /// Date    :   12th Feb 2016
    /// 
    /// </summary>
    public class PQAreaList
    {
        [JsonProperty("areas")]
        public IEnumerable<PQAreaBase> Areas { get; set; }
    }
}
