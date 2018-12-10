using Carwale.Entity.Classified.UsedDealers;
using Carwale.Notifications;
using Carwale.Notifications.Logs;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;

namespace Carwale.DAL.Classified.UsedDealers
{
    public class DealersForCityRepository : RepositoryBase
    {
        public List<DealersForCity> GetDealerForCity(long cityid)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("v_cityid", cityid);
                using (var con = ClassifiedMySqlReadConnection)
                {
                    LogLiveSps.LogSpInGrayLog("getdealersforcity_v16_12_7");
                    return con.Query<DealersForCity>("getdealersforcity_v16_12_7", parameters, commandType: CommandType.StoredProcedure).AsList();
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, ex.Message);
                objErr.LogException();
                return new List<DealersForCity>();
            }
        }

    }
}
