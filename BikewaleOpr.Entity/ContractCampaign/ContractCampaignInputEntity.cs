
namespace BikewaleOpr.Entity.ContractCampaign
{
    /// <summary>
    /// Created by  :   Sushil Kumar on 14 July 2016
    /// Description :   Contract Campaign InputEntity for API to sync data with cw
    /// </summary>
    public class ContractCampaignInputEntity
    {
        public int ConsumerId { get; set; }
        public int LeadCampaignId { get; set; }
        public int LastUpdatedBy { get; set; }
        public int ProductTypeId { get; set; }
        public int DealerType { get; set; }
        public int NCDBranchId { get; set; }
        public string OldMaskingNumber { get; set; }
        public string MaskingNumber { get; set; }
        public int SellerMobileMaskingId { get; set; }

    }
}
