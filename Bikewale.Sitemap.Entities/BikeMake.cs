using Newtonsoft.Json;

namespace Bikewale.Sitemap.Entities
{
    public class BikeMake
    {
        [JsonProperty("makeMaskingName")]
        public string MakeMaskingName { get; set; }
    }
}
