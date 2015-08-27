using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Entities.Compare
{
    public class BikeSpecification
    {
        public uint VersionId { get; set; }
        public float? Displacement { get; set; }
        public UInt16? Cylinders { get; set; }
        public float? MaxPower { get; set; }
        public UInt32? MaxPowerRpm { get; set; }
        public float? MaximumTorque { get; set; }
        public UInt32? MaximumTorqueRpm { get; set; }
        public float? Bore { get; set; }
        public float? Stroke { get; set; }
        public UInt16? ValvesPerCylinder { get; set; }
        public string FuelDeliverySystem { get; set; }
        public string FuelType { get; set; }
        public string Ignition { get; set; }
        public string SparkPlugsPerCylinder { get; set; }
        public string CoolingSystem { get; set; }
        public string GearboxType { get; set; }
        public UInt16? NoOfGears { get; set; }
        public string TransmissionType { get; set; }
        public string Clutch { get; set; }
        public float? Performance_0_60_kmph { get; set; }
        public float? Performance_0_80_kmph { get; set; }
        public float? Performance_0_40_m { get; set; }
        public float? TopSpeed { get; set; }
        public string Performance_60_0_kmph { get; set; }
        public string Performance_80_0_kmph { get; set; }
        public UInt16? KerbWeight { get; set; }
        public UInt16? OverallLength { get; set; }
        public UInt16? OverallWidth { get; set; }
        public UInt16? OverallHeight { get; set; }
        public UInt16? Wheelbase { get; set; }
        public UInt16? GroundClearance { get; set; }
        public UInt16? SeatHeight { get; set; }
        public float? FuelTankCapacity { get; set; }
        public float? ReserveFuelCapacity { get; set; }
        public UInt16? FuelEfficiencyOverall { get; set; }
        public UInt16? FuelEfficiencyRange { get; set; }
        public string ChassisType { get; set; }
        public string FrontSuspension { get; set; }
        public string RearSuspension { get; set; }
        public string BrakeType { get; set; }
        public bool? FrontDisc { get; set; }
        public UInt16? FrontDisc_DrumSize { get; set; }
        public bool? RearDisc { get; set; }
        public UInt16? RearDisc_DrumSize { get; set; }
        public string CalliperType { get; set; }
        public float? WheelSize { get; set; }
        public string FrontTyre { get; set; }
        public string RearTyre { get; set; }
        public bool? TubelessTyres { get; set; }
        public bool? RadialTyres { get; set; }
        public bool? AlloyWheels { get; set; }
        public string ElectricSystem { get; set; }
        public string Battery { get; set; }
        public string HeadlightType { get; set; }
        public string HeadlightBulbType { get; set; }
        public string Brake_Tail_Light { get; set; }
        public string TurnSignal { get; set; }
        public bool? PassLight { get; set; }
    }
}
