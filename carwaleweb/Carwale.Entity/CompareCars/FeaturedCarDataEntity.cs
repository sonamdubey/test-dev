using System;

namespace Carwale.Entity.CompareCars
{
    [Serializable]
    public class FeaturedCarDataEntity
    {
        public string ExternalLinkText { get; set; }
        public string ExternalLinkTracker { get; set; }
        public string CarNameClickTracker { get; set; }
        public string CarImageClickTracker { get; set; }
        public string CarImpressionTracker { get; set; }
        public int FeaturedVersionId { get; set; }
        public int TargetVersionId { get; set; }
    }
}