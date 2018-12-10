using Carwale.Entity.Classified.SellCarUsed;
using Carwale.Notifications.Logs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.Data;
using Carwale.Entity.Classified.CarDetails;
using Carwale.Entity;
using Newtonsoft.Json;

namespace Carwale.DAL.Classified.SellCar
{
    public class C2BStockRepository : RepositoryBase, IC2BStockRepository
    {
        public C2BStockDetails GetC2BStockDetails(int inquiryId)
        {
            C2BStockDetails c2bStockDetails = null;
            try
            {
                using (var con = ClassifiedMySqlReadConnection)
                {
                    var param = new DynamicParameters();
                    param.Add("v_inquiryid", inquiryId);
                    c2bStockDetails = con.Query<C2BStockDetails, Customer, BasicCarInfo, C2BStockDetails>("getc2bstockdetails",
                        (c2bStockdetails, customerInfo, carInfo)
                        =>
                        {
                            c2bStockdetails.CustomerInfo = customerInfo;
                            c2bStockdetails.CarInfo = carInfo;
                            return c2bStockdetails;
                        }, param, commandType: CommandType.StoredProcedure, splitOn: "name,inquiryid").First();
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex, "Error in Fetching C2B Stock details from Database : Inquiryid :" + inquiryId);
            }
            return c2bStockDetails;
        }

        public void SellCarHotLead(int inquiryId, int hotLeadPrice)
        {
            try
            {
                using (var con = ClassifiedMySqlMasterConnection)
                {
                    var param = new DynamicParameters();
                    param.Add("v_inquiryid", inquiryId);
                    param.Add("v_hotleadprice", hotLeadPrice);
                    con.Execute("insertsellcarhotlead", param, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex, "Failed to save c2b hot lead : Inquiryid :" + inquiryId);
            }

        }

        public void LogC2BApiErrors(int inquiryId, int tempId, List<ValidationErrors> errors)
        {
            if (errors != null)
            {
                try
                {
                    var param = new DynamicParameters();
                    param.Add("v_inquiryid", inquiryId);
                    param.Add("v_tempid", tempId);
                    param.Add("v_errors", JsonConvert.SerializeObject(errors));

                    using (var con = ClassifiedMySqlMasterConnection)
                    {
                        con.Execute("logc2bapierrors", param, commandType: CommandType.StoredProcedure);
                    }
                }
                catch (Exception ex)
                {
                    Logger.LogException(ex, "Failed to log c2b api errors");
                }
            }
        }

        public void RemoveC2BApiError(int inquiryId)
        {
            var param = new DynamicParameters();
            param.Add("v_inquiryid",inquiryId);
            try
            {
                using (var con = ClassifiedMySqlMasterConnection)
                {
                    con.Execute("removec2bapierror", param, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex, "Failed to remove c2b api error");
            }
        }

        public void ResendFailedC2BStocks()
        {
            try
            {
                using (var con = ClassifiedMySqlMasterConnection)
                {
                    con.Execute("resendfailedc2bstocks", commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex, "Failed to Resend Failed C2B Stocks");
            }
        }
    }
}
