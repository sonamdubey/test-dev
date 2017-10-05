using Newtonsoft.Json;

namespace Bikewale.DTO.Campaign
{
    public class PreRenderCampaignBase : CampaignBaseDTO
    {
        [JsonProperty("templateHtml")]
        public string TemplateHtml { get; set; }
    }
}
