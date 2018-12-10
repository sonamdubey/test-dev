using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.Geolocation
{
    [Serializable]
    [JsonObject]
    public class Location
    {
        [JsonProperty("StateId")]
        public int StateId { get; set; }
        [JsonProperty("StateName")]
        public string StateName { get; set; }
        [JsonProperty("CityId")]
        public int CityId { get; set; }
        [JsonProperty("CityName")]
        public string CityName { get; set; }
        [JsonProperty("AreaId")]
        public int AreaId { get; set; }
        [JsonProperty("AreaName")]
        public string AreaName { get; set; }
        [JsonProperty("ZoneId")]
        public int ZoneId { get; set; }
        [JsonProperty("ZoneName")]
        public string ZoneName { get; set; }
        [JsonProperty("CityMaskingName")]
        public string CityMaskingName { get; set; }
        [JsonProperty("StateMaskingName")]
        public string StateMaskingName { get; set; }
	}

    //TODO: Resolve zoneid used as string
    public class LocationV2
    {
        public int StateId { get; set; }
        public string StateName { get; set; }
        public int CityId { get; set; }
        public string CityName { get; set; }
        public int AreaId { get; set; }
        public string AreaName { get; set; }
        public string ZoneId { get; set; }
        public string ZoneName { get; set; }
    }
}
