using Newtonsoft.Json;
using System.Collections.Generic;

namespace Carwale.DTOs.CarData
{
    public class ModelSpecificationsDTO
    {
        [JsonProperty("modelId")]
        public int ModelId { get; set; }
        [JsonProperty("modelName")]
        public string ModelName { get; set; }
        [JsonProperty("maskingName")]
        public string MaskingName { get; set; }
        [JsonProperty("makeId")]
        public int MakeId { get; set; }
        [JsonProperty("makeName")]
        public string MakeName { get; set; }
        [JsonProperty("makeMaskingName")]
        public string MakeMaskingName { get; set; }
        [JsonProperty("rootId")]
        public int RootId { get; set; }
        [JsonProperty("rootName")]
        public string RootName { get; set; }
        [JsonProperty("subSegment")]
        public string SubSegment { get; set; }
        [JsonProperty("bodyStyle")]
        public string BodyStyle { get; set; }
        [JsonProperty("avgPrice")]
        public int AvgPrice { get; set; }
        [JsonProperty("isUpcoming")]
        public bool IsUpcoming { get; set; }
        [JsonProperty("isNew")]
        public bool IsNew { get; set; }
        [JsonProperty("fuelEconomyRating")]
        public double FuelEconomy { get; set; }
        [JsonProperty("valueForMoneyRating")]
        public double ValueForMoney { get; set; }
        [JsonProperty("comfortRating")]
        public double Comfort { get; set; }
        [JsonProperty("performanceRating")]
        public double Performance { get; set; }
        [JsonProperty("looksRating")]
        public double Looks { get; set; }
        [JsonProperty("reviewCount")]
        public int ReviewCount { get; set; }
        [JsonProperty("reviewRate")]
        public double ReviewRate { get; set; }
        [JsonProperty("versions")]
        public List<VersionSubSegmentDTO> Versions { get; set; }

    }
}
