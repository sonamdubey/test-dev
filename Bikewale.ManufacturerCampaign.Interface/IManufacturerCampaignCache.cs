
using Bikewale.ManufacturerCampaign.Entities;
namespace Bikewale.ManufacturerCampaign.Interface
{
    public interface IManufacturerCampaignCache
    {
        ManufacturerCampaignEntity GetCampaigns(uint modelId, uint cityId, ManufacturerCampaignServingPages pageId);
    }
}
