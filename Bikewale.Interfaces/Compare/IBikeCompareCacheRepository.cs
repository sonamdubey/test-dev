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
    /// Modified By:- Subidh Jain 14 march 2017
    /// Summary :- Added ScooterCompareList
    /// Modified by : Aditi Srivastava on 25 Apr 2017
    /// Summary     : Added popular compare list with new entity return type
    /// Modified by : Aditi Srivastava on 2 June 2017
    /// Summary     : Added popular comparions of scooter function
    /// Modified By :Snehal Dange on 25th Oct 2017
    /// Summary : Added GetSimilarBikesForComparisions() for similar comparisons
    /// </summary>
    public interface IBikeCompareCacheRepository
    {
        IEnumerable<TopBikeCompareBase> CompareList(uint topCount);
        BikeCompareEntity DoCompare(string versions);
        BikeCompareEntity DoCompare(string versions, uint cityId);
        ICollection<SimilarCompareBikeEntity> GetSimilarCompareBikes(string versionList, ushort topCount, int cityid);
        ICollection<SimilarCompareBikeEntity> GetSimilarCompareBikeSponsored(string versionList, ushort topCount, int cityid, uint sponsoredVersionId);
        IEnumerable<TopBikeCompareBase> ScooterCompareList(uint topCount);
        IEnumerable<SimilarCompareBikeEntity> GetPopularCompareList(uint cityId);
        IEnumerable<SimilarCompareBikeEntity> GetScooterCompareList(uint cityId);
        SimilarBikeComparisonWrapper GetSimilarBikes(string modelList, ushort topCount);
    }
}
