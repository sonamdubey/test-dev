using Bikewale.DTO.Campaign;

namespace Bikewale.Interfaces.Campaign
{
    /// <summary>
    /// Author  : Kartik Rathod on 12 sept 2018
    /// </summary>
    public interface ICampaignBL
    {
        ESDSCampaignDto GetCampaignLocationWise(uint cityId, uint areaId, uint modelId);
    }
}
