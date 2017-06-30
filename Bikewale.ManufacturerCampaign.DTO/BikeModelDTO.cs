using Newtonsoft.Json;

namespace Bikewale.ManufacturerCampaign.Entities
{
    /// <summary>
    /// Created by : Aditi Srivastava on 22 Jun 2017
    /// Summary    : DTO for bikemodels
    /// </summary>
    public class BikeModelDTO
   {
        [JsonProperty("modelId")]
        public uint ModelId { get; set; }

        [JsonProperty("modelName")]
        public string ModelName { get; set; }

    }
}
