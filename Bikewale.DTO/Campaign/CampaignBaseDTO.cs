using Newtonsoft.Json;

namespace Bikewale.DTO.Campaign
{
    /// <summary>
    /// Created by  :   Sumit Kate on 05 Oct 2017
    /// Description :   Campaign Base DTO. All campaigns must inherit from this DTO
    /// </summary>
    public class CampaignBaseDTO
    {
        [JsonProperty("type")]
        public CampaignType CampaignType { get; set; }
    }
}
