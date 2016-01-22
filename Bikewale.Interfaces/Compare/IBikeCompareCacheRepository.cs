using Bikewale.Entities.Compare;
using System.Collections.Generic;

namespace Bikewale.Interfaces.Compare
{
    /// <summary>
    /// Created By : Lucky Rathore on 06 Nov. 2015
    /// Description : Interface to define Comapre Bike Cache.
    /// Modified by :   Sumit Kate on 22 Jan 2016
    /// Description :   Added new function
    /// </summary>
    public interface IBikeCompareCacheRepository
    {
        IEnumerable<TopBikeCompareBase> CompareList(uint topCount);
        Entities.Compare.BikeCompareEntity DoCompare(string versions);
    }
}
