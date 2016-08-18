using BikewaleOpr.Entities;
using BikeWaleOpr.Common;
using MySql.CoreDAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace BikewaleOpr.Common
{
    /// <summary>
    /// Author      :   Sumit Kate on 01 Feb 2016
    /// Descrpition :   Manage App Version
    /// </summary>
    public class ManageAppVersion
    {
        /// <summary>
        /// Get the app versions by apptype
        /// </summary>
        /// <param name="appType"></param>
        /// <returns></returns>
        public IEnumerable<AppVersionEntity> GetAppVersions(AppEnum appType)
        {
            IList<AppVersionEntity> AppVersions = null;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("getappversions"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_apptype", DbType.Byte, Convert.ToInt16(appType)));

                    using (IDataReader reader = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (reader != null)
                        {
                            AppVersions = new List<AppVersionEntity>();
                            while (reader.Read())
                            {
                                AppVersions.Add(new AppVersionEntity()
                                {
                                    Id = Convert.ToInt32(reader["AppVersionId"]),
                                    AppType = appType,
                                    Description = Convert.ToString(reader["Description"]),
                                    IsLatest = Convert.ToBoolean(reader["IsLatest"]),
                                    IsSupported = Convert.ToBoolean(reader["IsSupported"])
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "ManageAppVersion.GetAppVersions");
                objErr.SendMail();
            }

            return AppVersions;
        }

        /// <summary>
        /// Inserts or updates the app version
        /// Author  :   Sumit Kate
        /// Description :   remove the cache if exists
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public bool SaveAppVersion(AppVersionEntity entity, String userId)
        {
            bool isSaved = false;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("addorupdateappversion"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_apptype", DbType.Byte, Convert.ToInt16(entity.AppType)));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_appversionid", DbType.Int32, entity.Id));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_issupported", DbType.Boolean, entity.IsSupported));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_islatest", DbType.Boolean, entity.IsLatest));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_description", DbType.String, 50, entity.Description));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_userid", DbType.Int32, userId));

                    isSaved = MySqlDatabase.InsertQuery(cmd, ConnectionType.MasterDatabase);

                    Enyim.Caching.MemcachedClient _mc1 = new Enyim.Caching.MemcachedClient("memcached");
                    string cacheKey = String.Format("BW_AppVersion_{0}_Src_{1}", entity.Id, Convert.ToInt16(entity.AppType));
                    var cacheObject = _mc1.Get(cacheKey);
                    if (cacheObject != null)
                    {
                        _mc1.Remove(cacheKey);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "ManageAppVersion.SaveAppVersion");
                objErr.SendMail();
            }

            return isSaved;
        }
    }
}