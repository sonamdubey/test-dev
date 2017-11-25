using Bikewale.DTO.Model;
using Newtonsoft.Json;
using System;

namespace Bikewale.DTO.BikeData
{
    /// <summary>
    /// Modified by : Aditi Srivastava on 11 May 2017
    /// Summary   : Changed launchDate display formatting
    /// </summary>
    public class LaunchedBike : ModelDetail
    {
        [JsonProperty("id")]
        public uint BikeLaunchId { get; set; }
        [JsonIgnore]
        public DateTime LaunchDate { get; set; }
        [JsonProperty("launchDate")]
        public String DisplayLaunchDate { get { return LaunchDate.ToString("dd MMM yyyy"); } }
        [JsonProperty("specs")]
        public MinSpecs Specs { get; set; }
    }
}
