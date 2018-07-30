using Newtonsoft.Json;
using System.Collections.Generic;

namespace Bikewale.DTO.BikeData
{
    public class UpcomingBikeList
    {
        [JsonProperty(PropertyName = "upcomingBike")]
        public IEnumerable<UpcomingBike> UpcomingBike { get; set; }
    }
}
