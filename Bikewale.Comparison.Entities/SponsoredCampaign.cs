using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Comparison.Entities
{
    /// <summary>
    /// Created by: Sangram Nandkhie 27-Jul-2017
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
        public uint UpdatedBy { get; set; }
    }

    /// <summary>
    /// Created by: Sangram Nandkhie 27-Jul-2017
    /// Summary: Entity for sponsored Comparison model
    /// </summary>
    public class SponsoredComparisonModel
    {
        public uint Id { get; set; }
        public uint ComparisonId { get; set; }
        public string TargetModelId { get; set; }
        public string TargetVersionId { get; set; }
        public string SponsoredModelId { get; set; }
        public string SponsoredVersionId { get; set; }
        public string ImpressionUrl { get; set; }
        public bool IsActive { get; set; }
        public DateTime EntryDate { get; set; }
    }

    /// <summary>
    /// Created by: Sangram Nandkhie 27-Jul-2017
    /// Summary: Entity for sponsored Comparison status
    /// </summary>
    public enum SponsoredComparisonStatus
    {
        Unstarted = 0,
        Active = 1,
        Paused = 2,
        Closed = 3,
        Aborted = 4
    }

    /// <summary>
    /// Created by: Sangram Nandkhie 27-Jul-2017
    /// Summary: Entity for sponsored Comparison bike version
    /// </summary>
    /// 
    public class BikeVersion
    {
        public uint Id { get; set; }
        public string Name { get; set; }
        public string ImpressionUrl { get; set; }
        public double Price { get; set; }
    }


    public class SponsoredVersionMapping
    {
        IEnumerable<BikeVersion> TargettedVersions { get; set; }
        IEnumerable<BikeVersion> SponsoredVerisions { get; set; }
    }

    public class TargetedModel
    {
        public uint ModelId { get; set; }
        public IEnumerable<BikeVersion> versionList { get; set; }
    }
}
