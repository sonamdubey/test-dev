using Carwale.Entity.Geolocation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.LandingPage
{
    [Serializable]
    public class LandingPageDetails
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string PrimaryHeading { get; set; }
        public string SecondaryHeading { get; set; }
        public bool IsEmailRequired { get; set; }
        public int DefaultModel { get; set; }
        public string ButtonText { get; set; }
        public string TrailingText { get; set; }
        public bool IsDesktop { get; set; }
        public bool IsMobile { get; set; }
        public string DesktopHtml { get; set; }
        public string MobileHtml { get; set; }
        public int CampaignType { get; set; }
        public int PQCampaignId { get; set; }
        public int DealerId { get; set; }
        public int Id { get; set; }
    }
}
