using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using Bikewale.CoreDAL;
using Bikewale.Interfaces.BikeData;
using Bikewale.Entities.BikeData;
using Bikewale.Notifications;
using System.Collections;

namespace Bikewale.DAL.BikeData
{
    /// <summary>
    /// Created By : Ashish G. Kamble
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="U"></typeparam>
    public class BikeModelsRepository<T,U> : IBikeModelsRepository<T,U> where T : BikeModelEntity, new()
    {
        /// <summary>
        /// Summary : Function to get models list for a given make id.
        /// </summary>
        /// <param name="requestType">Bike data type</param>
        /// <param name="makeId"></param>
        /// <returns>Returns list containing objects of each model's basic data.</returns>
        public List<BikeModelEntityBase> GetModelsByType(EnumBikeType requestType, int makeId)
        {
            List<BikeModelEntityBase> objModelsList = null;

            Database db = null;
            SqlDataReader dr = null;

            try
            {
                using (SqlCommand cmd = new SqlCommand("GetBikeModels_New"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@RequestType", SqlDbType.VarChar, 20).Value = requestType.ToString();
                    cmd.Parameters.Add("@MakeId", SqlDbType.Int).Value = makeId;

                    db = new Database();

                    using (dr = db.SelectQry(cmd))
                    {
                        if (dr != null)
                        {
                            objModelsList = new List<BikeModelEntityBase>();

                            while (dr.Read())
                            {
                                objModelsList.Add(new BikeModelEntityBase()
                                {
                                    ModelId = Convert.ToInt32(dr["ID"]),
                                    ModelName = dr["NAME"].ToString(),
                                    MaskingName = dr["MaskingName"].ToString()
                                });
                            }
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

            return objModelsList;
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

        /// <summary>
        /// Summary : Function to get particular model's all details.
        /// Modified By : Sadhana Upadhyay on 20 Aug 2014
        /// Summary : To retrieve new and used flag
        /// </summary>
        /// <param name="id">Model Id should be a positive number.</param>
        /// <returns>Returns object containing the particular model's all details.</returns>
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
                        cmd.CommandText = "GetModelDetails_New";
                        cmd.Connection = conn;

                        HttpContext.Current.Trace.Warn("modelId : " + id);

                        cmd.Parameters.Add("@ModelId", SqlDbType.Int).Value = id;
                        cmd.Parameters.Add("@Make", SqlDbType.VarChar, 30).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@MakeId", SqlDbType.Int).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@Model", SqlDbType.VarChar, 30).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@IsFuturistic", SqlDbType.Bit).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@IsNew", SqlDbType.Bit).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@IsUsed", SqlDbType.Bit).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@SmallPic", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@LargePic", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@HostURL", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@MinPrice", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@MaxPrice", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@MaskingName", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@MakeMaskingName", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@SeriesId", SqlDbType.Int).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@SeriesName", SqlDbType.VarChar, 30).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@SeriesMaskingName", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@ReviewCount", SqlDbType.Int).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@ReviewRate", SqlDbType.Float).Direction = ParameterDirection.Output;

                        conn.Open();
                        cmd.ExecuteNonQuery();

                        HttpContext.Current.Trace.Warn("qry success");

                        if (!string.IsNullOrEmpty(cmd.Parameters["@MakeId"].Value.ToString()))
                        {

                            t.ModelId = Convert.ToInt32(cmd.Parameters["@ModelId"].Value);
                            t.ModelName = cmd.Parameters["@Model"].Value.ToString();
                            t.MakeBase.MakeId = Convert.ToInt32(cmd.Parameters["@MakeId"].Value);
                            t.MakeBase.MakeName = cmd.Parameters["@Make"].Value.ToString();
                            t.Futuristic = Convert.ToBoolean(cmd.Parameters["@IsFuturistic"].Value);
                            t.New = Convert.ToBoolean(cmd.Parameters["@IsNew"].Value);
                            t.Used = Convert.ToBoolean(cmd.Parameters["@IsUsed"].Value);
                            t.SmallPicUrl = cmd.Parameters["@SmallPic"].Value.ToString();
                            t.LargePicUrl = cmd.Parameters["@LargePic"].Value.ToString();
                            t.HostUrl = cmd.Parameters["@HostURL"].Value.ToString();
                            t.MinPrice = Convert.ToInt64(cmd.Parameters["@MinPrice"].Value);
                            t.MaxPrice = Convert.ToInt64(cmd.Parameters["@MaxPrice"].Value);
                            t.MaskingName = cmd.Parameters["@MaskingName"].Value.ToString();
                            t.MakeBase.MaskingName = cmd.Parameters["@MakeMaskingName"].Value.ToString();
                            t.ModelSeries.SeriesId = Convert.ToInt32(cmd.Parameters["@SeriesId"].Value);
                            t.ModelSeries.SeriesName = Convert.ToString(cmd.Parameters["@SeriesName"].Value);
                            t.ModelSeries.MaskingName = Convert.ToString(cmd.Parameters["@SeriesMaskingName"].Value);
                            t.ReviewCount = Convert.ToInt32(cmd.Parameters["@ReviewCount"].Value);
                            t.ReviewRate = Convert.ToDouble(cmd.Parameters["@ReviewRate"].Value);

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
        /// Summary  : Function to get the bike versions list for the given model id
        /// Modified By : Sadhana on 20 Aug 2014 
        /// passed new parameter to sp to retrieve version list for new as well as discontinued bikes
        /// </summary>
        /// <param name="modelId">Only positive numbers are allowed.</param>
        /// <returns>Returns the list containg BikeVersionsList</returns>
        public List<BikeVersionsListEntity> GetVersionsList(U modelId , bool isNew)
        {
            List<BikeVersionsListEntity> objList = null;
            Database db = null;
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "GetVersions_New";

                    cmd.Parameters.Add("@ModelId", SqlDbType.Int).Value = modelId;
                    cmd.Parameters.Add("@New", SqlDbType.Bit).Value = isNew;

                    db = new Database();

                    using (SqlDataReader dr = db.SelectQry(cmd))
                    {
                        if (dr != null)
                        {
                            objList = new List<BikeVersionsListEntity>();

                            while (dr.Read())
                            {
                                BikeVersionsListEntity version = new BikeVersionsListEntity();

                                version.VersionId = Convert.ToUInt32(dr["VersionId"]);
                                version.VersionName = Convert.ToString(dr["VersionName"]);
                                version.ModelName = Convert.ToString(dr["ModelName"]);
                                version.Price = Convert.ToUInt64(dr["Price"]);

                                objList.Add(version);
                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                HttpContext.Current.Trace.Warn("GetVersionsList sql ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("GetVersionsList ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            finally
            {
                db.CloseConnection();
            }

            return objList;
        }

        /// <summary>
        /// Summary : function to get the model description.
        /// </summary>
        /// <param name="modelId">model id should be positive number.</param>
        /// <returns>Returns description in the object.</returns>
        public BikeDescriptionEntity GetModelSynopsis(U modelId)
        {
            BikeDescriptionEntity objModel = null;
            Database db = null;
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = "GetModelSynopsis";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@ModelId", SqlDbType.Int).Value = modelId;

                    db = new Database();

                    using (SqlDataReader dr = db.SelectQry(cmd))
                    {
                        if (dr != null && dr.Read())
                        {
                            objModel = new BikeDescriptionEntity()
                            {
                                SmallDescription = Convert.ToString(dr["SmallDescription"]),
                                FullDescription = Convert.ToString(dr["FullDescription"])
                            };
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                HttpContext.Current.Trace.Warn("GetModelDescription sql ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("GetModelDescription ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            finally
            {
                db.CloseConnection();
            }

            return objModel;
        }   // End of GetModelDescription

        /// <summary>
        /// Summary : Function to get upcoming models information
        /// </summary>
        /// <param name="modelId">Only positive numbers are allowed.</param>
        /// <returns>Returns Upcoming bikes data in the UpcomingBikesEntity object.</returns>
        public UpcomingBikeEntity GetUpcomingBikeDetails(U modelId)
        {
            UpcomingBikeEntity objModel = null;
            Database db = null;
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = "GetUpcomingBikeDetails";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@ModelId", SqlDbType.Int).Value = modelId;

                    db = new Database();

                    using (SqlDataReader dr = db.SelectQry(cmd))
                    {
                        if (dr != null && dr.Read())
                        {
                            objModel = new UpcomingBikeEntity()
                            {
                                ExpectedLaunchId = Convert.ToUInt16(dr["id"]),
                                ExpectedLaunchDate = Convert.ToString(dr["ExpectedLaunch"]),
                                EstimatedPriceMin = Convert.ToUInt64(dr["EstimatedPriceMin"]),
                                EstimatedPriceMax = Convert.ToUInt64(dr["EstimatedPriceMax"]),
                                HostUrl = Convert.ToString(dr["HostURL"]),
                                LargePicImagePath = Convert.ToString(dr["LargePicImagePath"])
                            };
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                HttpContext.Current.Trace.Warn("GetUpcomingBikeDetails sql ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("GetUpcomingBikeDetails ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            finally
            {
                db.CloseConnection();
            }

            return objModel;
        }   // End of GetUpcomingBikeDetails

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inputParams"></param>
        /// <param name="sortBy"></param>
        /// <param name="recordCount"></param>
        /// <returns></returns>
        public List<UpcomingBikeEntity> GetUpcomingBikesList(UpcomingBikesListInputEntity inputParams, EnumUpcomingBikesFilter sortBy, out int recordCount)
        {
            List<UpcomingBikeEntity> objModelList = null;

            recordCount = 0;
            Database db = null;
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = "GetUpcomingBikesList_New";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@StartIndex", SqlDbType.Int).Value = inputParams.StartIndex;
                    cmd.Parameters.Add("@EndIndex", SqlDbType.Int).Value = inputParams.EndIndex;

                    if (inputParams.MakeId > 0) cmd.Parameters.Add("@MakeId", SqlDbType.Int).Value = inputParams.MakeId;
                    if (inputParams.ModelId > 0) cmd.Parameters.Add("@ModelId", SqlDbType.Int).Value = inputParams.ModelId;

                    if (sortBy != EnumUpcomingBikesFilter.Default)
                    {
                        if (sortBy == EnumUpcomingBikesFilter.PriceLowToHigh)
                            cmd.Parameters.Add("@EstimatedPrice", SqlDbType.Bit).Value = 0;
                        else if (sortBy == EnumUpcomingBikesFilter.PriceHighToLow)
                            cmd.Parameters.Add("@EstimatedPrice", SqlDbType.Bit).Value = 1;
                        else if (sortBy == EnumUpcomingBikesFilter.LaunchDateSooner)
                            cmd.Parameters.Add("@LaunchDate", SqlDbType.Bit).Value = 0;
                        else if (sortBy == EnumUpcomingBikesFilter.LaunchDateLater)
                            cmd.Parameters.Add("@LaunchDate", SqlDbType.Bit).Value = 1;
                    }

                    db = new Database();

                    using (SqlDataReader dr = db.SelectQry(cmd))
                    {
                        if (dr != null)
                        {
                            objModelList = new List<UpcomingBikeEntity>();

                            while (dr.Read())
                            {
                                UpcomingBikeEntity objModel = new UpcomingBikeEntity();

                                objModel.ExpectedLaunchId = Convert.ToUInt16(dr["ExpectedLaunchId"]);
                                objModel.ExpectedLaunchDate = Convert.ToString(dr["ExpectedLaunch"]);
                                objModel.EstimatedPriceMin = Convert.ToUInt64(dr["EstimatedPriceMin"]);
                                objModel.EstimatedPriceMax = Convert.ToUInt64(dr["EstimatedPriceMax"]);
                                objModel.HostUrl = Convert.ToString(dr["HostURL"]);
                                objModel.LargePicImagePath = Convert.ToString(dr["LargePicImagePath"]);
                                objModel.BikeDescription.SmallDescription = Convert.ToString(dr["Description"]);
                                objModel.MakeBase.MakeName = Convert.ToString(dr["MakeName"]);
                                objModel.MakeBase.MaskingName = Convert.ToString(dr["MakeMaskingName"]);
                                objModel.ModelBase.ModelName = Convert.ToString(dr["ModelName"]);
                                objModel.ModelBase.MaskingName = Convert.ToString(dr["ModelMaskingName"]);

                                objModelList.Add(objModel);

                                recordCount = Convert.ToInt32(dr["RecordCount"]);
                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                HttpContext.Current.Trace.Warn("GetUpcomingBikesList sql ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("GetUpcomingBikesList ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            finally
            {
                db.CloseConnection();
            }

            return objModelList;
        }

        /// <summary>
        /// Created By : Sadhana Upadhyay on 4 June 2014
        /// Summary : to get all recently launched bikes
        /// </summary>
        /// <param name="startIndex"></param>
        /// <param name="endIndex"></param>
        /// <param name="recordCount"></param>
        /// <returns></returns>
        public List<NewLaunchedBikeEntity> GetNewLaunchedBikesList(int startIndex, int endIndex, out int recordCount)
        {
            List<NewLaunchedBikeEntity> objModelList = null;
            Database db = null;
            recordCount = 0;
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = "GetNewLaunchedBikes";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@StartIndex", SqlDbType.Int).Value = startIndex;
                    cmd.Parameters.Add("@EndIndex", SqlDbType.Int).Value = endIndex;

                    db = new Database();

                    using (SqlDataReader dr = db.SelectQry(cmd))
                    {
                        if (dr != null)
                        {
                            objModelList = new List<NewLaunchedBikeEntity>();

                            while (dr.Read())
                            {
                                NewLaunchedBikeEntity objModels = new NewLaunchedBikeEntity();

                                objModels.BikeLaunchId = Convert.ToUInt16(dr["BikeLaunchId"]);
                                objModels.MakeBase.MakeId = Convert.ToInt32(dr["BikeMakeId"]);
                                objModels.MakeBase.MakeName = dr["Make"].ToString();
                                objModels.MakeBase.MaskingName = dr["MakeMaskingName"].ToString();
                                objModels.ModelId = Convert.ToInt32(dr["ModelId"]);
                                objModels.ModelName = dr["Model"].ToString();
                                objModels.MaskingName = dr["ModelMaskingName"].ToString();
                                objModels.HostUrl = dr["HostURL"].ToString();
                                objModels.LargePicUrl = dr["LargePic"].ToString();
                                objModels.SmallPicUrl = dr["SmallPic"].ToString();
                                objModels.ReviewCount = Convert.ToInt16(dr["ReviewCount"]);
                                objModels.ReviewRate = Convert.ToDouble(dr["ReviewRate"]);
                                objModels.MinPrice = Convert.ToInt64(dr["MinPrice"]);
                                objModels.MaxPrice = Convert.ToInt64(dr["MaxPrice"]);
                                objModels.LaunchDate = Convert.ToDateTime(dr["LaunchDate"]);
                                objModels.BasicId = Convert.ToUInt64(dr["BasicId"]);
                                objModels.RoadTestUrl = dr["RoadTestUrl"].ToString();

                                objModelList.Add(objModels);

                                recordCount = Convert.ToInt16(dr["RecordCount"]);
                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                HttpContext.Current.Trace.Warn("GetNewLaunchedBikesList sql ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("GetNewLaunchedBikesList ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            finally
            {
                db.CloseConnection();
            }

            return objModelList;
        }

        /// <summary>
        /// Created By : Suresh Prajapati on 24th Sep-14
        /// Summary : To create hash table for masking names
        /// </summary>
        /// <returns></returns>
        public Hashtable GetMaskingNames()
        {
            Database db = null;
            Hashtable ht = null;

            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = "SP_GetModelMappingNames";
                    cmd.CommandType = CommandType.StoredProcedure;

                    db = new Database();

                    using (SqlDataReader dr = db.SelectQry(cmd))
                    {
                        if(dr != null)
                        {
                            ht = new Hashtable();
           
                            while (dr.Read())
                            {
                                if (!ht.ContainsKey(dr["MaskingName"]))
                                ht.Add(dr["MaskingName"],dr["ID"]);
                            }
                        }
                     }
                 }
             }

             catch (SqlException ex)
             {
                 HttpContext.Current.Trace.Warn("SP_GetModelMappingNames sql ex : " + ex.Message + ex.Source);
                 ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                 objErr.SendMail();
             }
             catch (Exception ex)
             {
                 HttpContext.Current.Trace.Warn("SP_GetModelMappingNames ex : " + ex.Message + ex.Source);
                 ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                 objErr.SendMail();
             }
             finally
             {
                 db.CloseConnection();
             }
            return ht;
         }
       
        
        /// <summary>
        /// Created By : Suresh Prajapati on 24th Sep-2014
        /// Summary : To create Hash table for old masking names
        /// </summary>
        /// <returns></returns>
        public Hashtable GetOldMaskingNames()
        {
            Database db = null;
            Hashtable ht = null;

            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = "GetOldMaskingNamesList";
                    cmd.CommandType = CommandType.StoredProcedure;

                    db = new Database();

                    using (SqlDataReader dr = db.SelectQry(cmd))
                    {
                        if(dr != null )
                        {         
                            ht = new Hashtable();

                            while(dr.Read())
                            {
                                if (!ht.ContainsKey(dr["OldMaskingName"]))
                                    ht.Add(dr["OldMaskingName"],dr["NewMaskingName"]);
                            }
                        }
                     }
                 }
             }

             catch (SqlException ex)
             {
                 HttpContext.Current.Trace.Warn("GetOldMaskingNamesList sql ex : " + ex.Message + ex.Source);
                 ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                 objErr.SendMail();
             }
             catch (Exception ex)
             {
                 HttpContext.Current.Trace.Warn("GetOldMaskingNamesList ex : " + ex.Message + ex.Source);
                 ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                 objErr.SendMail();
             }
             finally
             {
                 db.CloseConnection();
             }
            return ht;
         }
       
    }   // class
}   // namespace
