using Newtonsoft.Json;
namespace Bikewale.Sitemap.Entities
{
    public class SiteMapEntity
    {
        [JsonProperty("bikeMake")]
        public BikeMakeEntity BikeMake { get; set; }
        [JsonProperty("bikeModel")]
        public BikeModelEntity BikeModel { get; set; }
    }
}
