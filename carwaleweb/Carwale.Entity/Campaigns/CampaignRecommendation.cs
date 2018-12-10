using Carwale.Entity.CarData;
using Carwale.Entity.PriceQuote;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.Campaigns
{
    public class CampaignRecommendation
    {
        public CarModelDetails CarData { get; set; }
        public Campaign Campaign { get; set; }
        public PriceOverview PricesOverview { get; set; }
    }
}
