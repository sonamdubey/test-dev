using Newtonsoft.Json;

namespace Bikewale.DTO.BikeBooking.Model
{
    /// <summary>
    /// Bikebooking model base
    /// Author  :   Sumit Kate
    /// Date    :   20 Aug 2015
    /// </summary>
    public class BBModelBase
    {
        [JsonProperty("modelId")]
        public int ModelId { get; set; }
        [JsonProperty("modelName")]
        public string ModelName { get; set; }
        [JsonProperty("maskingName")]
        public string MaskingName { get; set; }
    }
}
