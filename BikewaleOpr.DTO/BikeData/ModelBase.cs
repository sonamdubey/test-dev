using Newtonsoft.Json;

namespace BikewaleOpr.DTO.BikeData
{
    /// <summary>
    /// Created By : Ashish G. Kamble on 18 Apr 2017
    /// Summary : Class have properties in the dto
    /// </summary>

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
