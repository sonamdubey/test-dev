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
        IEnumerable<BikeMakeEntity> GetBikeMakes();
        IEnumerable<BikeModelEntity> GetBikeModels(uint makeId);
        IEnumerable<StateEntity> GetStates();
        IEnumerable<CityEntity> GetCitiesByState(uint stateId);
        IEnumerable<MfgRuleEntity> GetManufacturerCampaignRules(uint campaignId);
        bool SaveManufacturerCampaignRules(uint campaignId, string modelIds, string stateIds, string cityIds, bool isAllIndia, uint userId);
        bool DeleteManufacturerCampaignRules(uint campaignId, uint modelId, uint stateId, uint cityId, uint userId, bool isAllIndia);
        Entities.ManufacturerCampaignEntity GetCampaigns(uint modelId, uint cityId, ManufacturerCampaignServingPages pageId);
        bool SaveManufacturerCampaignLead(uint dealerid, uint pqId, string customerName, string customerEmail, string customerMobile, uint colorId, uint leadSourceId, string utma, string utmz, string deviceId, uint campaignId);
    }
}
