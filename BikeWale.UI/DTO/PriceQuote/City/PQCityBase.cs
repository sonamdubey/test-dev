using Newtonsoft.Json;

namespace Bikewale.DTO.PriceQuote.City
{
    /// <summary>
    /// Price Quote City base
    /// Author  :   Sumit Kate
    /// Date    :   20 Aug 2015
    /// Modified By :   Sumit Kate on 12 Jan 2016
    /// Summary     :   Added new property HasAreas
    /// </summary>
    public class PQCityBase
    {
        [JsonProperty("cityId")]
        public uint CityId { get; set; }

        [JsonProperty("cityName")]
        public string CityName { get; set; }

        [JsonProperty("maskingName")]
        public string CityMaskingName { get; set; }

        [JsonProperty("isPopular")]
        public bool IsPopular { get; set; }

        [JsonProperty("hasAreas")]
        public bool HasAreas { get; set; }
    }
}
