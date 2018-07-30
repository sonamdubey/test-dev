using Bikewale.Notifications;
using Enyim.Caching;
using Enyim.Caching.Memcached;
using MySql.CoreDAL;
using System;
using System.Collections;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

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
            DbParameter  param;

            try
            {
                if (key.Equals("BW_BikeMakes"))
                {
                    param =   DbFactory.GetDbParam("par_condition", DbType.String, 10, "Make") ;

                    ds = FetchDataFromDatabase("GetMakeModelVersion", param);
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.Cache.Memcache.BWMemcache.FetchDSfromDb"
                    + "\r\nMemcacheKey : " + key
                    + "\r\nMemcache Object Type : " + (_mc.Get(key) == null ? "null" : _mc.Get(key).GetType().ToString()));
                
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
                    ds = FetchDataFromDatabase("GetNewBikeLaunches", param);
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.Cache.Memcache.BWMemcache.FetchDSfromDb"
                    + "\r\nMemcacheKey : " + key
                    + "\r\nMemcache Object Type : " + (_mc.Get(key) == null ? "null" : _mc.Get(key).GetType().ToString()));
                
            }
            return ds;
        }   // End of FetchDSfromDb


        /// <summary>
        /// PopulateWhere to get data from database for the particular sp provided.
        /// </summary>
        /// <param name="spName">Name of the sp from which data is required.</param>
        /// <param name="param">If any sql parameters to be passed on to sp. (optional)</param>
        /// <returns>Function returns dataset containing required data.</returns>
        private DataSet FetchDataFromDatabase(string spName, DbParameter param = null)
        {
            DataSet ds = null;

            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = spName;

                    if (param != null)
                    {
                        cmd.Parameters.Add(DbFactory.GetDbParam(param.ParameterName, DbType.String, param.Value));
                    }

                    // Fetch the data from the database into DataSet
                    ds = MySqlDatabase.SelectAdapterQuery(cmd, ConnectionType.ReadOnly);
                }
            }
            catch (SqlException ex)
            {
                ErrorClass.LogError(ex, "Bikewale.Cache.Memcache.BWMemcache.FetchDataFromDatabase");
                
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.Cache.Memcache.BWMemcache.FetchDataFromDatabase");
                
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
                ErrorClass.LogError(ex, "Bikewale.Cache.Memcache.BWMemcache.GetDataSet"
                    + "\r\nMemcacheKey : " + key
                    + "\r\nMemcache Object Type : " + (_mc.Get(key) == null ? "null" : _mc.Get(key).GetType().ToString()));
                
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
                ErrorClass.LogError(ex, "Bikewale.Cache.Memcache.BWMemcache.GetDataSet"//HttpContext.Current.Request.ServerVariables["URL"]
                    + "\r\nMemcacheKey : " + key
                    + "\r\nMemcache Object Type : " + (_mc.Get(key) == null ? "null" : _mc.Get(key).GetType().ToString()));
                
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

                        ds = FetchDataFromDatabase("getcwmappedbasicids");
                        dt = ds.Tables[0];

                        foreach (DataRow dr in dt.Rows)
                        {
                            if (!ht.ContainsKey(dr["BWBasicId"]))
                                ht.Add(dr["BWBasicId"], dr["CWBasicId"]);
                        }

                        break;

                    case "BW_ModelMapping":

                        ds = FetchDataFromDatabase("sp_getmodelmappingnames");
                        dt = ds.Tables[0];

                        foreach (DataRow dr in dt.Rows)
                        {
                            if (!ht.ContainsKey(dr["MaskingName"]))
                                ht.Add(dr["MaskingName"], dr["ID"]);
                        }

                        break;

                    case "BW_MakeMapping":

                        ds = FetchDataFromDatabase("getmakemappingnames");
                        dt = ds.Tables[0];

                        foreach (DataRow dr in dt.Rows)
                        {
                            if (!ht.ContainsKey(dr["MaskingName"]))
                                ht.Add(dr["MaskingName"], dr["ID"]);
                        }

                        break;
                    case "BW_CityMapping":

                        ds = FetchDataFromDatabase("getcitymappingnames");
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

                        ds = FetchDataFromDatabase("getseriesmappingnames");
                        dt = ds.Tables[0];

                        foreach (DataRow dr in dt.Rows)
                        {
                            if (!ht.ContainsKey(dr["MaskingName"]))
                                ht.Add(dr["MaskingName"], dr["ID"]);
                        }
                        break;

                    case "BW_ModelWiseUsedBikesCount":

                        ds = FetchDataFromDatabase("getmodelwiseusedbikecounts");
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
                        ds = FetchDataFromDatabase("getmakewiseusedbikecounts");
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
                        ds = FetchDataFromDatabase("getmodeltopversionids");
                        dt = ds.Tables[0];

                        foreach (DataRow dr in dt.Rows)
                        {
                            if (!ht.ContainsKey(dr["ModelMaskingName"]))
                                ht.Add(dr["ModelMaskingName"], dr["TopVersionId"]);
                        }
                        break;
                    case "BW_AuthorMapping":
                        ds = FetchDataFromDatabase("getauthorsmappingname");
                        if(ds != null && ds.Tables != null)
                        {
                            dt = ds.Tables[0];

                            foreach (DataRow dr in dt.Rows)
                            {
                                if (!ht.ContainsKey(dr["MaskingName"]))
                                    ht.Add(dr["MaskingName"], dr["AuthorId"]);
                            }
                        }
                        break;
                }


            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.Cache.Memcache.BWMemcache.GetDataSet.FeatureHTfromDB"
                    + "\r\nMemcacheKey : " + key
                    + "\r\nMemcache Object Type : " + (_mc.Get(key) == null ? "null" : _mc.Get(key).GetType().ToString()));
                
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
                ErrorClass.LogError(ex, ""
                    + "\r\nMemcacheKey : " + key
                    + "\r\nMemcache Object Type : " + (_mc.Get(key) == null ? "null" : _mc.Get(key).GetType().ToString())
                    );
                
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
