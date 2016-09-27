using Newtonsoft.Json;

namespace Bikewale.DTO.UsedBikes
{
    /// <summary>
    /// Created by  :   Sumit Kate on 23 Sep 2016
    /// Description :   Purchase Inquiry Result DTO
    /// </summary>
    public class PurchaseInquiryResultDTO
    {
        [JsonProperty("inquiryStatus")]
        public PurchaseInquiryStatusDTO InquiryStatus { get; set; }

        [JsonProperty("seller")]
        public DTO.Customer.CustomerBase Seller { get; set; }

        [JsonProperty("sellerAddress")]
        public string SellerAddress { get; set; }
    }
}
