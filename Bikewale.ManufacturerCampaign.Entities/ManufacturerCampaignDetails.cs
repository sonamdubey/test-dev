﻿using System;

namespace Bikewale.ManufacturerCampaign.Entities
{
    /// <summary>
    /// Modified by : Ashutosh Sharma on 25 Jan 2017
    /// Description : Added 'DailyStartTime' and 'DailyEndTime'.
    /// </summary>
    public class ManufacturerCampaignDetails
    {
        public uint Id { get; set; }
        public string Description { get; set; }
        public string MaskingNumber { get; set; }
        public DateTime CampaignStartDate { get; set; }

        public DateTime CampaignEndDate { get; set; }
        public int DailyLeadLimit { get; set; }
        public int TotalLeadLimit { get; set; }
        public int DailyLeadsDelivered { get; set; }
        public int TotalLeadsDelivered { get; set; }
        public string CampaignStatus { get; set; }

        public bool ShowCampaignOnExshowroom { get; set; }
        public string DealerName { get; set; }
        public string MobileNo { get; set; }
        public DateTime DailyStartTime { get; set; }
        public DateTime DailyEndTime { get; set; }
        public ushort CampaignDays { get; set; }
    }
}
