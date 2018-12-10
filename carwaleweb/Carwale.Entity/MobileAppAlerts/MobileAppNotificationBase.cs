using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.MobileAppAlerts
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
