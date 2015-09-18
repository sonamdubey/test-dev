using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.DTO.PriceQuote.DetailedDealerQuotation
{
    /// <summary>
    /// Detailed dealer quotation EMI Entity
    /// Author  :   Sumit Kate
    /// Date    :   24 Aug 2015
    /// </summary>
    public class DDQEMI
    {
        [JsonProperty("id")]
        public UInt32 Id { get; set; }

        [JsonProperty("tenure")]
        public UInt16 Tenure { get; set; }

        [JsonProperty("loanToValue")]
        public UInt16 LoanToValue { get; set; }

        [JsonProperty("rateOfInterest")]
        public float RateOfInterest { get; set; }

        [JsonProperty("loanProvider")]
        public string LoanProvider { get; set; }
    }
}
