using Bikewale.Entities.BikeData;
using Bikewale.Interfaces.BikeData;
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


namespace Bikewale.DAL.BikeData
{
    /// <summary>
    /// Created By : Ashish G. Kamble
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="U"></typeparam>
    public class BikeVersionsRepository<T, U> : IBikeVersions<T, U> where T : BikeVersionEntity, new()
    {
        /// <summary>
        /// Summary : Function to get all versions basic data in list.
        /// Modified By : Sadhana Upadhyay on 25 Aug 2014
        /// Summary : Changed return type to get price
        /// </summary>
        /// <param name="requestType"></param>
        /// <param name="modelId">model id whose versions are required. ModelId should be a positive number.</param>
        /// <returns>Returns list containing objects of the versions.</returns>
        public List<BikeVersionsListEntity> GetVersionsByType(EnumBikeType requestType, int modelId, int? cityId = null)
        {
            List<BikeVersionsListEntity> objVersionsList = null;

            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("getbikeversions_new"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_requesttype", DbType.Int32, (int)requestType));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_modelid", DbType.Int32, modelId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_cityid", DbType.Int32, (cityId.HasValue && cityId.Value > 0) ? cityId : Convert.DBNull));


                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null)
                        {
                            objVersionsList = new List<BikeVersionsListEntity>();

                            while (dr.Read())
                            {
                                objVersionsList.Add(new BikeVersionsListEntity()
                                {
                                    VersionId = Convert.ToInt32(dr["VersionId"]),
                                    VersionName = dr["VersionName"].ToString(),
                                    Price = Convert.ToUInt64(dr["Price"])
                                });
                            }
                            dr.Close();
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                HttpContext.Current.Trace.Warn(ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn(ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            return objVersionsList;
        }

        public U Add(T t)
        {
            throw new NotImplementedException();
        }

        public bool Update(T t)
        {
            throw new NotImplementedException();
        }

        public bool Delete(U id)
        {
            throw new NotImplementedException();
        }

        public List<T> GetAll()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<BikeModelVersionsDetails> GetModelVersions()
        {
            throw new NotImplementedException();
        }

        public List<BikeVersionMinSpecs> GetVersionMinSpecs(uint modelId, bool isNew)
        {
            List<BikeVersionMinSpecs> objMinSpecs = new List<BikeVersionMinSpecs>();
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "getversions";
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_modelid", DbType.Int32, modelId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_new", DbType.Boolean, isNew));

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null)
                        {
                            while (dr.Read())
                            {
                                objMinSpecs.Add(new BikeVersionMinSpecs()
                                {
                                    VersionId = Convert.ToInt32(dr["ID"]),
                                    VersionName = dr["Version"].ToString(),
                                    ModelName = dr["Model"].ToString(),
                                    Price = Convert.ToUInt64(dr["VersionPrice"]),
                                    BrakeType = dr["BrakeType"].ToString(),
                                    AlloyWheels = Convert.ToBoolean(dr["AlloyWheels"]),
                                    ElectricStart = Convert.ToBoolean(dr["ElectricStart"]),
                                    AntilockBrakingSystem = Convert.ToBoolean(dr["AntilockBrakingSystem"])
                                });
                            }
                            dr.Close();
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            return objMinSpecs;
        }   // End of GetVersionsMinSpecs method


        /// <summary>
        /// Summary : Function to get all details of a particular version.
        /// Modified By :   Sumit Kate on 12 Apr 2016
        /// Summary :   Fetch the New,used and futuristic flags
        /// </summary>
        /// <param name="id">Version id should be a positive number.</param>
        /// <returns>Returns object containing details of the given version id.</returns>
        public T GetById(U id)
        {
            T t = default(T);
            try
            {
                t = new T();
                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "getversiondetails_new_12042016";

                    var paramColl = cmd.Parameters;

                    cmd.Parameters.Add(DbFactory.GetDbParam("par_versionid", DbType.Int32, id));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_makeid", DbType.Int32, ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_make", DbType.String, ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_modelid", DbType.Int32, ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_model", DbType.String, ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_version", DbType.String, ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_hosturl", DbType.String, 100, ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_largepic", DbType.String, 50, ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_smallpic", DbType.String, 50, ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_price", DbType.Int32, ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_bike", DbType.String, 100, ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_maskingname", DbType.String, 50, ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_makemaskingname", DbType.String, 50, ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_originalimagepath", DbType.String, 150, ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_new", DbType.Boolean, ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_used", DbType.Boolean, ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_futuristic", DbType.Boolean, ParameterDirection.Output));


                    MySqlDatabase.ExecuteNonQuery(cmd, ConnectionType.ReadOnly);

                    if (!string.IsNullOrEmpty(cmd.Parameters["par_makeid"].Value.ToString()))
                    {
                        t.VersionId = Convert.ToInt32(cmd.Parameters["par_versionid"].Value);
                        t.VersionName = cmd.Parameters["par_version"].Value.ToString();
                        t.ModelBase.ModelId = Convert.ToInt32(cmd.Parameters["par_modelid"].Value);
                        t.ModelBase.ModelName = cmd.Parameters["par_model"].Value.ToString();
                        t.MakeBase.MakeId = Convert.ToInt32(cmd.Parameters["par_makeid"].Value);
                        t.MakeBase.MakeName = cmd.Parameters["par_make"].Value.ToString();
                        t.BikeName = cmd.Parameters["par_bike"].Value.ToString();
                        t.HostUrl = cmd.Parameters["par_hosturl"].Value.ToString();
                        t.LargePicUrl = cmd.Parameters["par_largepic"].Value.ToString();
                        t.SmallPicUrl = cmd.Parameters["par_smallpic"].Value.ToString();
                        t.Price = Convert.ToInt64(cmd.Parameters["par_price"].Value);
                        t.ModelBase.MaskingName = cmd.Parameters["par_maskingname"].Value.ToString();
                        t.MakeBase.MaskingName = cmd.Parameters["par_makemaskingname"].Value.ToString();
                        t.OriginalImagePath = cmd.Parameters["par_originalimagepath"].Value.ToString();
                        t.New = !Convert.IsDBNull(cmd.Parameters["par_new"].Value) ? Convert.ToBoolean(cmd.Parameters["par_new"].Value) : default(bool);
                        t.Used = !Convert.IsDBNull(cmd.Parameters["par_used"].Value) ? Convert.ToBoolean(cmd.Parameters["par_used"].Value) : default(bool);
                        t.Futuristic = !Convert.IsDBNull(cmd.Parameters["par_futuristic"].Value) ? Convert.ToBoolean(cmd.Parameters["par_futuristic"].Value) : default(bool);
                    }
                }
            }
            catch (SqlException ex)
            {
                HttpContext.Current.Trace.Warn("GetModelDetails sql ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("GetModelDetails ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }

            return t;
        }

