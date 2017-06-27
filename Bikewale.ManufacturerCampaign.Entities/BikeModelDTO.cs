using Newtonsoft.Json;

namespace Bikewale.ManufacturerCampaign.Entities
{
    public class BikeModelDTO
   {
        [JsonProperty("modelId")]
        public uint ModelId { get; set; }

        [JsonProperty("modelName")]
        public string ModelName { get; set; }

    }
}
