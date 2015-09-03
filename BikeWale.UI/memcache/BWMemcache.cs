using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using Bikewale.Common;
using System.Data.SqlClient;
using Enyim.Caching;
using Enyim.Caching.Memcached;
using System.Configuration;
using System.Collections;

namespace Bikewale.Memcache
{
    /// <summary>
    /// Created By : Ashish G. Kamble on 29 Oct 2013
    /// Summary : Class have business logic related to bikewale memchache.
    /// </summary>
    public class BWMemcache
    {
        bool _isMemcachedUsed;
        protected static MemcachedClient _mc = null;

        public BWMemcache()
        {
            // Check if memchache is to be used or not.
            _isMemcachedUsed = bool.Parse(ConfigurationManager.AppSettings.Get("IsMemcachedUsed"));
            if (_mc == null)
            {
                InitializeMemcached();
            }
        }

        #region Initialize Memcache
        /// <summary>
        /// Function to initialize the memchache instance. Singletone.
        /// </summary>
        private void InitializeMemcached()
        {
            _mc = new MemcachedClient("memcached");
        }
        #endregion

        #region DataSet in Memcache

        /// <summary>
        /// Fetch the DataSet from Database based on the Memcache Key provided
        /// </summary>
        /// <param name="key">The Memcache Key (key should be like BW_ModelMapping)</param>
        /// <returns>DataSet</returns>
        private DataSet FetchDSfromDb(string key)
        {
            DataSet ds = new DataSet();
            SqlParameter param;

            try
            {
                if (key.Equals("BW_BikeMakes"))
                {
                    param = new SqlParameter("@condition", SqlDbType.VarChar, 10);
                    param.Value = "Make";

                    ds = FetchDataFromDatabase("[dbo].[GetMakeModelVersion]", param);
                }
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn(ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]
                    + "\r\nMemcacheKey : " + key
                    + "\r\nMemcache Object Type : " + (_mc.Get(key) == null ? "null" : _mc.Get(key).GetType().ToString())
                    + "\r\nRequest Type: " + HttpContext.Current.Request.RequestType);
                objErr.SendMail();
            }
            return ds;
        }   // End of FetchDSfromDb

