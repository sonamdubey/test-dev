using Newtonsoft.Json;
using System;

namespace Bikewale.DTO.PriceQuote.DetailedDealerQuotation
{
    /// <summary>
    /// Detailed dealer quotation EMI Entity
    /// Author  :   Sumit Kate
    /// Date    :   24 Aug 2015
    /// Modified By : Sushil Kumar on 14th March 2016
    /// Description : Modified DTO for new subscription model
    /// </summary>
    public class DDQEMI
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
