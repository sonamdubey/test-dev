using System;

namespace Carwale.Entity.ES
{
    [Serializable]
    public class SponsoredNavigation
    {
        public int Id { get; set; }
        public int SectionId { get; set; }
        public string Title { get; set; }
        public string CTA { get; set; }
        public string LinkUrl { get; set; }
        public string OriginalImgPath { get; set; }
        public string HostUrl { get; set; }
        public bool IsSponsored { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; }
        public bool IsDefault { get; set; }
    }
}
