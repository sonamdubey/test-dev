using Newtonsoft.Json;

namespace Bikewale.DTO.Used.Search
{
    public class BikePhoto
    {
        [JsonProperty("hostUrl")]
        public string HostUrl { get; set; }
        [JsonProperty("originalImagePath")]
        public string OriginalImagePath { get; set; }
        [JsonProperty("isMain")]
        public bool IsMain { get; set; }
    }
}
