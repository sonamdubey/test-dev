using System;
using Newtonsoft.Json;

namespace Carwale.DTOs.Geolocation
{
    public class AreaDto
    {
        [JsonProperty("areaId")]
        public int AreaId { get; set; }

        [JsonProperty("areaName")]
        public string AreaName { get; set; }
    }
}
