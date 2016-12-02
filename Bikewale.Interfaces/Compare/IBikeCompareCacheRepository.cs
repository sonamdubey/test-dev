using Bikewale.Entities.BikeData;
using Bikewale.Entities.Compare;
using System.Collections.Generic;

namespace Bikewale.Interfaces.Compare
{
    /// <summary>
    /// Created By : Lucky Rathore on 06 Nov. 2015
    /// Description : Interface to define Comapre Bike Cache.
    /// Modified by :   Sumit Kate on 22 Jan 2016
    /// Description :   Added new function
    /// Modified By : Sushil Kumar on 2nd Dec 2016
    /// Description : Added methods for comparision bikes comparisions
    /// </summary>
    public interface IBikeCompareCacheRepository
    {
        IEnumerable<TopBikeCompareBase> CompareList(uint topCount);
        Entities.Compare.BikeCompareEntity DoCompare(string versions);
        ICollection<SimilarCompareBikeEntity> GetSimilarCompareBikes(string versionList, ushort topCount, int cityid);
        ICollection<SimilarCompareBikeEntity> GetSimilarCompareBikeSponsored(string versionList, ushort topCount, int cityid, uint sponsoredVersionId);

    }
}
