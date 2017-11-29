namespace Bikewale.Comparison.Entities
{
    /// <summary>
    /// Created by: Sangram Nandkhile on 28-Jul-2017
    /// Summary: Entity for target version mapping
    /// 
    /// </summary>
    public class VersionTargetMapping
    {
        public uint ComparisonId { get; set; }
        public bool IsVersionMapping { get; set; }
        public string TargetSponsoredIds { get; set; }
        public string ImpressionUrl { get; set; }
    }
}
