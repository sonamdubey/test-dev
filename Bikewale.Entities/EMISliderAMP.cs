using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Entities
{
    public class EMISliderAMP
    {
        [JsonProperty("totalAmount")]
        public string TotalAmount { get; set; }

        [JsonProperty("formatedTotalAmount")]
        public int FormatedTotalAmount { get; set; }

        [JsonProperty("downPayment")]
        public int DownPayment { get; set; }

        [JsonProperty("formatedDownPayment")]
        public int FormatedDownPayment { get; set; }

        [JsonProperty("loanAmount")]
        public double LoanAmount { get; set; }

        [JsonProperty("formatedLoanAmount")]
        public int FormatedLoanAmount { get; set; }

        [JsonProperty("tenure")]
        public int Tenure { get; set; }

        [JsonProperty("interest")]
        public double RateOfInterest{ get; set; }

        [JsonProperty("fees")]
        public int Fees { get; set; }

        [JsonProperty("bikePrice")]
        public ulong BikePrice { get; set; }

        [JsonProperty("emi")]
        public int EMI { get; set; }
    }
}
