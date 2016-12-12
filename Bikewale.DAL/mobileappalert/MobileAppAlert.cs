

using Bikewale.Interfaces.MobileAppAlert;
using Bikewale.Notifications;
using MySql.CoreDAL;
using System;
using System.Data;
using System.Data.Common;
namespace Bikewale.DAL.MAA
{
    public class MobileAppAlert : IMobileAppAlert
    {
        public bool CompleteNotificationProcess(int alertTypeId)
        {
            bool isNotificationComplete = false;

            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("resetsubscriptionmaster_isprocessing"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_obj_type_id", DbType.Int32, alertTypeId));
                    isNotificationComplete = MySqlDatabase.UpdateQuery(cmd, ConnectionType.MasterDatabase);
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, String.Format("MobileAppAlert.CompleteNotificationProcess, alertTypeId = {0} ", alertTypeId));
                objErr.SendMail();
            }

            return isNotificationComplete;
        }
    }
}
