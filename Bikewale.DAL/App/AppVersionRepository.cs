using Bikewale.CoreDAL;
using Bikewale.Notifications;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Bikewale.Interfaces.App;
using Bikewale.Entities.App;
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

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd))
                    {
                        if (dr!=null)
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
                ErrorClass objErr = new ErrorClass(err, "CheckVersionStatus");
                objErr.SendMail();
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
