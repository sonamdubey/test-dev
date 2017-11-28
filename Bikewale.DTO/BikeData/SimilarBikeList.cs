using Newtonsoft.Json;
using System.Collections.Generic;

namespace Bikewale.DTO.BikeData
{
    public class SimilarBikeList
    {
        [JsonProperty("similarBike")]
        public IEnumerable<SimilarBike> SimilarBike { get; set; }
    }
}
