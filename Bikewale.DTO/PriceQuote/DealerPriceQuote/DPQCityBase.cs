using Newtonsoft.Json;

namespace Bikewale.DTO.PriceQuote.DealerPriceQuote
{
    /// <summary>
    /// Dealer Price Quote City Base
    /// Author  :   Sumit Kate
    /// Date    :   20 Aug 2015
    /// </summary>
    public class DPQCityBase
    {
        [JsonProperty("cityId")]
        public uint CityId { get; set; }

        [JsonProperty("cityName")]
        public string CityName { get; set; }

        [JsonProperty("cityMaskingName")]
        public string CityMaskingName { get; set; }
    }
}
