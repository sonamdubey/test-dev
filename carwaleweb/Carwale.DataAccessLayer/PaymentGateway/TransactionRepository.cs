using System;
using System.Data;
using System.Web;
using Carwale.Notifications;
using Carwale.Entity.PaymentGateway;
using Carwale.Interfaces.PaymentGateway;
using System.Configuration;
using Carwale.Notifications.Logs;
using Dapper;

namespace Carwale.DAL.PaymentGateway
{
    public class TransactionRepository : RepositoryBase, ITransactionRepository
    {
        public long BeginTransaction(TransactionDetails _inputs)
        {	
            try
            {
                var param = new DynamicParameters();
                param.Add("v_id", -1);
                param.Add("v_consumerType", _inputs.ConsumerType);
                param.Add("v_consumerId",  _inputs.CustomerID);
                param.Add("v_carId", _inputs.PGId);
                param.Add("v_packageId", _inputs.PackageId);
                param.Add("v_amount", _inputs.Amount);
                param.Add("v_entryDateTime", DateTime.Now);
                param.Add("v_responseCode", -1);
                param.Add("v_responseMessage", "");
                param.Add("v_ePGTransactionId", "");
                param.Add("v_authId", "");
                param.Add("v_processCompleted", false);
                param.Add("v_transactionCompleted", false);
                param.Add("v_iPAddress", _inputs.ClientIP);
                param.Add("v_entryDate", DateTime.Today);
                param.Add("v_userAgent", _inputs.UserAgent);
                param.Add("v_pGSource", _inputs.SourceId);
                param.Add("v_platformId", (_inputs.PlatformId < 1 ) ? 1 : _inputs.PlatformId);
                param.Add("v_applicationId", (_inputs.ApplicationId < 1) ? 1: _inputs.ApplicationId);
                param.Add("v_transactionReferenceId", _inputs.UniqueTransactionId);
                param.Add("v_recordId", direction: ParameterDirection.Output);

                using (var con = AdvantageMySqlMasterConnection)
                {
                    con.Execute("cwmasterdb.insertpgtransactions", param, commandType: CommandType.StoredProcedure);
                    LogLiveSps.LogSpInGrayLog("insertpgtransactions");
                    return param.Get<int>("v_recordId");
                }
            }
            catch (Exception err)
            {
                Logger.LogException(err);
                return -1;
            }
        }

        public TransactionDetails CompleteTransaction(GatewayResponse _inputs)
        {
            string recordId = string.Empty;
            TransactionDetails transaction = null;
            try
            {
                var param = new DynamicParameters();
                param.Add("v_id", _inputs.PGTransId);
                param.Add("v_consumerType", direction: ParameterDirection.Output);
                param.Add("v_consumerId", direction: ParameterDirection.Output);
                param.Add("v_packageId", direction: ParameterDirection.Output);
                param.Add("v_amount", direction: ParameterDirection.Output);
                param.Add("v_responseCode", _inputs.PGRespCode);
                param.Add("v_responseMessage", _inputs.PGMessage);
                param.Add("v_ePGTransactionId", _inputs.PGEPGTransId);
                param.Add("v_authId", string.IsNullOrEmpty(_inputs.PGAuthIdCode) ? Convert.DBNull : _inputs.PGAuthIdCode);
                param.Add("v_processCompleted", true);
                param.Add("v_transactionCompleted", _inputs.IsTransactionCompleted, direction: ParameterDirection.InputOutput);
                param.Add("v_recordId", direction: ParameterDirection.Output);

                using (var con = AdvantageMySqlMasterConnection)
                {
                    con.Execute("cwmasterdb.completetransaction", param, commandType: CommandType.StoredProcedure);
                    LogLiveSps.LogSpInGrayLog("cwmasterdb.completetransaction");
                    if (param.Get<byte>("v_transactionCompleted") > 0)
                    {
                        transaction = new TransactionDetails();
                        transaction.PGRecordId = param.Get<int>("v_recordId");
                        transaction.ConsumerType = param.Get<Int16>("v_consumerType");
                        transaction.CustomerID = Convert.ToUInt64(param.Get<Int64>("v_consumerId"));
                        transaction.PackageId = param.Get<int>("v_packageId");
                        transaction.Amount = param.Get<int>("v_amount");
                    }
                }
            }
            catch (Exception err)
            {
                Logger.LogException(err);
            }
            return transaction;
        }

        public void InsertConsumerInvoice(int pgtransId, int pkgReqId, TransactionDetails transaction)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("v_pgTransId", pgtransId);
                param.Add("v_packageReqId", pkgReqId);
                param.Add("v_consumerName", transaction.CustomerName);
                param.Add("v_consumerEmail", transaction.CustEmail);
                param.Add("v_consumerContactNo", transaction.CustMobile);
                param.Add("v_consumerAddress", transaction.CustCity +','+ transaction.CustState);

                using (var con = AdvantageMySqlMasterConnection)
                {
                    con.Execute("cwmasterdb.insertconsumerinvoice-v_16_12_1", param, commandType: CommandType.StoredProcedure);
                    LogLiveSps.LogSpInGrayLog("insertconsumerinvoice-v_16_12_1");
                }
            }
            catch (Exception err)
            {
                Logger.LogException(err);
            }
        }

        public void UpdateInquiriesPaymentMode(ulong carid, int transctionType)
        {
            string sql = string.Empty;
            if(transctionType == (int)TransactionType.Online)
                sql = "UPDATE customersellinquiries SET PaymentMode = 1 WHERE ID = @CarId";
            else if (transctionType == (int)TransactionType.Cheque)
                sql = "UPDATE customersellinquiries SET PaymentMode = 0 WHERE ID = @CarId";
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@CarId", carid, DbType.UInt64);
                using (var con = ClassifiedMySqlMasterConnection)
                {
                    LogLiveSps.LogSpInGrayLog(sql);
                    con.Execute(sql, parameters, commandType: CommandType.Text);
                }
            }
            catch (Exception err)
            {
                Logger.LogException(err);
            } 
        }
    }

    public enum TransactionType
    {
        Online = 1,
        Cheque = 2
    }


}