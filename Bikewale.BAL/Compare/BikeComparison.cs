using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Bikewale.Cache.Compare;
using Bikewale.Cache.Core;
using Bikewale.DAL.Compare;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.Compare;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.Compare;
using Bikewale.Notifications;
using Bikewale.Utility;
using log4net;
using Microsoft.Practices.Unity;

namespace Bikewale.BAL.Compare
{
    /// <summary>
    /// Modified By : Sushil Kumar on 2nd Dec 2016
    /// Description : Added methods for featured bike and sponsored bike comparisions
    /// Modified By : Sushil Kumar on 2nd Feb 2017
    /// Description : Added methods for comparisions bikes binding using transpose methodology
    /// Modified by : Aditi Srivastava on 5 June 2017
    /// Summary     : Added cache call for comparison carousel for further processing in element order
    /// </summary>
    public class BikeComparison : IBikeCompare
    {
        private readonly IBikeCompare _objCompare = null;
        private readonly IBikeCompareCacheRepository _objCache = null;

        static bool _logGrpcErrors = Convert.ToBoolean(Bikewale.Utility.BWConfiguration.Instance.LogGrpcErrors);
        static readonly ILog _logger = LogManager.GetLogger(typeof(BikeComparison));
        static bool _useGrpc = Convert.ToBoolean(Bikewale.Utility.BWConfiguration.Instance.UseGrpc);

        /// <summary>
        /// Modified by : Aditi Srivastava on 5 June 2017
        /// Summary     : Resolution for cache layer functions
        /// </summary>
        public BikeComparison()
        {
            using (IUnityContainer objPQCont = new UnityContainer())
            {
                objPQCont.RegisterType<IBikeCompare, BikeCompareRepository>();
                objPQCont.RegisterType<ICacheManager, MemcacheManager>();
                objPQCont.RegisterType<IBikeCompareCacheRepository, BikeCompareCacheRepository>();
                _objCompare = objPQCont.Resolve<IBikeCompare>();
                _objCache = objPQCont.Resolve<IBikeCompareCacheRepository>();

            }
        }

        public Entities.Compare.BikeCompareEntity DoCompare(string versions)
        {
            Entities.Compare.BikeCompareEntity compareEntity = null;
            try
            {
                if (!string.IsNullOrEmpty(versions))
                {
                    compareEntity = _objCompare.DoCompare(versions);
                    TransposeCompareBikeData(ref compareEntity, versions);
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "DoCompare");
                
            }
            return compareEntity;
        }

