using Bikewale.ManufacturerCampaign.Entities;
using Bikewale.ManufacturerCampaign.Interface;

namespace Bikewale.ManufacturerCampaign.BAL
{
    public class ManufacturerCampaign : IManufacturerCampaign
    {
        private readonly Interface.IManufacturerCampaignRepository _repo = null;
        public ManufacturerCampaign(Interface.IManufacturerCampaignRepository repo)
        {
            _repo = repo;
        }
        public ManufacturerCampaignEntity GetCampaigns(uint modelId, uint cityId, ManufacturerCampaignServingPages pageId)
        {
            return _repo.GetCampaigns(modelId, cityId, pageId);
        }
    }
}
