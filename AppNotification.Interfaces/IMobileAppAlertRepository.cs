using AppNotification.Entity;
using System.Collections.Generic;

namespace AppNotification.Interfaces
{
    public interface IMobileAppAlertRepository
    {
        int GetTotalNumberOfSubs(int alertTypeId);
        List<string> GetRegistrationIds(int alertTypeId, int startNum, int endNum);
        bool SubscriberActivity(MobileAppNotificationRegistration t);
        bool CompleteNotificationProcess(int alertTypeId, string response);
    }
}
