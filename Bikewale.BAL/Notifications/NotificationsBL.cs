using Bikewale.DAL.Notifications;
using Bikewale.Entities.UpcomingNotification;
using Bikewale.Interfaces.Notifications;
using Bikewale.Notifications;
using Bikewale.Notifications.MailTemplates;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Bikewale.BAL.Notifications
{
    /// <summary>
    /// Created by: Dhruv Joshi
    /// Dated: 8th Feb 2018
    /// Description: BAL Layer for notifiactions
    /// </summary>
    public class NotificationsBL : INotifications 
    {
        private readonly INotificationsRepository _notificationsRepository;
        
        public NotificationsBL(INotificationsRepository notificationsRepository)
        {
            _notificationsRepository = notificationsRepository;
        }

        /// <summary>
        /// Created by: Dhruv Joshi
        /// Dated: 8th Feb 2018
        /// Description: Call the DAL layer for Inserting user data into tables (upcoming bikes notification subscription)
        /// </summary>
        /// <param name="emailId"></param>
        /// <param name="entityNotif"></param>
        /// <param name="notificationTypeId"></param>
        /// <returns></returns>
        public bool UpcomingSubscription(string emailId,UpcomingBikeEntity entityNotif , uint notificationTypeId)
        {
            try
            {
                int rows_returned = _notificationsRepository.UpcomingSubscription(emailId, entityNotif, notificationTypeId);
                /*
                 * rows_returned - no. of rows inserted in db (user subscribes to new notification), 
                 * if > 0, send email to user,
                 * if = 0, user has subscribed for that bike notification more than once, display "thank you" message but don't send email,
                 * if <0, some error has occured, no email sent, error message displayed.
                 * 
                */
                if(rows_returned > 0)
                {
                        ComposeEmailBase objNotify = new UpcomingBikesSubscription(entityNotif.BikeName);
                        objNotify.Send(emailId, string.Format("You have subscribed to notifications for upcoming bike {0}", entityNotif.BikeName));
                        return true;
                }
                else if (rows_returned == 0)
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Exception : Bikewale.BAL.Notifications.NotificationsBL.UpcomingSubscription");
            }
            return false;
        }
    }
}
