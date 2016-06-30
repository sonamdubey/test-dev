using System;
using System.Runtime.Serialization;

namespace Bikewale.Entities.Compare
{
    [Serializable,DataContract]
    public class BikeSpecification
    {
        [DataMember]
        public uint VersionId { get; set; }
        [DataMember]
        public float? Displacement { get; set; }
        [DataMember]
        public UInt16? Cylinders { get; set; }
        [DataMember]
        public float? MaxPower { get; set; }
        [DataMember]
        public UInt32? MaxPowerRpm { get; set; }
        [DataMember]
        public float? MaximumTorque { get; set; }
        [DataMember]
        public UInt32? MaximumTorqueRpm { get; set; }
        [DataMember]
        public float? Bore { get; set; }
        [DataMember]
        public float? Stroke { get; set; }
        [DataMember]
        public UInt16? ValvesPerCylinder { get; set; }
        [DataMember]
        public string FuelDeliverySystem { get; set; }
        [DataMember]
        public string FuelType { get; set; }
        [DataMember]
        public string Ignition { get; set; }
        [DataMember]
        public string SparkPlugsPerCylinder { get; set; }
        [DataMember]
        public string CoolingSystem { get; set; }
        [DataMember]
        public string GearboxType { get; set; }
        [DataMember]
        public UInt16? NoOfGears { get; set; }
        [DataMember]
        public string TransmissionType { get; set; }
        [DataMember]
        public string Clutch { get; set; }
        [DataMember]
        public float? Performance_0_60_kmph { get; set; }
        [DataMember]
        public float? Performance_0_80_kmph { get; set; }
        [DataMember]
        public float? Performance_0_40_m { get; set; }
        [DataMember]
        public float? TopSpeed { get; set; }
        [DataMember]
        public string Performance_60_0_kmph { get; set; }
        [DataMember]
        public string Performance_80_0_kmph { get; set; }
        [DataMember]
        public UInt16? KerbWeight { get; set; }
        [DataMember]
        public UInt16? OverallLength { get; set; }
        [DataMember]
        public UInt16? OverallWidth { get; set; }
        [DataMember]
        public UInt16? OverallHeight { get; set; }
        [DataMember]
        public UInt16? Wheelbase { get; set; }
        [DataMember]
        public UInt16? GroundClearance { get; set; }
        [DataMember]
        public UInt16? SeatHeight { get; set; }
        [DataMember]
        public float? FuelTankCapacity { get; set; }
        [DataMember]
        public float? ReserveFuelCapacity { get; set; }
        [DataMember]
        public UInt16? FuelEfficiencyOverall { get; set; }
        [DataMember]
        public UInt16? FuelEfficiencyRange { get; set; }
        [DataMember]
        public string ChassisType { get; set; }
        [DataMember]
        public string FrontSuspension { get; set; }
        [DataMember]
        public string RearSuspension { get; set; }
        [DataMember]
        public string BrakeType { get; set; }
        [DataMember]
        public bool? FrontDisc { get; set; }
        [DataMember]
        public UInt16? FrontDisc_DrumSize { get; set; }
        [DataMember]
        public bool? RearDisc { get; set; }
        [DataMember]
        public UInt16? RearDisc_DrumSize { get; set; }
        [DataMember]
        public string CalliperType { get; set; }
        [DataMember]
        public float? WheelSize { get; set; }
        [DataMember]
        public string FrontTyre { get; set; }
        [DataMember]
        public string RearTyre { get; set; }
        [DataMember]
        public bool? TubelessTyres { get; set; }
        [DataMember]
        public bool? RadialTyres { get; set; }
        [DataMember]
        public bool? AlloyWheels { get; set; }
        [DataMember]
        public string ElectricSystem { get; set; }
        [DataMember]
        public string Battery { get; set; }
        [DataMember]
        public string HeadlightType { get; set; }
        [DataMember]
        public string HeadlightBulbType { get; set; }
        [DataMember]
        public string Brake_Tail_Light { get; set; }
        [DataMember]
        public string TurnSignal { get; set; }
        [DataMember]
        public bool? PassLight { get; set; }
    }
}
