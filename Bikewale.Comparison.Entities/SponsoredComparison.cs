using System;

namespace Bikewale.Comparison.Entities
{
    /// <summary>
    /// Created by: Sangram Nandkhile 27-Jul-2017
    /// Summary: Entity for sponsored Comparison
    /// </summary>
    public class SponsoredComparison
    {
        public uint Id { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string LinkText { get; set; }
        public string LinkUrl { get; set; }
        public string NameImpressionUrl { get; set; }
        public string ImgImpressionUrl { get; set; }
        public SponsoredComparisonStatus Status { get; set; }
        public DateTime EntryDate { get; set; }
        public DateTime LastUpdated { get; set; }
        public string UpdatedBy { get; set; }
    }
}
