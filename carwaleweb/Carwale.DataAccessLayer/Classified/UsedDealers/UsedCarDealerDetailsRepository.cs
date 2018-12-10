using Carwale.Entity.Classified.UsedDealers;
using Carwale.Notifications.Logs;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;

namespace Carwale.DAL.Classified.UsedDealers
{
    public class UsedCarDealerDetailsRepository : RepositoryBase
    {
        public UsedCarDealerDetails GetUsedDealerDetails(string dealerId)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("v_dealerid", dealerId);
                using (var con = ClassifiedMySqlReadConnection)
                {
                    var list = con.Query<UsedCarDealerDetails>("getdealerdetails_v17_6_1", parameters, commandType: CommandType.StoredProcedure).AsList();
                    LogLiveSps.LogSpInGrayLog("getdealerdetails_v17_6_1");

                    if (list != null && list.Count > 0)
                    {
                        return list[0];
                    }
                }
            }
            catch (Exception err)
            {
                Logger.LogException(err);
            }

            return new UsedCarDealerDetails();
        }
    }
}
