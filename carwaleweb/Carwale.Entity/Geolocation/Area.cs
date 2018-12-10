using Nest;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.Geolocation
{
    public class Area
    {
        [JsonProperty("areaId")]
        [Number(Store = true)]
        public long areaid { get; set; }
        [JsonProperty("cityId")]
        public long cityid { get; set; }
        [JsonProperty("name")]
        [Keyword]
        public string name { get; set; }
        [JsonProperty("cityName")]
        [Keyword]
        public string cityname { get; set; }
        [JsonProperty("zoneId")]
        public long zoneid { get; set; }
        [JsonProperty("zoneName")]
        [Keyword]
        public string zonename { get; set; }
        [JsonProperty("location")]
        [Keyword]
        public string location { get; set; }
        [JsonProperty("pinCode")] 
        public string pincode { get; set; }
        [JsonProperty("latitude")]
        public double Latitude { get; set; }
        [JsonProperty("longitude")]
        public double Longitude { get; set; }
    }
}
