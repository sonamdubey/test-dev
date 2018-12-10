using Carwale.DAL.CoreDAL.MySql;
using Carwale.Entity.Notifications;
using Carwale.Interfaces.Notifications;
using Carwale.Notifications.Logs;
using Dapper;
using System;
using System.Data;
using System.Data.Common;
using System.Linq;

namespace Carwale.DAL.Notifications
{
    public class SMSRepository : RepositoryBase, ISMSRepository
    {
        /// <summary>
        /// For Saving data to SmsSent table
        /// Written By : Ashish Verma on 2/6/2014
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public string SaveSMSSentData(SMS sms)
        {
            int? currentId = null;
            try
            {
                var param = new DynamicParameters();
                param.Add("v_number", sms.Mobile, DbType.String);
                param.Add("v_message", sms.Message, DbType.String);
                param.Add("v_servicetype", sms.SMSType, DbType.Int32);
                param.Add("v_smssentdatetime", DateTime.Now, DbType.DateTime);
                param.Add("v_successfull", sms.Status, DbType.Boolean);
                param.Add("v_returnedmsg", sms.ReturnedMsg, DbType.String);
                param.Add("v_smspageurl", sms.PageUrl, DbType.String);

                using (var con = ClassifiedMySqlMasterConnection)
                {
                    currentId = con.Query<int?>("cwmasterdb.insertsmssent", param, commandType: CommandType.StoredProcedure).FirstOrDefault();
                }
            }
            catch (Exception er)
            {
                Logger.LogError(er);
            }
            return currentId.HasValue ? currentId.ToString() : null;
        }
       public void SaveDataForSMS(string tempCustomerId)
        {
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("SaveDataForSms_v16_11_7"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_CustomerId", DbType.Int64, tempCustomerId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_EntryDateTime", DbType.DateTime, DateTime.Now));
                    MySqlDatabase.InsertQuery(cmd, DbConnections.CarDataMySqlMasterConnection);
                }
            }
            catch (Exception er)
            {
                Logger.LogError(er);
            } // catch Exception
        }
    }
}
