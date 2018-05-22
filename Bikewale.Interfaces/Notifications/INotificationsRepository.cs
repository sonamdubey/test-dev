using Bikewale.Entities.UpcomingNotification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Interfaces.Notifications
{
    /// <summary>
    /// Created by: Dhruv Joshi
    /// Dated: 8th Feb 2018
    /// Description: Interface for Notifications (DAL Layer)
    /// </summary>
    public interface INotificationsRepository
    {
        int UpcomingSubscription(string emailId, UpcomingBikeEntity entityNotif, uint notificationTypeId);
    }
}
