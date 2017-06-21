using Bikewale.ManufacturerCampaign.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.ManufacturerCampaign.Interface
{
   public interface IManufacturerCampaign
    {
        IEnumerable<BikeMakeEntity> GetBikeMakes();
        IEnumerable<BikeModelEntity> GetBikeModels(uint makeId);
        IEnumerable<StateEntity> GetStates();
        IEnumerable<CityEntity> GetCitiesByState(uint stateId);
        bool SaveManufacturerCampaignRules(uint campaignId, string modelIds, string stateIds, string cityIds, bool isAllIndia, uint userId);
        bool DeleteManufacturerCampaignRules(uint campaignId, uint modelId, uint stateId, uint cityId, uint userId);
    }
}
