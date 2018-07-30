using Newtonsoft.Json;

namespace Bikewale.DTO.UsedBikes
{
    public class PopularUsedBikesBase
    {
        [JsonProperty("makeName")]
        public string MakeName { get; set; }

        [JsonProperty("totalBikes")]
        public uint TotalBikes { get; set; }

        [JsonProperty("avgPrice")]
        public double AvgPrice { get; set; }

        [JsonProperty("hostURL")]
        public string HostURL { get; set; }

        [JsonProperty("originalImagePath")]
        public string OriginalImagePath { get; set; }

        [JsonProperty("makeMaskingName")]
        public string MakeMaskingName { get; set; }

        [JsonProperty("cityMaskingName")]
        public string CityMaskingName { get; set; }
    }
}
