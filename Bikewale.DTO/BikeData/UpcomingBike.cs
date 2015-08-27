using Bikewale.DTO.Make;
using Bikewale.DTO.Model;
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
    public class UpcomingBike
    {
        [JsonProperty(PropertyName = "id"), DataMember]
        public uint ExpectedLaunchId { get; set; }

        [JsonProperty(PropertyName = "launchDate"), DataMember]
        public string ExpectedLaunchDate { get; set; }

        [JsonProperty(PropertyName = "minPrice"), DataMember]
        public ulong EstimatedPriceMin { get; set; }

        [JsonProperty(PropertyName = "maxPrice"), DataMember]
        public ulong EstimatedPriceMax { get; set; }

        [JsonProperty(PropertyName = "hostUrl"), DataMember]
        public string HostUrl { get; set; }

        [JsonProperty(PropertyName = "imagePath"), DataMember]
        public string OriginalImagePath { get; set; }

        [JsonProperty(PropertyName = "makeBase"), DataMember]
        public MakeBase MakeBase { get; set; }

        [JsonProperty(PropertyName = "modelBase"), DataMember]
        public ModelBase ModelBase { get; set; }

        [JsonProperty(PropertyName = "bikeDesc"), DataMember]
        public BikeDiscription BikeDescription { get; set; }
    }
}
