using Carwale.Entity.Finance;
using Carwale.Interfaces.Finance;
using Carwale.Notifications;
using Carwale.Notifications.Logs;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;

namespace Carwale.DAL.CarFinance
{
    public class FinanceLinkDataRepository : RepositoryBase, IFinanceLinkDataRepository
    {
        public FinanceLinkData GetUrlData(int platformId, int screenId)
        {
            var linkData = new List<FinanceLinkData>();
            var param = new DynamicParameters();
            try
            {                
                param.Add("v_platformId", platformId);
                param.Add("v_screenId", screenId);

                using (var con = EsMySqlReadConnection)
                {
                    linkData = con.Query<FinanceLinkData>("CW_GetFinanceLinkData", param, commandType: CommandType.StoredProcedure).AsList();
                    LogLiveSps.LogSpInGrayLog("CW_GetFinanceLinkData");
                }
                if (linkData.Count > 0)
                    return linkData[0]; 
            }
            catch (Exception ex)
            {
                var obj = new ExceptionHandler(ex, "GetUrlData()");
                obj.LogException();
            }
            return null;
        }
    }
}
