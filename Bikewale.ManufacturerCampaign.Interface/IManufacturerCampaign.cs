using Bikewale.ManufacturerCampaign.Entities;

namespace Bikewale.ManufacturerCampaign.Interface
{
    /// <summary>
    /// Created by  :   Sumit Kate on 29 Jun 2017
    /// Description :   Manufacturer Campaign Business Layer Interface
    /// </summary>
    public interface IManufacturerCampaign
    {
        ManufacturerCampaignEntity GetCampaigns(uint modelId, uint cityId, ManufacturerCampaignServingPages pageId);
        bool SaveManufacturerIdInPricequotes(uint pqId,uint dealerId);
    }
}
