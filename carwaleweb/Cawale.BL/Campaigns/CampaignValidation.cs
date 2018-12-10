namespace Carwale.BL.Campaigns
{
    public static class CampaignValidation
    {
        public static bool IsCampaignValid(Entity.Campaigns.Campaign campaign)
        {
            return campaign != null && campaign.Id > 0 && campaign.DealerId > 0 ? true : false;
        }
    }
}
