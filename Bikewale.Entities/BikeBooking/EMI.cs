using Newtonsoft.Json;
using System;

namespace Bikewale.Entities.BikeBooking
{
    /// <summary>
    /// Written by Ashwini Todkar on 29 Oct 2014
    /// Modified By : Sushil Kumar on 14th March 2016
    /// Description : Modified ENtity for new subscription model
    /// </summary>
    [Serializable]
    public class EMI
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


        [JsonProperty("tenure")]
        public UInt16 Tenure { get; set; }

        [JsonProperty("loanToValue")]
        public UInt16 LoanToValue { get; set; }

        [JsonProperty("rateOfInterest")]
        public float RateOfInterest { get; set; }

        [JsonProperty("emiAmount")]
        public UInt32 EMIAmount { get; set; }

    }
}
