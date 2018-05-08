
namespace BikewaleOpr.Entities
{
    /// <summary>
    /// Created by  :   Sumit Kate on 11 July 2016
    /// Description :   BW Dealer Campaign entity
    /// Modified by :   Sumit Kate on 29 Dec 2016
    /// Description :   Added DailyLimit and Call To action value
    /// </summary>
    public class DealerCampaignEntity
    {
        public int CampaignId { get; set; }
        public string CampaignName { get; set; }
        public string MaskingNumber { get; set; }
        public string EmailId { get; set; }
        public int ServingRadius { get; set; }
        public uint DailyLeadLimit { get; set; }
        public ushort CallToAction { get; set; }
        public string DealerMobile { get; set; }
        public string CommunicationNumbers { get; set; }
        public string CommunicationEmails { get; set; }
    }
}
