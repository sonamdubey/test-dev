using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.DTO.PriceQuote.Version
{
    /// <summary>
    /// Price Quote Version list
    /// Author  :   Sumit Kate
    /// Date    :   21 Aug 2015
    /// </summary>
    public class PQVersionList
    {
        [JsonProperty("versions")]
        public IEnumerable<PQVersionBase> Versions { get; set; }
    }
}
