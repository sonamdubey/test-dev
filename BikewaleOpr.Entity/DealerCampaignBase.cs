﻿using System.Collections.Generic;

namespace BikewaleOpr.Entities
{
    public class DealerCampaignBase
    {
        public string DealerName { get; set; }
        public string DealerNumber { get; set; }
        public DealerCampaignEntity CurrentCampaign { get; set; }
        public IEnumerable<DealerCampaignEntity> DealerCampaigns { get; set; }
        public string ActiveMaskingNumber { get; set; }
    }
}
