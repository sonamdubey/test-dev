using Bikewale.Interfaces;
using Bikewale.ManufacturerCampaign.Entities;

namespace Bikewale.BAL.ManufacturerCampaign
{
    /// <summary>
    /// Created by  :   Sumit Kate on 29 Jun 2017
    /// Description :   Manufacturer Campaign Business Layer
    /// </summary>
    public class ManufacturerCampaign : IManufacturerCampaign
    {
        private readonly Bikewale.ManufacturerCampaign.Interface.IManufacturerCampaignRepository _campaignRepo;
        /// <summary>
        /// Type Initializer
        /// </summary>
        /// <param name="campaignRepo"></param>
        public ManufacturerCampaign(Bikewale.ManufacturerCampaign.Interface.IManufacturerCampaignRepository campaignRepo)
        {
            _campaignRepo = campaignRepo;
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 29 Jun 2017
        /// Description :   Return Manufacturer campaigns
        /// </summary>
        /// <param name="modelId"></param>
        /// <param name="cityId"></param>
        /// <param name="pageId"></param>
        /// <returns></returns>
        public ManufacturerCampaignEntity GetCampaigns(uint modelId, uint cityId, ManufacturerCampaignServingPages pageId)
        {
            return _campaignRepo.GetCampaigns(modelId, cityId, pageId);
        }
    }
}
