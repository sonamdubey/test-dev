using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Carwale.Notifications;
using Carwale.DAL.CoreDAL;
using Carwale.Entity.PaymentGateway;
using Carwale.Interfaces.PaymentGateway;
using Carwale.Notifications.Logs;
using Dapper;
using Carwale.Entity.Classified;

namespace Carwale.DAL.PaymentGateway
{
    public class PackageRepository : RepositoryBase,IPackageRepository
    {
        public Package GetPackageDetails(int packageId)
        {
            Package package = null;
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("v_packageid", packageId, DbType.Int32);
                using (var con = ClassifiedMySqlReadConnection)
                {
                    package = con.Query<Package>("getpackagedetails", parameters, commandType: CommandType.StoredProcedure).FirstOrDefault<Package>();
                    LogLiveSps.LogSpInGrayLog("getpackagedetails");
                }
            }
            catch (Exception err)
            {
                Logger.LogException(err);
            }
            return package;
        }

        public bool ChangePackage(ulong inquiryId, ulong consumerId, int packageId)
        {
            bool ret = false;
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("v_carid", inquiryId, DbType.UInt64);
                parameters.Add("v_consumerid", consumerId, DbType.UInt64);
                parameters.Add("v_packageid", packageId, DbType.Int32);
                parameters.Add("v_affectedrows", 0, DbType.Int32, direction: ParameterDirection.Output);
                using (var con = ClassifiedMySqlMasterConnection)
                {
                    con.Execute("changelistingpackage", parameters, commandType: CommandType.StoredProcedure);
                    LogLiveSps.LogSpInGrayLog("changelistingpackage");
                    ret = parameters.Get<int>("v_affectedrows") > 0;
                }
            }
            catch (Exception err)
            {
                Logger.LogException(err);
            }
            return ret;  

        }

        public void GetCustomerInfoByCustomerID(TransactionDetails transaction)
        {
            transaction.CustomerName = "-1";
            transaction.CustCity = "-1";
            transaction.CustState = "-1";
            transaction.CustEmail = "-1";
            transaction.CustMobile = "-1";

            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("v_CustomerID", transaction.CustomerID, DbType.String);
                parameters.Add("v_CustName", "-1", DbType.String, direction: ParameterDirection.Output);
                parameters.Add("v_CustCity", "-1", DbType.String, direction: ParameterDirection.Output);
                parameters.Add("v_CustState", "-1", DbType.String, direction: ParameterDirection.Output);
                parameters.Add("v_CustEmail", "-1", DbType.String, direction: ParameterDirection.Output);
                parameters.Add("v_CustMobile", "-1", DbType.String, direction: ParameterDirection.Output);
                using (var con = ClassifiedMySqlReadConnection)
                {
                    con.Execute("cwmasterdb.GetCustomerInfoByCustomerID", parameters, commandType: CommandType.StoredProcedure);
                    transaction.CustomerName = parameters.Get<string>("v_CustName");
                    transaction.CustCity = parameters.Get<string>("v_CustCity");
                    transaction.CustState = parameters.Get<string>("v_CustState");
                    transaction.CustEmail = parameters.Get<string>("v_CustEmail");
                    transaction.CustMobile = parameters.Get<string>("v_CustMobile");
                }
            }
            catch (Exception err)
            {
                Logger.LogException(err);
            }
        }


        public bool UpgradePackageTypeToListingType(int consumerType, ulong carId, ulong consumerId)
        {
            bool isUpgraded = false;
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("v_consumertype", consumerType, DbType.Int32);
                parameters.Add("v_carid", carId, DbType.UInt64);
                parameters.Add("v_consumerid", consumerId, DbType.UInt64);
                parameters.Add("v_isoffline", 0, DbType.Int32);
                parameters.Add("v_isupgraded", dbType: DbType.Int32, direction: ParameterDirection.Output);
                using (var con = ClassifiedMySqlMasterConnection)
                {
                    con.Execute("upgradepackagetypetolistingtype", parameters, commandType: CommandType.StoredProcedure);
                    LogLiveSps.LogSpInGrayLog("upgradepackagetypetolistingtype");
                }
                isUpgraded = parameters.Get<int>("v_isupgraded") == 1 ? true : false;
            }
            catch (Exception err)
            {
                Logger.LogException(err);
            }
            return isUpgraded;
        }

        public int InsertConsumerPackageRequests(TransactionDetails transaction, int resposeCode)
        {
            int packageRequestsId = 0;
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("v_id", -1);
                parameters.Add("v_consumertype", transaction.ConsumerType);
                parameters.Add("v_consumerid", Convert.ToInt64(transaction.CustomerID));
                parameters.Add("v_packageid", transaction.PackageId);
                parameters.Add("v_actualvalidity", 0);
                parameters.Add("v_actualinquirypoints", 0);
                parameters.Add("v_actualamount", transaction.Amount);
                parameters.Add("v_paymentmodeid", 4);
                parameters.Add("v_chk_dd_date", DateTime.Now);
                parameters.Add("v_entrydate", DateTime.Now);
                parameters.Add("v_enteredby", 2);
                parameters.Add("v_enteredbyid", Convert.ToInt64(transaction.CustomerID));
                parameters.Add("v_itemid", Convert.ToInt64(transaction.PGId));
                parameters.Add("v_status", 0, direction: ParameterDirection.Output);
                parameters.Add("v_newid", 0, direction: ParameterDirection.Output);
                parameters.Add("v_responsecode",resposeCode);
                using (var con = ClassifiedMySqlMasterConnection)
                {
                    con.Execute("cwmasterdb.insertconsumerpackagerequests_v_16_12_1", parameters, commandType: CommandType.StoredProcedure);
                    LogLiveSps.LogSpInGrayLog("insertconsumerpackagerequests_v_16_12_1");
                }
                packageRequestsId = parameters.Get<int>("v_newid");
            }
            catch (Exception err)
            {
                Logger.LogException(err);
            }
            return packageRequestsId;
        }

        public List<MyPaymentsEntity> GetPaymentsDetails(int customerId)
        {
            List<MyPaymentsEntity> paymentDetails = null;
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("v_customerId", customerId);
                using (var con = ClassifiedMySqlReadConnection)
                {
                    paymentDetails = con.Query<MyPaymentsEntity>("getpaymentdetails", parameters, commandType: CommandType.StoredProcedure).AsList();
                    LogLiveSps.LogSpInGrayLog("getpaymentdetails");
                }
            }
            catch (Exception err)
            {
                Logger.LogException(err);
            }
            return paymentDetails;
        }

        public InvoiceDetails GetInvoiceDetails(int customerId, int invoiceId)
        {
            InvoiceDetails invoiceDetails = null;
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("v_customerId", customerId);
                parameters.Add("v_invoiceId", invoiceId);
                using (var con = ClassifiedMySqlReadConnection)
                {
                    invoiceDetails = con.Query<InvoiceDetails>("getinvoicedetails", parameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    LogLiveSps.LogSpInGrayLog("getinvoicedetails");
                }
            }
            catch (Exception err)
            {
                Logger.LogException(err);
            }
            return invoiceDetails;
        }
    }
}
