using BikewaleOpr.Entities;
using BikeWaleOpr.Common;
using System;
using System.Collections.Generic;
using System.Data;
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
            Database db = null;
            try
            {
                using (SqlCommand cmd = new SqlCommand("GetAppVersions"))
                {
                    db = new Database();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@AppType", Convert.ToInt16(appType));

                    using (SqlDataReader reader = db.SelectQry(cmd))
                    {
                        if (reader != null && reader.HasRows)
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
            finally
            {
                if (db != null)
                    db.CloseConnection();
                db = null;
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
            Database db = null;
            try
            {
                using (SqlCommand cmd = new SqlCommand("AddOrUpdateAppVersion"))
                {
                    db = new Database();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@AppType", Convert.ToInt16(entity.AppType));
                    cmd.Parameters.AddWithValue("@AppVersionId", entity.Id);
                    cmd.Parameters.AddWithValue("@IsSupported", entity.IsSupported);
                    cmd.Parameters.AddWithValue("@IsLatest", entity.IsLatest);
                    cmd.Parameters.AddWithValue("@Description", entity.Description);
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    isSaved = db.InsertQry(cmd);
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "ManageAppVersion.SaveAppVersion");
                objErr.SendMail();
            }
            finally
            {
                if (db != null)
                    db.CloseConnection();
                db = null;
            }
            return isSaved;
        }
    }
}