using Bikewale.ManufacturerCampaign.Entities;
using Bikewaleopr.ManufacturerCampaign.Entities;
using BikewaleOpr.Entities;
using BikewaleOpr.Entity.ManufacturerCampaign;
using BikewaleOpr.Models.ManufacturerCampaign;
using System.Collections.Generic;


namespace Bikewale.ManufacturerCampaign.Interface
{
    public interface IManufacturerCampaignRepository 
    {
        ConfigureCampaignEntity GetManufacturerCampaign(uint dealerId, uint campaignId);
        CampaignPropertyEntity GetManufacturerCampaignProperties(uint campaignId);
        uint saveManufacturerCampaign(ConfigureCampaignSave objCampaign);
        IEnumerable<ManufacturerEntity> GetManufacturersList();
        void saveManufacturerCampaignPopup(ManufacturerCampaignPopup objData);
        ManufacturerCampaignPopup getManufacturerCampaignPopup(uint campaignId);
        IEnumerable<BikeMakeEntity> GetBikeMakes();
        IEnumerable<BikeModelEntity> GetBikeModels(uint makeId);
        IEnumerable<StateEntity> GetStates();
        IEnumerable<CityEntity> GetCitiesByState(uint stateId);
        ManufacturerCampaignRulesWrapper GetManufacturerCampaignRules(uint campaignId);
        bool SaveManufacturerCampaignRules(uint campaignId, string modelIds, string stateIds, string cityIds, bool isAllIndia, uint userId);
        bool DeleteManufacturerCampaignRules(uint campaignId, uint modelId, uint stateId, uint cityId, uint userId, bool isAllIndia);
        bool SaveManufacturerCampaignProperties(CampaignPropertiesVM objCampaign);
    }
}
