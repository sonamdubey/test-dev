using Newtonsoft.Json;

namespace BikewaleOpr.DTO.BwPrice
{
    /// <summary>
    /// Created by : Ashutosh Sharma on 10 Nov 2017
    /// Description : Input DTO for copy dealer pricing to BikeWale pricing
    /// </summary>
    public class BWPriceInputDTO
    {
        [JsonProperty("versionAndPriceList")]
        public string VersionAndPriceList { get; set; }
        [JsonProperty("citiesList")]
        public string CitiesList { get; set; }
        [JsonProperty("makeId")]
        public uint MakeId { get; set; }
        [JsonProperty("modelIds")]
        public string ModelIds { get; set; }
        [JsonProperty("userId")]
        public int UserId { get; set; }
    }
}
