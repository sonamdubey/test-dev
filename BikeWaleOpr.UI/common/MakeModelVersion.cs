using BikeWaleOPR.DAL.CoreDAL;
using BikeWaleOPR.Utilities;
using Enyim.Caching;
using System;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Web;

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
            if (_isMemcachedUsed && _mc == null)
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
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("getbikemakes"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_requesttype", DbParamTypeMapper.GetInstance[SqlDbType.VarChar], 20, RequestType));

                    using (DataSet ds = MySqlDatabase.SelectAdapterQuery(cmd))
                    {
                        if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
                            dt = ds.Tables[0];
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

            try
            {

                using (DbCommand cmd = DbFactory.GetDBCommand("getbikemodels"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(DbFactory.GetDbParam("par_requesttype", DbParamTypeMapper.GetInstance[SqlDbType.VarChar], 20, RequestType));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_makeid", DbParamTypeMapper.GetInstance[SqlDbType.Int], MakeId));


                    using (DataSet ds = MySqlDatabase.SelectAdapterQuery(cmd))
                    {
                        if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
                            dt = ds.Tables[0];
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

            DataTable dt = null;

            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("getbikeversions"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_requesttype", DbParamTypeMapper.GetInstance[SqlDbType.VarChar], 20, RequestType));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_modelid", DbParamTypeMapper.GetInstance[SqlDbType.Int], ModelId));


                    using (DataSet ds = MySqlDatabase.SelectAdapterQuery(cmd))
                    {
                        if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
                            dt = ds.Tables[0];
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
            return dt;
        }

        /// <summary>
        /// Written By : Ashish G. Kamble on 3/8/2012
        /// Method will set makeid, make, model on the basis of model id
        /// </summary>
        /// <param name="modelId"></param>
        //public void GetModelDetails(string modelId)
        //{
        //throw new NotImplementedException("public void GetModelDetails(string modelId)");
        //try
        //{
        //    using (DbCommand cmd = DbFactory.GetDBCommand())
        //    {
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.CommandText = "getmodeldetails";

        //        cmd.Parameters.Add(DbFactory.GetDbParam("par_modelid", DbParamTypeMapper.GetInstance[SqlDbType.Int], modelId));
        //        cmd.Parameters.Add(DbFactory.GetDbParam("par_cityid", DbParamTypeMapper.GetInstance[SqlDbType.Int], Configuration.GetDefaultCityId));    // Prices for default city in webconfig
        //        cmd.Parameters.Add(DbFactory.GetDbParam("par_makeid", DbParamTypeMapper.GetInstance[SqlDbType.Int], ParameterDirection.Output));
        //        cmd.Parameters.Add(DbFactory.GetDbParam("par_make", DbParamTypeMapper.GetInstance[SqlDbType.VarChar], 30, ParameterDirection.Output));
        //        cmd.Parameters.Add(DbFactory.GetDbParam("par_model", DbParamTypeMapper.GetInstance[SqlDbType.VarChar], 30, ParameterDirection.Output));
        //        cmd.Parameters.Add(DbFactory.GetDbParam("par_isfuturistic", DbParamTypeMapper.GetInstance[SqlDbType.Bit], ParameterDirection.Output));
        //        cmd.Parameters.Add(DbFactory.GetDbParam("par_smallpic", DbParamTypeMapper.GetInstance[SqlDbType.VarChar], 50, ParameterDirection.Output));
        //        cmd.Parameters.Add(DbFactory.GetDbParam("par_largepic", DbParamTypeMapper.GetInstance[SqlDbType.VarChar], 50, ParameterDirection.Output));
        //        cmd.Parameters.Add(DbFactory.GetDbParam("par_hosturl", DbParamTypeMapper.GetInstance[SqlDbType.VarChar], 50, ParameterDirection.Output));
        //        cmd.Parameters.Add(DbFactory.GetDbParam("par_minprice", DbParamTypeMapper.GetInstance[SqlDbType.VarChar], 50, ParameterDirection.Output));
        //        cmd.Parameters.Add(DbFactory.GetDbParam("par_maxprice", DbParamTypeMapper.GetInstance[SqlDbType.VarChar], 50, ParameterDirection.Output));

        //        MySqlDatabase.ExecuteNonQuery(cmd);

        //        ModelId = cmd.Parameters["par_modelid"].Value.ToString();
        //        Model = cmd.Parameters["par_model"].Value.ToString();
        //        MakeId = cmd.Parameters["par_makeid"].Value.ToString();
        //        Make = cmd.Parameters["par_Make"].Value.ToString();
        //        IsFuturistic = Convert.ToBoolean(cmd.Parameters["par_isfuturistic"].Value);
        //        SmallPic = cmd.Parameters["par_smallpic"].Value.ToString();
        //        LargePic = cmd.Parameters["par_largepic"].Value.ToString();
        //        HostUrl = cmd.Parameters["par_hosturl"].Value.ToString();
        //        MinPrice = cmd.Parameters["par_minprice"].Value.ToString();
        //        MaxPrice = cmd.Parameters["par_maxprice"].Value.ToString();
        //    }
        //}
        //catch (SqlException ex)
        //{
        //    HttpContext.Current.Trace.Warn("GetModelDetails sql ex : " + ex.Message + ex.Source);
        //    ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
        //    objErr.SendMail();
        //}
        //catch (Exception ex)
        //{
        //    HttpContext.Current.Trace.Warn("GetModelDetails ex : " + ex.Message + ex.Source);
        //    ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
        //    objErr.SendMail();
        //}
        //}   // End of GetMakeFromModelId method

        public void GetVersionDetails(string versionId)
        {

            throw new NotImplementedException();
            //try
            //{
            //    using (DbCommand cmd = DbFactory.GetDBCommand())
            //    {
            //        cmd.CommandType = CommandType.StoredProcedure;
            //        cmd.CommandText = "getversiondetails";

            //        cmd.Parameters.Add(DbFactory.GetDbParam("par_versionid", DbParamTypeMapper.GetInstance[SqlDbType.Int], versionId));
            //        cmd.Parameters.Add(DbFactory.GetDbParam("par_cityid", DbParamTypeMapper.GetInstance[SqlDbType.Int], Configuration.GetDefaultCityId));
            //        cmd.Parameters.Add(DbFactory.GetDbParam("par_makeid", DbParamTypeMapper.GetInstance[SqlDbType.Int], ParameterDirection.Output));
            //        cmd.Parameters.Add(DbFactory.GetDbParam("par_make", DbParamTypeMapper.GetInstance[SqlDbType.VarChar], 30, ParameterDirection.Output));
            //        cmd.Parameters.Add(DbFactory.GetDbParam("par_modelid", DbParamTypeMapper.GetInstance[SqlDbType.Int], ParameterDirection.Output));
            //        cmd.Parameters.Add(DbFactory.GetDbParam("par_model", DbParamTypeMapper.GetInstance[SqlDbType.VarChar], 30, ParameterDirection.Output));
            //        cmd.Parameters.Add(DbFactory.GetDbParam("par_version", DbParamTypeMapper.GetInstance[SqlDbType.VarChar], 30, ParameterDirection.Output));
            //        cmd.Parameters.Add(DbFactory.GetDbParam("par_hosturl", DbParamTypeMapper.GetInstance[SqlDbType.VarChar], 100, ParameterDirection.Output));
            //        cmd.Parameters.Add(DbFactory.GetDbParam("par_largepic", DbParamTypeMapper.GetInstance[SqlDbType.VarChar], 50, ParameterDirection.Output));
            //        cmd.Parameters.Add(DbFactory.GetDbParam("par_smallpic", DbParamTypeMapper.GetInstance[SqlDbType.VarChar], 50, ParameterDirection.Output));
            //        cmd.Parameters.Add(DbFactory.GetDbParam("par_minprice", DbParamTypeMapper.GetInstance[SqlDbType.Int], ParameterDirection.Output));
            //        cmd.Parameters.Add(DbFactory.GetDbParam("par_maxprice", DbParamTypeMapper.GetInstance[SqlDbType.Int], ParameterDirection.Output));
            //        cmd.Parameters.Add(DbFactory.GetDbParam("par_bike", DbParamTypeMapper.GetInstance[SqlDbType.VarChar], 100, ParameterDirection.Output));
            //        cmd.Parameters.Add(DbFactory.GetDbParam("par_maskingname", DbParamTypeMapper.GetInstance[SqlDbType.VarChar], 50, ParameterDirection.Output));
            //        cmd.Parameters.Add(DbFactory.GetDbParam("par_makemaskingname", DbParamTypeMapper.GetInstance[SqlDbType.VarChar], 50, ParameterDirection.Output));
            //        cmd.Parameters.Add(DbFactory.GetDbParam("par_originalimagepath", DbParamTypeMapper.GetInstance[SqlDbType.VarChar], 150, ParameterDirection.Output));

            //        MySqlDatabase.ExecuteNonQuery(cmd);

            //        if (!String.IsNullOrEmpty(cmd.Parameters["par_makeid"].Value.ToString()))
            //        {
            //            VersionId = cmd.Parameters["par_VersionId"].Value.ToString();
            //            Version = cmd.Parameters["par_version"].Value.ToString();
            //            ModelId = cmd.Parameters["par_modelid"].Value.ToString();
            //            Model = cmd.Parameters["par_model"].Value.ToString();
            //            MakeId = cmd.Parameters["par_makeid"].Value.ToString();
            //            Make = cmd.Parameters["par_make"].Value.ToString();
            //            BikeName = cmd.Parameters["par_bike"].Value.ToString();
            //            HostUrl = cmd.Parameters["par_hosturl"].Value.ToString();
            //            LargePic = cmd.Parameters["par_largepic"].Value.ToString();
            //            SmallPic = cmd.Parameters["par_smallpic"].Value.ToString();
            //            MinPrice = cmd.Parameters["par_minprice"].Value.ToString();
            //            MaxPrice = cmd.Parameters["par_maxprice"].Value.ToString();
            //        }
            //    }
            //}
            //catch (SqlException ex)
            //{
            //    HttpContext.Current.Trace.Warn(ex.Message + ex.Source);
            //    ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
            //    objErr.SendMail();
            //}
            //catch (Exception ex)
            //{
            //    HttpContext.Current.Trace.Warn(ex.Message + ex.Source);
            //    ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
            //    objErr.SendMail();
            //}
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

            sql = " select name as makename, id as makeid from bikemakes where id = @makeid ";

            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand(sql))
                {
                    cmd.Parameters.Add(DbFactory.GetDbParam("@makeid", DbParamTypeMapper.GetInstance[SqlDbType.Int], makeId));
                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd))
                    {
                        if (dr != null && dr.Read())
                        {
                            Make = dr["MakeName"].ToString();
                            MakeId = dr["MakeId"].ToString();

                            BikeName = dr["MakeName"].ToString();
                        }
                    }
                }

            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
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
            bool isSuccess = false;
            try
            {

                using (DbCommand cmd = DbFactory.GetDBCommand("updatemakemaskingname"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(DbFactory.GetDbParam("par_maskingname", DbParamTypeMapper.GetInstance[SqlDbType.VarChar], 50, maskingName));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_makeid", DbParamTypeMapper.GetInstance[SqlDbType.Int], makeId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_updatedby", DbParamTypeMapper.GetInstance[SqlDbType.Int], updatedBy));

                    isSuccess = MySqlDatabase.UpdateQuery(cmd);

                   // _mc.Remove("BW_MakeMapping");
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
            bool isSuccess = false;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("updatemodelmaskingname"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(DbFactory.GetDbParam("par_maskingname", DbParamTypeMapper.GetInstance[SqlDbType.VarChar], 50, maskingName));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_updatedby", DbParamTypeMapper.GetInstance[SqlDbType.Int], updatedBy));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_modelid", DbParamTypeMapper.GetInstance[SqlDbType.Int], modelId));

                    isSuccess = MySqlDatabase.UpdateQuery(cmd);

                    //_mc.Remove("BW_ModelMapping");
                    //_mc.Remove("BW_NewBikeLaunches");
                    //_mc.Remove("BW_OldModelMaskingNames");
                    //_mc.Remove("BW_NewModelMaskingNames");
                    //_mc.Remove("BW_TopVersionId");
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

            return isSuccess;
        }   // End of UpdateModelMaskingName

        /// <summary>
        /// Written By : Ashwini Todkar on 17 Feb 2014
        /// Method to discontinue all versions of a model
        /// </summary>
        /// <param name="modelId"></param>
        public void DiscontinueBikeModel(string modelId)
        {
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("discontinuebikemodel"))
                {

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_modelid", DbParamTypeMapper.GetInstance[SqlDbType.Int], modelId));

                    MySqlDatabase.UpdateQuery(cmd);
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
        }//End of DiscontinueBikeModel

        /// <summary>
        /// Written By : Ashwini Todkar on 20 Feb 2014
        /// Summary    : Method to save or update the make synopsis.
        /// </summary>
        /// <param name="makeId">Id of the make whose synopsis is to be updated.</param>
        public void ManageMakeSynopsis(string makeId, string synopsis)
        {
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("managemakesynopsis"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_makeid", DbParamTypeMapper.GetInstance[SqlDbType.Int], makeId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_discription", DbParamTypeMapper.GetInstance[SqlDbType.VarChar], synopsis.Trim()));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_userid", DbParamTypeMapper.GetInstance[SqlDbType.BigInt], CurrentUser.Id));

                    MySqlDatabase.UpdateQuery(cmd);

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
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("updatemodelversionisdeleted"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(DbFactory.GetDbParam("par_makeid", DbParamTypeMapper.GetInstance[SqlDbType.Int], makeId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_updatedby", DbParamTypeMapper.GetInstance[SqlDbType.Int], deletedBy));

                    MySqlDatabase.UpdateQuery(cmd);
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
        public void DeleteModelVersions(string modelId, string deletedBy)
        {
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("updateversionisdeleted"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(DbFactory.GetDbParam("par_modelid", DbParamTypeMapper.GetInstance[SqlDbType.Int], modelId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_updatedby", DbParamTypeMapper.GetInstance[SqlDbType.Int], deletedBy));

                    MySqlDatabase.UpdateQuery(cmd);
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
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("getmakesynopsis"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(DbFactory.GetDbParam("par_makeid", DbParamTypeMapper.GetInstance[SqlDbType.Int], makeId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_make", DbParamTypeMapper.GetInstance[SqlDbType.VarChar], 30, ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_synopsis", DbParamTypeMapper.GetInstance[SqlDbType.VarChar], 8000, ParameterDirection.Output));

                    MySqlDatabase.ExecuteNonQuery(cmd);

                    make = cmd.Parameters["par_makeid"].Value.ToString();
                    synopsis = cmd.Parameters["par_synopsis"].Value.ToString();
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
        }   // End of GetMakeSynopsis

    }   // End of class
}   // End of namespace