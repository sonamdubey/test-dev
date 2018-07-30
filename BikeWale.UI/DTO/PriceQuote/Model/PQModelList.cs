using Newtonsoft.Json;
using System.Collections.Generic;

namespace Bikewale.DTO.PriceQuote.Model
{
    /// <summary>
    /// Price Quote Model list
    /// Author  :   Sumit Kate
    /// Date    :   21 Aug 2015
    /// </summary>
    public class PQModelList
    {
        [JsonProperty("models")]
        public IEnumerable<PQModelBase> Models { get; set; }
    }
}
