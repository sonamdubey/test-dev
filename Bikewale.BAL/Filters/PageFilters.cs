using Bikewale.Interfaces.Filters;
using System;
using System.Collections.Generic;

namespace Bikewale.BAL.Filters
{
    /// <summary>
    /// Created by : Snehal Dange on 20th Feb 2018
    /// Description: Class contains the logic to give customized filters with respect to make.
    /// </summary>
    public class PageFilters : IPageFilters
    {
        /// <summary>
        /// Following dictionary contains the filtersPriority - MakeCategories mapping  
        /// </summary>
        IDictionary<uint, IList<UInt16>> categoryFilterPriorityMap = new Dictionary<uint, IList<UInt16>>
        {
            {1,new List<UInt16> {1,3,4}},
            {2,new List<UInt16> {1,2,3,4}},
            {3,new List<UInt16> {1,2,3,4}},
            {4,new List<UInt16> {1,2,4}},
            {5,new List<UInt16> {1,2,4}}
        };

    }
}
