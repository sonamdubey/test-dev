using Bikewale.ManufacturerCampaign.Entities;
using Bikewaleopr.ManufacturerCampaign.Entities;
using BikewaleOpr.Entities;
using BikewaleOpr.Entity.ManufacturerCampaign;
using System.Collections.Generic;


namespace Bikewale.ManufacturerCampaign.Interface
{
    public interface IManufacturerCampaignRepository 
    {
        ConfigureCampaignEntity getManufacturerCampaign(uint dealerId, uint campaignId);
        uint saveManufacturerCampaign(ConfigureCampaignSave objCampaign);
        IEnumerable<ManufacturerEntity> GetManufacturersList();

        void saveManufacturerCampaignPopup(ManufacturerCampaignPopup objData);
        ManufacturerCampaignPopup getManufacturerCampaignPopup(uint campaignId);
    }
}
