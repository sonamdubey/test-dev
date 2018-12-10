
using System;
namespace Carwale.Entity.Advertizings.Apps
{
    [Serializable]
    public class SplashScreenBanner
    {
        public int Id { get; set; }
        public string CampaignName { get; set; }
        public string Splashurl { get; set; }
        public int PlatformId { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public int IsActive { get; set; }
        public int ModifiedBy { get; set; }
        public int Priority { get; set; }
        public int AdTimeOut { get; set; }
        public bool IsDefault { get; set; }
    }
}
