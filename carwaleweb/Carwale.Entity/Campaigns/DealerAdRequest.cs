using Carwale.Entity.Geolocation;
using Newtonsoft.Json;
using System;

namespace Carwale.Entity.Campaigns
{
    [Serializable]
    [JsonObject]
    public class DealerAdRequest
    {
        public Location Location { get; set; }
        public int ModelId { get; set; }
        public int CampaignId { get; set; }
        public int PlatformId { get; set; }
        public int ApplicationId { get; set; }
        public int PageId { get; set; }
        public bool ShowRecommendation { get; set; }
        public bool NoZoneFilter { get; set; }
        public bool ShowDealerLocator { get; set; }
        public bool DealerAdminFilter { get; set; }
    }
}
