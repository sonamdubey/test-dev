using Bikewale.Entities;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.UserReviews;
using System.Collections.Generic;

namespace Bikewale.Interfaces.BikeData
{
    /// <summary>
    /// Modified By : Lucky Rathore on 07 June 2016
    /// Description : BikeSpecificationEntity MVSpecsFeatures(int versionId) added for caching specification and feature of versions.
    /// Modified By : Sushil Kumar on 5th Jan 2016
    /// Description : To get similar bikes with photos count
    /// modified By:-Subodh jain 9 jan 2017
    /// Description :- Added cache call
    ///modified By:-Subodh jain 17 jan 2017
    /// Description :- Added GetDetailsByModel,GetDetailsByVersion,GetDetails
    /// Modified By :- Subodh Jain 2 Feb 2017
    /// Summary :- Added get make if video is present
    /// Modified By :- Subodh Jain 2 Feb 2017
    /// Summary :- Added GetSimilarBikesVideos method
    /// Modified By :- Sajal Gupta on 08-05-2017
    /// Summary :- Added GetSimilarBikesUserReviews method
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="U"></typeparam>
    public interface IBikeMaskingCacheRepository<T, U>
    {
        ModelMaskingResponse GetModelMaskingResponse(string maskingName);
        IEnumerable<SimilarBikesWithPhotos> GetSimilarBikeWithPhotos(U modelId, ushort totalRecords, uint cityId);
        ReviewDetailsEntity GetDetailsByModel(U modelId, uint cityId);
        ReviewDetailsEntity GetDetailsByVersion(U versionId, uint cityId);
        ReviewDetailsEntity GetDetails(string reviewId, bool isAlreadyViewed);
        IEnumerable<BikeMakeEntityBase> GetMakeIfVideo();
        IEnumerable<SimilarBikeWithVideo> GetSimilarBikesVideos(uint modelId, uint totalcount,uint cityid);
        IEnumerable<SimilarBikeUserReview> GetSimilarBikesUserReviews(uint modelId, uint cityId, uint totalRecords);
        T GetById(U id);
    }
}
