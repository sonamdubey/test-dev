using Newtonsoft.Json;
using System.Collections.Generic;

namespace Carwale.DTOs.CarData
{
    public class SponsoredAdContentDTO
    {
        [JsonProperty("categoryId")]
        public int CategoryId;

        [JsonProperty("adType")]
        public string AdType;

        [JsonProperty("banners")]
        public List<SponsoredAdHtmlContentDTO> AdHtmlContent;
        
    }

    public class AdMonetizationDTO
    {
        [JsonProperty("position")]
        public List<int> Position { get; set; }

        [JsonProperty("imageUrl")]
        public string ImageUrl { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("subtitle")]
        public string Subtitle { get; set; }

        [JsonProperty("content")]
        public string Content { get; set; }

        [JsonProperty("cardHeader")]
        public string CardHeader { get; set; }

        [JsonProperty("links")]
        public List<AdLinkDTO> Links;
    }

    public class AdLinkDTO
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("isInsideApp")]
        public bool IsInsideApp { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("isUpcoming")]
        public bool IsUpcoming { get; set; }

        [JsonProperty("extraParams")]
        public object PayLoad { get; set; }
    }
}
