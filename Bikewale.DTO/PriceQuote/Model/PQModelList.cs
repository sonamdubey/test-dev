using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
