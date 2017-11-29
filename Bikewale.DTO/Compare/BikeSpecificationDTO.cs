using Newtonsoft.Json;
using System;

namespace Bikewale.DTO.Compare
{
    /// <summary>
    /// Created By  :   Sumit Kate on 22 Jan 2016
    /// Description :   Bike Specification DTO
    /// </summary>
    public class BikeSpecificationDTO
    {
        [JsonProperty("versionId")]
        public uint VersionId { get; set; }
        [JsonProperty("displacement")]
        public float? Displacement { get; set; }
        [JsonProperty("cylinders")]
        public UInt16? Cylinders { get; set; }
        [JsonProperty("maxPower")]
        public float? MaxPower { get; set; }
        [JsonProperty("maxPowerRpm")]
        public UInt32? MaxPowerRpm { get; set; }
        [JsonProperty("maximumTorque")]
        public float? MaximumTorque { get; set; }
        [JsonProperty("maximumTorqueRpm")]
        public UInt32? MaximumTorqueRpm { get; set; }
        [JsonProperty("bore")]
        public float? Bore { get; set; }
        [JsonProperty("stroke")]
        public float? Stroke { get; set; }
        [JsonProperty("valvesPerCylinder")]
        public UInt16? ValvesPerCylinder { get; set; }
        [JsonProperty("fuelDeliverySystem")]
        public string FuelDeliverySystem { get; set; }
        [JsonProperty("fuelType")]
        public string FuelType { get; set; }
        [JsonProperty("ignition")]
        public string Ignition { get; set; }
        [JsonProperty("sparkPlugsPerCylinder")]
        public string SparkPlugsPerCylinder { get; set; }
        [JsonProperty("coolingSystem")]
        public string CoolingSystem { get; set; }
        [JsonProperty("gearboxType")]
        public string GearboxType { get; set; }
        [JsonProperty("noOfGears")]
        public UInt16? NoOfGears { get; set; }
        [JsonProperty("transmissionType")]
        public string TransmissionType { get; set; }
        [JsonProperty("clutch")]
        public string Clutch { get; set; }
        [JsonProperty("performance_0_60_kmph")]
        public float? Performance_0_60_kmph { get; set; }
        [JsonProperty("performance_0_80_kmph")]
        public float? Performance_0_80_kmph { get; set; }
        [JsonProperty("performance_0_40_m")]
        public float? Performance_0_40_m { get; set; }
        [JsonProperty("topSpeed")]
        public float? TopSpeed { get; set; }
        [JsonProperty("performance_60_0_kmph")]
        public string Performance_60_0_kmph { get; set; }
        [JsonProperty("performance_80_0_kmph")]
        public string Performance_80_0_kmph { get; set; }
        [JsonProperty("kerbWeight")]
        public UInt16? KerbWeight { get; set; }
        [JsonProperty("overallLength")]
        public UInt16? OverallLength { get; set; }
        [JsonProperty("overallWidth")]
        public UInt16? OverallWidth { get; set; }
        [JsonProperty("OverallHeight")]
        public UInt16? OverallHeight { get; set; }
        [JsonProperty("wheelbase")]
        public UInt16? Wheelbase { get; set; }
        [JsonProperty("groundClearance")]
        public UInt16? GroundClearance { get; set; }
        [JsonProperty("seatHeight")]
        public UInt16? SeatHeight { get; set; }
        [JsonProperty("fuelTankCapacity")]
        public float? FuelTankCapacity { get; set; }
        [JsonProperty("reserveFuelCapacity")]
        public float? ReserveFuelCapacity { get; set; }
        [JsonProperty("fuelEfficiencyOverall")]
        public UInt16? FuelEfficiencyOverall { get; set; }
        [JsonProperty("fuelEfficiencyRange")]
        public UInt16? FuelEfficiencyRange { get; set; }
        [JsonProperty("chassisType")]
        public string ChassisType { get; set; }
        [JsonProperty("frontSuspension")]
        public string FrontSuspension { get; set; }
        [JsonProperty("rearSuspension")]
        public string RearSuspension { get; set; }
        [JsonProperty("brakeType")]
        public string BrakeType { get; set; }
        [JsonProperty("frontDisc")]
        public bool? FrontDisc { get; set; }
        [JsonProperty("frontDisc_DrumSize")]
        public UInt16? FrontDisc_DrumSize { get; set; }
        [JsonProperty("rearDisc")]
        public bool? RearDisc { get; set; }
        [JsonProperty("rearDisc_DrumSize")]
        public UInt16? RearDisc_DrumSize { get; set; }
        [JsonProperty("calliperType")]
        public string CalliperType { get; set; }
        [JsonProperty("wheelSize")]
        public float? WheelSize { get; set; }
        [JsonProperty("frontTyre")]
        public string FrontTyre { get; set; }
        [JsonProperty("rearTyre")]
        public string RearTyre { get; set; }
        [JsonProperty("tubelessTyres")]
        public bool? TubelessTyres { get; set; }
        [JsonProperty("radialTyres")]
        public bool? RadialTyres { get; set; }
        [JsonProperty("alloyWheels")]
        public bool? AlloyWheels { get; set; }
        [JsonProperty("electricSystem")]
        public string ElectricSystem { get; set; }
        [JsonProperty("battery")]
        public string Battery { get; set; }
        [JsonProperty("headlightType")]
        public string HeadlightType { get; set; }
        [JsonProperty("headlightBulbType")]
        public string HeadlightBulbType { get; set; }
        [JsonProperty("brake_Tail_Light")]
        public string Brake_Tail_Light { get; set; }
        [JsonProperty("turnSignal")]
        public string TurnSignal { get; set; }
        [JsonProperty("passLight")]
        public bool? PassLight { get; set; }
    }
}
