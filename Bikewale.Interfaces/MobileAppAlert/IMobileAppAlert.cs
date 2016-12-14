using Bikewale.Entities.MobileAppAlert;
namespace Bikewale.Interfaces.MobileAppAlert
{
    /// <summary>
    /// Created By  : Sushil Kumar on 12th Dec 2016
    /// Description : Merged app notification related method into one interface
    /// </summary>
    public interface IMobileAppAlert
    {
        bool SendFCMNotification(MobilePushNotificationData payload);
        bool SubscribeFCMUser(AppFCMInput input);
        bool UnSubscribeFCMUser(AppFCMInput input);
    }
}
