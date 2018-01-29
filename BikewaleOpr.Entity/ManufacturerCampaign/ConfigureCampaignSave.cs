using System;

namespace BikewaleOpr.Entity.ManufacturerCampaign
{
    /// <summary>
    /// Modified by : Ashutosh Sharma on 25 Jan 2017
    /// Description : Added 'DailyStartTime' and 'DailyEndTime'.
    /// </summary>
    public class ConfigureCampaignSave
    {
        public string Description { get; set; }
        public string MaskingNumber { get; set; }
        public uint DailyLeadLimit { get; set; }
        public uint TotalLeadLimit { get; set; }
        public bool ShowOnExShowroomPrice { get; set; }
        public uint CampaignId { get; set;}
        public uint DealerId { get; set; }
        public string CampaignPages { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string UserId { get; set; }
        public string OldMaskingNumber { get; set; }
        public string MobileNumber { get; set; }
        public DateTime DailyStartTime { get; set; }
        public DateTime DailyEndTime { get; set; }
    }
}
