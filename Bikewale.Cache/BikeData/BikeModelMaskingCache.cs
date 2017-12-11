using System;
using System.Collections;
using System.Collections.Generic;
using Bikewale.Entities;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.UserReviews;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Notifications;

namespace Bikewale.Cache.BikeData
{
    public class BikeModelMaskingCache<T, U> : IBikeMaskingCacheRepository<T, U> where T : BikeModelEntity, new()
    {
        private readonly ICacheManager _cache;
        private readonly IBikeModelsRepository<T, U> _modelsRepository;

        public BikeModelMaskingCache(ICacheManager cache, IBikeModelsRepository<T, U> modelsRepository)
        {
            _cache = cache;
            _modelsRepository = modelsRepository;
        }

        public ModelMaskingResponse GetModelMaskingResponse(string maskingName)
        {
            ModelMaskingResponse response = new ModelMaskingResponse();

            try
            {
                // Get MaskingNames from Memcache
                var htNewMaskingNames = _cache.GetFromCache<Hashtable>("BW_NewModelMaskingNames_v1", new TimeSpan(1, 0, 0), () => _modelsRepository.GetMaskingNames());

                if (htNewMaskingNames != null && htNewMaskingNames.Contains(maskingName))
                {
                    response.ModelId = Convert.ToUInt32(htNewMaskingNames[maskingName]);
                }

                // If modelId is not null 
                if (response.ModelId > 0)
                {
                    response.MaskingName = maskingName;
                    response.StatusCode = 200;

                    return response;
                }
                else
                {
                    // Get old MaskingNames from memcache
                    var htOldMaskingNames = _cache.GetFromCache<Hashtable>("BW_OldModelMaskingNames_v1", new TimeSpan(1, 0, 0), () => _modelsRepository.GetOldMaskingNames());

                    // new masking name found for given masking name. Its renamed so 301 permanant redirect.
                    if (htOldMaskingNames != null && htOldMaskingNames[maskingName] != null)
                    {
                        response.MaskingName = htOldMaskingNames[maskingName].ToString();
                        response.StatusCode = 301;
                    }
                    else
                        response.StatusCode = 404;                // Not found. The given masking name does not exist on bikewale.
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "GetModelMaskingResponse");
                
            }

            return response;
        }
        /// <summary>
        /// Created By : Lucky Rathore On 07 June 2016
        /// Description : To cache version Specification Detail.
        /// </summary>
        /// <param name="versionId"></param>
        /// <returns></returns>
        public BikeSpecificationEntity MVSpecsFeatures(int versionId)
        {
            BikeSpecificationEntity specs = null;
            string key = "BW_VersionSpecs_" + versionId;

            try
            {
                specs = _cache.GetFromCache<BikeSpecificationEntity>(key, new TimeSpan(1, 0, 0), () => _modelsRepository.MVSpecsFeatures(versionId));
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "BikeModelsCacheRepository.MVSpecsFeatures");
                
            }

            return specs;
        }

