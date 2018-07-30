using Newtonsoft.Json;
using System.Collections.Generic;

namespace Bikewale.DTO.BikeData.NewLaunched
{
    /// <summary>
    /// Created by  :   Sumit Kate on 13 Feb 2017
    /// Description :   New Launched Bike Result DTO
    /// </summary>
    public class NewLaunchedBikeResultDTO
    {
        [JsonProperty("bikes")]
        public IEnumerable<NewLaunchedBikeDTOBase> Bikes { get; set; }
        [JsonProperty("filter")]
        public InputFilterDTO Filter { get; set; }
        [JsonProperty("totalCount")]
        public uint TotalCount { get; set; }
    }
}
