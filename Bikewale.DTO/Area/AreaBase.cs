using Newtonsoft.Json;
using System;

namespace Bikewale.DTO.Area
{
    public class AreaBase
    {
        [JsonProperty("areaId")]
        public UInt32 AreaId { get; set; }

        [JsonProperty("areaName")]
        public string AreaName { get; set; }

        [JsonProperty("areaMaskingName")]
        public string AreaMaskingName { get; set; }
    }
}
