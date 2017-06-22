using Bikewale.ManufacturerCampaign.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.ManufacturerCampaign.Interface
{
    public interface IManufacturerCampaignRepository 
    {
        ConfigureCampaignEntity getManufacturerCampaign(uint dealerId, uint campaignId);
    }
}
