using Carwale.Entity.Campaigns;
using Carwale.Entity.CarData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.CrossSell
{
    public class CrossSellDetail
    {
        public Campaign CampaignDetail { get; set; }
        public CarVersionDetails CarVersionDetail { get; set; }
    }
}
