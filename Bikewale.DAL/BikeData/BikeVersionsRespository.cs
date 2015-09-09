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
            SqlDataReader dr = null;

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

                    dr = db.SelectQry(cmd);

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
                if (dr != null)
                    dr.Close();
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
                        
                        if(dr!=null)
                        {
                            while(dr.Read())
                            {
                                objMinSpecs.Add( new BikeVersionMinSpecs(){
                                   VersionId  = Convert.ToInt32(dr["ID"]),
                                   VersionName = Convert.ToString(dr["Version"]),
                                   ModelName =Convert.ToString(dr["Model"]),
                                   Price  = Convert.ToUInt64(dr["VersionPrice"]),
                                   BrakeType  = Convert.ToString(dr["BrakeType"]),
                                   AlloyWheels  = Convert.ToBoolean(dr["AlloyWheels"]),
                                   ElectricStart  = Convert.ToBoolean(dr["ElectricStart"]),
                                   AntilockBrakingSystem  = Convert.ToBoolean(dr["AntilockBrakingSystem"])
                                }) ;
                            }
                        }
                    }
                }                 
            
            }
            catch (SqlException ex)
            {                     
                //ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"]);
                //objErr.SendMail();
            }
            catch (Exception ex)
            {   
              //  ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"]);
              //  objErr.SendMail();
            }
            finally
            {
                db.CloseConnection();
            }

            return objMinSpecs;
        }   // End of GetVersionsMinSpecs method


        /// <summary>
        /// Summary : Function to get all details of a particular version.
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
                        cmd.CommandText = "GetVersionDetails_New";
                        cmd.Connection = conn;

                        cmd.Parameters.Add("@VersionId", SqlDbType.Int).Value = id;
                        cmd.Parameters.Add("@MakeId", SqlDbType.Int).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@Make", SqlDbType.VarChar, 30).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@ModelId", SqlDbType.Int).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@Model", SqlDbType.VarChar, 30).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@Version", SqlDbType.VarChar, 30).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@HostUrl", SqlDbType.VarChar, 100).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@LargePic", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@SmallPic", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@Price", SqlDbType.Int).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@Bike", SqlDbType.VarChar, 100).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@MaskingName", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@MakeMaskingName", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@OriginalImagePath", SqlDbType.VarChar, 150).Direction = ParameterDirection.Output;
                        conn.Open();
                        cmd.ExecuteNonQuery();

                        HttpContext.Current.Trace.Warn("qry success");

                        if (!string.IsNullOrEmpty(cmd.Parameters["@MakeId"].Value.ToString()))
                        {
                            t.VersionId = Convert.ToInt32(cmd.Parameters["@VersionId"].Value);
                            t.VersionName = cmd.Parameters["@Version"].Value.ToString();
                            t.ModelBase.ModelId = Convert.ToInt32(cmd.Parameters["@ModelId"].Value);
                            t.ModelBase.ModelName = cmd.Parameters["@Model"].Value.ToString();
                            t.MakeBase.MakeId = Convert.ToInt32(cmd.Parameters["@MakeId"].Value);
                            t.MakeBase.MakeName = cmd.Parameters["@Make"].Value.ToString();
                            t.BikeName = cmd.Parameters["@Bike"].Value.ToString();
                            t.HostUrl = cmd.Parameters["@HostUrl"].Value.ToString();
                            t.LargePicUrl = cmd.Parameters["@LargePic"].Value.ToString();
                            t.SmallPicUrl = cmd.Parameters["@SmallPic"].Value.ToString();
                            t.Price = Convert.ToInt64(cmd.Parameters["@Price"].Value);
                            t.ModelBase.MaskingName = cmd.Parameters["@MaskingName"].Value.ToString();
                            t.MakeBase.MaskingName = cmd.Parameters["@MakeMaskingName"].Value.ToString();
                            t.OriginalImagePath = cmd.Parameters["@OriginalImagePath"].Value.ToString();
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
            finally
            {
                db.CloseConnection();
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
            try
            {
                db = new Database();

                using (SqlConnection conn = new SqlConnection(db.GetConString()))
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "GetNewBikesSpecification_SP_New";
                        cmd.Connection = conn;

                        cmd.Parameters.Add("@BikeVersionId", SqlDbType.SmallInt).Value = versionId;
                        cmd.Parameters.Add("@Displacement", SqlDbType.Float).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@Cylinders", SqlDbType.SmallInt).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@MaxPower", SqlDbType.Float).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@MaximumTorque", SqlDbType.Float).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@Bore", SqlDbType.Float).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@Stroke", SqlDbType.Float).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@ValvesPerCylinder", SqlDbType.SmallInt).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@FuelDeliverySystem", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@FuelType", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@Ignition", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@SparkPlugsPerCylinder", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@CoolingSystem", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@GearboxType", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@NoOfGears", SqlDbType.SmallInt).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@TransmissionType", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@Clutch", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@Performance_0_60_kmph", SqlDbType.Float).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@Performance_0_80_kmph", SqlDbType.Float).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@Performance_0_40_m", SqlDbType.Float).Direction = ParameterDirection.Output;
                        //changed topspeed data type from small int to Float
                        //Modified By : Sushil Kumar on 15-07-2015
                        cmd.Parameters.Add("@TopSpeed", SqlDbType.Float).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@Performance_60_0_kmph", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@Performance_80_0_kmph", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@KerbWeight", SqlDbType.SmallInt).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@OverallLength", SqlDbType.SmallInt).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@OverallWidth", SqlDbType.SmallInt).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@OverallHeight", SqlDbType.SmallInt).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@Wheelbase", SqlDbType.SmallInt).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@GroundClearance", SqlDbType.SmallInt).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@SeatHeight", SqlDbType.SmallInt).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@FuelTankCapacity", SqlDbType.Float).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@ReserveFuelCapacity", SqlDbType.Float).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@FuelEfficiencyOverall", SqlDbType.SmallInt).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@FuelEfficiencyRange", SqlDbType.SmallInt).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@ChassisType", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@FrontSuspension", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@RearSuspension", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@BrakeType", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@FrontDisc", SqlDbType.Bit).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@FrontDisc_DrumSize", SqlDbType.SmallInt).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@RearDisc", SqlDbType.Bit).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@RearDisc_DrumSize", SqlDbType.SmallInt).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@CalliperType", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@WheelSize", SqlDbType.Float).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@FrontTyre", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@RearTyre", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@TubelessTyres", SqlDbType.Bit).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@RadialTyres", SqlDbType.Bit).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@AlloyWheels", SqlDbType.Bit).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@ElectricSystem", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@Battery", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@HeadlightType", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@HeadlightBulbType", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@Brake_Tail_Light", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@TurnSignal", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@PassLight", SqlDbType.Bit).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@Speedometer", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@Tachometer", SqlDbType.Bit).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@TachometerType", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@ShiftLight", SqlDbType.Bit).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@ElectricStart", SqlDbType.Bit).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@Tripmeter", SqlDbType.Bit).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@NoOfTripmeters", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@TripmeterType", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@LowFuelIndicator", SqlDbType.Bit).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@LowOilIndicator", SqlDbType.Bit).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@LowBatteryIndicator", SqlDbType.Bit).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@FuelGauge", SqlDbType.Bit).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@DigitalFuelGauge", SqlDbType.Bit).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@PillionSeat", SqlDbType.Bit).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@PillionFootrest", SqlDbType.Bit).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@PillionBackrest", SqlDbType.Bit).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@PillionGrabrail", SqlDbType.Bit).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@StandAlarm", SqlDbType.Bit).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@SteppedSeat", SqlDbType.Bit).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@AntilockBrakingSystem", SqlDbType.Bit).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@Killswitch", SqlDbType.Bit).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@Clock", SqlDbType.Bit).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@Colors", SqlDbType.VarChar, 150).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@MaxPowerRPM",SqlDbType.Float).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@MaximumTorqueRPM", SqlDbType.Float).Direction = ParameterDirection.Output;
                        
                        cmd.Parameters.Add("@RowCount", SqlDbType.TinyInt).Direction = ParameterDirection.Output;

                        conn.Open();
                        int rowsAffected = cmd.ExecuteNonQuery();
                        int rowCount = Convert.ToInt16(cmd.Parameters["@RowCount"].Value);

                        if (rowCount > 0)
                        {
                            objSpecs = new BikeSpecificationEntity();

                            objSpecs.Displacement = Convert.ToSingle(cmd.Parameters["@Displacement"].Value);
                            objSpecs.Cylinders = Convert.ToUInt16(cmd.Parameters["@Cylinders"].Value);
                            objSpecs.MaxPower = Convert.ToUInt16(cmd.Parameters["@MaxPower"].Value);
                            objSpecs.MaximumTorque = Convert.ToUInt16(cmd.Parameters["@MaximumTorque"].Value);
                            objSpecs.Bore = Convert.ToSingle(cmd.Parameters["@Bore"].Value);
                            objSpecs.Stroke = Convert.ToSingle(cmd.Parameters["@Stroke"].Value);
                            objSpecs.ValvesPerCylinder = Convert.ToUInt16(cmd.Parameters["@ValvesPerCylinder"].Value);
                            objSpecs.FuelDeliverySystem = Convert.ToString(cmd.Parameters["@FuelDeliverySystem"].Value);
                            objSpecs.FuelType = Convert.ToString(cmd.Parameters["@FuelType"].Value);
                            objSpecs.Ignition = Convert.ToString(cmd.Parameters["@Ignition"].Value);
                            objSpecs.SparkPlugsPerCylinder = Convert.ToString(cmd.Parameters["@SparkPlugsPerCylinder"].Value);
                            objSpecs.CoolingSystem = Convert.ToString(cmd.Parameters["@CoolingSystem"].Value);
                            objSpecs.GearboxType = Convert.ToString(cmd.Parameters["@GearboxType"].Value);
                            objSpecs.NoOfGears = Convert.ToUInt16(cmd.Parameters["@NoOfGears"].Value);
                            objSpecs.TransmissionType = Convert.ToString(cmd.Parameters["@TransmissionType"].Value);
                            objSpecs.Clutch = Convert.ToString(cmd.Parameters["@Clutch"].Value);
                            objSpecs.Performance_0_60_kmph = Convert.ToSingle(cmd.Parameters["@Performance_0_60_kmph"].Value);
                            objSpecs.Performance_0_80_kmph = Convert.ToSingle(cmd.Parameters["@Performance_0_80_kmph"].Value);
                            objSpecs.Performance_0_40_m = Convert.ToSingle(cmd.Parameters["@Performance_0_40_m"].Value);
                            objSpecs.TopSpeed = Convert.ToUInt16(cmd.Parameters["@TopSpeed"].Value);
                            objSpecs.Performance_60_0_kmph = Convert.ToString(cmd.Parameters["@Performance_60_0_kmph"].Value);
                            objSpecs.Performance_80_0_kmph = Convert.ToString(cmd.Parameters["@Performance_80_0_kmph"].Value);
                            objSpecs.KerbWeight = Convert.ToUInt16(cmd.Parameters["@KerbWeight"].Value);
                            objSpecs.OverallLength = Convert.ToUInt16(cmd.Parameters["@OverallLength"].Value);
                            objSpecs.OverallWidth = Convert.ToUInt16(cmd.Parameters["@OverallWidth"].Value);
                            objSpecs.OverallHeight = Convert.ToUInt16(cmd.Parameters["@OverallHeight"].Value);
                            objSpecs.Wheelbase = Convert.ToUInt16(cmd.Parameters["@Wheelbase"].Value);
                            objSpecs.GroundClearance = Convert.ToUInt16(cmd.Parameters["@GroundClearance"].Value);
                            objSpecs.SeatHeight = Convert.ToUInt16(cmd.Parameters["@SeatHeight"].Value);
                            objSpecs.FuelTankCapacity = Convert.ToUInt16(cmd.Parameters["@FuelTankCapacity"].Value);
                            objSpecs.ReserveFuelCapacity = Convert.ToSingle(cmd.Parameters["@ReserveFuelCapacity"].Value);
                            objSpecs.FuelEfficiencyOverall = Convert.ToUInt16(cmd.Parameters["@FuelEfficiencyOverall"].Value);
                            objSpecs.FuelEfficiencyRange = Convert.ToUInt16(cmd.Parameters["@FuelEfficiencyRange"].Value);
                            objSpecs.ChassisType = Convert.ToString(cmd.Parameters["@ChassisType"].Value);
                            objSpecs.FrontSuspension = Convert.ToString(cmd.Parameters["@FrontSuspension"].Value);
                            objSpecs.RearSuspension = Convert.ToString(cmd.Parameters["@RearSuspension"].Value);
                            objSpecs.BrakeType = Convert.ToString(cmd.Parameters["@BrakeType"].Value);
                            objSpecs.FrontDisc = Convert.ToBoolean(cmd.Parameters["@FrontDisc"].Value);
                            objSpecs.FrontDisc_DrumSize = Convert.ToUInt16(cmd.Parameters["@FrontDisc_DrumSize"].Value);
                            objSpecs.RearDisc = Convert.ToBoolean(cmd.Parameters["@RearDisc"].Value);
                            objSpecs.RearDisc_DrumSize = Convert.ToUInt16(cmd.Parameters["@RearDisc_DrumSize"].Value);
                            objSpecs.CalliperType = Convert.ToString(cmd.Parameters["@CalliperType"].Value);
                            objSpecs.WheelSize = Convert.ToSingle(cmd.Parameters["@WheelSize"].Value);
                            objSpecs.FrontTyre = Convert.ToString(cmd.Parameters["@FrontTyre"].Value);
                            objSpecs.RearTyre = Convert.ToString(cmd.Parameters["@RearTyre"].Value);
                            objSpecs.TubelessTyres = Convert.ToBoolean(cmd.Parameters["@TubelessTyres"].Value);
                            objSpecs.RadialTyres = Convert.ToBoolean(cmd.Parameters["@RadialTyres"].Value);
                            objSpecs.AlloyWheels = Convert.ToBoolean(cmd.Parameters["@AlloyWheels"].Value);
                            objSpecs.ElectricSystem = Convert.ToString(cmd.Parameters["@ElectricSystem"].Value);
                            objSpecs.Battery = Convert.ToString(cmd.Parameters["@Battery"].Value);
                            objSpecs.HeadlightType = Convert.ToString(cmd.Parameters["@HeadlightType"].Value);
                            objSpecs.HeadlightBulbType = Convert.ToString(cmd.Parameters["@HeadlightBulbType"].Value);
                            objSpecs.Brake_Tail_Light = Convert.ToString(cmd.Parameters["@Brake_Tail_Light"].Value);
                            objSpecs.TurnSignal = Convert.ToString(cmd.Parameters["@TurnSignal"].Value);
                            objSpecs.PassLight = Convert.ToBoolean(cmd.Parameters["@PassLight"].Value);
                            objSpecs.Speedometer = Convert.ToString(cmd.Parameters["@Speedometer"].Value);
                            objSpecs.Tachometer = Convert.ToBoolean(cmd.Parameters["@Tachometer"].Value);
                            objSpecs.TachometerType = Convert.ToString(cmd.Parameters["@TachometerType"].Value);
                            objSpecs.ShiftLight = Convert.ToBoolean(cmd.Parameters["@ShiftLight"].Value);
                            objSpecs.ElectricStart = Convert.ToBoolean(cmd.Parameters["@ElectricStart"].Value);
                            objSpecs.Tripmeter = Convert.ToBoolean(cmd.Parameters["@Tripmeter"].Value);
                            objSpecs.NoOfTripmeters = Convert.ToString(cmd.Parameters["@NoOfTripmeters"].Value);
                            objSpecs.TripmeterType = Convert.ToString(cmd.Parameters["@TripmeterType"].Value);
                            objSpecs.LowFuelIndicator = Convert.ToBoolean(cmd.Parameters["@LowFuelIndicator"].Value);
                            objSpecs.LowOilIndicator = Convert.ToBoolean(cmd.Parameters["@LowOilIndicator"].Value);
                            objSpecs.LowBatteryIndicator = Convert.ToBoolean(cmd.Parameters["@LowBatteryIndicator"].Value);
                            objSpecs.FuelGauge = Convert.ToBoolean(cmd.Parameters["@FuelGauge"].Value);
                            objSpecs.DigitalFuelGauge = Convert.ToBoolean(cmd.Parameters["@DigitalFuelGauge"].Value);
                            objSpecs.PillionSeat = Convert.ToBoolean(cmd.Parameters["@PillionSeat"].Value);
                            objSpecs.PillionFootrest = Convert.ToBoolean(cmd.Parameters["@PillionFootrest"].Value);
                            objSpecs.PillionBackrest = Convert.ToBoolean(cmd.Parameters["@PillionBackrest"].Value);
                            objSpecs.PillionGrabrail = Convert.ToBoolean(cmd.Parameters["@PillionGrabrail"].Value);
                            objSpecs.StandAlarm = Convert.ToBoolean(cmd.Parameters["@StandAlarm"].Value);
                            objSpecs.SteppedSeat = Convert.ToBoolean(cmd.Parameters["@SteppedSeat"].Value);
                            objSpecs.AntilockBrakingSystem = Convert.ToBoolean(cmd.Parameters["@AntilockBrakingSystem"].Value);
                            objSpecs.Killswitch = Convert.ToBoolean(cmd.Parameters["@Killswitch"].Value);
                            objSpecs.Clock = Convert.ToBoolean(cmd.Parameters["@Clock"].Value);
                            objSpecs.MaxPowerRPM = Convert.ToSingle(cmd.Parameters["@MaxPowerRPM"].Value);
                            objSpecs.MaximumTorqueRPM = Convert.ToSingle(cmd.Parameters["@MaximumTorqueRPM"].Value);
                            objSpecs.Colors = Convert.ToString(cmd.Parameters["@Colors"].Value);
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
                db.CloseConnection();
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
                            objBike.MakeBase.MakeName = Convert.ToString(dr["MakeName"]);
                            objBike.MakeBase.MaskingName = Convert.ToString(dr["MakeMaskingName"]);
                            objBike.ModelBase.ModelId = Convert.ToInt32(dr["ModelId"]);
                            objBike.ModelBase.ModelName = Convert.ToString(dr["ModelName"]);
                            objBike.ModelBase.MaskingName = Convert.ToString(dr["ModelMaskingName"]);
                            objBike.VersionBase.VersionId = Convert.ToInt32(dr["VersionId"]);
                            objBike.HostUrl = Convert.ToString(dr["HostUrl"]);
                            objBike.LargePicUrl = "/bikewaleimg/models/" + Convert.ToString(dr["LargePic"]);
                            objBike.SmallPicUrl = "/bikewaleimg/models/" + Convert.ToString(dr["SmallPic"]);
                            objBike.MinPrice = Convert.ToInt32(dr["MinPrice"]);
                            objBike.MaxPrice = Convert.ToInt32(dr["MaxPrice"]);
                            objBike.VersionPrice = Convert.ToInt32(dr["VersionPrice"]);
                            objBike.OriginalImagePath = Convert.ToString(dr["OriginalImagePath"]);
                            objBike.Displacement = SqlReaderConvertor.ToNullableFloat(dr["Displacement"]);
                            objBike.FuelEfficiencyOverall = SqlReaderConvertor.ToNullableUInt16(dr["FuelEfficiencyOverall"]);
                            objBike.MaximumTorque = SqlReaderConvertor.ToNullableFloat(dr["MaximumTorque"]);
                            objBike.MaxPower = SqlReaderConvertor.ToNullableUInt16(dr["MaxPower"]);
                            objBike.ReviewCount = Convert.ToUInt16(dr["ReviewCount"]);
                            objBike.ReviewRate = Convert.ToDouble(dr["ReviewRate"]);
                            objSimilarBikes.Add(objBike);
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
