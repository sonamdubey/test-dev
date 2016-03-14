using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Entities.BikeBooking
{
    /// <summary>
    /// Written by Ashwini Todkar on 29 Oct 2014
    /// </summary>    
    public class EMI
    {        
        [JsonProperty("tenure")]
        public UInt16 Tenure { get; set; }

        [JsonProperty("loanToValue")]
        public UInt16 LoanToValue { get; set; }

        [JsonProperty("rateOfInterest")]
        public float RateOfInterest { get; set; }

        [JsonProperty("loanProvider")]
        public string LoanProvider { get; set; }
    }

    /// <summary>
    /// Created By: Sangram Nandkhile
    /// Created on: 11 march 2016
    /// </summary>
    public class EmiLoanAmount
    {
        [JsonProperty("id")]
        public UInt32 Id { get; set; }

        [JsonProperty("minTenure")]
        public UInt16 MinTenure { get; set; }

        [JsonProperty("maxTenure")]
        public UInt16 MaxTenure { get; set; }

        [JsonProperty("minDownPayment")]
        public float MinDownPayment { get; set; }
        
        [JsonProperty("maxDownPayment")]
        public float MaxDownPayment { get; set; }

        [JsonProperty("minLoanToValue")]
        public UInt32 MinLoanToValue { get; set; }

        [JsonProperty("maxLoanToValue")]
        public UInt32 MaxLoanToValue { get; set; }

        [JsonProperty("minRateOfInterest")]
        public float MinRateOfInterest { get; set; }

        [JsonProperty("maxRateOfInterest")]
        public float MaxRateOfInterest { get; set; }

        [JsonProperty("loanProvider")]
        public string LoanProvider { get; set; }

        [JsonProperty("processingFee")]
        public float ProcessingFee { get; set; }
    }

}
