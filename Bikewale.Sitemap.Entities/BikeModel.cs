using Newtonsoft.Json;

namespace Bikewale.Sitemap.Entities
{
    public class BikeModel
    {
        [JsonProperty("modelMaskingName")]
        public string ModelMaskingName { get; set; }
    }
}
