using System;
using System.Runtime.Serialization;

namespace Bikewale.Entities.Compare
{
    [Serializable,DataContract]
    public class BikeFeature
    {
        [DataMember]
        public uint VersionId { get; set; }
        [DataMember]
        public string Speedometer { get; set; }
        [DataMember]
        public bool? Tachometer { get; set; }
        [DataMember]
        public string TachometerType { get; set; }
        [DataMember]
        public bool? ShiftLight { get; set; }
        [DataMember]
        public bool? ElectricStart { get; set; }
        [DataMember]
        public bool? Tripmeter { get; set; }
        [DataMember]
        public string NoOfTripmeters { get; set; }
        [DataMember]
        public string TripmeterType { get; set; }
        [DataMember]
        public bool? LowFuelIndicator { get; set; }
        [DataMember]
        public bool? LowOilIndicator { get; set; }
        [DataMember]
        public bool? LowBatteryIndicator { get; set; }
        [DataMember]
        public bool? FuelGauge { get; set; }
        [DataMember]
        public bool? DigitalFuelGauge { get; set; }
        [DataMember]
        public bool? PillionSeat { get; set; }
        [DataMember]
        public bool? PillionFootrest { get; set; }
        [DataMember]
        public bool? PillionBackrest { get; set; }
        [DataMember]
        public bool? PillionGrabrail { get; set; }
        [DataMember]
        public bool? StandAlarm { get; set; }
        [DataMember]
        public bool? SteppedSeat { get; set; }
        [DataMember]
        public bool? AntilockBrakingSystem { get; set; }
        [DataMember]
        public bool? Killswitch { get; set; }
        [DataMember]
        public bool? Clock { get; set; }        
    }
}
