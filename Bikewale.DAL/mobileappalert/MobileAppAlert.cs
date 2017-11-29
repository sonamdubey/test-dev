

using Bikewale.Entities.MobileAppAlert;
using Bikewale.Interfaces.MobileAppAlert;
using Bikewale.Notifications;
using MySql.CoreDAL;
using System;
using System.Data;
using System.Data.Common;
namespace Bikewale.DAL.MobileAppAlert
{
    public class MobileAppAlert : IMobileAppAlertRepository
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
                    MySqlDatabase.ExecuteNonQuery(cmd, ConnectionType.MasterDatabase);

                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("Bikewale.DAL.MobileAppAlert.SaveIMEIFCMData : imei : {0}, gcmid : {1}, osType {2}: ,subsMaterId : {3}", imei, gcmId, osType, subsMasterId));
                isResult = false;
            }
            return isResult;
        }


        /// <summary>
        /// Created By  : Sushil Kumar on 23rd Jan 2017
        /// Description : Log notification ids,status,timestamp and other details 
        /// </summary>
        /// <param name="payload"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public bool CompleteNotificationProcess(MobilePushNotificationData payload, NotificationResponse result)
        {
            bool isNotificationComplete = false;

            if (payload != null)
            {
                try
                {
                    using (DbCommand cmd = DbFactory.GetDBCommand("logmobilenotifications"))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_alertid", DbType.Int32, payload.AlertId));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_alerttypeid", DbType.Int32, payload.AlertTypeId));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_articletitle", DbType.String, payload.Title));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_status", DbType.String, (result != null && string.IsNullOrEmpty(result.Error)) ? "Success" : result.Error));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_messageid", DbType.String, (result != null) ? result.MessageId : ""));
                        MySqlDatabase.InsertQuery(cmd, ConnectionType.MasterDatabase);
                        isNotificationComplete = true;
                    }
                }
                catch (Exception ex)
                {
                    ErrorClass.LogError(ex, String.Format("MobileAppAlert.CompleteNotificationProcess, alertTypeId = {0} ,alertId = {1}, articleTitle = {2}, status = {3}, messageid = {4}", payload.AlertTypeId, payload.AlertId, payload.Title, result.Error, result.MessageId));
                }
            }
            return isNotificationComplete;
        }
    }
}