        /// <summary>
        /// Fetch the DataSet from Database based on the Memcache Key provided
        /// </summary>
        /// <param name="key">The Memcache Key (key should be like BW_ModelMapping)</param>
        /// <returns>DataSet</returns>
        private DataSet FetchDSfromDb(string key, SqlParameter param)
        {
            DataSet ds = new DataSet();
            
            try
            {                
                if (key.Equals("BW_NewBikeLaunches"))
                {
                    ds = FetchDataFromDatabase("[dbo].[GetNewBikeLaunches]", param);
                }
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn(ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]
                    + "\r\nMemcacheKey : " + key
                    + "\r\nMemcache Object Type : " + (_mc.Get(key) == null ? "null" : _mc.Get(key).GetType().ToString())
                    + "\r\nRequest Type: " + HttpContext.Current.Request.RequestType);
                objErr.SendMail();
            }
            return ds;
        }   // End of FetchDSfromDb


        /// <summary>
        /// PopulateWhere to get data from database for the particular sp provided.
        /// </summary>
        /// <param name="spName">Name of the sp from which data is required.</param>
        /// <param name="param">If any sql parameters to be passed on to sp. (optional)</param>
        /// <returns>Function returns dataset containing required data.</returns>
        private DataSet FetchDataFromDatabase(string spName, SqlParameter param = null)
        {
            DataSet ds = null;

            try
            {
                Database db = new Database();

                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = spName;

                    if (param != null)
                    {
                        cmd.Parameters.Add(param.ParameterName, param.SqlDbType).Value = param.Value;
                        HttpContext.Current.Trace.Warn("Added " + param.ParameterName + " with value " + param.Value.ToString() + " to SP " + spName);
                    }

                    // Fetch the data from the database into DataSet
                    HttpContext.Current.Trace.Warn("Fetched " + spName + " from Database");
                    ds = db.SelectAdaptQry(cmd);
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
            return ds;
        }   // End of FetchDataFromDatabase
        

        /// <summary>
        /// Get the DataSet from the Memcached Key. If the corresponding DataSet does not exist, create one in Memcached
        /// </summary>
        /// <param name="key">The Memcached Ket</param>
        /// <param name="duration">The time Duration for which the object is created</param>
        /// <returns>DataSet</returns>
        public DataSet GetDataSet(string key)
        {
            DataSet ds = new DataSet();
            try
            {
                if (_isMemcachedUsed)
                {
                    ds = (DataSet)_mc.Get(key);
                    HttpContext.Current.Trace.Warn("Read " + key + " from Memcached");
                    if (ds == null)
                    {
                        HttpContext.Current.Trace.Warn(key + " not found in Memcached");
                        if (_mc.Store(StoreMode.Add, key + "_lock", "lock", DateTime.Now.AddSeconds(60)))
                        {
                            ds = FetchDSfromDb(key);
                            // Insert the DataSet into the Cache
                            bool isCreated = _mc.Store(StoreMode.Add, key, ds,
                                DateTime.Now.AddMinutes(double.Parse(ConfigurationManager.AppSettings.Get("MemcacheTimespan"))));
                            HttpContext.Current.Trace.Warn("Inserted " + key + " into Memcached " + isCreated.ToString());
                            _mc.Remove(key + "_lock");
                        }
                        else
                        {
                            ds = FetchDSfromDb(key);
                        }
                    }
                }
                else
                {
                    ds = FetchDSfromDb(key);
                }
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn(ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]
                    + "\r\nMemcacheKey : " + key
                    + "\r\nMemcache Object Type : " + (_mc.Get(key) == null ? "null" : _mc.Get(key).GetType().ToString())
                    + "\r\nRequest Type: " + HttpContext.Current.Request.RequestType);
                objErr.SendMail();
            }
            finally
            {
                // If the DataSet is null, finally fetch it from the Database
                if (ds == null)
                {
                    ds = FetchDSfromDb(key);
                }
            }
            return ds;
        }   // End of GetDataSet
        
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public DataSet GetDataSet(string key,SqlParameter parameters)
        {
            DataSet ds = new DataSet();
            try
            {
                if (_isMemcachedUsed)
                {
                    ds = (DataSet)_mc.Get(key);
                    HttpContext.Current.Trace.Warn("Read " + key + " from Memcached");
                    if (ds == null)
                    {
                        HttpContext.Current.Trace.Warn(key + " not found in Memcached");
                        if (_mc.Store(StoreMode.Add, key + "_lock", "lock", DateTime.Now.AddSeconds(60)))
                        {
                            ds = FetchDSfromDb(key, parameters);
                            // Insert the DataSet into the Cache
                            _mc.Store(StoreMode.Add, key, ds,
                                DateTime.Now.AddMinutes(double.Parse(ConfigurationManager.AppSettings.Get("MemcacheTimespan"))));
                            HttpContext.Current.Trace.Warn("Inserted " + key + " into Memcached");
                            _mc.Remove(key + "_lock");
                        }
                        else
                        {
                            ds = FetchDSfromDb(key, parameters);
                        }
                    }
                }
                else
                {
                    ds = FetchDSfromDb(key, parameters);
                }
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn(ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]
                    + "\r\nMemcacheKey : " + key
                    + "\r\nMemcache Object Type : " + (_mc.Get(key) == null ? "null" : _mc.Get(key).GetType().ToString())
                    + "\r\nRequest Type: " + HttpContext.Current.Request.RequestType);
                objErr.SendMail();
            }
            finally
            {
                // If the DataSet is null, finally fetch it from the Database
                if (ds == null)
                {
                    ds = FetchDSfromDb(key, parameters);
                }
            }
            return ds;
        }   // End of GetDataSet

        #endregion

        #region Hashtable in Memcache

        /// <summary>
        /// Fetch the Hashtable from Database based on the Memcache Key provided
        /// </summary>
        /// <param name="key">The Memcache Key (key should be like BW_ModelMapping)</param>
        /// <returns>Hashtable</returns>
        private Hashtable FetchHTfromDb(string key)
        {
            Hashtable ht = new Hashtable();
            //SqlParameter param;
            HttpContext.Current.Trace.Warn("key = " + key);
            HttpContext.Current.Trace.Warn("inside fetch ht ");
            try
            {
                DataSet ds = null;
                DataTable dt = null;

                switch (key)
                {
                    case "BW_BasicIdMapping" :

                        ds = FetchDataFromDatabase("GetCWMappedBasicIds");
                        dt = ds.Tables[0];

                        foreach (DataRow dr in dt.Rows)
                        {
                            if (!ht.ContainsKey(dr["BWBasicId"]))
                                ht.Add(dr["BWBasicId"], dr["CWBasicId"]);
                            //else
                              //  HttpContext.Current.Trace.Warn("Duplicate Key : ", dr["BWBasicId"].ToString());
                        }

                        break;

                    case "BW_ModelMapping" :

                        ds = FetchDataFromDatabase("SP_GetModelMappingNames");
                        dt = ds.Tables[0];

                        foreach (DataRow dr in dt.Rows)
                        {
                            if (!ht.ContainsKey(dr["MaskingName"]))
                                ht.Add(dr["MaskingName"], dr["ID"]);
                        }

                        break;

                    case "BW_MakeMapping" :

                        ds = FetchDataFromDatabase("GetMakeMappingNames");
                        dt = ds.Tables[0];

                        foreach (DataRow dr in dt.Rows)
                        {
                            if (!ht.ContainsKey(dr["MaskingName"]))
                                ht.Add(dr["MaskingName"], dr["ID"]);
                        }

                        break;
                    case "BW_CityMapping":

                        ds = FetchDataFromDatabase("GetCityMappingNames");
                        dt = ds.Tables[0];

                        foreach (DataRow dr in dt.Rows)
                        {
                            //check for key duplication
                            if (!ht.ContainsKey(dr["MaskingName"]))
                            {
                                ht.Add(dr["MaskingName"], dr["ID"]);
                            }
                        }
                        break;

                    case "BW_SeriesMapping" :

                         ds = FetchDataFromDatabase("GetSeriesMappingNames");
                         dt = ds.Tables[0];

                         foreach (DataRow dr in dt.Rows)
                         {
                            if (!ht.ContainsKey(dr["MaskingName"]))
                                ht.Add(dr["MaskingName"], dr["ID"]);
                         }
                        break;

                    case "BW_ModelWiseUsedBikesCount" :

                        ds = FetchDataFromDatabase("GetModelwiseUsedBikeCounts");
                        dt = ds.Tables[0];

                        foreach (DataRow dr in dt.Rows)
                        {
                            if (!ht.ContainsKey(dr["ID"]))
                            {
                                ht.Add(dr["ID"], dr["UsedBikesCount"]);
                            }
                        }
                        break;
                    case "BW_MakeWiseUsedBikesCount" :
                         ds = FetchDataFromDatabase("GetMakewiseUsedBikeCounts");
                         dt = ds.Tables[0];

                            foreach (DataRow dr in dt.Rows)
                            {
                                if (!ht.ContainsKey(dr["ID"]))
                                {
                                    ht.Add(dr["ID"], dr["UsedBikesCount"]);
                                }                          
                            }
                        break;
                    case "BW_TopVersionId":
                        ds = FetchDataFromDatabase("GetModelTopVersionIds");
                        dt = ds.Tables[0];

                        foreach (DataRow dr in dt.Rows)
                        {
                            if (!ht.ContainsKey(dr["ModelMaskingName"]))
                                ht.Add(dr["ModelMaskingName"], dr["TopVersionId"]);
                        }
                        break;

                }
              

                //if (key.Equals("BW_MakeMapping"))
                //{
                //    //DataSet 
                //        ds = FetchDataFromDatabase("GetMakeMappingNames");
                    
                //    DataTable dt = ds.Tables[0];

                //    foreach (DataRow dr in dt.Rows)
                //    {
                //        if (!ht.ContainsKey(dr["MaskingName"]))
                //            ht.Add(dr["MaskingName"], dr["ID"]);
                //        else
                //            HttpContext.Current.Trace.Warn("Duplicate Key : ", dr["MaskingName"].ToString());
                //    }
                //}

                //if (key.Equals("BW_CityMapping"))
                //{
                //    DataSet ds = FetchDataFromDatabase("GetCityMappingNames");

                //    DataTable dt = ds.Tables[0];

                //    foreach (DataRow dr in dt.Rows)
                //    {
                //        //check for key duplication
                //        if (!ht.ContainsKey(dr["MaskingName"]))
                //        {
                //            ht.Add(dr["MaskingName"], dr["ID"]);
                //        }
                //        else
                //        {
                //            HttpContext.Current.Trace.Warn("duplicate key :", dr["MaskingName"].ToString());
                //        }
                //    }
                //}

                //if (key.Equals("BW_SeriesMapping"))
                //{
                //    DataSet ds = FetchDataFromDatabase("GetSeriesMappingNames");

                //    DataTable dt = ds.Tables[0];

                //    foreach (DataRow dr in dt.Rows)
                //    {
                //        if (!ht.ContainsKey(dr["MaskingName"]))
                //            ht.Add(dr["MaskingName"], dr["ID"]);
                //        else
                //            HttpContext.Current.Trace.Warn("duplicate key :", dr["MaskingName"].ToString());
                //    }
                //}

                //if (key.Equals("BW_ModelWiseUsedBikesCount"))
                //{
                //    DataSet ds = FetchDataFromDatabase("GetModelwiseUsedBikeCounts");

                //    DataTable dt = ds.Tables[0];
                    

                //    foreach (DataRow dr in dt.Rows)
                //    {
                //        if (!ht.ContainsKey(dr["ID"]))
                //        {
                //            ht.Add(dr["ID"], dr["UsedBikesCount"]);
                //        }
                //        else
                //        {
                //            HttpContext.Current.Trace.Warn("duplicate key :", dr["ID"].ToString());
                //            //Exception ex = null;
                //            //ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]+ "\r\n Duplicate Key in hash table : " + dr["ID"].ToString());
                //            //objErr.SendMail();
                //        }
                //    }
                //}

                //if (key.Equals("BW_MakeWiseUsedBikesCount"))
                //{
                //    DataSet ds = FetchDataFromDatabase("GetMakewiseUsedBikeCounts");

                //    DataTable dt = ds.Tables[0];


                //    foreach (DataRow dr in dt.Rows)
                //    {
                //        if (!ht.ContainsKey(dr["ID"]))
                //        {
                //            ht.Add(dr["ID"], dr["UsedBikesCount"]);
                //        }
                //        else
                //        {
                //            HttpContext.Current.Trace.Warn("duplicate key :", dr["ID"].ToString());
                //            //Exception ex = null;
                //            //ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]+ "\r\n Duplicate Key in hash table : " + dr["ID"].ToString());
                //            //objErr.SendMail();
                //        }
                //    }
                //}

                //if (key.Equals("BW_TopVersionId"))
                //{
                //    DataSet ds = FetchDataFromDatabase("GetModelTopVersionIds");

                //    DataTable dt = ds.Tables[0];

                //    foreach (DataRow dr in dt.Rows)
                //    {
                //        if (!ht.ContainsKey(dr["ModelMaskingName"]))
                //            ht.Add(dr["ModelMaskingName"], dr["TopVersionId"]);
                //        else
                //            HttpContext.Current.Trace.Warn("duplicate key :", dr["MaskingName"].ToString());
                //    }
                //}
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn(ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]
                    + "\r\nMemcacheKey : " + key
                    + "\r\nMemcache Object Type : " + (_mc.Get(key) == null ? "null" : _mc.Get(key).GetType().ToString())
                    + "\r\nRequest Type: " + HttpContext.Current.Request.RequestType);
                objErr.SendMail();
            }
            return ht;
        }   // End of FetchDSfromDb        


        /// <summary>
        /// Get the hashtable from the Memcached Key. If the corresponding hashtable does not exist, create one in Memcached
        /// </summary>
        /// <param name="key">The Memcached Ket</param>
        /// <param name="duration">The time Duration for which the object is created</param>
        /// <returns>Hashtable</returns>
        public Hashtable GetHashTable(string key)
        {            
            Hashtable ht = new Hashtable();       
            try
            {
                if (_isMemcachedUsed)
                {
                    ht = (Hashtable)_mc.Get(key);
                    HttpContext.Current.Trace.Warn("Read " + key + " from Memcached");
                    HttpContext.Current.Trace.Warn(" ht :" + (ht == null));
                    if (ht == null)
                    {
                        HttpContext.Current.Trace.Warn(key + " not found in Memcached");
                        if (_mc.Store(StoreMode.Add, key + "_lock", "lock", DateTime.Now.AddSeconds(60)))
                        {
                            ht = FetchHTfromDb(key);
                            // Insert the DataSet into the Cache
                            _mc.Store(StoreMode.Add, key, ht,
                                DateTime.Now.AddMinutes(double.Parse(ConfigurationManager.AppSettings.Get("MemcacheTimespan"))));
                            HttpContext.Current.Trace.Warn("Inserted " + key + " into Memcached");
                            _mc.Remove(key + "_lock");
                        }
                        else
                        {
                            ht = FetchHTfromDb(key);
                        }
                    }
                }
                else
                {
                    ht = FetchHTfromDb(key);
                }
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn(ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]
                    + "\r\nMemcacheKey : " + key
                    + "\r\nMemcache Object Type : " + (_mc.Get(key) == null ? "null" : _mc.Get(key).GetType().ToString())
                    + "\r\nRequest Type: " + HttpContext.Current.Request.RequestType);
                objErr.SendMail();
            }
            finally
            {
                // If the DataSet is null, finally fetch it from the Database
                if (ht == null)
                {
                    ht = FetchHTfromDb(key);
                }
            }
            return ht;
        }   // End of GetHashTable 
        #endregion

    }   // class
}   // namespace