using Carwale.Interfaces.Logs;
using Dapper;
using System.Data;

namespace Carwale.DAL.Logs
{
    public class LoggingRepository : RepositoryBase, ILoggingRepository
    {
        public void LogRequestBody(string jsonData, string requestMethod)
        {
            var param = new DynamicParameters();
            param.Add("v_jsonData", jsonData, DbType.String);
            param.Add("v_method", requestMethod, DbType.String);

            using (var con = ClassifiedMySqlMasterConnection)
            {
                con.Execute("LogRequest", param, commandType: CommandType.StoredProcedure);
            }
        }
    }
}
