using Carwale.Interfaces.UserProfiling;
using Carwale.Notifications;
using Carwale.Notifications.Logs;
using Dapper;
using System;
using System.Data;
using System.Linq;

namespace Carwale.DAL.UserProfiling
{
    public class UserProfilingRepo : RepositoryBase, IUserProfilingRepo
    {
        public bool UserProfilingStatus(int platformId)
        {            
            try
            {
                var param = new DynamicParameters();
                param.Add("v_PlatformId", platformId);

                using (var con = EsMySqlReadConnection)
                {
                    LogLiveSps.LogSpInGrayLog("UserProfilingStatus");
                    return con.Query<bool>("UserProfilingStatus", param, commandType: CommandType.StoredProcedure).Single();                    
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return false;
            }            
        }
    }
}
