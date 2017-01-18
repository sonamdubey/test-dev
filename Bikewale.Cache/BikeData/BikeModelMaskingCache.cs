using Bikewale.Entities.BikeData;
using Bikewale.Entities.UserReviews;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Notifications;
using System;
using System.Collections;
using System.Collections.Generic;

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
                var htNewMaskingNames = _cache.GetFromCache<Hashtable>("BW_NewModelMaskingNames", new TimeSpan(1, 0, 0), () => _modelsRepository.GetMaskingNames());

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
                    var htOldMaskingNames = _cache.GetFromCache<Hashtable>("BW_OldModelMaskingNames", new TimeSpan(1, 0, 0), () => _modelsRepository.GetOldMaskingNames());

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
                ErrorClass objErr = new ErrorClass(ex, "GetModelMaskingResponse");
                objErr.SendMail();
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
                ErrorClass objErr = new ErrorClass(ex, "BikeModelsCacheRepository.MVSpecsFeatures");
                objErr.SendMail();
            }

            return specs;
        }

        /// <summary>
        /// Created By:-Subodh jain 9 jan 2017
        /// Description :- Added cache call for model helper
        /// </summary>
        /// <param name="modelid"></param>
        /// <returns></returns>
        public T GetById(U modelid)
        {
            T objModel = default(T);
            string key = string.Format("BW_GetModelById_{0}", modelid);
            try
            {
                objModel = _cache.GetFromCache<T>(key, new TimeSpan(1, 0, 0), () => _modelsRepository.GetById(modelid));
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, string.Format("BikeModelsCacheRepository.GetById_{0}", modelid));

            }
            return objModel;
        }

        /// <summary>
        /// Created By : Sushil Kumar on 5th Jan 2016
        /// Description : To get similar bikes with photos count
        /// </summary>
        /// <param name="versionId"></param>
        /// <returns></returns>
        public IEnumerable<SimilarBikesWithPhotos> GetSimilarBikeWithPhotos(U modelId, ushort totalRecords)
        {
            IEnumerable<SimilarBikesWithPhotos> similarBikes = null;
            string key = "BW_SimilarBikes_PhotosCnt_" + modelId;
            try
            {
                similarBikes = _cache.GetFromCache<IEnumerable<SimilarBikesWithPhotos>>(key, new TimeSpan(1, 0, 0), () => _modelsRepository.GetAlternativeBikesWithPhotos(modelId, totalRecords));
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Bikewale.Cache.BikeData.GetSimilarBikeWithPhotos");
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
                ErrorClass objErr = new ErrorClass(ex, string.Format("Bikewale.Cache.BikeData.GetDetailsByModel_ModelId_{0}_CityId_{1}", modelId, cityId));
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
                ErrorClass objErr = new ErrorClass(ex, string.Format("Bikewale.Cache.BikeData.GetDetailsByVersion_ModelId_{0}_CityId_{1}", versionId, cityId));
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
                ErrorClass objErr = new ErrorClass(ex, string.Format("Bikewale.Cache.BikeData.GetDetails_ReviewId_{0}", reviewId));
            }

            return objReview;
        }
    }
}
