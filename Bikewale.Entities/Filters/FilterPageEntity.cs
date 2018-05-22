using System.Collections.Generic;

namespace Bikewale.Entities.Filters
{
    /// <summary>
    /// Created by : Snehal Dange on 20th Feb 2018
    /// Description: Filter wrapper with all the required filters for the page
    /// Modifid by: Dhruv Joshi
    /// Dated: 23rd Feb 2018
    /// Description: FilterResults changed from IEnumerable to List
    /// </summary>
    public class FilterPageEntity
    {
        public List<FilterBase> FilterResults { get; set; }
    }
}
