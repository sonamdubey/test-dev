using System;
namespace Bikewale.ManufacturerCampaign.Entities.SearchCampaign
{
    /// <summary>
    /// Modified by : Ashutosh Sharma on 25 Jan 2017
    /// Description : Added 'DailyStartTime' and 'DailyEndTime'.
    /// Modified by : Rajan Chauhan on 08 Mar 2017
    /// Description : Added 'CampaignDays'
    /// </summary>
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
        public string DailyStartTime { get; set; }
        public string DailyEndTime { get; set; }
        public ushort? CampaignDays { get; set; }
    }
}
