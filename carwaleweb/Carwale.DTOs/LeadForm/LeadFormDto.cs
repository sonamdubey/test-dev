using Carwale.Entity.CarData;
using Carwale.Entity.Dealers;
using Carwale.Entity.Geolocation;

namespace Carwale.DTOs.LeadForm
{
    public class LeadFormDto
    {
        public SponsoredDealer CampaignDetails { get; set; }
        public CarModelDetails CarDetails { get; set; }
        public int VersionId { get; set; }
        public Location CustLocation { get; set; }
        public int ScreenId { get; set; }
        public int LeadClickSource { get; set; }
        public int InquiryClickSource { get; set; }
        public int PlatformId { get; set; }
        public string AppVersionId { get; set; }
    }
}
