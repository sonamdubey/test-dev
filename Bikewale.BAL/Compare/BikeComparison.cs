using Bikewale.DAL.Compare;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.Compare;
using Bikewale.Interfaces.Compare;
using Bikewale.Notifications;
using log4net;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bikewale.BAL.Compare
{
    /// <summary>
    /// Modified By : Sushil Kumar on 2nd Dec 2016
    /// Description : Added methods for featured bike and sponsored bike comparisions
    /// Modified By : Sushil Kumar on 2nd Feb 2017
    /// Description : Added methods for comparisions bikes binding using transpose methodology
    /// </summary>
    public class BikeComparison : IBikeCompare
    {
        private readonly IBikeCompare _objCompare = null;

        static bool _logGrpcErrors = Convert.ToBoolean(Bikewale.Utility.BWConfiguration.Instance.LogGrpcErrors);
        static readonly ILog _logger = LogManager.GetLogger(typeof(BikeComparison));
        static bool _useGrpc = Convert.ToBoolean(Bikewale.Utility.BWConfiguration.Instance.UseGrpc);


        public BikeComparison()
        {
            using (IUnityContainer objPQCont = new UnityContainer())
            {
                objPQCont.RegisterType<IBikeCompare, BikeCompareRepository>();
                _objCompare = objPQCont.Resolve<IBikeCompare>();
            }
        }


        /***********************************************************/
        // Input: Version id of Bikes selected by the user to compare
        // Output: Version id of featured Bike
        // Written By: Satish Sharma On 2009-09-29 5:40 PM
        // Modified By : Sadhana Upadhyay on 9 Sept 2014
        // Summary : to get sponsored bike by web api
        /***********************************************************/

        public Int64 GetFeaturedBike(string versions)
        {
            Int64 featuredBikeId = -1;

            try
            {
           
                //sets the base URI for HTTP requests
                string _apiUrl = String.Format("/webapi/SponsoredCarVersion/GetSponsoredCarVersion/?vids={0}&categoryId=1&platformId=2", versions);

                using (Utility.BWHttpClient objClient = new Utility.BWHttpClient())
                {
                    return objClient.GetApiResponseSync<Int64>(Utility.APIHost.CW, Utility.BWConfiguration.Instance.APIRequestTypeJSON, _apiUrl, featuredBikeId);
                }
            }
            catch (Exception err)
            {
                _logger.Error(err);
            }

            return featuredBikeId;
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
                ErrorClass objErr = new ErrorClass(ex, "DoCompare");
                objErr.SendMail();
            }
            return compareEntity;
        }

        /// <summary>
        /// Created By : Sushil Kumar on 2nd Feb 2017
        /// Description : Removed transpose logic from DoCompare and moved into common for both app and website
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
                    compareEntity.CompareSpecifications = new CompareMainCategory();
                    compareEntity.CompareSpecifications.Text = "Specifications";
                    compareEntity.CompareSpecifications.Value = "Specifications";
                    compareEntity.CompareSpecifications.Spec = new List<CompareSubMainCategory>();
                    #region Specifications
                    CompareSubMainCategory engineTransmission = new CompareSubMainCategory();
                    engineTransmission.Text = "Engine & Transmission";
                    engineTransmission.Value = "2";
                    engineTransmission.SpecCategory = new List<CompareSubCategory>();
                    #region Engine & Transmission
                    CompareSubCategory etDisplacement = new CompareSubCategory();
                    etDisplacement.Text = "Displacement (cc)";
                    etDisplacement.Value = "Displacement";
                    etDisplacement.CompareSpec = new List<CompareBikeData>();
                    foreach (var version in arrVersion)
                    {
                        etDisplacement.CompareSpec.Add(new CompareBikeData() { Value = Bikewale.Utility.FormatMinSpecs.ShowAvailable(compareEntity.Specifications.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version)).Displacement.Value.ToString(), "cc"), Text = Bikewale.Utility.FormatMinSpecs.ShowAvailable(compareEntity.Specifications.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version)).Displacement.Value.ToString(), "cc") });
                    }
                    engineTransmission.SpecCategory.Add(etDisplacement);

                    CompareSubCategory etCylinders = new CompareSubCategory();
                    etCylinders.Text = "Cylinders";
                    etCylinders.Value = "Cylinders";
                    etCylinders.CompareSpec = new List<CompareBikeData>();
                    foreach (var version in arrVersion)
                    {
                        etCylinders.CompareSpec.Add(new CompareBikeData() { Value = Bikewale.Utility.FormatMinSpecs.ShowAvailable(compareEntity.Specifications.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version)).Cylinders.Value.ToString()), Text = Bikewale.Utility.FormatMinSpecs.ShowAvailable(compareEntity.Specifications.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version)).Cylinders.Value.ToString()) });
                    }
                    engineTransmission.SpecCategory.Add(etCylinders);

                    CompareSubCategory etMaxPower = new CompareSubCategory();
                    etMaxPower.Text = "Max Power";
                    etMaxPower.Value = "Max Power";
                    etMaxPower.CompareSpec = new List<CompareBikeData>();
                    foreach (var version in arrVersion)
                    {
                        etMaxPower.CompareSpec.Add(new CompareBikeData() { Value = Bikewale.Utility.FormatMinSpecs.ShowAvailable(compareEntity.Specifications.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version)).MaxPower.Value, "bhp", compareEntity.Specifications.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version)).MaxPowerRpm.Value, "rpm"), Text = Bikewale.Utility.FormatMinSpecs.ShowAvailable(compareEntity.Specifications.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version)).MaxPower.Value, "bhp", compareEntity.Specifications.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version)).MaxPowerRpm.Value, "rpm") });
                    }
                    engineTransmission.SpecCategory.Add(etMaxPower);

                    CompareSubCategory etMaximumTorque = new CompareSubCategory();
                    etMaximumTorque.Text = "Maximum Torque";
                    etMaximumTorque.Value = "Maximum Torque";
                    etMaximumTorque.CompareSpec = new List<CompareBikeData>();
                    foreach (var version in arrVersion)
                    {
                        etMaximumTorque.CompareSpec.Add(new CompareBikeData() { Value = Bikewale.Utility.FormatMinSpecs.ShowAvailable(compareEntity.Specifications.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version)).MaximumTorque.Value, "Nm", compareEntity.Specifications.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version)).MaximumTorqueRpm.Value, "rpm"), Text = Bikewale.Utility.FormatMinSpecs.ShowAvailable(compareEntity.Specifications.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version)).MaximumTorque.Value, "Nm", compareEntity.Specifications.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version)).MaximumTorqueRpm.Value, "rpm") });
                    }
                    engineTransmission.SpecCategory.Add(etMaximumTorque);

                    CompareSubCategory etBore = new CompareSubCategory();
                    etBore.Text = "Bore (mm)";
                    etBore.Value = "Bore (mm)";
                    etBore.CompareSpec = new List<CompareBikeData>();
                    foreach (var version in arrVersion)
                    {
                        etBore.CompareSpec.Add(new CompareBikeData() { Value = Bikewale.Utility.FormatMinSpecs.ShowAvailable(compareEntity.Specifications.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version)).Bore.Value), Text = Bikewale.Utility.FormatMinSpecs.ShowAvailable(compareEntity.Specifications.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version)).Bore.Value) });
                    }
                    engineTransmission.SpecCategory.Add(etBore);

                    CompareSubCategory etStroke = new CompareSubCategory();
                    etStroke.Text = "Stroke (mm)";
                    etStroke.Value = "Stroke (mm)";
                    etStroke.CompareSpec = new List<CompareBikeData>();
                    foreach (var version in arrVersion)
                    {
                        etStroke.CompareSpec.Add(new CompareBikeData() { Value = Bikewale.Utility.FormatMinSpecs.ShowAvailable(compareEntity.Specifications.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version)).Stroke.Value), Text = Bikewale.Utility.FormatMinSpecs.ShowAvailable(compareEntity.Specifications.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version)).Stroke.Value) });
                    }
                    engineTransmission.SpecCategory.Add(etStroke);

                    CompareSubCategory etValvesPerCylinder = new CompareSubCategory();
                    etValvesPerCylinder.Text = "Valves Per Cylinder";
                    etValvesPerCylinder.Value = "Valves Per Cylinder";
                    etValvesPerCylinder.CompareSpec = new List<CompareBikeData>();
                    foreach (var version in arrVersion)
                    {
                        etValvesPerCylinder.CompareSpec.Add(new CompareBikeData() { Value = Bikewale.Utility.FormatMinSpecs.ShowAvailable(compareEntity.Specifications.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version)).ValvesPerCylinder.Value), Text = Bikewale.Utility.FormatMinSpecs.ShowAvailable(compareEntity.Specifications.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version)).ValvesPerCylinder.Value) });
                    }
                    engineTransmission.SpecCategory.Add(etValvesPerCylinder);

                    CompareSubCategory etFuelDeliverySystem = new CompareSubCategory();
                    etFuelDeliverySystem.Text = "Fuel Delivery System";
                    etFuelDeliverySystem.Value = "Fuel Delivery System";
                    etFuelDeliverySystem.CompareSpec = new List<CompareBikeData>();
                    foreach (var version in arrVersion)
                    {
                        etFuelDeliverySystem.CompareSpec.Add(new CompareBikeData() { Value = Bikewale.Utility.FormatMinSpecs.ShowAvailable(compareEntity.Specifications.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version)).FuelDeliverySystem), Text = Bikewale.Utility.FormatMinSpecs.ShowAvailable(compareEntity.Specifications.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version)).FuelDeliverySystem) });
                    }
                    engineTransmission.SpecCategory.Add(etFuelDeliverySystem);

                    CompareSubCategory etFuelType = new CompareSubCategory();
                    etFuelType.Text = "Fuel Type";
                    etFuelType.Value = "Fuel Type";
                    etFuelType.CompareSpec = new List<CompareBikeData>();
                    foreach (var version in arrVersion)
                    {
                        etFuelType.CompareSpec.Add(new CompareBikeData() { Value = Bikewale.Utility.FormatMinSpecs.ShowAvailable(compareEntity.Specifications.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version)).FuelType), Text = Bikewale.Utility.FormatMinSpecs.ShowAvailable(compareEntity.Specifications.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version)).FuelType) });
                    }
                    engineTransmission.SpecCategory.Add(etFuelType);

                    CompareSubCategory etIgnition = new CompareSubCategory();
                    etIgnition.Text = "Ignition";
                    etIgnition.Value = "Ignition";
                    etIgnition.CompareSpec = new List<CompareBikeData>();
                    foreach (var version in arrVersion)
                    {
                        etIgnition.CompareSpec.Add(new CompareBikeData() { Value = Bikewale.Utility.FormatMinSpecs.ShowAvailable(compareEntity.Specifications.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version)).Ignition), Text = Bikewale.Utility.FormatMinSpecs.ShowAvailable(compareEntity.Specifications.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version)).Ignition) });
                    }
                    engineTransmission.SpecCategory.Add(etIgnition);

                    CompareSubCategory etSparkPlugs = new CompareSubCategory();
                    etSparkPlugs.Text = "Spark Plugs (Per Cylinder)";
                    etSparkPlugs.Value = "Spark Plugs (Per Cylinder)";
                    etSparkPlugs.CompareSpec = new List<CompareBikeData>();
                    foreach (var version in arrVersion)
                    {
                        etSparkPlugs.CompareSpec.Add(new CompareBikeData() { Value = Bikewale.Utility.FormatMinSpecs.ShowAvailable(compareEntity.Specifications.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version)).SparkPlugsPerCylinder), Text = Bikewale.Utility.FormatMinSpecs.ShowAvailable(compareEntity.Specifications.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version)).SparkPlugsPerCylinder) });
                    }
                    engineTransmission.SpecCategory.Add(etSparkPlugs);

                    CompareSubCategory etCoolingSystem = new CompareSubCategory();
                    etCoolingSystem.Text = "Cooling System";
                    etCoolingSystem.Value = "Cooling System";
                    etCoolingSystem.CompareSpec = new List<CompareBikeData>();
                    foreach (var version in arrVersion)
                    {
                        etCoolingSystem.CompareSpec.Add(new CompareBikeData() { Value = Bikewale.Utility.FormatMinSpecs.ShowAvailable(compareEntity.Specifications.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version)).CoolingSystem), Text = Bikewale.Utility.FormatMinSpecs.ShowAvailable(compareEntity.Specifications.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version)).CoolingSystem) });
                    }
                    engineTransmission.SpecCategory.Add(etCoolingSystem);

                    CompareSubCategory etGearBox = new CompareSubCategory();
                    etGearBox.Text = "Gearbox Type";
                    etGearBox.Value = "Gearbox Type";
                    etGearBox.CompareSpec = new List<CompareBikeData>();
                    foreach (var version in arrVersion)
                    {
                        etGearBox.CompareSpec.Add(new CompareBikeData() { Value = Bikewale.Utility.FormatMinSpecs.ShowAvailable(compareEntity.Specifications.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version)).GearboxType), Text = Bikewale.Utility.FormatMinSpecs.ShowAvailable(compareEntity.Specifications.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version)).GearboxType) });
                    }
                    engineTransmission.SpecCategory.Add(etGearBox);

                    CompareSubCategory etNoGears = new CompareSubCategory();
                    etNoGears.Text = "No. of gears";
                    etNoGears.Value = "No. of gears";
                    etNoGears.CompareSpec = new List<CompareBikeData>();
                    foreach (var version in arrVersion)
                    {
                        etNoGears.CompareSpec.Add(new CompareBikeData() { Value = Bikewale.Utility.FormatMinSpecs.ShowAvailable(compareEntity.Specifications.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version)).NoOfGears.Value), Text = Bikewale.Utility.FormatMinSpecs.ShowAvailable(compareEntity.Specifications.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version)).NoOfGears.Value) });
                    }
                    engineTransmission.SpecCategory.Add(etNoGears);

                    CompareSubCategory etTransmissionType = new CompareSubCategory();
                    etTransmissionType.Text = "Transmission Type";
                    etTransmissionType.Value = "Transmission Type";
                    etTransmissionType.CompareSpec = new List<CompareBikeData>();
                    foreach (var version in arrVersion)
                    {
                        etTransmissionType.CompareSpec.Add(new CompareBikeData() { Value = Bikewale.Utility.FormatMinSpecs.ShowAvailable(compareEntity.Specifications.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version)).TransmissionType), Text = Bikewale.Utility.FormatMinSpecs.ShowAvailable(compareEntity.Specifications.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version)).TransmissionType) });
                    }
                    engineTransmission.SpecCategory.Add(etTransmissionType);

                    CompareSubCategory etClutch = new CompareSubCategory();
                    etClutch.Text = "Clutch";
                    etClutch.Value = "Clutch";
                    etClutch.CompareSpec = new List<CompareBikeData>();
                    foreach (var version in arrVersion)
                    {
                        etClutch.CompareSpec.Add(new CompareBikeData() { Value = Bikewale.Utility.FormatMinSpecs.ShowAvailable(compareEntity.Specifications.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version)).Clutch), Text = Bikewale.Utility.FormatMinSpecs.ShowAvailable(compareEntity.Specifications.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version)).Clutch) });
                    }
                    engineTransmission.SpecCategory.Add(etClutch);
                    #endregion
                    compareEntity.CompareSpecifications.Spec.Add(engineTransmission);

                    CompareSubMainCategory brakesWheelsSuspension = new CompareSubMainCategory();
                    brakesWheelsSuspension.Text = "Brakes, Wheels and Suspension";
                    brakesWheelsSuspension.Value = "3";
                    brakesWheelsSuspension.SpecCategory = new List<CompareSubCategory>();
                    #region Brakes, Wheels and Suspension
                    CompareSubCategory bwsBreakType = new CompareSubCategory();
                    bwsBreakType.Text = "Brake Type";
                    bwsBreakType.Value = "Brake Type";
                    bwsBreakType.CompareSpec = new List<CompareBikeData>();
                    foreach (var version in arrVersion)
                    {
                        bwsBreakType.CompareSpec.Add(new CompareBikeData() { Value = Bikewale.Utility.FormatMinSpecs.ShowAvailable(compareEntity.Specifications.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version)).BrakeType), Text = Bikewale.Utility.FormatMinSpecs.ShowAvailable(compareEntity.Specifications.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version)).BrakeType) });
                    }
                    brakesWheelsSuspension.SpecCategory.Add(bwsBreakType);

                    CompareSubCategory bwsFrontDisc = new CompareSubCategory();
                    bwsFrontDisc.Text = "Front Disc";
                    bwsFrontDisc.Value = "Front Disc";
                    bwsFrontDisc.CompareSpec = new List<CompareBikeData>();
                    foreach (var version in arrVersion)
                    {
                        bwsFrontDisc.CompareSpec.Add(new CompareBikeData() { Value = Bikewale.Utility.FormatMinSpecs.ShowAvailable(compareEntity.Specifications.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version)).FrontDisc.Value), Text = Bikewale.Utility.FormatMinSpecs.ShowAvailable(compareEntity.Specifications.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version)).FrontDisc.Value) });
                    }
                    brakesWheelsSuspension.SpecCategory.Add(bwsFrontDisc);

                    CompareSubCategory bwsFrontDisc_DrumSize = new CompareSubCategory();
                    bwsFrontDisc_DrumSize.Text = "Front Disc/Drum Size (mm)";
                    bwsFrontDisc_DrumSize.Value = "Front Disc/Drum Size (mm)";
                    bwsFrontDisc_DrumSize.CompareSpec = new List<CompareBikeData>();
                    foreach (var version in arrVersion)
                    {
                        bwsFrontDisc_DrumSize.CompareSpec.Add(new CompareBikeData() { Value = Bikewale.Utility.FormatMinSpecs.ShowAvailable(compareEntity.Specifications.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version)).FrontDisc_DrumSize.Value), Text = Bikewale.Utility.FormatMinSpecs.ShowAvailable(compareEntity.Specifications.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version)).FrontDisc_DrumSize.Value) });
                    }
                    brakesWheelsSuspension.SpecCategory.Add(bwsFrontDisc_DrumSize);

                    CompareSubCategory bwsRearDisc = new CompareSubCategory();
                    bwsRearDisc.Text = "Rear Disc";
                    bwsRearDisc.Value = "Rear Disc";
                    bwsRearDisc.CompareSpec = new List<CompareBikeData>();
                    foreach (var version in arrVersion)
                    {
                        bwsRearDisc.CompareSpec.Add(new CompareBikeData() { Value = Bikewale.Utility.FormatMinSpecs.ShowAvailable(compareEntity.Specifications.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version)).RearDisc.Value), Text = Bikewale.Utility.FormatMinSpecs.ShowAvailable(compareEntity.Specifications.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version)).RearDisc.Value) });
                    }
                    brakesWheelsSuspension.SpecCategory.Add(bwsRearDisc);

                    CompareSubCategory bwsRearDisc_DrumSize = new CompareSubCategory();
                    bwsRearDisc_DrumSize.Text = "Rear Disc/Drum Size (mm)";
                    bwsRearDisc_DrumSize.Value = "Rear Disc/Drum Size (mm)";
                    bwsRearDisc_DrumSize.CompareSpec = new List<CompareBikeData>();
                    foreach (var version in arrVersion)
                    {
                        bwsRearDisc_DrumSize.CompareSpec.Add(new CompareBikeData() { Value = Bikewale.Utility.FormatMinSpecs.ShowAvailable(compareEntity.Specifications.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version)).RearDisc_DrumSize.Value), Text = Bikewale.Utility.FormatMinSpecs.ShowAvailable(compareEntity.Specifications.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version)).RearDisc_DrumSize.Value) });
                    }
                    brakesWheelsSuspension.SpecCategory.Add(bwsRearDisc_DrumSize);

                    CompareSubCategory bwsCalliperType = new CompareSubCategory();
                    bwsCalliperType.Text = "Calliper Type";
                    bwsCalliperType.Value = "Calliper Type";
                    bwsCalliperType.CompareSpec = new List<CompareBikeData>();
                    foreach (var version in arrVersion)
                    {
                        bwsCalliperType.CompareSpec.Add(new CompareBikeData() { Value = Bikewale.Utility.FormatMinSpecs.ShowAvailable(compareEntity.Specifications.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version)).CalliperType), Text = Bikewale.Utility.FormatMinSpecs.ShowAvailable(compareEntity.Specifications.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version)).CalliperType) });
                    }
                    brakesWheelsSuspension.SpecCategory.Add(bwsCalliperType);

                    CompareSubCategory bwsWheelSize = new CompareSubCategory();
                    bwsWheelSize.Text = "Wheel Size (inches)";
                    bwsWheelSize.Value = "Wheel Size (inches)";
                    bwsWheelSize.CompareSpec = new List<CompareBikeData>();
                    foreach (var version in arrVersion)
                    {
                        bwsWheelSize.CompareSpec.Add(new CompareBikeData() { Value = Bikewale.Utility.FormatMinSpecs.ShowAvailable(compareEntity.Specifications.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version)).WheelSize.Value), Text = Bikewale.Utility.FormatMinSpecs.ShowAvailable(compareEntity.Specifications.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version)).WheelSize.Value) });
                    }
                    brakesWheelsSuspension.SpecCategory.Add(bwsWheelSize);

                    CompareSubCategory bwsFrontTyre = new CompareSubCategory();
                    bwsFrontTyre.Text = "Front Tyre";
                    bwsFrontTyre.Value = "Front Tyre";
                    bwsFrontTyre.CompareSpec = new List<CompareBikeData>();
                    foreach (var version in arrVersion)
                    {
                        bwsFrontTyre.CompareSpec.Add(new CompareBikeData() { Value = Bikewale.Utility.FormatMinSpecs.ShowAvailable(compareEntity.Specifications.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version)).FrontTyre), Text = Bikewale.Utility.FormatMinSpecs.ShowAvailable(compareEntity.Specifications.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version)).FrontTyre) });
                    }
                    brakesWheelsSuspension.SpecCategory.Add(bwsFrontTyre);

                    CompareSubCategory bwsRearTyre = new CompareSubCategory();
                    bwsRearTyre.Text = "Rear Tyre";
                    bwsRearTyre.Value = "Rear Tyre";
                    bwsRearTyre.CompareSpec = new List<CompareBikeData>();
                    foreach (var version in arrVersion)
                    {
                        bwsRearTyre.CompareSpec.Add(new CompareBikeData() { Value = Bikewale.Utility.FormatMinSpecs.ShowAvailable(compareEntity.Specifications.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version)).RearTyre), Text = Bikewale.Utility.FormatMinSpecs.ShowAvailable(compareEntity.Specifications.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version)).RearTyre) });
                    }
                    brakesWheelsSuspension.SpecCategory.Add(bwsRearTyre);

                    CompareSubCategory bwsTubelessTyres = new CompareSubCategory();
                    bwsTubelessTyres.Text = "Tubeless Tyres";
                    bwsTubelessTyres.Value = "Tubeless Tyres";
                    bwsTubelessTyres.CompareSpec = new List<CompareBikeData>();
                    foreach (var version in arrVersion)
                    {
                        bwsTubelessTyres.CompareSpec.Add(new CompareBikeData() { Value = Bikewale.Utility.FormatMinSpecs.ShowAvailable(compareEntity.Specifications.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version)).TubelessTyres.Value), Text = Bikewale.Utility.FormatMinSpecs.ShowAvailable(compareEntity.Specifications.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version)).TubelessTyres.Value) });
                    }
                    brakesWheelsSuspension.SpecCategory.Add(bwsTubelessTyres);

                    CompareSubCategory bwsRadialTyres = new CompareSubCategory();
                    bwsRadialTyres.Text = "Radial Tyres";
                    bwsRadialTyres.Value = "Radial Tyres";
                    bwsRadialTyres.CompareSpec = new List<CompareBikeData>();
                    foreach (var version in arrVersion)
                    {
                        bwsRadialTyres.CompareSpec.Add(new CompareBikeData() { Value = Bikewale.Utility.FormatMinSpecs.ShowAvailable(compareEntity.Specifications.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version)).RadialTyres.Value), Text = Bikewale.Utility.FormatMinSpecs.ShowAvailable(compareEntity.Specifications.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version)).RadialTyres.Value) });
                    }
                    brakesWheelsSuspension.SpecCategory.Add(bwsRadialTyres);

                    CompareSubCategory bwsAlloyWheels = new CompareSubCategory();
                    bwsAlloyWheels.Text = "Alloy Wheels";
                    bwsAlloyWheels.Value = "Alloy Wheels";
                    bwsAlloyWheels.CompareSpec = new List<CompareBikeData>();
                    foreach (var version in arrVersion)
                    {
                        bwsAlloyWheels.CompareSpec.Add(new CompareBikeData() { Value = Bikewale.Utility.FormatMinSpecs.ShowAvailable(compareEntity.Specifications.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version)).AlloyWheels.Value), Text = Bikewale.Utility.FormatMinSpecs.ShowAvailable(compareEntity.Specifications.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version)).AlloyWheels.Value) });
                    }
                    brakesWheelsSuspension.SpecCategory.Add(bwsAlloyWheels);

                    CompareSubCategory bwsFrontSuspension = new CompareSubCategory();
                    bwsFrontSuspension.Text = "Front Suspension";
                    bwsFrontSuspension.Value = "Front Suspension";
                    bwsFrontSuspension.CompareSpec = new List<CompareBikeData>();
                    foreach (var version in arrVersion)
                    {
                        bwsFrontSuspension.CompareSpec.Add(new CompareBikeData() { Value = Bikewale.Utility.FormatMinSpecs.ShowAvailable(compareEntity.Specifications.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version)).FrontSuspension), Text = Bikewale.Utility.FormatMinSpecs.ShowAvailable(compareEntity.Specifications.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version)).FrontSuspension) });
                    }
                    brakesWheelsSuspension.SpecCategory.Add(bwsFrontSuspension);

                    CompareSubCategory bwsRearSuspension = new CompareSubCategory();
                    bwsRearSuspension.Text = "Rear Suspension";
                    bwsRearSuspension.Value = "Rear Suspension";
                    bwsRearSuspension.CompareSpec = new List<CompareBikeData>();
                    foreach (var version in arrVersion)
                    {
                        bwsRearSuspension.CompareSpec.Add(new CompareBikeData() { Value = Bikewale.Utility.FormatMinSpecs.ShowAvailable(compareEntity.Specifications.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version)).RearSuspension), Text = Bikewale.Utility.FormatMinSpecs.ShowAvailable(compareEntity.Specifications.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version)).RearSuspension) });
                    }
                    brakesWheelsSuspension.SpecCategory.Add(bwsRearSuspension);

                    #endregion
                    compareEntity.CompareSpecifications.Spec.Add(brakesWheelsSuspension);

                    CompareSubMainCategory dimensionsChassis = new CompareSubMainCategory();
                    dimensionsChassis.Text = "Dimensions and Chassis";
                    dimensionsChassis.Value = "4";
                    dimensionsChassis.SpecCategory = new List<CompareSubCategory>();
                    #region Dimensions and Chassis
                    CompareSubCategory KerbWeight = new CompareSubCategory();
                    KerbWeight.Text = "Kerb Weight (Kg)";
                    KerbWeight.Value = "Kerb Weight (Kg)";
                    KerbWeight.CompareSpec = new List<CompareBikeData>();
                    foreach (var version in arrVersion)
                    {
                        KerbWeight.CompareSpec.Add(new CompareBikeData() { Value = Bikewale.Utility.FormatMinSpecs.ShowAvailable(compareEntity.Specifications.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version)).KerbWeight.Value), Text = Bikewale.Utility.FormatMinSpecs.ShowAvailable(compareEntity.Specifications.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version)).KerbWeight.Value) });
                    }
                    dimensionsChassis.SpecCategory.Add(KerbWeight);


                    CompareSubCategory OverallLength = new CompareSubCategory();
                    OverallLength.Text = "Overall Length (mm)";
                    OverallLength.Value = "Overall Length (mm)";
                    OverallLength.CompareSpec = new List<CompareBikeData>();
                    foreach (var version in arrVersion)
                    {
                        OverallLength.CompareSpec.Add(new CompareBikeData() { Value = Bikewale.Utility.FormatMinSpecs.ShowAvailable(compareEntity.Specifications.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version)).OverallLength.Value), Text = Bikewale.Utility.FormatMinSpecs.ShowAvailable(compareEntity.Specifications.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version)).OverallLength.Value) });
                    }
                    dimensionsChassis.SpecCategory.Add(OverallLength);

                    CompareSubCategory OverallWidth = new CompareSubCategory();
                    OverallWidth.Text = "Overall Width (mm)";
                    OverallWidth.Value = "Overall Width (mm)";
                    OverallWidth.CompareSpec = new List<CompareBikeData>();
                    foreach (var version in arrVersion)
                    {
                        OverallWidth.CompareSpec.Add(new CompareBikeData() { Value = Bikewale.Utility.FormatMinSpecs.ShowAvailable(compareEntity.Specifications.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version)).OverallWidth.Value), Text = Bikewale.Utility.FormatMinSpecs.ShowAvailable(compareEntity.Specifications.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version)).OverallWidth.Value) });
                    }
                    dimensionsChassis.SpecCategory.Add(OverallWidth);

                    CompareSubCategory OverallHeight = new CompareSubCategory();
                    OverallHeight.Text = "Overall Height (mm)";
                    OverallHeight.Value = "Overall Height (mm)";
                    OverallHeight.CompareSpec = new List<CompareBikeData>();
                    foreach (var version in arrVersion)
                    {
                        OverallHeight.CompareSpec.Add(new CompareBikeData() { Value = Bikewale.Utility.FormatMinSpecs.ShowAvailable(compareEntity.Specifications.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version)).OverallHeight.Value), Text = Bikewale.Utility.FormatMinSpecs.ShowAvailable(compareEntity.Specifications.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version)).OverallHeight.Value) });
                    }
                    dimensionsChassis.SpecCategory.Add(OverallHeight);

                    CompareSubCategory Wheelbase = new CompareSubCategory();
                    Wheelbase.Text = "Wheelbase (mm)";
                    Wheelbase.Value = "Wheelbase (mm)";
                    Wheelbase.CompareSpec = new List<CompareBikeData>();
                    foreach (var version in arrVersion)
                    {
                        Wheelbase.CompareSpec.Add(new CompareBikeData() { Value = Bikewale.Utility.FormatMinSpecs.ShowAvailable(compareEntity.Specifications.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version)).Wheelbase.Value), Text = Bikewale.Utility.FormatMinSpecs.ShowAvailable(compareEntity.Specifications.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version)).Wheelbase.Value) });
                    }
                    dimensionsChassis.SpecCategory.Add(Wheelbase);

                    CompareSubCategory GroundClearance = new CompareSubCategory();
                    GroundClearance.Text = "Ground Clearance (mm)";
                    GroundClearance.Value = "Ground Clearance (mm)";
                    GroundClearance.CompareSpec = new List<CompareBikeData>();
                    foreach (var version in arrVersion)
                    {
                        GroundClearance.CompareSpec.Add(new CompareBikeData() { Value = Bikewale.Utility.FormatMinSpecs.ShowAvailable(compareEntity.Specifications.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version)).GroundClearance.Value), Text = Bikewale.Utility.FormatMinSpecs.ShowAvailable(compareEntity.Specifications.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version)).GroundClearance.Value) });
                    }
                    dimensionsChassis.SpecCategory.Add(GroundClearance);

                    CompareSubCategory SeatHeight = new CompareSubCategory();
                    SeatHeight.Text = "Seat Height (mm)";
                    SeatHeight.Value = "Seat Height (mm)";
                    SeatHeight.CompareSpec = new List<CompareBikeData>();
                    foreach (var version in arrVersion)
                    {
                        SeatHeight.CompareSpec.Add(new CompareBikeData() { Value = Bikewale.Utility.FormatMinSpecs.ShowAvailable(compareEntity.Specifications.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version)).SeatHeight.Value), Text = Bikewale.Utility.FormatMinSpecs.ShowAvailable(compareEntity.Specifications.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version)).SeatHeight.Value) });
                    }
                    dimensionsChassis.SpecCategory.Add(SeatHeight);
                    #endregion
                    compareEntity.CompareSpecifications.Spec.Add(dimensionsChassis);

                    CompareSubMainCategory fuelEfficiencyPerformance = new CompareSubMainCategory();
                    fuelEfficiencyPerformance.Text = "Fuel efficiency and Performance";
                    fuelEfficiencyPerformance.Value = "5";
                    fuelEfficiencyPerformance.SpecCategory = new List<CompareSubCategory>();
                    #region Fuel efficiency and Performance
                    CompareSubCategory FuelTankCapacity = new CompareSubCategory();
                    FuelTankCapacity.Text = "Fuel Tank Capacity (Litres)";
                    FuelTankCapacity.Value = "Fuel Tank Capacity (Litres)";
                    FuelTankCapacity.CompareSpec = new List<CompareBikeData>();
                    foreach (var version in arrVersion)
                    {
                        FuelTankCapacity.CompareSpec.Add(new CompareBikeData() { Value = Bikewale.Utility.FormatMinSpecs.ShowAvailable(compareEntity.Specifications.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version)).FuelTankCapacity.Value), Text = Bikewale.Utility.FormatMinSpecs.ShowAvailable(compareEntity.Specifications.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version)).FuelTankCapacity.Value) });
                    }
                    fuelEfficiencyPerformance.SpecCategory.Add(FuelTankCapacity);

                    CompareSubCategory ReserveFuelCapacity = new CompareSubCategory();
                    ReserveFuelCapacity.Text = "Reserve Fuel Capacity (Litres)";
                    ReserveFuelCapacity.Value = "Reserve Fuel Capacity (Litres)";
                    ReserveFuelCapacity.CompareSpec = new List<CompareBikeData>();
                    foreach (var version in arrVersion)
                    {
                        ReserveFuelCapacity.CompareSpec.Add(new CompareBikeData() { Value = Bikewale.Utility.FormatMinSpecs.ShowAvailable(compareEntity.Specifications.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version)).ReserveFuelCapacity.Value), Text = Bikewale.Utility.FormatMinSpecs.ShowAvailable(compareEntity.Specifications.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version)).ReserveFuelCapacity.Value) });
                    }
                    fuelEfficiencyPerformance.SpecCategory.Add(ReserveFuelCapacity);

                    CompareSubCategory FuelEfficiencyOverall = new CompareSubCategory();
                    FuelEfficiencyOverall.Text = "FuelEfficiency Overall (Kmpl)";
                    FuelEfficiencyOverall.Value = "FuelEfficiency Overall (Kmpl)";
                    FuelEfficiencyOverall.CompareSpec = new List<CompareBikeData>();
                    foreach (var version in arrVersion)
                    {
                        FuelEfficiencyOverall.CompareSpec.Add(new CompareBikeData() { Value = Bikewale.Utility.FormatMinSpecs.ShowAvailable(compareEntity.Specifications.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version)).FuelEfficiencyOverall.Value), Text = Bikewale.Utility.FormatMinSpecs.ShowAvailable(compareEntity.Specifications.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version)).FuelEfficiencyOverall.Value) });
                    }
                    fuelEfficiencyPerformance.SpecCategory.Add(FuelEfficiencyOverall);

                    CompareSubCategory FuelEfficiencyRange = new CompareSubCategory();
                    FuelEfficiencyRange.Text = "Fuel Efficiency Range (Km)";
                    FuelEfficiencyRange.Value = "Fuel Efficiency Range (Km)";
                    FuelEfficiencyRange.CompareSpec = new List<CompareBikeData>();
                    foreach (var version in arrVersion)
                    {
                        FuelEfficiencyRange.CompareSpec.Add(new CompareBikeData() { Value = Bikewale.Utility.FormatMinSpecs.ShowAvailable(compareEntity.Specifications.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version)).FuelEfficiencyRange.Value), Text = Bikewale.Utility.FormatMinSpecs.ShowAvailable(compareEntity.Specifications.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version)).FuelEfficiencyRange.Value) });
                    }
                    fuelEfficiencyPerformance.SpecCategory.Add(FuelEfficiencyRange);

                    CompareSubCategory Performance_0_60_kmph = new CompareSubCategory();
                    Performance_0_60_kmph.Text = "0 to 60 kmph (Seconds)";
                    Performance_0_60_kmph.Value = "0 to 60 kmph (Seconds)";
                    Performance_0_60_kmph.CompareSpec = new List<CompareBikeData>();
                    foreach (var version in arrVersion)
                    {
                        Performance_0_60_kmph.CompareSpec.Add(new CompareBikeData() { Value = Bikewale.Utility.FormatMinSpecs.ShowAvailable(compareEntity.Specifications.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version)).Performance_0_60_kmph.Value), Text = Bikewale.Utility.FormatMinSpecs.ShowAvailable(compareEntity.Specifications.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version)).Performance_0_60_kmph.Value) });
                    }
                    fuelEfficiencyPerformance.SpecCategory.Add(Performance_0_60_kmph);


                    CompareSubCategory Performance_0_80_kmph = new CompareSubCategory();
                    Performance_0_80_kmph.Text = "0 to 80 kmph (Seconds)";
                    Performance_0_80_kmph.Value = "0 to 80 kmph (Seconds)";
                    Performance_0_80_kmph.CompareSpec = new List<CompareBikeData>();
                    foreach (var version in arrVersion)
                    {
                        Performance_0_80_kmph.CompareSpec.Add(new CompareBikeData() { Value = Bikewale.Utility.FormatMinSpecs.ShowAvailable(compareEntity.Specifications.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version)).Performance_0_80_kmph.Value), Text = Bikewale.Utility.FormatMinSpecs.ShowAvailable(compareEntity.Specifications.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version)).Performance_0_80_kmph.Value) });
                    }
                    fuelEfficiencyPerformance.SpecCategory.Add(Performance_0_80_kmph);

                    CompareSubCategory Performance_0_40_m = new CompareSubCategory();
                    Performance_0_40_m.Text = "0 to 40 m (Seconds)";
                    Performance_0_40_m.Value = "0 to 40 m (Seconds)";
                    Performance_0_40_m.CompareSpec = new List<CompareBikeData>();
                    foreach (var version in arrVersion)
                    {
                        Performance_0_40_m.CompareSpec.Add(new CompareBikeData() { Value = Bikewale.Utility.FormatMinSpecs.ShowAvailable(compareEntity.Specifications.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version)).Performance_0_40_m.Value), Text = Bikewale.Utility.FormatMinSpecs.ShowAvailable(compareEntity.Specifications.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version)).Performance_0_40_m.Value) });
                    }
                    fuelEfficiencyPerformance.SpecCategory.Add(Performance_0_40_m);

                    CompareSubCategory TopSpeed = new CompareSubCategory();
                    TopSpeed.Text = "Top Speed (Kmph)";
                    TopSpeed.Value = "Top Speed (Kmph)";
                    TopSpeed.CompareSpec = new List<CompareBikeData>();
                    foreach (var version in arrVersion)
                    {
                        TopSpeed.CompareSpec.Add(new CompareBikeData() { Value = Bikewale.Utility.FormatMinSpecs.ShowAvailable(compareEntity.Specifications.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version)).TopSpeed.Value), Text = Bikewale.Utility.FormatMinSpecs.ShowAvailable(compareEntity.Specifications.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version)).TopSpeed.Value) });
                    }
                    fuelEfficiencyPerformance.SpecCategory.Add(TopSpeed);

                    CompareSubCategory Performance_60_0_kmph = new CompareSubCategory();
                    Performance_60_0_kmph.Text = "60 to 0 Kmph (Seconds, metres)";
                    Performance_60_0_kmph.Value = "60 to 0 Kmph (Seconds, metres)";
                    Performance_60_0_kmph.CompareSpec = new List<CompareBikeData>();
                    foreach (var version in arrVersion)
                    {
                        Performance_60_0_kmph.CompareSpec.Add(new CompareBikeData() { Value = Bikewale.Utility.FormatMinSpecs.ShowAvailable(compareEntity.Specifications.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version)).Performance_60_0_kmph), Text = Bikewale.Utility.FormatMinSpecs.ShowAvailable(compareEntity.Specifications.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version)).Performance_60_0_kmph) });
                    }
                    fuelEfficiencyPerformance.SpecCategory.Add(Performance_60_0_kmph);

                    CompareSubCategory Performance_80_0_kmph = new CompareSubCategory();
                    Performance_80_0_kmph.Text = "80 to 0 kmph (Seconds, metres)";
                    Performance_80_0_kmph.Value = "80 to 0 kmph (Seconds, metres)";
                    Performance_80_0_kmph.CompareSpec = new List<CompareBikeData>();
                    foreach (var version in arrVersion)
                    {
                        Performance_80_0_kmph.CompareSpec.Add(new CompareBikeData() { Value = Bikewale.Utility.FormatMinSpecs.ShowAvailable(compareEntity.Specifications.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version)).Performance_80_0_kmph), Text = Bikewale.Utility.FormatMinSpecs.ShowAvailable(compareEntity.Specifications.FirstOrDefault(m => m.VersionId == Convert.ToUInt32(version)).Performance_80_0_kmph) });
                    }
                    fuelEfficiencyPerformance.SpecCategory.Add(Performance_80_0_kmph);
                    #endregion
                    compareEntity.CompareSpecifications.Spec.Add(fuelEfficiencyPerformance);

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
                        var objBikeColor = new List<BikeColor>();
                        foreach (var color in compareEntity.Color)
                        {
                            if (color.VersionId == Convert.ToUInt32(version))
                            {
                                objBikeColor.Add(color);
                            }
                        }
                        compareEntity.CompareColors.bikes.Add(new CompareBikeColor() { bikeColors = objBikeColor });
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, string.Format("Bikewale.BAL.Compare.BikeComparison.TransposeCompareBikeData - {0}", versions));
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
        /// </summary>
        /// <param name="versionList"></param>
        /// <param name="topCount"></param>
        /// <param name="cityid"></param>
        /// <returns></returns>
        public ICollection<SimilarCompareBikeEntity> GetSimilarCompareBikes(string versionList, ushort topCount, int cityid)
        {
            return _objCompare.GetSimilarCompareBikes(versionList, topCount, cityid);
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
            Entities.Compare.BikeCompareEntity compareEntity = null;
            try
            {
                if (!string.IsNullOrEmpty(versions))
                {
                    compareEntity = _objCompare.DoCompare(versions, cityId);
                    TransposeCompareBikeData(ref compareEntity, versions);
                }
                compareEntity.Features = null;
                compareEntity.Specifications = null;
                compareEntity.Color = null;
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, string.Format("Bikewale.BAL.Compare.BikeComparison.DoCompare - {0} - {1}", versions, cityId));
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
    }
}
