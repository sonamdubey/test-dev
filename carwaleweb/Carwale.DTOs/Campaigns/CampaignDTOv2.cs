using Carwale.DTOs.Dealer;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;


namespace Carwale.DTOs.Campaigns
{
    public class CampaignDTOv2
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("displayName")]
        public string DisplayName { get; set; }

        [JsonProperty("maskingNumber")]
        public string MaskingNumber { get; set; }

        [JsonProperty("type")]
        public short Type { get; set; }

        [JsonProperty("templateHtml")]
        public string TemplateHtml { get; set; }

        [JsonProperty("dealers")]
        public IEnumerable<CTDealerDTO> Dealers { get; set; }

        [JsonProperty("linkText")]
        public string LinkText { get; set; }

        [JsonProperty("isEmailRequired")]
        public bool IsEmailRequired { get; set; }

        [JsonProperty("priority")]
        public short Priority { get; set; }

        CampaignDTOv2()
        {
            this.Dealers = Enumerable.Empty<CTDealerDTO>();
        }
    }
}
