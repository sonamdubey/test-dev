using Newtonsoft.Json;
using System.Collections.Generic;

namespace Bikewale.DTO.BikeData
{
    /// <summary>
    /// created by sajal gupta on 23-05-2017
    /// desc : dto for getverison api.
    /// </summary>
    public class ModelSpecificationsDTO
    {
        [JsonProperty("bodyStyle")]
        public string BodyStyle { get; set; }

        [JsonProperty("maskingName")]
        public string MaskingName { get; set; }

        [JsonProperty("modelId")]
        public uint ModelId { get; set; }

        [JsonProperty("modelName")]
        public string ModelName { get; set; }

        [JsonProperty("ccSegment")]
        public string CCSegment { get; set; }

        [JsonProperty("versions")]
        public IEnumerable<VersionSegmentDTO> Versions { get; set; }
    }
}
