using System;
using System.Collections.Generic;

namespace Bikewale.Entities.DealerLocator
{
    /// <summary>
    /// Created By : Sushil Kumar
    /// Created on : 21 March 2016
    /// Description : Wrapper for DealerList with dealer count
    /// </summary>
    public class Dealers
    {
        public IEnumerable<DealersList> DealerList { get; set; }
        public UInt16 TotalCount { get; set; }
    }
}
