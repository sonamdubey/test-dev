using Carwale.Interfaces.Blocking;
using Carwale.Notifications.Logs;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;

namespace Carwale.DAL.Blocking
{
    public class BlockMobileRepository : RepositoryBase, IBlockMobileRepository
    {
        public void BlockMobileNos(IEnumerable<string> mobiles, string reasonForBlocking, string addedBy)
        {
            var param = new DynamicParameters();
            param.Add("v_mobiles", string.Join(",", mobiles), DbType.String);
            param.Add("v_reason", reasonForBlocking, DbType.String);
            param.Add("v_addedby", addedBy, DbType.String);

            using (var con = ClassifiedMySqlMasterConnection)
            {
                con.Execute("cwmasterdb.BlockMobileNos", param, commandType: CommandType.StoredProcedure);
            }
        }

        public void UnblockMobileNos(IEnumerable<string> mobiles)
        {
            var param = new DynamicParameters();
            param.Add("v_mobiles", string.Join(",", mobiles), DbType.String);

            using (var con = ClassifiedMySqlMasterConnection)
            {
                con.Execute("cwmasterdb.UnblockMobileNos", param, commandType: CommandType.StoredProcedure);
            }
        }

        public bool IsNumberBlocked(string number)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("v_Number", number, DbType.String);
                param.Add("v_IsBlocked", dbType: DbType.Int16, direction: ParameterDirection.Output);

                using (var con = ClassifiedMySqlReadConnection)
                {
                    con.Query("cwmasterdb.IsNumberBlocked", param, commandType: CommandType.StoredProcedure);
                    return param.Get<short>("v_IsBlocked") == 1;
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
