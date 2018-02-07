using Bikewale.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bikewale.Service.AutoMappers.UpcomingNotification
{
    public class NotificationMapper
    {
        internal static Entities.UpcomingNotification.UpcomingNotificationEntity Convert(DTO.Upcoming.UpcomingNotificationDTO entityNotif)
        {
            try
            {
                AutoMapper.Mapper.CreateMap<DTO.Upcoming.UpcomingNotificationDTO, Entities.UpcomingNotification.UpcomingNotificationEntity>();
                return AutoMapper.Mapper.Map<DTO.Upcoming.UpcomingNotificationDTO, Entities.UpcomingNotification.UpcomingNotificationEntity>(entityNotif);
            }
            catch(Exception ex)
            {
                ErrorClass.LogError(ex, "Exception : Bikewale.Service.AutoMappers.UpcomingNotification.NotificationMapper");
                return null;
            }
        }
    }
}