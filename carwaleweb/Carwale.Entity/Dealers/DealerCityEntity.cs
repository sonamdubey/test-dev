using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.Dealers
{
    [Serializable]
    public class DealerCityEntity
    {
        [JsonProperty("cityId")]
        public int CityId { get; set; }

        [JsonProperty("cityName")]
        public string CityName { get; set; }

		[JsonProperty("cityMaskingName")]
		public string CityMaskingName { get; set; }

		[JsonProperty("totalCount")]
        public int TotalCount { get; set; }
    }
}
