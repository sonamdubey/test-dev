using Carwale.Interfaces.Stock;
using Carwale.Notifications.Logs;
using Dapper;
using System;
using System.Data;
using System.Linq;

namespace Carwale.DAL.Stock
{
    public class StockRegCertificatesRepository : RepositoryBase, IStockRegCertificatesRepository
    {
        public string GetStockRegCertificate(int inquiryId)
        {
            string rcOriginalImgPath = null;
            try
            {
                var param = new DynamicParameters();
                param.Add("v_inquiryid", inquiryId);

                using (var con = ClassifiedMySqlReadConnection)
                {
                    rcOriginalImgPath = con.Query<string>("getregistrationcertificate", param, commandType: CommandType.StoredProcedure).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return rcOriginalImgPath;
        }

        public int AddStockRegCertificate(int inquiryId, string hostUrl, string originalImgPath)
        {
            int rcId = -1;
            try
            {
                var param = new DynamicParameters();
                param.Add("v_inquiryid", inquiryId);
                param.Add("v_hosturl", hostUrl);
                param.Add("v_originalimgpath", originalImgPath);
                param.Add("v_rcid", dbType: DbType.Int32, direction: ParameterDirection.Output);

                using (var con = ClassifiedMySqlMasterConnection)
                {
                    con.Execute("InsertRegistrationCertificate", param, commandType: CommandType.StoredProcedure);
                    rcId = param.Get<int>("v_rcid");
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return rcId;
        }

        public void DeleteStockRegCertificate(int inquiryId, int rcId)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("v_inquiryid", inquiryId);
                param.Add("v_rcid", rcId);

                using (var con = ClassifiedMySqlMasterConnection)
                {
                    con.Execute("RemoveRegistrationCertificate", param, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }
    }
}
