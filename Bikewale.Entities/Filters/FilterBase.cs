using System.Collections.Generic;

namespace Bikewale.Entities.Filters
{
    /// <summary>
    /// Created by : Snehal Dange on 20th Feb 2018
    /// Description: Entity created to store list of filtered results .
    /// </summary>
    public class FilterBase
    {
        public IList<uint> RangeList { get; set; }
        public string Unit { get; set; }
    }
}
