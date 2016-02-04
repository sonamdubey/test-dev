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
            ICollection<BikeEntityBase> basicInfos = null;
            ICollection<BikeSpecification> specs = null;
            ICollection<BikeFeature> features = null;
            List<BikeColor> color = null;
            ICollection<BikeModelColor> hexCodes = null;            
            Database db = null;
            SqlCommand command = null;
            SqlConnection connection = null;

            try
            {
                db = new Database();
                using (connection = new SqlConnection(db.GetConString()))
                {
                    using (command = new SqlCommand())
                    {
                        command.CommandText = "GetComparisonDetails_20012016";
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Connection = connection;
                        command.Parameters.Add("@BikeVersions", System.Data.SqlDbType.VarChar).Value = versions;

                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader != null && reader.HasRows)
                            {
                                #region Basic Info
                                basicInfos = new List<BikeEntityBase>();
                                while (reader.Read())
                                {
                                    basicInfos.Add(new BikeEntityBase()
                                    {
                                        HostUrl = GetString(reader["HostURL"]),
                                        ImagePath = GetString(reader["OriginalImagePath"]),
                                        Make = GetString(reader["Make"]),
                                        MakeMaskingName = GetString(reader["MakeMaskingName"]),
                                        Model = GetString(reader["Model"]),
                                        ModelMaskingName = GetString(reader["ModelMaskingName"]),
                                        ModelRating = GetUInt16(reader["ModelRating"]),
                                        Name = GetString(reader["Bike"]),
                                        Price = GetInt32(reader["Price"]),
                                        Version = GetString(reader["Version"]),
                                        VersionId = GetUint32(reader["BikeVersionId"]),
                                        VersionRating = GetUInt16(reader["VersionRating"])
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
                                            Battery = GetString(reader["Battery"]),
                                            Bore = SqlReaderConvertor.ToNullableFloat(reader["Bore"]),
                                            Brake_Tail_Light = GetString(reader["Brake_Tail_Light"]),
                                            BrakeType = GetString(reader["BrakeType"]),
                                            CalliperType = GetString(reader["CalliperType"]),
                                            ChassisType = GetString(reader["ChassisType"]),
                                            Clutch = GetString(reader["Clutch"]),
                                            CoolingSystem = GetString(reader["CoolingSystem"]),
                                            Cylinders = SqlReaderConvertor.ToNullableUInt16(reader["Cylinders"]),
                                            Displacement = SqlReaderConvertor.ToNullableFloat(reader["Displacement"]),
                                            ElectricSystem = GetString(reader["ElectricSystem"]),
                                            FrontDisc = SqlReaderConvertor.ToNullableBool(reader["FrontDisc"]),
                                            FrontDisc_DrumSize = SqlReaderConvertor.ToNullableUInt16(reader["FrontDisc_DrumSize"]),
                                            FrontSuspension = GetString(reader["FrontSuspension"]),
                                            FrontTyre = GetString(reader["FrontTyre"]),
                                            FuelDeliverySystem = GetString(reader["FuelDeliverySystem"]),
                                            FuelEfficiencyOverall = SqlReaderConvertor.ToNullableUInt16(reader["FuelEfficiencyOverall"]),
                                            FuelEfficiencyRange = SqlReaderConvertor.ToNullableUInt16(reader["FuelEfficiencyRange"]),
                                            FuelTankCapacity = SqlReaderConvertor.ToNullableFloat(reader["FuelTankCapacity"]),
                                            FuelType = GetString(reader["FuelType"]),
                                            GearboxType = GetString(reader["GearboxType"]),
                                            GroundClearance = SqlReaderConvertor.ToNullableUInt16(reader["GroundClearance"]),
                                            HeadlightBulbType = GetString(reader["HeadlightBulbType"]),
                                            HeadlightType = GetString(reader["HeadlightType"]),
                                            Ignition = GetString(reader["Ignition"]),
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
                                            Performance_60_0_kmph = GetString(reader["Performance_60_0_kmph"]),
                                            Performance_80_0_kmph = GetString(reader["Performance_80_0_kmph"]),
                                            RadialTyres = SqlReaderConvertor.ToNullableBool(reader["RadialTyres"]),
                                            RearDisc = SqlReaderConvertor.ToNullableBool(reader["RearDisc"]),
                                            RearDisc_DrumSize = SqlReaderConvertor.ToNullableUInt16(reader["RearDisc_DrumSize"]),
                                            RearSuspension = GetString(reader["RearSuspension"]),
                                            RearTyre = GetString(reader["RearTyre"]),
                                            ReserveFuelCapacity = SqlReaderConvertor.ToNullableFloat(reader["ReserveFuelCapacity"]),
                                            SeatHeight = SqlReaderConvertor.ToNullableUInt16(reader["SeatHeight"]),
                                            SparkPlugsPerCylinder = GetString(reader["SparkPlugsPerCylinder"]),
                                            Stroke = SqlReaderConvertor.ToNullableFloat(reader["Stroke"]),
                                            TopSpeed = SqlReaderConvertor.ToNullableFloat(reader["TopSpeed"]),
                                            TransmissionType = GetString(reader["TransmissionType"]),
                                            TubelessTyres = SqlReaderConvertor.ToNullableBool(reader["TubelessTyres"]),
                                            TurnSignal = GetString(reader["TurnSignal"]),
                                            ValvesPerCylinder = SqlReaderConvertor.ToNullableUInt16(reader["ValvesPerCylinder"]),
                                            VersionId = GetUint32(reader["BikeVersionId"]),
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
                                            NoOfTripmeters = GetString(reader["NoOfTripmeters"]),
                                            PillionBackrest = SqlReaderConvertor.ToNullableBool(reader["PillionBackrest"]),
                                            PillionFootrest = SqlReaderConvertor.ToNullableBool(reader["PillionFootrest"]),
                                            PillionGrabrail = SqlReaderConvertor.ToNullableBool(reader["PillionGrabrail"]),
                                            PillionSeat = SqlReaderConvertor.ToNullableBool(reader["PillionSeat"]),
                                            ShiftLight = SqlReaderConvertor.ToNullableBool(reader["ShiftLight"]),
                                            Speedometer = GetString(reader["Speedometer"]),
                                            StandAlarm = SqlReaderConvertor.ToNullableBool(reader["StandAlarm"]),
                                            SteppedSeat = SqlReaderConvertor.ToNullableBool(reader["SteppedSeat"]),
                                            Tachometer = SqlReaderConvertor.ToNullableBool(reader["Tachometer"]),
                                            TachometerType = GetString(reader["TachometerType"]),
                                            Tripmeter = SqlReaderConvertor.ToNullableBool(reader["Tripmeter"]),
                                            TripmeterType = GetString(reader["TripmeterType"]),
                                            VersionId = GetUint32(reader["BikeVersionId"])
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
                                            ColorId = GetInt32(reader["ModelColorId"]),
                                            Color = GetString(reader["ColorName"]),
                                            VersionId = GetUint32(reader["VersionId"])
                                        });
                                    }
                                }
                                #endregion
                                #region Color Hex Codes
                                if (reader.NextResult())
                                {
                                    hexCodes = new List<BikeModelColor>();
                                    while (reader.Read())
                                    {
                                        hexCodes.Add(new BikeModelColor()
                                        {
                                            ModelColorId = GetInt32(reader["ModelColorId"]),
                                            HexCode = GetString(reader["HexCode"])
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

                        compare.Color.ForEach(
                            _color => _color.HexCodes = 
                                (from hexCode in hexCodes
                                where hexCode.ModelColorId == _color.ColorId                                
                                select hexCode.HexCode)
                            );
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
            finally
            {
                if (connection != null && connection.State == System.Data.ConnectionState.Open)
                    connection.Close();
            }
            return compare;
        }

        /// <summary>
        /// Modified by :   Sumit Kate on 29 Jan 2016
        /// Description :   Populate Versions image related entity properties.
        /// </summary>
        /// <param name="topCount"></param>
        /// <returns></returns>
        public IEnumerable<TopBikeCompareBase> CompareList(uint topCount)
        {
            Database db = null;
            SqlCommand command = null;
            List<TopBikeCompareBase> topBikeList = null;
            try
            {
                db = new Database();
                using (SqlConnection connection = new SqlConnection(db.GetConString()))
                {
                    using (command = new SqlCommand())
                    {
                        command.CommandText = "GetBikeComparisonMin_29012016";
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Connection = connection;
                        command.Parameters.Add("@TopCount", System.Data.SqlDbType.SmallInt).Value = topCount;

                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader != null && reader.HasRows)
                            {
                                topBikeList = new List<TopBikeCompareBase>();
                                while (reader.Read())
                                {
                                    topBikeList.Add(new TopBikeCompareBase()
                                    {
                                        Bike1 = GetString(reader["Bike1"]),
                                        Bike2 = GetString(reader["Bike2"]),
                                        HostURL = GetString(reader["HostURL"]),
                                        ID = GetInt32(reader["ID"]),
                                        MakeMaskingName2 = GetString(reader["MakeMakingName2"]),
                                        MakeMaskingName1 = GetString(reader["MakeMaskingName1"]),
                                        ModelId1 = GetUInt16(reader["ModelId1"]),
                                        ModelId2 = GetUInt16(reader["ModelId2"]),
                                        ModelMaskingName1 = GetString(reader["ModelMaskingName1"]),
                                        ModelMaskingName2 = GetString(reader["ModelMaskingName2"]),
                                        OriginalImagePath = GetString(reader["OriginalImagePath"]),
                                        Price1 = GetUint32(reader["Price1"]),
                                        Price2 = GetUint32(reader["Price2"]),
                                        Review1 = GetUInt16(reader["Review1"]),
                                        Review2 = GetUInt16(reader["Review2"]),
                                        ReviewCount1 = GetUInt16(reader["ReviewCount1"]),
                                        ReviewCount2 = GetUInt16(reader["ReviewCount2"]),
                                        VersionId1 = GetUInt16(reader["VersionId1"]),
                                        VersionId2 = GetUInt16(reader["VersionId2"]),
                                        VersionImgUrl1 = GetString(reader["VersionImgUrl1"]),
                                        VersionImgUrl2 = GetString(reader["VersionImgUrl2"]),
                                        HostUrl1 = GetString(reader["HostUrl1"]),
                                        HostUrl2 = GetString(reader["HostUrl2"])
                                    });
                                }
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

        private uint GetUint32(object o)
        {
            return (DBNull.Value == o) ? 0 : Convert.ToUInt32(o);
        }

        private int GetInt32(object o)
        {
            return (DBNull.Value == o) ? 0 : Convert.ToInt32(o);
        }

        private short GetInt16(object o)
        {
            short s = 0;
            return (DBNull.Value == o) ? s : Convert.ToInt16(o);
        }

        private ushort GetUInt16(object o)
        {
            ushort s = 0;
            return (DBNull.Value == o) ? s : Convert.ToUInt16(o);
        }

        private string GetString(object o)
        {
            return (DBNull.Value == o) ? string.Empty : o.ToString();
        }
    }
}
