using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.DTO.PriceQuote
{
    /// <summary>
    /// Author      :   Sumit Kate
    /// Created     :   16 Oct 2015
    /// Description :   PQ Update output DTO
    /// </summary>
    public class PQUpdateOutput
    {
        [JsonProperty("isUpdated")]
        public bool IsUpdated { get; set; }
    }
}
