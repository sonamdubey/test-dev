using Bikewale.ManufacturerCampaign.Entities;
using Bikewaleopr.ManufacturerCampaign.Entities;
using BikewaleOpr.Entities;
using BikewaleOpr.Entity.ManufacturerCampaign;
using BikewaleOpr.Models.ManufacturerCampaign;
using System;
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
        Entities.ManufacturerCampaignEntity GetCampaigns(uint modelId, uint cityId, ManufacturerCampaignServingPages pageId);
        uint SaveManufacturerCampaignLead(uint dealerid, uint pqId, UInt64 customerId, string customerName, string customerEmail, string customerMobile, uint leadSourceId, string utma, string utmz, string deviceId, uint campaignId, uint leadId);
        bool ResetTotalLeadDelivered(uint campaignId, uint userId);
        IEnumerable<BikeModelEntity> GetUnmappedHondaModels( uint dealerId);
        uint SaveManufacturerCampaignLead(ES_SaveEntity campaignDetails);
    }
}
