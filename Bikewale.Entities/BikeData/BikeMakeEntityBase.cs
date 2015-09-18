using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace Bikewale.Entities.BikeData
{
    public class BikeMakeEntityBase
    {
        [JsonProperty(PropertyName = "makeId")]
        public int MakeId { get; set; }

        [JsonProperty(PropertyName = "makeName")]
        public string MakeName { get; set; }

        [JsonProperty(PropertyName = "maskingName")]
        public string MaskingName { get; set; }
    }
}
