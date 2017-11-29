using Newtonsoft.Json;
using System;

namespace Bikewale.DTO.PriceQuote.v2
{
    /// <summary>
    /// Created By : Sumit Kate
    /// Created on : 3rd June 2016
    /// Description : New EMI version for api/dealerversionprices and api/v2/onroadprice
    /// </summary>
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
    }
}
