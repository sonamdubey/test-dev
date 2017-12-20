using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Web;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.Compare;
using Bikewale.Entities.UserReviews;
using Bikewale.Interfaces.Compare;
using Bikewale.Notifications;
using Bikewale.Utility;
using MySql.CoreDAL;

namespace Bikewale.DAL.Compare
{
    /// <summary>
    /// Modified By : Sushil Kumar on 2nd Dec 2016
    /// Description : Added methods for featured bike and sponsored bike comparisions
    /// Modified By :   Sushil Kumar on 2nd Feb 2017
    /// Description :   Implemented the newly added method of IBikeCompare : BikeCompareEntity DoCompare(string versions, uint cityId)
    /// </summary>
    public class BikeCompareRepository : IBikeCompare
    {
        /// Modified by : Aditi Srivastava on 18 May 2017
        /// Summary      : Used nullable bool for specs and features
        public Entities.Compare.BikeCompareEntity DoCompare(string versions)
        {
            Entities.Compare.BikeCompareEntity compare = null;
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
                                        AlloyWheels = reader["AlloyWheels"] != DBNull.Value ? SqlReaderConvertor.ToNullableBool(reader["AlloyWheels"]) : null,
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
                                        FrontDisc = reader["FrontDisc"] != DBNull.Value ? SqlReaderConvertor.ToNullableBool(reader["FrontDisc"]) : null,
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
                                        PassLight = reader["PassLight"] != DBNull.Value ? SqlReaderConvertor.ToNullableBool(reader["PassLight"]) : null,
                                        Performance_0_40_m = SqlReaderConvertor.ToNullableFloat(reader["Performance_0_40_m"]),
                                        Performance_0_60_kmph = SqlReaderConvertor.ToNullableFloat(reader["Performance_0_60_kmph"]),
                                        Performance_0_80_kmph = SqlReaderConvertor.ToNullableFloat(reader["Performance_0_80_kmph"]),
                                        Performance_60_0_kmph = Convert.ToString(reader["Performance_60_0_kmph"]),
                                        Performance_80_0_kmph = Convert.ToString(reader["Performance_80_0_kmph"]),
                                        RadialTyres = reader["RadialTyres"] != DBNull.Value ? SqlReaderConvertor.ToNullableBool(reader["RadialTyres"]) : null,
                                        RearDisc = reader["RearDisc"] != DBNull.Value ? SqlReaderConvertor.ToNullableBool(reader["RearDisc"]) : null,
                                        RearDisc_DrumSize = SqlReaderConvertor.ToNullableUInt16(reader["RearDisc_DrumSize"]),
                                        RearSuspension = Convert.ToString(reader["RearSuspension"]),
                                        RearTyre = Convert.ToString(reader["RearTyre"]),
                                        ReserveFuelCapacity = SqlReaderConvertor.ToNullableFloat(reader["ReserveFuelCapacity"]),
                                        SeatHeight = SqlReaderConvertor.ToNullableUInt16(reader["SeatHeight"]),
                                        SparkPlugsPerCylinder = Convert.ToString(reader["SparkPlugsPerCylinder"]),
                                        Stroke = SqlReaderConvertor.ToNullableFloat(reader["Stroke"]),
                                        TopSpeed = SqlReaderConvertor.ToNullableFloat(reader["TopSpeed"]),
                                        TransmissionType = Convert.ToString(reader["TransmissionType"]),
                                        TubelessTyres = reader["TubelessTyres"] != DBNull.Value ? SqlReaderConvertor.ToNullableBool(reader["TubelessTyres"]) : null,
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
                                        AntilockBrakingSystem = reader["AntilockBrakingSystem"] != DBNull.Value ? SqlReaderConvertor.ToNullableBool(reader["AntilockBrakingSystem"]) : null,
                                        Clock = reader["Clock"] != DBNull.Value ? SqlReaderConvertor.ToNullableBool(reader["Clock"]) : null,
                                        DigitalFuelGauge = reader["DigitalFuelGauge"] != DBNull.Value ? SqlReaderConvertor.ToNullableBool(reader["DigitalFuelGauge"]) : null,
                                        ElectricStart = reader["ElectricStart"] != DBNull.Value ? SqlReaderConvertor.ToNullableBool(reader["ElectricStart"]) : null,
                                        FuelGauge = reader["FuelGauge"] != DBNull.Value ? SqlReaderConvertor.ToNullableBool(reader["FuelGauge"]) : null,
                                        Killswitch = reader["Killswitch"] != DBNull.Value ? SqlReaderConvertor.ToNullableBool(reader["Killswitch"]) : null,
                                        LowBatteryIndicator = reader["LowBatteryIndicator"] != DBNull.Value ? SqlReaderConvertor.ToNullableBool(reader["LowBatteryIndicator"]) : null,
                                        LowFuelIndicator = reader["LowFuelIndicator"] != DBNull.Value ? SqlReaderConvertor.ToNullableBool(reader["LowFuelIndicator"]) : null,
                                        LowOilIndicator = reader["LowOilIndicator"] != DBNull.Value ? SqlReaderConvertor.ToNullableBool(reader["LowOilIndicator"]) : null,
                                        NoOfTripmeters = Convert.ToString(reader["NoOfTripmeters"]),
                                        PillionBackrest = reader["PillionBackrest"] != DBNull.Value ? SqlReaderConvertor.ToNullableBool(reader["PillionBackrest"]) : null,
                                        PillionFootrest = reader["PillionFootrest"] != DBNull.Value ? SqlReaderConvertor.ToNullableBool(reader["PillionFootrest"]) : null,
                                        PillionGrabrail = reader["PillionGrabrail"] != DBNull.Value ? SqlReaderConvertor.ToNullableBool(reader["PillionGrabrail"]) : null,
                                        PillionSeat = reader["PillionSeat"] != DBNull.Value ? SqlReaderConvertor.ToNullableBool(reader["PillionSeat"]) : null,
                                        ShiftLight = reader["ShiftLight"] != DBNull.Value ? SqlReaderConvertor.ToNullableBool(reader["ShiftLight"]) : null,
                                        Speedometer = Convert.ToString(reader["Speedometer"]),
                                        StandAlarm = reader["StandAlarm"] != DBNull.Value ? SqlReaderConvertor.ToNullableBool(reader["StandAlarm"]) : null,
                                        SteppedSeat = reader["SteppedSeat"] != DBNull.Value ? SqlReaderConvertor.ToNullableBool(reader["SteppedSeat"]) : null,
                                        Tachometer = reader["Tachometer"] != DBNull.Value ? SqlReaderConvertor.ToNullableBool(reader["Tachometer"]) : null,
                                        TachometerType = Convert.ToString(reader["TachometerType"]),
                                        Tripmeter = reader["Tripmeter"] != DBNull.Value ? SqlReaderConvertor.ToNullableBool(reader["Tripmeter"]) : null,
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
                            compare = new Entities.Compare.BikeCompareEntity();
                            compare.BasicInfo = basicInfos;
                            compare.Specifications = specs;
                            compare.Features = features;
                            compare.Color = color;
                            reader.Close();
                        }
                    }

