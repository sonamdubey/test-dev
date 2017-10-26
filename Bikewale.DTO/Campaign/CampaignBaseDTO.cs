using Newtonsoft.Json;

namespace Bikewale.DTO.Campaign
{
    /// <summary>
    /// Created by  :   Sumit Kate on 05 Oct 2017
    /// Description :   Campaign Base DTO. All campaigns must inherit from this DTO
    /// </summary>
    public class CampaignBaseDto
    {
        [JsonProperty("type")]
        public CampaignType CampaignType { get; set; }
        [JsonProperty("detailsCampaign", NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Ignore)]
        public DetailsDto DetailsCampaign { get; set; }
        [JsonProperty("campaignLeadSource", NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Ignore)]
        public ESCampaignBase CampaignLeadSource { get; set; }
    }
}