        /// <summary>
        /// Created By : Sushil Kumar on 2nd Feb 2017
        /// Description : Removed transpose logic from DoCompare and moved into common for both app and website
        /// Modified by : Aditi Srivastava on 18 May 2017
        /// Summary     : used nullable bool function to format specs and features
        /// Modified by : Vivek Singh Tomar on 20th Dec 2017
        /// Summary     : Optimization of TransposeCompareBikeData method reduced extra operations
        /// </summary>
        /// <param name="compareEntity"></param>
        /// <param name="versions"></param>
        private static void TransposeCompareBikeData(ref Entities.Compare.BikeCompareEntity compareEntity, string versions)
        {
            try
            {
                string[] arrVersion = versions.Split(',');

                if (compareEntity != null)
                {
                    CompareMainCategory compareSpecifications = new CompareMainCategory {
                        Text = "Specifications",
                        Value = "Specifications",
                        Spec = new List<CompareSubMainCategory>()
                    };

                    #region Specifications
                    CompareSubMainCategory engineTransmission = new CompareSubMainCategory {
                        Text = "Engine & Transmission",
                        Value = "2"
                    };

                    #region Engine & Transmission
                    CompareSubCategory etDisplacement = new CompareSubCategory {
                        Text = "Displacement (cc)",
                        Value = "Displacement",
                        CompareSpec = new List<CompareBikeData>()
                    };

                    CompareSubCategory etCylinders = new CompareSubCategory {
                        Text = "Cylinders",
                        Value = "Cylinders",
                        CompareSpec = new List<CompareBikeData>()
                    };

                    CompareSubCategory etMaxPower = new CompareSubCategory {
                        Text = "Max Power",
                        Value = "Max Power",
                        CompareSpec = new List<CompareBikeData>()
                    };

                    CompareSubCategory etMaximumTorque = new CompareSubCategory {
                        Text = "Maximum Torque",
                        Value = "Maximum Torque",
                        CompareSpec = new List<CompareBikeData>()
                    };

                    CompareSubCategory etBore = new CompareSubCategory {
                        Text = "Bore (mm)",
                        Value = "Bore (mm)",
                        CompareSpec = new List<CompareBikeData>()
                    };

                    CompareSubCategory etStroke = new CompareSubCategory {
                        Text = "Stroke (mm)",
                        Value = "Stroke (mm)",
                        CompareSpec = new List<CompareBikeData>()
                    };

                    CompareSubCategory etValvesPerCylinder = new CompareSubCategory {
                        Text = "Valves Per Cylinder",
                        Value = "Valves Per Cylinder",
                        CompareSpec = new List<CompareBikeData>()
                    };

                    CompareSubCategory etFuelDeliverySystem = new CompareSubCategory {
                        Text = "Fuel Delivery System",
                        Value = "Fuel Delivery System",
                        CompareSpec = new List<CompareBikeData>()
                    };

                    CompareSubCategory etFuelType = new CompareSubCategory {
                        Text = "Fuel Type",
                        Value = "Fuel Type",
                        CompareSpec = new List<CompareBikeData>()
                    };

                    CompareSubCategory etIgnition = new CompareSubCategory {
                        Text = "Ignition",
                        Value = "Ignition",
                        CompareSpec = new List<CompareBikeData>()
                    };

                    CompareSubCategory etSparkPlugs = new CompareSubCategory {
                        Text = "Spark Plugs (Per Cylinder)",
                        Value = "Spark Plugs (Per Cylinder)",
                        CompareSpec = new List<CompareBikeData>()
                    };

                    CompareSubCategory etCoolingSystem = new CompareSubCategory {
                        Text = "Cooling System",
                        Value = "Cooling System",
                        CompareSpec = new List<CompareBikeData>()
                    };

                    CompareSubCategory etGearBox = new CompareSubCategory {
                        Text = "Gearbox Type",
                        Value = "Gearbox Type",
                        CompareSpec = new List<CompareBikeData>()
                    };

                    CompareSubCategory etNoGears = new CompareSubCategory {
                        Text = "No. of gears",
                        Value = "No. of gears",
                        CompareSpec = new List<CompareBikeData>()
                    };

                    CompareSubCategory etTransmissionType = new CompareSubCategory {
                        Text = "Transmission Type",
                        Value = "Transmission Type",
                        CompareSpec = new List<CompareBikeData>()
                    };

                    CompareSubCategory etClutch = new CompareSubCategory {
                        Text = "Clutch",
                        Value = "Clutch",
                        CompareSpec = new List<CompareBikeData>()
                    };

                    foreach (var version in arrVersion)
                    {
                        var spec = compareEntity.Specifications.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version));
                        if (spec != null)
                        {
                            etDisplacement.CompareSpec.Add(new CompareBikeData() { Value = FormatMinSpecs.ShowAvailable(spec.Displacement.Value, "cc"), Text = FormatMinSpecs.ShowAvailable(spec.Displacement.Value, "cc") });
                            etCylinders.CompareSpec.Add(new CompareBikeData() { Value = FormatMinSpecs.ShowAvailable(spec.Cylinders.Value), Text = FormatMinSpecs.ShowAvailable(spec.Cylinders.Value) });
                            etMaxPower.CompareSpec.Add(new CompareBikeData() { Value = FormatMinSpecs.ShowAvailable(spec.MaxPower.Value, "bhp", spec.MaxPowerRpm.Value, "rpm"), Text = FormatMinSpecs.ShowAvailable(spec.MaxPower.Value, "bhp", spec.MaxPowerRpm.Value, "rpm") });
                            etMaximumTorque.CompareSpec.Add(new CompareBikeData() { Value = FormatMinSpecs.ShowAvailable(spec.MaximumTorque.Value, "Nm", spec.MaximumTorqueRpm.Value, "rpm"), Text = FormatMinSpecs.ShowAvailable(spec.MaximumTorque.Value, "Nm", spec.MaximumTorqueRpm.Value, "rpm") });
                            etBore.CompareSpec.Add(new CompareBikeData() { Value = FormatMinSpecs.ShowAvailable(spec.Bore.Value), Text = FormatMinSpecs.ShowAvailable(spec.Bore.Value) });
                            etStroke.CompareSpec.Add(new CompareBikeData() { Value = FormatMinSpecs.ShowAvailable(spec.Stroke.Value), Text = FormatMinSpecs.ShowAvailable(spec.Stroke.Value) });
                            etValvesPerCylinder.CompareSpec.Add(new CompareBikeData() { Value = FormatMinSpecs.ShowAvailable(spec.ValvesPerCylinder.Value), Text = FormatMinSpecs.ShowAvailable(spec.ValvesPerCylinder.Value) });
                            etFuelDeliverySystem.CompareSpec.Add(new CompareBikeData() { Value = FormatMinSpecs.ShowAvailable(spec.FuelDeliverySystem), Text = FormatMinSpecs.ShowAvailable(spec.FuelDeliverySystem) });
                            etFuelType.CompareSpec.Add(new CompareBikeData() { Value = FormatMinSpecs.ShowAvailable(spec.FuelType), Text = FormatMinSpecs.ShowAvailable(spec.FuelType) });
                            etIgnition.CompareSpec.Add(new CompareBikeData() { Value = FormatMinSpecs.ShowAvailable(spec.Ignition), Text = FormatMinSpecs.ShowAvailable(spec.Ignition) });
                            etSparkPlugs.CompareSpec.Add(new CompareBikeData() { Value = FormatMinSpecs.ShowAvailable(spec.SparkPlugsPerCylinder), Text = FormatMinSpecs.ShowAvailable(spec.SparkPlugsPerCylinder) });
                            etCoolingSystem.CompareSpec.Add(new CompareBikeData() { Value = FormatMinSpecs.ShowAvailable(spec.CoolingSystem), Text = FormatMinSpecs.ShowAvailable(spec.CoolingSystem) });
                            etGearBox.CompareSpec.Add(new CompareBikeData() { Value = FormatMinSpecs.ShowAvailable(spec.GearboxType), Text = FormatMinSpecs.ShowAvailable(spec.GearboxType) });
                            etNoGears.CompareSpec.Add(new CompareBikeData() { Value = FormatMinSpecs.ShowAvailable(spec.NoOfGears.Value), Text = FormatMinSpecs.ShowAvailable(spec.NoOfGears.Value) });
                            etTransmissionType.CompareSpec.Add(new CompareBikeData() { Value = FormatMinSpecs.ShowAvailable(spec.TransmissionType), Text = FormatMinSpecs.ShowAvailable(spec.TransmissionType) });
                            etClutch.CompareSpec.Add(new CompareBikeData() { Value = FormatMinSpecs.ShowAvailable(spec.Clutch), Text = FormatMinSpecs.ShowAvailable(spec.Clutch) });

                        }
                    }

                    // Add created sub-categories to the main engine-Transmission category
                    engineTransmission.SpecCategory = new List<CompareSubCategory> {
                        etDisplacement,
                        etCylinders,
                        etMaxPower,
                        etMaximumTorque,
                        etBore,
                        etStroke,
                        etValvesPerCylinder,
                        etFuelDeliverySystem,
                        etFuelType,
                        etIgnition,
                        etSparkPlugs,
                        etCoolingSystem,
                        etGearBox,
                        etNoGears,
                        etTransmissionType,
                        etClutch
                    };

                    #endregion
                    compareSpecifications.Spec.Add(engineTransmission);

                    CompareSubMainCategory brakesWheelsSuspension = new CompareSubMainCategory {
                        Text = "Brakes, Wheels and Suspension",
                        Value = "3"
                    };

                    #region Brakes, Wheels and Suspension
                    CompareSubCategory bwsBreakType = new CompareSubCategory {
                        Text = "Brake Type",
                        Value = "Brake Type",
                        CompareSpec = new List<CompareBikeData>()
                    };

                    CompareSubCategory bwsFrontDisc = new CompareSubCategory {
                        Text = "Front Disc",
                        Value = "Front Disc",
                        CompareSpec = new List<CompareBikeData>()
                    };

                    CompareSubCategory bwsFrontDisc_DrumSize = new CompareSubCategory {
                        Text = "Front Disc/Drum Size (mm)",
                        Value = "Front Disc/Drum Size (mm)",
                        CompareSpec = new List<CompareBikeData>()
                    };

                    CompareSubCategory bwsRearDisc = new CompareSubCategory {
                        Text = "Rear Disc",
                        Value = "Rear Disc",
                        CompareSpec = new List<CompareBikeData>()
                    };


                    CompareSubCategory bwsRearDisc_DrumSize = new CompareSubCategory {
                        Text = "Rear Disc/Drum Size (mm)",
                        Value = "Rear Disc/Drum Size (mm)",
                        CompareSpec = new List<CompareBikeData>()
                    };

                    CompareSubCategory bwsCalliperType = new CompareSubCategory {
                        Text = "Calliper Type",
                        Value = "Calliper Type",
                        CompareSpec = new List<CompareBikeData>()
                    };

                    CompareSubCategory bwsWheelSize = new CompareSubCategory {
                        Text = "Wheel Size (inches)",
                        Value = "Wheel Size (inches)",
                        CompareSpec = new List<CompareBikeData>()
                    };

                    CompareSubCategory bwsFrontTyre = new CompareSubCategory {
                        Text = "Front Tyre",
                        Value = "Front Tyre",
                        CompareSpec = new List<CompareBikeData>()
                    };

                    CompareSubCategory bwsRearTyre = new CompareSubCategory {
                        Text = "Rear Tyre",
                        Value = "Rear Tyre",
                        CompareSpec = new List<CompareBikeData>()
                    };

                    CompareSubCategory bwsTubelessTyres = new CompareSubCategory {
                        Text = "Tubeless Tyres",
                        Value = "Tubeless Tyres",
                        CompareSpec = new List<CompareBikeData>()
                    };


                    CompareSubCategory bwsRadialTyres = new CompareSubCategory {
                        Text = "Radial Tyres",
                        Value = "Radial Tyres",
                        CompareSpec = new List<CompareBikeData>()
                    };

                    CompareSubCategory bwsAlloyWheels = new CompareSubCategory {
                        Text = "Alloy Wheels",
                        Value = "Alloy Wheels",
                        CompareSpec = new List<CompareBikeData>()
                    };

                    CompareSubCategory bwsFrontSuspension = new CompareSubCategory {
                        Text = "Front Suspension",
                        Value = "Front Suspension",
                        CompareSpec = new List<CompareBikeData>()
                    };

                    CompareSubCategory bwsRearSuspension = new CompareSubCategory {
                        Text = "Rear Suspension",
                        Value = "Rear Suspension",
                        CompareSpec = new List<CompareBikeData>()
                    };


                    foreach (var version in arrVersion)
                    {
                        var spec = compareEntity.Specifications.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version));
                        if (spec != null)
                        {
                            bwsBreakType.CompareSpec.Add(new CompareBikeData() { Value = FormatMinSpecs.ShowAvailable(spec.BrakeType), Text = FormatMinSpecs.ShowAvailable(spec.BrakeType) });
                            bwsFrontDisc.CompareSpec.Add(new CompareBikeData() { Value = FormatMinSpecs.ShowAvailable(spec.FrontDisc), Text = FormatMinSpecs.ShowAvailable(spec.FrontDisc) });
                            bwsFrontDisc_DrumSize.CompareSpec.Add(new CompareBikeData() { Value = FormatMinSpecs.ShowAvailable(spec.FrontDisc_DrumSize.Value), Text = FormatMinSpecs.ShowAvailable(spec.FrontDisc_DrumSize.Value) });
                            bwsRearDisc.CompareSpec.Add(new CompareBikeData() { Value = FormatMinSpecs.ShowAvailable(spec.RearDisc), Text = FormatMinSpecs.ShowAvailable(spec.RearDisc) });
                            bwsRearDisc_DrumSize.CompareSpec.Add(new CompareBikeData() { Value = FormatMinSpecs.ShowAvailable(spec.RearDisc_DrumSize.Value), Text = FormatMinSpecs.ShowAvailable(spec.RearDisc_DrumSize.Value) });
                            bwsCalliperType.CompareSpec.Add(new CompareBikeData() { Value = FormatMinSpecs.ShowAvailable(spec.CalliperType), Text = FormatMinSpecs.ShowAvailable(spec.CalliperType) });
                            bwsWheelSize.CompareSpec.Add(new CompareBikeData() { Value = FormatMinSpecs.ShowAvailable(spec.WheelSize.Value), Text = FormatMinSpecs.ShowAvailable(spec.WheelSize.Value) });
                            bwsFrontTyre.CompareSpec.Add(new CompareBikeData() { Value = FormatMinSpecs.ShowAvailable(spec.FrontTyre), Text = FormatMinSpecs.ShowAvailable(spec.FrontTyre) });
                            bwsRearTyre.CompareSpec.Add(new CompareBikeData() { Value = FormatMinSpecs.ShowAvailable(spec.RearTyre), Text = FormatMinSpecs.ShowAvailable(spec.RearTyre) });
                            bwsTubelessTyres.CompareSpec.Add(new CompareBikeData() { Value = FormatMinSpecs.ShowAvailable(spec.TubelessTyres), Text = FormatMinSpecs.ShowAvailable(spec.TubelessTyres) });
                            bwsRadialTyres.CompareSpec.Add(new CompareBikeData() { Value = FormatMinSpecs.ShowAvailable(spec.RadialTyres), Text = FormatMinSpecs.ShowAvailable(spec.RadialTyres) });
                            bwsAlloyWheels.CompareSpec.Add(new CompareBikeData() { Value = FormatMinSpecs.ShowAvailable(spec.AlloyWheels), Text = FormatMinSpecs.ShowAvailable(spec.AlloyWheels) });
                            bwsFrontSuspension.CompareSpec.Add(new CompareBikeData() { Value = FormatMinSpecs.ShowAvailable(spec.FrontSuspension), Text = FormatMinSpecs.ShowAvailable(spec.FrontSuspension) });
                            bwsRearSuspension.CompareSpec.Add(new CompareBikeData() { Value = FormatMinSpecs.ShowAvailable(spec.RearSuspension), Text = FormatMinSpecs.ShowAvailable(spec.RearSuspension) });

                        }
                    }

