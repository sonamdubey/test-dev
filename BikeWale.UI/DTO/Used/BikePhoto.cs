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
        [JsonProperty("id")]
        public uint Id { get; set; }
        [JsonProperty("imageUrl")]
        public string ImageUrl { get; set; }
    }
}
