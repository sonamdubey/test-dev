using System.Collections.Generic;

namespace Bikewale.Entities.Filters
{
    /// <summary>
    /// Created by : Snehal Dange on 20th Feb 2018
    /// Description: Filter wrapper with all the required filters for the page
    /// </summary>
    public class FilterPageEntity
    {
        public IEnumerable<IEnumerable<FilterBase>> FilterResults { get; set; }
    }
}
