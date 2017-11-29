namespace Bikewale.ManufacturerCampaign.Entities.SearchCampaign
{
    public class ManufacturerCampaignDetailsList
    {
        public uint Id { get; set; }
        public string  Description { get; set; }
        public string MaskingNumber { get; set; }
        public string CampaignStartDate { get; set; }
       
        public string CampaignEndDate { get; set; }
        public int DailyLeadLimit { get; set; }
        public int TotalLeadLimit { get; set; }
        public int DailyLeadsDelivered { get; set; }
        public int TotalLeadsDelivered { get; set; }
        public string  Status { get; set; }

        public bool ShowCampaignOnExshowroom { get; set; }
    }
}
