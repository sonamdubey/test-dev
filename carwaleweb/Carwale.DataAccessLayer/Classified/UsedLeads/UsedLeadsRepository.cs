using Carwale.Entity.Classified.UsedLeads;
using Carwale.Interfaces.Classified.Leads;
using Carwale.Notifications;
using Carwale.Notifications.Logs;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Carwale.DAL.Classified.UsedLeads
{
    public class UsedLeadsRepository : RepositoryBase, IUsedLeadsRepository
    {
        public DealerLeadsCount GetDealerLeadsCount(int dealerId, int month, int year)
        {
            DealerLeadsCount leadCount = new DealerLeadsCount();
            var param = new DynamicParameters();
            param.Add("v_dealerid", dealerId, DbType.Int32);
            param.Add("v_month", month, DbType.Int32);
            param.Add("v_year", year, DbType.Int32);
            param.Add("v_verifiedleadcount", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("v_unverifiedleadcount", dbType: DbType.Int32, direction: ParameterDirection.Output);
            try
            {
                using (var con = ClassifiedMySqlReadConnection)
                {
                    con.Execute("getuseddealersleadscount", param, commandType: CommandType.StoredProcedure);
                    LogLiveSps.LogSpInGrayLog("getuseddealersleadscount");
                    leadCount.VerifiedLeadCount = param.Get<int>("v_verifiedleadcount");
                    leadCount.UnverifiedLeadCount = param.Get<int>("v_unverifiedleadcount");
                }
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, "UsedLeadsRepository.GetDealerLeadsCount");
                objErr.SendMail();
                throw;
            }
            return leadCount;
        }
    }
}
