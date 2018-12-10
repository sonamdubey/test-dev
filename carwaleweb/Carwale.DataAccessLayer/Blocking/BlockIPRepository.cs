using Carwale.Interfaces.Blocking;
using Carwale.Notifications.Logs;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;

namespace Carwale.DAL.Blocking
{
    public class BlockIPRepository : RepositoryBase, IBlockIPRepository
    {
        public void BlockIpAddresses(IEnumerable<string> ipAddresses, string reasonForBlocking, string addedBy)
        {
            var param = new DynamicParameters();
            param.Add("v_ips", string.Join(",", ipAddresses), DbType.String);
            param.Add("v_reason", reasonForBlocking, DbType.String);
            param.Add("v_addedby", addedBy, DbType.String);

            using (var con = ClassifiedMySqlMasterConnection)
            {
                con.Execute("BlockIpAddresses", param, commandType: CommandType.StoredProcedure);
            }
        }

        public void UnblockIpAddresses(IEnumerable<string> ipAddresses)
        {
            var param = new DynamicParameters();
            param.Add("v_ips", string.Join(",", ipAddresses), DbType.String);

            using (var con = ClassifiedMySqlMasterConnection)
            {
                con.Execute("UnblockIpAddresses", param, commandType: CommandType.StoredProcedure);
            }
        }

        public bool IsIpBlocked(string ip)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("v_ip", ip, DbType.String);
                param.Add("v_IsBlocked", dbType: DbType.Int32, direction: ParameterDirection.Output);

                using (var con = ClassifiedMySqlReadConnection)
                {
                    con.Execute("IsIpBlocked", param, commandType: CommandType.StoredProcedure);
                    return param.Get<int>("v_IsBlocked") == 1;
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
