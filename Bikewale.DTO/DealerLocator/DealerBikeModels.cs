using System.Collections.Generic;
using Bikewale.DTO.Widgets.v2;
using Newtonsoft.Json;

namespace Bikewale.DTO.DealerLocator
{
    public class DealerBikeModels
    {
        [JsonProperty("cityName")]
        public string CityName { get; set; }

        [JsonProperty("dealerBikes")]
        public IEnumerable<MostPopularBikes> Models { get; set; }
    }
}
