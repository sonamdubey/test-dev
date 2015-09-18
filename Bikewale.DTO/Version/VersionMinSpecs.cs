using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.DTO.Version
{
    public class VersionMinSpecs : ModelVersionList
    {   
        [JsonProperty("brakeType")]
        public string BrakeType { get; set; }

        [JsonProperty("alloyWheels")]
        public bool AlloyWheels { get; set; }

        [JsonProperty("electricStart")]
        public bool ElectricStart { get; set; }

        [JsonProperty("antilockBrakingSystem")]
        public bool AntilockBrakingSystem { get; set; }

        //public float Displacement { get; set; }
        //public string TransmissionType { get; set; }
    }
}
