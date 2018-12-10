using Carwale.DTOs.CarData;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Carwale.DTOs.ES
{
    public class SponsoredAdCampaignDto : CarMakeModelDTO
    {
        [JsonProperty("id")]
        public int Id { get; set; }        

        [JsonProperty("isSponsored")]
        public bool IsSponsored { get; set; }

        [JsonProperty("campaignCategoryId")]
        public int CampaignCategoryId { get; set; }

        [JsonProperty("categorySection")]
        public int CategorySection { get; set; }

        [JsonProperty("platformId")]
        public int PlatformId { get; set; }

        [JsonProperty("adHtml")]
        public string AdHtml { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("imageUrl")]
        public string ImageUrl { get; set; }

        [JsonProperty("jumbotronPosition")]
        public string JumbotronPosition { get; set; }

        [JsonProperty("verticalPosition")]
        public string VerticalPosition { get; set; }

        [JsonProperty("horizontalPosition")]
        public string HorizontalPosition { get; set; }

        [JsonProperty("widgetPosition")]
        public string WidgetPosition { get; set; }

        [JsonProperty("startDate")]
        public DateTime? StartDate { get; set; }

        [JsonProperty("endDate")]
        public DateTime? EndDate { get; set; }

        [JsonProperty("isDafault")]
        public bool IsDafault { get; set; }

        [JsonProperty("backgroundColor")]
        public string BackgroundColor { get; set; }

        [JsonProperty("subtitle")]
        public string Subtitle { get; set; }

        [JsonProperty("cardHeader")]
        public string CardHeader { get; set; }

        [JsonProperty("position")]
        public List<int> Position { get; set; }

        [JsonProperty("links")]
        public List<AdLinkDTO> Links { get; set; }
    }
}