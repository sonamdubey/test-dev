using Bikewale.Entities.BikeData;
using System.Collections.Generic;

namespace Bikewale.Interfaces.BikeData
{

    /// <summary>
    /// Created by  :   Sushil Kumar on 28th June 2016
    /// Description :   Bike Versions  Cache Repository
    /// Modified by :   Aditi Srivastava on 20 Oct 2016
    /// Description :   Added method to get versions colors by version id
    /// Modified by : Ashutosh Sharma on 26 Dec 2017
    /// Description : Added GetSpecifications.
    /// Modified by :   Rajan Chauhan on 11 Apr 2017
    /// Description :   Removed GetSpecifications.
    /// Modifier    : Kartik Rathod on 11 May 2018, added GetSimilarBikesForEMI
    /// </summary>
    public interface IBikeVersionCacheRepository<T, U>
    {

        IEnumerable<SimilarBikeEntity> GetSimilarBikesList(U versionId, uint topCount, uint cityid);
        IEnumerable<SimilarBikeEntity> GetSimilarBikesByModel(U modelId, uint topCount, uint cityid);
        IEnumerable<SimilarBikeEntity> GetSimilarBikesByMinPriceDiff(U modelId, uint topCount, uint cityid);
        List<BikeVersionsListEntity> GetVersionsByType(EnumBikeType requestType, int modelId, int? cityId = null);
        T GetById(U versionId);
        IEnumerable<BikeVersionMinSpecs> GetVersionMinSpecs(uint modelId, bool isNew);
        IEnumerable<BikeColorsbyVersion> GetColorsbyVersionId(uint versionId);
        IEnumerable<BikeVersionWithMinSpec> GetDealerVersionsByModel(uint dealerId, uint modelId);
        IEnumerable<SimilarBikesForEMIEntity> GetSimilarBikesForEMI(int versionId, short topcount, int cityId);
    }
}
