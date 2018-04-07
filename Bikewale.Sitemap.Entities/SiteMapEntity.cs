using Newtonsoft.Json;
namespace Bikewale.Sitemap.Entities
{
    public class SiteMapEntity
    {
        [JsonProperty("bikeMake")]
        public BikeMake BikeMake { get; set; }
        [JsonProperty("bikeModel")]
        public BikeModel BikeModel { get; set; }
    }
}