                    brakesWheelsSuspension.SpecCategory = new List<CompareSubCategory> {
                         bwsBreakType
                        ,bwsFrontDisc
                        ,bwsFrontDisc_DrumSize
                        ,bwsRearDisc
                        ,bwsRearDisc_DrumSize
                        ,bwsCalliperType
                        ,bwsWheelSize
                        ,bwsFrontTyre
                        ,bwsRearTyre
                        ,bwsTubelessTyres
                        ,bwsRadialTyres
                        ,bwsAlloyWheels
                        ,bwsFrontSuspension
                        ,bwsRearSuspension
                    };

                    #endregion
                    compareSpecifications.Spec.Add(brakesWheelsSuspension);

                    CompareSubMainCategory dimensionsChassis = new CompareSubMainCategory {
                        Text = "Dimensions and Chassis",
                        Value = "4"
                    };

                    #region Dimensions and Chassis
                    CompareSubCategory KerbWeight = new CompareSubCategory {
                        Text = "Kerb Weight (Kg)",
                        Value = "Kerb Weight (Kg)",
                        CompareSpec = new List<CompareBikeData>()
                    };

                    CompareSubCategory OverallLength = new CompareSubCategory {
                        Text = "Overall Length (mm)",
                        Value = "Overall Length (mm)",
                        CompareSpec = new List<CompareBikeData>()
                    };

                    CompareSubCategory OverallWidth = new CompareSubCategory {
                        Text = "Overall Width (mm)",
                        Value = "Overall Width (mm)",
                        CompareSpec = new List<CompareBikeData>()
                    };

                    CompareSubCategory OverallHeight = new CompareSubCategory {
                        Text = "Overall Height (mm)",
                        Value = "Overall Height (mm)",
                        CompareSpec = new List<CompareBikeData>()
                    };


                    CompareSubCategory Wheelbase = new CompareSubCategory {
                        Text = "Wheelbase (mm)",
                        Value = "Wheelbase (mm)",
                        CompareSpec = new List<CompareBikeData>()
                    };

                    CompareSubCategory GroundClearance = new CompareSubCategory {
                        Text = "Ground Clearance (mm)",
                        Value = "Ground Clearance (mm)",
                        CompareSpec = new List<CompareBikeData>()
                    };

                    CompareSubCategory SeatHeight = new CompareSubCategory {
                        Text = "Seat Height (mm)",
                        Value = "Seat Height (mm)",
                        CompareSpec = new List<CompareBikeData>()
                    };

