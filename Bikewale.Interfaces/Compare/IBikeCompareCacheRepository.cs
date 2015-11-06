using Bikewale.Entities.Compare;
using System.Collections.Generic;

namespace Bikewale.Interfaces.Compare
{
    /// <summary>
    /// Created By : Lucky Rathore on 06-11-2015
    /// Description : Interface to define Comapre Bike Cache.
    /// </summary>
    public interface IBikeCompareCacheRepository
    {
        IEnumerable<TopBikeCompareBase> CompareList(uint topCount);
    }
}
