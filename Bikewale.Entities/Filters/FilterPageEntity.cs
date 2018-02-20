using System.Collections.Generic;

namespace Bikewale.Entities.Filters
{
    /// <summary>
    /// Created by : Snehal Dange on 20th Feb 2018
    /// Description: Filter wrapper with all the required filters for the page
    /// </summary>
    public class FilterPageEntity
    {
        public IList<FilterBase> FirstFilter { get; set; }
        public IList<FilterBase> SecondFilter { get; set; }
        public IList<FilterBase> ThirdFilter { get; set; }
        public IList<FilterBase> FourthFilter { get; set; }
    }
}
