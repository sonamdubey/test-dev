using Carwale.Entity.Classified.EmailAlerts;
using Carwale.Notifications.Logs;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Carwale.DAL.Classified.EmailAlerts
{
    public class DailyWeeklyEmailRepository : RepositoryBase
    {
        public List<DailyWeeklyEmail> GetEmailData(bool isWeekly)
        {
            List<DailyWeeklyEmail> customers = null;
            try
            {
                using (var con = ClassifiedMySqlReadConnection)
                {
                    if (isWeekly)
                        customers = con.Query<DailyWeeklyEmail>("getweeklyalerts", commandType: CommandType.StoredProcedure).ToList();
                    else
                        customers = con.Query<DailyWeeklyEmail>("getdailyalerts", commandType: CommandType.StoredProcedure).ToList();

                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
            }
            return customers;
        }
        public void SetMailedStatus(string Id, bool isWeekly)
        {
            try
            {
                using (var con = ClassifiedMySqlReadConnection)
                {
                    var param = new DynamicParameters();
                    param.Add("v_Id", Id, dbType: DbType.String, direction: ParameterDirection.Input);
                    if (isWeekly)
                        con.Execute("setmailstatusforweeklyalertIds", param, commandType: CommandType.StoredProcedure);
                    else
                        con.Execute("setmailstatusfordailyalertIds", param, commandType: CommandType.StoredProcedure);
                }

            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
            }

        }
    }
}
