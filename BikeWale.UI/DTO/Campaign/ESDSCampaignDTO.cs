using Newtonsoft.Json;

namespace Bikewale.DTO.Campaign
{
    /// <summary>
    /// Author  : Kartik Rathod on 12 sept 2018
    /// Desc    : get campaign details for Ds ANd Es campaings related to lead popup
    /// </summary>
    public class ESDSCampaignDto
    {
        [JsonProperty("manufacturerCampaign")]
        public ManufacturerCampaignDto ManufacturerCampaign { get; set; }
        [JsonProperty("dealerCampaign")]
        public DealerCampaignDto DealerCampaign { get; set; }
    }
}
