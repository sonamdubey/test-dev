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
    /// Modified By : Sushil Kumar on 2nd Feb 2017
    /// Description : Added methods for BikeCompareEntity DoCompare(string versions, uint cityId) 
    /// </summary>
    public interface IBikeCompareCacheRepository
    {
        IEnumerable<TopBikeCompareBase> CompareList(uint topCount);
        BikeCompareEntity DoCompare(string versions);
        BikeCompareEntity DoCompare(string versions, uint cityId);
        ICollection<SimilarCompareBikeEntity> GetSimilarCompareBikes(string versionList, ushort topCount, int cityid);
        ICollection<SimilarCompareBikeEntity> GetSimilarCompareBikeSponsored(string versionList, ushort topCount, int cityid, uint sponsoredVersionId);
        IEnumerable<TopBikeCompareBase> ScooterCompareList(uint topCount, uint cityId);
        IEnumerable<TopBikeCompareBase> ScooterCompareList(uint topCount);
    }
}
