using System;

namespace Bikewale.Entities.MobileAppAlerts
{
    [Serializable]
    public class MobileAppNotificationBase
    {
        public string title { get; set; }
        public string smallPicUrl { get; set; }
        public string detailUrl { get; set; }
        public int alertTypeId { get; set; }
        public int alertId { get; set; }
        public bool isFeatured { get; set; }
        public string largePicUrl { get; set; }
        public string publishDate { get; set; }
    }
}
