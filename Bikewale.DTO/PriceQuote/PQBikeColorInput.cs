using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.DTO.PriceQuote
{
    /// <summary>
    /// Price Quote Bike color input entity
    /// Author  :   Sumit Kate
    /// Date    :   21 Aug 2015
    /// </summary>
    public class PQBikeColorInput
    {
        [JsonProperty("pqId")]
        public uint PQId { get; set; }
        [JsonProperty("colorId")]
        public uint ColorId { get; set; }
    }
}
