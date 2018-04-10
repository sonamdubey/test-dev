
using Bikewale.Utility;
using Enyim.Caching;
using MySql.CoreDAL;
using System;
using System.Collections.Specialized;
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
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_requesttype", DbType.String, 20, RequestType));

                    using (DataSet ds = MySqlDatabase.SelectAdapterQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
                            dt = ds.Tables[0];
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

                    cmd.Parameters.Add(DbFactory.GetDbParam("par_requesttype", DbType.String, 20, RequestType));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_makeid", DbType.Int32, MakeId));


                    using (DataSet ds = MySqlDatabase.SelectAdapterQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
                            dt = ds.Tables[0];
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
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_requesttype", DbType.String, 20, RequestType));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_modelid", DbType.Int32, ModelId));


                    using (DataSet ds = MySqlDatabase.SelectAdapterQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
                            dt = ds.Tables[0];
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
            return dt;
        }


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
                    cmd.Parameters.Add(DbFactory.GetDbParam("@makeid", DbType.Int32, makeId));
                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
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
                ErrorClass.LogError(err, HttpContext.Current.Request.ServerVariables["URL"]);

            }

            return Make;
        }   // End of getMakeDetails



        /// <summary>
        /// Written By : Ashwini Todkar
        /// Method to update masking name in BikeMakes table and insert old masking name to OldMaskingLog 
        /// Modified By : Sushil Kumar on 9th July 2017
        /// Description : Change input parametres as per carwale mysql master base conventions
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
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_maskingname", DbType.String, 50, maskingName));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_makeid", DbType.Int32, makeId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_updatedby", DbType.Int32, updatedBy));
                    isSuccess = MySqlDatabase.UpdateQuery(cmd, ConnectionType.MasterDatabase);

                    // Update the Make Masking Name in CW database
                    NameValueCollection nvc = new NameValueCollection();
                    nvc.Add("v_MakeId", makeId);
                    nvc.Add("v_MaskingName", maskingName);
                    nvc.Add("v_MakeName", null);
                    nvc.Add("v_IsNew", null);
                    nvc.Add("v_IsUsed", null);
                    nvc.Add("v_IsFuturistic", null);
                    nvc.Add("v_IsDeleted", null);

                    SyncBWData.PushToQueue("BW_UpdateBikeMakes", DataBaseName.CW, nvc);

                    if (_mc != null)
                    {
                        _mc.Remove("BW_OldMakeMaskingNames");
                        _mc.Remove("BW_MakeMapping");
                    }
                    isSuccess = true;
                }
            }
            catch (SqlException err)
            {
                ErrorClass.LogError(err, HttpContext.Current.Request.ServerVariables["URL"]);

            }
            catch (Exception err)
            {
                ErrorClass.LogError(err, HttpContext.Current.Request.ServerVariables["URL"]);

            }

            return isSuccess;
        }//End of UpdateMakeMaskingName


        /// <summary>
        ///  Written By : Ashwini Todkar on 7 oct 2013
        ///  Method to update masking name in BikeModel Table and insert old masking Name to OldMaskingLog
        /// Modified By : Sushil Kumar on 9th July 2017
        /// Description : Change input parametres as per carwale mysql master base conventions
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

                    cmd.Parameters.Add(DbFactory.GetDbParam("par_maskingname", DbType.String, 50, maskingName));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_modelid", DbType.Int32, modelId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_updatedby", DbType.Int32, updatedBy));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_ismodelmaskingexist", DbType.Boolean, ParameterDirection.Output));
                    isSuccess = MySqlDatabase.UpdateQuery(cmd, ConnectionType.MasterDatabase);

                    if (!Convert.ToBoolean(cmd.Parameters["par_ismodelmaskingexist"].Value))
                    {
                        NameValueCollection nvc = new NameValueCollection();
                        nvc.Add("v_ModelMaskingName", maskingName);
                        nvc.Add("v_MakeId", null);
                        nvc.Add("v_ModelName", null);
                        nvc.Add("v_HostUrl", null);
                        nvc.Add("v_OriginalImagePath", null);
                        nvc.Add("v_IsUsed", null);
                        nvc.Add("v_IsNew", null);
                        nvc.Add("v_IsFuturistic", null);
                        nvc.Add("v_IsDeleted", null);
                        nvc.Add("v_ModelId", modelId);
                        SyncBWData.PushToQueue("BW_UpdateBikeModels", DataBaseName.CW, nvc);
                        isSuccess = true;
                    }
                    if (_mc != null)
                    {
                        _mc.Remove("BW_ModelMapping");
                        _mc.Remove("BW_NewBikeLaunches");
                        _mc.Remove("BW_OldModelMaskingNames_v1");
                        _mc.Remove("BW_NewModelMaskingNames_v1");
                        _mc.Remove("BW_TopVersionId");
                    }

                }
            }
            catch (SqlException err)
            {
                ErrorClass.LogError(err, HttpContext.Current.Request.ServerVariables["URL"]);

            }
            catch (Exception err)
            {
                ErrorClass.LogError(err, HttpContext.Current.Request.ServerVariables["URL"]);

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
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_modelid", DbType.Int32, modelId));

                    MySqlDatabase.UpdateQuery(cmd, ConnectionType.MasterDatabase);
                }
            }
            catch (SqlException err)
            {
                ErrorClass.LogError(err, HttpContext.Current.Request.ServerVariables["URL"]);

            }
            catch (Exception err)
            {
                ErrorClass.LogError(err, HttpContext.Current.Request.ServerVariables["URL"]);

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
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_makeid", DbType.Int32, makeId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_discription", DbType.String, synopsis.Trim()));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_userid", DbType.Int64, CurrentUser.Id));

                    MySqlDatabase.UpdateQuery(cmd, ConnectionType.MasterDatabase);

                }
            }
            catch (SqlException sqlEx)
            {
                HttpContext.Current.Trace.Warn("ManageMakeSynopsis Sql Error : ", sqlEx.Message);
                ErrorClass.LogError(sqlEx, HttpContext.Current.Request.ServerVariables["URL"]);

            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("ManageMakeSynopsis Exception : ", ex.Message);
                ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"]);

            }
        }   // End of ManageMakeSynopsis

        /// <summary>
        /// Written By : Ashwini Todkar on 29 July 2014
        /// Summary    : Method to set isdeleted flag of bike make ,its models and versions to 1 i.e. deleting a bike details
        /// Modified By : Sushil Kumar on 9th July 2017
        /// Description : Change input parametres as per carwale mysql master base conventions
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
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_makeid", DbType.Int32, makeId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_updatedby", DbType.Int32, deletedBy));
                    MySqlDatabase.UpdateQuery(cmd, ConnectionType.MasterDatabase);

                    // Push the data to carwale DB
                    // Create name value collection
                    NameValueCollection nvc = new NameValueCollection();
                    nvc.Add("v_MakeId", makeId);
                    nvc.Add("v_MaskingName", null);
                    nvc.Add("v_MakeName", null);
                    nvc.Add("v_IsNew", null);
                    nvc.Add("v_IsUsed", null);
                    nvc.Add("v_IsFuturistic", null);
                    nvc.Add("v_IsDeleted", "1");
                    SyncBWData.PushToQueue("BW_UpdateBikeMakes", DataBaseName.CW, nvc);

                }
            }
            catch (SqlException sqlEx)
            {
                HttpContext.Current.Trace.Warn("DeleteMakeModelVersion Sql Error : ", sqlEx.Message);
                ErrorClass.LogError(sqlEx, HttpContext.Current.Request.ServerVariables["URL"]);

            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("DeleteMakeModelVersion Exception : ", ex.Message);
                ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"]);

            }
        }

        /// <summary>
        /// Written By : Ashwini Todkar on 29 July 2014
        /// Summary    : Method to set isdeleted flag = 1 of a models and its versions on deleting a model
        /// Modified By : Sushil Kumar on 9th July 2017
        /// Description : Change input parametres as per carwale mysql master base conventions
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
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_modelid", DbType.Int32, modelId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_updatedby", DbType.Int32, deletedBy));
                    MySqlDatabase.UpdateQuery(cmd, ConnectionType.MasterDatabase);
                    NameValueCollection nvc = new NameValueCollection();
                    nvc.Add("v_MakeId", null);
                    nvc.Add("v_ModelName", null);
                    nvc.Add("v_ModelMaskingName", null);
                    nvc.Add("v_HostUrl", null);
                    nvc.Add("v_OriginalImagePath", null);
                    nvc.Add("v_IsUsed", null);
                    nvc.Add("v_IsNew", null);
                    nvc.Add("v_IsFuturistic", null);
                    nvc.Add("v_IsDeleted", "1");
                    nvc.Add("v_ModelId", modelId);
                    SyncBWData.PushToQueue("BW_UpdateBikeModels", DataBaseName.CW, nvc);
                }
            }
            catch (SqlException sqlEx)
            {
                HttpContext.Current.Trace.Warn("DeleteModelVersions Sql Error : ", sqlEx.Message);
                ErrorClass.LogError(sqlEx, HttpContext.Current.Request.ServerVariables["URL"]);

            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("DeleteModelVersions Exception : ", ex.Message);
                ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"]);

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

                    cmd.Parameters.Add(DbFactory.GetDbParam("par_makeid", DbType.Int32, makeId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_make", DbType.String, 30, ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_synopsis", DbType.String, 8000, ParameterDirection.Output));

                    MySqlDatabase.ExecuteNonQuery(cmd, ConnectionType.ReadOnly);

                    make = cmd.Parameters["par_makeid"].Value.ToString();
                    synopsis = cmd.Parameters["par_synopsis"].Value.ToString();
                }
            }
            catch (SqlException sqlEx)
            {
                HttpContext.Current.Trace.Warn("GetMakeSynopsis Sql Error : ", sqlEx.Message);
                ErrorClass.LogError(sqlEx, HttpContext.Current.Request.ServerVariables["URL"]);

            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("GetMakeSynopsis Exception : ", ex.Message);
                ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"]);

            }
        }   // End of GetMakeSynopsis

    }   // End of class
}   // End of namespace