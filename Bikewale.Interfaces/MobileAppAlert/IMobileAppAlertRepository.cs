
namespace Bikewale.Interfaces.MobileAppAlert
{
    /// <summary>
    /// 
    /// </summary>
    public interface IMobileAppAlertRepository
    {
        bool CompleteNotificationProcess(int alertid);
        bool SaveIMEIFCMData(string imei, string gcmId, string osType, string subsMasterId);
    }
}
