using Newtonsoft.Json;

namespace Carwale.Entity.Dealers.URI
{
    public class CampaignInputURI
    {
        [JsonProperty(PropertyName = "platformid")]
        public short PlatformId { get; set; }

        [JsonProperty(PropertyName = "campaignid")]
        public int CampaignId { get; set; }

        [JsonProperty(PropertyName = "cityid")]
        public int CityId { get; set; }

        [JsonProperty(PropertyName = "modelid")]
        public int ModelId { get; set; }

        [JsonProperty(PropertyName = "zoneid")]
        public string ZoneId { get; set; }

        [JsonProperty(PropertyName = "areaid")]
        public int AreaId { get; set; }

        [JsonProperty(PropertyName = "screentype")]
        public string ScreenType { get; set; }
    }
}
