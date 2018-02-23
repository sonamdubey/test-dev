
using Newtonsoft.Json;
namespace Bikewale.DTO.NewBikeSearch
{
    public class SearchFilterDTO
    {

        [JsonProperty("minPrice")]
        public int MinPrice { get; set; }
        [JsonProperty("maxPrice")]
        public int MaxPrice { get; set; }
        [JsonProperty("maxMileage")]
        public double MaxMileage { get; set; }
        [JsonProperty("minMileage")]
        public double MinMileage { get; set; }
        [JsonProperty("bodyStyle")]
        public ushort BodyStyle { get; set; }
        [JsonProperty("minDisplacement")]
        public double MinDisplacement { get; set; }
        [JsonProperty("maxDisplacement")]
        public double MaxDisplacement { get; set; }
        [JsonProperty("makeId")]
        public uint MakeId { get; set; }
        [JsonProperty("abs")]
        public bool ABS { get; set; }
        [JsonProperty("discBrake")]
        public bool DiscBrake { get; set; }
        [JsonProperty("drumBrake")]
        public bool DrumBrake { get; set; }
        [JsonProperty("alloyWheel")]
        public bool AlloyWheel { get; set; }
        [JsonProperty("spokeWheel")]
        public bool SpokeWheel { get; set; }
        [JsonProperty("electric")]
        public bool Electric { get; set; }
        [JsonProperty("manual")]
        public bool Manual { get; set; }
    }
}
