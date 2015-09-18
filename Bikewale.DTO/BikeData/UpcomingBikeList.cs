using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.DTO.BikeData
{
    public class UpcomingBikeList
    {
        [JsonProperty(PropertyName = "upcomingBike")]
        public IEnumerable<UpcomingBike> UpcomingBike { get; set; }
    }
}
