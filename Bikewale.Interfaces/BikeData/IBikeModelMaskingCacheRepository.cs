using Bikewale.Entities.BikeData;

namespace Bikewale.Interfaces.BikeData
{
    /// <summary>
    /// Modified By : Lucky Rathore on 07 June 2016
    /// Description : BikeSpecificationEntity MVSpecsFeatures(int versionId) added for caching specification and feature of versions.
    /// modified By:-Subodh jain 9 jan 2017
    /// Description :- Added cache call
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="U"></typeparam>
    public interface IBikeMaskingCacheRepository<T, U>
    {
        ModelMaskingResponse GetModelMaskingResponse(string maskingName);
        BikeSpecificationEntity MVSpecsFeatures(int versionId);
        IEnumerable<SimilarBikesWithPhotos> GetSimilarBikeWithPhotos(U modelId, ushort totalRecords);
        T GetById(U id);
    }
}
