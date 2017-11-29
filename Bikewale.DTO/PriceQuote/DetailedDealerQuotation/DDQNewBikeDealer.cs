using Newtonsoft.Json;
using System;

namespace Bikewale.DTO.PriceQuote.DetailedDealerQuotation
{
    /// <summary>
    /// Detailed dealer quotation new bike dealer entity
    /// Author  :   Sumit Kate
    /// Date    :   24 Aug 2015
    /// </summary>
    public class DDQNewBikeDealer
    {
        [JsonProperty("dealerId")]
        public UInt32 DealerId { get; set; }

        [JsonProperty("dealerName")]
        public string Name { get; set; }

        [JsonProperty("emailId")]
        public string EmailId { get; set; }

        [JsonProperty("mobileNo")]
        public string MobileNo { get; set; }

        [JsonProperty("phoneNo")]
        public string PhoneNo { get; set; }

        [JsonProperty("organization")]
        public string Organization { get; set; }

        [JsonProperty("website")]
        public string Website { get; set; }

        [JsonProperty("workingTime")]
        public string WorkingTime { get; set; }

        [JsonProperty("address")]
        public string Address { get; set; }

        [JsonProperty("state")]
        public DDQStateBase objState { get; set; }

        [JsonProperty("city")]
        public DDQCityBase objCity { get; set; }

        [JsonProperty("area")]
        public DDQAreaBase objArea { get; set; }
    }
}
