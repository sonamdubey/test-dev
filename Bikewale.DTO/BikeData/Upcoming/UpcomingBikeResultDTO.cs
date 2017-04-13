using Bikewale.DTO.BikeData.Upcoming;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Bikewale.DTO.BikeData
{
    /// <summary>
    /// Created by Sajal Gupta on 07-04-2017
    /// Description : Upcoming Bike result dto for api
    /// </summary>
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
