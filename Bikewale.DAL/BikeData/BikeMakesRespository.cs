using Bikewale.CoreDAL;
using Bikewale.Entities.BikeData;
using Bikewale.Interfaces.BikeData;
using Bikewale.Notifications;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
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


            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("getbikemakes_new_29032016"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_requesttype", DbType.String, 20, makeType.ToString()));

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd))
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
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn(ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
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
            try
            {
                t = new T();

                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "getmakedetails";

                    cmd.Parameters.Add(DbFactory.GetDbParam("par_makeid", DbType.Int32, id));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_makename", DbType.String, 30, ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_makemaskingname", DbType.String, 50, ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_new", DbType.Boolean, ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_used", DbType.Boolean, ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_futuristic", DbType.Boolean, ParameterDirection.Output));

                    // LogLiveSps.LogSpInGrayLog(cmd);
                    MySqlDatabase.ExecuteNonQuery(cmd);

                    HttpContext.Current.Trace.Warn("qry success");

                    if (!string.IsNullOrEmpty(cmd.Parameters["par_makename"].Value.ToString()))
                    {
                        t.MakeName = cmd.Parameters["par_makename"].Value.ToString();
                        t.MaskingName = cmd.Parameters["par_makemaskingname"].Value.ToString();
                        t.New = Convert.ToBoolean(cmd.Parameters["par_new"].Value);
                        t.Used = Convert.ToBoolean(cmd.Parameters["par_used"].Value);
                        t.Futuristic = Convert.ToBoolean(cmd.Parameters["par_futuristic"].Value);
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
            //Database db = null;
            //try
            //{
            //    using (SqlCommand cmd = new SqlCommand())
            //    {
            //        cmd.CommandText = "GetSerieswiseModels_New";
            //        cmd.CommandType = CommandType.StoredProcedure;

            //        cmd.Parameters.Add("@MakeId", SqlDbType.VarChar, 10).Value = makeId;

            //        db = new Database();

            //        using (SqlDataReader dr = db.SelectQry(cmd))
            //        {
            //            if (dr != null)
            //            {
            //                objList = new List<BikeModelsListEntity>();

            //                while (dr.Read())
            //                {
            //                    BikeModelsListEntity objModel = new BikeModelsListEntity();

            //                    objModel.ModelSeries.SeriesId = Convert.ToInt16(dr["SeriesId"]);
            //                    objModel.ModelSeries.SeriesName = Convert.ToString(dr["Series"]);
            //                    objModel.ModelSeries.MaskingName = Convert.ToString(dr["SeriesMaskingName"]);
            //                    objModel.ModelId = Convert.ToInt32(dr["ModelId"]);
            //                    objModel.ModelName = Convert.ToString(dr["Model"]);
            //                    objModel.ModelCount = Convert.ToUInt16(dr["ModelCount"]);
            //                    objModel.MinPrice = Convert.ToInt64(dr["MinPrice"]);
            //                    objModel.MaxPrice = Convert.ToInt64(dr["MaxPrice"]);
            //                    objModel.ReviewRate = Convert.ToDouble(dr["ReviewRate"]);
            //                    objModel.ReviewCount = Convert.ToUInt16(dr["ReviewCount"]);
            //                    objModel.SeriesSmallPicUrl = Convert.ToString(dr["SmallPicUrl"]);
            //                    objModel.SeriesHostUrl = Convert.ToString(dr["HostUrl"]);
            //                    objModel.MaskingName = Convert.ToString(dr["ModelMaskingName"]);
            //                    objModel.MakeBase.MaskingName = Convert.ToString(dr["MakeMaskingName"]);
            //                    objModel.ModelRank = Convert.ToUInt16(dr["ModelRank"]);
            //                    objModel.MakeBase.MakeName = Convert.ToString(dr["MakeName"]);
            //                    objModel.OriginalImagePath = Convert.ToString(dr["OriginalImagePath"]);
            //                    objList.Add(objModel);
            //                }
            //            }
            //        }
            //    }
            //}
            //catch (SqlException err)
            //{
            //    HttpContext.Current.Trace.Warn("SQL Exception in GetModelsList", err.Message);
            //    ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"]);
            //    objErr.SendMail();
            //}
            //catch (Exception err)
            //{
            //    HttpContext.Current.Trace.Warn("Exception in GetModelsList", err.Message);
            //    ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"]);
            //    objErr.SendMail();
            //}
            //finally
            //{
            //    db.CloseConnection();
            //}

            return objList;
        }

        public BikeDescriptionEntity GetMakeDescription(U makeId)
        {
            BikeDescriptionEntity objMake = null;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("getmakesynopsis"))
                {
                    //cmd.CommandText = "getmakesynopsis";
                    cmd.CommandType = CommandType.StoredProcedure;

                    //cmd.Parameters.Add("@MakeId", SqlDbType.Int).Value = makeId;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_makeid", DbType.Int32, makeId));

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd))
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
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("GetMakeDescription ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
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

            using (DbCommand cmd = DbFactory.GetDBCommand("getbikemakes"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(DbFactory.GetDbParam("par_requesttype", DbType.String, 20, RequestType));
                try
                {
                    using (DataSet ds = MySqlDatabase.SelectAdapterQuery(cmd))
                    {
                        if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
                            dt = ds.Tables[0];
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

            BikeMakeEntityBase makeDetails = null;

            try
            {

                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "getmakedetails";

                    cmd.Parameters.Add(DbFactory.GetDbParam("par_makeid", DbType.Int32, makeId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_makename", DbType.String, 30, ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_makemaskingname", DbType.String, 50, ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_new", DbType.Boolean, ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_used", DbType.Boolean, ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_futuristic", DbType.Boolean, ParameterDirection.Output));

                    // LogLiveSps.LogSpInGrayLog(cmd);
                    MySqlDatabase.ExecuteNonQuery(cmd);

                    if (!string.IsNullOrEmpty(cmd.Parameters["par_makename"].Value.ToString()))
                    {
                        makeDetails = new BikeMakeEntityBase();
                        makeDetails.MakeName = cmd.Parameters["par_makename"].Value.ToString();
                        makeDetails.MaskingName = cmd.Parameters["par_makemaskingname"].Value.ToString();
                        makeDetails.MakeId = Convert.ToInt32(makeId);
                    }
                }
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
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
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("getupcomingbikemakes"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (IDataReader reader = MySqlDatabase.SelectQuery(cmd))
                    {
                        if (reader != null)
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
                            reader.Close();
                        }
                    }
                }
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
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
            try
            {
                using (DbCommand DbCommand = DbFactory.GetDBCommand("getdiscontinuedbikemodelsbymake"))
                {
                    DbCommand.CommandType = CommandType.StoredProcedure;
                    DbCommand.Parameters.Add(DbFactory.GetDbParam("par_makeid", DbType.UInt32, makeId));
                    using (IDataReader dr = MySqlDatabase.SelectQuery(DbCommand))
                    {
                        if (dr != null)
                        {
                            bikeLinkList = new List<BikeVersionEntity>();
                            while (dr.Read())
                            {
                                bikeLinkList.Add(
                                        new BikeVersionEntity()
                                        {
                                            ModelMasking = Convert.ToString(dr["modelmaskingname"]),
                                            ModelName = Convert.ToString(dr["Name"])
                                        }
                                    );
                            }
                        }

                        dr.Close();
                    }
                }
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }

            return bikeLinkList;

        }
    }
}