                    if (hexCodes != null && hexCodes.Count > 0 && compare.Color != null && compare.Color.Any())
                    {
                        foreach (var mColor in compare.Color)
                        {
                            mColor.HexCodes = new List<string>();
                            foreach (var hexCode in hexCodes)
                            {
                                if (hexCode.ModelColorId.Equals(mColor.ColorId))
                                {
                                    mColor.HexCodes.Add(hexCode.HexCode);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("Bikewale.DAL.Compare.BikeCompareRepository.DoCompare : {0}", versions));
            }

            return compare;
        }


        /// <summary>
        /// Modified By :   Sushil Kumar on 2nd Feb 2017
        /// Description :   To fetch bike comparisiosn data along with its versions and colors
        ///  Modified by : Aditi Srivastava on 18 May 2017
        /// Summary     : used nullable bool for specs and features
        /// Modified By :Snehal Dange on 11 Sep 2017
        /// Summary : Changed sp name and used other version for BikeCompareEntity with 3 additional paramters
        /// Modified BY:Snehal Dange on 10 Nov 2017
        /// Summary : Changed Sp name from 'getcomparisondetails_01092017' to 'getcomparisondetails_10112017'
        /// -----------Added logic for most recent reviews.
        /// </summary>
        /// <param name="versions"></param>
        /// <param name="cityId"></param>
        /// <returns></returns>
        public BikeCompareEntity DoCompare(string versions, uint cityId)
        {

            BikeCompareEntity compare = null;
            IList<BikeEntityBase> basicInfos = null;
            IList<BikeSpecification> specs = null;
            IList<BikeFeature> features = null;
            IList<BikeColor> color = null;
            IList<Bikewale.Entities.Compare.BikeModelColor> hexCodes = null;
            IList<BikeReview> userReviews = null;
            IList<QuestionRatingsValueEntity> userReviewQuestionList = null;
            IList<BikeVersionCompareEntity> versionsList = null;

            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("getcomparisondetails_20122017"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_bikeversions", DbType.String, versions));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_cityid", DbType.UInt16, cityId));

                    if (!string.IsNullOrEmpty(versions) && versions.Contains(','))
                    {
                        compare = GetCompareDataFromReader(ref basicInfos, ref specs, ref features, ref color, ref hexCodes, ref userReviews, ref userReviewQuestionList, ref versionsList, cmd);
                    }

                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("Bikewale.DAL.Compare.DoCompare : {0} - {1}", versions, cityId));
            }
            return compare;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="compare"></param>
        /// <param name="basicInfos"></param>
        /// <param name="specs"></param>
        /// <param name="features"></param>
        /// <param name="color"></param>
        /// <param name="hexCodes"></param>
        /// <param name="userReviews"></param>
        /// <param name="userReviewQuestionList"></param>
        /// <param name="versionsList"></param>
        /// <param name="cmd"></param>
        private static BikeCompareEntity GetCompareDataFromReader(ref IList<BikeEntityBase> basicInfos, ref IList<BikeSpecification> specs, ref IList<BikeFeature> features, ref IList<BikeColor> color, ref IList<Entities.Compare.BikeModelColor> hexCodes, ref IList<BikeReview> userReviews, ref IList<QuestionRatingsValueEntity> userReviewQuestionList, ref IList<BikeVersionCompareEntity> versionsList, DbCommand cmd)
        {
            BikeCompareEntity compare = new BikeCompareEntity();
            using (IDataReader reader = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
            {
                if (reader != null)
                {
                    versionsList = new List<BikeVersionCompareEntity>();
                    while (reader.Read())
                    {
                        versionsList.Add(new BikeVersionCompareEntity()
                        {
                            ModelId = SqlReaderConvertor.ToUInt32(reader["BikeModelId"]),
                            VersionId = SqlReaderConvertor.ToInt32(reader["VersionId"]),
                            VersionName = Convert.ToString(reader["VersionName"])
                        });
                    }
                }

                if (reader != null && reader.NextResult())
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
                            IsNew = SqlReaderConvertor.ToBoolean(reader["isnewmodel"]),
                            IsUpcoming = SqlReaderConvertor.ToBoolean(reader["futuristic"]),
                            IsDiscontinued = SqlReaderConvertor.ToBoolean(reader["isusedmodel"]) && !SqlReaderConvertor.ToBoolean(reader["isnewmodel"]),
                            UsedBikeCount = new Entities.Used.UsedBikesCountInCity()
                            {
                                BikeCount = SqlReaderConvertor.ToUInt32(reader["bikeCount"]),
                                StartingPrice = SqlReaderConvertor.ToUInt32(reader["minPrice"]),
                                CityMaskingName = Convert.ToString(reader["citymaskingname"])
                            },
                            Versions = versionsList.Where(x => x.ModelId == modelId).ToList()
                        });


                        #endregion

                        #region Bike Specification
                        specs.Add(new BikeSpecification()
                        {
                            AlloyWheels = reader["AlloyWheels"] != DBNull.Value ? SqlReaderConvertor.ToNullableBool(reader["AlloyWheels"]) : null,
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
                            FrontDisc = reader["FrontDisc"] != DBNull.Value ? SqlReaderConvertor.ToNullableBool(reader["FrontDisc"]) : null,
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
                            PassLight = reader["PassLight"] != DBNull.Value ? SqlReaderConvertor.ToNullableBool(reader["PassLight"]) : null,
                            Performance_0_40_m = SqlReaderConvertor.ToNullableFloat(reader["Performance_0_40_m"]),
                            Performance_0_60_kmph = SqlReaderConvertor.ToNullableFloat(reader["Performance_0_60_kmph"]),
                            Performance_0_80_kmph = SqlReaderConvertor.ToNullableFloat(reader["Performance_0_80_kmph"]),
                            Performance_60_0_kmph = Convert.ToString(reader["Performance_60_0_kmph"]),
                            Performance_80_0_kmph = Convert.ToString(reader["Performance_80_0_kmph"]),
                            RadialTyres = reader["RadialTyres"] != DBNull.Value ? SqlReaderConvertor.ToNullableBool(reader["RadialTyres"]) : null,
                            RearDisc = reader["RearDisc"] != DBNull.Value ? SqlReaderConvertor.ToNullableBool(reader["RearDisc"]) : null,
                            RearDisc_DrumSize = SqlReaderConvertor.ToNullableUInt16(reader["RearDisc_DrumSize"]),
                            RearSuspension = Convert.ToString(reader["RearSuspension"]),
                            RearTyre = Convert.ToString(reader["RearTyre"]),
                            ReserveFuelCapacity = SqlReaderConvertor.ToNullableFloat(reader["ReserveFuelCapacity"]),
                            SeatHeight = SqlReaderConvertor.ToNullableUInt16(reader["SeatHeight"]),
                            SparkPlugsPerCylinder = Convert.ToString(reader["SparkPlugsPerCylinder"]),
                            Stroke = SqlReaderConvertor.ToNullableFloat(reader["Stroke"]),
                            TopSpeed = SqlReaderConvertor.ToNullableFloat(reader["TopSpeed"]),
                            TransmissionType = Convert.ToString(reader["TransmissionType"]),
                            TubelessTyres = reader["TubelessTyres"] != DBNull.Value ? SqlReaderConvertor.ToNullableBool(reader["TubelessTyres"]) : null,
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
                            AntilockBrakingSystem = reader["TubelessTyres"] != DBNull.Value ? SqlReaderConvertor.ToNullableBool(reader["AntilockBrakingSystem"]) : null,
                            Clock = reader["Clock"] != DBNull.Value ? SqlReaderConvertor.ToNullableBool(reader["Clock"]) : null,
                            DigitalFuelGauge = reader["DigitalFuelGauge"] != DBNull.Value ? SqlReaderConvertor.ToNullableBool(reader["DigitalFuelGauge"]) : null,
                            ElectricStart = reader["ElectricStart"] != DBNull.Value ? SqlReaderConvertor.ToNullableBool(reader["ElectricStart"]) : null,
                            FuelGauge = reader["FuelGauge"] != DBNull.Value ? SqlReaderConvertor.ToNullableBool(reader["FuelGauge"]) : null,
                            Killswitch = reader["Killswitch"] != DBNull.Value ? SqlReaderConvertor.ToNullableBool(reader["Killswitch"]) : null,
                            LowBatteryIndicator = reader["LowBatteryIndicator"] != DBNull.Value ? SqlReaderConvertor.ToNullableBool(reader["LowBatteryIndicator"]) : null,
                            LowFuelIndicator = reader["LowFuelIndicator"] != DBNull.Value ? SqlReaderConvertor.ToNullableBool(reader["LowFuelIndicator"]) : null,
                            LowOilIndicator = reader["LowOilIndicator"] != DBNull.Value ? SqlReaderConvertor.ToNullableBool(reader["LowOilIndicator"]) : null,
                            NoOfTripmeters = Convert.ToString(reader["NoOfTripmeters"]),
                            PillionBackrest = reader["PillionBackrest"] != DBNull.Value ? SqlReaderConvertor.ToNullableBool(reader["PillionBackrest"]) : null,
                            PillionFootrest = reader["PillionFootrest"] != DBNull.Value ? SqlReaderConvertor.ToNullableBool(reader["PillionFootrest"]) : null,
                            PillionGrabrail = reader["PillionGrabrail"] != DBNull.Value ? SqlReaderConvertor.ToNullableBool(reader["PillionGrabrail"]) : null,
                            PillionSeat = reader["PillionSeat"] != DBNull.Value ? SqlReaderConvertor.ToNullableBool(reader["PillionSeat"]) : null,
                            ShiftLight = reader["ShiftLight"] != DBNull.Value ? SqlReaderConvertor.ToNullableBool(reader["ShiftLight"]) : null,
                            Speedometer = Convert.ToString(reader["Speedometer"]),
                            StandAlarm = reader["StandAlarm"] != DBNull.Value ? SqlReaderConvertor.ToNullableBool(reader["StandAlarm"]) : null,
                            SteppedSeat = reader["SteppedSeat"] != DBNull.Value ? SqlReaderConvertor.ToNullableBool(reader["SteppedSeat"]) : null,
                            Tachometer = reader["Tachometer"] != DBNull.Value ? SqlReaderConvertor.ToNullableBool(reader["Tachometer"]) : null,
                            TachometerType = Convert.ToString(reader["TachometerType"]),
                            Tripmeter = reader["Tripmeter"] != DBNull.Value ? SqlReaderConvertor.ToNullableBool(reader["Tripmeter"]) : null,
                            TripmeterType = Convert.ToString(reader["TripmeterType"]),
                            VersionId = SqlReaderConvertor.ToUInt32(reader["BikeVersionId"])
                        });
                        #endregion
                    }
                }

                if (reader != null && reader.NextResult())
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


                #region User Reviews

                if (reader != null && reader.NextResult())
                {
                    userReviews = new List<BikeReview>();
                    while (reader.Read())
                    {
                        userReviews.Add(new BikeReview()
                        {
                            VersionId = SqlReaderConvertor.ToUInt32(reader["versionid"]),
                            ModelReview = new Entities.UserReviews.V2.ModelWiseUserReview()
                            {
                                VersionId = SqlReaderConvertor.ToUInt32(reader["versionid"]),
                                ModelId = SqlReaderConvertor.ToUInt32(reader["ModelId"]),
                                RatingCount = SqlReaderConvertor.ToUInt32(reader["ratingscount"]),
                                ReviewRate = SqlReaderConvertor.ToFloat(reader["reviewrate"]),
                                Mileage = SqlReaderConvertor.ToUInt32(reader["modelmileage"]),
                                ReviewCount = SqlReaderConvertor.ToUInt32(reader["reviewcount"]),
                                UserReviews = new Entities.UserReviews.V2.UserReviewSummary()
                                {
                                    ReviewId = SqlReaderConvertor.ToUInt32(reader["Id"]),
                                    OverallRatingId = SqlReaderConvertor.ToUInt16(reader["OverallRatingId"]),
                                    Description = Convert.ToString(reader["Review"]),
                                    Title = Convert.ToString(reader["Title"])
                                }
                            }
                        });
                    }
                }

                if (reader != null && reader.NextResult())
                {
                    userReviewQuestionList = new List<QuestionRatingsValueEntity>();
                    while (reader.Read())
                    {
                        userReviewQuestionList.Add(new QuestionRatingsValueEntity()
                        {
                            VersionId = SqlReaderConvertor.ToUInt32(reader["versionid"]),
                            ModelId = SqlReaderConvertor.ToUInt32(reader["modelId"]),
                            QuestionId = SqlReaderConvertor.ToUInt16(reader["questionId"]),
                            AverageRatingValue = SqlReaderConvertor.ToFloat(reader["aggregateValue"]),
                            QuestionHeading = Convert.ToString(reader["heading"]),
                            QuestionDescription = Convert.ToString(reader["description"])
                        });
                    }

                }
                if (reader != null)
                {
                    reader.Close();
                }
                #endregion

                compare.BasicInfo = basicInfos;
                compare.Specifications = specs;
                compare.Features = features;
                compare.Color = color;

            }

            if (userReviewQuestionList != null && userReviews != null)
            {
                var groups = userReviewQuestionList.GroupBy(x => x.VersionId);
                foreach (var group in groups)
                {
                    userReviews.FirstOrDefault(s => s.VersionId == group.Key).ModelReview.Questions = group.ToList();
                }

                compare.Reviews = userReviews;
            }

            if (hexCodes != null && hexCodes.Count > 0 && compare.Color != null && compare.Color.Any())
            {
                foreach (var mColor in compare.Color)
                {
                    mColor.HexCodes = new List<string>();
                    foreach (var hexCode in hexCodes)
                    {
                        if (hexCode.ModelColorId.Equals(mColor.ColorId))
                        {
                            mColor.HexCodes.Add(hexCode.HexCode);
                        }
                    }
                }
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

            catch (Exception ex)
            {
                ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"]);
            }

            return topBikeList;
        }

        /// <summary>
        /// Created by : Aditi Srivastava on 24 Apr 2017
        /// Summary    : Get comparison of popular bikes
        /// Modified by : Aditi Srivastava on 2 June 2017
        /// Summary     : Added flag and end and start date for sponsored comparison and DisplayPriority
        /// </summary>
        public IEnumerable<SimilarCompareBikeEntity> GetPopularCompareList(uint cityId)
        {
            List<SimilarCompareBikeEntity> topBikeList = null;
            IList<SimilarCompareBikeEntity> topBikes = null;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("getbikecomparison_17062017"))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_cityid", DbType.Int16, cityId));
                    using (IDataReader reader = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (reader != null)
                        {
                            topBikeList = new List<SimilarCompareBikeEntity>();
                            while (reader.Read())
                            {
                                SimilarCompareBikeEntity obj = new SimilarCompareBikeEntity();
                                obj.ID = SqlReaderConvertor.ToInt32(reader["ID"]);
                                obj.Make1 = Convert.ToString(reader["Make"]);
                                obj.MakeMasking1 = Convert.ToString(reader["MakeMaskingName"]);
                                obj.ModelId1 = SqlReaderConvertor.ToUInt16(reader["ModelId"]);
                                obj.Model1 = Convert.ToString(reader["Model"]);
                                obj.ModelMasking1 = Convert.ToString(reader["ModelMaskingName"]);
                                obj.Bike1 = string.Format("{0} {1}", obj.Make1, obj.Model1);

                                obj.Price1 = SqlReaderConvertor.ToInt32(reader["Price"]);
                                obj.City1 = Convert.ToString(reader["City"]);

                                obj.VersionId1 = Convert.ToString(reader["VersionId"]);
                                obj.OriginalImagePath1 = Convert.ToString(reader["VersionImgUrl"]);
                                obj.HostUrl1 = Convert.ToString(reader["HostUrl"]);
                                obj.IsScooterOnly = SqlReaderConvertor.ToBoolean(reader["IsScooter"]);
                                obj.BodyStyle1 = SqlReaderConvertor.ToUInt16(reader["bodystyleid"]);
                                obj.IsSponsored = SqlReaderConvertor.ToBoolean(reader["IsSponsored"]);
                                obj.SponsoredStartDate = SqlReaderConvertor.ToDateTime(reader["SponsoredStartDate"]);
                                obj.SponsoredEndDate = SqlReaderConvertor.ToDateTime(reader["SponsoredEndDate"]);
                                obj.DisplayPriority = SqlReaderConvertor.ToUInt16(reader["priority"]);
                                topBikeList.Add(obj);
                            }
                            reader.Close();
                        }
                    }
                    if (topBikeList != null)
                    {
                        var bikeList = topBikeList.GroupBy(x => x.ID);
                        topBikes = new List<SimilarCompareBikeEntity>();
                        foreach (var bike in bikeList)
                        {
                            var bike1 = bike.First();
                            var bike2 = bike.Last();
                            topBikes.Add(new SimilarCompareBikeEntity()
                            {
                                ID = bike.Key,
                                Bike1 = bike1.Bike1,
                                Bike2 = bike2.Bike1,
                                Make1 = bike1.Make1,
                                Make2 = bike2.Make1,
                                Model1 = bike1.Model1,
                                Model2 = bike2.Model1,
                                MakeMasking1 = bike1.MakeMasking1,
                                MakeMasking2 = bike2.MakeMasking1,
                                ModelId1 = bike1.ModelId1,
                                ModelId2 = bike2.ModelId1,
                                ModelMasking1 = bike1.ModelMasking1,
                                ModelMasking2 = bike2.ModelMasking1,
                                Price1 = bike1.Price1,
                                Price2 = bike2.Price1,
                                City1 = bike1.City1,
                                City2 = bike2.City1,
                                VersionId1 = bike1.VersionId1,
                                VersionId2 = bike2.VersionId1,
                                OriginalImagePath1 = bike1.OriginalImagePath1,
                                OriginalImagePath2 = bike2.OriginalImagePath1,
                                HostUrl1 = bike1.HostUrl1,
                                HostUrl2 = bike2.HostUrl1,
                                BodyStyle1 = bike1.BodyStyle1,
                                BodyStyle2 = bike2.BodyStyle1,
                                IsScooterOnly = (bike1.IsScooterOnly && bike2.IsScooterOnly),
                                IsSponsored = (bike1.IsSponsored && bike2.IsSponsored),
                                SponsoredStartDate = bike1.SponsoredStartDate,
                                SponsoredEndDate = bike1.SponsoredEndDate,
                                DisplayPriority = bike1.DisplayPriority
                            });
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.DAL.Compare.GetPopularCompareList");
            }

            return topBikes;
        }

        /// <summary>
        /// Created by: Sangram Nandkhile on 11 May 2016
        /// Modified by :Subodh Jain on 21 oct 2016
        /// Desc : Added cityid as parameter
        /// Modified By : Sushil Kumar on 2nd Dec 2016
        /// Description : DAL layer to similar comaprisions bikes changed topcount to ushort  
        /// Modified By :-Subodh Jain 23 May 2017 
        /// Added :- Bike1 and Bike 2
        /// Modified by : Ashutosh Sharma on 03 Oct 2017
        /// Description : Changed SP from 'getsimilarcomparebikeslist_27042017' to 'getsimilarcomparebikeslist_03102017', to get avg price.
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
                    cmd.CommandText = "getsimilarcomparebikeslist_03102017";
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_bikeversionidlist", DbType.String, 100, versionList));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_topcount", DbType.Int16, topCount));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_cityid", DbType.Int32, cityid));
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
                                    ModelId1 = SqlReaderConvertor.ToUInt32(reader["ModelId1"]),
                                    ModelId2 = SqlReaderConvertor.ToUInt32(reader["ModelId2"]),
                                    OriginalImagePath1 = Convert.ToString(reader["OriginalImagePath1"]),
                                    OriginalImagePath2 = Convert.ToString(reader["OriginalImagePath2"]),
                                    Price1 = SqlReaderConvertor.ToInt32(reader["Price1"]),
                                    Price2 = SqlReaderConvertor.ToInt32(reader["Price2"]),
                                    AvgPrice1 = SqlReaderConvertor.ToInt32(reader["AvgPrice1"]),
                                    AvgPrice2 = SqlReaderConvertor.ToInt32(reader["AvgPrice2"]),
                                    HostUrl1 = Convert.ToString(reader["HostUrl1"]),
                                    HostUrl2 = Convert.ToString(reader["HostUrl2"]),
                                    City1 = Convert.ToString(reader["city1"]),
                                    City2 = Convert.ToString(reader["city2"]),
                                    Bike1 = string.Format("{0} {1}", Convert.ToString(reader["Make1"]), Convert.ToString(reader["Model1"])),
                                    Bike2 = string.Format("{0} {1}", Convert.ToString(reader["Make2"]), Convert.ToString(reader["Model2"]))
                                });
                            }
                            reader.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("BikeCompareRepository_GetSimilarCompareBikes_{0}_Cnt_{1}_City_{2}", versionList, topCount, cityid));
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
                ErrorClass.LogError(ex, string.Format("BikeCompareCacheRepository_GetSimilarCompareBikeSponsored_{0}_Cnt_{1}_SP_{2}_City_{3}", versionList, topCount, sponsoredVersionId, cityid));
                
            }

            return similarBikeList;
        }


        /// <summary>
        /// Created By :- Subodh Jain 10 March 2017
        /// Summary :- Populate Compare ScootersList
        /// </summary>
        public IEnumerable<TopBikeCompareBase> ScooterCompareList(uint topCount)
        {
            IList<TopBikeCompareBase> topBikeList = null;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("getscootercomparisonmin"))
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

            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("BikeCompareRepository.ScooterCompareList topCount:{0}", topCount));
            }

            return topBikeList;

        }

        /// <summary>
        /// Created by : Aditi Srivastava on 2 June 2017
        /// Summary    : Get comparison of popular scooters
        /// </summary>
        public IEnumerable<SimilarCompareBikeEntity> GetScooterCompareList(uint cityId)
        {
            List<SimilarCompareBikeEntity> topBikeList = null;
            IList<SimilarCompareBikeEntity> topBikes = null;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("getscootercomparison_17062017"))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_cityid", DbType.Int16, cityId));
                    using (IDataReader reader = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (reader != null)
                        {
                            topBikeList = new List<SimilarCompareBikeEntity>();
                            while (reader.Read())
                            {
                                SimilarCompareBikeEntity obj = new SimilarCompareBikeEntity();
                                obj.ID = SqlReaderConvertor.ToInt32(reader["ID"]);
                                obj.Make1 = Convert.ToString(reader["Make"]);
                                obj.MakeMasking1 = Convert.ToString(reader["MakeMaskingName"]);
                                obj.ModelId1 = SqlReaderConvertor.ToUInt16(reader["ModelId"]);
                                obj.Model1 = Convert.ToString(reader["Model"]);
                                obj.ModelMasking1 = Convert.ToString(reader["ModelMaskingName"]);
                                obj.Bike1 = string.Format("{0} {1}", obj.Make1, obj.Model1);

                                obj.Price1 = SqlReaderConvertor.ToInt32(reader["Price"]);
                                obj.City1 = Convert.ToString(reader["City"]);

                                obj.VersionId1 = Convert.ToString(reader["VersionId"]);
                                obj.OriginalImagePath1 = Convert.ToString(reader["VersionImgUrl"]);
                                obj.HostUrl1 = Convert.ToString(reader["HostUrl"]);
                                obj.BodyStyle1 = SqlReaderConvertor.ToUInt16(reader["bodystyleid"]);
                                obj.IsSponsored = SqlReaderConvertor.ToBoolean(reader["IsSponsored"]);
                                obj.SponsoredStartDate = SqlReaderConvertor.ToDateTime(reader["SponsoredStartDate"]);
                                obj.SponsoredEndDate = SqlReaderConvertor.ToDateTime(reader["SponsoredEndDate"]);
                                obj.DisplayPriority = SqlReaderConvertor.ToUInt16(reader["priority"]);
                                topBikeList.Add(obj);
                            }
                            reader.Close();
                        }
                    }
                    if (topBikeList != null)
                    {
                        var bikeList = topBikeList.GroupBy(x => x.ID);
                        topBikes = new List<SimilarCompareBikeEntity>();
                        foreach (var bike in bikeList)
                        {
                            var bike1 = bike.First();
                            var bike2 = bike.Last();
                            topBikes.Add(new SimilarCompareBikeEntity()
                            {
                                ID = bike.Key,
                                Bike1 = bike1.Bike1,
                                Bike2 = bike2.Bike1,
                                Make1 = bike1.Make1,
                                Make2 = bike2.Make1,
                                Model1 = bike1.Model1,
                                Model2 = bike2.Model1,
                                MakeMasking1 = bike1.MakeMasking1,
                                MakeMasking2 = bike2.MakeMasking1,
                                ModelId1 = bike1.ModelId1,
                                ModelId2 = bike2.ModelId1,
                                ModelMasking1 = bike1.ModelMasking1,
                                ModelMasking2 = bike2.ModelMasking1,
                                Price1 = bike1.Price1,
                                Price2 = bike2.Price1,
                                City1 = bike1.City1,
                                City2 = bike2.City1,
                                VersionId1 = bike1.VersionId1,
                                VersionId2 = bike2.VersionId1,
                                OriginalImagePath1 = bike1.OriginalImagePath1,
                                OriginalImagePath2 = bike2.OriginalImagePath1,
                                HostUrl1 = bike1.HostUrl1,
                                HostUrl2 = bike2.HostUrl1,
                                BodyStyle1 = bike1.BodyStyle1,
                                BodyStyle2 = bike2.BodyStyle1,
                                IsSponsored = (bike1.IsSponsored && bike2.IsSponsored),
                                SponsoredStartDate = bike1.SponsoredStartDate,
                                SponsoredEndDate = bike1.SponsoredEndDate,
                                DisplayPriority = bike1.DisplayPriority
                            });
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("Bikewale.DAL.Compare.GetScooterCompareList- CityId : {0}", cityId));
            }

            return topBikes;
        }


        /// <summary>
        /// Created by:Snehal Dange on 24th Oct 2017
        /// Description : Get similar bikes for bike comparison
        /// </summary>
        /// <param name="versionList"></param>
        /// <param name="topCount"></param>
        /// <param name="cityid"></param>
        /// <returns></returns>
        public SimilarBikeComparisonWrapper GetSimilarBikes(string modelList, ushort topCount)
        {
            SimilarBikeComparisonWrapper similarBikeComparison = null;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("getsimilarbikes"))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_modelidlist", DbType.String, 20, modelList));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_topcount", DbType.Int16, topCount));
                    using (IDataReader reader = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (reader != null)
                        {
                            similarBikeComparison = new SimilarBikeComparisonWrapper();

                            IList<SimilarBikeComparisonData> similarBikeList = new List<SimilarBikeComparisonData>();

                            while (reader.Read())
                            {
                                similarBikeList.Add(new SimilarBikeComparisonData()
                                {
                                    BikeMake = new BikeMakeBase()
                                    {
                                        MakeMaskingName = Convert.ToString(reader["MakeMaskingName"]),
                                        MakeName = Convert.ToString(reader["MakeName"])
                                    },
                                    BikeModel = new BikeModelEntityBase()
                                    {
                                        ModelId = SqlReaderConvertor.ToInt32(reader["similarModelId"]),
                                        MaskingName = Convert.ToString(reader["modelmaskingname"]),
                                        ModelName = Convert.ToString(reader["modelname"])
                                    },
                                    HostUrl = Convert.ToString(reader["HostUrl"]),
                                    OriginalImagePath = Convert.ToString(reader["OriginalImagePath"]),
                                    ModelId1 = SqlReaderConvertor.ToUInt32(reader["bike1"]),
                                    ModelId2 = SqlReaderConvertor.ToUInt32(reader["bike2"]),
                                });
                            }

                            similarBikeComparison.SimilarBikes = similarBikeList;

                            IList<BasicBikeEntityBase> BikeList = new List<BasicBikeEntityBase>();

                            if (reader.NextResult())
                            {
                                while (reader.Read())
                                {
                                    BikeList.Add(new BasicBikeEntityBase()
                                    {
                                        Make = new BikeMakeEntityBase()
                                        {
                                            MaskingName = Convert.ToString(reader["MakeMaskingName"]),
                                            MakeName = Convert.ToString(reader["MakeName"])
                                        },
                                        Model = new BikeModelEntityBase()
                                        {
                                            ModelId = SqlReaderConvertor.ToInt32(reader["modelId"]),
                                            MaskingName = Convert.ToString(reader["modelmaskingname"]),
                                            ModelName = Convert.ToString(reader["modelname"])
                                        },
                                        HostUrl = Convert.ToString(reader["HostUrl"]),
                                        OriginalImagePath = Convert.ToString(reader["OriginalImagePath"])
                                    });
                                }

                            }

                            similarBikeComparison.BikeList = BikeList;

                            reader.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("BikeCompareRepository_GetSimilarBikesForComparisions_{0}_Cnt_{1}", modelList, topCount));
            }

            return similarBikeComparison;
        }

    }
}
