using System;
using System.Runtime.Serialization;

namespace Bikewale.Entities.BikeData
{
    [Serializable, DataContract]
    public class MinSpecsEntity
    {
        [DataMember]
        public float? Displacement { get; set; }

        [DataMember]
        public ushort? FuelEfficiencyOverall { get; set; }

        [DataMember]
        public float? MaxPower { get; set; }

        [DataMember]
        public float? MaximumTorque { get; set; }

        [DataMember]
        public UInt16? KerbWeight { get; set; }
    }
}
