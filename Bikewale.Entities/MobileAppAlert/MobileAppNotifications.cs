using System;
using System.Collections.Generic;


namespace Bikewale.Entities.MobileAppAlerts
{
    [Serializable]
    public class MobileAppNotifications : MobileAppNotificationBase
    {        
        public List<string> GCMList { get; set; }
        public List<string> ApnsList { get; set; }
    }
}
