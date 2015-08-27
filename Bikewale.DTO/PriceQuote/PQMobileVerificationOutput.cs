using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.DTO.PriceQuote
{
    /// <summary>
    /// Price Quote Mobile verification output entity
    /// Author  :   Sumit Kate
    /// Date    :   21 Aug 2015
    /// </summary>
    public class PQMobileVerificationOutput
    {
        [JsonProperty("isSuccess")]
        public bool IsSuccess { get; set; }
    }
}
