using Newtonsoft.Json;

namespace Carwale.DTOs.CarData
{
    public class CarIdDTO
    {
        [JsonProperty("makeId")]
        public int MakeId { get; set; }
        [JsonProperty("modelId")]
        public int ModelId { get; set; }
        [JsonProperty("versionId")]
        public int VersionId { get; set; }
    }

    public class CarIdWithImageDto : CarIdDTO
    {
        [JsonProperty("makeName")]
        public string MakeName { get; set; }

        [JsonProperty("modelName")]
        public string ModelName { get; set; }

        [JsonProperty("hostUrl")]
        public string HostUrl { get; set; }

        [JsonProperty("originalImgPath")]
        public string OriginalImage { get; set; }
    }
}
