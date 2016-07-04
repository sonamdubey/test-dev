﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using Bikewale.Entities.BikeData;
using Bikewale.CoreDAL;
using Bikewale.Interfaces.BikeData;
using Bikewale.Notifications;
using System.Diagnostics;
using Bikewale.Utility;
using System.Data.Common;

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


                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd))
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

        public List<BikeVersionMinSpecs> GetVersionMinSpecs(uint modelId, bool isNew)
        {
            List<BikeVersionMinSpecs> objMinSpecs = new List<BikeVersionMinSpecs>();
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "getversions";

                    //cmd.Parameters.Add("@ModelId", SqlDbType.Int).Value = modelId;
                    //cmd.Parameters.Add("@New", SqlDbType.Bit).Value = isNew;

                    cmd.Parameters.Add(DbFactory.GetDbParam("par_modelid", DbType.Int32, modelId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_modelid", DbType.Boolean, isNew));

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd))
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

                        // LogLiveSps.LogSpInGrayLog(cmd);


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


                    MySqlDatabase.ExecuteNonQuery(cmd);

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
        /// </summary>
        /// <param name="versionId">Only positive numbers are allowed.</param>
        /// <returns>Returns object containing all specifications of the given version.</returns>
        public BikeSpecificationEntity GetSpecifications(U versionId)
        {
            BikeSpecificationEntity objSpecs = null;

            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("getnewbikesspecification_sp_new"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    // cmd.CommandText = "getnewbikesspecification_sp_new";

                    DbParameterCollection paramColl = cmd.Parameters;
                    paramColl.Add(DbFactory.GetDbParam("par_bikeversionid", DbType.Int16, versionId));
                    paramColl.Add(DbFactory.GetDbParam("par_displacement", DbType.Double, ParameterDirection.Output));
                    paramColl.Add(DbFactory.GetDbParam("par_cylinders", DbType.Int16, ParameterDirection.Output));
                    paramColl.Add(DbFactory.GetDbParam("par_maxpower", DbType.Double, ParameterDirection.Output));
                    paramColl.Add(DbFactory.GetDbParam("par_maximumtorque", DbType.Double, ParameterDirection.Output));
                    paramColl.Add(DbFactory.GetDbParam("par_bore", DbType.Double, ParameterDirection.Output));
                    paramColl.Add(DbFactory.GetDbParam("par_stroke", DbType.Double, ParameterDirection.Output));
                    paramColl.Add(DbFactory.GetDbParam("par_valvespercylinder", DbType.Int16, ParameterDirection.Output));
                    paramColl.Add(DbFactory.GetDbParam("par_fueldeliverysystem", DbType.String, 50, ParameterDirection.Output));
                    paramColl.Add(DbFactory.GetDbParam("par_fueltype", DbType.String, 50, ParameterDirection.Output));
                    paramColl.Add(DbFactory.GetDbParam("par_ignition", DbType.String, 50, ParameterDirection.Output));
                    paramColl.Add(DbFactory.GetDbParam("par_sparkplugspercylinder", DbType.String, 50, ParameterDirection.Output));
                    paramColl.Add(DbFactory.GetDbParam("par_coolingsystem", DbType.String, 50, ParameterDirection.Output));
                    paramColl.Add(DbFactory.GetDbParam("par_gearboxtype", DbType.String, 50, ParameterDirection.Output));
                    paramColl.Add(DbFactory.GetDbParam("par_noofgears", DbType.Int16, ParameterDirection.Output));
                    paramColl.Add(DbFactory.GetDbParam("par_transmissiontype", DbType.String, 50, ParameterDirection.Output));
                    paramColl.Add(DbFactory.GetDbParam("par_clutch", DbType.String, 50, ParameterDirection.Output));
                    paramColl.Add(DbFactory.GetDbParam("par_performance_0_60_kmph", DbType.Double, ParameterDirection.Output));
                    paramColl.Add(DbFactory.GetDbParam("par_performance_0_80_kmph", DbType.Double, ParameterDirection.Output));
                    paramColl.Add(DbFactory.GetDbParam("par_performance_0_40_m", DbType.Double, ParameterDirection.Output));
                    //changed topspeed data type from small int to Float
                    //Modified By : Sushil Kumar on 15-07-2015
                    paramColl.Add(DbFactory.GetDbParam("par_topspeed", DbType.Double, ParameterDirection.Output));
                    paramColl.Add(DbFactory.GetDbParam("par_performance_60_0_kmph", DbType.String, 50, ParameterDirection.Output));
                    paramColl.Add(DbFactory.GetDbParam("par_performance_80_0_kmph", DbType.String, 50, ParameterDirection.Output));
                    paramColl.Add(DbFactory.GetDbParam("par_kerbweight", DbType.Int16, ParameterDirection.Output));
                    paramColl.Add(DbFactory.GetDbParam("par_overalllength", DbType.Int16, ParameterDirection.Output));
                    paramColl.Add(DbFactory.GetDbParam("par_overallwidth", DbType.Int16, ParameterDirection.Output));
                    paramColl.Add(DbFactory.GetDbParam("par_overallheight", DbType.Int16, ParameterDirection.Output));
                    paramColl.Add(DbFactory.GetDbParam("par_wheelbase", DbType.Int16, ParameterDirection.Output));
                    paramColl.Add(DbFactory.GetDbParam("par_groundclearance", DbType.Int16, ParameterDirection.Output));
                    paramColl.Add(DbFactory.GetDbParam("par_seatheight", DbType.Int16, ParameterDirection.Output));
                    paramColl.Add(DbFactory.GetDbParam("par_fueltankcapacity", DbType.Double, ParameterDirection.Output));
                    paramColl.Add(DbFactory.GetDbParam("par_reservefuelcapacity", DbType.Double, ParameterDirection.Output));
                    paramColl.Add(DbFactory.GetDbParam("par_fuelefficiencyoverall", DbType.Int16, ParameterDirection.Output));
                    paramColl.Add(DbFactory.GetDbParam("par_fuelefficiencyrange", DbType.Int16, ParameterDirection.Output));
                    paramColl.Add(DbFactory.GetDbParam("par_chassistype", DbType.String, 50, ParameterDirection.Output));
                    paramColl.Add(DbFactory.GetDbParam("par_frontsuspension", DbType.String, 50, ParameterDirection.Output));
                    paramColl.Add(DbFactory.GetDbParam("par_rearsuspension", DbType.String, 50, ParameterDirection.Output));
                    paramColl.Add(DbFactory.GetDbParam("par_braketype", DbType.String, 50, ParameterDirection.Output));
                    paramColl.Add(DbFactory.GetDbParam("par_frontdisc", DbType.Boolean, ParameterDirection.Output));
                    paramColl.Add(DbFactory.GetDbParam("par_frontdisc_drumsize", DbType.Int16, ParameterDirection.Output));
                    paramColl.Add(DbFactory.GetDbParam("par_reardisc", DbType.Boolean, ParameterDirection.Output));
                    paramColl.Add(DbFactory.GetDbParam("par_reardisc_drumsize", DbType.Int16, ParameterDirection.Output));
                    paramColl.Add(DbFactory.GetDbParam("par_callipertype", DbType.String, 50, ParameterDirection.Output));
                    paramColl.Add(DbFactory.GetDbParam("par_wheelsize", DbType.Double, ParameterDirection.Output));
                    paramColl.Add(DbFactory.GetDbParam("par_fronttyre", DbType.String, 50, ParameterDirection.Output));
                    paramColl.Add(DbFactory.GetDbParam("par_reartyre", DbType.String, 50, ParameterDirection.Output));
                    paramColl.Add(DbFactory.GetDbParam("par_tubelesstyres", DbType.Boolean, ParameterDirection.Output));
                    paramColl.Add(DbFactory.GetDbParam("par_radialtyres", DbType.Boolean, ParameterDirection.Output));
                    paramColl.Add(DbFactory.GetDbParam("par_alloywheels", DbType.Boolean, ParameterDirection.Output));
                    paramColl.Add(DbFactory.GetDbParam("par_electricsystem", DbType.String, 50, ParameterDirection.Output));
                    paramColl.Add(DbFactory.GetDbParam("par_battery", DbType.String, 50, ParameterDirection.Output));
                    paramColl.Add(DbFactory.GetDbParam("par_headlighttype", DbType.String, 50, ParameterDirection.Output));
                    paramColl.Add(DbFactory.GetDbParam("par_headlightbulbtype", DbType.String, 50, ParameterDirection.Output));
                    paramColl.Add(DbFactory.GetDbParam("par_brake_tail_light", DbType.String, 50, ParameterDirection.Output));
                    paramColl.Add(DbFactory.GetDbParam("par_turnsignal", DbType.String, 50, ParameterDirection.Output));
                    paramColl.Add(DbFactory.GetDbParam("par_passlight", DbType.Boolean, ParameterDirection.Output));
                    paramColl.Add(DbFactory.GetDbParam("par_speedometer", DbType.String, 50, ParameterDirection.Output));
                    paramColl.Add(DbFactory.GetDbParam("par_tachometer", DbType.Boolean, ParameterDirection.Output));
                    paramColl.Add(DbFactory.GetDbParam("par_tachometertype", DbType.String, 50, ParameterDirection.Output));
                    paramColl.Add(DbFactory.GetDbParam("par_shiftlight", DbType.Boolean, ParameterDirection.Output));
                    paramColl.Add(DbFactory.GetDbParam("par_electricstart", DbType.Boolean, ParameterDirection.Output));
                    paramColl.Add(DbFactory.GetDbParam("par_tripmeter", DbType.Boolean, ParameterDirection.Output));
                    paramColl.Add(DbFactory.GetDbParam("par_nooftripmeters", DbType.String, 50, ParameterDirection.Output));
                    paramColl.Add(DbFactory.GetDbParam("par_tripmetertype", DbType.String, 50, ParameterDirection.Output));
                    paramColl.Add(DbFactory.GetDbParam("par_lowfuelindicator", DbType.Boolean, ParameterDirection.Output));
                    paramColl.Add(DbFactory.GetDbParam("par_lowoilindicator", DbType.Boolean, ParameterDirection.Output));
                    paramColl.Add(DbFactory.GetDbParam("par_lowbatteryindicator", DbType.Boolean, ParameterDirection.Output));
                    paramColl.Add(DbFactory.GetDbParam("par_fuelgauge", DbType.Boolean, ParameterDirection.Output));
                    paramColl.Add(DbFactory.GetDbParam("par_digitalfuelgauge", DbType.Boolean, ParameterDirection.Output));
                    paramColl.Add(DbFactory.GetDbParam("par_pillionseat", DbType.Boolean, ParameterDirection.Output));
                    paramColl.Add(DbFactory.GetDbParam("par_pillionfootrest", DbType.Boolean, ParameterDirection.Output));
                    paramColl.Add(DbFactory.GetDbParam("par_pillionbackrest", DbType.Boolean, ParameterDirection.Output));
                    paramColl.Add(DbFactory.GetDbParam("par_pilliongrabrail", DbType.Boolean, ParameterDirection.Output));
                    paramColl.Add(DbFactory.GetDbParam("par_standalarm", DbType.Boolean, ParameterDirection.Output));
                    paramColl.Add(DbFactory.GetDbParam("par_steppedseat", DbType.Boolean, ParameterDirection.Output));
                    paramColl.Add(DbFactory.GetDbParam("par_antilockbrakingsystem", DbType.Boolean, ParameterDirection.Output));
                    paramColl.Add(DbFactory.GetDbParam("par_killswitch", DbType.Boolean, ParameterDirection.Output));
                    paramColl.Add(DbFactory.GetDbParam("par_clock", DbType.Boolean, ParameterDirection.Output));
                    paramColl.Add(DbFactory.GetDbParam("par_colors", DbType.String, 150, ParameterDirection.Output));
                    paramColl.Add(DbFactory.GetDbParam("par_maxpowerrpm", DbType.Double, ParameterDirection.Output));
                    paramColl.Add(DbFactory.GetDbParam("par_maximumtorquerpm", DbType.Double, ParameterDirection.Output));

                    paramColl.Add(DbFactory.GetDbParam("par_rowcount", DbType.Byte, ParameterDirection.Output));

                        // LogLiveSps.LogSpInGrayLog(cmd);

                    int rowsAffected = MySqlDatabase.ExecuteNonQuery(cmd);

                    int rowCount = Convert.ToInt16(paramColl["par_rowcount"].Value);

                    if (rowCount > 0)
                    {
                        objSpecs = new BikeSpecificationEntity();
                        objSpecs.BikeVersionId = Convert.ToUInt32(versionId);
                        objSpecs.Displacement = Convert.ToSingle(paramColl["par_displacement"].Value);
                        objSpecs.Cylinders = Convert.ToUInt16(paramColl["par_cylinders"].Value);
                        objSpecs.MaxPower = Convert.ToSingle(paramColl["par_maxpower"].Value);
                        objSpecs.MaximumTorque = Convert.ToSingle(paramColl["par_maximumtorque"].Value);
                        objSpecs.Bore = Convert.ToSingle(paramColl["par_bore"].Value);
                        objSpecs.Stroke = Convert.ToSingle(paramColl["par_stroke"].Value);
                        objSpecs.ValvesPerCylinder = Convert.ToUInt16(paramColl["par_valvespercylinder"].Value);
                        objSpecs.FuelDeliverySystem = paramColl["par_fueldeliverysystem"].Value.ToString();
                        objSpecs.FuelType = paramColl["par_fueltype"].Value.ToString();
                        objSpecs.Ignition = paramColl["par_ignition"].Value.ToString();
                        objSpecs.SparkPlugsPerCylinder = paramColl["par_sparkplugspercylinder"].Value.ToString();
                        objSpecs.CoolingSystem = paramColl["par_coolingsystem"].Value.ToString();
                        objSpecs.GearboxType = paramColl["par_gearboxtype"].Value.ToString();
                        objSpecs.NoOfGears = Convert.ToUInt16(paramColl["par_noofgears"].Value.ToString());
                        objSpecs.TransmissionType = paramColl["par_transmissiontype"].Value.ToString();
                        objSpecs.Clutch = paramColl["par_clutch"].Value.ToString();
                        objSpecs.Performance_0_60_kmph = Convert.ToSingle(paramColl["par_performance_0_60_kmph"].Value);
                        objSpecs.Performance_0_80_kmph = Convert.ToSingle(paramColl["par_performance_0_80_kmph"].Value);
                        objSpecs.Performance_0_40_m = Convert.ToSingle(paramColl["par_performance_0_40_m"].Value);
                        objSpecs.TopSpeed = Convert.ToUInt16(paramColl["par_topspeed"].Value);
                        objSpecs.Performance_60_0_kmph = paramColl["par_performance_60_0_kmph"].Value.ToString();
                        objSpecs.Performance_80_0_kmph = paramColl["par_performance_80_0_kmph"].Value.ToString();
                        objSpecs.KerbWeight = Convert.ToUInt16(paramColl["par_kerbweight"].Value);
                        objSpecs.OverallLength = Convert.ToUInt16(paramColl["par_overalllength"].Value);
                        objSpecs.OverallWidth = Convert.ToUInt16(paramColl["par_overallwidth"].Value);
                        objSpecs.OverallHeight = Convert.ToUInt16(paramColl["par_overallheight"].Value);
                        objSpecs.Wheelbase = Convert.ToUInt16(paramColl["par_wheelbase"].Value);
                        objSpecs.GroundClearance = Convert.ToUInt16(paramColl["par_groundclearance"].Value);
                        objSpecs.SeatHeight = Convert.ToUInt16(paramColl["par_seatheight"].Value);
                        objSpecs.FuelTankCapacity = Convert.ToUInt16(paramColl["par_fueltankcapacity"].Value);
                        objSpecs.ReserveFuelCapacity = Convert.ToSingle(paramColl["par_reservefuelcapacity"].Value);
                        objSpecs.FuelEfficiencyOverall = Convert.ToUInt16(paramColl["par_fuelefficiencyoverall"].Value);
                        objSpecs.FuelEfficiencyRange = Convert.ToUInt16(paramColl["par_fuelefficiencyrange"].Value);
                        objSpecs.ChassisType = paramColl["par_chassistype"].Value.ToString();
                        objSpecs.FrontSuspension = paramColl["par_frontsuspension"].Value.ToString();
                        objSpecs.RearSuspension = paramColl["par_rearsuspension"].Value.ToString();
                        objSpecs.BrakeType = paramColl["par_braketype"].Value.ToString();
                        objSpecs.FrontDisc = Convert.ToBoolean(paramColl["par_frontdisc"].Value);
                        objSpecs.FrontDisc_DrumSize = Convert.ToUInt16(paramColl["par_frontdisc_drumsize"].Value);
                        objSpecs.RearDisc = Convert.ToBoolean(paramColl["par_reardisc"].Value);
                        objSpecs.RearDisc_DrumSize = Convert.ToUInt16(paramColl["par_reardisc_drumsize"].Value);
                        objSpecs.CalliperType = paramColl["par_callipertype"].Value.ToString();
                        objSpecs.WheelSize = Convert.ToSingle(paramColl["par_wheelsize"].Value);
                        objSpecs.FrontTyre = paramColl["par_fronttyre"].Value.ToString();
                        objSpecs.RearTyre = paramColl["par_reartyre"].Value.ToString();
                        objSpecs.TubelessTyres = Convert.ToBoolean(paramColl["par_tubelesstyres"].Value);
                        objSpecs.RadialTyres = Convert.ToBoolean(paramColl["par_radialtyres"].Value);
                        objSpecs.AlloyWheels = Convert.ToBoolean(paramColl["par_alloywheels"].Value);
                        objSpecs.ElectricSystem = paramColl["par_electricsystem"].Value.ToString();
                        objSpecs.Battery = paramColl["par_battery"].Value.ToString();
                        objSpecs.HeadlightType = paramColl["par_headlighttype"].Value.ToString();
                        objSpecs.HeadlightBulbType = paramColl["par_headlightbulbtype"].Value.ToString();
                        objSpecs.Brake_Tail_Light = paramColl["par_brake_tail_light"].Value.ToString();
                        objSpecs.TurnSignal = paramColl["par_turnsignal"].Value.ToString();
                        objSpecs.PassLight = Convert.ToBoolean(paramColl["par_passlight"].Value);
                        objSpecs.Speedometer = paramColl["par_speedometer"].Value.ToString();
                        objSpecs.Tachometer = Convert.ToBoolean(paramColl["par_tachometer"].Value);
                        objSpecs.TachometerType = paramColl["par_tachometertype"].Value.ToString();
                        objSpecs.ShiftLight = Convert.ToBoolean(paramColl["par_shiftlight"].Value);
                        objSpecs.ElectricStart = Convert.ToBoolean(paramColl["par_electricstart"].Value);
                        objSpecs.Tripmeter = Convert.ToBoolean(paramColl["par_tripmeter"].Value);
                        objSpecs.NoOfTripmeters = paramColl["par_nooftripmeters"].Value.ToString();
                        objSpecs.TripmeterType = paramColl["par_tripmetertype"].Value.ToString();
                        objSpecs.LowFuelIndicator = Convert.ToBoolean(paramColl["par_lowfuelindicator"].Value);
                        objSpecs.LowOilIndicator = Convert.ToBoolean(paramColl["par_lowoilindicator"].Value);
                        objSpecs.LowBatteryIndicator = Convert.ToBoolean(paramColl["par_lowbatteryindicator"].Value);
                        objSpecs.FuelGauge = Convert.ToBoolean(paramColl["par_fuelgauge"].Value);
                        objSpecs.DigitalFuelGauge = Convert.ToBoolean(paramColl["par_digitalfuelgauge"].Value);
                        objSpecs.PillionSeat = Convert.ToBoolean(paramColl["par_pillionseat"].Value);
                        objSpecs.PillionFootrest = Convert.ToBoolean(paramColl["par_pillionfootrest"].Value);
                        objSpecs.PillionBackrest = Convert.ToBoolean(paramColl["par_pillionbackrest"].Value);
                        objSpecs.PillionGrabrail = Convert.ToBoolean(paramColl["par_PillionGrabrail"].Value);
                        objSpecs.StandAlarm = Convert.ToBoolean(paramColl["par_standalarm"].Value);
                        objSpecs.SteppedSeat = Convert.ToBoolean(paramColl["par_SteppedSeat"].Value);
                        objSpecs.AntilockBrakingSystem = Convert.ToBoolean(paramColl["par_antilockbrakingsystem"].Value);
                        objSpecs.Killswitch = Convert.ToBoolean(paramColl["par_killswitch"].Value);
                        objSpecs.Clock = Convert.ToBoolean(paramColl["par_clock"].Value);
                        objSpecs.MaxPowerRPM = Convert.ToSingle(paramColl["par_maxpowerrpm"].Value);
                        objSpecs.MaximumTorqueRPM = Convert.ToSingle(paramColl["par_maximumtorquerpm"].Value);
                        objSpecs.Colors = paramColl["par_colors"].Value.ToString();
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
        /// </summary>
        /// <param name="versionId"></param>
        /// <param name="topCount"></param>
        /// <param name="percentDeviation"></param>
        /// <returns></returns>
        public List<SimilarBikeEntity> GetSimilarBikesList(U versionId, uint topCount, uint percentDeviation)
        {
            List<SimilarBikeEntity> objSimilarBikes = null;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    cmd.CommandText = "getsimilarbikeslist";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(DbFactory.GetDbParam("v_topcount", DbType.Int32, topCount));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_bikeversionid", DbType.Int32, versionId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_percentdeviation", DbType.Int32, (percentDeviation > 0) ? percentDeviation : Convert.DBNull)); 

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd))
                    {
                        objSimilarBikes = new List<SimilarBikeEntity>();

                        if (dr!=null)
                        {
                            while (dr.Read())
                            {
                                SimilarBikeEntity objBike = new SimilarBikeEntity();

                                objBike.MakeBase.MakeId = Convert.ToInt32(dr["MakeId"]);
                                objBike.MakeBase.MakeName = dr["MakeName"].ToString();
                                objBike.MakeBase.MaskingName = dr["MakeMaskingName"].ToString();
                                objBike.ModelBase.ModelId = Convert.ToInt32(dr["ModelId"]);
                                objBike.ModelBase.ModelName = dr["ModelName"].ToString();
                                objBike.ModelBase.MaskingName = dr["ModelMaskingName"].ToString();
                                objBike.VersionBase.VersionId = Convert.ToInt32(dr["VersionId"]);
                                objBike.HostUrl = dr["HostUrl"].ToString();
                                objBike.LargePicUrl = "/bikewaleimg/models/" + dr["LargePic"].ToString();
                                objBike.SmallPicUrl = "/bikewaleimg/models/" + dr["SmallPic"].ToString();
                                objBike.MinPrice = Convert.ToInt32(dr["MinPrice"]);
                                objBike.MaxPrice = Convert.ToInt32(dr["MaxPrice"]);
                                objBike.VersionPrice = Convert.ToInt32(dr["VersionPrice"]);
                                objBike.OriginalImagePath = dr["OriginalImagePath"].ToString();
                                objBike.Displacement = SqlReaderConvertor.ToNullableFloat(dr["Displacement"]);
                                objBike.FuelEfficiencyOverall = SqlReaderConvertor.ToNullableUInt16(dr["FuelEfficiencyOverall"]);
                                objBike.MaximumTorque = SqlReaderConvertor.ToNullableFloat(dr["MaximumTorque"]);
                                objBike.MaxPower = SqlReaderConvertor.ToNullableUInt16(dr["MaxPower"]);
                                objBike.ReviewCount = Convert.ToUInt16(dr["ReviewCount"]);
                                objBike.ReviewRate = Convert.ToDouble(dr["ReviewRate"]);
                                objSimilarBikes.Add(objBike);
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

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd))
                    {
                        objColors = new List<VersionColor>();

                        if (dr!=null)
                        {
                            while (dr.Read())
                                objColors.Add(new VersionColor() { ColorName = dr["Color"].ToString(), ColorCode = dr["HexCode"].ToString(), CompanyCode = dr["CompanyCode"].ToString(), ColorId = Convert.ToUInt32(dr["ColorId"]) });

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


    }   // class
}   // namespace
