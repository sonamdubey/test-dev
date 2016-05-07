using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using Enyim.Caching;
using Enyim.Caching.Memcached;
using Bikewale.CoreDAL;
using System.Data.Common;

/// <summary>
/// Getting All details of make model and versions of the bikes
/// </summary>

namespace BikeWaleOpr.Common
{
    /// <summary>
    ///     Class written for make, model and version operations
    /// </summary>
    public class MakeModelVersion
    {
        // Properties written 
        public string MakeId { get; set; }
        public string Make { get; set; }
        public string ModelId { get; set; }
        public string Model { get; set; }
        public string VersionId { get; set; }
        public string Version { get; set; }
        public bool IsFuturistic { get; set; }
        public string SmallPic { get; set; }
        public string LargePic { get; set; }
        public string BikeName { get; set; }
        public string HostUrl { get; set; }
        public string MinPrice { get; set; }
        public string MaxPrice { get; set; }

        bool _isMemcachedUsed;
        protected static MemcachedClient _mc = null;

        public MakeModelVersion()
        {
            _isMemcachedUsed = bool.Parse(ConfigurationManager.AppSettings.Get("IsMemcachedUsed"));
            if (_mc == null)
            {
                InitializeMemcached();
            }
        }

        #region Initialize Memcache
        private void InitializeMemcached()
        {
            _mc = new MemcachedClient("memcached");
        }
        #endregion

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
        /// Getting Models only by providing MakeId and request type
        /// </summary>
        /// <param name="MakeId"></param>
        /// <param name="RequestType">Pass value as New or Used or Upcoming or PriceQuote</param>
        /// <returns></returns>
        public DataTable GetModels(string MakeId, string RequestType)
        {
            DataTable dt = null;
            Database db = null;

            using (SqlCommand cmd = new SqlCommand("GetBikeModels"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@RequestType", SqlDbType.VarChar, 20).Value = RequestType;
                cmd.Parameters.Add("@MakeId", SqlDbType.Int).Value = MakeId;

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
        /// Written By : Ashish G. Kamble on 3/8/2012
        /// Getting Versions only by providing ModelId and request type
        /// </summary>
        /// <param name="ModelId"></param>
        /// <param name="RequestType">Pass value as New or Used or Upcoming or PriceQuote</param>
        /// <returns></returns>
        public DataTable GetVersions(string ModelId, string RequestType)
        {
            Database db = null;
            DataTable dt = null;

            using (SqlCommand cmd = new SqlCommand("GetBikeVersions"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@RequestType", SqlDbType.VarChar, 20).Value = RequestType;
                cmd.Parameters.Add("@ModelId", SqlDbType.Int).Value = ModelId;

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
        /// Written By : Ashish G. Kamble on 3/8/2012
        /// Method will set makeid, make, model on the basis of model id
        /// </summary>
        /// <param name="modelId"></param>
        public void GetModelDetails(string modelId)
        {
            Database db = null;
            SqlConnection conn = null;

            try
            {
                db = new Database();
                conn = new SqlConnection(db.GetConString());

                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "GetModelDetails";
                    cmd.Connection = conn;

                    HttpContext.Current.Trace.Warn("modelId : " + modelId);

                    cmd.Parameters.Add("@ModelId", SqlDbType.Int).Value = modelId;
                    cmd.Parameters.Add("@CityId", SqlDbType.Int).Value = Configuration.GetDefaultCityId;    // Prices for default city in webconfig
                    cmd.Parameters.Add("@MakeId", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@Make", SqlDbType.VarChar, 30).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@Model", SqlDbType.VarChar, 30).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@IsFuturistic", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@SmallPic", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@LargePic", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@HostURL", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@MinPrice", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@MaxPrice", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;                    

                    conn.Open();
                    cmd.ExecuteNonQuery();

                    HttpContext.Current.Trace.Warn("qry success");

                    ModelId = cmd.Parameters["@ModelId"].Value.ToString();
                    Model = cmd.Parameters["@Model"].Value.ToString();
                    MakeId = cmd.Parameters["@MakeId"].Value.ToString();
                    Make = cmd.Parameters["@Make"].Value.ToString();
                    IsFuturistic = Convert.ToBoolean(cmd.Parameters["@IsFuturistic"].Value);
                    SmallPic = cmd.Parameters["@SmallPic"].Value.ToString();
                    LargePic = cmd.Parameters["@LargePic"].Value.ToString();
                    HostUrl = cmd.Parameters["@HostURL"].Value.ToString();
                    MinPrice = cmd.Parameters["@MinPrice"].Value.ToString();
                    MaxPrice = cmd.Parameters["@MaxPrice"].Value.ToString();                    
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
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
        }   // End of GetMakeFromModelId method

        /// <summary>
        /// Written By : Ashish G. Kamble on 3/8/2012
        /// Method will set makeid, make, model id, model, version id, version on the basis of version id
        /// </summary>
        /// <param name="versionId"></param>
        /// <returns></returns>
        //public void GetVersionDetails(string versionId)
        //{
        //    Database db = null;
        //    SqlConnection conn = null;

        //    try
        //    {
        //        db = new Database();
        //        conn = new SqlConnection(db.GetConString());

        //        using (SqlCommand cmd = new SqlCommand())
        //        {
        //            cmd.CommandType = CommandType.StoredProcedure;
        //            cmd.CommandText = "GetVersionDetails";
        //            cmd.Connection = conn;

        //            cmd.Parameters.Add("@VersionId", SqlDbType.Int).Value = versionId;
        //            cmd.Parameters.Add("@CityId", SqlDbType.Int).Value = Configuration.GetDefaultCityId;    // Prices for default city in webconfig
        //            cmd.Parameters.Add("@MakeId", SqlDbType.Int).Direction = ParameterDirection.Output;
        //            cmd.Parameters.Add("@Make", SqlDbType.VarChar, 20).Direction = ParameterDirection.Output;
        //            cmd.Parameters.Add("@ModelId", SqlDbType.Int).Direction = ParameterDirection.Output;
        //            cmd.Parameters.Add("@Model", SqlDbType.VarChar, 20).Direction = ParameterDirection.Output;
        //            cmd.Parameters.Add("@Version", SqlDbType.VarChar, 20).Direction = ParameterDirection.Output;
        //            cmd.Parameters.Add("@HostUrl", SqlDbType.VarChar, 20).Direction = ParameterDirection.Output;
        //            cmd.Parameters.Add("@LargePic", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
        //            cmd.Parameters.Add("@SmallPic", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
        //            cmd.Parameters.Add("@MinPrice", SqlDbType.Int).Direction = ParameterDirection.Output;
        //            cmd.Parameters.Add("@MaxPrice", SqlDbType.Int).Direction = ParameterDirection.Output;
        //            cmd.Parameters.Add("@Bike", SqlDbType.VarChar, 100).Direction = ParameterDirection.Output;

        //            conn.Open();
        //            cmd.ExecuteNonQuery();

        //            VersionId = cmd.Parameters["@VersionId"].Value.ToString();
        //            Version = cmd.Parameters["@Version"].Value.ToString();
        //            ModelId = cmd.Parameters["@ModelId"].Value.ToString();
        //            Model = cmd.Parameters["@Model"].Value.ToString();
        //            MakeId = cmd.Parameters["@MakeId"].Value.ToString();
        //            Make = cmd.Parameters["@Make"].Value.ToString();
        //            BikeName = cmd.Parameters["@Bike"].Value.ToString();
        //            HostUrl = cmd.Parameters["@HostUrl"].Value.ToString();
        //            LargePic = cmd.Parameters["@LargePic"].Value.ToString();
        //            SmallPic = cmd.Parameters["@SmallPic"].Value.ToString();                    
        //            MinPrice = cmd.Parameters["@MinPrice"].Value.ToString();
        //            MaxPrice = cmd.Parameters["@MaxPrice"].Value.ToString();    
        //        }
        //    }
        //    catch (SqlException ex)
        //    {
        //        HttpContext.Current.Trace.Warn(ex.Message + ex.Source);
        //        ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
        //        objErr.SendMail();
        //    }
        //    catch (Exception ex)
        //    {
        //        HttpContext.Current.Trace.Warn(ex.Message + ex.Source);
        //        ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
        //        objErr.SendMail();
        //    }
        //    finally
        //    {
        //        if (conn.State == ConnectionState.Open)
        //        {
        //            conn.Close();
        //        }
        //    }
        //}   // End of GetVersionDetails method

        public void GetVersionDetails(string versionId)
        {
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "getversiondetails";

                    cmd.Parameters.Add(DbFactory.GetDbParam("par_versionid", DbParamTypeMapper.GetInstance[SqlDbType.Int], versionId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_cityid", DbParamTypeMapper.GetInstance[SqlDbType.Int], Configuration.GetDefaultCityId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_makeid", DbParamTypeMapper.GetInstance[SqlDbType.Int], ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_make", DbParamTypeMapper.GetInstance[SqlDbType.VarChar], 30, ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_modelid", DbParamTypeMapper.GetInstance[SqlDbType.Int], ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_model", DbParamTypeMapper.GetInstance[SqlDbType.VarChar], 30, ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_version", DbParamTypeMapper.GetInstance[SqlDbType.VarChar], 30, ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_hosturl", DbParamTypeMapper.GetInstance[SqlDbType.VarChar], 100, ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_largepic", DbParamTypeMapper.GetInstance[SqlDbType.VarChar], 50, ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_smallpic", DbParamTypeMapper.GetInstance[SqlDbType.VarChar], 50, ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_minprice", DbParamTypeMapper.GetInstance[SqlDbType.Int], ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_maxprice", DbParamTypeMapper.GetInstance[SqlDbType.Int], ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_bike", DbParamTypeMapper.GetInstance[SqlDbType.VarChar], 100, ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_maskingname", DbParamTypeMapper.GetInstance[SqlDbType.VarChar], 50, ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_makemaskingname", DbParamTypeMapper.GetInstance[SqlDbType.VarChar], 50, ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_originalimagepath", DbParamTypeMapper.GetInstance[SqlDbType.VarChar], 150, ParameterDirection.Output));

                    cmd.ExecuteNonQuery();

                    if (!String.IsNullOrEmpty(cmd.Parameters["par_makeid"].Value.ToString()))
                    {
                        VersionId = cmd.Parameters["par_VersionId"].Value.ToString();
                        Version = cmd.Parameters["par_version"].Value.ToString();
                        ModelId = cmd.Parameters["par_modelid"].Value.ToString();
                        Model = cmd.Parameters["par_model"].Value.ToString();
                        MakeId = cmd.Parameters["par_makeid"].Value.ToString();
                        Make = cmd.Parameters["par_make"].Value.ToString();
                        BikeName = cmd.Parameters["par_bike"].Value.ToString();
                        HostUrl = cmd.Parameters["par_hosturl"].Value.ToString();
                        LargePic = cmd.Parameters["par_largepic"].Value.ToString();
                        SmallPic = cmd.Parameters["par_smallpic"].Value.ToString();
                        MinPrice = cmd.Parameters["par_minprice"].Value.ToString();
                        MaxPrice = cmd.Parameters["par_maxprice"].Value.ToString();
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
        }   // End of GetVersionDetails method

        /// <summary>
        ///     Get Makeid and make name from the make id
        /// </summary>
        /// <param name="makeId"></param>
        /// <returns></returns>
        public string GetMakeDetails(string makeId)
        {
            // Validate the makeId
            if (!CommonOpn.IsNumeric(makeId))
                return "";

            string sql = "";

            sql = " SELECT Name AS MakeName, ID AS MakeId FROM BikeMakes "
                + " WHERE ID = @makeId ";

            SqlDataReader dr = null;
            Database db = new Database();
            SqlParameter[] param = { new SqlParameter("@makeId", makeId) };

            try
            {
                dr = db.SelectQry(sql, param);

                if (dr.Read())
                {
                    Make = dr["MakeName"].ToString();
                    MakeId = dr["MakeId"].ToString();

                    BikeName = dr["MakeName"].ToString();
                }

            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            finally
            {
                if (dr != null)
                {
                    dr.Close();
                }
                db.CloseConnection();
            }

            return Make;
        }   // End of getMakeDetails



        /// <summary>
        /// Written By : Ashwini Todkar
        /// Method to update masking name in BikeMakes table and insert old masking name to OldMaskingLog 
        /// </summary>
        /// <param name="maskingName"></param>
        /// <param name="updatedBy"></param>
        /// <param name="makeId"></param>
        public bool UpdateMakeMaskingName(string maskingName, string updatedBy, string makeId)
        {
            SqlCommand cmd;
            SqlParameter prm;
            Database db = null;
            bool isSuccess = false;

            try
            {
                db = new Database();

                cmd = new SqlCommand("UpdateMakeMaskingName");
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@MaskingName", SqlDbType.VarChar, 50).Value = maskingName;

                cmd.Parameters.Add("@MakeId", SqlDbType.Int).Value = makeId;

                prm = cmd.Parameters.Add("@UpdatedBy", SqlDbType.Int);
                prm.Value = updatedBy;

                isSuccess = db.UpdateQry(cmd);

                _mc.Remove("BW_MakeMapping");                
            }
            catch (SqlException err)
            {
                ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }

            return isSuccess;
        }//End of UpdateMakeMaskingName


        /// <summary>
        ///  Written By : Ashwini Todkar on 7 oct 2013
        ///  Method to update masking name in BikeModel Table and insert old masking Name to OldMaskingLog
        /// </summary>
        /// <param name="maskingName">passed as model masking name for url formation to bikemodel table</param>
        /// <param name="updatedBy">passed which user has updated last time</param>
        /// <param name="modelId">identify which model mask name is changed</param>
        /// <returns>nothing</returns>
        
        public bool UpdateModelMaskingName(string maskingName, string updatedBy, string modelId)
        {
            SqlCommand cmd;
            SqlParameter prm;
            Database db = null;
            bool isSuccess = false;

            try
            {   
                db = new Database();
                cmd = new SqlCommand("UpdateModelMaskingName");
                cmd.CommandType = CommandType.StoredProcedure;

                prm = cmd.Parameters.Add("@MaskingName", SqlDbType.VarChar,50);
                prm.Value = maskingName;

                prm = cmd.Parameters.Add("@updatedBy", SqlDbType.Int);
                prm.Value = updatedBy;

                prm = cmd.Parameters.Add("@ModelId", SqlDbType.Int);
                prm.Value = modelId;

                isSuccess = db.UpdateQry(cmd);

                _mc.Remove("BW_ModelMapping");
                _mc.Remove("BW_NewBikeLaunches");
                _mc.Remove("BW_OldModelMaskingNames");
                _mc.Remove("BW_NewModelMaskingNames");
                _mc.Remove("BW_TopVersionId");
            }
            catch (SqlException err)
            {
                ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            finally
            {
                if(db != null)
                    db.CloseConnection();
            }

            return isSuccess;
        }   // End of UpdateModelMaskingName

        /// <summary>
        /// Written By : Ashwini Todkar on 17 Feb 2014
        /// Method to discontinue all versions of a model
        /// </summary>
        /// <param name="modelId"></param>
        public void DiscontinueBikeModel(string modelId)
        {
            Database db = null;

            try 
            {
                using (SqlCommand cmd = new SqlCommand("DiscontinueBikeModel"))
                {
                    db = new Database();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@ModelId", SqlDbType.Int).Value = modelId;
                    db.UpdateQry(cmd);
                }
            }
            catch (SqlException err)
            {
                ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            finally
            {
                if (db != null)
                    db.CloseConnection();
            }
        }//End of DiscontinueBikeModel

        /// <summary>
        /// Written By : Ashwini Todkar on 20 Feb 2014
        /// Summary    : Method to save or update the make synopsis.
        /// </summary>
        /// <param name="makeId">Id of the make whose synopsis is to be updated.</param>
        public void ManageMakeSynopsis(string makeId, string synopsis)
        {
            Database db = null;
            
            try
            {
                db = new Database();

                using (SqlCommand cmd = new SqlCommand("ManageMakeSynopsis"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@MakeId", SqlDbType.Int).Value = makeId;
                    cmd.Parameters.Add("@Discription", SqlDbType.VarChar).Value = synopsis.Trim();
                    cmd.Parameters.Add("@UserId", SqlDbType.BigInt).Value = CurrentUser.Id;

                    db.UpdateQry(cmd);
                    HttpContext.Current.Trace.Warn("Success");
                }
            }
            catch (SqlException sqlEx)
            {
                HttpContext.Current.Trace.Warn("ManageMakeSynopsis Sql Error : ", sqlEx.Message);
                ErrorClass errObj = new ErrorClass(sqlEx, HttpContext.Current.Request.ServerVariables["URL"]);
                errObj.SendMail();
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("ManageMakeSynopsis Exception : ", ex.Message);
                ErrorClass errObj = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                errObj.SendMail();
            }
        }   // End of ManageMakeSynopsis

        /// <summary>
        /// Written By : Ashwini Todkar on 29 July 2014
        /// Summary    : Method to set isdeleted flag of bike make ,its models and versions to 1 i.e. deleting a bike details
        /// </summary>
        /// <param name="makeId"></param>
        /// <param name="deletedBy"></param>
        public void DeleteMakeModelVersion(string makeId, string deletedBy)
        {
            Database db = null;

            try
            {
                db = new Database();

                using (SqlCommand cmd = new SqlCommand("UpdateModelVersionIsDeleted"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@MakeId", SqlDbType.Int).Value = makeId;
                    cmd.Parameters.Add("@UpdatedBy", SqlDbType.Int).Value = deletedBy;

                    db.UpdateQry(cmd);
                }
            }
            catch (SqlException sqlEx)
            {
                HttpContext.Current.Trace.Warn("DeleteMakeModelVersion Sql Error : ", sqlEx.Message);
                ErrorClass errObj = new ErrorClass(sqlEx, HttpContext.Current.Request.ServerVariables["URL"]);
                errObj.SendMail();
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("DeleteMakeModelVersion Exception : ", ex.Message);
                ErrorClass errObj = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                errObj.SendMail();
            }
        }

        /// <summary>
        /// Written By : Ashwini Todkar on 29 July 2014
        /// Summary    : Method to set isdeleted flag = 1 of a models and its versions on deleting a model
        /// </summary>
        /// <param name="modelId"></param>
        /// <param name="deletedBy"></param>
        public void DeleteModelVersions(string modelId , string deletedBy)
        {
            Database db = null;

            try
            {
                db = new Database();

                using (SqlCommand cmd = new SqlCommand("UpdateVersionIsDeleted"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@ModelId", SqlDbType.Int).Value = modelId;
                    cmd.Parameters.Add("@UpdatedBy", SqlDbType.Int).Value = deletedBy;

                    db.UpdateQry(cmd);
                }
            }
            catch (SqlException sqlEx)
            {
                HttpContext.Current.Trace.Warn("DeleteModelVersions Sql Error : ", sqlEx.Message);
                ErrorClass errObj = new ErrorClass(sqlEx, HttpContext.Current.Request.ServerVariables["URL"]);
                errObj.SendMail();
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("DeleteModelVersions Exception : ", ex.Message);
                ErrorClass errObj = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                errObj.SendMail();
            }
        }

        /// <summary>
        /// Written By : Ashwini Todkar on 20 Feb 2014
        /// Summary    : Method to fill data in textbox when synopsis exists for bikemake
        /// </summary>
        /// <param name="makeId"></param>
        public void GetMakeSynopsis(string makeId, ref string make, ref string synopsis)
        {            
            Database db = null;
            
            try
            {
                db = new Database();

                using (SqlConnection conn = new SqlConnection(db.GetConString()))
                {
                    using (SqlCommand cmd = new SqlCommand("GetMakeSynopsis"))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Connection = conn;

                        cmd.Parameters.Add("@MakeId", SqlDbType.Int).Value = makeId;
                        cmd.Parameters.Add("@Make", SqlDbType.VarChar, 30).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@Synopsis", SqlDbType.VarChar,8000).Direction = ParameterDirection.Output;

                        conn.Open();
                        cmd.ExecuteNonQuery();

                        make = cmd.Parameters["@Make"].Value.ToString();
                        synopsis = cmd.Parameters["@Synopsis"].Value.ToString();

                        if (conn.State == ConnectionState.Open)
                            conn.Close();
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                HttpContext.Current.Trace.Warn("GetMakeSynopsis Sql Error : ", sqlEx.Message);
                ErrorClass errObj = new ErrorClass(sqlEx, HttpContext.Current.Request.ServerVariables["URL"]);
                errObj.SendMail();
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("GetMakeSynopsis Exception : ", ex.Message);
                ErrorClass errObj = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                errObj.SendMail();
            }
            finally
            {
                db.CloseConnection();
            }
        }   // End of GetMakeSynopsis

    }   // End of class
}   // End of namespace