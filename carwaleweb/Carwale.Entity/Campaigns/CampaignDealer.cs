using Carwale.Entity.Dealers;
using System;

namespace Carwale.Entity.Campaigns
{
    [Serializable]
    public class CampaignDealer
    {
        public Campaign Campaign { get; set; }
        public DealerDetails DealerDetails { get; set; }

    }
}
