using Bikewale.DTO.Upcoming;
using Bikewale.Entities.UpcomingNotification;
using Bikewale.Interfaces.Notifications;
using Bikewale.Notifications;
using Bikewale.Service.AutoMappers.UpcomingNotification;
using Bikewale.Service.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Bikewale.Service.Controllers.Notifications
{
    /// <summary>
    /// Created by: Dhruv Joshi
    /// Dated: 8th Feb 2018
    /// Description: Controllers for upcoming notifications
    /// </summary>
    public class UpcomingController : CompressionApiController
    {
        private readonly INotifications _notifications= null;

        public UpcomingController(INotifications notifications)
        {
            _notifications = notifications;
        }
        
        /// <summary>
        /// Created by: Dhruv Joshi
        /// Dated: 8th Feb 2018
        /// Description: Method for Notification Subscription on Upcoming Bikes
        /// </summary>
        /// <param name="dtoNotif"></param>
        /// <returns></returns>
        [Route("api/notifyuser/")]
        public bool UpcomingSubscriptionNotification([FromBody]UpcomingNotificationDTO dtoNotif)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ushort notificationTypeId = (ushort)EnumNotifTypeId.UpcomingSubscription;
                    string emailId = dtoNotif.EmailId.ToString();
                    UpcomingBikeEntity entitiyNotif = NotificationMapper.Convert(dtoNotif);
                    if (entitiyNotif != null && _notifications.UpcomingSubscription(emailId, entitiyNotif, notificationTypeId))
                    {
                        return true;
                    }
                    else
                    {
                        throw new HttpResponseException(HttpStatusCode.BadRequest);
                    }
                }
                else
                {
                    throw new HttpResponseException(HttpStatusCode.BadRequest);
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Exception : Bikewale.Service.Controllers.Make.MakePageController");
                return false;
            }
        }
    }
}
