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
        public string FormatedTotalAmount { get; set; }

        [JsonProperty("downPayment")]
        public string DownPayment { get; set; }

        [JsonProperty("formatedDownPayment")]
        public string FormatedDownPayment { get; set; }

        [JsonProperty("loanAmount")]
        public string LoanAmount { get; set; }

        [JsonProperty("formatedLoanAmount")]
        public string FormatedLoanAmount { get; set; }


        [JsonProperty("tenure")]
        public ushort Tenure { get; set; }

        [JsonProperty("formatedTenure")]
        public string FormatedTenure { get; set; }

        [JsonProperty("interest")]
        public double RateOfInterest{ get; set; }

        [JsonProperty("fees")]
        public int Fees { get; set; }

        [JsonProperty("bikePrice")]
        public ulong BikePrice { get; set; }

        [JsonProperty("emi")]
        public string EMI { get; set; }
    }
}
