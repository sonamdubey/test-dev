using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.DTO.PriceQuote.Make
{
    /// <summary>
    /// Price Quote make base
    /// Author  :   Sumit Kate
    /// Date    :   19 Aug 2015
    /// </summary>
    public class PQMakeBase
    {
        [JsonProperty("makeId")]
        public int MakeId { get; set; }
        [JsonProperty("makeName")]
        public string MakeName { get; set; }
        [JsonProperty("maskingName")]
        public string MaskingName { get; set; }
    }
}
