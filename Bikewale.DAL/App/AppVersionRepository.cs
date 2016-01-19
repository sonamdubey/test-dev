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
            Database db = null;
            try
            {
                using (SqlCommand cmd = new SqlCommand("CheckVersionStatusForApp"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@AppVersionId", SqlDbType.Int).Value = appVersion;
                    cmd.Parameters.Add("@SourceId", SqlDbType.TinyInt).Value = sourceId;

                    db = new Database();
                    using (SqlDataReader dr = db.SelectQry(cmd))
                    {
                        while (dr.Read())
                        {
                            isSupported = Convert.ToBoolean(dr["IsSupported"].ToString());
                            isLatest = Convert.ToBoolean(dr["IsLatest"].ToString());
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
                if (db != null)
                    db.CloseConnection();
                objAppVersion = new AppVersion();
                objAppVersion.Id = appVersion;
                objAppVersion.IsLatest = isLatest;
                objAppVersion.IsSupported = isSupported;
            }
            return objAppVersion;
        }

    }
}
