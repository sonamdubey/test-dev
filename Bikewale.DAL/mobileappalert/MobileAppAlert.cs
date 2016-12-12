

using Bikewale.Interfaces.MobileAppAlert;
using Bikewale.Notifications;
using MySql.CoreDAL;
using System;
using System.Data;
using System.Data.Common;
using System.Web;
namespace Bikewale.DAL.MobileAppAlert
{
    public class MobileAppAlert : IMobileAppAlert
    {
        /// <summary>
        /// Auth: Sangram Nandkhile on 5th January 2016
        /// Desc: Push IMEI, GCM id, OS Type, Subscription Master Id
        /// </summary>
        /// <param name="imei"></param>
        /// <param name="gcmId"></param>
        /// <param name="osType"></param>
        /// <param name="subsMasterId"></param>
        /// <returns></returns>
        public bool SaveIMEIFCMData(string imei, string gcmId, string osType, string subsMasterId)
        {
            bool isResult = true;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    cmd.CommandText = "subscriptionactivity";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(DbFactory.GetDbParam("par_imei", DbType.String, 50, imei));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_gcmid", DbType.String, 200, gcmId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_ostype", DbType.Byte, osType));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_subsmasterid", DbType.String, 100, subsMasterId));
                    // LogLiveSps.LogSpInGrayLog(cmd);
                    MySqlDatabase.ExecuteNonQuery(cmd, ConnectionType.MasterDatabase);

                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
                isResult = false;
            }
            return isResult;
        }

        /// <summary>
        /// Created By  : Sushil Kumar on 12th Dec 2016
        /// Description : To complete notification process
        /// </summary>
        /// <param name="alertTypeId"></param>
        /// <returns></returns>
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


        public Entities.MobileAppAlert.SubscriptionResponse SubscribeFCMNotification(string action, string payload, int retries)
        {
            throw new NotImplementedException();
        }


        public bool SendFCMNotification(Entities.MobileAppAlert.MobilePushNotificationData payload)
        {
            throw new NotImplementedException();
        }
    }
}
