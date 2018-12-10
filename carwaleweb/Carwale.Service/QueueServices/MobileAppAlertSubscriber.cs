using Carwale.Interfaces;
using Carwale.Entity.MobileAppAlerts;
using System;

namespace Carwale.Service.QueueServices
{
    public class MobileAppAlertSubscriber<T> : IRequestManager<T> where T : MobileAppNotificationRegistration                                                                      
    {
        private readonly IMobileAppAlertRepository _mobileAppAlertRepo;

        public MobileAppAlertSubscriber(IMobileAppAlertRepository mobileAppAlertRepo)
        {
            _mobileAppAlertRepo = mobileAppAlertRepo;
        }

        public U ProcessRequest<U>(T t)
        {
            return (U)Convert.ChangeType(_mobileAppAlertRepo.SubscriberActivity(t), typeof(U));  //calling dal layer      
        }

    }
}
