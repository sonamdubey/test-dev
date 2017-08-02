using Bikewale.Comparison.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikewaleOpr.Models.Comparison
{
    public class ManageSponsoredComparisonRulesVM
    {
        public uint ComparisonId { get; set; }
        public IEnumerable<SponsoredVersion> ComparisonVersionMapping {get;set;}
    }
}
