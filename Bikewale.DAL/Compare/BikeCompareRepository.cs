using Bikewale.CoreDAL;
using Bikewale.Entities.Compare;
using Bikewale.Interfaces.Compare;
using Bikewale.Notifications;
using Bikewale.Utility;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Bikewale.DAL.Compare
{
    public class BikeCompareRepository : IBikeCompare
    {
        public BikeCompareEntity DoCompare(string versions)
        {
            BikeCompareEntity compare = null;
            List<BikeEntityBase> basicInfos = null;
            List<BikeSpecification> specs = null;
            List<BikeFeature> features = null;
            List<BikeColor> color = null;
            Database db = null;
            SqlCommand command = null;
            SqlConnection connection = null;
            SqlDataReader reader = null;
            try
            {
                db = new Database();
                using (connection = new SqlConnection(db.GetConString()))
                {
                    using (command = new SqlCommand())
                    {
                        command.CommandText = "GetComparisonDetails";
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Connection = connection;
                        command.Parameters.Add("@BikeVersions", System.Data.SqlDbType.VarChar).Value = versions;

                        connection.Open();
                        reader = command.ExecuteReader();
                        if (reader != null && reader.HasRows)
                        {
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
                                    ModelRating = Convert.ToUInt16(reader["ModelRating"]),
                                    Name = Convert.ToString(reader["Bike"]),
                                    Price = Convert.ToInt32(reader["Price"]),
                                    Version = Convert.ToString(reader["Version"]),
                                    VersionId = Convert.ToUInt32(reader["BikeVersionId"]),
                                    VersionRating = Convert.ToUInt16(reader["VersionRating"])
                                });
                            }
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
                                        VersionId = Convert.ToUInt32(reader["BikeVersionId"]),
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
                                        Colors = Convert.ToString(reader["Colors"]),
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
                                        VersionId = Convert.ToUInt32(reader["BikeVersionId"])
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
                                        ColorId = Convert.ToInt32(reader["ColorId"]),
                                        Color = Convert.ToString(reader["Color"]),
                                        HexCode = Convert.ToString(reader["HexCode"]),
                                        VersionId = Convert.ToUInt32(reader["BikeVersionID"])
                                    });
                                }
                            }
                            #endregion
                            compare = new BikeCompareEntity();
                            compare.BasicInfo = basicInfos;
                            compare.Specifications = specs;
                            compare.Features = features;
                            compare.Color = color;
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
            return compare;
        }


        public IEnumerable<TopBikeCompareBase> CompareList(uint topCount)
        {
            Database db = null;
            SqlCommand command = null;
            SqlConnection connection = null;
            SqlDataReader reader = null;
            List<TopBikeCompareBase> topBikeList = null;
            try
            {
                db = new Database();
                using (connection = new SqlConnection(db.GetConString()))
                {
                    using (command = new SqlCommand())
                    {
                        command.CommandText = "GetBikeComparisonMin";
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Connection = connection;
                        command.Parameters.Add("@TopCount", System.Data.SqlDbType.SmallInt).Value = topCount;

                        connection.Open();
                        reader = command.ExecuteReader();
                        if (reader != null && reader.HasRows)
                        {
                            topBikeList = new List<TopBikeCompareBase>();
                            while (reader.Read())
                            {
                                topBikeList.Add(new TopBikeCompareBase()
                                {
                                    Bike1 = Convert.ToString(reader["Bike1"]),
                                    Bike2 = Convert.ToString(reader["Bike2"]),
                                    HostURL = Convert.ToString(reader["HostURL"]),
                                    ID = Convert.ToInt32(reader["ID"]),
                                    MakeMaskingName2 = Convert.ToString(reader["MakeMakingName2"]),
                                    MakeMaskingName1 = Convert.ToString(reader["MakeMaskingName1"]),
                                    ModelId1 = Convert.ToUInt16(reader["ModelId1"]),
                                    ModelId2 = Convert.ToUInt16(reader["ModelId2"]),
                                    ModelMaskingName1 = Convert.ToString(reader["ModelMaskingName1"]),
                                    ModelMaskingName2 = Convert.ToString(reader["ModelMaskingName2"]),
                                    OriginalImagePath = Convert.ToString(reader["OriginalImagePath"]),
                                    Price1 = Convert.ToUInt32(reader["Price1"]),
                                    Price2 = Convert.ToUInt32(reader["Price2"]),
                                    Review1 = Convert.ToUInt16(reader["Review1"]),
                                    Review2 = Convert.ToUInt16(reader["Review2"]),
                                    ReviewCount1 = Convert.ToUInt16(reader["ReviewCount1"]),
                                    ReviewCount2 = Convert.ToUInt16(reader["ReviewCount2"]),
                                    VersionId1 = Convert.ToUInt16(reader["VersionId1"]),
                                    VersionId2 = Convert.ToUInt16(reader["VersionId2"])
                                });
                            }
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
    }
}
