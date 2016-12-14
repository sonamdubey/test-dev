
namespace Bikewale.Entities.MobileAppAlert
{
    /// <summary>
    /// 
    /// </summary>
    public class AppFCMInput
    {
        public string Imei { get; set; }
        public string GcmId { get; set; }
        public string OsType { get; set; }
        public string SubsMasterId { get; set; }
    }
}
