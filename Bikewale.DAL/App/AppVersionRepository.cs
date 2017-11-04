using Bikewale.Entities.App;
using Bikewale.Interfaces.App;
using Bikewale.Notifications;
using MySql.CoreDAL;
using System;
using System.Data;
using System.Data.Common;

namespace Bikewale.DAL.App
{
    /// <summary>
    /// Author      :   Sumit Kate
    /// Description :   APP Version repository
    /// Created On  :   07 Dec 2015
    /// </summary>
    public class AppVersionRepository : IAppVersion
    {
        /// <summary>
        /// Author      :   Sumit Kate
        /// Description :   To check whether APP Versions is supported and Latest
        /// Created On  :   07 Dec 2015        
        /// </summary>
        /// <param name="appVersion">APP Version Id</param>
        /// <param name="sourceId">3 - Android / 4 - iOS</param>        
        /// <returns></returns>
        public AppVersion CheckVersionStatus(uint appVersion, uint sourceId)
        {
            bool isSupported = false, isLatest = false;
            AppVersion objAppVersion = null;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("checkversionstatusforapp"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_appversionid", DbType.Int32, appVersion));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_sourceid", DbType.Byte, sourceId));

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null)
                        {
                            while (dr.Read())
                            {
                                isSupported = Convert.ToBoolean(dr["IsSupported"].ToString());
                                isLatest = Convert.ToBoolean(dr["IsLatest"].ToString());
                            }
                            dr.Close();
                        }
                    }
                }
            }
            catch (Exception err)
            {
                ErrorClass.LogError(err, "CheckVersionStatus");
            }
            finally
            {
                objAppVersion = new AppVersion();
                objAppVersion.Id = appVersion;
                objAppVersion.IsLatest = isLatest;
                objAppVersion.IsSupported = isSupported;
            }
            return objAppVersion;
        }

    }
}
