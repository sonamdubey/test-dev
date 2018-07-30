using Newtonsoft.Json;
using System;

namespace Bikewale.DTO.BikeData.v2
{
    /// <summary>
    /// Created By : Pratibha Verma on 30 Mar 2018
    /// </summary>
    [Serializable]
    public class VersionMinSpecs
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("icon")]
        public string Icon { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }

        [JsonProperty("unitType")]
        public string UnitType { get; set; }
    }
}
