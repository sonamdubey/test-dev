﻿using Bikewale.Entities.BikeData;
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

                    // LogLiveSps.LogSpInGrayLog(cmd);
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
                                hexCodes = new List<Bikewale.Entities.Compare.BikeModelColor>();
                                while (reader.Read())
                                {
                                    hexCodes.Add(new Bikewale.Entities.Compare.BikeModelColor()
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

        /// <summary>
        /// Modified by :   Sumit Kate on 29 Jan 2016
        /// Description :   Populate Versions image related entity properties.
        /// </summary>
        /// <param name="topCount"></param>
        /// <returns></returns>
        public IEnumerable<TopBikeCompareBase> CompareList(uint topCount)
        {
            List<TopBikeCompareBase> topBikeList = null;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("getbikecomparisonmin_29012016"))
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
                                    Bike1 = GetString(reader["Bike1"]),
                                    Bike2 = GetString(reader["Bike2"]),
                                    ID = GetInt32(reader["ID"]),
                                    MakeMaskingName2 = GetString(reader["MakeMakingName2"]),
                                    MakeMaskingName1 = GetString(reader["MakeMaskingName1"]),
                                    ModelId1 = GetUInt16(reader["ModelId1"]),
                                    ModelId2 = GetUInt16(reader["ModelId2"]),
                                    ModelMaskingName1 = GetString(reader["ModelMaskingName1"]),
                                    ModelMaskingName2 = GetString(reader["ModelMaskingName2"]),

                                    Price1 = GetUint32(reader["Price1"]),
                                    Price2 = GetUint32(reader["Price2"]),

                                    HostURL = GetString(reader["HostUrl"]),
                                    OriginalImagePath = GetString(reader["OriginalImagePath"]),

                                    VersionId1 = GetUInt16(reader["VersionId1"]),
                                    VersionId2 = GetUInt16(reader["VersionId2"]),
                                    VersionImgUrl1 = GetString(reader["VersionImgUrl1"]),
                                    VersionImgUrl2 = GetString(reader["VersionImgUrl2"]),
                                    HostUrl1 = GetString(reader["HostUrl1"]),
                                    HostUrl2 = GetString(reader["HostUrl2"])
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
        /// </summary>
        /// <param name="versionList"></param>
        /// <param name="topCount"></param>
        /// <returns></returns>
        public IEnumerable<SimilarCompareBikeEntity> GetSimilarCompareBikes(string versionList, uint topCount, uint cityid)
        {
            List<SimilarCompareBikeEntity> similarBikeList = null;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    cmd.CommandText = "getsimilarcomparebikeslist_13102016";
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_bikeversionid", DbType.String, 20, versionList));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_topcount", DbType.Int16, topCount));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_cityid", DbType.Int16, cityid));
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
                                    Make1 = GetString(reader["Make1"]),
                                    MakeMasking1 = GetString(reader["MakeMaskingName1"]),
                                    Make2 = GetString(reader["Make2"]),
                                    MakeMasking2 = GetString(reader["MakeMaskingName2"]),
                                    Model1 = GetString(reader["Model1"]),
                                    ModelMasking1 = GetString(reader["ModelMaskingName1"]),
                                    Model2 = GetString(reader["Model2"]),
                                    ModelMasking2 = GetString(reader["ModelMaskingName2"]),
                                    VersionId1 = GetString(reader["VersionId1"]),
                                    VersionId2 = GetString(reader["VersionId2"]),
                                    ModelMaskingName1 = GetString(reader["ModelMaskingName1"]),
                                    ModelMaskingName2 = GetString(reader["ModelMaskingName2"]),
                                    OriginalImagePath1 = GetString(reader["OriginalImagePath1"]),
                                    OriginalImagePath2 = GetString(reader["OriginalImagePath2"]),
                                    Price1 = GetInt32(reader["Price1"]),
                                    Price2 = GetInt32(reader["Price2"]),
                                    HostUrl1 = GetString(reader["HostUrl1"]),
                                    HostUrl2 = GetString(reader["HostUrl2"])
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

            return similarBikeList;
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
