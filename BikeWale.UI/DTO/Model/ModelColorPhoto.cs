using Newtonsoft.Json;

namespace Bikewale.DTO.Model
{
    /// <summary>
    /// Created by : Vivek Singh Tomar on 5th Oct 2017
    /// Modified by : Rajan Chauhan on 23rd Jan 2017
    /// Description : Changed originalImagePath to originalImgPath for consistency
    /// </summary>
    public class ModelColorPhoto: ModelColorDto
    {
        [JsonProperty("hostUrl")]
        public string Host { get; set; }
        [JsonProperty("originalImgPath")]
        public string OriginalImagePath { get; set; }
        [JsonProperty("isImageExists")]
        public bool IsImageExists { get; set; }
        [JsonProperty("bikeModelColorId")]
        public uint BikeModelColorId { get; set; }
        [JsonProperty("imageCategory")]
        public string ImageCategory { get; set; }
    }
}
