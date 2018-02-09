using Bikewale.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bikewale.Service.AutoMappers.UpcomingNotification
{
    /// <summary>
    /// Created By: Dhruv Joshi
    /// Dated: 8th Feb 2018
    /// Description: Notificaion Mapper Dto to entity
    /// </summary>
    public class NotificationMapper
    {
        /// <summary>
        /// Created by: Dhruv Joshi
        /// Dated: 8th Feb 2018
        /// Description: Mapper for upcoming bikes notification subscription
        /// </summary>
        /// <param name="entityNotif"></param>
        /// <returns></returns>
        internal static Entities.UpcomingNotification.UpcomingBikeEntity Convert(DTO.Upcoming.UpcomingNotificationDTO entityNotif)
        {
            try
            {
                AutoMapper.Mapper.CreateMap<DTO.Upcoming.UpcomingNotificationDTO, Entities.UpcomingNotification.UpcomingBikeEntity>();
                return AutoMapper.Mapper.Map<DTO.Upcoming.UpcomingNotificationDTO, Entities.UpcomingNotification.UpcomingBikeEntity>(entityNotif);
            }
            catch(Exception ex)
            {
                ErrorClass.LogError(ex, "Exception : Bikewale.Service.AutoMappers.UpcomingNotification.NotificationMapper");
                return null;
            }
        }
    }
}