using Newtonsoft.Json;
using System;

namespace Bikewale.DTO.AutoBiz
{
    /// <summary>
    /// Written by Ashwini Todkar on 29 Oct 2014
    /// Modified by :   Sumit Kate on 11 Mar 2016
    /// Description :   Added MinDownPayment,MaxDownPayment,
    ///                 MinTenure,MaxTenure,MinRateOfInterest,
    ///                 MaxRateOfInterest,ProcessingFee
    /// Modified By : Ashwini Dhamankar on March 14,2016 (added EMIAmount)
    /// Modified by :   Sangram Nandkhile on 15 Mar 2016
    /// Description :   Added MinLoanToValue and MaxLoanToValue    
    /// </summary>    
    public class EMIDTO
    {
        [JsonProperty("tenure")]
        public UInt16 Tenure { get; set; }

        [JsonProperty("loanToValue")]
        public UInt16 LoanToValue { get; set; }

        [JsonProperty("rateOfInterest")]
        public float RateOfInterest { get; set; }

        [JsonProperty("loanProvider")]
        public string LoanProvider { get; set; }

        [JsonProperty("minDownPayment")]
        public float MinDownPayment { get; set; }

        [JsonProperty("maxDownPayment")]
        public float MaxDownPayment { get; set; }

        [JsonProperty("minTenure")]
        public UInt16 MinTenure { get; set; }

        [JsonProperty("maxTenure")]
        public UInt16 MaxTenure { get; set; }

        [JsonProperty("minRateOfInterest")]
        public float MinRateOfInterest { get; set; }

        [JsonProperty("maxRateOfInterest")]
        public float MaxRateOfInterest { get; set; }

        [JsonProperty("minLoanToValue")]
        public float MinLoanToValue { get; set; }

        [JsonProperty("maxLoanToValue")]
        public float MaxLoanToValue { get; set; }

        [JsonProperty("processingFee")]
        public float ProcessingFee { get; set; }

        [JsonProperty("id")]
        public UInt32 Id { get; set; }
        [JsonProperty("emiAmount")]
        public UInt32 EMIAmount { get; set; }
    }
}
