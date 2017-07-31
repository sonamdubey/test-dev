using System;

namespace Bikewale.Comparison.Entities
{
    /// <summary>
    /// Created by  :   Sumit Kate on 31 Jul 2017
    /// Description :   Active Sponsored Campaign Entity
    /// </summary>
    [Serializable]
    public class ActiveSponsoredCampaign
    {
        public uint ComparisonId { get; set; }
        public uint SponsoredVersionId { get; set; }
        public uint TargetVersionId { get; set; }
        public DateTime StartDate { get; set; }
    }
}
