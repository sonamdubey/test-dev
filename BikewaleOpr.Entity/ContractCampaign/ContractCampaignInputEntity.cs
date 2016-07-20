
using Newtonsoft.Json;
namespace BikewaleOpr.Entity.ContractCampaign
{
    /// <summary>
    /// Created by  :   Sushil Kumar on 14 July 2016
    /// Description :   Contract Campaign InputEntity for API to sync data with cw
    /// </summary>
    public class ContractCampaignInputEntity
    {
        [JsonProperty("consumerId")]
        public int ConsumerId { get; set; }
        [JsonProperty("leadCampaignId")]
        public int LeadCampaignId { get; set; }
        [JsonProperty("lastUpdatedBy")]
        public int LastUpdatedBy { get; set; }
        [JsonProperty("productTypeId")]
        public int ProductTypeId { get; set; }
        [JsonProperty("dealerType")]
        public int DealerType { get; set; }
        [JsonProperty("ncdBrandId")]
        public int NCDBranchId { get; set; }
        [JsonProperty("oldMaskingNumber")]
        public string OldMaskingNumber { get; set; }
        [JsonProperty("maskingNumber")]
        public string MaskingNumber { get; set; }
        [JsonProperty("sellerMobileMaskingId")]
        public int SellerMobileMaskingId { get; set; }

    }
}
