using Carwale.DTOs.Elastic.Autocomplete;
using Newtonsoft.Json;

namespace Carwale.DTOs.Elastic
{
    public class CityPayLoad : BaseAutoComplete
    {
        [JsonProperty("cityId")]
        public int Id { get; set; }

        [JsonProperty("cityName")]
        public string Name { get; set; }
		[JsonProperty("cityMaskingName")]
		public string CityMaskingName { get; set; }
		[JsonProperty("isDuplicate")]
        public bool IsDuplicate { get; set; }

        [JsonProperty("isAreaAvailable")]
        public bool IsAreaAvailable { get; set; }
    }
}
