using Bikewale.Entities.MobileAppAlert;
namespace Bikewale.Interfaces.MobileAppAlert
{
    /// <summary>
    /// Created By  : Sushil Kumar on 12th Dec 2016
    /// Description : Merged app notification related method into one interface
    /// </summary>
    public interface IMobileAppAlert
    {
        bool CompleteNotificationProcess(int alertid);
        bool SaveIMEIFCMData(string imei, string gcmId, string osType, string subsMasterId);
        SubscriptionResponse SubscribeFCMNotification(string action, string payload, int retries);
        bool SendFCMNotification(MobilePushNotificationData payload);
    }
}
