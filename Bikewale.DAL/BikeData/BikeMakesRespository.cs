using Bikewale.CoreDAL;
using Bikewale.Entities.BikeData;
using Bikewale.Interfaces.BikeData;
using Bikewale.Notifications;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web;

namespace Bikewale.DAL.BikeData
{
    /// <summary>
    /// Created By : Ashish G. Kamble
    /// Modified By : Lucky Rathore
    /// Summary : changes in function GetMakesByType
    /// Modified By :   Sumit Kate on 16 Nov 2015
    /// Summary :   Added new function UpcomingBikeMakes
    /// </summary>
    /// <typeparam name="T">Generic type (need to specify type while implementing this class)</typeparam>
    /// <typeparam name="U">Generic type (need to specify type while implementing this class)</typeparam>
    public class BikeMakesRepository<T, U> : IBikeMakes<T, U> where T : BikeMakeEntity, new()
    {
        /// <summary>
        /// Summary : Function to get all makes base entities
        /// Modified By : Lucky Rathore
        /// Summary : Added HostUrl and LogoUrl for BikeMakeEntityBase in GetMakesByType function.
        /// Modified by :   Sumit Kate on 03 Mar 2016
        /// Description :   Updated SP GetBikeMakes_New_03032016. Populate PopularityIndex.
        /// Modified by :   Sumit Kate on 29 Mar 2016
        /// Description :   GetBikeMakes_New_29032016 support Dealer request type which returns the makes list of BW and AB dealers
        /// </summary>
        /// <param name="makeType">Type of bike data</param>
        /// <returns>Returns list of type BikeMakeEntityBase</returns>
        public List<BikeMakeEntityBase> GetMakesByType(EnumBikeType makeType)
        {
            List<BikeMakeEntityBase> objMakesList = null;

            Database db = null;

            try
            {
                using (SqlCommand cmd = new SqlCommand("GetBikeMakes_New_29032016"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@RequestType", SqlDbType.VarChar, 20).Value = makeType.ToString();

                    db = new Database();

                    using (SqlDataReader dr = db.SelectQry(cmd))
                    {
                        if (dr != null)
                        {
                            objMakesList = new List<BikeMakeEntityBase>();

                            while (dr.Read())
                            {
                                objMakesList.Add(new BikeMakeEntityBase()
                                {
                                    MakeId = Convert.ToInt32(dr["ID"]),
                                    MakeName = dr["NAME"].ToString(),
                                    MaskingName = dr["MaskingName"].ToString(),
                                    HostUrl = Convert.IsDBNull(dr["HostUrl"]) ? "" : dr["HostUrl"].ToString(),
                                    LogoUrl = Convert.IsDBNull(dr["LogoUrl"]) ? "" : dr["LogoUrl"].ToString(),
                                    PopularityIndex = Convert.IsDBNull(dr["PopularityIndex"]) ? default(UInt16) : Convert.ToUInt16(dr["PopularityIndex"])
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

            return objMakesList;
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
        /// Summary : Function to get the make details by make id.
        /// </summary>
        /// <param name="id">MakeId. Only numbers are allowed. Negative values are not allowed.</param>
        /// <returns>Returns particular make's details in an object.</returns>
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
                        cmd.CommandText = "GetMakeDetails";
                        cmd.Connection = conn;

                        HttpContext.Current.Trace.Warn("modelId : " + id);

                        cmd.Parameters.Add("@MakeId", SqlDbType.Int).Value = id;
                        cmd.Parameters.Add("@MakeName", SqlDbType.VarChar, 30).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@MakeMaskingName", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@New", SqlDbType.Bit).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@Used", SqlDbType.Bit).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@Futuristic", SqlDbType.Bit).Direction = ParameterDirection.Output;

                        LogLiveSps.LogSpInGrayLog(cmd);
                        conn.Open();
                        cmd.ExecuteNonQuery();

                        HttpContext.Current.Trace.Warn("qry success");

                        if (!string.IsNullOrEmpty(cmd.Parameters["@MakeName"].Value.ToString()))
                        {
                            t.MakeName = cmd.Parameters["@MakeName"].Value.ToString();
                            t.MaskingName = cmd.Parameters["@MakeMaskingName"].Value.ToString();
                            t.New = Convert.ToBoolean(cmd.Parameters["@New"].Value);
                            t.Used = Convert.ToBoolean(cmd.Parameters["@Used"].Value);
                            t.Futuristic = Convert.ToBoolean(cmd.Parameters["@Futuristic"].Value);
                        }

                    }
                }
            }
            catch (SqlException ex)
            {
                HttpContext.Current.Trace.Warn("GetById sql ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("GetById ex : " + ex.Message + ex.Source);
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
        /// Create By : Ashish Kamble on 22 Apr 2014
        /// Summary : Function to get all models with series information for the given makeid.
        /// </summary>
        /// <param name="makeId">Only positive numbers are allowed.</param>
        /// <returns>Returns list containg BikeModelsListEntity.</returns>
        public List<BikeModelsListEntity> GetModelsList(U makeId)
        {
            List<BikeModelsListEntity> objList = null;
            Database db = null;
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = "GetSerieswiseModels_New";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@MakeId", SqlDbType.VarChar, 10).Value = makeId;

                    db = new Database();

                    using (SqlDataReader dr = db.SelectQry(cmd))
                    {
                        if (dr != null)
                        {
                            objList = new List<BikeModelsListEntity>();

                            while (dr.Read())
                            {
                                BikeModelsListEntity objModel = new BikeModelsListEntity();

                                objModel.ModelSeries.SeriesId = Convert.ToInt16(dr["SeriesId"]);
                                objModel.ModelSeries.SeriesName = Convert.ToString(dr["Series"]);
                                objModel.ModelSeries.MaskingName = Convert.ToString(dr["SeriesMaskingName"]);
                                objModel.ModelId = Convert.ToInt32(dr["ModelId"]);
                                objModel.ModelName = Convert.ToString(dr["Model"]);
                                objModel.ModelCount = Convert.ToUInt16(dr["ModelCount"]);
                                objModel.MinPrice = Convert.ToInt64(dr["MinPrice"]);
                                objModel.MaxPrice = Convert.ToInt64(dr["MaxPrice"]);
                                objModel.ReviewRate = Convert.ToDouble(dr["ReviewRate"]);
                                objModel.ReviewCount = Convert.ToUInt16(dr["ReviewCount"]);
                                objModel.SeriesSmallPicUrl = Convert.ToString(dr["SmallPicUrl"]);
                                objModel.SeriesHostUrl = Convert.ToString(dr["HostUrl"]);
                                objModel.MaskingName = Convert.ToString(dr["ModelMaskingName"]);
                                objModel.MakeBase.MaskingName = Convert.ToString(dr["MakeMaskingName"]);
                                objModel.ModelRank = Convert.ToUInt16(dr["ModelRank"]);
                                objModel.MakeBase.MakeName = Convert.ToString(dr["MakeName"]);
                                objModel.OriginalImagePath = Convert.ToString(dr["OriginalImagePath"]);
                                objList.Add(objModel);
                            }
                        }
                    }
                }
            }
            catch (SqlException err)
            {
                HttpContext.Current.Trace.Warn("SQL Exception in GetModelsList", err.Message);
                ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            catch (Exception err)
            {
                HttpContext.Current.Trace.Warn("Exception in GetModelsList", err.Message);
                ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            finally
            {
                db.CloseConnection();
            }

            return objList;
        }

        public BikeDescriptionEntity GetMakeDescription(U makeId)
        {
            BikeDescriptionEntity objMake = null;
            Database db = null;
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = "GetMakeSynopsis";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@MakeId", SqlDbType.Int).Value = makeId;

                    db = new Database();

                    using (SqlDataReader dr = db.SelectQry(cmd))
                    {
                        if (dr != null && dr.Read())
                        {
                            objMake = new BikeDescriptionEntity()
                            {
                                Name = Convert.ToString("MakeName"),
                                SmallDescription = Convert.ToString(dr["Description"]),
                                FullDescription = Convert.ToString(dr["Description"])
                            };

                            dr.Close();
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                HttpContext.Current.Trace.Warn("GetMakeDescription sql ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("GetMakeDescription ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            finally
            {
                db.CloseConnection();
            }

            return objMake;
        }

        /// <summary>
        /// Getting makes only by providing only request type
        /// </summary>
        /// <param name="RequestType">Pass value as New or Used or Upcoming or PriceQuote or ALL</param>
        /// <returns></returns>
        public DataTable GetMakes(string RequestType)
        {
            DataTable dt = null;
            Database db = null;

            using (SqlCommand cmd = new SqlCommand("GetBikeMakes"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@RequestType", SqlDbType.VarChar, 20).Value = RequestType;
                try
                {
                    db = new Database();
                    dt = db.SelectAdaptQry(cmd).Tables[0];
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
            }
            return dt;
        }

        /// <summary>
        ///     Get Makeid and make name from the make id
        /// </summary>
        /// <param name="makeId"></param>
        /// <returns></returns>
        public BikeMakeEntityBase GetMakeDetails(string makeId)
        {
            // Validate the makeId
            if (!CommonOpn.IsNumeric(makeId))
                return null;

            Database db = null;

            BikeMakeEntityBase makeDetails = null;

            //string sql = "";
            //BikeMakeEntityBase makeDetails = new BikeMakeEntityBase();

            //sql = " SELECT Name AS MakeName, ID AS MakeId , MaskingName FROM BikeMakes With(NoLock) "
            //    + " WHERE ID = @makeId ";

            //Database db = new Database();
            //SqlParameter[] param = { new SqlParameter("@makeId", makeId) };

            //try
            //{
            //    //using (SqlDataReader dr = db.SelectQry(sql, param))
            //    //{
            //    //    if (dr.Read())
            //    //    {
            //    //        makeDetails.MakeName = Convert.ToString(dr["MakeName"]);
            //    //        makeDetails.MakeId = Convert.ToInt32(dr["MakeId"]);
            //    //        makeDetails.MaskingName = Convert.ToString(dr["MaskingName"]);
            //    //    }
            //    //}


            //    var _makeDetails  = GetById(makeId);
            //        makeDetails.MakeName = _makeDetails.MakeName;
            //        makeDetails.MakeId = _makeDetails.MakeId ;
            //     makeDetails.MaskingName  = _makeDetails.MaskingName;    
            //}
            try
            {
                db = new Database();

                using (SqlConnection conn = new SqlConnection(db.GetConString()))
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "GetMakeDetails";
                        cmd.Connection = conn;

                        cmd.Parameters.Add("@MakeId", SqlDbType.Int).Value = makeId;
                        cmd.Parameters.Add("@MakeName", SqlDbType.VarChar, 30).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@MakeMaskingName", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@New", SqlDbType.Bit).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@Used", SqlDbType.Bit).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@Futuristic", SqlDbType.Bit).Direction = ParameterDirection.Output;
                        Bikewale.Notifications.LogLiveSps.LogSpInGrayLog(cmd);
                        conn.Open();
                        cmd.ExecuteNonQuery();

                        if (!string.IsNullOrEmpty(cmd.Parameters["@MakeName"].Value.ToString()))
                        {
                            makeDetails = new BikeMakeEntityBase();
                            makeDetails.MakeName = cmd.Parameters["@MakeName"].Value.ToString();
                            makeDetails.MaskingName = cmd.Parameters["@MakeMaskingName"].Value.ToString();
                            makeDetails.MakeId = Convert.ToInt32(makeId);
                        }
                    }
                }
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            finally
            {
                db.CloseConnection();
            }

            return makeDetails;
        }   // End of getMakeDetails
        /// <summary>
        /// Returns the Upcoming Bike's Make list
        /// Author  :   Sumit Kate
        /// Created :   16 Nov 2015
        /// </summary>
        /// <returns>Upcoming Bike's Make list</returns>
        public IEnumerable<BikeMakeEntityBase> UpcomingBikeMakes()
        {
            IList<BikeMakeEntityBase> makes = null;
            Database db = null;
            try
            {
                using (SqlCommand sqlCommand = new SqlCommand("GetUpcomingBikeMakes"))
                {
                    db = new Database();
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader reader = db.SelectQry(sqlCommand))
                    {
                        if (reader != null && reader.HasRows)
                        {
                            makes = new List<BikeMakeEntityBase>();
                            while (reader.Read())
                            {
                                makes.Add(
                                    new BikeMakeEntityBase()
                                    {
                                        MakeId = Convert.ToInt32(reader["ID"]),
                                        MakeName = Convert.ToString(reader["Name"]),
                                        MaskingName = Convert.ToString(reader["MaskingName"]),
                                        LogoUrl = Convert.ToString(reader["LogoUrl"]),
                                        HostUrl = Convert.ToString(reader["HostURL"])
                                    }
                                    );
                            }
                        }
                    }
                }
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            finally
            {
                db.CloseConnection();
            }
            return makes;
        }
        /// <summary>
        /// Written By : Sangram Nandkhile on 17 Jun 2016
        /// Description: Fetches discontinued bikes for a branch
        /// </summary>
        /// <param name="makeId">Make Id eg. 7 for Honda bikes</param>
        /// <returns></returns>
        public IEnumerable<BikeVersionEntity> GetDiscontinuedBikeModelsByMake(uint makeId)
        {
            IList<BikeVersionEntity> bikeLinkList = null;
            Database db = null;
            try
            {
                using (SqlCommand sqlCommand = new SqlCommand("GetDiscontinuedBikeModelsByMake"))
                {
                    db = new Database();
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.Add("@MakeId", SqlDbType.Int, 20).Value = makeId;
                    using (SqlDataReader reader = db.SelectQry(sqlCommand))
                    {
                        if (reader != null && reader.HasRows)
                        {
                            bikeLinkList = new List<BikeVersionEntity>();
                            while (reader.Read())
                            {
                                bikeLinkList.Add(
                                        new BikeVersionEntity()
                                        {
                                            ModelMasking = Convert.ToString(reader["modelmaskingname"]),
                                            ModelName = Convert.ToString(reader["Name"])
                                        }
                                    );
                            }
                        }
                    }
                }
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            finally
            {
                db.CloseConnection();
            }
            return bikeLinkList;

        }
    }
}
