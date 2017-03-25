
using Bikewale.Entities.Compare;
using System.Collections.Generic;
namespace Bikewale.Models.CompareBikes
{
    public class ComparisonMinWidgetVM
    {
        public TopBikeCompareBase TopComparisonRecord { get; set; }
        public IEnumerable<TopBikeCompareBase> RemainingCompareList { get; set; }
        public bool ShowComparisonButton { get; set; }
    }
}
