using Newtonsoft.Json;
using System.Collections.Generic;

namespace Bikewale.DTO.Campaign
{
    /// <summary>
    /// Created by  :   Sumit Kate on 05 Oct 2017
    /// Description :   Campaign Details Base
    /// </summary>
    public class CampaignDetailsBase 
    {
        [JsonProperty("captionText", NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string CaptionText { get; set; }
        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Name { get; set; }
        [JsonProperty("maskingNumber", NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string MaskingNumber { get; set; }
        [JsonProperty("offers", NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Ignore)]
        public IEnumerable<PriceQuote.v2.DPQOffer> Offers { get; set; }
    }
}
