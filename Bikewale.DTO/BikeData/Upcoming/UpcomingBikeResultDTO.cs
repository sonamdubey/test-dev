using Bikewale.DTO.BikeData.Upcoming;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Bikewale.DTO.BikeData
{
    public class UpcomingBikeResultDTO
    {
        [JsonProperty("bikes")]
        public IEnumerable<UpcomingBikeDTOBase> Bikes { get; set; }
        [JsonProperty("filter")]
        public InputFilterDTO Filter { get; set; }
        [JsonProperty("totalCount")]
        public uint TotalCount { get; set; }
    }
}
