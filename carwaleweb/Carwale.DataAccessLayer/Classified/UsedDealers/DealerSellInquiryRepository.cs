using Carwale.Entity.Classified.UsedDealers;
using Carwale.Notifications;
using Carwale.Notifications.Logs;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Carwale.DAL.Classified.UsedDealers
{
    public class DealerSellInquiryRepository:RepositoryBase
    {
        public List<DealerSellInquiry> GetSellInquiriesCarMakeForDealer(int dealerId)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("v_dealerid", dealerId);
                using (var con = ClassifiedMySqlReadConnection)
                {
                    LogLiveSps.LogSpInGrayLog("getsellinquiriescarmakefordealer");
                    return con.Query<DealerSellInquiry>("getsellinquiriescarmakefordealer", parameters, commandType: CommandType.StoredProcedure).ToList();
                }
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, "DealerSellInquiryRepository");
                objErr.SendMail();
            }
            return new List<DealerSellInquiry>();
        }
    }
}
