using Bikewale.Entities.BikeData;
using Bikewale.Entities.Compare;
using Bikewale.Interfaces.Compare;
using Bikewale.Notifications;
using Bikewale.Utility;
using MySql.CoreDAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Bikewale.DAL.Compare
{
    /// <summary>
    /// Modified By : Sushil Kumar on 2nd Dec 2016
    /// Description : Added methods for featured bike and sponsored bike comparisions
    /// </summary>
    public class BikeCompareRepository : IBikeCompare
    {
        public BikeCompareEntity DoCompare(string versions)
        {
            BikeCompareEntity compare = null;
            IList<BikeEntityBase> basicInfos = null;
            IList<BikeSpecification> specs = null;
            IList<BikeFeature> features = null;
            List<BikeColor> color = null;
            IList<Bikewale.Entities.Compare.BikeModelColor> hexCodes = null;


            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    cmd.CommandText = "getcomparisondetails_20012016";
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_bikeversions", DbType.String, versions));

                    using (IDataReader reader = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (reader != null)
                        {
                            #region Basic Info
                            basicInfos = new List<BikeEntityBase>();
                            while (reader.Read())
                            {
                                basicInfos.Add(new BikeEntityBase()
                                {
                                    HostUrl = Convert.ToString(reader["HostURL"]),
                                    ImagePath = Convert.ToString(reader["OriginalImagePath"]),
                                    Make = Convert.ToString(reader["Make"]),
                                    MakeMaskingName = Convert.ToString(reader["MakeMaskingName"]),
                                    Model = Convert.ToString(reader["Model"]),
                                    ModelMaskingName = Convert.ToString(reader["ModelMaskingName"]),
                                    ModelRating = SqlReaderConvertor.ToUInt16(reader["ModelRating"]),
                                    Name = Convert.ToString(reader["Bike"]),
                                    Price = SqlReaderConvertor.ToInt32(reader["Price"]),
                                    Version = Convert.ToString(reader["Version"]),
                                    VersionId = SqlReaderConvertor.ToUInt32(reader["BikeVersionId"]),
                                    VersionRating = SqlReaderConvertor.ToUInt16(reader["VersionRating"])
                                });
                            }
                            #endregion
                            #region Bike Specification
                            if (reader.NextResult())
                            {
                                specs = new List<BikeSpecification>();
                                while (reader.Read())
                                {
                                    specs.Add(new BikeSpecification()
                                    {
                                        AlloyWheels = SqlReaderConvertor.ToNullableBool(reader["AlloyWheels"]),
                                        Battery = Convert.ToString(reader["Battery"]),
                                        Bore = SqlReaderConvertor.ToNullableFloat(reader["Bore"]),
                                        Brake_Tail_Light = Convert.ToString(reader["Brake_Tail_Light"]),
                                        BrakeType = Convert.ToString(reader["BrakeType"]),
                                        CalliperType = Convert.ToString(reader["CalliperType"]),
                                        ChassisType = Convert.ToString(reader["ChassisType"]),
                                        Clutch = Convert.ToString(reader["Clutch"]),
                                        CoolingSystem = Convert.ToString(reader["CoolingSystem"]),
                                        Cylinders = SqlReaderConvertor.ToNullableUInt16(reader["Cylinders"]),
                                        Displacement = SqlReaderConvertor.ToNullableFloat(reader["Displacement"]),
                                        ElectricSystem = Convert.ToString(reader["ElectricSystem"]),
                                        FrontDisc = SqlReaderConvertor.ToNullableBool(reader["FrontDisc"]),
                                        FrontDisc_DrumSize = SqlReaderConvertor.ToNullableUInt16(reader["FrontDisc_DrumSize"]),
                                        FrontSuspension = Convert.ToString(reader["FrontSuspension"]),
                                        FrontTyre = Convert.ToString(reader["FrontTyre"]),
                                        FuelDeliverySystem = Convert.ToString(reader["FuelDeliverySystem"]),
                                        FuelEfficiencyOverall = SqlReaderConvertor.ToNullableUInt16(reader["FuelEfficiencyOverall"]),
                                        FuelEfficiencyRange = SqlReaderConvertor.ToNullableUInt16(reader["FuelEfficiencyRange"]),
                                        FuelTankCapacity = SqlReaderConvertor.ToNullableFloat(reader["FuelTankCapacity"]),
                                        FuelType = Convert.ToString(reader["FuelType"]),
                                        GearboxType = Convert.ToString(reader["GearboxType"]),
                                        GroundClearance = SqlReaderConvertor.ToNullableUInt16(reader["GroundClearance"]),
                                        HeadlightBulbType = Convert.ToString(reader["HeadlightBulbType"]),
                                        HeadlightType = Convert.ToString(reader["HeadlightType"]),
                                        Ignition = Convert.ToString(reader["Ignition"]),
                                        KerbWeight = SqlReaderConvertor.ToNullableUInt16(reader["KerbWeight"]),
                                        MaximumTorque = SqlReaderConvertor.ToNullableFloat(reader["MaximumTorque"]),
                                        MaximumTorqueRpm = SqlReaderConvertor.ToNullableUInt32(reader["MaximumTorqueRpm"]),
                                        MaxPower = SqlReaderConvertor.ToNullableFloat(reader["MaxPower"]),
                                        MaxPowerRpm = SqlReaderConvertor.ToNullableUInt32(reader["MaxPowerRpm"]),
                                        NoOfGears = SqlReaderConvertor.ToNullableUInt16(reader["NoOfGears"]),
                                        OverallHeight = SqlReaderConvertor.ToNullableUInt16(reader["OverallHeight"]),
                                        OverallLength = SqlReaderConvertor.ToNullableUInt16(reader["OverallLength"]),
                                        OverallWidth = SqlReaderConvertor.ToNullableUInt16(reader["OverallWidth"]),
                                        PassLight = SqlReaderConvertor.ToNullableBool(reader["PassLight"]),
                                        Performance_0_40_m = SqlReaderConvertor.ToNullableFloat(reader["Performance_0_40_m"]),
                                        Performance_0_60_kmph = SqlReaderConvertor.ToNullableFloat(reader["Performance_0_60_kmph"]),
                                        Performance_0_80_kmph = SqlReaderConvertor.ToNullableFloat(reader["Performance_0_80_kmph"]),
                                        Performance_60_0_kmph = Convert.ToString(reader["Performance_60_0_kmph"]),
                                        Performance_80_0_kmph = Convert.ToString(reader["Performance_80_0_kmph"]),
                                        RadialTyres = SqlReaderConvertor.ToNullableBool(reader["RadialTyres"]),
                                        RearDisc = SqlReaderConvertor.ToNullableBool(reader["RearDisc"]),
                                        RearDisc_DrumSize = SqlReaderConvertor.ToNullableUInt16(reader["RearDisc_DrumSize"]),
                                        RearSuspension = Convert.ToString(reader["RearSuspension"]),
                                        RearTyre = Convert.ToString(reader["RearTyre"]),
                                        ReserveFuelCapacity = SqlReaderConvertor.ToNullableFloat(reader["ReserveFuelCapacity"]),
                                        SeatHeight = SqlReaderConvertor.ToNullableUInt16(reader["SeatHeight"]),
                                        SparkPlugsPerCylinder = Convert.ToString(reader["SparkPlugsPerCylinder"]),
                                        Stroke = SqlReaderConvertor.ToNullableFloat(reader["Stroke"]),
                                        TopSpeed = SqlReaderConvertor.ToNullableFloat(reader["TopSpeed"]),
                                        TransmissionType = Convert.ToString(reader["TransmissionType"]),
                                        TubelessTyres = SqlReaderConvertor.ToNullableBool(reader["TubelessTyres"]),
                                        TurnSignal = Convert.ToString(reader["TurnSignal"]),
                                        ValvesPerCylinder = SqlReaderConvertor.ToNullableUInt16(reader["ValvesPerCylinder"]),
                                        VersionId = SqlReaderConvertor.ToUInt32(reader["BikeVersionId"]),
                                        Wheelbase = SqlReaderConvertor.ToNullableUInt16(reader["Wheelbase"]),
                                        WheelSize = SqlReaderConvertor.ToNullableFloat(reader["WheelSize"])
                                    });
                                }
                            }
                            #endregion
                            #region Bike Features
                            if (reader.NextResult())
                            {
                                features = new List<BikeFeature>();
                                while (reader.Read())
                                {
                                    features.Add(new BikeFeature()
                                    {
                                        AntilockBrakingSystem = SqlReaderConvertor.ToNullableBool(reader["AntilockBrakingSystem"]),
                                        Clock = SqlReaderConvertor.ToNullableBool(reader["Clock"]),
                                        DigitalFuelGauge = SqlReaderConvertor.ToNullableBool(reader["DigitalFuelGauge"]),
                                        ElectricStart = SqlReaderConvertor.ToNullableBool(reader["ElectricStart"]),
                                        FuelGauge = SqlReaderConvertor.ToNullableBool(reader["FuelGauge"]),
                                        Killswitch = SqlReaderConvertor.ToNullableBool(reader["Killswitch"]),
                                        LowBatteryIndicator = SqlReaderConvertor.ToNullableBool(reader["LowBatteryIndicator"]),
                                        LowFuelIndicator = SqlReaderConvertor.ToNullableBool(reader["LowFuelIndicator"]),
                                        LowOilIndicator = SqlReaderConvertor.ToNullableBool(reader["LowOilIndicator"]),
                                        NoOfTripmeters = Convert.ToString(reader["NoOfTripmeters"]),
                                        PillionBackrest = SqlReaderConvertor.ToNullableBool(reader["PillionBackrest"]),
                                        PillionFootrest = SqlReaderConvertor.ToNullableBool(reader["PillionFootrest"]),
                                        PillionGrabrail = SqlReaderConvertor.ToNullableBool(reader["PillionGrabrail"]),
                                        PillionSeat = SqlReaderConvertor.ToNullableBool(reader["PillionSeat"]),
                                        ShiftLight = SqlReaderConvertor.ToNullableBool(reader["ShiftLight"]),
                                        Speedometer = Convert.ToString(reader["Speedometer"]),
                                        StandAlarm = SqlReaderConvertor.ToNullableBool(reader["StandAlarm"]),
                                        SteppedSeat = SqlReaderConvertor.ToNullableBool(reader["SteppedSeat"]),
                                        Tachometer = SqlReaderConvertor.ToNullableBool(reader["Tachometer"]),
                                        TachometerType = Convert.ToString(reader["TachometerType"]),
                                        Tripmeter = SqlReaderConvertor.ToNullableBool(reader["Tripmeter"]),
                                        TripmeterType = Convert.ToString(reader["TripmeterType"]),
                                        VersionId = SqlReaderConvertor.ToUInt32(reader["BikeVersionId"])
                                    });
                                }
                            }
                            #endregion
                            #region Bike Colors
                            if (reader.NextResult())
                            {
                                color = new List<BikeColor>();
                                while (reader.Read())
                                {
                                    color.Add(new BikeColor()
                                    {
                                        ColorId = SqlReaderConvertor.ToInt32(reader["ModelColorId"]),
                                        Color = Convert.ToString(reader["ColorName"]),
                                        VersionId = SqlReaderConvertor.ToUInt32(reader["VersionId"])
                                    });
                                }
                            }
                            #endregion
                            #region Color Hex Codes
                            if (reader.NextResult())
                            {
                                hexCodes = new List<Bikewale.Entities.Compare.BikeModelColor>();
                                while (reader.Read())
                                {
                                    hexCodes.Add(new Bikewale.Entities.Compare.BikeModelColor()
                                    {
                                        ModelColorId = SqlReaderConvertor.ToInt32(reader["ModelColorId"]),
                                        HexCode = Convert.ToString(reader["HexCode"])
                                    });
                                }
                            }
                            #endregion
                            compare = new BikeCompareEntity();
                            compare.BasicInfo = basicInfos;
                            compare.Specifications = specs;
                            compare.Features = features;
                            compare.Color = color;
                            reader.Close();
                        }
                    }

                    if (hexCodes != null && hexCodes.Count > 0 && compare.Color != null && compare.Color.Count > 0)
                    {
                        compare.Color.ForEach(
                                            _color => _color.HexCodes =
                                                (from hexCode in hexCodes
                                                 where hexCode.ModelColorId == _color.ColorId
                                                 select hexCode.HexCode).ToList()
                                            );
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, string.Format("Bikewale.DAL.Compare.BikeCompareRepository.DoCompare : {0}", versions));
            }

            return compare;
        }


        /// <summary>
        /// Created By : Sadhana Upadhyay on 24 Sept 2014
        /// Summary : To get compare bike details by version id list
        /// Modified By : Lucky Rathore
        /// Modified On : 15 Feb 2016
        /// Summary : SP name Changed.
        /// Modified By : Lucky Rathore
        /// Modified On : 26 Feb 2016
        /// Summary : SP name Changed.
        /// Modified On : 23 Jan 2017 by Sangram Nandkhile
        /// Summary : New parameter added - cityId
        /// </summary>
        /// <param name="versions"></param>
        /// <returns></returns>
        public BikeCompareEntity DoCompare(string versions, uint cityId)
        {

            BikeCompareEntity compare = null;
            IList<BikeEntityBase> basicInfos = null;
            IList<BikeSpecification> specs = null;
            IList<BikeFeature> features = null;
            List<BikeColor> color = null;
            IList<Bikewale.Entities.Compare.BikeModelColor> hexCodes = null;
            // KeyValuePair<uint, BikeVersionEntityBase> modelVersio = new KeyValuePair<uint, BikeVersionEntityBase>();

            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("getcomparisondetails_02022017"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_bikeversions", DbType.String, versions));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_cityid", DbType.UInt16, cityId));

                    using (IDataReader reader = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (reader != null)
                        {
                            IList<BikeVersionCompareEntity> versionsList = new List<BikeVersionCompareEntity>();
                            while (reader.Read())
                            {
                                versionsList.Add(new BikeVersionCompareEntity()
                                {
                                    ModelId = SqlReaderConvertor.ToUInt32(reader["BikeModelId"]),
                                    VersionId = SqlReaderConvertor.ToInt32(reader["VersionId"]),
                                    VersionName = Convert.ToString(reader["VersionName"])
                                });
                            }

                            if (reader.NextResult())
                            {
                                basicInfos = new List<BikeEntityBase>();
                                specs = new List<BikeSpecification>();
                                features = new List<BikeFeature>();
                                while (reader.Read())
                                {
                                    #region Basic Info

                                    uint modelId = SqlReaderConvertor.ToUInt32(reader["ModelId"]);

                                    basicInfos.Add(new BikeEntityBase()
                                    {
                                        HostUrl = Convert.ToString(reader["HostURL"]),
                                        ImagePath = Convert.ToString(reader["OriginalImagePath"]),
                                        Make = Convert.ToString(reader["Make"]),
                                        MakeMaskingName = Convert.ToString(reader["MakeMaskingName"]),
                                        Model = Convert.ToString(reader["Model"]),
                                        ModelMaskingName = Convert.ToString(reader["ModelMaskingName"]),
                                        ModelRating = SqlReaderConvertor.ToUInt16(reader["ModelRating"]),
                                        Name = Convert.ToString(reader["Bike"]),
                                        Price = SqlReaderConvertor.ToInt32(reader["Price"]),
                                        Version = Convert.ToString(reader["Version"]),
                                        VersionId = SqlReaderConvertor.ToUInt32(reader["BikeVersionId"]),
                                        ModelId = SqlReaderConvertor.ToUInt32(reader["modelId"]),
                                        VersionRating = SqlReaderConvertor.ToUInt16(reader["VersionRating"]),
                                        ExpectedLaunch = SqlReaderConvertor.ToDateTime(reader["ExpectedLaunch"]),
                                        EstimatedPriceMin = SqlReaderConvertor.ToUInt32(reader["EstimatedPriceMin"]),
                                        EstimatedPriceMax = SqlReaderConvertor.ToUInt32(reader["EstimatedPriceMax"]),
                                        UsedBikeCount = new Entities.Used.UsedBikesCountInCity()
                                        {
                                            BikeCount = SqlReaderConvertor.ToUInt32(reader["bikeCount"]),
                                            StartingPrice = SqlReaderConvertor.ToUInt32(reader["minPrice"]),
                                            CityMaskingName = Convert.ToString(reader["citymaskingname"])
                                        },
                                        Versions = versionsList.Where(x => x.ModelId == modelId)
                                    });


                                    #endregion

                                    #region Bike Specification
                                    specs.Add(new BikeSpecification()
                                        {
                                            AlloyWheels = SqlReaderConvertor.ToNullableBool(reader["AlloyWheels"]),
                                            Battery = Convert.ToString(reader["Battery"]),
                                            Bore = SqlReaderConvertor.ToNullableFloat(reader["Bore"]),
                                            Brake_Tail_Light = Convert.ToString(reader["Brake_Tail_Light"]),
                                            BrakeType = Convert.ToString(reader["BrakeType"]),
                                            CalliperType = Convert.ToString(reader["CalliperType"]),
                                            ChassisType = Convert.ToString(reader["ChassisType"]),
                                            Clutch = Convert.ToString(reader["Clutch"]),
                                            CoolingSystem = Convert.ToString(reader["CoolingSystem"]),
                                            Cylinders = SqlReaderConvertor.ToNullableUInt16(reader["Cylinders"]),
                                            Displacement = SqlReaderConvertor.ToNullableFloat(reader["Displacement"]),
                                            ElectricSystem = Convert.ToString(reader["ElectricSystem"]),
                                            FrontDisc = SqlReaderConvertor.ToNullableBool(reader["FrontDisc"]),
                                            FrontDisc_DrumSize = SqlReaderConvertor.ToNullableUInt16(reader["FrontDisc_DrumSize"]),
                                            FrontSuspension = Convert.ToString(reader["FrontSuspension"]),
                                            FrontTyre = Convert.ToString(reader["FrontTyre"]),
                                            FuelDeliverySystem = Convert.ToString(reader["FuelDeliverySystem"]),
                                            FuelEfficiencyOverall = SqlReaderConvertor.ToNullableUInt16(reader["FuelEfficiencyOverall"]),
                                            FuelEfficiencyRange = SqlReaderConvertor.ToNullableUInt16(reader["FuelEfficiencyRange"]),
                                            FuelTankCapacity = SqlReaderConvertor.ToNullableFloat(reader["FuelTankCapacity"]),
                                            FuelType = Convert.ToString(reader["FuelType"]),
                                            GearboxType = Convert.ToString(reader["GearboxType"]),
                                            GroundClearance = SqlReaderConvertor.ToNullableUInt16(reader["GroundClearance"]),
                                            HeadlightBulbType = Convert.ToString(reader["HeadlightBulbType"]),
                                            HeadlightType = Convert.ToString(reader["HeadlightType"]),
                                            Ignition = Convert.ToString(reader["Ignition"]),
                                            KerbWeight = SqlReaderConvertor.ToNullableUInt16(reader["KerbWeight"]),
                                            MaximumTorque = SqlReaderConvertor.ToNullableFloat(reader["MaximumTorque"]),
                                            MaximumTorqueRpm = SqlReaderConvertor.ToNullableUInt32(reader["MaximumTorqueRpm"]),
                                            MaxPower = SqlReaderConvertor.ToNullableFloat(reader["MaxPower"]),
                                            MaxPowerRpm = SqlReaderConvertor.ToNullableUInt32(reader["MaxPowerRpm"]),
                                            NoOfGears = SqlReaderConvertor.ToNullableUInt16(reader["NoOfGears"]),
                                            OverallHeight = SqlReaderConvertor.ToNullableUInt16(reader["OverallHeight"]),
                                            OverallLength = SqlReaderConvertor.ToNullableUInt16(reader["OverallLength"]),
                                            OverallWidth = SqlReaderConvertor.ToNullableUInt16(reader["OverallWidth"]),
                                            PassLight = SqlReaderConvertor.ToNullableBool(reader["PassLight"]),
                                            Performance_0_40_m = SqlReaderConvertor.ToNullableFloat(reader["Performance_0_40_m"]),
                                            Performance_0_60_kmph = SqlReaderConvertor.ToNullableFloat(reader["Performance_0_60_kmph"]),
                                            Performance_0_80_kmph = SqlReaderConvertor.ToNullableFloat(reader["Performance_0_80_kmph"]),
                                            Performance_60_0_kmph = Convert.ToString(reader["Performance_60_0_kmph"]),
                                            Performance_80_0_kmph = Convert.ToString(reader["Performance_80_0_kmph"]),
                                            RadialTyres = SqlReaderConvertor.ToNullableBool(reader["RadialTyres"]),
                                            RearDisc = SqlReaderConvertor.ToNullableBool(reader["RearDisc"]),
                                            RearDisc_DrumSize = SqlReaderConvertor.ToNullableUInt16(reader["RearDisc_DrumSize"]),
                                            RearSuspension = Convert.ToString(reader["RearSuspension"]),
                                            RearTyre = Convert.ToString(reader["RearTyre"]),
                                            ReserveFuelCapacity = SqlReaderConvertor.ToNullableFloat(reader["ReserveFuelCapacity"]),
                                            SeatHeight = SqlReaderConvertor.ToNullableUInt16(reader["SeatHeight"]),
                                            SparkPlugsPerCylinder = Convert.ToString(reader["SparkPlugsPerCylinder"]),
                                            Stroke = SqlReaderConvertor.ToNullableFloat(reader["Stroke"]),
                                            TopSpeed = SqlReaderConvertor.ToNullableFloat(reader["TopSpeed"]),
                                            TransmissionType = Convert.ToString(reader["TransmissionType"]),
                                            TubelessTyres = SqlReaderConvertor.ToNullableBool(reader["TubelessTyres"]),
                                            TurnSignal = Convert.ToString(reader["TurnSignal"]),
                                            ValvesPerCylinder = SqlReaderConvertor.ToNullableUInt16(reader["ValvesPerCylinder"]),
                                            VersionId = SqlReaderConvertor.ToUInt32(reader["BikeVersionId"]),
                                            Wheelbase = SqlReaderConvertor.ToNullableUInt16(reader["Wheelbase"]),
                                            WheelSize = SqlReaderConvertor.ToNullableFloat(reader["WheelSize"])
                                        });

                                    #endregion

                                    #region Bike Features
                                    features.Add(new BikeFeature()
                                            {
                                                AntilockBrakingSystem = SqlReaderConvertor.ToNullableBool(reader["AntilockBrakingSystem"]),
                                                Clock = SqlReaderConvertor.ToNullableBool(reader["Clock"]),
                                                DigitalFuelGauge = SqlReaderConvertor.ToNullableBool(reader["DigitalFuelGauge"]),
                                                ElectricStart = SqlReaderConvertor.ToNullableBool(reader["ElectricStart"]),
                                                FuelGauge = SqlReaderConvertor.ToNullableBool(reader["FuelGauge"]),
                                                Killswitch = SqlReaderConvertor.ToNullableBool(reader["Killswitch"]),
                                                LowBatteryIndicator = SqlReaderConvertor.ToNullableBool(reader["LowBatteryIndicator"]),
                                                LowFuelIndicator = SqlReaderConvertor.ToNullableBool(reader["LowFuelIndicator"]),
                                                LowOilIndicator = SqlReaderConvertor.ToNullableBool(reader["LowOilIndicator"]),
                                                NoOfTripmeters = Convert.ToString(reader["NoOfTripmeters"]),
                                                PillionBackrest = SqlReaderConvertor.ToNullableBool(reader["PillionBackrest"]),
                                                PillionFootrest = SqlReaderConvertor.ToNullableBool(reader["PillionFootrest"]),
                                                PillionGrabrail = SqlReaderConvertor.ToNullableBool(reader["PillionGrabrail"]),
                                                PillionSeat = SqlReaderConvertor.ToNullableBool(reader["PillionSeat"]),
                                                ShiftLight = SqlReaderConvertor.ToNullableBool(reader["ShiftLight"]),
                                                Speedometer = Convert.ToString(reader["Speedometer"]),
                                                StandAlarm = SqlReaderConvertor.ToNullableBool(reader["StandAlarm"]),
                                                SteppedSeat = SqlReaderConvertor.ToNullableBool(reader["SteppedSeat"]),
                                                Tachometer = SqlReaderConvertor.ToNullableBool(reader["Tachometer"]),
                                                TachometerType = Convert.ToString(reader["TachometerType"]),
                                                Tripmeter = SqlReaderConvertor.ToNullableBool(reader["Tripmeter"]),
                                                TripmeterType = Convert.ToString(reader["TripmeterType"]),
                                                VersionId = SqlReaderConvertor.ToUInt32(reader["BikeVersionId"])
                                            });
                                    #endregion
                                }
                            }

                            if (reader.NextResult())
                            {
                                color = new List<BikeColor>();
                                hexCodes = new List<Bikewale.Entities.Compare.BikeModelColor>();
                                while (reader.Read())
                                {
                                    #region Bike Colors
                                    color.Add(new BikeColor()
                                    {
                                        ColorId = SqlReaderConvertor.ToInt32(reader["ModelColorId"]),
                                        Color = Convert.ToString(reader["ColorName"]),
                                        VersionId = SqlReaderConvertor.ToUInt32(reader["VersionId"])
                                    });
                                    #endregion
                                    #region Color Hex Codes

                                    hexCodes.Add(new Bikewale.Entities.Compare.BikeModelColor()
                                   {
                                       ModelColorId = SqlReaderConvertor.ToInt32(reader["ModelColorId"]),
                                       HexCode = Convert.ToString(reader["HexCode"])
                                   });
                                    #endregion
                                }
                            }

                            compare = new BikeCompareEntity();
                            compare.BasicInfo = basicInfos;
                            compare.Specifications = specs;
                            compare.Features = features;
                            compare.Color = color.GroupBy(x => x.ColorId).Select(y => y.First()).ToList();
                            reader.Close();
                        }
                    }

                    if (hexCodes != null && hexCodes.Count > 0 && compare.Color != null && compare.Color.Count > 0)
                    {
                        compare.Color.ForEach(
                                            _color => _color.HexCodes =
                                                (from hexCode in hexCodes
                                                 where hexCode.ModelColorId == _color.ColorId
                                                 select hexCode.HexCode).ToList()
                                            );
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Bikewale.DAL.Compare.DoCompare : " + versions);
            }
            return compare;
        }

        /// <summary>
        /// Modified by :   Sumit Kate on 29 Jan 2016
        /// Description :   Populate Versions image related entity properties.
        /// Modified By : Sushil Kumar on 27th Oct 2016
        /// Description : Removed unused properties binding image
        /// </summary>
        /// <param name="topCount"></param>
        /// <returns></returns>
        public IEnumerable<TopBikeCompareBase> CompareList(uint topCount)
        {
            List<TopBikeCompareBase> topBikeList = null;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("getbikecomparisonmin_27102016"))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_topcount", DbType.Int16, topCount));
                    // LogLiveSps.LogSpInGrayLog(cmd);
                    using (IDataReader reader = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (reader != null)
                        {
                            topBikeList = new List<TopBikeCompareBase>();
                            while (reader.Read())
                            {
                                topBikeList.Add(new TopBikeCompareBase()
                                {
                                    Bike1 = Convert.ToString(reader["Bike1"]),
                                    Bike2 = Convert.ToString(reader["Bike2"]),
                                    ID = SqlReaderConvertor.ToInt32(reader["ID"]),
                                    MakeMaskingName2 = Convert.ToString(reader["MakeMakingName2"]),
                                    MakeMaskingName1 = Convert.ToString(reader["MakeMaskingName1"]),
                                    ModelId1 = SqlReaderConvertor.ToUInt16(reader["ModelId1"]),
                                    ModelId2 = SqlReaderConvertor.ToUInt16(reader["ModelId2"]),
                                    ModelMaskingName1 = Convert.ToString(reader["ModelMaskingName1"]),
                                    ModelMaskingName2 = Convert.ToString(reader["ModelMaskingName2"]),

                                    Price1 = SqlReaderConvertor.ToUInt32(reader["Price1"]),
                                    Price2 = SqlReaderConvertor.ToUInt32(reader["Price2"]),

                                    VersionId1 = SqlReaderConvertor.ToUInt16(reader["VersionId1"]),
                                    VersionId2 = SqlReaderConvertor.ToUInt16(reader["VersionId2"]),
                                    VersionImgUrl1 = Convert.ToString(reader["VersionImgUrl1"]),
                                    VersionImgUrl2 = Convert.ToString(reader["VersionImgUrl2"]),
                                    HostUrl1 = Convert.ToString(reader["HostUrl1"]),
                                    HostUrl2 = Convert.ToString(reader["HostUrl2"])
                                });
                            }

                            reader.Close();
                        }
                    }
                }
            }
            catch (SqlException sqEx)
            {
                ErrorClass objErr = new ErrorClass(sqEx, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }

            return topBikeList;
        }

        /// <summary>
        /// Created by: Sangram Nandkhile on 11 May 2016
        /// Modified by :Subodh Jain on 21 oct 2016
        /// Desc : Added cityid as parameter
        /// Modified By : Sushil Kumar on 2nd Dec 2016
        /// Description : DAL layer to similar comaprisions bikes changed topcount to ushort  
        /// <param name="versionList"></param>
        /// <param name="topCount"></param>
        /// <returns></returns>
        public ICollection<SimilarCompareBikeEntity> GetSimilarCompareBikes(string versionList, ushort topCount, int cityid)
        {
            List<SimilarCompareBikeEntity> similarBikeList = null;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    cmd.CommandText = "getsimilarcomparebikeslist_13102016";
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_bikeversionidlist", DbType.String, 20, versionList));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_topcount", DbType.Int16, topCount));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_cityid", DbType.Int32, cityid));
                    // LogLiveSps.LogSpInGrayLog(command);
                    using (IDataReader reader = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (reader != null)
                        {
                            similarBikeList = new List<SimilarCompareBikeEntity>();
                            while (reader.Read())
                            {
                                similarBikeList.Add(new SimilarCompareBikeEntity()
                                {
                                    Make1 = Convert.ToString(reader["Make1"]),
                                    MakeMasking1 = Convert.ToString(reader["MakeMaskingName1"]),
                                    Make2 = Convert.ToString(reader["Make2"]),
                                    MakeMasking2 = Convert.ToString(reader["MakeMaskingName2"]),
                                    Model1 = Convert.ToString(reader["Model1"]),
                                    ModelMasking1 = Convert.ToString(reader["ModelMaskingName1"]),
                                    Model2 = Convert.ToString(reader["Model2"]),
                                    ModelMasking2 = Convert.ToString(reader["ModelMaskingName2"]),
                                    VersionId1 = Convert.ToString(reader["VersionId1"]),
                                    VersionId2 = Convert.ToString(reader["VersionId2"]),
                                    ModelMaskingName1 = Convert.ToString(reader["ModelMaskingName1"]),
                                    ModelMaskingName2 = Convert.ToString(reader["ModelMaskingName2"]),
                                    OriginalImagePath1 = Convert.ToString(reader["OriginalImagePath1"]),
                                    OriginalImagePath2 = Convert.ToString(reader["OriginalImagePath2"]),
                                    Price1 = SqlReaderConvertor.ToInt32(reader["Price1"]),
                                    Price2 = SqlReaderConvertor.ToInt32(reader["Price2"]),
                                    HostUrl1 = Convert.ToString(reader["HostUrl1"]),
                                    HostUrl2 = Convert.ToString(reader["HostUrl2"]),
                                    City1 = Convert.ToString(reader["city1"]),
                                    City2 = Convert.ToString(reader["city2"])
                                });
                            }
                            reader.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, string.Format("BikeCompareRepository_GetSimilarCompareBikes_{0}_Cnt_{1}_City_{2}", versionList, topCount, cityid));
                objErr.SendMail();
            }

            return similarBikeList;
        }


        /// <summary>
        /// Created By : Sushil Kumar on 2nd Dec 2016
        /// Description : DAL layer to similar comaprisions bikes with sponsored comparision 
        /// </summary>
        /// <param name="versionList"></param>
        /// <param name="topCount"></param>
        /// <param name="cityid"></param>
        /// <param name="sponsoredVersionId"></param>
        /// <returns></returns>
        public ICollection<SimilarCompareBikeEntity> GetSimilarCompareBikeSponsored(string versionList, ushort topCount, int cityid, uint sponsoredVersionId)
        {
            ICollection<SimilarCompareBikeEntity> similarBikeList = null;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    cmd.CommandText = "getsimilarcomparebikeslist_sponsored";
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_bikeversionidlist", DbType.String, 20, versionList));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_topcount", DbType.Int16, topCount));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_cityid", DbType.Int32, cityid));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_sponsoredversionid", DbType.UInt32, sponsoredVersionId));
                    // LogLiveSps.LogSpInGrayLog(command);
                    using (IDataReader reader = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (reader != null)
                        {
                            similarBikeList = new List<SimilarCompareBikeEntity>();
                            while (reader.Read())
                            {
                                similarBikeList.Add(new SimilarCompareBikeEntity()
                                {
                                    Make1 = Convert.ToString(reader["Make1"]),
                                    MakeMasking1 = Convert.ToString(reader["MakeMaskingName1"]),
                                    Make2 = Convert.ToString(reader["Make2"]),
                                    MakeMasking2 = Convert.ToString(reader["MakeMaskingName2"]),
                                    Model1 = Convert.ToString(reader["Model1"]),
                                    ModelMasking1 = Convert.ToString(reader["ModelMaskingName1"]),
                                    Model2 = Convert.ToString(reader["Model2"]),
                                    ModelMasking2 = Convert.ToString(reader["ModelMaskingName2"]),
                                    VersionId1 = Convert.ToString(reader["VersionId1"]),
                                    VersionId2 = Convert.ToString(reader["VersionId2"]),
                                    ModelMaskingName1 = Convert.ToString(reader["ModelMaskingName1"]),
                                    ModelMaskingName2 = Convert.ToString(reader["ModelMaskingName2"]),
                                    OriginalImagePath1 = Convert.ToString(reader["OriginalImagePath1"]),
                                    OriginalImagePath2 = Convert.ToString(reader["OriginalImagePath2"]),
                                    Price1 = SqlReaderConvertor.ToInt32(reader["Price1"]),
                                    Price2 = SqlReaderConvertor.ToInt32(reader["Price2"]),
                                    HostUrl1 = Convert.ToString(reader["HostUrl1"]),
                                    HostUrl2 = Convert.ToString(reader["HostUrl2"]),
                                    City1 = Convert.ToString(reader["city1"]),
                                    City2 = Convert.ToString(reader["city2"]),
                                    ModelId1 = SqlReaderConvertor.ToUInt32(reader["ModelId1"]),
                                    ModelId2 = SqlReaderConvertor.ToUInt32(reader["ModelId2"])
                                });
                            }
                            reader.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, string.Format("BikeCompareCacheRepository_GetSimilarCompareBikeSponsored_{0}_Cnt_{1}_SP_{2}_City_{3}", versionList, topCount, sponsoredVersionId, cityid));
                objErr.SendMail();
            }

            return similarBikeList;
        }


        public Int64 GetFeaturedBike(string versions)
        {
            throw new NotImplementedException();
        }
    }
}
