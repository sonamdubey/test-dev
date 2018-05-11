using Newtonsoft.Json;

namespace Bikewale.Sitemap.Entities
{
    public class BikeModelEntity
    {
        [JsonProperty("modelMaskingName")]
        public string ModelMaskingName { get; set; }
    }
}