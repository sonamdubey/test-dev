using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.DTOs.Finance
{
    public class HdfcLoanParams
    {
        [JsonProperty("loanAmount")]
        public int LoanAmount { get; set; }
        [JsonProperty("tenor")]
        public int Tenor { get; set; }
        [JsonProperty("maxTenor")]
        public int MaxTenor { get; set; }
        [JsonProperty("ltv")]
        public decimal LTV { get; set; }
        [JsonProperty("roi")]
        public decimal ROI { get; set; }
        [JsonProperty("processingFees")]
        public int ProcessingFees { get; set; }
        [JsonProperty("isPermitted")]
        public bool IsPermitted { get; set; }
        [JsonProperty("exShowroomPrice")]
        public int ExShowroomPrice { get; set; }
    }
}
