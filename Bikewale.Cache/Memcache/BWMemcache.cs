﻿using Bikewale.Notifications;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Enyim.Caching;
using Enyim.Caching.Memcached;
using System.Data;
using System.Data.SqlClient;
using Bikewale.CoreDAL;

namespace Bikewale.Cache.Memcache
{
    /// <summary>
    /// Created By : Sumit Kate
    /// Created on : 10 march 2016
    /// Description : Bikewale.UI.Memcache is used.
    /// </summary>
    public class BWMemcache
    {
        bool _isMemcachedUsed;
        protected static MemcachedClient _mc = null;

        public BWMemcache()
        {
            // Check if memchache is to be used or not.
            _isMemcachedUsed = bool.Parse(Bikewale.Utility.BWConfiguration.Instance.IsMemcachedUsed);
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
                ErrorClass objErr = new ErrorClass(ex, "Bikewale.Cache.Memcache.BWMemcache.FetchDSfromDb"
                    + "\r\nMemcacheKey : " + key
                    + "\r\nMemcache Object Type : " + (_mc.Get(key) == null ? "null" : _mc.Get(key).GetType().ToString()));
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
                ErrorClass objErr = new ErrorClass(ex, "Bikewale.Cache.Memcache.BWMemcache.FetchDSfromDb"
                    + "\r\nMemcacheKey : " + key
                    + "\r\nMemcache Object Type : " + (_mc.Get(key) == null ? "null" : _mc.Get(key).GetType().ToString()));
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
                    }

                    // Fetch the data from the database into DataSet
                    ds = db.SelectAdaptQry(cmd);
                }
            }
            catch (SqlException ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Bikewale.Cache.Memcache.BWMemcache.FetchDataFromDatabase");
                objErr.SendMail();
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Bikewale.Cache.Memcache.BWMemcache.FetchDataFromDatabase");
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
                    if (ds == null)
                    {
                        if (_mc.Store(StoreMode.Add, key + "_lock", "lock", DateTime.Now.AddSeconds(60)))
                        {
                            ds = FetchDSfromDb(key);
                            // Insert the DataSet into the Cache
                            bool isCreated = _mc.Store(StoreMode.Add, key, ds,
                                DateTime.Now.AddMinutes(double.Parse(Bikewale.Utility.BWConfiguration.Instance.MemcacheTimespan)));
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
                ErrorClass objErr = new ErrorClass(ex, "Bikewale.Cache.Memcache.BWMemcache.GetDataSet"
                    + "\r\nMemcacheKey : " + key
                    + "\r\nMemcache Object Type : " + (_mc.Get(key) == null ? "null" : _mc.Get(key).GetType().ToString()));
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
        public DataSet GetDataSet(string key, SqlParameter parameters)
        {
            DataSet ds = new DataSet();
            try
            {
                if (_isMemcachedUsed)
                {
                    ds = (DataSet)_mc.Get(key);
                    if (ds == null)
                    {
                        if (_mc.Store(StoreMode.Add, key + "_lock", "lock", DateTime.Now.AddSeconds(60)))
                        {
                            ds = FetchDSfromDb(key, parameters);
                            // Insert the DataSet into the Cache
                            _mc.Store(StoreMode.Add, key, ds,
                                DateTime.Now.AddMinutes(double.Parse(Bikewale.Utility.BWConfiguration.Instance.MemcacheTimespan)));
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
                ErrorClass objErr = new ErrorClass(ex, "Bikewale.Cache.Memcache.BWMemcache.GetDataSet"//HttpContext.Current.Request.ServerVariables["URL"]
                    + "\r\nMemcacheKey : " + key
                    + "\r\nMemcache Object Type : " + (_mc.Get(key) == null ? "null" : _mc.Get(key).GetType().ToString()));
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
            try
            {
                DataSet ds = null;
                DataTable dt = null;

                switch (key)
                {
                    case "BW_BasicIdMapping":

                        ds = FetchDataFromDatabase("GetCWMappedBasicIds");
                        dt = ds.Tables[0];

                        foreach (DataRow dr in dt.Rows)
                        {
                            if (!ht.ContainsKey(dr["BWBasicId"]))
                                ht.Add(dr["BWBasicId"], dr["CWBasicId"]);
                        }

                        break;

                    case "BW_ModelMapping":

                        ds = FetchDataFromDatabase("SP_GetModelMappingNames");
                        dt = ds.Tables[0];

                        foreach (DataRow dr in dt.Rows)
                        {
                            if (!ht.ContainsKey(dr["MaskingName"]))
                                ht.Add(dr["MaskingName"], dr["ID"]);
                        }

                        break;

                    case "BW_MakeMapping":

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
                            if (!ht.ContainsKey(dr["CityMaskingName"]))
                            {
                                ht.Add(dr["CityMaskingName"], dr["ID"]);
                            }
                        }
                        break;

                    case "BW_SeriesMapping":

                        ds = FetchDataFromDatabase("GetSeriesMappingNames");
                        dt = ds.Tables[0];

                        foreach (DataRow dr in dt.Rows)
                        {
                            if (!ht.ContainsKey(dr["MaskingName"]))
                                ht.Add(dr["MaskingName"], dr["ID"]);
                        }
                        break;

                    case "BW_ModelWiseUsedBikesCount":

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
                    case "BW_MakeWiseUsedBikesCount":
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


            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Bikewale.Cache.Memcache.BWMemcache.GetDataSet.FeatureHTfromDB"
                    + "\r\nMemcacheKey : " + key
                    + "\r\nMemcache Object Type : " + (_mc.Get(key) == null ? "null" : _mc.Get(key).GetType().ToString()));
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
                    
                    if (ht == null)
                    {
                        
                        if (_mc.Store(StoreMode.Add, key + "_lock", "lock", DateTime.Now.AddSeconds(60)))
                        {
                            ht = FetchHTfromDb(key);
                            // Insert the DataSet into the Cache
                            _mc.Store(StoreMode.Add, key, ht,
                                DateTime.Now.AddMinutes(double.Parse(Bikewale.Utility.BWConfiguration.Instance.MemcacheTimespan)));
                        
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
                ErrorClass objErr = new ErrorClass(ex, ""
                    + "\r\nMemcacheKey : " + key
                    + "\r\nMemcache Object Type : " + (_mc.Get(key) == null ? "null" : _mc.Get(key).GetType().ToString())
                    );
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

    } 
}
