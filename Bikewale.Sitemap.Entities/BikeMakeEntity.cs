using Newtonsoft.Json;

namespace Bikewale.Sitemap.Entities
{
    public class BikeMakeEntity
    {
        [JsonProperty("makeMaskingName")]
        public string MakeMaskingName { get; set; }
    }
}