using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.CrossSell
{
    [Serializable]
    public class FeaturedVersion
    {
        public int VersionId { get; set; }
        public int CampaignId { get; set; }
    }
}
