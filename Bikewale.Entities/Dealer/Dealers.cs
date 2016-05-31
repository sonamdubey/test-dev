using System;
using System.Collections.Generic;

namespace Bikewale.Entities.DealerLocator
{
    /// <summary>
    /// Created By : Sushil Kumar
    /// Created on : 21 March 2016
    /// Description : Wrapper for DealerList with dealer count
    /// Modified By : Vivek Gupta on 31-05-2016
    /// Desc : MakeName , CityName, CityMaskingName and MakeMaskingName retrieved
    /// </summary>
    public class DealersEntity
    {
        public IEnumerable<DealersList> Dealers { get; set; }
        public UInt16 TotalCount { get; set; }
        public string MakeName { get; set; }
        public string CityName { get; set; }

        public string CityMaskingName { get; set; }

        public string MakeMaskingName { get; set; }
    }
}
