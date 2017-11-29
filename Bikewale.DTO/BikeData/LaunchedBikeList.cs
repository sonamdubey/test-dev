using Newtonsoft.Json;
using System.Collections.Generic;

namespace Bikewale.DTO.BikeData
{
    public class LaunchedBikeList
    {
        [JsonProperty("launchedBike")]
        public IEnumerable<LaunchedBike> LaunchedBike { get; set; }
    }
}
