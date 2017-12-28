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
    /// </summary>
    public interface IBikeVersionCacheRepository<T, U>
    {

        IEnumerable<SimilarBikeEntity> GetSimilarBikesList(U versionId, uint topCount, uint cityid);
        List<BikeVersionsListEntity> GetVersionsByType(EnumBikeType requestType, int modelId, int? cityId = null);
        T GetById(U versionId);
        List<BikeVersionMinSpecs> GetVersionMinSpecs(uint modelId, bool isNew);
        IEnumerable<BikeColorsbyVersion> GetColorsbyVersionId(uint versionId);
        TransposeModelSpecEntity GetSpecifications(U versionId);
        IEnumerable<BikeVersionWithMinSpec> GetDealerVersionsByModel(uint dealerId, uint modelId);
    }
}
