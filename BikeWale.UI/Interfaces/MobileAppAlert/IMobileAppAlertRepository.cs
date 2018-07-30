
using Bikewale.Entities.MobileAppAlert;
namespace Bikewale.Interfaces.MobileAppAlert
{
    /// <summary>
    /// Created By  : Sushil Kumar on 12th Dec 2016
    /// Description : Interface to handle mobile notifications
    /// Modified By  : Sushil Kumar on 23rd Jan 2016
    /// Description : Added method to log notifications with status
    /// </summary>
    public interface IMobileAppAlertRepository
    {
        bool CompleteNotificationProcess(MobilePushNotificationData payload, NotificationResponse result);
        bool SaveIMEIFCMData(string imei, string gcmId, string osType, string subsMasterId);
    }
}