        /// <summary>
        /// Created By:-Subodh jain 9 jan 2017
        /// Description :- Added cache call for model helper
        /// Modified By: Snehal Dange on 13thOct 2017
        /// Description : - Versioned the cache key
        /// </summary>
        /// <param name="modelid"></param>
        /// <returns></returns>
        public T GetById(U modelid)
        {
            T objModel = default(T);
            string key = string.Format("BW_GetModelById_{0}_V1", modelid);
            try
            {
                objModel = _cache.GetFromCache<T>(key, new TimeSpan(1, 0, 0), () => _modelsRepository.GetById(modelid));
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("BikeModelsCacheRepository.GetById_{0}", modelid));

            }
            return objModel;
        }

        /// <summary>
        /// Created By : Sushil Kumar on 5th Jan 2016
        /// Description : To get similar bikes with photos count
        /// Modified By :Snehal Dange on 6th September 2017
        /// Description : Added CityId to get Exshowroom price or OnRoad Price
        /// </summary>
        /// <param name="versionId"></param>
        /// <returns></returns>
        public IEnumerable<SimilarBikesWithPhotos> GetSimilarBikeWithPhotos(U modelId, ushort totalRecords,uint cityId)
        {
            IEnumerable<SimilarBikesWithPhotos> similarBikes = null;
            try
            {
                if(cityId >0)
                {
                  
                    string key = string.Format("BW_SimilarBikes_Photos_{0}_City_{1}", modelId, cityId) ;
                    similarBikes = _cache.GetFromCache<IEnumerable<SimilarBikesWithPhotos>>(key, new TimeSpan(1, 0, 0), () => _modelsRepository.GetAlternativeBikesWithPhotosInCity(modelId, totalRecords, cityId));
                }
                else
                {
                    string key = "BW_SimilarBikes_Photos_" + modelId;
                    similarBikes = _cache.GetFromCache<IEnumerable<SimilarBikesWithPhotos>>(key, new TimeSpan(6, 0, 0), () => _modelsRepository.GetAlternativeBikesWithPhotos(modelId, totalRecords));
                }
               
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.Cache.BikeData.GetSimilarBikeWithPhotos");
            }

            return similarBikes;
        }
        /// <summary>
        /// Created By :- Subodh Jain 17 Jan 2017
        /// Summary :- Get details by model and city for review
        /// </summary>
        /// <param name="reviewId"></param>
        /// <param name="isAlreadyViewed"></param>
        public ReviewDetailsEntity GetDetailsByModel(U modelId, uint cityId)
        {
            ReviewDetailsEntity objReview = null;
            string key = string.Format("BW_DetailsByModel_ModelId_{0}_CityId_{1}", modelId, cityId);
            try
            {
                objReview = _cache.GetFromCache<ReviewDetailsEntity>(key, new TimeSpan(1, 0, 0), () => _modelsRepository.GetDetailsByModel(modelId, cityId));
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("Bikewale.Cache.BikeData.GetDetailsByModel_ModelId_{0}_CityId_{1}", modelId, cityId));
            }

            return objReview;
        }
        /// <summary>
        /// Created By :- Subodh Jain 17 Jan 2017
        /// Summary :- Get details by version and city for review
        /// </summary>
        /// <param name="reviewId"></param>
        /// <param name="isAlreadyViewed"></param>
        public ReviewDetailsEntity GetDetailsByVersion(U versionId, uint cityId)
        {
            ReviewDetailsEntity objReview = null;
            string key = string.Format("BW_DetailsByVersion_VersionId_{0}_CityId_{1}", versionId, cityId);
            try
            {
                objReview = _cache.GetFromCache<ReviewDetailsEntity>(key, new TimeSpan(1, 0, 0), () => _modelsRepository.GetDetailsByVersion(versionId, cityId));
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("Bikewale.Cache.BikeData.GetDetailsByVersion_ModelId_{0}_CityId_{1}", versionId, cityId));
            }

            return objReview;
        }
        /// <summary>
        /// Created By :- Subodh Jain 17 Jan 2017
        /// Summary :- Get details for review
        /// </summary>
        /// <param name="reviewId"></param>
        /// <param name="isAlreadyViewed"></param>
        public ReviewDetailsEntity GetDetails(string reviewId, bool isAlreadyViewed)
        {
            ReviewDetailsEntity objReview = null;
            string key = string.Format("BW_Details_ReviewId_{0}", reviewId);
            try
            {
                objReview = _cache.GetFromCache<ReviewDetailsEntity>(key, new TimeSpan(1, 0, 0), () => _modelsRepository.GetDetails(reviewId, isAlreadyViewed));
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("Bikewale.Cache.BikeData.GetDetails_ReviewId_{0}", reviewId));
            }
            return objReview;
        }
        /// <summary>
        /// Created by :- Subodh jain 3 feb 2017
        /// Summary :- Get Make if video is present
        /// </summary>
        /// <returns></returns>
        public IEnumerable<BikeMakeEntityBase> GetMakeIfVideo()
        {
            IEnumerable<BikeMakeEntityBase> objVideoMake = null;
            string key = "BW_Details_GetMakeIfVideo";
            try
            {
                objVideoMake = _cache.GetFromCache<IEnumerable<BikeMakeEntityBase>>(key, new TimeSpan(1, 0, 0), () => _modelsRepository.GetMakeIfVideo());
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.Cache.BikeData.GetMakeIfVideo");
            }
            return objVideoMake;
        }
        /// <summary>
        /// Created by :- Subodh Jain 3 feb 2017
        /// Summary :- Bind Video details for similar bikes
        /// </summary>
        /// <param name="modelId"></param>
        /// <param name="totalRecords"></param>
        /// <returns></returns>
        public IEnumerable<SimilarBikeWithVideo> GetSimilarBikesVideos(uint modelId, uint totalcount,uint cityid)
        {
            IEnumerable<SimilarBikeWithVideo> similarBikes = null;
            string key = string.Format("BW_SimilarBikes_VideoCnt_V1_{0}", modelId);
            try
            {
                similarBikes = _cache.GetFromCache<IEnumerable<SimilarBikeWithVideo>>(key, new TimeSpan(1, 0, 0), () => _modelsRepository.GetSimilarBikesVideos(modelId, totalcount, cityid));
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("Bikewale.Cache.BikeData.GetSimilarBikesVideos_modelid: {0}", modelId));
            }

            return similarBikes;
        }

        public IEnumerable<SimilarBikeUserReview> GetSimilarBikesUserReviews(uint modelId, uint cityId, uint totalRecords)
        {
            IEnumerable<SimilarBikeUserReview> similarBikes = null;
            string key = null;

            try
            {
                if (cityId > 0)
                {
                    key = string.Format("BW_SimilarBikes_UserReviews_{0}_City_{1}", modelId, cityId);
                    similarBikes = _cache.GetFromCache<IEnumerable<SimilarBikeUserReview>>(key, new TimeSpan(1, 0, 0), () => _modelsRepository.GetSimilarBikesUserReviewsWithPriceInCity(modelId, cityId, totalRecords));
                }
                else
                {
                    key = string.Format("BW_SimilarBikes_UserReviews_{0}_V1", modelId);
                    similarBikes = _cache.GetFromCache<IEnumerable<SimilarBikeUserReview>>(key, new TimeSpan(1, 0, 0), () => _modelsRepository.GetSimilarBikesUserReviewsWithPrice(modelId, totalRecords));
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("Bikewale.Cache.BikeData.GetSimilarBikesVideos_modelid: {0}", modelId));
            }

            return similarBikes;
        }


        
    }
}
