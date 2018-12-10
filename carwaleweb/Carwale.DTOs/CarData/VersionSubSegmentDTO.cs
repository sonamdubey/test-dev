using Newtonsoft.Json;

namespace Carwale.DTOs.CarData
{
    public class VersionSubSegmentDTO
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("versionName")]
        public string VersionName { get; set; }
        [JsonProperty("subSegment")]
        public string SubSegment { get; set; }
        [JsonProperty("segment")]
        public string Segment { get; set; }
        [JsonProperty("carTransmission")]
        public string CarTransmission { get; set; }
        [JsonProperty("fuelType")]
        public string FuelType { get; set; }
        [JsonProperty("maskingName")]
        public string MaskingName { get; set; }
        [JsonProperty("avgPrice")]
        public int AvgPrice { get; set; }
        [JsonProperty("isNew")]
        public bool IsNew { get; set; }
        [JsonProperty("isUpcoming")]
        public bool IsUpcoming { get; set; }
    }
}

