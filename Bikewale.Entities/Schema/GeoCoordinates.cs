using Newtonsoft.Json;

namespace Bikewale.Entities.Schema
{
    /// <summary>
    /// Created By : Sushil Kumar on 23rd Aug 2017
    /// Description : GeoCoordinates schema for local bussiness and places 
    /// </summary>
    public class GeoCoordinates
    {
        [JsonProperty("@type")]
        public string Type { get { return "GeoCoordinates"; } }

        [JsonProperty("latitude", NullValueHandling = NullValueHandling.Ignore)]
        public double Latitude { get; set; }

        [JsonProperty("longitude", NullValueHandling = NullValueHandling.Ignore)]
        public double Longitude { get; set; }
    }
}
