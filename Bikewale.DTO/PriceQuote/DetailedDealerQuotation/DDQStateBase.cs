using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.DTO.PriceQuote.DetailedDealerQuotation
{
    /// <summary>
    /// Detailed Dealer Quotation State base
    /// Author  :   Sumit Kate
    /// Date    :   24 Aug 2015
    /// </summary>
    public class DDQStateBase
    {
        [JsonProperty("stateId")]
        public uint StateId { get; set; }

        [JsonProperty("stateName")]
        public string StateName { get; set; }
    }
}