                    foreach (var version in arrVersion)
                    {
                        var spec = compareEntity.Specifications.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version));
                        if (spec != null) {
                            KerbWeight.CompareSpec.Add(new CompareBikeData() { Value = FormatMinSpecs.ShowAvailable(spec.KerbWeight.Value), Text = FormatMinSpecs.ShowAvailable(spec.KerbWeight.Value) });
                            OverallLength.CompareSpec.Add(new CompareBikeData() { Value = FormatMinSpecs.ShowAvailable(spec.OverallLength.Value), Text = FormatMinSpecs.ShowAvailable(spec.OverallLength.Value) });
                            OverallWidth.CompareSpec.Add(new CompareBikeData() { Value = FormatMinSpecs.ShowAvailable(spec.OverallWidth.Value), Text = FormatMinSpecs.ShowAvailable(spec.OverallWidth.Value) });
                            OverallHeight.CompareSpec.Add(new CompareBikeData() { Value = FormatMinSpecs.ShowAvailable(spec.OverallHeight.Value), Text = FormatMinSpecs.ShowAvailable(spec.OverallHeight.Value) });
                            Wheelbase.CompareSpec.Add(new CompareBikeData() { Value = FormatMinSpecs.ShowAvailable(spec.Wheelbase.Value), Text = FormatMinSpecs.ShowAvailable(spec.Wheelbase.Value) });
                            GroundClearance.CompareSpec.Add(new CompareBikeData() { Value = FormatMinSpecs.ShowAvailable(spec.GroundClearance.Value), Text = FormatMinSpecs.ShowAvailable(spec.GroundClearance.Value) });
                            SeatHeight.CompareSpec.Add(new CompareBikeData() { Value = FormatMinSpecs.ShowAvailable(spec.SeatHeight.Value), Text = FormatMinSpecs.ShowAvailable(spec.SeatHeight.Value) });
                        }
                    }

                    dimensionsChassis.SpecCategory = new List<CompareSubCategory> {
                        KerbWeight
                        ,OverallLength
                        ,OverallWidth
                        ,OverallHeight
                        ,Wheelbase
                        ,GroundClearance
                        ,SeatHeight
                    };
                    #endregion
                    compareSpecifications.Spec.Add(dimensionsChassis);

                    CompareSubMainCategory fuelEfficiencyPerformance = new CompareSubMainCategory {
                        Text = "Fuel efficiency and Performance",
                        Value = "5",
                        SpecCategory = new List<CompareSubCategory>()
                    };

                    #region Fuel efficiency and Performance
                    CompareSubCategory FuelTankCapacity = new CompareSubCategory {
                        Text = "Fuel Tank Capacity (Litres)",
                        Value = "Fuel Tank Capacity (Litres)",
                        CompareSpec = new List<CompareBikeData>()
                    };

                    CompareSubCategory ReserveFuelCapacity = new CompareSubCategory {
                        Text = "Reserve Fuel Capacity (Litres)",
                        Value = "Reserve Fuel Capacity (Litres)",
                        CompareSpec = new List<CompareBikeData>()
                    };

                    CompareSubCategory FuelEfficiencyOverall = new CompareSubCategory {
                        Text = "FuelEfficiency Overall (Kmpl)",
                        Value = "FuelEfficiency Overall (Kmpl)",
                        CompareSpec = new List<CompareBikeData>()
                    };

                    CompareSubCategory FuelEfficiencyRange = new CompareSubCategory {
                        Text = "Fuel Efficiency Range (Km)",
                        Value = "Fuel Efficiency Range (Km)",
                        CompareSpec = new List<CompareBikeData>()
                    };

                    CompareSubCategory Performance_0_60_kmph = new CompareSubCategory {
                        Text = "0 to 60 kmph (Seconds)",
                        Value = "0 to 60 kmph (Seconds)",
                        CompareSpec = new List<CompareBikeData>()
                    };


                    CompareSubCategory Performance_0_80_kmph = new CompareSubCategory {
                        Text = "0 to 80 kmph (Seconds)",
                        Value = "0 to 80 kmph (Seconds)",
                        CompareSpec = new List<CompareBikeData>()
                    };

                    CompareSubCategory Performance_0_40_m = new CompareSubCategory {
                        Text = "0 to 40 m (Seconds)",
                        Value = "0 to 40 m (Seconds)",
                        CompareSpec = new List<CompareBikeData>()
                    };

                    CompareSubCategory TopSpeed = new CompareSubCategory {
                        Text = "Top Speed (Kmph)",
                        Value = "Top Speed (Kmph)",
                        CompareSpec = new List<CompareBikeData>()
                    };

                    CompareSubCategory Performance_60_0_kmph = new CompareSubCategory {
                        Text = "60 to 0 Kmph (Seconds, metres)",
                        Value = "60 to 0 Kmph (Seconds, metres)",
                        CompareSpec = new List<CompareBikeData>()
                    };

                    CompareSubCategory Performance_80_0_kmph = new CompareSubCategory {
                        Text = "80 to 0 kmph (Seconds, metres)",
                        Value = "80 to 0 kmph (Seconds, metres)",
                        CompareSpec = new List<CompareBikeData>()
                    };

                    foreach (var version in arrVersion)
                    {
                        var spec = compareEntity.Specifications.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version));
                        if (spec != null)
                        {
                            FuelTankCapacity.CompareSpec.Add(new CompareBikeData() { Value = FormatMinSpecs.ShowAvailable(spec.FuelTankCapacity.Value), Text = FormatMinSpecs.ShowAvailable(spec.FuelTankCapacity.Value) });
                            ReserveFuelCapacity.CompareSpec.Add(new CompareBikeData() { Value = FormatMinSpecs.ShowAvailable(spec.ReserveFuelCapacity.Value), Text = FormatMinSpecs.ShowAvailable(spec.ReserveFuelCapacity.Value) });
                            FuelEfficiencyOverall.CompareSpec.Add(new CompareBikeData() { Value = FormatMinSpecs.ShowAvailable(spec.FuelEfficiencyOverall.Value), Text = FormatMinSpecs.ShowAvailable(spec.FuelEfficiencyOverall.Value) });
                            FuelEfficiencyRange.CompareSpec.Add(new CompareBikeData() { Value = FormatMinSpecs.ShowAvailable(spec.FuelEfficiencyRange.Value), Text = FormatMinSpecs.ShowAvailable(spec.FuelEfficiencyRange.Value) });
                            Performance_0_60_kmph.CompareSpec.Add(new CompareBikeData() { Value = FormatMinSpecs.ShowAvailable(spec.Performance_0_60_kmph.Value), Text = FormatMinSpecs.ShowAvailable(spec.Performance_0_60_kmph.Value) });
                            Performance_0_80_kmph.CompareSpec.Add(new CompareBikeData() { Value = FormatMinSpecs.ShowAvailable(spec.Performance_0_80_kmph.Value), Text = FormatMinSpecs.ShowAvailable(spec.Performance_0_80_kmph.Value) });
                            Performance_0_40_m.CompareSpec.Add(new CompareBikeData() { Value = FormatMinSpecs.ShowAvailable(spec.Performance_0_40_m.Value), Text = FormatMinSpecs.ShowAvailable(spec.Performance_0_40_m.Value) });
                            TopSpeed.CompareSpec.Add(new CompareBikeData() { Value = FormatMinSpecs.ShowAvailable(spec.TopSpeed.Value), Text = FormatMinSpecs.ShowAvailable(spec.TopSpeed.Value) });
                            Performance_60_0_kmph.CompareSpec.Add(new CompareBikeData() { Value = FormatMinSpecs.ShowAvailable(spec.Performance_60_0_kmph), Text = FormatMinSpecs.ShowAvailable(spec.Performance_60_0_kmph) });
                            Performance_80_0_kmph.CompareSpec.Add(new CompareBikeData() { Value = FormatMinSpecs.ShowAvailable(spec.Performance_80_0_kmph), Text = FormatMinSpecs.ShowAvailable(spec.Performance_80_0_kmph) });
                        }
                    }

                    fuelEfficiencyPerformance.SpecCategory = new List<CompareSubCategory> {
                        FuelTankCapacity
                        ,ReserveFuelCapacity
                        ,FuelEfficiencyOverall
                        ,FuelEfficiencyRange
                        ,Performance_0_60_kmph
                        ,Performance_0_80_kmph
                        ,Performance_0_40_m
                        ,TopSpeed
                        ,Performance_60_0_kmph
                        ,Performance_80_0_kmph
                    };

                    #endregion
                    compareSpecifications.Spec.Add(fuelEfficiencyPerformance);

                    compareEntity.CompareSpecifications = compareSpecifications;

                    #endregion

                    CompareMainCategory compareFeatures = new CompareMainCategory {
                        Text = "Features",
                        Value = "Features",
                        Spec = new List<CompareSubMainCategory>()
                    };

                    #region Features
                    CompareSubMainCategory features = new CompareSubMainCategory {
                        Value = "Features",
                        Text = "Features",
                    };

                    //Speedometer
                    CompareSubCategory Speedometer = new CompareSubCategory {
                        Text = "Speedometer",
                        Value = "Speedometer",
                        CompareSpec = new List<CompareBikeData>()
                    };

                    //Tachometer
                    CompareSubCategory Tachometer = new CompareSubCategory {
                        Text = "Tachometer",
                        Value = "Tachometer",
                        CompareSpec = new List<CompareBikeData>()
                    };

                    //Tachometer Type
                    CompareSubCategory TachometerType = new CompareSubCategory();
                    TachometerType.Text = "Tachometer Type";
                    TachometerType.Value = "Tachometer Type";
                    TachometerType.CompareSpec = new List<CompareBikeData>();

                    //Shift Light
                    CompareSubCategory ShiftLight = new CompareSubCategory {
                        Text = "Shift Light",
                        Value = "Shift Light",
                        CompareSpec = new List<CompareBikeData>()
                    };


                    //Electric Start
                    CompareSubCategory ElectricStart = new CompareSubCategory {
                        Text = "Electric Start",
                        Value = "Electric Start",
                        CompareSpec = new List<CompareBikeData>()
                    };


                    //Tripmeter
                    CompareSubCategory Tripmeter = new CompareSubCategory {
                        Text = "Tripmeter",
                        Value = "Tripmeter",
                        CompareSpec = new List<CompareBikeData>()
                    };

                    //No Of Tripmeters
                    CompareSubCategory NoOfTripmeters = new CompareSubCategory {
                        Text = "No Of Tripmeters",
                        Value = "No Of Tripmeters",
                        CompareSpec = new List<CompareBikeData>()
                    };

                    //Tripmeter Type
                    CompareSubCategory TripmeterType = new CompareSubCategory {
                        Text = "Tripmeter Type",
                        Value = "Tripmeter Type",
                        CompareSpec = new List<CompareBikeData>()
                    };

                    //Low Fuel Indicator
                    CompareSubCategory LowFuelIndicator = new CompareSubCategory {
                        Text = "Low Fuel Indicator",
                        Value = "Low Fuel Indicator",
                        CompareSpec = new List<CompareBikeData>()
                    };


                    //Low Oil Indicator
                    CompareSubCategory LowOilIndicator = new CompareSubCategory {
                        Text = "Low Oil Indicator",
                        Value = "Low Oil Indicator",
                        CompareSpec = new List<CompareBikeData>()
                    };

                    //Low Battery Indicator
                    CompareSubCategory LowBatteryIndicator = new CompareSubCategory {
                        Text = "Low Battery Indicator",
                        Value = "Low Battery Indicator",
                        CompareSpec = new List<CompareBikeData>()
                    };

                    //Fuel Gauge
                    CompareSubCategory FuelGauge = new CompareSubCategory {
                        Text = "Fuel Gauge",
                        Value = "Fuel Gauge",
                        CompareSpec = new List<CompareBikeData>()
                    };

                    //Digital Fuel Gauges
                    CompareSubCategory DigitalFuelGauges = new CompareSubCategory {
                        Text = "Digital Fuel Gauges",
                        Value = "Digital Fuel Gauges",
                        CompareSpec = new List<CompareBikeData>()
                    };

                    //Pillion Seat
                    CompareSubCategory PillionSeat = new CompareSubCategory {
                        Text = "Pillion Seat",
                        Value = "Pillion Seat",
                        CompareSpec = new List<CompareBikeData>()
                    };

                    //Pillion Footrest
                    CompareSubCategory PillionFootrest = new CompareSubCategory {
                        Text = "Pillion Footrest",
                        Value = "Pillion Footrest",
                        CompareSpec = new List<CompareBikeData>()
                    };


                    //Pillion Backrest
                    CompareSubCategory PillionBackrest = new CompareSubCategory {
                        Text = "Pillion Backrest",
                        Value = "Pillion Backrest",
                        CompareSpec = new List<CompareBikeData>()
                    };


                    //Pillion Grabrail
                    CompareSubCategory PillionGrabrail = new CompareSubCategory
                    {
                        Text = "Pillion Grabrail",
                        Value = "Pillion Grabrail",
                        CompareSpec = new List<CompareBikeData>()
                    };

                    //Stand Alarm
                    CompareSubCategory StandAlarm = new CompareSubCategory {
                        Text = "Stand Alarm",
                        Value = "Stand Alarm",
                        CompareSpec = new List<CompareBikeData>()
                    };

                    //Stepped Seat
                    CompareSubCategory SteppedSeat = new CompareSubCategory {
                        Text = "Stepped Seat",
                        Value = "Stepped Seat",
                        CompareSpec = new List<CompareBikeData>()
                    };

                    //Antilock Braking System
                    CompareSubCategory AntilockBrakingSystem = new CompareSubCategory {
                        Text = "Antilock Braking System",
                        Value = "Antilock Braking System",
                        CompareSpec = new List<CompareBikeData>()
                    };

                    //Killswitch
                    CompareSubCategory Killswitch = new CompareSubCategory {
                        Text = "Killswitch",
                        Value = "Killswitch",
                        CompareSpec = new List<CompareBikeData>()
                    };


                    //Clock
                    CompareSubCategory Clock = new CompareSubCategory {
                        Text = "Clock",
                        Value = "Clock",
                        CompareSpec = new List<CompareBikeData>()
                    };

                    foreach (var version in arrVersion)
                    {
                        var feature = compareEntity.Features.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version));
                        if(feature != null)
                        {
                            Speedometer.CompareSpec.Add(new CompareBikeData() { Value = Bikewale.Utility.FormatMinSpecs.ShowAvailable(compareEntity.Features.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version)).Speedometer) });
                            Tachometer.CompareSpec.Add(new CompareBikeData() { Value = Bikewale.Utility.FormatMinSpecs.ShowAvailable(compareEntity.Features.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version)).Tachometer) });
                            TachometerType.CompareSpec.Add(new CompareBikeData() { Value = Bikewale.Utility.FormatMinSpecs.ShowAvailable(compareEntity.Features.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version)).TachometerType) });
                            ShiftLight.CompareSpec.Add(new CompareBikeData() { Value = Bikewale.Utility.FormatMinSpecs.ShowAvailable(compareEntity.Features.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version)).ShiftLight) });
                            ElectricStart.CompareSpec.Add(new CompareBikeData() { Value = Bikewale.Utility.FormatMinSpecs.ShowAvailable(compareEntity.Features.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version)).ElectricStart) });
                            Tripmeter.CompareSpec.Add(new CompareBikeData() { Value = Bikewale.Utility.FormatMinSpecs.ShowAvailable(compareEntity.Features.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version)).Tripmeter) });
                            NoOfTripmeters.CompareSpec.Add(new CompareBikeData() { Value = Bikewale.Utility.FormatMinSpecs.ShowAvailable(compareEntity.Features.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version)).NoOfTripmeters) });
                            TripmeterType.CompareSpec.Add(new CompareBikeData() { Value = Bikewale.Utility.FormatMinSpecs.ShowAvailable(compareEntity.Features.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version)).TripmeterType) });
                            LowFuelIndicator.CompareSpec.Add(new CompareBikeData() { Value = Bikewale.Utility.FormatMinSpecs.ShowAvailable(compareEntity.Features.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version)).LowFuelIndicator) });
                            LowOilIndicator.CompareSpec.Add(new CompareBikeData() { Value = Bikewale.Utility.FormatMinSpecs.ShowAvailable(compareEntity.Features.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version)).LowOilIndicator) });
                            LowBatteryIndicator.CompareSpec.Add(new CompareBikeData() { Value = Bikewale.Utility.FormatMinSpecs.ShowAvailable(compareEntity.Features.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version)).LowBatteryIndicator) });
                            FuelGauge.CompareSpec.Add(new CompareBikeData() { Value = Bikewale.Utility.FormatMinSpecs.ShowAvailable(compareEntity.Features.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version)).FuelGauge) });
                            DigitalFuelGauges.CompareSpec.Add(new CompareBikeData() { Value = Bikewale.Utility.FormatMinSpecs.ShowAvailable(compareEntity.Features.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version)).DigitalFuelGauge) });
                            PillionSeat.CompareSpec.Add(new CompareBikeData() { Value = Bikewale.Utility.FormatMinSpecs.ShowAvailable(compareEntity.Features.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version)).PillionSeat) });
                            PillionFootrest.CompareSpec.Add(new CompareBikeData() { Value = Bikewale.Utility.FormatMinSpecs.ShowAvailable(compareEntity.Features.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version)).PillionFootrest) });
                            PillionBackrest.CompareSpec.Add(new CompareBikeData() { Value = Bikewale.Utility.FormatMinSpecs.ShowAvailable(compareEntity.Features.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version)).PillionBackrest) });
                            PillionGrabrail.CompareSpec.Add(new CompareBikeData() { Value = Bikewale.Utility.FormatMinSpecs.ShowAvailable(compareEntity.Features.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version)).PillionGrabrail) });
                            StandAlarm.CompareSpec.Add(new CompareBikeData() { Value = Bikewale.Utility.FormatMinSpecs.ShowAvailable(compareEntity.Features.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version)).StandAlarm) });
                            SteppedSeat.CompareSpec.Add(new CompareBikeData() { Value = Bikewale.Utility.FormatMinSpecs.ShowAvailable(compareEntity.Features.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version)).SteppedSeat) });
                            AntilockBrakingSystem.CompareSpec.Add(new CompareBikeData() { Value = Bikewale.Utility.FormatMinSpecs.ShowAvailable(compareEntity.Features.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version)).AntilockBrakingSystem) });
                            Killswitch.CompareSpec.Add(new CompareBikeData() { Value = Bikewale.Utility.FormatMinSpecs.ShowAvailable(compareEntity.Features.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version)).Killswitch) });
                            Clock.CompareSpec.Add(new CompareBikeData() { Value = Bikewale.Utility.FormatMinSpecs.ShowAvailable(compareEntity.Features.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version)).Clock) });

                        }
                    }

                    features.SpecCategory = new List<CompareSubCategory> {
                        Speedometer
                        ,Tachometer
                        ,TachometerType
                        ,ShiftLight
                        ,ElectricStart
                        ,Tripmeter
                        ,NoOfTripmeters
                        ,TripmeterType
                        ,LowFuelIndicator
                        ,LowOilIndicator
                        ,LowBatteryIndicator
                        ,FuelGauge
                        ,DigitalFuelGauges
                        ,PillionSeat
                        ,PillionFootrest
                        ,PillionBackrest
                        ,PillionGrabrail
                        ,StandAlarm
                        ,SteppedSeat
                        ,AntilockBrakingSystem
                        ,Killswitch
                        ,Clock
                    };

                    compareFeatures.Spec.Add(features);
                    compareEntity.CompareFeatures = compareFeatures;
                    #endregion

                    CompareBikeColorCategory compareColors = new CompareBikeColorCategory {
                        Text = "Colours",
                        Value = "Colours",
                        bikes = new List<CompareBikeColor>()
                    };
                    #region Colors
                    foreach (var version in arrVersion)
                    {
                        var objBikeColor = new List<BikeColor>();
                        foreach (var color in compareEntity.Color)
                        {
                            if (color.VersionId == Convert.ToUInt32(version))
                            {
                                objBikeColor.Add(color);
                            }
                        }
                        compareColors.bikes.Add(new CompareBikeColor() { bikeColors = objBikeColor.GroupBy(p => p.ColorId).Select(grp => grp.First()).ToList<BikeColor>() });
                    }

                    compareEntity.CompareColors = compareColors;
                    #endregion

                    #region Reviews

                    if (compareEntity.Reviews != null && compareEntity.Reviews.Any())
                    {

                        compareEntity.UserReviewData = new CompareReviewsData();

                        compareEntity.UserReviewData.CompareReviews = new CompareMainCategory();
                        compareEntity.UserReviewData.CompareReviews.Text = "Reviews";
                        compareEntity.UserReviewData.CompareReviews.Value = "Reviews";
                        compareEntity.UserReviewData.CompareReviews.Spec = new List<CompareSubMainCategory>();



                        IList<UserReviewComparisonObject> objReviewList = new List<UserReviewComparisonObject>();

                        CompareSubMainCategory ratings = new CompareSubMainCategory();
                        ratings.Text = "Ratings";
                        ratings.Value = "Ratings";
                        ratings.SpecCategory = new List<CompareSubCategory>();

                        foreach (var version in arrVersion)
                        {
                            UserReviewComparisonObject objReview = new UserReviewComparisonObject();
                            var reviewObj = compareEntity.Reviews.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version));
                            var basicInfoObj = compareEntity.BasicInfo != null ? compareEntity.BasicInfo.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version)) : null;
                            if (reviewObj != null)
                            {
                                objReview.ReviewRate = FormatMinSpecs.ShowAvailable(reviewObj.ModelReview.ReviewRate.ToString("0.0"));
                                objReview.RatingCount = FormatMinSpecs.ShowAvailable(reviewObj.ModelReview.RatingCount.ToString());
                                objReview.ReviewCount = FormatMinSpecs.ShowAvailable(reviewObj.ModelReview.ReviewCount.ToString());

                                if (basicInfoObj != null && reviewObj.ModelReview != null && reviewObj.ModelReview.UserReviews != null)
                                    objReview.ReviewListUrl = string.Format("/{0}-bikes/{1}/reviews/", basicInfoObj.MakeMaskingName, basicInfoObj.ModelMaskingName);
                            }
                            else
                            {
                                objReview.ReviewRate = FormatMinSpecs.ShowAvailable("");
                                objReview.RatingCount = FormatMinSpecs.ShowAvailable("");
                                objReview.ReviewCount = FormatMinSpecs.ShowAvailable("");
                            }
                            objReviewList.Add(objReview);
                        }
                        compareEntity.UserReviewData.OverallRating = objReviewList;

                        //Mileage
                        CompareSubCategory mileage = new CompareSubCategory();
                        mileage.Text = "Mileage by Users";
                        mileage.Value = "Mileage by Users ";
                        mileage.CompareSpec = new List<CompareBikeData>();
                        int valuesCount = 0;
                        foreach (var version in arrVersion)
                        {
                            var reviewsObj = compareEntity.Reviews.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version));
                            if (reviewsObj != null)
                            {
                                mileage.CompareSpec.Add(new CompareBikeData() { Value = FormatMinSpecs.ShowAvailable(reviewsObj.ModelReview.Mileage), Text = Bikewale.Utility.FormatMinSpecs.ShowAvailable(reviewsObj.ModelReview.Mileage) });

                                if (reviewsObj.ModelReview.Mileage > 0)
                                    valuesCount++;
                            }
                            else
                                mileage.CompareSpec.Add(new CompareBikeData() { Value = FormatMinSpecs.ShowAvailable(""), Text = Bikewale.Utility.FormatMinSpecs.ShowAvailable("") });
                        }
                        if (valuesCount > 1)
                            ratings.SpecCategory.Add(mileage);
                        #endregion
                        compareEntity.UserReviewData.CompareReviews.Spec.Add(ratings);
                        #region Performance Paramters
                        CompareSubMainCategory performanceParameters = new CompareSubMainCategory();
                        performanceParameters.Text = "Performance Parameters";
                        performanceParameters.Value = "Performance Parameters";
                        performanceParameters.SpecCategory = new List<CompareSubCategory>();
                        //visual Appeal
                        CompareSubCategory visualAppeal = new CompareSubCategory();
                        visualAppeal.Text = "Visual appeal";
                        visualAppeal.Value = "Visual appeal";
                        visualAppeal.CompareSpec = new List<CompareBikeData>();
                        valuesCount = 0;
                        foreach (var version in arrVersion)
                        {
                            var firstRow = compareEntity.Reviews.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version));
                            var objQuestion = firstRow != null && firstRow.ModelReview.Questions != null ? firstRow.ModelReview.Questions.FirstOrDefault(m => m.QuestionId == 4) : null;
                            if (firstRow != null && objQuestion != null)
                            {
                                visualAppeal.CompareSpec.Add(new CompareBikeData() { Value = Bikewale.Utility.FormatMinSpecs.ShowAvailable(objQuestion.AverageRatingValue.ToString()), Text = Bikewale.Utility.FormatMinSpecs.ShowAvailable(objQuestion.AverageRatingValue.ToString()) });

                                if (objQuestion.AverageRatingValue > 0)
                                    valuesCount++;
                            }
                            else
                                visualAppeal.CompareSpec.Add(new CompareBikeData() { Value = Bikewale.Utility.FormatMinSpecs.ShowAvailable(""), Text = Bikewale.Utility.FormatMinSpecs.ShowAvailable("") });
                        }
                        if (valuesCount > 1)
                            performanceParameters.SpecCategory.Add(visualAppeal);
                        // Reliability
                        CompareSubCategory reliability = new CompareSubCategory();
                        reliability.Text = "Reliability";
                        reliability.Value = "Reliability";
                        reliability.CompareSpec = new List<CompareBikeData>();
                        valuesCount = 0;
                        foreach (var version in arrVersion)
                        {
                            var firstRow = compareEntity.Reviews.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version));
                            var objQuestion = firstRow != null && firstRow.ModelReview.Questions != null ? firstRow.ModelReview.Questions.FirstOrDefault(m => m.QuestionId == 5) : null;
                            if (firstRow != null && objQuestion != null)
                            {
                                reliability.CompareSpec.Add(new CompareBikeData() { Value = Bikewale.Utility.FormatMinSpecs.ShowAvailable(objQuestion.AverageRatingValue.ToString()), Text = Bikewale.Utility.FormatMinSpecs.ShowAvailable(objQuestion.AverageRatingValue.ToString()) });

                                if (objQuestion.AverageRatingValue > 0)
                                    valuesCount++;
                            }
                            else
                                reliability.CompareSpec.Add(new CompareBikeData() { Value = Bikewale.Utility.FormatMinSpecs.ShowAvailable(""), Text = Bikewale.Utility.FormatMinSpecs.ShowAvailable("") });
                        }
                        if (valuesCount > 1)
                            performanceParameters.SpecCategory.Add(reliability);
                        // Performance
                        CompareSubCategory performance = new CompareSubCategory();
                        performance.Text = "Performance";
                        performance.Value = "Performance";
                        performance.CompareSpec = new List<CompareBikeData>();
                        valuesCount = 0;
                        foreach (var version in arrVersion)
                        {
                            var firstRow = compareEntity.Reviews.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version));
                            var objQuestion = firstRow != null && firstRow.ModelReview.Questions != null ? firstRow.ModelReview.Questions.FirstOrDefault(m => m.QuestionId == 6) : null;
                            if (firstRow != null && objQuestion != null)
                            {
                                performance.CompareSpec.Add(new CompareBikeData() { Value = Bikewale.Utility.FormatMinSpecs.ShowAvailable(objQuestion.AverageRatingValue.ToString()), Text = Bikewale.Utility.FormatMinSpecs.ShowAvailable(objQuestion.AverageRatingValue.ToString()) });

                                if (objQuestion.AverageRatingValue > 0)
                                    valuesCount++;
                            }
                            else
                                performance.CompareSpec.Add(new CompareBikeData() { Value = Bikewale.Utility.FormatMinSpecs.ShowAvailable(""), Text = Bikewale.Utility.FormatMinSpecs.ShowAvailable("") });
                        }
                        if (valuesCount > 1)
                            performanceParameters.SpecCategory.Add(performance);
                        // Comfort
                        CompareSubCategory comfort = new CompareSubCategory();
                        comfort.Text = "Comfort";
                        comfort.Value = "Comfort";
                        comfort.CompareSpec = new List<CompareBikeData>();
                        valuesCount = 0;
                        foreach (var version in arrVersion)
                        {
                            var firstRow = compareEntity.Reviews.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version));
                            var objQuestion = firstRow != null && firstRow.ModelReview.Questions != null ? firstRow.ModelReview.Questions.FirstOrDefault(m => m.QuestionId == 7) : null;
                            if (firstRow != null && objQuestion != null)
                            {
                                comfort.CompareSpec.Add(new CompareBikeData() { Value = Bikewale.Utility.FormatMinSpecs.ShowAvailable(objQuestion.AverageRatingValue.ToString()), Text = Bikewale.Utility.FormatMinSpecs.ShowAvailable(objQuestion.AverageRatingValue.ToString()) });

                                if (objQuestion.AverageRatingValue > 0)
                                    valuesCount++;
                            }
                            else
                                comfort.CompareSpec.Add(new CompareBikeData() { Value = Bikewale.Utility.FormatMinSpecs.ShowAvailable(""), Text = Bikewale.Utility.FormatMinSpecs.ShowAvailable("") });
                        }
                        if (valuesCount > 1)
                            performanceParameters.SpecCategory.Add(comfort);
                        // Service Experience
                        CompareSubCategory serviceExperience = new CompareSubCategory();
                        serviceExperience.Text = "Service Experience";
                        serviceExperience.Value = "Service Experience";
                        serviceExperience.CompareSpec = new List<CompareBikeData>();
                        valuesCount = 0;
                        foreach (var version in arrVersion)
                        {
                            var firstRow = compareEntity.Reviews.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version));
                            var objQuestion = firstRow != null && firstRow.ModelReview.Questions != null ? firstRow.ModelReview.Questions.FirstOrDefault(m => m.QuestionId == 8) : null;
                            if (firstRow != null && objQuestion != null)
                            {
                                serviceExperience.CompareSpec.Add(new CompareBikeData() { Value = Bikewale.Utility.FormatMinSpecs.ShowAvailable(objQuestion.AverageRatingValue.ToString()), Text = Bikewale.Utility.FormatMinSpecs.ShowAvailable(objQuestion.AverageRatingValue.ToString()) });

                                if (objQuestion.AverageRatingValue > 0)
                                    valuesCount++;
                            }
                            else
                                serviceExperience.CompareSpec.Add(new CompareBikeData() { Value = Bikewale.Utility.FormatMinSpecs.ShowAvailable(""), Text = Bikewale.Utility.FormatMinSpecs.ShowAvailable("") });
                        }
                        if (valuesCount > 1)
                            performanceParameters.SpecCategory.Add(serviceExperience);
                        // Maintenance Cost
                        CompareSubCategory maintenanceCost = new CompareSubCategory();
                        maintenanceCost.Text = "Maintenance Cost";
                        maintenanceCost.Value = "Maintenance Cost";
                        maintenanceCost.CompareSpec = new List<CompareBikeData>();
                        valuesCount = 0;
                        foreach (var version in arrVersion)
                        {
                            var firstRow = compareEntity.Reviews.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version));
                            var objQuestion = firstRow != null && firstRow.ModelReview.Questions != null ? firstRow.ModelReview.Questions.FirstOrDefault(m => m.QuestionId == 9) : null;
                            if (firstRow != null && objQuestion != null)
                            {
                                maintenanceCost.CompareSpec.Add(new CompareBikeData() { Value = Bikewale.Utility.FormatMinSpecs.ShowAvailable(objQuestion.AverageRatingValue.ToString()), Text = Bikewale.Utility.FormatMinSpecs.ShowAvailable(objQuestion.AverageRatingValue.ToString()) });

                                if (objQuestion.AverageRatingValue > 0)
                                    valuesCount++;
                            }
                            else
                                maintenanceCost.CompareSpec.Add(new CompareBikeData() { Value = Bikewale.Utility.FormatMinSpecs.ShowAvailable(""), Text = Bikewale.Utility.FormatMinSpecs.ShowAvailable("") });
                        }
                        if (valuesCount > 1)
                            performanceParameters.SpecCategory.Add(maintenanceCost);
                        // Value for Money
                        CompareSubCategory valueForMoney = new CompareSubCategory();
                        valueForMoney.Text = "Value for money";
                        valueForMoney.Value = "Value for money";
                        valueForMoney.CompareSpec = new List<CompareBikeData>();
                        valuesCount = 0;
                        foreach (var version in arrVersion)
                        {
                            var firstRow = compareEntity.Reviews.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version));
                            var objQuestion = firstRow != null && firstRow.ModelReview.Questions != null ? firstRow.ModelReview.Questions.FirstOrDefault(m => m.QuestionId == 10) : null;
                            if (firstRow != null && objQuestion != null)
                            {
                                valueForMoney.CompareSpec.Add(new CompareBikeData() { Value = Bikewale.Utility.FormatMinSpecs.ShowAvailable(objQuestion.AverageRatingValue.ToString()), Text = Bikewale.Utility.FormatMinSpecs.ShowAvailable(objQuestion.AverageRatingValue.ToString()) });

                                if (objQuestion.AverageRatingValue > 0)
                                    valuesCount++;
                            }
                            else
                                valueForMoney.CompareSpec.Add(new CompareBikeData() { Value = Bikewale.Utility.FormatMinSpecs.ShowAvailable(""), Text = Bikewale.Utility.FormatMinSpecs.ShowAvailable("") });
                        }
                        if (valuesCount > 1)
                            performanceParameters.SpecCategory.Add(valueForMoney);
                        // Extra Features
                        CompareSubCategory extraFeatures = new CompareSubCategory();
                        extraFeatures.Text = "Extra Features";
                        extraFeatures.Value = "Extra Features";
                        extraFeatures.CompareSpec = new List<CompareBikeData>();
                        valuesCount = 0;
                        foreach (var version in arrVersion)
                        {
                            var firstRow = compareEntity.Reviews.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version));
                            var objQuestion = firstRow != null && firstRow.ModelReview.Questions != null ? firstRow.ModelReview.Questions.FirstOrDefault(m => m.QuestionId == 11) : null;
                            if (firstRow != null && objQuestion != null)
                            {
                                extraFeatures.CompareSpec.Add(new CompareBikeData() { Value = Bikewale.Utility.FormatMinSpecs.ShowAvailable(objQuestion.AverageRatingValue.ToString()), Text = Bikewale.Utility.FormatMinSpecs.ShowAvailable(objQuestion.AverageRatingValue.ToString()) });

                                if (objQuestion.AverageRatingValue > 0)
                                    valuesCount++;
                            }
                            else
                                extraFeatures.CompareSpec.Add(new CompareBikeData() { Value = Bikewale.Utility.FormatMinSpecs.ShowAvailable(""), Text = Bikewale.Utility.FormatMinSpecs.ShowAvailable("") });
                        }
                        if (valuesCount > 1)
                            performanceParameters.SpecCategory.Add(extraFeatures);
                        #endregion

                        if (performanceParameters.SpecCategory.Any())
                            compareEntity.UserReviewData.CompareReviews.Spec.Add(performanceParameters);
                        #region Reviews
                        CompareSubMainCategory reviews = new CompareSubMainCategory();
                        reviews.Text = "Reviews";
                        reviews.Value = "Reviews";
                        reviews.SpecCategory = new List<CompareSubCategory>();
                        //Most Helpful

                        IList<ReviewObject> objRecentList = new List<ReviewObject>();

                        foreach (var version in arrVersion)
                        {
                            ReviewObject objReview = new ReviewObject();
                            var reviewObj = compareEntity.Reviews.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version));
                            var basicInfoObj = compareEntity.BasicInfo != null ? compareEntity.BasicInfo.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version)) : null;
                            if (reviewObj != null && basicInfoObj != null && reviewObj.ModelReview != null && reviewObj.ModelReview.UserReviews != null)
                            {
                                objReview.RatingValue = reviewObj.ModelReview.UserReviews.OverallRatingId;
                                objReview.ReviewDescription = FormatDescription.TruncateDescription(FormatDescription.SanitizeHtml(reviewObj.ModelReview.UserReviews.Description), 85);
                                objReview.ReviewTitle = FormatDescription.TruncateDescription(reviewObj.ModelReview.UserReviews.Title, 40);
                                objReview.ReviewListUrl = string.Format("/{0}-bikes/{1}/reviews/", basicInfoObj.MakeMaskingName, basicInfoObj.ModelMaskingName);
                                objReview.ReviewDetailUrl = string.Format("/{0}-bikes/{1}/reviews/{2}/", basicInfoObj.MakeMaskingName, basicInfoObj.ModelMaskingName, reviewObj.ModelReview.UserReviews.ReviewId);
                            }
                            objRecentList.Add(objReview);
                        }

                        compareEntity.UserReviewData.MostRecentReviews = objRecentList;

                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("Bikewale.BAL.Compare.BikeComparison.TransposeCompareBikeData - {0}", versions));
            }
        }


        /// <summary>
        /// Created By : Sushil Kumar on 2nd Dec 2016
        /// Description : CAll DAL Layer
        /// </summary>
        /// <param name="topCount"></param>
        /// <returns></returns>
        public IEnumerable<Entities.Compare.TopBikeCompareBase> CompareList(uint topCount)
        {
            return _objCompare.CompareList(topCount);
        }

        /// <summary>
        /// Created By : Sushil Kumar on 2nd Dec 2016
        /// Description : BAL layer to similar cache comaprisions bikes   
        /// Modified by : Aditi Srivastava on 5 June 2017
        /// Summary     : Added cache call instead of DAL
        /// </summary>
        /// <param name="versionList"></param>
        /// <param name="topCount"></param>
        /// <param name="cityid"></param>
        /// <returns></returns>
        public ICollection<SimilarCompareBikeEntity> GetSimilarCompareBikes(string versionList, ushort topCount, int cityid)
        {
            return _objCache.GetSimilarCompareBikes(versionList, topCount, cityid);
        }


        /// <summary>
        /// Created By : Sushil Kumar on 2nd Dec 2016
        /// Description : BAL layer to similar cache comaprisions bikes with sponsored comparision 
        /// </summary>
        /// <param name="versionList"></param>
        /// <param name="topCount"></param>
        /// <param name="cityid"></param>
        /// <param name="sponsoredVersionId"></param>
        /// <returns></returns>
        public ICollection<SimilarCompareBikeEntity> GetSimilarCompareBikeSponsored(string versionList, ushort topCount, int cityid, uint sponsoredVersionId)
        {
            return _objCompare.GetSimilarCompareBikeSponsored(versionList, topCount, cityid, sponsoredVersionId);
        }

        /// <summary>
        /// CReated By : Sushil Kumar on 2nd Feb 2017
        /// Description : Added methods for comparisions bikes binding using transpose methodology
        /// </summary>
        /// <param name="versions"></param>
        /// <param name="cityId"></param>
        /// <returns></returns>
        public BikeCompareEntity DoCompare(string versions, uint cityId)
        {
            BikeCompareEntity compareEntity = null;
            try
            {
                if (!string.IsNullOrEmpty(versions))
                {
                    compareEntity = _objCompare.DoCompare(versions, cityId);
                    TransposeCompareBikeData(ref compareEntity, versions);
                }
                if (compareEntity != null)
                {
                    compareEntity.Features = null;
                    compareEntity.Specifications = null;
                    compareEntity.Color = null;
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("Bikewale.BAL.Compare.BikeComparison.DoCompare - {0} - {1}", versions, cityId));
            }
            return compareEntity;
        }

        /// <summary>
        /// Created By :- Subodh Jain 10 March 2017
        /// Summary :- Populate Compare ScootersList
        /// </summary>
        public IEnumerable<TopBikeCompareBase> ScooterCompareList(uint topCount)
        {
            return _objCompare.ScooterCompareList(topCount);
        }

        /// <summary>
        /// Created by : Aditi Srivastava on 25 Apr 2017
        /// Summary    : Populate compare bikes list 
        /// Modified by: Aditi Srivastava 5 June 2017
        /// Summary    : randomize order of sponsored comparison first
        /// </summary>
        public IEnumerable<SimilarCompareBikeEntity> GetPopularCompareList(uint cityId)
        {
            IEnumerable<SimilarCompareBikeEntity> compareBikes = null;
            try
            {
                compareBikes = (List<SimilarCompareBikeEntity>)_objCache.GetPopularCompareList(cityId);
                if (compareBikes != null && compareBikes.Any())
                {
                    Random rnd = new Random();
                    compareBikes = compareBikes.Where(x => x.IsSponsored && x.SponsoredEndDate >= DateTime.Now && x.SponsoredStartDate <= DateTime.Now).OrderBy(x => rnd.Next())
                                   .Union(compareBikes.Where(x => !x.IsSponsored).OrderBy(x => x.DisplayPriority));
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("Bikewale.BAL.Compare.BikeComparison.GetPopularCompareList - CityId: {0}", cityId));
            }
            return compareBikes;
        }

        /// <summary>
        /// Created by : Aditi Srivastava on 5 Apr 2017
        /// Summary    : Populate compare scooters list 
        /// Modified by: Aditi Srivastava 5 June 2017
        /// Summary    : randomize order of sponsored comparison first
        /// </summary>
        public IEnumerable<SimilarCompareBikeEntity> GetScooterCompareList(uint cityId)
        {
            IEnumerable<SimilarCompareBikeEntity> compareScooters = null;
            try
            {
                compareScooters = (List<SimilarCompareBikeEntity>)_objCache.GetScooterCompareList(cityId);
                if (compareScooters != null && compareScooters.Any())
                {
                    Random rnd = new Random();
                    compareScooters = compareScooters.Where(x => x.IsSponsored && x.SponsoredEndDate >= DateTime.Now && x.SponsoredStartDate <= DateTime.Now).OrderBy(x => rnd.Next())
                                   .Union(compareScooters.Where(x => !x.IsSponsored).OrderBy(x => x.DisplayPriority));
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("Bikewale.BAL.Compare.BikeComparison.GetScooterCompareList - CityId: {0}", cityId));
            }
            return compareScooters;
        }

        public SimilarBikeComparisonWrapper GetSimilarBikes(string modelList, ushort topCount)
        {
                return _objCompare.GetSimilarBikes(modelList, topCount);
        }
    }

}

