namespace Bikewale.Comparison.Entities
{
    /// <summary>
    /// Created by  :   Sumit Kate on 01 Aug 2017
    /// Description :   Sponsored version Entity
    /// </summary>
    public class SponsoredVersion
    {
        public uint ComparisonId { get; set; }
        public uint SponsoredMakeId { get; set; }
        public uint SponsoredModelId { get; set; }
        public uint SponsoredVersionId { get; set; }
        public string SponsoredMakeName { get; set; }
        public string SponsoredModelName { get; set; }
        public string SponsoredVersionName { get; set; }
        public TargetVersion Target { get; set; }
    }
}
