using Bikewale.Entities.BikeData;
using Bikewale.Entities.Compare;
//using Bikewale.Entities.Compare.V2;
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
    ///  Modified by : Aditi Srivastava on 24 Apr 2017
    /// Summary     : Added popular compare list with new entity return type
    /// Modified by : Aditi Srivastava on 2 June 2017
    /// Summary     : Added new function for comparison for popular scooters
    /// Modified By: Snehal Dange on 24th Oct 2017
    /// Summary : Added function GetSimilarBikesForComparisions for similar bikes for comparison
    /// </summary>
    public interface IBikeCompare
    {
        Entities.Compare.BikeCompareEntity DoCompare(string versions);
        Entities.Compare.BikeCompareEntity DoCompare(string versions, uint cityId);
        IEnumerable<TopBikeCompareBase> CompareList(uint topCount);
        ICollection<SimilarCompareBikeEntity> GetSimilarCompareBikes(string versionList, ushort topCount, int cityid);
        ICollection<SimilarCompareBikeEntity> GetSimilarCompareBikeSponsored(string versionList, ushort topCount, int cityid, uint sponsoredVersionId);
        IEnumerable<TopBikeCompareBase> ScooterCompareList(uint topCount);
        IEnumerable<SimilarCompareBikeEntity> GetPopularCompareList(uint cityId);
        IEnumerable<SimilarCompareBikeEntity> GetScooterCompareList(uint cityId);
        SimilarBikeComparisonWrapper GetSimilarBikes(string modelList, ushort topCount);
    }
}
