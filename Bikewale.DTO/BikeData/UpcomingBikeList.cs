using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.DTO.BikeData
{
    [Serializable, DataContract]
    public class UpcomingBikeList
    {
        [JsonProperty(PropertyName = "upcomingBike"), DataMember]
        public IEnumerable<UpcomingBike> UpcomingBike { get; set; }
    }
}
