﻿using Bikewale.DTO.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.DTO.Version
{
    public class VersionSpecifications
    {
        public uint BikeVersionId { get; set; }
        public float Displacement { get; set; }
        public ushort Cylinders { get; set; }
        public ushort MaxPower { get; set; }
        public float MaximumTorque { get; set; }
        public float Bore { get; set; }
        public float Stroke { get; set; }
        public ushort ValvesPerCylinder { get; set; }
        public string FuelDeliverySystem { get; set; }
        public string FuelType { get; set; }
        public string Ignition { get; set; }
        public string SparkPlugsPerCylinder { get; set; }
        public string CoolingSystem { get; set; }
        public string GearboxType { get; set; }
        public ushort NoOfGears { get; set; }
        public string TransmissionType { get; set; }
        public string Clutch { get; set; }
        public float Performance_0_60_kmph { get; set; }
        public float Performance_0_80_kmph { get; set; }
        public float Performance_0_40_m { get; set; }
        public uint TopSpeed { get; set; }
        public string Performance_60_0_kmph { get; set; }
        public string Performance_80_0_kmph { get; set; }
        public ushort KerbWeight { get; set; }
        public ushort OverallLength { get; set; }
        public ushort OverallWidth { get; set; }
        public ushort OverallHeight { get; set; }
        public ushort Wheelbase { get; set; }
        public ushort GroundClearance { get; set; }
        public ushort SeatHeight { get; set; }
        public float FuelTankCapacity { get; set; }
        public float ReserveFuelCapacity { get; set; }
        public ushort FuelEfficiencyOverall { get; set; }
        public ushort FuelEfficiencyRange { get; set; }
        public string ChassisType { get; set; }
        public string FrontSuspension { get; set; }
        public string RearSuspension { get; set; }
        public string BrakeType { get; set; }
        public bool FrontDisc { get; set; }
        public ushort FrontDisc_DrumSize { get; set; }
        public bool RearDisc { get; set; }
        public ushort RearDisc_DrumSize { get; set; }
        public string CalliperType { get; set; }
        public float WheelSize { get; set; }
        public string FrontTyre { get; set; }
        public string RearTyre { get; set; }
        public bool TubelessTyres { get; set; }
        public bool RadialTyres { get; set; }
        public bool AlloyWheels { get; set; }
        public string ElectricSystem { get; set; }
        public string Battery { get; set; }
        public string HeadlightType { get; set; }
        public string HeadlightBulbType { get; set; }
        public string Brake_Tail_Light { get; set; }
        public string TurnSignal { get; set; }
        public bool PassLight { get; set; }
        public string Speedometer { get; set; }
        public bool Tachometer { get; set; }
        public string TachometerType { get; set; }
        public bool ShiftLight { get; set; }
        public bool ElectricStart { get; set; }
        public bool Tripmeter { get; set; }
        public string NoOfTripmeters { get; set; }
        public string TripmeterType { get; set; }
        public bool LowFuelIndicator { get; set; }
        public bool LowOilIndicator { get; set; }
        public bool LowBatteryIndicator { get; set; }
        public bool FuelGauge { get; set; }
        public bool DigitalFuelGauge { get; set; }
        public bool PillionSeat { get; set; }
        public bool PillionFootrest { get; set; }
        public bool PillionBackrest { get; set; }
        public bool PillionGrabrail { get; set; }
        public bool StandAlarm { get; set; }
        public bool SteppedSeat { get; set; }
        public bool AntilockBrakingSystem { get; set; }
        public bool Killswitch { get; set; }
        public bool Clock { get; set; }
        public string Colors { get; set; }
        public float MaxPowerRPM { get; set; }
        public float MaximumTorqueRPM { get; set; }
    }
}
