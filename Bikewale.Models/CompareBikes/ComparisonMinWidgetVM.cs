
using Bikewale.Entities.Compare;
using System.Collections.Generic;
namespace Bikewale.Models
{
    /// <summary>
    /// Created by Sajal Gupta on 25-03-2017
    /// This class is Wrapper for comparison widget
    /// </summary>
    public class ComparisonMinWidgetVM
    {
        public TopBikeCompareBase TopComparisonRecord { get; set; }
        public IEnumerable<TopBikeCompareBase> RemainingCompareList { get; set; }
        public bool ShowComparisonButton { get; set; }
    }
}
