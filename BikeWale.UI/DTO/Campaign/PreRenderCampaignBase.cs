using Newtonsoft.Json;

namespace Bikewale.DTO.Campaign
{
    public class PreRenderCampaignBase 
    {
        [JsonProperty("templateHtml")]
        public string TemplateHtml { get; set; }
    }
}
