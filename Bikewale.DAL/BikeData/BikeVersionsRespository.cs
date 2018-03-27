using Bikewale.Entities.BikeData;
using Bikewale.Entities.GenericBikes;
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
                ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"]);

            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn(ex.Message + ex.Source);
                ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"]);

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

        /// <summary>
        /// Modified by : Ashutosh Sharma on 30 Aug 2017 
        /// Description : Changed SP from 'getversions_23082017' to 'getversions_30082017'
        /// Modified by : Ashutosh Sharma on 29 Sep 2017 
        /// Description : Changed SP from 'getversions_30082017' to 'getversions_29092017'
        /// Modified by : Ashutosh Sharma on 29 Sep 2017 
        /// Description : Changed SP from 'getversions_30082017' to 'getversions_29092017', to get avg price.
        /// </summary>
        /// <param name="modelId"></param>
        /// <param name="isNew"></param>
        /// <returns></returns>
        public List<BikeVersionMinSpecs> GetVersionMinSpecs(uint modelId, bool isNew)
        {
            List<BikeVersionMinSpecs> objMinSpecs = new List<BikeVersionMinSpecs>();
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "getversions_29092017";
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
                                    AverageExShowroom = Convert.ToUInt32(dr["AverageExShowroom"]),
                                    BrakeType = dr["BrakeType"].ToString(),
                                    AlloyWheels = Convert.ToBoolean(dr["AlloyWheels"]),
                                    ElectricStart = Convert.ToBoolean(dr["ElectricStart"]),
                                    AntilockBrakingSystem = Convert.ToBoolean(dr["AntilockBrakingSystem"]),
                                    BodyStyle = (EnumBikeBodyStyles)Enum.Parse(typeof(EnumBikeBodyStyles), Convert.ToString(dr["BodyStyleId"]))
                                });
                            }
                            dr.Close();
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"]);

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
                ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"]);

            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("GetModelDetails ex : " + ex.Message + ex.Source);
                ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"]);

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

                    int rowsAffected = MySqlDatabase.ExecuteNonQuery(cmd, ConnectionType.ReadOnly);

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
                        objSpecs.FuelTankCapacity = Convert.ToSingle(paramColl["par_fueltankcapacity"].Value);
                        objSpecs.ReserveFuelCapacity = Convert.ToSingle(paramColl["par_reservefuelcapacity"].Value);
                        objSpecs.FuelEfficiencyOverall = Convert.ToUInt16(paramColl["par_fuelefficiencyoverall"].Value);
                        objSpecs.FuelEfficiencyRange = Convert.ToUInt16(paramColl["par_fuelefficiencyrange"].Value);
                        objSpecs.ChassisType = paramColl["par_chassistype"].Value.ToString();
                        objSpecs.FrontSuspension = paramColl["par_frontsuspension"].Value.ToString();
                        objSpecs.RearSuspension = paramColl["par_rearsuspension"].Value.ToString();
                        objSpecs.BrakeType = paramColl["par_braketype"].Value.ToString();
                        objSpecs.FrontDisc = paramColl["par_frontdisc"].Value != DBNull.Value ? SqlReaderConvertor.ToNullableBool(paramColl["par_frontdisc"].Value) : null;
                        objSpecs.FrontDisc_DrumSize = Convert.ToUInt16(paramColl["par_frontdisc_drumsize"].Value);
                        objSpecs.RearDisc = paramColl["par_reardisc"].Value != DBNull.Value ? SqlReaderConvertor.ToNullableBool(paramColl["par_reardisc"].Value) : null;
                        objSpecs.RearDisc_DrumSize = Convert.ToUInt16(paramColl["par_reardisc_drumsize"].Value);
                        objSpecs.CalliperType = paramColl["par_callipertype"].Value.ToString();
                        objSpecs.WheelSize = Convert.ToSingle(paramColl["par_wheelsize"].Value);
                        objSpecs.FrontTyre = paramColl["par_fronttyre"].Value.ToString();
                        objSpecs.RearTyre = paramColl["par_reartyre"].Value.ToString();
                        objSpecs.TubelessTyres = paramColl["par_tubelesstyres"].Value != DBNull.Value ? SqlReaderConvertor.ToNullableBool(paramColl["par_tubelesstyres"].Value) : null;
                        objSpecs.RadialTyres = paramColl["par_radialtyres"].Value != DBNull.Value ? SqlReaderConvertor.ToNullableBool(paramColl["par_radialtyres"].Value) : null;
                        objSpecs.AlloyWheels = paramColl["par_alloywheels"].Value != DBNull.Value ? SqlReaderConvertor.ToNullableBool(paramColl["par_alloywheels"].Value) : null;
                        objSpecs.ElectricSystem = paramColl["par_electricsystem"].Value.ToString();
                        objSpecs.Battery = paramColl["par_battery"].Value.ToString();
                        objSpecs.HeadlightType = paramColl["par_headlighttype"].Value.ToString();
                        objSpecs.HeadlightBulbType = paramColl["par_headlightbulbtype"].Value.ToString();
                        objSpecs.Brake_Tail_Light = paramColl["par_brake_tail_light"].Value.ToString();
                        objSpecs.TurnSignal = paramColl["par_turnsignal"].Value.ToString();
                        objSpecs.PassLight = paramColl["par_passlight"].Value != DBNull.Value ? SqlReaderConvertor.ToNullableBool(paramColl["par_passlight"].Value) : null;
                        objSpecs.Speedometer = paramColl["par_speedometer"].Value.ToString();
                        objSpecs.Tachometer = paramColl["par_tachometer"].Value != DBNull.Value ? SqlReaderConvertor.ToNullableBool(paramColl["par_tachometer"].Value) : null;
                        objSpecs.TachometerType = paramColl["par_tachometertype"].Value.ToString();
                        objSpecs.ShiftLight = paramColl["par_shiftlight"].Value != DBNull.Value ? SqlReaderConvertor.ToNullableBool(paramColl["par_shiftlight"].Value) : null;
                        objSpecs.ElectricStart = paramColl["par_electricstart"].Value != DBNull.Value ? SqlReaderConvertor.ToNullableBool(paramColl["par_electricstart"].Value) : null;
                        objSpecs.Tripmeter = paramColl["par_tripmeter"].Value != DBNull.Value ? SqlReaderConvertor.ToNullableBool(paramColl["par_tripmeter"].Value) : null;
                        objSpecs.NoOfTripmeters = paramColl["par_nooftripmeters"].Value.ToString();
                        objSpecs.TripmeterType = paramColl["par_tripmetertype"].Value.ToString();
                        objSpecs.LowFuelIndicator = paramColl["par_lowfuelindicator"].Value != DBNull.Value ? SqlReaderConvertor.ToNullableBool(paramColl["par_lowfuelindicator"].Value) : null;
                        objSpecs.LowOilIndicator = paramColl["par_lowoilindicator"].Value != DBNull.Value ? SqlReaderConvertor.ToNullableBool(paramColl["par_lowoilindicator"].Value) : null;
                        objSpecs.LowBatteryIndicator = paramColl["par_lowbatteryindicator"].Value != DBNull.Value ? SqlReaderConvertor.ToNullableBool(paramColl["par_lowbatteryindicator"].Value) : null;
                        objSpecs.FuelGauge = paramColl["par_fuelgauge"].Value != DBNull.Value ? SqlReaderConvertor.ToNullableBool(paramColl["par_fuelgauge"].Value) : null;
                        objSpecs.DigitalFuelGauge = paramColl["par_digitalfuelgauge"].Value != DBNull.Value ? SqlReaderConvertor.ToNullableBool(paramColl["par_digitalfuelgauge"].Value) : null;
                        objSpecs.PillionSeat = paramColl["par_pillionseat"].Value != DBNull.Value ? SqlReaderConvertor.ToNullableBool(paramColl["par_pillionseat"].Value) : null;
                        objSpecs.PillionFootrest = paramColl["par_pillionfootrest"].Value != DBNull.Value ? SqlReaderConvertor.ToNullableBool(paramColl["par_pillionfootrest"].Value) : null;
                        objSpecs.PillionBackrest = paramColl["par_pillionbackrest"].Value != DBNull.Value ? SqlReaderConvertor.ToNullableBool(paramColl["par_pillionbackrest"].Value) : null;
                        objSpecs.PillionGrabrail = paramColl["par_PillionGrabrail"].Value != DBNull.Value ? SqlReaderConvertor.ToNullableBool(paramColl["par_PillionGrabrail"].Value) : null;
                        objSpecs.StandAlarm = paramColl["par_standalarm"].Value != DBNull.Value ? SqlReaderConvertor.ToNullableBool(paramColl["par_standalarm"].Value) : null;
                        objSpecs.SteppedSeat = paramColl["par_SteppedSeat"].Value != DBNull.Value ? SqlReaderConvertor.ToNullableBool(paramColl["par_SteppedSeat"].Value) : null;
                        objSpecs.AntilockBrakingSystem = paramColl["par_antilockbrakingsystem"].Value != DBNull.Value ? SqlReaderConvertor.ToNullableBool(paramColl["par_antilockbrakingsystem"].Value) : null;
                        objSpecs.Killswitch = paramColl["par_killswitch"].Value != DBNull.Value ? SqlReaderConvertor.ToNullableBool(paramColl["par_killswitch"].Value) : null;
                        objSpecs.Clock = paramColl["par_clock"].Value != DBNull.Value ? SqlReaderConvertor.ToNullableBool(paramColl["par_clock"].Value) : null;
                        objSpecs.MaxPowerRPM = Convert.ToSingle(paramColl["par_maxpowerrpm"].Value);
                        objSpecs.MaximumTorqueRPM = Convert.ToSingle(paramColl["par_maximumtorquerpm"].Value);
                        objSpecs.Colors = paramColl["par_colors"].Value.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"]);

            }

            return objSpecs;
        }

        /// <summary>
        /// Created By : Sadhana Upadhyay on 5th Aug 2014
        /// Summary : To get list of similar bikes by version id
        /// modified by:- Subodh Jain
        /// Summary :- To get list of similar bikes by version id and cityid
        /// Modified by : Ashutosh Sharma on 03 Oct 2017 
        /// Description : Changed SP from 'getsimilarbikeslist_13102016' to 'getsimilarbikeslist_02102017', to get avg price.
        /// Modified by : Pratibha Verma on 27 Mar 2018
        /// Description : Removed MinSpecs code
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
                    cmd.CommandText = "getsimilarbikeslist_02102017";
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
                                objBike.AvgExShowroomPrice = SqlReaderConvertor.ToUInt32(dr["AvgPrice"]);
                                objBike.OriginalImagePath = dr["originalimagepath"].ToString();
                                objBike.ReviewCount = Convert.ToUInt16(dr["reviewcount"]);
                                objBike.ReviewRate = Convert.ToDouble(dr["reviewrate"]);
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
                ErrorClass.LogError(ex, "GetSimilarBikesListByCity");

            }

            return objSimilarBikes;
        }   // End of GetSimilarBikesList

        /// <Summary>
        /// Modified by : Pratibha Verma on 27 Mar 2018
        /// Description : Removed MinSpecs code
        public IEnumerable<SimilarBikeEntity> GetSimilarBikesByModel(U modelId, uint topCount, uint cityid)
        {
            IList<SimilarBikeEntity> objSimilarBikes = null;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    cmd.CommandText = "getsimilarbikelistbymodelid";
                    cmd.CommandType = CommandType.StoredProcedure;


                    cmd.Parameters.Add(DbFactory.GetDbParam("par_modelid", DbType.Int32, modelId));
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
                                objBike.AvgExShowroomPrice = SqlReaderConvertor.ToUInt32(dr["AvgPrice"]);
                                objBike.OriginalImagePath = dr["originalimagepath"].ToString();
                                objBike.ReviewCount = Convert.ToUInt16(dr["reviewcount"]);
                                objBike.ReviewRate = Convert.ToDouble(dr["reviewrate"]);
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
                ErrorClass.LogError(ex, "GetSimilarBikesListByCity");

            }

            return objSimilarBikes;
        }


        /// <Summary>
        /// Modified by : Pratibha Verma on 27 Mar 2018
        /// Description : Removed MinSpecs code
        public IEnumerable<SimilarBikeEntity> GetSimilarBudgetBikes(U modelId, uint topCount, uint cityid)
        {
            IList<SimilarBikeEntity> objSimilarBikes = null;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    cmd.CommandText = "getsimilarbudgetbikes";
                    cmd.CommandType = CommandType.StoredProcedure;


                    cmd.Parameters.Add(DbFactory.GetDbParam("par_modelid", DbType.Int32, modelId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_topcount", DbType.Int32, topCount));

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
                                objBike.VersionPrice = SqlReaderConvertor.ToInt32(dr["versionprice"]);
                                objBike.OriginalImagePath = dr["originalimagepath"].ToString();
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
                ErrorClass.LogError(ex, "GetSimilarBikesListByCity");

            }

            return objSimilarBikes;
        }
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
                ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"]);

            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"]);

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
                ErrorClass.LogError(ex, String.Format("BikeVersionsRepository.GetColorsbyVersionId: {0}", versionId));

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
                ErrorClass.LogError(ex, "BikeVersionsRepository.GetModelVersions: {0}");
            }
            return objBikeVersions;
        }

        /// <summary>
        /// Gets the dealer versions by model.
        /// </summary>
        /// <param name="dealerId">The dealer identifier.</param>
        /// <param name="modelId">The model identifier.</param>
        /// <returns></returns>
        public IEnumerable<BikeVersionWithMinSpec> GetDealerVersionsByModel(uint dealerId, uint modelId)
        {
            ICollection<BikeVersionWithMinSpec> objBikeVersions = null;

            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("getdealerversionsbymodel"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_dealerid", DbType.UInt32, dealerId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_modelid", DbType.UInt32, modelId));

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null)
                        {
                            objBikeVersions = new List<BikeVersionWithMinSpec>();
                            while (dr.Read())
                            {
                                BikeVersionWithMinSpec objVersion = new BikeVersionWithMinSpec();
                                objVersion.VersionId = SqlReaderConvertor.ToUInt32(dr["bikeversionid"]);
                                objVersion.VersionName = Convert.ToString(dr["versionname"]);
                                objVersion.OnRoadPrice = SqlReaderConvertor.ToInt64(dr["onroadprice"]);
                                objVersion.BrakingSystem = SqlReaderConvertor.ToBoolean(dr["antilockbrakingsystem"]) ? "ABS" : string.Empty;
                                objVersion.BrakeType = string.Format("{0} Brake", Convert.ToString(dr["braketype"]));
                                objVersion.WheelType = SqlReaderConvertor.ToBoolean(dr["alloywheels"]) ? "Alloy Wheels" : string.Empty;
                                objVersion.StartType = SqlReaderConvertor.ToBoolean(dr["electricstart"]) ? "Electric Start" : "Kick Start";
                                objBikeVersions.Add(objVersion);
                            }
                            dr.Close();
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "BikeVersionsRepository.GetModelVersions: {0}");
            }
            return objBikeVersions;
        }

    }   // class
}   // namespace
