using Bikewale.ManufacturerCampaign.Entities;
using System.Collections.Generic;

namespace Bikewale.ManufacturerCampaign.Interface
{
    /// <summary>
    /// Created by  :   Sumit Kate on 29 Jun 2017
    /// Description :   Manufacturer Campaign Business Layer Interface
    /// </summary>
    public interface IManufacturerCampaign
    {
        ManufacturerCampaignEntity GetCampaigns(uint modelId, uint cityId, ManufacturerCampaignServingPages pageId);
        bool SaveManufacturerIdInPricequotes(uint pqId, uint dealerId);
        bool ClearCampaignCache(uint campaignId);
        bool ClearCampaignCache(uint campaignId, IEnumerable<string> modelIds, IEnumerable<string> cityIds);
    }
}
