namespace Bikewale.Comparison.Entities
{
    /// <summary>
    /// Created by  :   Sumit Kate on 31 Jul 2017
    /// Description :   Sponsored Version Entity Base
    /// </summary>
    public class SponsoredVersionEntityBase
    {
        public uint ComparisonId { get; set; }
        public uint VersionId { get; set; }
        public string BikeImpressionTracker { get; set; }
        public string ImgImpressionUrl { get; set; }
        public string LinkText { get; set; }
        public string LinkUrl { get; set; }
    }
}
