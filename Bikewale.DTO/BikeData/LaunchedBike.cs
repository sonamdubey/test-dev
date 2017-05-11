using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Bikewale.DTO.Model;

namespace Bikewale.DTO.BikeData
{
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
