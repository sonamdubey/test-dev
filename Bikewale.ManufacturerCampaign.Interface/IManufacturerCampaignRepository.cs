using Bikewale.ManufacturerCampaign.Entities;
using BikewaleOpr.Entities;
using BikewaleOpr.Entity.ManufacturerCampaign;
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
        uint saveManufacturerCampaign(ConfigureCampaignSave objCampaign);
        IEnumerable<ManufacturerEntity> GetManufacturersList();
        IEnumerable<BikeMakeEntity> GetBikeMakes();
        IEnumerable<BikeModelEntity> GetBikeModels(uint makeId);
        IEnumerable<StateEntity> GetStates();
        IEnumerable<CityEntity> GetCitiesByState(uint stateId);
        IEnumerable<MfgRuleEntity> GetManufacturerCampaignRules(uint campaignId);
        bool SaveManufacturerCampaignRules(uint campaignId, string modelIds, string stateIds, string cityIds, bool isAllIndia, uint userId);
        bool DeleteManufacturerCampaignRules(uint campaignId, uint modelId, uint stateId, uint cityId, uint userId, bool isAllIndia);
    }
}
