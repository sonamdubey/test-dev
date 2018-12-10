using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Carwale.Interfaces;
using System.Data.SqlClient;
using System.Data;
using Carwale.DAL.CoreDAL;
using Carwale.Notifications;
using Carwale.Entity.Security;
using Dapper;
using Carwale.Notifications.Logs;

namespace Carwale.DAL.Security
{
    public class SecurityRepository<T> : RepositoryBase, ISecurityRepository<T>
    {
        public string GetPassword(string username)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("v_UserName", username, DbType.String);
                param.Add("v_PassWord", dbType: DbType.String, direction: ParameterDirection.Output);
                using (var con = ClassifiedMySqlReadConnection)
                {
                    con.Execute("cwmasterdb.GetPassword", param, commandType: CommandType.StoredProcedure);
                    return param.Get<string>("v_PassWord");
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return "";
        }

        public bool IsValidSource(string SourceId, string CWK)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("v_SourceId", SourceId, DbType.Int32);
                param.Add("v_CWK", CWK, DbType.String);
                param.Add("v_IsValid", dbType: DbType.Int32, direction: ParameterDirection.Output);
                using (var con = ClassifiedMySqlReadConnection)
                {
                    con.Execute("cwmasterdb.IsValidSource", param, commandType: CommandType.StoredProcedure);
                    return param.Get<int>("v_IsValid") > 0;
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return false;
        }
    }
}
