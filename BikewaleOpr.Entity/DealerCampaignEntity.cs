
namespace BikewaleOpr.Entity
{
    /// <summary>
    /// Created by  :   Sumit Kate on 11 July 2016
    /// Description :   BW Dealer Campaign entity
    /// </summary>
    public class DealerCampaignEntity
    {
        public int CampaignId { get; set; }
        public string CampaignName { get; set; }
        public string MaskingNumber { get; set; }
        public string EmailId { get; set; }
        public int ServingRadius { get; set; }
    }
}