        /// <summary>
        /// Summary : Function to get all specifications for the given version id.
        /// Modified By : Sadhana on 20 Aug 2014 to get row no. retrieved.
        /// Modified by : Aditi Srivastava on 25 may 2017
        /// Summary     : Changed boolean parameters to nullable boolean
        /// </summary>
        /// <param name="versionId">Only positive numbers are allowed.</param>
        /// <returns>Returns object containing all specifications of the given version.</returns>
        public BikeSpecificationEntity GetSpecifications(U versionId)
        {
            BikeSpecificationEntity objSpecs = null;

            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("getbikespecifications"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_bikeversionid", DbType.Int16, versionId));

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null)
                        {

                            objSpecs = new BikeSpecificationEntity();

                            while (dr.Read())
                            {
                                objSpecs.BikeVersionId = SqlReaderConvertor.ToUInt32(versionId);
                                objSpecs.Displacement = SqlReaderConvertor.ToFloat(dr["displacement"]);
                                objSpecs.Cylinders = SqlReaderConvertor.ToUInt16(dr["cylinders"]);
                                objSpecs.MaxPower = SqlReaderConvertor.ToFloat(dr["maxpower"]);
                                objSpecs.MaximumTorque = SqlReaderConvertor.ToFloat(dr["maximumtorque"]);
                                objSpecs.Bore = SqlReaderConvertor.ToFloat(dr["bore"]);
                                objSpecs.Stroke = SqlReaderConvertor.ToFloat(dr["stroke"]);
                                objSpecs.ValvesPerCylinder = SqlReaderConvertor.ToUInt16(dr["valvespercylinder"]);
                                objSpecs.FuelDeliverySystem = Convert.ToString(dr["fueldeliverysystem"]);
                                objSpecs.FuelType = Convert.ToString(dr["fueltype"]);
                                objSpecs.Ignition = Convert.ToString(dr["ignition"]);
                                objSpecs.SparkPlugsPerCylinder = Convert.ToString(dr["sparkplugspercylinder"]);
                                objSpecs.CoolingSystem = Convert.ToString(dr["coolingsystem"]);
                                objSpecs.GearboxType = Convert.ToString(dr["gearboxtype"]);
                                objSpecs.NoOfGears = SqlReaderConvertor.ToUInt16(dr["noofgears"].ToString());
                                objSpecs.TransmissionType = dr["transmissiontype"].ToString();
                                objSpecs.Clutch = Convert.ToString(dr["clutch"]);
                                objSpecs.Performance_0_60_kmph = SqlReaderConvertor.ToFloat(dr["performance_0_60_kmph"]);
                                objSpecs.Performance_0_80_kmph = SqlReaderConvertor.ToFloat(dr["performance_0_80_kmph"]);
                                objSpecs.Performance_0_40_m = SqlReaderConvertor.ToFloat(dr["performance_0_40_m"]);
                                objSpecs.TopSpeed = SqlReaderConvertor.ToUInt16(dr["topspeed"]);
                                objSpecs.Performance_60_0_kmph = Convert.ToString(dr["performance_60_0_kmph"]);
                                objSpecs.Performance_80_0_kmph = Convert.ToString(dr["performance_80_0_kmph"]);
                                objSpecs.KerbWeight = SqlReaderConvertor.ToUInt16(dr["kerbweight"]);
                                objSpecs.OverallLength = SqlReaderConvertor.ToUInt16(dr["overalllength"]);
                                objSpecs.OverallWidth = SqlReaderConvertor.ToUInt16(dr["overallwidth"]);
                                objSpecs.OverallHeight = SqlReaderConvertor.ToUInt16(dr["overallheight"]);
                                objSpecs.Wheelbase = SqlReaderConvertor.ToUInt16(dr["wheelbase"]);
                                objSpecs.GroundClearance = SqlReaderConvertor.ToUInt16(dr["groundclearance"]);
                                objSpecs.SeatHeight = SqlReaderConvertor.ToUInt16(dr["seatheight"]);
                                objSpecs.FuelTankCapacity = SqlReaderConvertor.ToFloat(dr["fueltankcapacity"]);
                                objSpecs.ReserveFuelCapacity = SqlReaderConvertor.ToFloat(dr["reservefuelcapacity"]);
                                objSpecs.FuelEfficiencyOverall = SqlReaderConvertor.ToUInt16(dr["fuelefficiencyoverall"]);
                                objSpecs.FuelEfficiencyRange = SqlReaderConvertor.ToUInt16(dr["fuelefficiencyrange"]);
                                objSpecs.ChassisType = Convert.ToString(dr["chassistype"]);
                                objSpecs.FrontSuspension = Convert.ToString(dr["frontsuspension"]);
                                objSpecs.RearSuspension = Convert.ToString(dr["rearsuspension"]);
                                objSpecs.BrakeType = Convert.ToString(dr["braketype"]);
                                objSpecs.FrontDisc = !Convert.IsDBNull(dr["frontdisc"]) ? SqlReaderConvertor.ToNullableBool(dr["frontdisc"]) : null;
                                objSpecs.FrontDisc_DrumSize = SqlReaderConvertor.ToUInt16(dr["frontdisc_drumsize"]);
                                objSpecs.RearDisc = !Convert.IsDBNull(dr["reardisc"]) ? SqlReaderConvertor.ToNullableBool(dr["reardisc"]) : null;
                                objSpecs.RearDisc_DrumSize = SqlReaderConvertor.ToUInt16(dr["reardisc_drumsize"]);
                                objSpecs.CalliperType = Convert.ToString(dr["callipertype"]);
                                objSpecs.WheelSize = SqlReaderConvertor.ToFloat(dr["wheelsize"]);
                                objSpecs.FrontTyre = Convert.ToString(dr["fronttyre"]);
                                objSpecs.RearTyre = Convert.ToString(dr["reartyre"]);
                                objSpecs.TubelessTyres = !Convert.IsDBNull(dr["tubelesstyres"]) ? SqlReaderConvertor.ToNullableBool(dr["tubelesstyres"]) : null;
                                objSpecs.RadialTyres = !Convert.IsDBNull(dr["radialtyres"]) ? SqlReaderConvertor.ToNullableBool(dr["radialtyres"]) : null;
                                objSpecs.AlloyWheels = !Convert.IsDBNull(dr["alloywheels"]) ? SqlReaderConvertor.ToNullableBool(dr["alloywheels"]) : null;
                                objSpecs.ElectricSystem = Convert.ToString(dr["electricsystem"]);
                                objSpecs.Battery = Convert.ToString(dr["battery"]);
                                objSpecs.HeadlightType = Convert.ToString(dr["headlighttype"]);
                                objSpecs.HeadlightBulbType = Convert.ToString(dr["headlightbulbtype"]);
                                objSpecs.Brake_Tail_Light = Convert.ToString(dr["brake_tail_light"]);
                                objSpecs.TurnSignal = Convert.ToString(dr["turnsignal"]);
                                objSpecs.PassLight = !Convert.IsDBNull(dr["passlight"]) ? SqlReaderConvertor.ToNullableBool(dr["passlight"]) : null;
                                objSpecs.Speedometer = Convert.ToString(dr["speedometer"]);
                                objSpecs.Tachometer = !Convert.IsDBNull(dr["tachometer"]) ? SqlReaderConvertor.ToNullableBool(dr["tachometer"]) : null;
                                objSpecs.TachometerType = Convert.ToString(dr["tachometertype"]);
                                objSpecs.ShiftLight = !Convert.IsDBNull(dr["shiftlight"]) ? SqlReaderConvertor.ToNullableBool(dr["shiftlight"]) : null;
                                objSpecs.ElectricStart = !Convert.IsDBNull(dr["electricstart"]) ? SqlReaderConvertor.ToNullableBool(dr["electricstart"]) : null;
                                objSpecs.Tripmeter = !Convert.IsDBNull(dr["tripmeter"]) ? SqlReaderConvertor.ToNullableBool(dr["tripmeter"]) : null;
                                objSpecs.NoOfTripmeters = Convert.ToString(dr["nooftripmeters"]);
                                objSpecs.TripmeterType = Convert.ToString(dr["tripmetertype"]);
                                objSpecs.LowFuelIndicator = !Convert.IsDBNull(dr["lowfuelindicator"]) ? SqlReaderConvertor.ToNullableBool(dr["lowfuelindicator"]) : null;
                                objSpecs.LowOilIndicator = !Convert.IsDBNull(dr["lowoilindicator"]) ? SqlReaderConvertor.ToNullableBool(dr["lowoilindicator"]) : null;
                                objSpecs.LowBatteryIndicator = !Convert.IsDBNull(dr["lowbatteryindicator"]) ? SqlReaderConvertor.ToNullableBool(dr["lowbatteryindicator"]) : null;
                                objSpecs.FuelGauge = !Convert.IsDBNull(dr["fuelgauge"]) ? SqlReaderConvertor.ToNullableBool(dr["fuelgauge"]) : null;
                                objSpecs.DigitalFuelGauge = !Convert.IsDBNull(dr["digitalfuelgauge"]) ? SqlReaderConvertor.ToNullableBool(dr["digitalfuelgauge"]) : null;
                                objSpecs.PillionSeat = !Convert.IsDBNull(dr["pillionseat"]) ? SqlReaderConvertor.ToNullableBool(dr["pillionseat"]) : null;
                                objSpecs.PillionFootrest = !Convert.IsDBNull(dr["pillionfootrest"]) ? SqlReaderConvertor.ToNullableBool(dr["pillionfootrest"]) : null;
                                objSpecs.PillionBackrest = !Convert.IsDBNull(dr["pillionbackrest"]) ? SqlReaderConvertor.ToNullableBool(dr["pillionbackrest"]) : null;
                                objSpecs.PillionGrabrail = !Convert.IsDBNull(dr["PillionGrabrail"]) ? SqlReaderConvertor.ToNullableBool(dr["PillionGrabrail"]) : null;
                                objSpecs.StandAlarm = !Convert.IsDBNull(dr["standalarm"]) ? SqlReaderConvertor.ToNullableBool(dr["standalarm"]) : null;
                                objSpecs.SteppedSeat = !Convert.IsDBNull(dr["SteppedSeat"]) ? SqlReaderConvertor.ToNullableBool(dr["SteppedSeat"]) : null;
                                objSpecs.AntilockBrakingSystem = !Convert.IsDBNull(dr["antilockbrakingsystem"]) ? SqlReaderConvertor.ToNullableBool(dr["antilockbrakingsystem"]) : null;
                                objSpecs.Killswitch = !Convert.IsDBNull(dr["killswitch"]) ? SqlReaderConvertor.ToNullableBool(dr["killswitch"]) : null;
                                objSpecs.Clock = !Convert.IsDBNull(dr["clock"]) ? SqlReaderConvertor.ToNullableBool(dr["clock"]) : null;
                                objSpecs.MaxPowerRPM = SqlReaderConvertor.ToFloat(dr["maxpowerrpm"]);
                                objSpecs.MaximumTorqueRPM = SqlReaderConvertor.ToFloat(dr["maximumtorquerpm"]);
                                objSpecs.Colors = Convert.ToString(dr["colors"]);
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }

            return objSpecs;
        }

        /// <summary>
        /// Created By : Sadhana Upadhyay on 5th Aug 2014
        /// Summary : To get list of similar bikes by version id
        /// modified by:- Subodh Jain
        /// Summary :- To get list of similar bikes by version id and cityid
        /// </summary>
        /// <param name="versionId"></param>
        /// <param name="topCount"></param>
        /// <param name="cityid"></param>
        /// <returns></returns>
        public IEnumerable<SimilarBikeEntity> GetSimilarBikesList(U versionId, uint topCount, uint cityid)
        {
            IList<SimilarBikeEntity> objSimilarBikes = null;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    cmd.CommandText = "getsimilarbikeslist_13102016";
                    cmd.CommandType = CommandType.StoredProcedure;


                    cmd.Parameters.Add(DbFactory.GetDbParam("par_bikeversionid", DbType.Int32, versionId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_topcount", DbType.Int32, topCount));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_cityid", DbType.Int32, cityid));

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {


                        if (dr != null)
                        {
                            objSimilarBikes = new List<SimilarBikeEntity>();
                            while (dr.Read())
                            {
                                SimilarBikeEntity objBike = new SimilarBikeEntity();
                                objBike.MakeBase.MakeId = SqlReaderConvertor.ToInt32(dr["makeid"]);
                                objBike.MakeBase.MakeName = Convert.ToString(dr["makename"]);
                                objBike.MakeBase.MaskingName = Convert.ToString(dr["makemaskingname"]);
                                objBike.ModelBase.ModelId = SqlReaderConvertor.ToInt32(dr["modelid"]);
                                objBike.ModelBase.ModelName = Convert.ToString(dr["modelname"]);
                                objBike.ModelBase.MaskingName = Convert.ToString(dr["modelmaskingname"]);
                                objBike.VersionBase.VersionId = SqlReaderConvertor.ToInt32(dr["versionid"]);
                                objBike.HostUrl = Convert.ToString(dr["hosturl"]);
                                objBike.MinPrice = SqlReaderConvertor.ToInt32(dr["versionprice"]);
                                objBike.VersionPrice = SqlReaderConvertor.ToInt32(dr["versionprice"]);
                                objBike.OriginalImagePath = dr["originalimagepath"].ToString();
                                objBike.Displacement = SqlReaderConvertor.ToNullableFloat(dr["displacement"]);
                                objBike.FuelEfficiencyOverall = SqlReaderConvertor.ToNullableUInt16(dr["fuelefficiencyoverall"]);
                                objBike.MaximumTorque = SqlReaderConvertor.ToNullableFloat(dr["maximumTorque"]);
                                objBike.KerbWeight = SqlReaderConvertor.ToNullableUInt16(dr["kerbweight"]);
                                objBike.MaxPower = SqlReaderConvertor.ToNullableFloat(dr["maxpower"]);
                                objBike.ReviewCount = SqlReaderConvertor.ToUInt16(dr["reviewcount"]);
                                objBike.ReviewRate = SqlReaderConvertor.ToDouble(dr["reviewrate"]);
                                objBike.LargePicUrl = "/bikewaleimg/models/" + Convert.ToString(dr["largePic"]);
                                objBike.SmallPicUrl = "/bikewaleimg/models/" + Convert.ToString(dr["smallPic"]);
                                objBike.CityName = Convert.ToString(dr["cityname"]);
                                objBike.CityMaskingName = Convert.ToString(dr["CityMaskingName"]);
                                objSimilarBikes.Add(objBike);
                            }
                            dr.Close();
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "GetSimilarBikesListByCity");
                objErr.SendMail();
            }

            return objSimilarBikes;
        }   // End of GetSimilarBikesList


        /// <summary>
        /// Created By : Sadhana Upadhyay on 4 Dec 2014
        /// Summary : get version color by version id
        /// </summary>
        /// <param name="versionId"></param>
        /// <returns></returns>
        public List<VersionColor> GetColorByVersion(U versionId)
        {
            List<VersionColor> objColors = null;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    cmd.CommandText = "getversioncolors";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(DbFactory.GetDbParam("par_versionid", DbType.Int32, versionId));

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        objColors = new List<VersionColor>();

                        if (dr != null)
                        {
                            while (dr.Read())
                                objColors.Add(new VersionColor() { ColorName = dr["Color"].ToString(), ColorCode = dr["HexCode"].ToString(), ColorId = Convert.ToUInt32(dr["ColorId"]) });

                            dr.Close();
                        }

                    }
                }
            }
            catch (SqlException ex)
            {
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }

            return objColors;
        }
        /// <summary>
        /// Created by : Aditi Srivastava on 17 Oct 2016
        /// Description: get colors by version id for used bikes
        /// Modified by :   Sumit Kate on 22 Dec 2016
        /// Description :   Used List instead of IEnumerable for return value
        /// </summary>
        /// <returns></returns>
        public IEnumerable<BikeColorsbyVersion> GetColorsbyVersionId(uint versionId)
        {
            ICollection<BikeColorsbyVersion> objVersionColors = null;
            ICollection<VersionColor> versionColors = null;

            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("getbikecolorsbyversion"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_versionid", DbType.Int32, versionId));

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null)
                        {
                            versionColors = new List<VersionColor>();
                            while (dr.Read())
                            {
                                VersionColor objColor = new VersionColor();
                                objColor.ColorId = SqlReaderConvertor.ToUInt32(dr["colorid"]);
                                objColor.ColorName = Convert.ToString(dr["colorname"]);
                                objColor.ColorCode = Convert.ToString(dr["hexcode"]).Trim();
                                versionColors.Add(objColor);
                            }
                            dr.Close();
                        }
                    }
                }
                if (versionColors != null)
                {
                    objVersionColors = versionColors
                        .GroupBy(grp => new { grp.ColorId, grp.ColorName })
                        .Select(
                        vc => new BikeColorsbyVersion()
                        {
                            ColorId = vc.Key.ColorId,
                            ColorName = vc.Key.ColorName,
                            HexCode = vc.Select(hc => hc.ColorCode).ToList()
                        }
                        ).ToList();
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, String.Format("BikeVersionsRepository.GetColorsbyVersionId: {0}", versionId));
                objErr.SendMail();
            }
            return objVersionColors;
        }

        /// <summary>
        /// created by sajal gupta on 23-05-2017
        /// description : Dal layer function to get version segm,ent details
        /// </summary>
        /// <returns></returns>
        public IEnumerable<BikeVersionsSegment> GetModelVersionsDAL()
        {
            ICollection<BikeVersionsSegment> objBikeVersions = null;

            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("getmodelversions"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null)
                        {
                            objBikeVersions = new List<BikeVersionsSegment>();
                            while (dr.Read())
                            {
                                BikeVersionsSegment objVersion = new BikeVersionsSegment();
                                objVersion.VersionId = SqlReaderConvertor.ToUInt32(dr["versionId"]);
                                objVersion.BodyStyle = Convert.ToString(dr["bodystyle"]);
                                objVersion.ModelMaskingName = Convert.ToString(dr["ModelMaskingName"]);
                                objVersion.ModelId = SqlReaderConvertor.ToUInt32(dr["BikeModelId"]);
                                objVersion.ModelName = Convert.ToString(dr["ModelName"]);
                                objVersion.Segment = Convert.ToString(dr["bodySegment"]);
                                objVersion.VersionName = Convert.ToString(dr["versionName"]);
                                objVersion.CCSegment = Convert.ToString(dr["ClassSegmentName"]);
                                objVersion.TopVersionId = SqlReaderConvertor.ToUInt32(dr["topversionid"]);
                                objBikeVersions.Add(objVersion);
                            }
                            dr.Close();
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "BikeVersionsRepository.GetModelVersions: {0}");
            }
            return objBikeVersions;
        }

    }   // class
}   // namespace
