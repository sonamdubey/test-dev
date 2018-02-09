using Bikewale.Entities.UpcomingNotification;
using Bikewale.Interfaces.Notifications;
using Bikewale.Notifications;
using MySql.CoreDAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.DAL.Notifications
{
    /// <summary>
    /// Created by: Dhruv Joshi
    /// Dated: 8th Feb 2018
    /// Description: DAL Layer for Notifications 
    /// </summary>
    public class NotificationsRepository : INotificationsRepository
    {
        /// <summary>
        /// Created by: Dhruv Joshi
        /// Dated: 8th Feb 2018
        /// Description: Inserting user data into tables (upcoming bikes notification subscription)
        /// </summary>
        /// <param name="emailId"></param>
        /// <param name="entityNotif"></param>
        /// <param name="notificationTypeId"></param>
        /// <returns></returns>
        public bool UpcomingSubscription(string emailId, UpcomingBikeEntity entityNotif, uint notificationTypeId)
        {
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("addusernotification"))
                {

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(DbFactory.GetDbParam("par_emailid", DbType.String, emailId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_makeid", DbType.Int32, entityNotif.MakeId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_modelid", DbType.Int32, entityNotif.ModelId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_notificationtype", DbType.Int32, notificationTypeId));


                    bool success = MySqlDatabase.ExecuteNonQuery(cmd, ConnectionType.MasterDatabase) >= 0;

                    return success;
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("Bikewale.DAL.Notifications.NotificationsRepository.UpcomingSubscription: MakeId:{0} ModelId:{1} EmailId:{2} NotificationTypeId:{3}", entityNotif.MakeId, entityNotif.ModelId, emailId, notificationTypeId));
            }
            return false;
        }
    }
}
