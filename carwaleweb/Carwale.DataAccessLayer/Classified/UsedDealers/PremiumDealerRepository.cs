using Carwale.Entity.Classified.UsedDealers;
using Carwale.Notifications;
using Carwale.Notifications.Logs;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;

namespace Carwale.DAL.Classified.UsedDealers
{
   public class PremiumDealerRepository : RepositoryBase
    {
        public List<PremiumDealer> GetDealerForCity(int dealerID)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("v_dealerid", dealerID);
                using (var con = ClassifiedMySqlReadConnection)
                {
                    LogLiveSps.LogSpInGrayLog("GetPremiumDealerStock_v15_8_1");
                    return con.Query<PremiumDealer>("GetPremiumDealerStock_v15_8_1", parameters, commandType: CommandType.StoredProcedure).AsList();
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, ex.Message);
                objErr.LogException();
                return new List<PremiumDealer>();
            }
        }
    }
}
