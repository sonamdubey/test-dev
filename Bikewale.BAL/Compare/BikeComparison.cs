using Bikewale.DAL.Compare;
using Bikewale.Entities.Compare;
using Bikewale.Interfaces.Compare;
using Bikewale.Notifications;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.BAL.Compare
{
    public class BikeComparison : IBikeCompare
    {
        private readonly IBikeCompare _objCompare = null;
        public BikeComparison()
        {
            using (IUnityContainer objPQCont = new UnityContainer())
            {
                objPQCont.RegisterType<IBikeCompare, BikeCompareRepository>();
                _objCompare = objPQCont.Resolve<IBikeCompare>();
            }
        }
        public Entities.Compare.BikeCompareEntity DoCompare(string versions)
        {
            Entities.Compare.BikeCompareEntity compareEntity = null;
            string[] arrVersion = versions.Split(',');
            try
            {
                compareEntity = _objCompare.DoCompare(versions);
                compareEntity.ComapareSpecifications = new CompareMainCategory();
                compareEntity.ComapareSpecifications.Text = "Specs";
                compareEntity.ComapareSpecifications.Value = "Specs";
                compareEntity.ComapareSpecifications.Spec = new List<CompareSubMainCategory>();
                #region Specifications

                CompareSubMainCategory engine = new CompareSubMainCategory();
                engine.Text = "Engine";
                engine.Value = "Engine";
                #region Engine
                engine.SpecCategory = new List<CompareSubCategory>();

                CompareSubCategory displacement = new CompareSubCategory();
                displacement.Text = "Displacement (cc)";
                displacement.Value = "Displacement";
                displacement.CompareSpec = new List<CompareBikeData>();
                foreach (var version in arrVersion)
                {
                    displacement.CompareSpec.Add(new CompareBikeData() { Value = Bikewale.Utility.FormatMinSpecs.ShowAvailable(compareEntity.Specifications.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version)).Displacement.Value.ToString(), "cc") });
                }
                engine.SpecCategory.Add(displacement);


                CompareSubCategory cylinders = new CompareSubCategory();
                cylinders.Text = "Cylinders";
                cylinders.Value = "Cylinders";
                cylinders.CompareSpec = new List<CompareBikeData>();
                foreach (var version in arrVersion)
                {
                    cylinders.CompareSpec.Add(new CompareBikeData() { Value = Bikewale.Utility.FormatMinSpecs.ShowAvailable(compareEntity.Specifications.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version)).Cylinders.Value.ToString()) });
                }
                engine.SpecCategory.Add(cylinders);

                CompareSubCategory maxPower = new CompareSubCategory();
                maxPower.Text = "Max Power";
                maxPower.Value = "Max Power";
                maxPower.CompareSpec = new List<CompareBikeData>();
                foreach (var version in arrVersion)
                {
                    maxPower.CompareSpec.Add(new CompareBikeData() { Value = Bikewale.Utility.FormatMinSpecs.ShowAvailable(compareEntity.Specifications.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version)).MaxPower.Value, "bhp", compareEntity.Specifications.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version)).MaxPowerRpm.Value, "rpm") });
                }
                engine.SpecCategory.Add(maxPower);

                CompareSubCategory MaximumTorque = new CompareSubCategory();
                MaximumTorque.Text = "Maximum Torque";
                MaximumTorque.Value = "Maximum Torque";
                MaximumTorque.CompareSpec = new List<CompareBikeData>();
                foreach (var version in arrVersion)
                {
                    MaximumTorque.CompareSpec.Add(new CompareBikeData() { Value = Bikewale.Utility.FormatMinSpecs.ShowAvailable(compareEntity.Specifications.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version)).MaximumTorque.Value, "Nm", compareEntity.Specifications.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version)).MaximumTorqueRpm.Value, "rpm") });
                }
                engine.SpecCategory.Add(MaximumTorque);

                CompareSubCategory Bore = new CompareSubCategory();
                Bore.Text = "Bore (mm)";
                Bore.Value = "Bore (mm)";
                Bore.CompareSpec = new List<CompareBikeData>();
                foreach (var version in arrVersion)
                {
                    Bore.CompareSpec.Add(new CompareBikeData() { Value = Bikewale.Utility.FormatMinSpecs.ShowAvailable(compareEntity.Specifications.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version)).MaximumTorque.Value, "Nm", compareEntity.Specifications.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version)).Bore.Value, "mm") });
                }
                engine.SpecCategory.Add(Bore);

                CompareSubCategory Stroke = new CompareSubCategory();
                Stroke.Text = "Stroke (mm)";
                Stroke.Value = "Stroke (mm)";
                Stroke.CompareSpec = new List<CompareBikeData>();
                foreach (var version in arrVersion)
                {
                    Stroke.CompareSpec.Add(new CompareBikeData() { Value = Bikewale.Utility.FormatMinSpecs.ShowAvailable(compareEntity.Specifications.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version)).MaximumTorque.Value, "Nm", compareEntity.Specifications.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version)).Stroke.Value, "mm") });
                }
                engine.SpecCategory.Add(Stroke);

                CompareSubCategory ValvesPerCylinder = new CompareSubCategory();
                ValvesPerCylinder.Text = "Valves Per Cylinder";
                ValvesPerCylinder.Value = "Valves Per Cylinder";
                ValvesPerCylinder.CompareSpec = new List<CompareBikeData>();
                foreach (var version in arrVersion)
                {
                    ValvesPerCylinder.CompareSpec.Add(new CompareBikeData() { Value = Bikewale.Utility.FormatMinSpecs.ShowAvailable(compareEntity.Specifications.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version)).ValvesPerCylinder.Value) });
                }
                engine.SpecCategory.Add(ValvesPerCylinder);

                CompareSubCategory fuelDeliverySystem = new CompareSubCategory();
                fuelDeliverySystem.Text = "Fuel Delivery System";
                fuelDeliverySystem.Value = "Fuel Delivery System";
                fuelDeliverySystem.CompareSpec = new List<CompareBikeData>();
                foreach (var version in arrVersion)
                {
                    fuelDeliverySystem.CompareSpec.Add(new CompareBikeData() { Value = Bikewale.Utility.FormatMinSpecs.ShowAvailable(compareEntity.Specifications.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version)).FuelDeliverySystem) });
                }
                engine.SpecCategory.Add(fuelDeliverySystem);

                CompareSubCategory FuelType = new CompareSubCategory();
                FuelType.Text = "Fuel Type";
                FuelType.Value = "Fuel Type";
                FuelType.CompareSpec = new List<CompareBikeData>();
                foreach (var version in arrVersion)
                {
                    FuelType.CompareSpec.Add(new CompareBikeData() { Value = Bikewale.Utility.FormatMinSpecs.ShowAvailable(compareEntity.Specifications.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version)).FuelType) });
                }
                engine.SpecCategory.Add(FuelType);

                CompareSubCategory Ignition = new CompareSubCategory();
                Ignition.Text = "Ignition";
                Ignition.Value = "Ignition";
                Ignition.CompareSpec = new List<CompareBikeData>();
                foreach (var version in arrVersion)
                {
                    Ignition.CompareSpec.Add(new CompareBikeData() { Value = Bikewale.Utility.FormatMinSpecs.ShowAvailable(compareEntity.Specifications.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version)).Ignition) });
                }
                engine.SpecCategory.Add(Ignition);

                CompareSubCategory SparkPlugs = new CompareSubCategory();
                SparkPlugs.Text = "Spark Plugs (Per Cylinder)";
                SparkPlugs.Value = "Spark Plugs (Per Cylinder)";
                SparkPlugs.CompareSpec = new List<CompareBikeData>();
                foreach (var version in arrVersion)
                {
                    SparkPlugs.CompareSpec.Add(new CompareBikeData() { Value = Bikewale.Utility.FormatMinSpecs.ShowAvailable(compareEntity.Specifications.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version)).SparkPlugsPerCylinder) });
                }
                engine.SpecCategory.Add(SparkPlugs);

                CompareSubCategory coolingSystem = new CompareSubCategory();
                coolingSystem.Text = "Cooling System";
                coolingSystem.Value = "Cooling System";
                coolingSystem.CompareSpec = new List<CompareBikeData>();
                foreach (var version in arrVersion)
                {
                    coolingSystem.CompareSpec.Add(new CompareBikeData() { Value = Bikewale.Utility.FormatMinSpecs.ShowAvailable(compareEntity.Specifications.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version)).CoolingSystem) });
                }
                engine.SpecCategory.Add(coolingSystem);
                #endregion
                compareEntity.ComapareSpecifications.Spec.Add(engine);

                CompareSubMainCategory transmission = new CompareSubMainCategory();
                transmission.Text = "Transmission";
                transmission.Value = "Transmission";
                #region Transmission
                transmission.SpecCategory = new List<CompareSubCategory>();

                CompareSubCategory gearBox = new CompareSubCategory();
                gearBox.Text = "Gearbox Type";
                gearBox.Value = "Gearbox Type";
                gearBox.CompareSpec = new List<CompareBikeData>();
                foreach (var version in arrVersion)
                {
                    gearBox.CompareSpec.Add(new CompareBikeData() { Value = Bikewale.Utility.FormatMinSpecs.ShowAvailable(compareEntity.Specifications.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version)).GearboxType) });
                }
                transmission.SpecCategory.Add(gearBox);


                CompareSubCategory NoOfGears = new CompareSubCategory();
                NoOfGears.Text = "No Of Gears";
                NoOfGears.Value = "No Of Gears";
                NoOfGears.CompareSpec = new List<CompareBikeData>();
                foreach (var version in arrVersion)
                {
                    NoOfGears.CompareSpec.Add(new CompareBikeData() { Value = Bikewale.Utility.FormatMinSpecs.ShowAvailable(compareEntity.Specifications.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version)).NoOfGears.Value) });
                }
                transmission.SpecCategory.Add(NoOfGears);

                CompareSubCategory tGearboxType = new CompareSubCategory();
                tGearboxType.Text = "Gearbox Type";
                tGearboxType.Value = "Gearbox Type";
                tGearboxType.CompareSpec = new List<CompareBikeData>();
                foreach (var version in arrVersion)
                {
                    tGearboxType.CompareSpec.Add(new CompareBikeData() { Value = Bikewale.Utility.FormatMinSpecs.ShowAvailable(compareEntity.Specifications.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version)).GearboxType) });
                }
                transmission.SpecCategory.Add(tGearboxType);

                CompareSubCategory tNoOfGears = new CompareSubCategory();
                tNoOfGears.Text = "No Of Gears";
                tNoOfGears.Value = "No Of Gears";
                tNoOfGears.CompareSpec = new List<CompareBikeData>();
                foreach (var version in arrVersion)
                {
                    tNoOfGears.CompareSpec.Add(new CompareBikeData() { Value = Bikewale.Utility.FormatMinSpecs.ShowAvailable(compareEntity.Specifications.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version)).NoOfGears.Value) });
                }
                transmission.SpecCategory.Add(tNoOfGears);

                CompareSubCategory TransmissionType = new CompareSubCategory();
                TransmissionType.Text = "Transmission Type";
                TransmissionType.Value = "Transmission Type";
                TransmissionType.CompareSpec = new List<CompareBikeData>();
                foreach (var version in arrVersion)
                {
                    TransmissionType.CompareSpec.Add(new CompareBikeData() { Value = Bikewale.Utility.FormatMinSpecs.ShowAvailable(compareEntity.Specifications.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version)).TransmissionType) });
                }
                transmission.SpecCategory.Add(TransmissionType);

                CompareSubCategory Clutch = new CompareSubCategory();
                Clutch.Text = "Clutch";
                Clutch.Value = "Clutch";
                Clutch.CompareSpec = new List<CompareBikeData>();
                foreach (var version in arrVersion)
                {
                    Clutch.CompareSpec.Add(new CompareBikeData() { Value = Bikewale.Utility.FormatMinSpecs.ShowAvailable(compareEntity.Specifications.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version)).Clutch) });
                }
                transmission.SpecCategory.Add(Clutch);
                #endregion
                compareEntity.ComapareSpecifications.Spec.Add(transmission);


                CompareSubMainCategory performance = new CompareSubMainCategory();
                performance.Text = "Performance";
                performance.Value = "Performance";
                #region Performance
                performance.SpecCategory = new List<CompareSubCategory>();

                CompareSubCategory Performance_0_60_kmph = new CompareSubCategory();
                Performance_0_60_kmph.Text = "0 to 60 kmph (Seconds)";
                Performance_0_60_kmph.Value = "0 to 60 kmph (Seconds)";
                Performance_0_60_kmph.CompareSpec = new List<CompareBikeData>();
                foreach (var version in arrVersion)
                {
                    Performance_0_60_kmph.CompareSpec.Add(new CompareBikeData() { Value = Bikewale.Utility.FormatMinSpecs.ShowAvailable(compareEntity.Specifications.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version)).Performance_0_60_kmph.Value) });
                }
                performance.SpecCategory.Add(Performance_0_60_kmph);


                CompareSubCategory Performance_0_80_kmph = new CompareSubCategory();
                Performance_0_80_kmph.Text = "0 to 80 kmph (Seconds)";
                Performance_0_80_kmph.Value = "0 to 80 kmph (Seconds)";
                Performance_0_80_kmph.CompareSpec = new List<CompareBikeData>();
                foreach (var version in arrVersion)
                {
                    Performance_0_80_kmph.CompareSpec.Add(new CompareBikeData() { Value = Bikewale.Utility.FormatMinSpecs.ShowAvailable(compareEntity.Specifications.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version)).Performance_0_80_kmph.Value) });
                }
                performance.SpecCategory.Add(Performance_0_80_kmph);

                CompareSubCategory Performance_0_40_m = new CompareSubCategory();
                Performance_0_40_m.Text = "0 to 40 m (Seconds)";
                Performance_0_40_m.Value = "0 to 40 m (Seconds)";
                Performance_0_40_m.CompareSpec = new List<CompareBikeData>();
                foreach (var version in arrVersion)
                {
                    Performance_0_40_m.CompareSpec.Add(new CompareBikeData() { Value = Bikewale.Utility.FormatMinSpecs.ShowAvailable(compareEntity.Specifications.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version)).Performance_0_40_m.Value) });
                }
                performance.SpecCategory.Add(Performance_0_40_m);

                CompareSubCategory TopSpeed = new CompareSubCategory();
                TopSpeed.Text = "Top Speed (Kmph)";
                TopSpeed.Value = "Top Speed (Kmph)";
                TopSpeed.CompareSpec = new List<CompareBikeData>();
                foreach (var version in arrVersion)
                {
                    TopSpeed.CompareSpec.Add(new CompareBikeData() { Value = Bikewale.Utility.FormatMinSpecs.ShowAvailable(compareEntity.Specifications.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version)).TopSpeed.Value) });
                }
                performance.SpecCategory.Add(TopSpeed);

                CompareSubCategory Performance_60_0_kmph = new CompareSubCategory();
                Performance_60_0_kmph.Text = "60 to 0 Kmph (Seconds, metres)";
                Performance_60_0_kmph.Value = "60 to 0 Kmph (Seconds, metres)";
                Performance_60_0_kmph.CompareSpec = new List<CompareBikeData>();
                foreach (var version in arrVersion)
                {
                    Performance_60_0_kmph.CompareSpec.Add(new CompareBikeData() { Value = Bikewale.Utility.FormatMinSpecs.ShowAvailable(compareEntity.Specifications.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version)).Performance_60_0_kmph) });
                }
                performance.SpecCategory.Add(Performance_60_0_kmph);

                CompareSubCategory Performance_80_0_kmph = new CompareSubCategory();
                Performance_80_0_kmph.Text = "80 to 0 kmph (Seconds, metres)";
                Performance_80_0_kmph.Value = "80 to 0 kmph (Seconds, metres)";
                Performance_80_0_kmph.CompareSpec = new List<CompareBikeData>();
                foreach (var version in arrVersion)
                {
                    Performance_80_0_kmph.CompareSpec.Add(new CompareBikeData() { Value = Bikewale.Utility.FormatMinSpecs.ShowAvailable(compareEntity.Specifications.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version)).Performance_80_0_kmph) });
                }
                performance.SpecCategory.Add(Performance_80_0_kmph);
                #endregion
                compareEntity.ComapareSpecifications.Spec.Add(performance);


                CompareSubMainCategory dimensionsWeight = new CompareSubMainCategory();
                dimensionsWeight.Text = "Dimensions & Weight";
                dimensionsWeight.Value = "Dimensions & Weight";
                #region Dimensions & Weight
                dimensionsWeight.SpecCategory = new List<CompareSubCategory>();

                CompareSubCategory KerbWeight = new CompareSubCategory();
                KerbWeight.Text = "Kerb Weight (Kg)";
                KerbWeight.Value = "Kerb Weight (Kg)";
                KerbWeight.CompareSpec = new List<CompareBikeData>();
                foreach (var version in arrVersion)
                {
                    KerbWeight.CompareSpec.Add(new CompareBikeData() { Value = Bikewale.Utility.FormatMinSpecs.ShowAvailable(compareEntity.Specifications.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version)).KerbWeight.Value) });
                }
                dimensionsWeight.SpecCategory.Add(KerbWeight);


                CompareSubCategory OverallLength = new CompareSubCategory();
                OverallLength.Text = "Overall Length (mm)";
                OverallLength.Value = "Overall Length (mm)";
                OverallLength.CompareSpec = new List<CompareBikeData>();
                foreach (var version in arrVersion)
                {
                    OverallLength.CompareSpec.Add(new CompareBikeData() { Value = Bikewale.Utility.FormatMinSpecs.ShowAvailable(compareEntity.Specifications.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version)).OverallLength.Value) });
                }
                dimensionsWeight.SpecCategory.Add(KerbWeight);

                CompareSubCategory OverallWidth = new CompareSubCategory();
                OverallWidth.Text = "Overall Width (mm)";
                OverallWidth.Value = "Overall Width (mm)";
                OverallWidth.CompareSpec = new List<CompareBikeData>();
                foreach (var version in arrVersion)
                {
                    OverallWidth.CompareSpec.Add(new CompareBikeData() { Value = Bikewale.Utility.FormatMinSpecs.ShowAvailable(compareEntity.Specifications.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version)).OverallWidth.Value) });
                }
                dimensionsWeight.SpecCategory.Add(OverallWidth);

                CompareSubCategory OverallHeight = new CompareSubCategory();
                OverallHeight.Text = "Overall Height (mm)";
                OverallHeight.Value = "Overall Height (mm)";
                OverallHeight.CompareSpec = new List<CompareBikeData>();
                foreach (var version in arrVersion)
                {
                    OverallHeight.CompareSpec.Add(new CompareBikeData() { Value = Bikewale.Utility.FormatMinSpecs.ShowAvailable(compareEntity.Specifications.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version)).OverallHeight.Value) });
                }
                dimensionsWeight.SpecCategory.Add(OverallHeight);

                CompareSubCategory Wheelbase = new CompareSubCategory();
                Wheelbase.Text = "Wheelbase (mm)";
                Wheelbase.Value = "Wheelbase (mm)";
                Wheelbase.CompareSpec = new List<CompareBikeData>();
                foreach (var version in arrVersion)
                {
                    Wheelbase.CompareSpec.Add(new CompareBikeData() { Value = Bikewale.Utility.FormatMinSpecs.ShowAvailable(compareEntity.Specifications.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version)).Wheelbase.Value) });
                }
                dimensionsWeight.SpecCategory.Add(Wheelbase);

                CompareSubCategory GroundClearance = new CompareSubCategory();
                GroundClearance.Text = "Ground Clearance (mm)";
                GroundClearance.Value = "Ground Clearance (mm)";
                GroundClearance.CompareSpec = new List<CompareBikeData>();
                foreach (var version in arrVersion)
                {
                    GroundClearance.CompareSpec.Add(new CompareBikeData() { Value = Bikewale.Utility.FormatMinSpecs.ShowAvailable(compareEntity.Specifications.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version)).GroundClearance.Value) });
                }
                dimensionsWeight.SpecCategory.Add(GroundClearance);

                CompareSubCategory SeatHeight = new CompareSubCategory();
                SeatHeight.Text = "Seat Height (mm)";
                SeatHeight.Value = "Seat Height (mm)";
                SeatHeight.CompareSpec = new List<CompareBikeData>();
                foreach (var version in arrVersion)
                {
                    SeatHeight.CompareSpec.Add(new CompareBikeData() { Value = Bikewale.Utility.FormatMinSpecs.ShowAvailable(compareEntity.Specifications.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version)).SeatHeight.Value) });
                }
                dimensionsWeight.SpecCategory.Add(SeatHeight);
                #endregion
                compareEntity.ComapareSpecifications.Spec.Add(dimensionsWeight);


                CompareSubMainCategory fuelEfficiencyRange = new CompareSubMainCategory();
                fuelEfficiencyRange.Text = "Fuel Efficiency & Range";
                fuelEfficiencyRange.Value = "Fuel Efficiency & Range";
                #region Dimensions & Weight
                fuelEfficiencyRange.SpecCategory = new List<CompareSubCategory>();

                CompareSubCategory FuelTankCapacity = new CompareSubCategory();
                FuelTankCapacity.Text = "Fuel Tank Capacity (Litres)";
                FuelTankCapacity.Value = "Fuel Tank Capacity (Litres)";
                FuelTankCapacity.CompareSpec = new List<CompareBikeData>();
                foreach (var version in arrVersion)
                {
                    FuelTankCapacity.CompareSpec.Add(new CompareBikeData() { Value = Bikewale.Utility.FormatMinSpecs.ShowAvailable(compareEntity.Specifications.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version)).FuelTankCapacity.Value) });
                }
                fuelEfficiencyRange.SpecCategory.Add(FuelTankCapacity);

                CompareSubCategory ReserveFuelCapacity = new CompareSubCategory();
                ReserveFuelCapacity.Text = "Reserve Fuel Capacity (Litres)";
                ReserveFuelCapacity.Value = "Reserve Fuel Capacity (Litres)";
                ReserveFuelCapacity.CompareSpec = new List<CompareBikeData>();
                foreach (var version in arrVersion)
                {
                    ReserveFuelCapacity.CompareSpec.Add(new CompareBikeData() { Value = Bikewale.Utility.FormatMinSpecs.ShowAvailable(compareEntity.Specifications.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version)).ReserveFuelCapacity.Value) });
                }
                fuelEfficiencyRange.SpecCategory.Add(ReserveFuelCapacity);

                CompareSubCategory FuelEfficiencyOverall = new CompareSubCategory();
                FuelEfficiencyOverall.Text = "FuelEfficiency Overall (Kmpl)";
                FuelEfficiencyOverall.Value = "FuelEfficiency Overall (Kmpl)";
                FuelEfficiencyOverall.CompareSpec = new List<CompareBikeData>();
                foreach (var version in arrVersion)
                {
                    FuelEfficiencyOverall.CompareSpec.Add(new CompareBikeData() { Value = Bikewale.Utility.FormatMinSpecs.ShowAvailable(compareEntity.Specifications.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version)).FuelEfficiencyOverall.Value) });
                }
                fuelEfficiencyRange.SpecCategory.Add(FuelEfficiencyOverall);

                CompareSubCategory FuelEfficiencyRange = new CompareSubCategory();
                FuelEfficiencyRange.Text = "Fuel Efficiency Range (Km)";
                FuelEfficiencyRange.Value = "Fuel Efficiency Range (Km)";
                FuelEfficiencyRange.CompareSpec = new List<CompareBikeData>();
                foreach (var version in arrVersion)
                {
                    FuelEfficiencyRange.CompareSpec.Add(new CompareBikeData() { Value = Bikewale.Utility.FormatMinSpecs.ShowAvailable(compareEntity.Specifications.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version)).FuelEfficiencyRange.Value) });
                }
                fuelEfficiencyRange.SpecCategory.Add(FuelEfficiencyRange);
                #endregion
                compareEntity.ComapareSpecifications.Spec.Add(fuelEfficiencyRange);

                CompareSubMainCategory chassisType = new CompareSubMainCategory();
                chassisType.Text = "Chassis & Suspension";
                chassisType.Value = "Chassis & Suspension";
                #region Chassis & Suspension
                chassisType.SpecCategory = new List<CompareSubCategory>();

                CompareSubCategory ChassisType = new CompareSubCategory();
                ChassisType.Text = "Chassis Type";
                ChassisType.Value = "Chassis Type";
                ChassisType.CompareSpec = new List<CompareBikeData>();
                foreach (var version in arrVersion)
                {
                    ChassisType.CompareSpec.Add(new CompareBikeData() { Value = Bikewale.Utility.FormatMinSpecs.ShowAvailable(compareEntity.Specifications.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version)).ChassisType) });
                }
                chassisType.SpecCategory.Add(ChassisType);

                CompareSubCategory FrontSuspension = new CompareSubCategory();
                FrontSuspension.Text = "Front Suspension";
                FrontSuspension.Value = "Front Suspension";
                FrontSuspension.CompareSpec = new List<CompareBikeData>();
                foreach (var version in arrVersion)
                {
                    FrontSuspension.CompareSpec.Add(new CompareBikeData() { Value = Bikewale.Utility.FormatMinSpecs.ShowAvailable(compareEntity.Specifications.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version)).FrontSuspension) });
                }
                chassisType.SpecCategory.Add(FrontSuspension);

                CompareSubCategory RearSuspension = new CompareSubCategory();
                RearSuspension.Text = "Rear Suspension";
                RearSuspension.Value = "Rear Suspension";
                RearSuspension.CompareSpec = new List<CompareBikeData>();
                foreach (var version in arrVersion)
                {
                    RearSuspension.CompareSpec.Add(new CompareBikeData() { Value = Bikewale.Utility.FormatMinSpecs.ShowAvailable(compareEntity.Specifications.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version)).RearSuspension) });
                }
                chassisType.SpecCategory.Add(RearSuspension);
                #endregion
                compareEntity.ComapareSpecifications.Spec.Add(chassisType);


                CompareSubMainCategory braking = new CompareSubMainCategory();
                braking.Text = "Braking";
                braking.Value = "Braking";
                #region Chassis & Suspension
                braking.SpecCategory = new List<CompareSubCategory>();

                CompareSubCategory breakType = new CompareSubCategory();
                breakType.Text = "Brake Type";
                breakType.Value = "Brake Type";
                breakType.CompareSpec = new List<CompareBikeData>();
                foreach (var version in arrVersion)
                {
                    breakType.CompareSpec.Add(new CompareBikeData() { Value = Bikewale.Utility.FormatMinSpecs.ShowAvailable(compareEntity.Specifications.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version)).BrakeType) });
                }
                braking.SpecCategory.Add(breakType);

                CompareSubCategory FrontDisc = new CompareSubCategory();
                FrontDisc.Text = "Front Disc";
                FrontDisc.Value = "Front Disc";
                FrontDisc.CompareSpec = new List<CompareBikeData>();
                foreach (var version in arrVersion)
                {
                    FrontDisc.CompareSpec.Add(new CompareBikeData() { Value = Bikewale.Utility.FormatMinSpecs.ShowAvailable(compareEntity.Specifications.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version)).FrontDisc.Value) });
                }
                braking.SpecCategory.Add(FrontDisc);

                CompareSubCategory FrontDisc_DrumSize = new CompareSubCategory();
                FrontDisc_DrumSize.Text = "Front Disc/Drum Size (mm)";
                FrontDisc_DrumSize.Value = "Front Disc/Drum Size (mm)";
                FrontDisc_DrumSize.CompareSpec = new List<CompareBikeData>();
                foreach (var version in arrVersion)
                {
                    FrontDisc_DrumSize.CompareSpec.Add(new CompareBikeData() { Value = Bikewale.Utility.FormatMinSpecs.ShowAvailable(compareEntity.Specifications.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version)).FrontDisc_DrumSize.Value) });
                }
                braking.SpecCategory.Add(FrontDisc_DrumSize);

                CompareSubCategory RearDisc = new CompareSubCategory();
                RearDisc.Text = "Rear Disc";
                RearDisc.Value = "Rear Disc";
                RearDisc.CompareSpec = new List<CompareBikeData>();
                foreach (var version in arrVersion)
                {
                    RearDisc.CompareSpec.Add(new CompareBikeData() { Value = Bikewale.Utility.FormatMinSpecs.ShowAvailable(compareEntity.Specifications.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version)).RearDisc.Value) });
                }
                braking.SpecCategory.Add(RearDisc);

                CompareSubCategory RearDisc_DrumSize = new CompareSubCategory();
                RearDisc_DrumSize.Text = "Rear Disc/Drum Size (mm)";
                RearDisc_DrumSize.Value = "Rear Disc/Drum Size (mm)";
                RearDisc_DrumSize.CompareSpec = new List<CompareBikeData>();
                foreach (var version in arrVersion)
                {
                    RearDisc_DrumSize.CompareSpec.Add(new CompareBikeData() { Value = Bikewale.Utility.FormatMinSpecs.ShowAvailable(compareEntity.Specifications.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version)).RearDisc_DrumSize.Value) });
                }
                braking.SpecCategory.Add(RearDisc_DrumSize);

                CompareSubCategory CalliperType = new CompareSubCategory();
                CalliperType.Text = "Calliper Type";
                CalliperType.Value = "Calliper Type";
                CalliperType.CompareSpec = new List<CompareBikeData>();
                foreach (var version in arrVersion)
                {
                    CalliperType.CompareSpec.Add(new CompareBikeData() { Value = Bikewale.Utility.FormatMinSpecs.ShowAvailable(compareEntity.Specifications.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version)).CalliperType) });
                }
                braking.SpecCategory.Add(CalliperType);
                #endregion
                compareEntity.ComapareSpecifications.Spec.Add(braking);

                CompareSubMainCategory wheelsTyres = new CompareSubMainCategory();
                wheelsTyres.Text = "Wheels & Tyres";
                wheelsTyres.Value = "Wheels & Tyres";
                #region Wheels & Tyres
                wheelsTyres.SpecCategory = new List<CompareSubCategory>();

                CompareSubCategory WheelSize = new CompareSubCategory();
                WheelSize.Text = "Wheel Size (inches)";
                WheelSize.Value = "Wheel Size (inches)";
                WheelSize.CompareSpec = new List<CompareBikeData>();
                foreach (var version in arrVersion)
                {
                    WheelSize.CompareSpec.Add(new CompareBikeData() { Value = Bikewale.Utility.FormatMinSpecs.ShowAvailable(compareEntity.Specifications.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version)).WheelSize.Value) });
                }
                wheelsTyres.SpecCategory.Add(WheelSize);

                CompareSubCategory FrontTyre = new CompareSubCategory();
                FrontTyre.Text = "Front Tyre";
                FrontTyre.Value = "Front Tyre";
                FrontTyre.CompareSpec = new List<CompareBikeData>();
                foreach (var version in arrVersion)
                {
                    FrontTyre.CompareSpec.Add(new CompareBikeData() { Value = Bikewale.Utility.FormatMinSpecs.ShowAvailable(compareEntity.Specifications.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version)).FrontTyre) });
                }
                wheelsTyres.SpecCategory.Add(FrontTyre);

                CompareSubCategory RearTyre = new CompareSubCategory();
                RearTyre.Text = "Rear Tyre";
                RearTyre.Value = "Rear Tyre";
                RearTyre.CompareSpec = new List<CompareBikeData>();
                foreach (var version in arrVersion)
                {
                    RearTyre.CompareSpec.Add(new CompareBikeData() { Value = Bikewale.Utility.FormatMinSpecs.ShowAvailable(compareEntity.Specifications.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version)).RearTyre) });
                }
                wheelsTyres.SpecCategory.Add(RearTyre);

                CompareSubCategory TubelessTyres = new CompareSubCategory();
                TubelessTyres.Text = "Tubeless Tyres";
                TubelessTyres.Value = "Tubeless Tyres";
                TubelessTyres.CompareSpec = new List<CompareBikeData>();
                foreach (var version in arrVersion)
                {
                    TubelessTyres.CompareSpec.Add(new CompareBikeData() { Value = Bikewale.Utility.FormatMinSpecs.ShowAvailable(compareEntity.Specifications.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version)).TubelessTyres.Value) });
                }
                wheelsTyres.SpecCategory.Add(TubelessTyres);

                CompareSubCategory RadialTyres = new CompareSubCategory();
                RadialTyres.Text = "Radial Tyres";
                RadialTyres.Value = "Radial Tyres";
                RadialTyres.CompareSpec = new List<CompareBikeData>();
                foreach (var version in arrVersion)
                {
                    RadialTyres.CompareSpec.Add(new CompareBikeData() { Value = Bikewale.Utility.FormatMinSpecs.ShowAvailable(compareEntity.Specifications.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version)).RadialTyres.Value) });
                }
                wheelsTyres.SpecCategory.Add(RadialTyres);

                CompareSubCategory AlloyWheels = new CompareSubCategory();
                AlloyWheels.Text = "Alloy Wheels";
                AlloyWheels.Value = "Alloy Wheels";
                AlloyWheels.CompareSpec = new List<CompareBikeData>();
                foreach (var version in arrVersion)
                {
                    AlloyWheels.CompareSpec.Add(new CompareBikeData() { Value = Bikewale.Utility.FormatMinSpecs.ShowAvailable(compareEntity.Specifications.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version)).AlloyWheels.Value) });
                }
                wheelsTyres.SpecCategory.Add(AlloyWheels);
                #endregion
                compareEntity.ComapareSpecifications.Spec.Add(wheelsTyres);


                CompareSubMainCategory electricals = new CompareSubMainCategory();
                electricals.Text = "Electricals";
                electricals.Value = "Electricals";
                #region Wheels & Tyres
                electricals.SpecCategory = new List<CompareSubCategory>();

                CompareSubCategory ElectricSystem = new CompareSubCategory();
                ElectricSystem.Text = "Electric System";
                ElectricSystem.Value = "Electric System";
                ElectricSystem.CompareSpec = new List<CompareBikeData>();
                foreach (var version in arrVersion)
                {
                    ElectricSystem.CompareSpec.Add(new CompareBikeData() { Value = Bikewale.Utility.FormatMinSpecs.ShowAvailable(compareEntity.Specifications.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version)).ElectricSystem) });
                }
                electricals.SpecCategory.Add(ElectricSystem);

                CompareSubCategory Battery = new CompareSubCategory();
                Battery.Text = "Battery";
                Battery.Value = "Battery";
                Battery.CompareSpec = new List<CompareBikeData>();
                foreach (var version in arrVersion)
                {
                    Battery.CompareSpec.Add(new CompareBikeData() { Value = Bikewale.Utility.FormatMinSpecs.ShowAvailable(compareEntity.Specifications.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version)).Battery) });
                }
                electricals.SpecCategory.Add(ElectricSystem);

                CompareSubCategory HeadlightType = new CompareSubCategory();
                HeadlightType.Text = "Headlight Type";
                HeadlightType.Value = "Headlight Type";
                HeadlightType.CompareSpec = new List<CompareBikeData>();
                foreach (var version in arrVersion)
                {
                    HeadlightType.CompareSpec.Add(new CompareBikeData() { Value = Bikewale.Utility.FormatMinSpecs.ShowAvailable(compareEntity.Specifications.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version)).HeadlightType) });
                }
                electricals.SpecCategory.Add(HeadlightType);

                CompareSubCategory HeadlightBulbType = new CompareSubCategory();
                HeadlightBulbType.Text = "Headlight Bulb Type";
                HeadlightBulbType.Value = "Headlight Bulb Type";
                HeadlightBulbType.CompareSpec = new List<CompareBikeData>();
                foreach (var version in arrVersion)
                {
                    HeadlightBulbType.CompareSpec.Add(new CompareBikeData() { Value = Bikewale.Utility.FormatMinSpecs.ShowAvailable(compareEntity.Specifications.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version)).HeadlightBulbType) });
                }
                electricals.SpecCategory.Add(HeadlightBulbType);

                CompareSubCategory Brake_Tail_Light = new CompareSubCategory();
                Brake_Tail_Light.Text = "Brake/Tail Light";
                Brake_Tail_Light.Value = "Brake/Tail Light";
                Brake_Tail_Light.CompareSpec = new List<CompareBikeData>();
                foreach (var version in arrVersion)
                {
                    Brake_Tail_Light.CompareSpec.Add(new CompareBikeData() { Value = Bikewale.Utility.FormatMinSpecs.ShowAvailable(compareEntity.Specifications.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version)).Brake_Tail_Light) });
                }
                electricals.SpecCategory.Add(Brake_Tail_Light);

                CompareSubCategory TurnSignal = new CompareSubCategory();
                TurnSignal.Text = "Turn Signal";
                TurnSignal.Value = "Turn Signal";
                TurnSignal.CompareSpec = new List<CompareBikeData>();
                foreach (var version in arrVersion)
                {
                    TurnSignal.CompareSpec.Add(new CompareBikeData() { Value = Bikewale.Utility.FormatMinSpecs.ShowAvailable(compareEntity.Specifications.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version)).TurnSignal) });
                }
                electricals.SpecCategory.Add(TurnSignal);

                CompareSubCategory PassLight = new CompareSubCategory();
                PassLight.Text = "Pass Light";
                PassLight.Value = "Pass Light";
                PassLight.CompareSpec = new List<CompareBikeData>();
                foreach (var version in arrVersion)
                {
                    PassLight.CompareSpec.Add(new CompareBikeData() { Value = Bikewale.Utility.FormatMinSpecs.ShowAvailable(compareEntity.Specifications.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version)).PassLight.Value) });
                }
                electricals.SpecCategory.Add(PassLight);
                #endregion
                compareEntity.ComapareSpecifications.Spec.Add(electricals);
                
                #endregion
                compareEntity.CompareFeatures = new CompareMainCategory();
                compareEntity.CompareFeatures.Text = "Features";
                compareEntity.CompareFeatures.Value = "Features";
                compareEntity.CompareFeatures.Spec = new List<CompareSubMainCategory>();
                #region Features
                CompareSubMainCategory feature = new CompareSubMainCategory();
                feature.Value = "Features";
                feature.Text = "Features";
                feature.SpecCategory = new List<CompareSubCategory>();
                CompareSubCategory subFeature = new CompareSubCategory();

                feature.SpecCategory = new List<CompareSubCategory>();
                
                //Speedometer
                CompareSubCategory Speedometer = new CompareSubCategory();
                Speedometer.Text = "Speedometer";
                Speedometer.Value = "Speedometer";
                Speedometer.CompareSpec = new List<CompareBikeData>();
                foreach (var version in arrVersion)
                {
                    Speedometer.CompareSpec.Add(new CompareBikeData() { Value = Bikewale.Utility.FormatMinSpecs.ShowAvailable(compareEntity.Features.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version)).Speedometer) });
                }
                feature.SpecCategory.Add(Speedometer);

                //Tachometer
                CompareSubCategory Tachometer = new CompareSubCategory();
                Tachometer.Text = "Tachometer";
                Tachometer.Value = "Tachometer";
                Tachometer.CompareSpec = new List<CompareBikeData>();
                foreach (var version in arrVersion)
                {
                    Tachometer.CompareSpec.Add(new CompareBikeData() { Value = Bikewale.Utility.FormatMinSpecs.ShowAvailable(compareEntity.Features.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version)).Tachometer.Value) });
                }
                feature.SpecCategory.Add(Tachometer);

                //Tachometer Type
                CompareSubCategory TachometerType = new CompareSubCategory();
                TachometerType.Text = "Tachometer Type";
                TachometerType.Value = "Tachometer Type";
                TachometerType.CompareSpec = new List<CompareBikeData>();
                foreach (var version in arrVersion)
                {
                    TachometerType.CompareSpec.Add(new CompareBikeData() { Value = Bikewale.Utility.FormatMinSpecs.ShowAvailable(compareEntity.Features.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version)).TachometerType) });
                }
                feature.SpecCategory.Add(TachometerType);

                //Shift Light
                CompareSubCategory ShiftLight = new CompareSubCategory();
                ShiftLight.Text = "Shift Light";
                ShiftLight.Value = "Shift Light";
                ShiftLight.CompareSpec = new List<CompareBikeData>();
                foreach (var version in arrVersion)
                {
                    ShiftLight.CompareSpec.Add(new CompareBikeData() { Value = Bikewale.Utility.FormatMinSpecs.ShowAvailable(compareEntity.Features.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version)).ShiftLight.Value) });
                }
                feature.SpecCategory.Add(ShiftLight);

                //Electric Start
                CompareSubCategory ElectricStart = new CompareSubCategory();
                ElectricStart.Text = "Electric Start";
                ElectricStart.Value = "Electric Start";
                ElectricStart.CompareSpec = new List<CompareBikeData>();
                foreach (var version in arrVersion)
                {
                    ElectricStart.CompareSpec.Add(new CompareBikeData() { Value = Bikewale.Utility.FormatMinSpecs.ShowAvailable(compareEntity.Features.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version)).ElectricStart.Value) });
                }
                feature.SpecCategory.Add(ElectricStart);

                //Tripmeter
                CompareSubCategory Tripmeter = new CompareSubCategory();
                Tripmeter.Text = "Tripmeter";
                Tripmeter.Value = "Tripmeter";
                Tripmeter.CompareSpec = new List<CompareBikeData>();
                foreach (var version in arrVersion)
                {
                    Tripmeter.CompareSpec.Add(new CompareBikeData() { Value = Bikewale.Utility.FormatMinSpecs.ShowAvailable(compareEntity.Features.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version)).Tripmeter.Value) });
                }
                feature.SpecCategory.Add(Tripmeter);

                //No Of Tripmeters
                CompareSubCategory NoOfTripmeters = new CompareSubCategory();
                NoOfTripmeters.Text = "No Of Tripmeters";
                NoOfTripmeters.Value = "No Of Tripmeters";
                NoOfTripmeters.CompareSpec = new List<CompareBikeData>();
                foreach (var version in arrVersion)
                {
                    NoOfTripmeters.CompareSpec.Add(new CompareBikeData() { Value = Bikewale.Utility.FormatMinSpecs.ShowAvailable(compareEntity.Features.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version)).NoOfTripmeters) });
                }
                feature.SpecCategory.Add(NoOfTripmeters);

                //Tripmeter Type
                CompareSubCategory TripmeterType = new CompareSubCategory();
                TripmeterType.Text = "Tripmeter Type";
                TripmeterType.Value = "Tripmeter Type";
                TripmeterType.CompareSpec = new List<CompareBikeData>();
                foreach (var version in arrVersion)
                {
                    TripmeterType.CompareSpec.Add(new CompareBikeData() { Value = Bikewale.Utility.FormatMinSpecs.ShowAvailable(compareEntity.Features.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version)).TripmeterType) });
                }
                feature.SpecCategory.Add(TripmeterType);

                //Low Fuel Indicator
                CompareSubCategory LowFuelIndicator = new CompareSubCategory();
                LowFuelIndicator.Text = "Low Fuel Indicator";
                LowFuelIndicator.Value = "Low Fuel Indicator";
                LowFuelIndicator.CompareSpec = new List<CompareBikeData>();
                foreach (var version in arrVersion)
                {
                    LowFuelIndicator.CompareSpec.Add(new CompareBikeData() { Value = Bikewale.Utility.FormatMinSpecs.ShowAvailable(compareEntity.Features.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version)).LowFuelIndicator.Value) });
                }
                feature.SpecCategory.Add(LowFuelIndicator);

                //Low Oil Indicator
                CompareSubCategory LowOilIndicator = new CompareSubCategory();
                LowOilIndicator.Text = "Low Oil Indicator";
                LowOilIndicator.Value = "Low Oil Indicator";
                LowOilIndicator.CompareSpec = new List<CompareBikeData>();
                foreach (var version in arrVersion)
                {
                    LowOilIndicator.CompareSpec.Add(new CompareBikeData() { Value = Bikewale.Utility.FormatMinSpecs.ShowAvailable(compareEntity.Features.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version)).LowOilIndicator.Value) });
                }
                feature.SpecCategory.Add(LowOilIndicator);

                //Low Battery Indicator
                CompareSubCategory LowBatteryIndicator = new CompareSubCategory();
                LowBatteryIndicator.Text = "Low Battery Indicator";
                LowBatteryIndicator.Value = "Low Battery Indicator";
                LowBatteryIndicator.CompareSpec = new List<CompareBikeData>();
                foreach (var version in arrVersion)
                {
                    LowBatteryIndicator.CompareSpec.Add(new CompareBikeData() { Value = Bikewale.Utility.FormatMinSpecs.ShowAvailable(compareEntity.Features.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version)).LowBatteryIndicator.Value) });
                }
                feature.SpecCategory.Add(LowBatteryIndicator);

                //Fuel Gauge
                CompareSubCategory FuelGauge = new CompareSubCategory();
                FuelGauge.Text = "Fuel Gauge";
                FuelGauge.Value = "Fuel Gauge";
                FuelGauge.CompareSpec = new List<CompareBikeData>();
                foreach (var version in arrVersion)
                {
                    FuelGauge.CompareSpec.Add(new CompareBikeData() { Value = Bikewale.Utility.FormatMinSpecs.ShowAvailable(compareEntity.Features.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version)).FuelGauge.Value) });
                }
                feature.SpecCategory.Add(FuelGauge);

                //Digital Fuel Gauges
                CompareSubCategory DigitalFuelGauges = new CompareSubCategory();
                DigitalFuelGauges.Text = "Digital Fuel Gauges";
                DigitalFuelGauges.Value = "Digital Fuel Gauges";
                DigitalFuelGauges.CompareSpec = new List<CompareBikeData>();
                foreach (var version in arrVersion)
                {
                    DigitalFuelGauges.CompareSpec.Add(new CompareBikeData() { Value = Bikewale.Utility.FormatMinSpecs.ShowAvailable(compareEntity.Features.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version)).DigitalFuelGauge.Value) });
                }
                feature.SpecCategory.Add(DigitalFuelGauges);

                //Pillion Seat
                CompareSubCategory PillionSeat = new CompareSubCategory();
                PillionSeat.Text = "Pillion Seat";
                PillionSeat.Value = "Pillion Seat";
                PillionSeat.CompareSpec = new List<CompareBikeData>();
                foreach (var version in arrVersion)
                {
                    PillionSeat.CompareSpec.Add(new CompareBikeData() { Value = Bikewale.Utility.FormatMinSpecs.ShowAvailable(compareEntity.Features.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version)).PillionSeat.Value) });
                }
                feature.SpecCategory.Add(PillionSeat);

                //Pillion Footrest
                CompareSubCategory PillionFootrest = new CompareSubCategory();
                PillionFootrest.Text = "Pillion Footrest";
                PillionFootrest.Value = "Pillion Footrest";
                PillionFootrest.CompareSpec = new List<CompareBikeData>();
                foreach (var version in arrVersion)
                {
                    PillionFootrest.CompareSpec.Add(new CompareBikeData() { Value = Bikewale.Utility.FormatMinSpecs.ShowAvailable(compareEntity.Features.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version)).PillionFootrest.Value) });
                }
                feature.SpecCategory.Add(PillionFootrest);

                //Pillion Backrest
                CompareSubCategory PillionBackrest = new CompareSubCategory();
                PillionBackrest.Text = "Pillion Backrest";
                PillionBackrest.Value = "Pillion Backrest";
                PillionBackrest.CompareSpec = new List<CompareBikeData>();
                foreach (var version in arrVersion)
                {
                    PillionBackrest.CompareSpec.Add(new CompareBikeData() { Value = Bikewale.Utility.FormatMinSpecs.ShowAvailable(compareEntity.Features.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version)).PillionBackrest.Value) });
                }
                feature.SpecCategory.Add(PillionBackrest);

                //Pillion Grabrail
                CompareSubCategory PillionGrabrail = new CompareSubCategory();
                PillionGrabrail.Text = "Pillion Grabrail";
                PillionGrabrail.Value = "Pillion Grabrail";
                PillionGrabrail.CompareSpec = new List<CompareBikeData>();
                foreach (var version in arrVersion)
                {
                    PillionGrabrail.CompareSpec.Add(new CompareBikeData() { Value = Bikewale.Utility.FormatMinSpecs.ShowAvailable(compareEntity.Features.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version)).PillionGrabrail.Value) });
                }
                feature.SpecCategory.Add(PillionGrabrail);

                //Stand Alarm
                CompareSubCategory StandAlarm = new CompareSubCategory();
                StandAlarm.Text = "Stand Alarm";
                StandAlarm.Value = "Stand Alarm";
                StandAlarm.CompareSpec = new List<CompareBikeData>();
                foreach (var version in arrVersion)
                {
                    StandAlarm.CompareSpec.Add(new CompareBikeData() { Value = Bikewale.Utility.FormatMinSpecs.ShowAvailable(compareEntity.Features.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version)).StandAlarm.Value) });
                }
                feature.SpecCategory.Add(StandAlarm);

                //Stepped Seat
                CompareSubCategory SteppedSeat = new CompareSubCategory();
                SteppedSeat.Text = "Stepped Seat";
                SteppedSeat.Value = "Stepped Seat";
                SteppedSeat.CompareSpec = new List<CompareBikeData>();
                foreach (var version in arrVersion)
                {
                    SteppedSeat.CompareSpec.Add(new CompareBikeData() { Value = Bikewale.Utility.FormatMinSpecs.ShowAvailable(compareEntity.Features.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version)).SteppedSeat.Value) });
                }
                feature.SpecCategory.Add(SteppedSeat);

                //Antilock Braking System
                CompareSubCategory AntilockBrakingSystem = new CompareSubCategory();
                AntilockBrakingSystem.Text = "Antilock Braking System";
                AntilockBrakingSystem.Value = "Antilock Braking System";
                AntilockBrakingSystem.CompareSpec = new List<CompareBikeData>();
                foreach (var version in arrVersion)
                {
                    AntilockBrakingSystem.CompareSpec.Add(new CompareBikeData() { Value = Bikewale.Utility.FormatMinSpecs.ShowAvailable(compareEntity.Features.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version)).AntilockBrakingSystem.Value) });
                }
                feature.SpecCategory.Add(AntilockBrakingSystem);

                //Killswitch
                CompareSubCategory Killswitch = new CompareSubCategory();
                Killswitch.Text = "Killswitch";
                Killswitch.Value = "Killswitch";
                Killswitch.CompareSpec = new List<CompareBikeData>();
                foreach (var version in arrVersion)
                {
                    Killswitch.CompareSpec.Add(new CompareBikeData() { Value = Bikewale.Utility.FormatMinSpecs.ShowAvailable(compareEntity.Features.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version)).Killswitch.Value) });
                }
                feature.SpecCategory.Add(Killswitch);

                //Clock
                CompareSubCategory Clock = new CompareSubCategory();
                Clock.Text = "Clock";
                Clock.Value = "Clock";
                Clock.CompareSpec = new List<CompareBikeData>();
                foreach (var version in arrVersion)
                {
                    Clock.CompareSpec.Add(new CompareBikeData() { Value = Bikewale.Utility.FormatMinSpecs.ShowAvailable(compareEntity.Features.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version)).Clock.Value) });
                }
                feature.SpecCategory.Add(Clock);
                compareEntity.CompareFeatures.Spec.Add(feature);
                #endregion                           
                compareEntity.CompareColors = new CompareBikeColorCategory();
                compareEntity.CompareColors.Text = "Colours";
                compareEntity.CompareColors.Value = "Colours";
                compareEntity.CompareColors.bikes = new List<CompareBikeColor>();
                foreach (var version in arrVersion)
                {
                    var bikeColor = from color in compareEntity.Color
                                    where color.VersionId == Convert.ToUInt32(version)
                                    select color;
                    compareEntity.CompareColors.bikes.Add(new CompareBikeColor() { bikeColors = bikeColor.ToList() });
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "DoCompare");
                objErr.SendMail();
            }
            return compareEntity;
        }

        public IEnumerable<Entities.Compare.TopBikeCompareBase> CompareList(uint topCount)
        {
            return _objCompare.CompareList(topCount);
        }
    }
}
