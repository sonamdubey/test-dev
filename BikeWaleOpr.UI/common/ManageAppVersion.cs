using BikewaleOpr.Entities;
using BikeWaleOpr.Common;
using BikeWaleOPR.Utilities;
using MySql.CoreDAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

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
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_apptype", DbParamTypeMapper.GetInstance[SqlDbType.TinyInt], Convert.ToInt16(appType)));

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
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_apptype", DbParamTypeMapper.GetInstance[SqlDbType.TinyInt], Convert.ToInt16(entity.AppType)));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_appversionid", DbParamTypeMapper.GetInstance[SqlDbType.Int], entity.Id));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_issupported", DbParamTypeMapper.GetInstance[SqlDbType.Bit], entity.IsSupported));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_islatest", DbParamTypeMapper.GetInstance[SqlDbType.Bit], entity.IsLatest));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_description", DbParamTypeMapper.GetInstance[SqlDbType.VarChar], 50, entity.Description));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_userid", DbParamTypeMapper.GetInstance[SqlDbType.Int], userId));

                    isSaved = MySqlDatabase.InsertQuery(cmd, ConnectionType.MasterDatabase);
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