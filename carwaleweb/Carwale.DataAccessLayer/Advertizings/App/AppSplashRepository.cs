using Carwale.DAL.CoreDAL;
using Carwale.Entity.Advertizings.Apps;
using Carwale.Interfaces.Advertizings.App;
using Carwale.Notifications;
using Carwale.Notifications.Logs;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Carwale.DAL.Advertizings.App
{
    public class AppSplashRepository : RepositoryBase, IAppSplashRepository
    {
        public List<SplashScreenBanner> GetSpalshSreenBanner(int platformId, int applicationId)
        {
            try
            {
                using (var con = EsMySqlReadConnection)
                {
                    var param = new DynamicParameters();
                    param.Add("v_PlatformId", platformId);
                    param.Add("v_ApplicationId", applicationId);
                    LogLiveSps.LogSpInGrayLog("GetSplashScreens");
                    var splashScreens = con.Query<SplashScreenBanner>("GetSplashScreens_v18_8_1", param, commandType: CommandType.StoredProcedure);

                    if (splashScreens != null && splashScreens.AsList().Count > 0)
                        return splashScreens.AsList();
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "AppSplashRepository.GetSpalshSreenBanner()");
                objErr.LogException();
            }
            return null;
        }
    }
}
