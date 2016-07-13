using System.Collections.Generic;

namespace BikewaleOpr.Entity
{
    public class DealerCampaignBase
    {
        public string DealerName { get; set; }
        public string DealerNumber { get; set; }
        public IEnumerable<DealerCampaignEntity> DealerCampaigns { get; set; }
    }
}
