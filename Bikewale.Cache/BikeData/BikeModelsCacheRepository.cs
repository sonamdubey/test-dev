using Bikewale.Entities.BikeData;
using Bikewale.Entities.CMS.Photos;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bikewale.Cache.BikeData
{
    /// <summary>
    /// Written By : Ashish G. Kamble on 7 Oct 2015
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="U"></typeparam>
    public class BikeModelsCacheRepository<T, U> : IBikeModelsCacheRepository<U>
    {
        private readonly ICacheManager _cache;
        private readonly IBikeModels<T, U> _objModels;
        private readonly IBikeModelsRepository<T, U> _modelRepository;

        /// <summary>
        /// Intitalize the references for the cache and BL
        /// </summary>
        /// <param name="cache"></param>
        /// <param name="objModels"></param>
        public BikeModelsCacheRepository(ICacheManager cache, IBikeModels<T, U> objModels, IBikeModelsRepository<T, U> modelRepository)
        {
            _cache = cache;
            _objModels = objModels;
            _modelRepository = modelRepository;
        }

        /// <summary>
        /// Written By : Ashish G. Kamble on 8 Oct 2015
        /// Summary : Function to get the model page details from the cache. If data is not available in the cache it will return data from BL.
        /// </summary>
        /// <param name="modelId"></param>
        /// <returns>Returns BikeModelPageEntity</returns>
        public BikeModelPageEntity GetModelPageDetails(U modelId)
        {
            BikeModelPageEntity objModelPage = null;
            string key = "BW_ModelDetails_" + modelId;

            try
            {
                objModelPage = _cache.GetFromCache<BikeModelPageEntity>(key, new TimeSpan(1, 0, 0), () => _objModels.GetModelPageDetails(modelId));
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "BikeModelsCacheRepository.GetModelPageDetails");
                objErr.SendMail();
            }

            return objModelPage;
        }
        /// <summary>
        /// Created By: Sangram Nandkhile on 01 Dec 2016
        /// Summary: To Create a overload of cached model entity with version Id
        /// </summary>
        public BikeModelPageEntity GetModelPageDetails(U modelId, int versionId)
        {
            BikeModelPageEntity objModelPage = null;
            string key = string.Format("BW_ModelDetails_{0}", modelId);
            try
            {
                objModelPage = _cache.GetFromCache<BikeModelPageEntity>(key, new TimeSpan(1, 0, 0), () => _objModels.GetModelPageDetails(modelId, versionId));
                if (objModelPage.ModelVersions != null && objModelPage.ModelVersions.Count() > 1)
                {
                    // First page load where version id is Zero, fetch default version properties
                    versionId = versionId == 0 ? (int)objModelPage.ModelVersionSpecs.BikeVersionId : 0;
                    objModelPage.ModelVersionSpecs = objModelPage.ModelVersionSpecsList.FirstOrDefault(m => m.BikeVersionId == (uint)versionId);
                    objModelPage.objOverview = objModelPage.TransposeModelSpecs.FirstOrDefault(m => m.BikeVersionId == versionId).objOverview;
                    objModelPage.objSpecs = objModelPage.TransposeModelSpecs.FirstOrDefault(m => m.BikeVersionId == versionId).objSpecs;
                    objModelPage.objFeatures = objModelPage.TransposeModelSpecs.FirstOrDefault(m => m.BikeVersionId == versionId).objFeatures;
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, string.Format("BikeModelsCacheRepository.GetModelPageDetails() => modelid {0}, versionId: {1}", modelId, versionId));
                objErr.SendMail();
            }

            return objModelPage;
        }
        /// <summary>
        /// Created by Subodh Jain 12 oct 2016
        /// Desc For getting colour count
        /// </summary>
        /// <param name="modelId"></param>
        /// <returns></returns>
        public IEnumerable<NewBikeModelColor> GetModelColor(U modelId)
        {
            IEnumerable<NewBikeModelColor> objModelPage = null;
            string key = "BW_ModelColor_" + modelId;
            try
            {
                objModelPage = _cache.GetFromCache<IEnumerable<NewBikeModelColor>>(key, new TimeSpan(1, 0, 0), () => _objModels.GetModelColor(modelId));
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "BikeModelsCacheRepository.GetModelColor");
                objErr.SendMail();
            }

            return objModelPage;

        }


        /// <summary>
        /// Written By : Sushil Kumar on 28th June 2016
        /// Summary : Function to get the upcoming bikes. If data is not available in the cache it will return data from BL.
        /// Modified by :   Sumit Kate on 08 Jul 2016
        /// Description :   Consider PageNo for Memcache key formation
        /// </summary>
        /// <param name="sortBy"></param>
        /// <param name="pageSize"></param>
        /// <param name="makeId"></param>
        /// <param name="modelId"></param>
        /// <param name="curPageNo"></param>
        /// <returns>Returns List<UpcomingBikeEntity></returns>
        public IEnumerable<UpcomingBikeEntity> GetUpcomingBikesList(EnumUpcomingBikesFilter sortBy, int pageSize, int? makeId = null, int? modelId = null, int? curPageNo = null)
        {
            IEnumerable<UpcomingBikeEntity> objUpcoming = null;
            string key = string.Format("BW_UpcomingBikes_Cnt_{0}_SO_{1}", pageSize, (int)sortBy);

            if (makeId.HasValue && makeId.Value > 0)
                key += "_MK_" + makeId;

            if (modelId.HasValue && modelId.Value > 0)
                key += "_MO_" + modelId;
            if (curPageNo.HasValue && curPageNo.Value > 0)
            {
                key += "_PgNo_" + curPageNo.Value;
            }
            try
            {
                objUpcoming = _cache.GetFromCache<IEnumerable<UpcomingBikeEntity>>(key, new TimeSpan(1, 0, 0), () => _objModels.GetUpcomingBikesList(sortBy, pageSize, makeId, modelId, curPageNo));
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "BikeModelsCacheRepository.GetUpcomingBikesList");
                objErr.SendMail();
            }

            return objUpcoming;
        }

        /// <summary>
        /// Written By : Sushil Kumar on 28th June 2016
        /// Summary : Function to get the popular bikes by make . If data is not available in the cache it will return data from BL.
        /// </summary>
        /// <param name="modelId"></param>
        /// <returns>Returns BikeModelPageEntity</returns>
        public IEnumerable<MostPopularBikesBase> GetMostPopularBikesByMake(int makeId)
        {
            IEnumerable<MostPopularBikesBase> objBikes = null;
            string key = "BW_PopularBikesByMake_" + makeId;

            try
            {
                objBikes = _cache.GetFromCache<IEnumerable<MostPopularBikesBase>>(key, new TimeSpan(1, 0, 0), () => _modelRepository.GetMostPopularBikesByMake(makeId));
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "BikeModelsCacheRepository.GetMostPopularBikesByMake");
                objErr.SendMail();
            }

            return objBikes;
        }
        /// <summary>
        /// Created by :Subodh Jain 22 sep 2013
        /// Des: method to get popular bike by make and city
        /// </summary>
        /// <param name="topCount"></param>
        /// <param name="makeId"></param>
        /// <param name="cityId"></param>
        /// <returns></returns>
        public IEnumerable<MostPopularBikesBase> GetMostPopularBikesbyMakeCity(uint topCount, uint makeId, uint cityId)
        {
            IEnumerable<MostPopularBikesBase> objBikes = null;
            string key = "BW_PopularBikesByMake_" + makeId;
            if (cityId > 0)
                key = key + "_City_" + cityId;

            try
            {
                objBikes = _cache.GetFromCache<IEnumerable<MostPopularBikesBase>>(key, new TimeSpan(1, 0, 0), () => _objModels.GetMostPopularBikesbyMakeCity(topCount, makeId, cityId));
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "BikeModelsCacheRepository.GetMostPopularBikesByMake");
                objErr.SendMail();
            }

            return objBikes;
        }

        /// <summary>
        /// Written By : Sushil Kumar on 30th June 2016
        /// Summary : Function to get the model dscription from the cache. If data is not available in the cache it will return data from BL.
        /// </summary>
        /// <param name="modelId"></param>
        /// <returns>Returns BikeDescriptionEntity</returns>
        public BikeDescriptionEntity GetModelSynopsis(U modelId)
        {
            BikeDescriptionEntity objModelPage = null;
            string key = "BW_ModelDesc_" + modelId;

            try
            {
                objModelPage = _cache.GetFromCache<BikeDescriptionEntity>(key, new TimeSpan(1, 0, 0), () => _modelRepository.GetModelSynopsis(modelId));
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "BikeModelsCacheRepository.GetModelSynopsis");
                objErr.SendMail();
            }

            return objModelPage;
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 01 Jul 2016
        /// Description :   Returns New Launched Bike List
        /// </summary>
        /// <param name="startIndex"></param>
        /// <param name="endIndex"></param>
        /// <param name="recordCount"></param>
        /// <returns></returns>
        public NewLaunchedBikesBase GetNewLaunchedBikesList(int startIndex, int endIndex, int? makeid = null)
        {
            NewLaunchedBikesBase objBikes = null;
            string key = String.Format("BW_NewLaunchedBikes_SI_{0}_EI_{1}", startIndex, endIndex);
            if (makeid.HasValue && makeid > 0)
                key = key + String.Format("_MKID_{0}", makeid);

            try
            {
                if (makeid.HasValue && makeid > 0)
                    objBikes = _cache.GetFromCache<NewLaunchedBikesBase>(key, new TimeSpan(1, 0, 0), () => _modelRepository.GetNewLaunchedBikesListByMake(startIndex, endIndex, makeid));
                else
                    objBikes = _cache.GetFromCache<NewLaunchedBikesBase>(key, new TimeSpan(1, 0, 0), () => _modelRepository.GetNewLaunchedBikesList(startIndex, endIndex));
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "BikeModelsCacheRepository.GetMostPopularBikesByMake");
                objErr.SendMail();
            }

            return objBikes;
        }
        public NewLaunchedBikesBase GetNewLaunchedBikesListByMake(int startIndex, int endIndex, int? makeid = null)
        {
            NewLaunchedBikesBase objBikes = null;
            string key = String.Format("BW_NewLaunchedBikes_SI_{0}_EI_{1}_MKID_{2}", startIndex, endIndex, makeid);

            try
            {
                objBikes = _cache.GetFromCache<NewLaunchedBikesBase>(key, new TimeSpan(1, 0, 0), () => _modelRepository.GetNewLaunchedBikesListByMake(startIndex, endIndex, makeid));
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "BikeModelsCacheRepository.GetMostPopularBikesByMake");
                objErr.SendMail();
            }

            return objBikes;
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 01 Jul 2016
        /// Description :   GetMostPopularBikes Caching
        /// </summary>
        /// <param name="topCount"></param>
        /// <param name="makeId"></param>
        /// <returns></returns>
        public IEnumerable<MostPopularBikesBase> GetMostPopularBikes(int? topCount = null, int? makeId = null)
        {
            IEnumerable<MostPopularBikesBase> objBikes = null;
            string key = "BW_PopularBikes" + (topCount.HasValue ? String.Format("_TC_{0}", topCount.Value) : "") + (makeId.HasValue ? String.Format("_MK_{0}", makeId.Value) : "");
            try
            {
                objBikes = _cache.GetFromCache<IEnumerable<MostPopularBikesBase>>(key, new TimeSpan(1, 0, 0), () => _modelRepository.GetMostPopularBikes(topCount, makeId));
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "BikeModelsCacheRepository.GetMostPopularBikesByMake");
                objErr.SendMail();
            }

            return objBikes;
        }


        /// <summary>
        /// Created by  : Sushil Kumar on 20th July 2016
        /// Description : Bike Models photos gallery caching
        /// </summary>
        /// <param name="modelId"></param>
        /// <param name="contentList"></param>
        /// <returns></returns>
        public List<ModelImage> GetModelPhotoGallery(U modelId)
        {
            List<ModelImage> objPhotos = null;

            string key = string.Format("BW_ModelPhotoGallery_MO_{0}", modelId);
            try
            {
                objPhotos = _cache.GetFromCache<List<ModelImage>>(key, new TimeSpan(1, 0, 0), () => _objModels.GetBikeModelPhotoGallery(modelId));
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "BikeModelsCacheRepository.GetModelPhotoGallery");
                objErr.SendMail();
            }

            return objPhotos;
        }

        /// <summary>
        /// Created By: Aditi Srivastava on 17th Aug, 2016
        /// Description: 
        /// </summary>
        /// <param name="modelId"></param>
        /// <returns></returns>
        public IEnumerable<ModelImage> GetModelPhotos(U modelId)
        {
            List<ModelImage> objPhotos = null;

            string key = string.Format("BW_ModelPhotoGallery_MO_{0}", modelId);
            try
            {
                objPhotos = _cache.GetFromCache<List<ModelImage>>(key, new TimeSpan(1, 0, 0), () => (List<ModelImage>)_objModels.GetModelPhotos(modelId));
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "BikeModelsCacheRepository.GetModelPhotos");
                objErr.SendMail();
            }

            return objPhotos;
        }
    }
}
