using Bikewale.Entities.BikeData;
using Bikewale.Entities.BikeData.NewLaunched;
using Bikewale.Entities.CMS.Photos;
using Bikewale.Entities.GenericBikes;
using Bikewale.Entities.UserReviews;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Notifications;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
            string key = string.Format("BW_ModelDetail_{0}", modelId);
            try
            {
                objModelPage = _cache.GetFromCache<BikeModelPageEntity>(key, new TimeSpan(1, 0, 0), () => _objModels.GetModelPageDetails(modelId, versionId));
                if (objModelPage != null)
                {
                    objModelPage.Photos = GetModelPhotoGallery(modelId);
                    #region Add first image
                    if (objModelPage.ModelDetails != null && objModelPage.ModelDetails.MakeBase != null)
                    {
                        if (objModelPage.Photos == null)
                        {
                            objModelPage.Photos = new List<ModelImage>();
                        }
                        objModelPage.Photos.Insert(0,
                                new ModelImage()
                                {
                                    HostUrl = objModelPage.ModelDetails.HostUrl,
                                    OriginalImgPath = objModelPage.ModelDetails.OriginalImagePath,
                                    ImageCategory = "Model Image",
                                    MakeBase = new BikeMakeEntityBase()
                                    {
                                        MakeName = objModelPage.ModelDetails.MakeBase.MakeName,
                                    },
                                    ModelBase = new BikeModelEntityBase()
                                    {
                                        ModelName = objModelPage.ModelDetails.ModelName,
                                    },
                                    ImageName = objModelPage.ModelDetails.ModelName
                                });
                    }
                    #endregion
                }
                //objModelPage.colorPhotos = GetModelColorPhotos(modelId);
                //_objModels.GetBikeModelPhotoGallery(modelId);
                if (objModelPage.ModelVersionSpecsList != null && objModelPage.ModelVersionSpecs != null && objModelPage.ModelVersions.Count() > 1)
                {
                    // First page load where version id is Zero, fetch default version properties
                    versionId = versionId == 0 ? (int)objModelPage.ModelVersionSpecs.BikeVersionId : versionId;
                    var curVersionSpecs = objModelPage.ModelVersionSpecsList.FirstOrDefault(m => m.BikeVersionId == (uint)versionId);
                    if (curVersionSpecs != null)
                        objModelPage.ModelVersionSpecs = curVersionSpecs;
                    if (objModelPage.TransposeModelSpecs != null)
                    {
                        var transposeSpecs = objModelPage.TransposeModelSpecs.FirstOrDefault(m => m.BikeVersionId == versionId);
                        if (transposeSpecs != null)
                        {
                            objModelPage.objOverview = transposeSpecs.objOverview;
                            objModelPage.objSpecs = transposeSpecs.objSpecs;
                            objModelPage.objFeatures = transposeSpecs.objFeatures;
                        }
                    }
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
        /// Created by Sangram Nandkhile 30 Jan 2017
        /// Create List of modelimage,color wise models
        /// </summary>
        /// <param name="modelId"></param>
        /// <returns></returns>
        public IEnumerable<ImageBaseEntity> GetAllPhotos(BikeModelPageEntity objModelPage)
        {
            IEnumerable<ImageBaseEntity> allPhotos = null;
            string key = "BW_Model_AllPhotos_{0}" + objModelPage.ModelDetails.ModelId;
            try
            {
                allPhotos = _cache.GetFromCache<IEnumerable<ImageBaseEntity>>(key, new TimeSpan(1, 0, 0), () => CreateAllPhotoList(objModelPage));
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "BikeModelsCacheRepository.GetAllPhotos");
            }

            return allPhotos;
        }

        private IEnumerable<ImageBaseEntity> CreateAllPhotoList(BikeModelPageEntity objModelPage)
        {
            int modelId = 0;
            List<ImageBaseEntity> allPhotos = null;
            try
            {
                if (objModelPage != null)
                {
                    allPhotos = new List<ImageBaseEntity>();

                    if (objModelPage.ModelDetails != null && objModelPage.ModelDetails.MakeBase != null)
                    {
                        modelId = objModelPage.ModelDetails.ModelId;
                        allPhotos.Add(
                            new ImageBaseEntity()
                            {
                                HostUrl = objModelPage.ModelDetails.HostUrl,
                                OriginalImgPath = objModelPage.ModelDetails.OriginalImagePath,
                                ImageType = ImageBaseType.ModelImage,
                                ImageTitle = objModelPage.ModelDetails != null ? string.Format("{0} Model Image", objModelPage.ModelDetails.ModelName) : string.Empty
                            });
                    }
                    if (objModelPage.Photos != null)
                    {
                        allPhotos.AddRange(objModelPage.Photos.Select(x => new ImageBaseEntity() { HostUrl = x.HostUrl, OriginalImgPath = x.OriginalImgPath, ImageTitle = x.ImageCategory, ImageType = ImageBaseType.ModelGallaryImage }));
                    }
                    if (objModelPage.colorPhotos != null)
                    {
                        allPhotos.AddRange(objModelPage.colorPhotos.Where(x => !string.IsNullOrEmpty(x.Host)).Select(x => new ColorImageBaseEntity() { HostUrl = x.Host, OriginalImgPath = x.OriginalImagePath, ColorId = x.BikeModelColorId, ImageTitle = x.Name, ImageType = ImageBaseType.ModelColorImage, Colors = x.ColorCodes.Select(y => y.HexCode) }));
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, string.Format("BikeModelsCacheRepository.GetAllPhotos() : ModelId => {0}", modelId));
            }

            return allPhotos;
        }
        /// <summary>
        /// Created by: Sangram Nandkhile on 31st Jan 2017
        /// Summary: Combine all photo details
        /// </summary>
        /// <param name="modelId"></param>
        /// <returns></returns>
        public IEnumerable<ImageBaseEntity> CreateAllPhotoList(U modelId)
        {
            List<ImageBaseEntity> allPhotos = null;
            BikeModelPageEntity objModelPage = null;
            string key = string.Format("BW_ModelDetails_{0}", modelId);
            try
            {
                objModelPage = _cache.GetFromCache<BikeModelPageEntity>(key, new TimeSpan(1, 0, 0), () => _objModels.GetModelPageDetails(modelId));
                objModelPage.Photos = GetModelPhotoGallery(modelId);
                objModelPage.colorPhotos = GetModelColorPhotos(modelId);
                allPhotos = GetAllPhotos(objModelPage).ToList();
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "BikeModelsCacheRepository.GetAllPhotos");
            }
            return allPhotos;
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
        /// Created by Subodh Jain 17 jan 2017
        /// Desc Get User Review Similar Bike
        /// </summary>
        /// <param name="modelId"></param>
        /// <param name="topCount"></param>
        /// <returns></returns>
        public IEnumerable<BikeUserReviewRating> GetUserReviewSimilarBike(uint modelId, uint topCount)
        {
            IEnumerable<BikeUserReviewRating> objReviewUser = null;
            string key = string.Format("BW_UserReviewSimilarBike_ModelId_{0}_Topcount_{1}", modelId, topCount);
            try
            {
                objReviewUser = _cache.GetFromCache<IEnumerable<BikeUserReviewRating>>(key, new TimeSpan(1, 0, 0), () => _objModels.GetUserReviewSimilarBike(modelId, topCount));
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, string.Format(" BikeModelsCacheRepository.GetUserReviewSimilarBike_modelid_{0}_topcount_{1}", modelId, topCount));

            }

            return objReviewUser;

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

            string key = string.Format("BW_ModelPhotos_MO_{0}", modelId);
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
        /// <summary>
        /// Created By :- Subodh Jain 17 Jan 2017
        /// Summary :- Get details by version and city for review
        /// </summary>
        /// <param name="reviewId"></param>
        /// <param name="isAlreadyViewed"></param>
        public IEnumerable<ModelColorImage> GetModelColorPhotos(U modelId)
        {
            IEnumerable<ModelColorImage> objColorImages = null;
            string key = string.Format("BW_ModelPhotosColorWise_{0}", modelId);
            try
            {
                objColorImages = _cache.GetFromCache<IEnumerable<ModelColorImage>>(key, new TimeSpan(1, 0, 0), () => _modelRepository.GetModelColorPhotos(modelId));
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, string.Format("BikeModelsCacheRepository.Getmodelcolorphotos ==> ModelId: {0}", modelId));
            }
            return objColorImages;
        }
        /// <summary>
        /// Created By : Aditi Srivastava on 25 Jan 2017
        /// Summary    : Get body type of a bike model
        /// </summary>
        /// <param name="modelId"></param>
        /// <returns></returns>
        public EnumBikeBodyStyles GetBikeBodyType(uint modelId)
        {
            EnumBikeBodyStyles bodystyle = EnumBikeBodyStyles.AllBikes;
            string key = string.Format("BW_BikeBodyType_MO_{0}", modelId);
            try
            {
                bodystyle = _cache.GetFromCache<EnumBikeBodyStyles>(key, new TimeSpan(1, 0, 0), () => (EnumBikeBodyStyles)_modelRepository.GetBikeBodyType(modelId));
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, string.Format("BikeModelsCacheRepository.GetBikeBodyType_ModelId {0}", modelId));
            }
            return bodystyle;
        }
        /// <summary>
        /// Created By : Aditi Srivastava on 25 Jan 2017
        /// Summary    : Get list of top popular bikes by category
        /// </summary>
        /// <param name="bodyStyleId"></param>
        /// <param name="topCount"></param>
        /// <param name="cityId"></param>
        /// <returns></returns>
        public ICollection<MostPopularBikesBase> GetPopularBikesByBodyStyle(int modelId, int topCount, uint cityId)
        {
            ICollection<MostPopularBikesBase> popularBikesList = null;
            string key = string.Format("BW_PopularBikesListByBodyType_MO_{0}_city_{1}_topcount_{2}", modelId, cityId, topCount);
            try
            {
                popularBikesList = _cache.GetFromCache<Collection<MostPopularBikesBase>>(key, new TimeSpan(1, 0, 0), () => (Collection<MostPopularBikesBase>)_modelRepository.GetPopularBikesByBodyStyle(modelId, topCount, cityId));
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, string.Format("BikeModelsCacheRepository.GetPopularBikesByBodyStyle: ModelId: {0},CityId {1}", modelId, cityId));

            }
            return popularBikesList;
        }
        /// <summary>
        /// Created by  :   Sushil Kumar on 2nd Jan 2016
        /// Description :   Calls DAL via Cache layer for generic bike info
        /// Modified By :- subodh Jain 10 Feb 2017
        /// Summary :- BikeInfo Slug details
        /// </summary>
        /// <param name="modelId"></param>
        /// <returns></returns>
        public GenericBikeInfo GetBikeInfo(uint modelId)
        {
            string key = string.Format("BW_GenericBikeInfo_MO_{0}", modelId);
            GenericBikeInfo objSearchList = null;
            try
            {
                objSearchList = _cache.GetFromCache<GenericBikeInfo>(key, new TimeSpan(0, 30, 0), () => _modelRepository.GetBikeInfo(modelId));

            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, string.Format("BestBikesCacheRepository.GetBikeInfo ModelId:{0}", modelId));
            }
            return objSearchList;
        }
        /// <summary>
        /// Created by  :   Subodh jain 9 Feb 2017
        /// Description :   Calls DAL via Cache layer for generic bike info
        /// </summary>
        /// <param name="modelId"></param>
        /// <returns></returns>
        public GenericBikeInfo GetBikeInfo(uint modelId, uint cityId)
        {
            string key = string.Format("BW_GenericBikeInfo_MO_{0}_cityId_{1}", modelId, cityId);
            GenericBikeInfo objSearchList = null;
            try
            {
                objSearchList = _cache.GetFromCache<GenericBikeInfo>(key, new TimeSpan(0, 30, 0), () => _modelRepository.GetBikeInfo(modelId, cityId));

            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, string.Format("BestBikesCacheRepository.GetBikeInfo ModelId:{0} CityId:{1}", modelId, cityId));
            }
            return objSearchList;
        }
        /// <summary>
        /// Created By : Aditi Srivastava on 12 Jan 2017
        /// Description : To get bike rankings by category
        /// </summary>
        /// <param name="modelId"></param>
        /// <returns></returns>
        public BikeRankingEntity GetBikeRankingByCategory(uint modelId)
        {
            string key = string.Format("BW_BikeRankingByModel_MO_{0}", modelId);
            BikeRankingEntity bikeRankObj = null;
            try
            {
                bikeRankObj = _cache.GetFromCache<BikeRankingEntity>(key, new TimeSpan(0, 30, 0), () => _modelRepository.GetBikeRankingByCategory(modelId));
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, string.Format("BestBikesCacheRepository.GetBikeRankingByCategory: ModelId:{0}", modelId));

            }
            return bikeRankObj;
        }

        /// <summary>
        /// Created By : Aditi Srivastava on 17 Jan 2017
        /// Description : To get top 10 bikes of a given body style
        /// Modified by : Sajal Gupta on 02-02-2017
        /// Description : Passed cityid to get used bikes count.  
        /// </summary>
        /// <param name="modelId"></param>
        /// <returns></returns>
        public ICollection<BestBikeEntityBase> GetBestBikesByCategory(EnumBikeBodyStyles bodyStyle, uint? cityId = null)
        {
            string key = string.Format("BW_BestBikesByBodyStyle_{0}", bodyStyle);

            if (cityId != null)
                key = string.Format("{0}_{1}", key, cityId.Value);

            ICollection<BestBikeEntityBase> bestBikesList = null;
            try
            {
                bestBikesList = _cache.GetFromCache<ICollection<BestBikeEntityBase>>(key, new TimeSpan(0, 30, 0), () => _modelRepository.GetBestBikesByCategory(bodyStyle, cityId));
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, string.Format("BestBikesCacheRepository.GetBestBikesByCategory: BodyStyle:{0}", bodyStyle));
            }
            return bestBikesList;
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 10 Feb 2017
        /// Description :   returns bikes list from Cache/DAL
        /// </summary>
        /// <returns></returns>
        public IEnumerable<NewLaunchedBikeEntityBase> GetNewLaunchedBikesList()
        {
            string key = "BW_NewLaunchedBikes";
            IEnumerable<NewLaunchedBikeEntityBase> bikes = null;
            try
            {
                bikes = _cache.GetFromCache<IEnumerable<NewLaunchedBikeEntityBase>>(key, new TimeSpan(0, 30, 0), () => _modelRepository.GetNewLaunchedBikesList());
            }
            catch (Exception ex)
            {
                ErrorClass err = new ErrorClass(ex, "Bikewale.DAL.BikeData.GetNewLaunchedBikesList");
            }
            return bikes;
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 13 Feb 2017
        /// Description :   GetNewLaunchedBikesList by City
        /// </summary>
        /// <param name="cityId"></param>
        /// <returns></returns>
        public IEnumerable<NewLaunchedBikeEntityBase> GetNewLaunchedBikesList(uint cityId)
        {
            string key = String.Format("BW_NewLaunchedBikes_Cid_{0}", cityId);
            IEnumerable<NewLaunchedBikeEntityBase> bikes = null;
            try
            {
                bikes = _cache.GetFromCache<IEnumerable<NewLaunchedBikeEntityBase>>(key, new TimeSpan(0, 30, 0), () => _modelRepository.GetNewLaunchedBikesList(cityId));
            }
            catch (Exception ex)
            {
                ErrorClass err = new ErrorClass(ex, "Bikewale.DAL.BikeData.GetNewLaunchedBikesList");
            }
            return bikes;
        }
    }
}
