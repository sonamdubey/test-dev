using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.ManufacturerCampaign.Entities
{
    public class ConfigureCampaignEntity
    {
        public ManufacturerCampaignDetails DealerDetails { get; set; }
        public IEnumerable<ManufacturerCampaignPages> CampaignPages { get; set; }
    }
}
