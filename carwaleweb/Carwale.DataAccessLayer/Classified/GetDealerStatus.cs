using Carwale.Entity.Stock;
using Carwale.Interfaces.Classified;
using Carwale.Notifications;
using Dapper;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Linq;
using Carwale.DAL.CoreDAL;
using Carwale.Notifications.Logs;

namespace Carwale.DAL.Classified
{
    public class GetDealerStatus : RepositoryBase, IGetDealerStatus
    {
        public DealerStatusEntity GetUsedCarDealerStatus(int? dealerId)
        {
            DealerStatusEntity dealerStatus = new DealerStatusEntity();
            try
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("v_dealerid", dealerId, DbType.Int32);
                param.Add("v_isdealermissing", 0, DbType.Int32, direction: ParameterDirection.Output);
                param.Add("v_ispackagemissing", 0, DbType.Int32, direction: ParameterDirection.Output);
                param.Add("v_packagestartdate", null, DbType.DateTime, direction: ParameterDirection.Output);
                param.Add("v_packageenddate", null, DbType.DateTime, direction: ParameterDirection.Output);
                param.Add("v_ismigrated", 0, DbType.Int32, direction: ParameterDirection.Output);

                using (var con = ClassifiedMySqlReadConnection)
                {
                    con.Execute("getusedcardealerdata", param, commandType: CommandType.StoredProcedure);
                    LogLiveSps.LogSpInGrayLog("getusedcardealerdata");
                }

                dealerStatus.IsDealerMissing = param.Get<int>("v_isdealermissing") == 1;
                dealerStatus.IsPackageMissing = param.Get<int>("v_ispackagemissing") == 1;
                dealerStatus.PackageStartDate = param.Get<DateTime?>("v_packagestartdate");
                dealerStatus.PackageEndDate = param.Get<DateTime?>("v_packageenddate");
                dealerStatus.IsMigrated = param.Get<int?>("v_ismigrated") == 1;
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"] + "Carwale.DAL.Classified.GetUsedCarDealerStatus " + err.ToString());
                objErr.SendMail();
            }
            return dealerStatus;
        }
    }
}
