using Newtonsoft.Json;

namespace Bikewale.DTO.Campaign
{
    /// <summary>
    /// Created by  :   Sumit Kate on 05 Oct 2017
    /// Description :   Campaign Details Base
    /// </summary>
    public class CampaignDetailsBase : CampaignBaseDTO
    {
        [JsonProperty("captionText")]
        public string CaptionText { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("maskingNumber")]
        public string MaskingNumber { get; set; }
        [JsonProperty("offers")]
        public PriceQuote.v2.DPQOffer Offers { get; set; }
    }
}
