using Bikewale.Comparison.Entities;
using System.Collections.Generic;

namespace BikewaleOpr.Models.Comparison
{
    public class ManageSponsoredComparisonRulesVM
    {
        public uint ComparisonId { get; set; }
        public IEnumerable<SponsoredVersion> ComparisonVersionMapping {get;set;}
    }
}
