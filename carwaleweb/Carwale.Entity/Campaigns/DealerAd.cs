using Carwale.Entity.Dealers;
using Carwale.Entity.Enum;
using Carwale.Entity.PriceQuote;
using Carwale.Entity.CarData;
using System;
using System.Collections.Generic;

namespace Carwale.Entity.Campaigns
{
    [Serializable]
    public class DealerAd
    {
        public Campaign Campaign { get; set; }
        public DealerDetails DealerDetails { get; set; }
        public List<Carwale.Entity.PageProperty.PageProperty> PageProperty { get; set; }
        public CarVersionDetails FeaturedCarData { get; set; }
        public CampaignAdType CampaignType { get; set; }
        public List<LeadSource> LeadSource { get; set; }
    }
}
