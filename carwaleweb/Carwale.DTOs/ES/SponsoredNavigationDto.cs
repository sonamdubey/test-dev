using Newtonsoft.Json;
using System;

namespace Carwale.DTOs.ES
{
    public class SponsoredNavigationDto
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("sectionId")]
        public int SectionId { get; set; }
        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("cta")]
        public string CTA { get; set; }
        [JsonProperty("linkUrl")]
        public string LinkUrl { get; set; }
        [JsonProperty("originalImgPath")]
        public string OriginalImgPath { get; set; }
        [JsonProperty("hostUrl")]
        public string HostUrl { get; set; }
        [JsonProperty("isSponsored")]
        public bool IsSponsored { get; set; }
        [JsonProperty("startDate")]
        public DateTime StartDate { get; set; }
        [JsonProperty("endDate")]
        public DateTime EndDate { get; set; }
        [JsonProperty("isActive")]
        public bool IsActive { get; set; }
    }
}
