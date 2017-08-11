using Bikewale.Comparison.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikewaleOpr.Models.Comparison
{
    public class ManageSponsoredComparisonVM
    {
        public IEnumerable<SponsoredComparison> Sponsoredcomparisons { get; set; }
    }
}
