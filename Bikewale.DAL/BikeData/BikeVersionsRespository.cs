using System;
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

namespace Bikewale.DAL.BikeData
{
    /// <summary>
    /// Created By : Ashish G. Kamble
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="U"></typeparam>
    public class BikeVersionsRepository<T,U> : IBikeVersions<T,U> where T : BikeVersionEntity, new()
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

            Database db = null;

            try
            {                
                using (SqlCommand cmd = new SqlCommand("GetBikeVersions_New"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@RequestType", SqlDbType.TinyInt).Value = (int)requestType;
                    cmd.Parameters.Add("@ModelId", SqlDbType.Int).Value = modelId;

                    if (cityId.HasValue && cityId.Value > 0)
                    {
                        cmd.Parameters.Add("@CityId", SqlDbType.Int).Value = cityId;
                    }

                    db = new Database();

                    using (SqlDataReader dr = db.SelectQry(cmd))
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
            finally
            {
                db.CloseConnection();
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

        public List<BikeVersionMinSpecs> GetVersionMinSpecs(uint modelId,bool isNew)
        {
           Database db = null;
           List<BikeVersionMinSpecs> objMinSpecs = new List<BikeVersionMinSpecs>();
            try
            {
                db = new Database();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "GetVersions";

                    cmd.Parameters.Add("@ModelId", SqlDbType.Int).Value = modelId;                    
                    cmd.Parameters.Add("@New", SqlDbType.Bit).Value = isNew;

                    using(SqlDataReader dr = db.SelectQry(cmd))
                    {                                                                   
                        while(dr.Read())
                        {
                            objMinSpecs.Add( new BikeVersionMinSpecs(){
                                VersionId  = Convert.ToInt32(dr["ID"]),
                                   VersionName = dr["Version"].ToString(),
                                   ModelName =dr["Model"].ToString(),
                                Price = Convert.ToUInt64(dr["VersionPrice"]),
                                   BrakeType  = dr["BrakeType"].ToString(),
                                AlloyWheels  = Convert.ToBoolean(dr["AlloyWheels"]),
                                ElectricStart  = Convert.ToBoolean(dr["ElectricStart"]),
                                AntilockBrakingSystem  = Convert.ToBoolean(dr["AntilockBrakingSystem"])
                            }) ;
                        }
                        dr.Close();
                    }
                }                 
            
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            finally
            {
                db.CloseConnection();
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
            Database db = null;
            try
            {
                db = new Database();
                t = new T();

                using (SqlConnection conn = new SqlConnection(db.GetConString()))
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "GetVersionDetails_New_12042016";
                        cmd.Connection = conn;
                        SqlParameterCollection paramColl = cmd.Parameters;

                        paramColl.Add("@VersionId", SqlDbType.Int).Value = id;
                        paramColl.Add("@MakeId", SqlDbType.Int).Direction = ParameterDirection.Output;
                        paramColl.Add("@Make", SqlDbType.VarChar, 30).Direction = ParameterDirection.Output;
                        paramColl.Add("@ModelId", SqlDbType.Int).Direction = ParameterDirection.Output;
                        paramColl.Add("@Model", SqlDbType.VarChar, 30).Direction = ParameterDirection.Output;
                        paramColl.Add("@Version", SqlDbType.VarChar, 30).Direction = ParameterDirection.Output;
                        paramColl.Add("@HostUrl", SqlDbType.VarChar, 100).Direction = ParameterDirection.Output;
                        paramColl.Add("@LargePic", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
                        paramColl.Add("@SmallPic", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
                        paramColl.Add("@Price", SqlDbType.Int).Direction = ParameterDirection.Output;
                        paramColl.Add("@Bike", SqlDbType.VarChar, 100).Direction = ParameterDirection.Output;
                        paramColl.Add("@MaskingName", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
                        paramColl.Add("@MakeMaskingName", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
                        paramColl.Add("@OriginalImagePath", SqlDbType.VarChar, 150).Direction = ParameterDirection.Output;
                        paramColl.Add("@New", SqlDbType.Bit).Direction = ParameterDirection.Output;
                        paramColl.Add("@Used", SqlDbType.Bit).Direction = ParameterDirection.Output;
                        paramColl.Add("@Futuristic", SqlDbType.Bit).Direction = ParameterDirection.Output;

                        LogLiveSps.LogSpInGrayLog(cmd);
                        conn.Open();
                        cmd.ExecuteNonQuery();

                        conn.Close();
                        HttpContext.Current.Trace.Warn("qry success");
                        
                        if (!string.IsNullOrEmpty(paramColl["@MakeId"].Value.ToString()))
                        {
                            t.VersionId = Convert.ToInt32(paramColl["@VersionId"].Value);
                            t.VersionName = paramColl["@Version"].Value.ToString();
                            t.ModelBase.ModelId = Convert.ToInt32(paramColl["@ModelId"].Value);
                            t.ModelBase.ModelName = paramColl["@Model"].Value.ToString();
                            t.MakeBase.MakeId = Convert.ToInt32(paramColl["@MakeId"].Value);
                            t.MakeBase.MakeName = paramColl["@Make"].Value.ToString();
                            t.BikeName = paramColl["@Bike"].Value.ToString();
                            t.HostUrl = paramColl["@HostUrl"].Value.ToString();
                            t.LargePicUrl = paramColl["@LargePic"].Value.ToString();
                            t.SmallPicUrl = paramColl["@SmallPic"].Value.ToString();
                            t.Price = Convert.ToInt64(paramColl["@Price"].Value);
                            t.ModelBase.MaskingName = paramColl["@MaskingName"].Value.ToString();
                            t.MakeBase.MaskingName = paramColl["@MakeMaskingName"].Value.ToString();
                            t.OriginalImagePath = paramColl["@OriginalImagePath"].Value.ToString();
                            t.New = !Convert.IsDBNull(paramColl["@New"].Value) ? Convert.ToBoolean(paramColl["@New"].Value) : default(bool);
                            t.Used = !Convert.IsDBNull(paramColl["@Used"].Value) ? Convert.ToBoolean(paramColl["@Used"].Value) : default(bool);
                            t.Futuristic = !Convert.IsDBNull(paramColl["@Futuristic"].Value) ? Convert.ToBoolean(paramColl["@Futuristic"].Value) : default(bool);
                        }
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
            Database db = null; 
            SqlConnection conn = null;

            try
            {
                db = new Database();

                using (conn = new SqlConnection(db.GetConString()))
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "GetNewBikesSpecification_SP_New";
                        cmd.Connection = conn;

                        SqlParameterCollection paramColl = cmd.Parameters;

                        paramColl.Add("@BikeVersionId", SqlDbType.SmallInt).Value = versionId;
                        paramColl.Add("@Displacement", SqlDbType.Float).Direction = ParameterDirection.Output;
                        paramColl.Add("@Cylinders", SqlDbType.SmallInt).Direction = ParameterDirection.Output;
                        paramColl.Add("@MaxPower", SqlDbType.Float).Direction = ParameterDirection.Output;
                        paramColl.Add("@MaximumTorque", SqlDbType.Float).Direction = ParameterDirection.Output;
                        paramColl.Add("@Bore", SqlDbType.Float).Direction = ParameterDirection.Output;
                        paramColl.Add("@Stroke", SqlDbType.Float).Direction = ParameterDirection.Output;
                        paramColl.Add("@ValvesPerCylinder", SqlDbType.SmallInt).Direction = ParameterDirection.Output;
                        paramColl.Add("@FuelDeliverySystem", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
                        paramColl.Add("@FuelType", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
                        paramColl.Add("@Ignition", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
                        paramColl.Add("@SparkPlugsPerCylinder", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
                        paramColl.Add("@CoolingSystem", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
                        paramColl.Add("@GearboxType", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
                        paramColl.Add("@NoOfGears", SqlDbType.SmallInt).Direction = ParameterDirection.Output;
                        paramColl.Add("@TransmissionType", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
                        paramColl.Add("@Clutch", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
                        paramColl.Add("@Performance_0_60_kmph", SqlDbType.Float).Direction = ParameterDirection.Output;
                        paramColl.Add("@Performance_0_80_kmph", SqlDbType.Float).Direction = ParameterDirection.Output;
                        paramColl.Add("@Performance_0_40_m", SqlDbType.Float).Direction = ParameterDirection.Output;
                        //changed topspeed data type from small int to Float
                        //Modified By : Sushil Kumar on 15-07-2015
                        paramColl.Add("@TopSpeed", SqlDbType.Float).Direction = ParameterDirection.Output;
                        paramColl.Add("@Performance_60_0_kmph", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
                        paramColl.Add("@Performance_80_0_kmph", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
                        paramColl.Add("@KerbWeight", SqlDbType.SmallInt).Direction = ParameterDirection.Output;
                        paramColl.Add("@OverallLength", SqlDbType.SmallInt).Direction = ParameterDirection.Output;
                        paramColl.Add("@OverallWidth", SqlDbType.SmallInt).Direction = ParameterDirection.Output;
                        paramColl.Add("@OverallHeight", SqlDbType.SmallInt).Direction = ParameterDirection.Output;
                        paramColl.Add("@Wheelbase", SqlDbType.SmallInt).Direction = ParameterDirection.Output;
                        paramColl.Add("@GroundClearance", SqlDbType.SmallInt).Direction = ParameterDirection.Output;
                        paramColl.Add("@SeatHeight", SqlDbType.SmallInt).Direction = ParameterDirection.Output;
                        paramColl.Add("@FuelTankCapacity", SqlDbType.Float).Direction = ParameterDirection.Output;
                        paramColl.Add("@ReserveFuelCapacity", SqlDbType.Float).Direction = ParameterDirection.Output;
                        paramColl.Add("@FuelEfficiencyOverall", SqlDbType.SmallInt).Direction = ParameterDirection.Output;
                        paramColl.Add("@FuelEfficiencyRange", SqlDbType.SmallInt).Direction = ParameterDirection.Output;
                        paramColl.Add("@ChassisType", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
                        paramColl.Add("@FrontSuspension", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
                        paramColl.Add("@RearSuspension", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
                        paramColl.Add("@BrakeType", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
                        paramColl.Add("@FrontDisc", SqlDbType.Bit).Direction = ParameterDirection.Output;
                        paramColl.Add("@FrontDisc_DrumSize", SqlDbType.SmallInt).Direction = ParameterDirection.Output;
                        paramColl.Add("@RearDisc", SqlDbType.Bit).Direction = ParameterDirection.Output;
                        paramColl.Add("@RearDisc_DrumSize", SqlDbType.SmallInt).Direction = ParameterDirection.Output;
                        paramColl.Add("@CalliperType", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
                        paramColl.Add("@WheelSize", SqlDbType.Float).Direction = ParameterDirection.Output;
                        paramColl.Add("@FrontTyre", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
                        paramColl.Add("@RearTyre", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
                        paramColl.Add("@TubelessTyres", SqlDbType.Bit).Direction = ParameterDirection.Output;
                        paramColl.Add("@RadialTyres", SqlDbType.Bit).Direction = ParameterDirection.Output;
                        paramColl.Add("@AlloyWheels", SqlDbType.Bit).Direction = ParameterDirection.Output;
                        paramColl.Add("@ElectricSystem", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
                        paramColl.Add("@Battery", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
                        paramColl.Add("@HeadlightType", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
                        paramColl.Add("@HeadlightBulbType", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
                        paramColl.Add("@Brake_Tail_Light", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
                        paramColl.Add("@TurnSignal", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
                        paramColl.Add("@PassLight", SqlDbType.Bit).Direction = ParameterDirection.Output;
                        paramColl.Add("@Speedometer", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
                        paramColl.Add("@Tachometer", SqlDbType.Bit).Direction = ParameterDirection.Output;
                        paramColl.Add("@TachometerType", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
                        paramColl.Add("@ShiftLight", SqlDbType.Bit).Direction = ParameterDirection.Output;
                        paramColl.Add("@ElectricStart", SqlDbType.Bit).Direction = ParameterDirection.Output;
                        paramColl.Add("@Tripmeter", SqlDbType.Bit).Direction = ParameterDirection.Output;
                        paramColl.Add("@NoOfTripmeters", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
                        paramColl.Add("@TripmeterType", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
                        paramColl.Add("@LowFuelIndicator", SqlDbType.Bit).Direction = ParameterDirection.Output;
                        paramColl.Add("@LowOilIndicator", SqlDbType.Bit).Direction = ParameterDirection.Output;
                        paramColl.Add("@LowBatteryIndicator", SqlDbType.Bit).Direction = ParameterDirection.Output;
                        paramColl.Add("@FuelGauge", SqlDbType.Bit).Direction = ParameterDirection.Output;
                        paramColl.Add("@DigitalFuelGauge", SqlDbType.Bit).Direction = ParameterDirection.Output;
                        paramColl.Add("@PillionSeat", SqlDbType.Bit).Direction = ParameterDirection.Output;
                        paramColl.Add("@PillionFootrest", SqlDbType.Bit).Direction = ParameterDirection.Output;
                        paramColl.Add("@PillionBackrest", SqlDbType.Bit).Direction = ParameterDirection.Output;
                        paramColl.Add("@PillionGrabrail", SqlDbType.Bit).Direction = ParameterDirection.Output;
                        paramColl.Add("@StandAlarm", SqlDbType.Bit).Direction = ParameterDirection.Output;
                        paramColl.Add("@SteppedSeat", SqlDbType.Bit).Direction = ParameterDirection.Output;
                        paramColl.Add("@AntilockBrakingSystem", SqlDbType.Bit).Direction = ParameterDirection.Output;
                        paramColl.Add("@Killswitch", SqlDbType.Bit).Direction = ParameterDirection.Output;
                        paramColl.Add("@Clock", SqlDbType.Bit).Direction = ParameterDirection.Output;
                        paramColl.Add("@Colors", SqlDbType.VarChar, 150).Direction = ParameterDirection.Output;
                        paramColl.Add("@MaxPowerRPM", SqlDbType.Float).Direction = ParameterDirection.Output;
                        paramColl.Add("@MaximumTorqueRPM", SqlDbType.Float).Direction = ParameterDirection.Output;

                        paramColl.Add("@RowCount", SqlDbType.TinyInt).Direction = ParameterDirection.Output;

                        LogLiveSps.LogSpInGrayLog(cmd);

                        conn.Open();
                        int rowsAffected = cmd.ExecuteNonQuery();
                        conn.Close();

                        int rowCount = Convert.ToInt16(paramColl["@RowCount"].Value);

                        if (rowCount > 0)
                        {
                            objSpecs = new BikeSpecificationEntity();
                            objSpecs.BikeVersionId = Convert.ToUInt32(versionId);
                            objSpecs.Displacement = Convert.ToSingle(paramColl["@Displacement"].Value);
                            objSpecs.Cylinders = Convert.ToUInt16(paramColl["@Cylinders"].Value);
                            objSpecs.MaxPower = Convert.ToSingle(paramColl["@MaxPower"].Value);
                            objSpecs.MaximumTorque = Convert.ToSingle(paramColl["@MaximumTorque"].Value);
                            objSpecs.Bore = Convert.ToSingle(paramColl["@Bore"].Value);
                            objSpecs.Stroke = Convert.ToSingle(paramColl["@Stroke"].Value);
                            objSpecs.ValvesPerCylinder = Convert.ToUInt16(paramColl["@ValvesPerCylinder"].Value);
                            objSpecs.FuelDeliverySystem = paramColl["@FuelDeliverySystem"].Value.ToString();
                            objSpecs.FuelType = paramColl["@FuelType"].Value.ToString();
                            objSpecs.Ignition = paramColl["@Ignition"].Value.ToString();
                            objSpecs.SparkPlugsPerCylinder = paramColl["@SparkPlugsPerCylinder"].Value.ToString();
                            objSpecs.CoolingSystem = paramColl["@CoolingSystem"].Value.ToString();
                            objSpecs.GearboxType = paramColl["@GearboxType"].Value.ToString();
                            objSpecs.NoOfGears = Convert.ToUInt16(paramColl["@NoOfGears"].Value.ToString());
                            objSpecs.TransmissionType = paramColl["@TransmissionType"].Value.ToString();
                            objSpecs.Clutch = paramColl["@Clutch"].Value.ToString();
                            objSpecs.Performance_0_60_kmph = Convert.ToSingle(paramColl["@Performance_0_60_kmph"].Value);
                            objSpecs.Performance_0_80_kmph = Convert.ToSingle(paramColl["@Performance_0_80_kmph"].Value);
                            objSpecs.Performance_0_40_m = Convert.ToSingle(paramColl["@Performance_0_40_m"].Value);
                            objSpecs.TopSpeed = Convert.ToUInt16(paramColl["@TopSpeed"].Value);
                            objSpecs.Performance_60_0_kmph = paramColl["@Performance_60_0_kmph"].Value.ToString();
                            objSpecs.Performance_80_0_kmph = paramColl["@Performance_80_0_kmph"].Value.ToString();
                            objSpecs.KerbWeight = Convert.ToUInt16(paramColl["@KerbWeight"].Value);
                            objSpecs.OverallLength = Convert.ToUInt16(paramColl["@OverallLength"].Value);
                            objSpecs.OverallWidth = Convert.ToUInt16(paramColl["@OverallWidth"].Value);
                            objSpecs.OverallHeight = Convert.ToUInt16(paramColl["@OverallHeight"].Value);
                            objSpecs.Wheelbase = Convert.ToUInt16(paramColl["@Wheelbase"].Value);
                            objSpecs.GroundClearance = Convert.ToUInt16(paramColl["@GroundClearance"].Value);
                            objSpecs.SeatHeight = Convert.ToUInt16(paramColl["@SeatHeight"].Value);
                            objSpecs.FuelTankCapacity = Convert.ToUInt16(paramColl["@FuelTankCapacity"].Value);
                            objSpecs.ReserveFuelCapacity = Convert.ToSingle(paramColl["@ReserveFuelCapacity"].Value);
                            objSpecs.FuelEfficiencyOverall = Convert.ToUInt16(paramColl["@FuelEfficiencyOverall"].Value);
                            objSpecs.FuelEfficiencyRange = Convert.ToUInt16(paramColl["@FuelEfficiencyRange"].Value);
                            objSpecs.ChassisType = paramColl["@ChassisType"].Value.ToString();
                            objSpecs.FrontSuspension = paramColl["@FrontSuspension"].Value.ToString();
                            objSpecs.RearSuspension = paramColl["@RearSuspension"].Value.ToString();
                            objSpecs.BrakeType = paramColl["@BrakeType"].Value.ToString();
                            objSpecs.FrontDisc = Convert.ToBoolean(paramColl["@FrontDisc"].Value);
                            objSpecs.FrontDisc_DrumSize = Convert.ToUInt16(paramColl["@FrontDisc_DrumSize"].Value);
                            objSpecs.RearDisc = Convert.ToBoolean(paramColl["@RearDisc"].Value);
                            objSpecs.RearDisc_DrumSize = Convert.ToUInt16(paramColl["@RearDisc_DrumSize"].Value);
                            objSpecs.CalliperType = paramColl["@CalliperType"].Value.ToString();
                            objSpecs.WheelSize = Convert.ToSingle(paramColl["@WheelSize"].Value);
                            objSpecs.FrontTyre = paramColl["@FrontTyre"].Value.ToString();
                            objSpecs.RearTyre = paramColl["@RearTyre"].Value.ToString();
                            objSpecs.TubelessTyres = Convert.ToBoolean(paramColl["@TubelessTyres"].Value);
                            objSpecs.RadialTyres = Convert.ToBoolean(paramColl["@RadialTyres"].Value);
                            objSpecs.AlloyWheels = Convert.ToBoolean(paramColl["@AlloyWheels"].Value);
                            objSpecs.ElectricSystem = paramColl["@ElectricSystem"].Value.ToString();
                            objSpecs.Battery = paramColl["@Battery"].Value.ToString();
                            objSpecs.HeadlightType = paramColl["@HeadlightType"].Value.ToString();
                            objSpecs.HeadlightBulbType = paramColl["@HeadlightBulbType"].Value.ToString();
                            objSpecs.Brake_Tail_Light = paramColl["@Brake_Tail_Light"].Value.ToString();
                            objSpecs.TurnSignal = paramColl["@TurnSignal"].Value.ToString();
                            objSpecs.PassLight = Convert.ToBoolean(paramColl["@PassLight"].Value);
                            objSpecs.Speedometer = paramColl["@Speedometer"].Value.ToString();
                            objSpecs.Tachometer = Convert.ToBoolean(paramColl["@Tachometer"].Value);
                            objSpecs.TachometerType = paramColl["@TachometerType"].Value.ToString();
                            objSpecs.ShiftLight = Convert.ToBoolean(paramColl["@ShiftLight"].Value);
                            objSpecs.ElectricStart = Convert.ToBoolean(paramColl["@ElectricStart"].Value);
                            objSpecs.Tripmeter = Convert.ToBoolean(paramColl["@Tripmeter"].Value);
                            objSpecs.NoOfTripmeters = paramColl["@NoOfTripmeters"].Value.ToString();
                            objSpecs.TripmeterType = paramColl["@TripmeterType"].Value.ToString();
                            objSpecs.LowFuelIndicator = Convert.ToBoolean(paramColl["@LowFuelIndicator"].Value);
                            objSpecs.LowOilIndicator = Convert.ToBoolean(paramColl["@LowOilIndicator"].Value);
                            objSpecs.LowBatteryIndicator = Convert.ToBoolean(paramColl["@LowBatteryIndicator"].Value);
                            objSpecs.FuelGauge = Convert.ToBoolean(paramColl["@FuelGauge"].Value);
                            objSpecs.DigitalFuelGauge = Convert.ToBoolean(paramColl["@DigitalFuelGauge"].Value);
                            objSpecs.PillionSeat = Convert.ToBoolean(paramColl["@PillionSeat"].Value);
                            objSpecs.PillionFootrest = Convert.ToBoolean(paramColl["@PillionFootrest"].Value);
                            objSpecs.PillionBackrest = Convert.ToBoolean(paramColl["@PillionBackrest"].Value);
                            objSpecs.PillionGrabrail = Convert.ToBoolean(paramColl["@PillionGrabrail"].Value);
                            objSpecs.StandAlarm = Convert.ToBoolean(paramColl["@StandAlarm"].Value);
                            objSpecs.SteppedSeat = Convert.ToBoolean(paramColl["@SteppedSeat"].Value);
                            objSpecs.AntilockBrakingSystem = Convert.ToBoolean(paramColl["@AntilockBrakingSystem"].Value);
                            objSpecs.Killswitch = Convert.ToBoolean(paramColl["@Killswitch"].Value);
                            objSpecs.Clock = Convert.ToBoolean(paramColl["@Clock"].Value);
                            objSpecs.MaxPowerRPM = Convert.ToSingle(paramColl["@MaxPowerRPM"].Value);
                            objSpecs.MaximumTorqueRPM = Convert.ToSingle(paramColl["@MaximumTorqueRPM"].Value);
                            objSpecs.Colors = paramColl["@Colors"].Value.ToString();
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
            finally
            {
                if (conn != null && conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
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
            Database db = null;
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = "GetSimilarBikesList";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@TopCount", SqlDbType.Int).Value = topCount;
                    cmd.Parameters.Add("@BikeVersionId", SqlDbType.Int).Value = versionId;
                    if (percentDeviation > 0)
                        cmd.Parameters.Add("@PercentDeviation", SqlDbType.Int).Value = percentDeviation;

                    db = new Database();
                    using (SqlDataReader dr = db.SelectQry(cmd))
                    {
                        objSimilarBikes = new List<SimilarBikeEntity>();
                
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
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            finally
            {
                db.CloseConnection();
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
            Database db = null;
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = "GetVersionColors";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@VersionId", SqlDbType.Int).Value = versionId;

                    db = new Database();
                    using (SqlDataReader dr = db.SelectQry(cmd))
                    {
                        objColors = new List<VersionColor>();

                        while (dr.Read())
                            objColors.Add(new VersionColor() { ColorName = dr["Color"].ToString(), ColorCode = dr["HexCode"].ToString(), CompanyCode = dr["CompanyCode"].ToString(), ColorId = Convert.ToUInt32(dr["ColorId"]) });
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
            finally
            {
                db.CloseConnection();
            }
            return objColors;
        }


    }   // class
}   // namespace
