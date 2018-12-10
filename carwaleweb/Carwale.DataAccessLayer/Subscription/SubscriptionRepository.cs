using Carwale.Interfaces.Subscription;
using Carwale.Notifications.Logs;
using Dapper;
using System;
using System.Data;

namespace Carwale.DAL.Subscription
{
    public class SubscriptionRepository : RepositoryBase, ISubscriptionRepository
    {
        public bool Subscribe(string email, int subscriptionCategory, int subscriptionType)
        {
            bool status = false;
            try
            {                
                var param = new DynamicParameters();
                param.Add("v_EmailAddress", email);
                param.Add("v_SubscriptionCategory", subscriptionCategory);
                param.Add("v_SubscriptionType", subscriptionType);
                param.Add("v_Frequency", subscriptionType == 2 ? "Weekly" : "Monthly");
                param.Add("v_SubscriptionDate", DateTime.Now);
                param.Add("v_Status", dbType: DbType.Int16, direction: ParameterDirection.Output);

                using (var con = CarDataMySqlMasterConnection)
                {
                    con.Query("AddSubscription", param, commandType: CommandType.StoredProcedure);
                    LogLiveSps.LogSpInGrayLog("AddSubscription");
                    status = Convert.ToBoolean(param.Get<Int16?>("v_Status"));
                }
            }
            catch (Exception err)
            {
                Logger.LogException(err);
            }
            return status;
        }

    }
}
