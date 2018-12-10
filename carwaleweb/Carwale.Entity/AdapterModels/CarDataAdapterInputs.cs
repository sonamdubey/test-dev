using Carwale.Entity.Campaigns;
using Carwale.Entity.Geolocation;
using Carwale.Entity.Template;
using System.Collections.Generic;

namespace Carwale.Entity.AdapterModels
{
    public class CarDataAdapterInputs
    {
        public Location CustLocation { get; set; }
        public CarEntity ModelDetails { get; set; }
        public bool IsCityPage { get; set; }
        public string CwcCookie { get; set; }
        public string UserModelHistory { get; set; }
        public string MobileCookies { get; set; }
        public string CompareVersionsCookie { get; set; }
        public int AbTest { get; set; }
        public bool IsMobile { get; set; }
        public bool IsAmp { get; set; }
        public CampaignInputv2 CampaignInput { get; set; }
        public List<int> ModelIds { get; set; }
        public List<int> VersionIds { get; set; }
        public bool ShowOfferUpfront { get; set; }
        public string Type { get; set; }

    }
}
