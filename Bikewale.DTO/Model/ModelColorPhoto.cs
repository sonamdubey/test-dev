using Newtonsoft.Json;

namespace Bikewale.DTO.Model
{
    /// <summary>
    /// Created by : Vivek Singh Tomar on 5th Oct 2017
    /// </summary>
    public class ModelColorPhoto: ModelColorDto
    {
        [JsonProperty("host")]
        public string Host { get; set; }
        [JsonProperty("originalImagePath")]
        public string OriginalImagePath { get; set; }
        [JsonProperty("isImageExists")]
        public bool IsImageExists { get; set; }
        [JsonProperty("bikeModelColoeId")]
        public uint BikeModelColorId { get; set; }
        [JsonProperty("imageCategory")]
        public string ImageCategory { get; set; }
    }
}
