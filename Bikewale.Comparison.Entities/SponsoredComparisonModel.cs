using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Comparison.Entities
{
    /// <summary>
    /// Created by: Sangram Nandkhile 27-Jul-2017
    /// Summary: Entity for sponsored comparison model
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
}
