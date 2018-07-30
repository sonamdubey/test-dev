using Newtonsoft.Json;
using System;

namespace Bikewale.DTO.BikeData.v2
{
    /// <summary>
    /// Created By : Sushil Kumar
    /// Created On :20th March 2016
    /// Summary : Created second version to pass json data into proper format  and namingConventions
    /// Modified by :   Sumit Kate on 13 Feb 2017
    /// Description :   Added Kerb weight
    /// </summary>
    public class MinSpecs
    {
        [JsonProperty("displacement")]
        public float Displacement { get; set; }

        [JsonProperty("fuelEffeciency")]
        public ushort FuelEfficiencyOverall { get; set; }

        [JsonProperty("maxPower")]
        public float MaxPower { get; set; }

        [JsonProperty("maxTorque")]
        public float MaximumTorque { get; set; }

        [JsonProperty("kerbWeight")]
        public UInt16 KerbWeight { get; set; }
    }
}
