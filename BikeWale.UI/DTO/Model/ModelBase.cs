using Newtonsoft.Json;

namespace Bikewale.DTO.Model
{
    public class ModelBase
    {
        [JsonProperty("modelId")]
        public int ModelId { get; set; }

        [JsonProperty("modelName")]
        public string ModelName { get; set; }

        [JsonProperty("maskingName")]
        public string MaskingName { get; set; }
    }
}
