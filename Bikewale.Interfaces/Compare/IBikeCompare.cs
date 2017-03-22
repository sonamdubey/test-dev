using Bikewale.Entities.BikeData;
using Bikewale.Entities.Compare;
using System;
using System.Collections.Generic;

namespace Bikewale.Interfaces.Compare
{
    /// <summary>
    /// Updated by : Sangram Nandkhile on 12 May 2016
    /// Desc       : Added GetSimilarCompareBikes functions
    /// Modified by :Subodh Jain on 21 oct 2016
    /// Desc : Added cityid as parameter
    /// Modified By : Sushil Kumar on 2nd Dec 2016
    /// Description : Added methods for similar comapre bikes and sponsored comparision bikes 
    /// Modified By : Sushil Kumar on 2nd Feb 2017
    /// Description : Added methods for BikeCompareEntity DoCompare(string versions, uint cityId) 
    /// Modified By:- Subidh Jain 14 march 2017
    /// Summary :- Added ScooterCompareList
    /// </summary>
    public interface IBikeCompare
    {
        BikeCompareEntity DoCompare(string versions);
        BikeCompareEntity DoCompare(string versions, uint cityId);
        IEnumerable<TopBikeCompareBase> CompareList(uint topCount);
        ICollection<SimilarCompareBikeEntity> GetSimilarCompareBikes(string versionList, ushort topCount, int cityid);
        ICollection<SimilarCompareBikeEntity> GetSimilarCompareBikeSponsored(string versionList, ushort topCount, int cityid, uint sponsoredVersionId);
        Int64 GetFeaturedBike(string versions);
        IEnumerable<TopBikeCompareBase> ScooterCompareList(uint topCount);
    }
}
