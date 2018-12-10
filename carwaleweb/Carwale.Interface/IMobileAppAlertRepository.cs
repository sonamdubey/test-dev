using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Carwale.Entity.MobileAppAlerts;

namespace Carwale.Interfaces
{
    public interface IMobileAppAlertRepository
    {        
        int GetTotalNumberOfSubs(int alertTypeId);
        List<string> GetRegistrationIds(int alertTypeId, int startNum, int endNum);
        bool SubscriberActivity(MobileAppNotificationRegistration t);        
    }
}
