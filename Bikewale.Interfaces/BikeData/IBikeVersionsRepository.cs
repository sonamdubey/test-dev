using Bikewale.Entities.BikeData;
using System.Collections.Generic;

namespace Bikewale.Interfaces.BikeData
{
    /// <summary>
    /// Created By : Rajan Chauhan on 11 Apr 2018
    /// Summary    : Interface for bike versions DAL
    /// Modifier    : Kartik Rathod on 11 May 2018, added GetSimilarBikesForEMI
    /// </summary>
    /// <typeparam name="T">Generic type (need to specify type while implementing this interface)</typeparam>
    /// <typeparam name="U">Generic type (need to specify type while implementing this interface)</typeparam>
    public interface IBikeVersionsRepository<T, U> : IRepository<T, U>
    {
        List<BikeVersionsListEntity> GetVersionsByType(EnumBikeType requestType, int modelId, int? cityId = null);
        BikeSpecificationEntity GetSpecifications(U versionId);
        IEnumerable<BikeVersionMinSpecs> GetVersionMinSpecs(uint modelId, bool isNew);
        IEnumerable<SimilarBikeEntity> GetSimilarBikesList(U versionId, uint topCount, uint cityId);
        IEnumerable<SimilarBikeEntity> GetSimilarBikesByModel(U modelId, uint topCount, uint cityId);
        IEnumerable<SimilarBikeEntity> GetSimilarBudgetBikes(U modelId, uint topCount, uint cityId);

        List<VersionColor> GetColorByVersion(U versionId);
        IEnumerable<BikeColorsbyVersion> GetColorsbyVersionId(uint versionId);
        IEnumerable<BikeVersionsSegment> GetModelVersionsDAL();
        IEnumerable<BikeVersionWithMinSpec> GetDealerVersionsByModel(uint dealerId, uint modelId);
        IEnumerable<SimilarBikesForEMIEntity> GetSimilarBikesForEMI(int modelId, byte topcount, int cityId);
    }
}

