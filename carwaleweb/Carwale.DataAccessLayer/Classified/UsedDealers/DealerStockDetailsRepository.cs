using Carwale.Notifications;
using Carwale.Notifications.Logs;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Carwale.DAL.Classified.UsedDealers
{
    public class DealerStockDetailsRepository : RepositoryBase
    {

        public int GetActiveDealerStockCount(int dealerId)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("v_dealerid", dealerId);
                using (var con = ClassifiedMySqlReadConnection)
                {
                    LogLiveSps.LogSpInGrayLog("getlivelistingcountforactivedealer");
                    return con.Query<int>("getlivelistingcountforactivedealer", parameters, commandType: CommandType.StoredProcedure).First();
                }
            }
            catch (Exception err)
            {
                HttpContext.Current.Trace.Warn("DealerStockDetailsRepository : " + err.Message);
                ErrorClass objErr = new ErrorClass(err, "DealerStockDetailsRepository");
                objErr.SendMail();
            }
            return 0;
        }
    }
}
