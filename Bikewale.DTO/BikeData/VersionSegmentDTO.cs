using Newtonsoft.Json;

namespace Bikewale.DTO.BikeData
{
    /// <summary>
    /// created by sajal gupta on 23-05-2017
    /// desc : dto for getverison api.
    /// </summary>
    public class VersionSegmentDTO
    {
        [JsonProperty("segment")]
        public string Segment { get; set; }

        [JsonProperty("VersionName")]
        public string VersionName { get; set; }
    }

}
