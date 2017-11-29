using Newtonsoft.Json;
using System.Collections.Generic;

namespace Bikewale.DTO.PriceQuote.Make
{
    /// <summary>
    /// Price Quote Make list
    /// Author  :   Sumit Kate
    /// Date    :   20 Aug 2015
    /// </summary>
    public class PQMakeList
    {
        [JsonProperty("makes")]
        public IEnumerable<PQMakeBase> Makes { get; set; }
    }
}
