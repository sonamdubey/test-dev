using Bikewale.BAL.EditCMS;
using Bikewale.BAL.GrpcFiles;
using Bikewale.Cache.BikeData;
using Bikewale.Cache.CMS;
using Bikewale.Cache.Core;
using Bikewale.DAL.BikeData;
using Bikewale.DAL.UserReviews;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.CMS;
using Bikewale.Entities.CMS.Articles;
using Bikewale.Entities.CMS.Photos;
using Bikewale.Entities.PhotoGallery;
using Bikewale.Entities.UserReviews;
using Bikewale.Entities.Videos;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.CMS;
using Bikewale.Interfaces.EditCMS;
using Bikewale.Interfaces.Pager;
using Bikewale.Interfaces.UserReviews;
using Bikewale.Interfaces.Videos;
using Bikewale.Notifications;
using Bikewale.Utility;
using Grpc.CMS;
using log4net;
using Microsoft.Practices.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Bikewale.BAL.BikeData
{
    /// <summary>
    /// Created By : Ashish G. Kamble on 24 Apr 2014
    /// Summary : 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="U"></typeparam>
    public class BikeModels<T, U> : IBikeModels<T, U> where T : BikeModelEntity, new()
    {
        private readonly IBikeModelsRepository<T, U> modelRepository = null;
        private readonly IPager _objPager = null;
        private readonly IUserReviewsCache _userReviewCache = null;
        private readonly IArticles _articles = null;
        private readonly ICMSCacheContent _cacheArticles = null;
        private readonly IBikeModelsCacheRepository<U> _modelCacheRepository = null;
        private readonly IVideos _videos = null;

        static bool _useGrpc = Convert.ToBoolean(BWConfiguration.Instance.UseGrpc);
        static bool _logGrpcErrors = Convert.ToBoolean(BWConfiguration.Instance.LogGrpcErrors);
        static readonly ILog _logger = LogManager.GetLogger(typeof(BikeModels<T, U>));
        static uint _applicationid = Convert.ToUInt32(BWConfiguration.Instance.ApplicationId);


        public BikeModels()
        {
            using (IUnityContainer container = new UnityContainer())
            {
                container.RegisterType<IBikeModelsRepository<T, U>, BikeModelsRepository<T, U>>();
                container.RegisterType<IPager, BAL.Pager.Pager>();
                container.RegisterType<IArticles, Articles>();
                container.RegisterType<IUserReviewsCache, Bikewale.Cache.UserReviews.UserReviewsCacheRepository>();
                container.RegisterType<ICacheManager, MemcacheManager>();
                container.RegisterType<IUserReviewsRepository, UserReviewsRepository>();
                container.RegisterType<ICMSCacheContent, CMSCacheRepository>();
                container.RegisterType<IBikeMakesCacheRepository<int>, BikeMakesCacheRepository<BikeMakeEntity, int>>();
                container.RegisterType<IBikeModelsCacheRepository<U>, BikeModelsCacheRepository<T, U>>();
                container.RegisterType<IVideos, Bikewale.BAL.Videos.Videos>();

                modelRepository = container.Resolve<IBikeModelsRepository<T, U>>();
                _objPager = container.Resolve<IPager>();
                _articles = container.Resolve<IArticles>();
                _cacheArticles = container.Resolve<ICMSCacheContent>();
                _modelCacheRepository = container.Resolve<IBikeModelsCacheRepository<U>>();
                _videos = container.Resolve<IVideos>();
                _userReviewCache = container.Resolve<IUserReviewsCache>();
            }
        }

        public List<BikeModelEntityBase> GetModelsByType(EnumBikeType requestType, int makeId)
        {
            List<BikeModelEntityBase> objModelList = null;

            objModelList = modelRepository.GetModelsByType(requestType, makeId);
            return objModelList;
        }

        /// <summary>
        /// Modified By : Sadhana Upadhyay on 20 Aug 
        /// Summary : to retrieve version list for new as well as discontinued bikes
        /// </summary>
        /// <param name="modelId"></param>
        /// <param name="isNew"></param>
        /// <returns></returns>
        public List<BikeVersionsListEntity> GetVersionsList(U modelId, bool isNew)
        {
            List<BikeVersionsListEntity> objVersionList = null;

            objVersionList = modelRepository.GetVersionsList(modelId, isNew);

            return objVersionList;
        }

        public BikeDescriptionEntity GetModelSynopsis(U modelId)
        {
            BikeDescriptionEntity objDesc = null;

            objDesc = modelRepository.GetModelSynopsis(modelId);

            return objDesc;
        }

        public UpcomingBikeEntity GetUpcomingBikeDetails(U modelId)
        {
            UpcomingBikeEntity objUpcomingBike = null;

            objUpcomingBike = modelRepository.GetUpcomingBikeDetails(modelId);

            return objUpcomingBike;
        }

        public U Add(T t)
        {
            throw new NotImplementedException();
        }

        public bool Update(T t)
        {
            throw new NotImplementedException();
        }

        public bool Delete(U id)
        {
            throw new NotImplementedException();
        }

        public List<T> GetAll()
        {
            throw new NotImplementedException();
        }

        public T GetById(U id)
        {
            T t = modelRepository.GetById(id);

            return t;
        }

        /// <summary>
        /// Created By : Ashish G. Kamble on 12 May 2014
        /// Summary : 
        /// </summary>
        /// <param name="inputParams">Start Index and End Index are mandetory.</param>
        /// <param name="sortBy">Optional. To get all results set default.</param>
        /// <param name="recordCount">Record count</param>
        /// <returns></returns>
        public List<UpcomingBikeEntity> GetUpcomingBikesList(UpcomingBikesListInputEntity inputParams, EnumUpcomingBikesFilter sortBy, out int recordCount)
        {
            List<UpcomingBikeEntity> objUpcomingBikeList = null;

            objUpcomingBikeList = modelRepository.GetUpcomingBikesList(inputParams, sortBy, out recordCount);

            return objUpcomingBikeList;
        }

        /// <summary>
        /// Created By : Sadhana Upadhyay on 4 June 2014
        /// Summary : To get all recently launched bikes
        /// </summary>
        /// <param name="startIndex">Start Index</param>
        /// <param name="endIndex">End Index</param>
        /// <param name="recordCount">Record Count</param>
        /// <returns></returns>
        public NewLaunchedBikesBase GetNewLaunchedBikesList(int startIndex, int endIndex, int? makeid = null)
        {
            NewLaunchedBikesBase objNewLaunchedBikeList = null;

            objNewLaunchedBikeList = modelRepository.GetNewLaunchedBikesList(startIndex, endIndex);
            return objNewLaunchedBikeList;
        }
        /// <summary>
        /// Created by Subodh jain 22 sep 2016
        /// des: to deicide to fetch by makecity or only make
        /// </summary>
        /// <param name="topCount"></param>
        /// <param name="makeId"></param>
        /// <param name="cityId"></param>
        /// <returns></returns>
        public IEnumerable<MostPopularBikesBase> GetMostPopularBikesbyMakeCity(uint topCount, uint makeId, uint cityId)
        {
            IEnumerable<MostPopularBikesBase> objList = null;
            if (cityId > 0)
                objList = modelRepository.GetMostPopularBikesbyMakeCity(topCount, makeId, cityId);
            else
                objList = modelRepository.GetMostPopularBikesByMake((int)makeId);
            return objList;

        }


        public Hashtable GetMaskingNames()
        {
            throw new NotImplementedException();
        }

        public Hashtable GetOldMaskingNames()
        {
            throw new NotImplementedException();
        }


        public List<FeaturedBikeEntity> GetFeaturedBikes(uint topRecords)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Written By : Ashish G. Kamble on 7 Oct 2015
        /// Summary : Function to get the model page details
        /// Modified By: Aditi Srivastava on 26 Aug 2016
        /// Summary: Added a condition to avoid fetching the whole model gallery in case of desktop model page 
        /// Modified by : Sajal Gupta on 28-02-2017
        /// Description : Call function to get images.
        /// </summary>
        /// <param name="modelId"></param>
        /// <returns></returns>
        public BikeModelPageEntity GetModelPageDetails(U modelId)
        {
            BikeModelPageEntity objModelPage = null;

            try
            {
                objModelPage = _modelCacheRepository.GetModelPageDetails(modelId);

                if (objModelPage != null)
                {
                    CreateAllPhotoList(modelId, objModelPage);
                }

            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Exception : Bikewale.BAL.BikeData.GetModelPageDetails");
            }

            return objModelPage;
        }

        /// <summary>
        /// Created by : Sajal Gupta on 28-02-2017
        /// Description : Function to get data from cache and photo data from bal itself;
        /// </summary>
        /// <param name="modelId"></param>
        /// <param name="versionId"></param>
        /// <returns></returns>
        public BikeModelPageEntity GetModelPageDetails(U modelId, int versionId)
        {
            BikeModelPageEntity objModelPage = null;
            try
            {
                objModelPage = _modelCacheRepository.GetModelPageDetails(modelId, versionId);
                if (objModelPage != null)
                {
                    CreateAllPhotoList(modelId, objModelPage);
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, string.Format("BikeModelsCacheRepository.GetModelPageDetails() => modelid {0}, versionId: {1}", modelId, versionId));
            }

            return objModelPage;
        }



        /// <summary>
        /// Created By : Sangram Nandkhile on 01 Dec 2016
        /// Summary: New overload method to cache Model page with versions and respective lists
        /// </summary>
        /// <param name="modelId"></param>
        /// <returns></returns>
        public BikeModelPageEntity GetModelPageDetailsNew(U modelId)
        {
            BikeModelPageEntity objModelPage = null;
            try
            {

            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, String.Format("Exception : Bikewale.BAL.BikeData.GetModelPageDetailsNew => ModelId: {0}"));
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
            return (modelRepository.GetModelColor(modelId));
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
            return modelRepository.GetUserReviewSimilarBike(modelId, topCount);
        }



        /// <summary>
        /// Created by: Sangram Nandkhile on 10 Feb 2017
        /// Desc: To Fetch model main image and other model images
        /// </summary>
        /// <param name="modelId"></param>
        /// <returns></returns>
        private IEnumerable<ModelImage> GetModelPhotoGalleryWithMainImage(BikeModelEntity objModel, U modelId)
        {
            //ModelHostImagePath modelInfo = modelRepository.GetModelPhotoInfo(modelId);
            List<ModelImage> modelImages = null;
            try
            {
                if (objModel != null && !String.IsNullOrEmpty(objModel.HostUrl) && !String.IsNullOrEmpty(objModel.OriginalImagePath))
                {
                    modelImages = new List<ModelImage>();
                    var imageDesc = String.Format("{0} Model Image", objModel.ModelName);
                    modelImages.Add(new ModelImage()
                    {
                        HostUrl = objModel.HostUrl,
                        OriginalImgPath = objModel.OriginalImagePath,
                        ImageCategory = "Model Image",
                        ImageTitle = imageDesc,
                        ImageDescription = imageDesc,
                        AltImageName = imageDesc
                    });

                    var galleryImages = GetBikeModelPhotoGallery(modelId);
                    if (galleryImages != null && galleryImages.Count() > 0)
                        modelImages.AddRange(galleryImages);
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Exception : Bikewale.BAL.BikeData.GetModelPhotoGalleryWithMainImage");
            }
            return modelImages;
        }



        /// <summary>
        /// Created by: Sangram Nandkhile on 10 Feb 2017
        /// Desc: To Fetch model main image and other model images
        /// </summary>
        /// <param name="modelId"></param>
        /// <returns></returns>
        public IEnumerable<ModelImage> GetModelPhotoGalleryWithMainImage(U modelId)
        {
            //ModelHostImagePath modelInfo = modelRepository.GetModelPhotoInfo(modelId);
            List<ModelImage> modelImages = null;
            try
            {

                var objModel = _modelCacheRepository.GetModelPageDetails(modelId);

                if (objModel != null && objModel.ModelDetails != null && !String.IsNullOrEmpty(objModel.ModelDetails.HostUrl) && !String.IsNullOrEmpty(objModel.ModelDetails.OriginalImagePath))
                {
                    modelImages = new List<ModelImage>();
                    var imageDesc = String.Format("{0} Model Image", objModel.ModelDetails.ModelName);
                    modelImages.Add(new ModelImage()
                    {
                        HostUrl = objModel.ModelDetails.HostUrl,
                        OriginalImgPath = objModel.ModelDetails.OriginalImagePath,
                        ImageCategory = "Model Image",
                        ImageTitle = imageDesc,
                        ImageDescription = imageDesc,
                        AltImageName = imageDesc
                    });
                }
                var galleryImages = GetBikeModelPhotoGallery(modelId);
                if (galleryImages != null && galleryImages.Count() > 0)
                    modelImages.AddRange(galleryImages);
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Exception : Bikewale.BAL.BikeData.GetModelPhotoGalleryWithMainImage");
            }
            return modelImages;
        }

        public IEnumerable<ModelImage> GetBikeModelPhotoGallery(U modelId)
        {
            try
            {

                string contentTypeList = CommonApiOpn.GetContentTypesString(new List<EnumCMSContentType>() { EnumCMSContentType.PhotoGalleries, EnumCMSContentType.RoadTest, EnumCMSContentType.ComparisonTests });

                var _objGrpcmodelPhotoList = GrpcMethods.GetModelPhotosList(_applicationid, Convert.ToInt32(modelId), contentTypeList);

                if (_objGrpcmodelPhotoList != null && _objGrpcmodelPhotoList.LstGrpcModelImage.Count > 0)
                {
                    return GrpcToBikeWaleConvert.ConvertFromGrpcToBikeWale(_objGrpcmodelPhotoList);
                }
            }
            catch (Exception err)
            {
                _logger.Error(err.Message, err);
            }
            return null;
        }

        /// <summary>
        /// Written by : Ashish G. Kamble on 15 dec 2015
        /// Summary : Function to get the upcoming bikes list.
        /// </summary>
        /// <param name="sortBy">Sorting order for the upcoming bikes.</param>
        /// <param name="pageSize">No of records to be shown on the page.</param>
        /// <param name="makeId">Optional.</param>
        /// <param name="modelId">Optional.</param>
        /// <param name="curPageNo">Optional. Current page number.</param>
        /// <returns></returns>
        public List<UpcomingBikeEntity> GetUpcomingBikesList(EnumUpcomingBikesFilter sortBy, int pageSize, int? makeId = null, int? modelId = null, int? curPageNo = null)
        {
            IEnumerable<UpcomingBikeEntity> objUpcoming = null;
            try
            {

                objUpcoming = _modelCacheRepository.GetUpcomingBikesList(sortBy, pageSize, makeId, modelId, curPageNo);
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Exception : Bikewale.BAL.BikeData.GetUpcomingBikesList");
            }

            if (objUpcoming != null)
                return objUpcoming.ToList();
            else
                return null;
        }

        public BikeModelContent GetRecentModelArticles(U modelId)
        {
            BikeModelContent objModelArticles = new BikeModelContent();
            IEnumerable<ReviewEntity> objReview = null;
            IEnumerable<ArticleSummary> objRecentNews = null;
            IEnumerable<ArticleSummary> objExpertReview = null;
            IEnumerable<BikeVideoEntity> objVideos = null;


            try
            {


                var reviewTask = Task.Factory.StartNew(() => objReview = _userReviewCache.GetBikeReviewsList(1, 2, Convert.ToUInt32(modelId), 0, FilterBy.MostRecent).ReviewList);
                var newsTask = Task.Factory.StartNew(() => objRecentNews = _cacheArticles.GetMostRecentArticlesByIdList(Convert.ToString((int)EnumCMSContentType.News), 2, 0, Convert.ToUInt32(modelId)));
                var expReviewTask = Task.Factory.StartNew(() => objExpertReview = _cacheArticles.GetMostRecentArticlesByIdList(Convert.ToString((int)EnumCMSContentType.RoadTest), 2, 0, Convert.ToUInt32(modelId)));
                var videosTask = Task.Factory.StartNew(() => objVideos = GetVideosByModelIdViaGrpc(Convert.ToInt32(modelId)));

                Task.WaitAll(reviewTask, newsTask, expReviewTask, videosTask); //calling tasks asynchronously, this will wait untill all tasks are completed

                objModelArticles.ReviewDetails = objReview;
                objModelArticles.News = objRecentNews;
                objModelArticles.ExpertReviews = objExpertReview;
                objModelArticles.Videos = objVideos;

            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
            }

            return objModelArticles;
        }

        /// <summary>
        /// Author: Prasad Gawde
        /// </summary>
        /// <returns></returns>
        private IEnumerable<BikeVideoEntity> GetVideosByModelIdViaGrpc(int modelId)
        {
            IEnumerable<BikeVideoEntity> videoDTOList = null;
            try
            {

                var _objVideoList = GrpcMethods.GetVideosByModelId(modelId, 1, UInt32.MaxValue);

                if (_objVideoList != null && _objVideoList.LstGrpcVideos.Count > 0)
                {
                    videoDTOList = GrpcToBikeWaleConvert.ConvertFromGrpcToBikeWale(_objVideoList.LstGrpcVideos);
                }
                else
                    videoDTOList = null;
            }
            catch (Exception err)
            {
                _logger.Error(err.Message, err);

            }

            return videoDTOList;
        }

        /// <summary>
        /// Created by : Sajal Gupta on 24-02-2017
        /// Description : Get model photo gallery data
        /// </summary>
        /// <param name="modelId"></param>
        /// <returns></returns>
        public ModelPhotoGalleryEntity GetPhotoGalleryData(U modelId)
        {
            ModelPhotoGalleryEntity objModelPhotoGalleryData = null;
            try
            {
                var modelPage = _modelCacheRepository.GetModelPageDetails(modelId);

                if (modelPage != null)
                {
                    objModelPhotoGalleryData = new ModelPhotoGalleryEntity();
                    objModelPhotoGalleryData.ObjModelEntity = modelPage.ModelDetails;

                    if (objModelPhotoGalleryData.ObjModelEntity != null && objModelPhotoGalleryData.ObjModelEntity.MakeBase != null)
                        objModelPhotoGalleryData.VideosList = _videos.GetVideosByMakeModel(1, 50, (uint)objModelPhotoGalleryData.ObjModelEntity.MakeBase.MakeId, Convert.ToUInt32(modelId));

                    CreateAllPhotoList(modelId, modelPage);
                    objModelPhotoGalleryData.ImageList = modelPage.AllPhotos;
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, string.Format("Bikewale.BAL.BikeData.BikeModels.GetPhotoGalleryData  modelId{0}", modelId));
            }
            return objModelPhotoGalleryData;
        }


        /// <summary>
        /// Created by: Sangram Nandkhile On 9 Feb 2017
        /// Summary: To sum up Model Photos
        /// Modified by : Aditi Srivastava on 3 Mar 2017
        /// Summary     : Added model color photos after first image
        /// </summary>
        /// <param name="objModelPage"></param>
        /// <returns></returns>
        public IEnumerable<ColorImageBaseEntity> CreateAllPhotoList(U modelId)
        {
            List<ColorImageBaseEntity> allPhotos = null;
            try
            {
                IEnumerable<ModelImage> modelPhotos = GetModelPhotoGalleryWithMainImage(modelId);

                if (modelPhotos != null && modelPhotos.Count() > 0)
                {
                    allPhotos = new List<ColorImageBaseEntity>();
                    var modelImage = modelPhotos.FirstOrDefault();

                    var galleryImages = modelPhotos.Count() > 1 ? modelPhotos.Skip(1).Select(
                            m => new ColorImageBaseEntity()
                            {
                                HostUrl = m.HostUrl,
                                OriginalImgPath = m.OriginalImgPath,
                                ImageCategory = m.ImageCategory,
                                ImageTitle = m.ImageTitle,
                                ImageType = ImageBaseType.ModelGallaryImage
                            }) : null;


                    var colorPics = GetModelColorPhotos(modelId);

                    IEnumerable<ModelColorImage> colorPhotos = colorPics != null ? colorPics.Where(m => !String.IsNullOrEmpty(m.OriginalImagePath)) : null;

                    var colorImages = (colorPhotos != null && colorPhotos.Count() > 0) ? colorPhotos.Select(x => new ColorImageBaseEntity()
                        {
                            HostUrl = x.Host,
                            OriginalImgPath = x.OriginalImagePath,
                            ColorId = x.BikeModelColorId,
                            ImageTitle = x.Name,
                            ImageType = ImageBaseType.ModelColorImage,
                            ImageCategory = x.ImageCategory,
                            Colors = x.ColorCodes.Select(y => y.HexCode)
                        }) : null;

                    //Add Model Image
                    allPhotos.Add(new ColorImageBaseEntity()
                    {
                        HostUrl = modelImage.HostUrl,
                        OriginalImgPath = modelImage.OriginalImgPath,
                        ImageTitle = modelImage.ImageCategory,
                        ImageType = ImageBaseType.ModelGallaryImage,
                        ImageCategory = modelImage.ImageCategory
                    });

                    //Add Color Photos
                    if (colorImages != null && colorImages.Count() > 0)
                    {
                        allPhotos.AddRange(colorImages);
                    }

                    //Add Model Gallery Photos
                    if (galleryImages != null)
                    {
                        allPhotos.AddRange(galleryImages);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, string.Format("Bikewale.BAL.BikeData.BikeModels.CreateAllPhotoList() : ModelId => {0}", modelId));
            }
            return allPhotos;
        }

        /// <summary>
        /// Created by: Sangram Nandkhile On 9 Feb 2017
        /// Summary: To sum up Model Photos
        /// Modified by : Aditi Srivastava on 3 Mar 2017
        /// Summary     : Added model color photos after first image
        /// </summary>
        /// <param name="objModelPage"></param>
        /// <returns></returns>
        private void CreateAllPhotoList(U modelId, BikeModelPageEntity objModelPage)
        {
            List<ColorImageBaseEntity> allPhotos = null;
            try
            {

                if (objModelPage != null && objModelPage.ModelDetails != null && !String.IsNullOrEmpty(objModelPage.ModelDetails.HostUrl) && !String.IsNullOrEmpty(objModelPage.ModelDetails.OriginalImagePath))
                {
                    allPhotos = new List<ColorImageBaseEntity>();
                    var galleryImages = GetBikeModelPhotoGallery(modelId);

                    var imageDesc = String.Format("{0} Model Image", objModelPage.ModelDetails.ModelName);
                    //Add Model Image
                    allPhotos.Add(new ColorImageBaseEntity()
                    {
                        HostUrl = objModelPage.ModelDetails.HostUrl,
                        OriginalImgPath = objModelPage.ModelDetails.OriginalImagePath,
                        ImageCategory = "Model Image",
                        ImageTitle = imageDesc,
                        ImageType = ImageBaseType.ModelImage
                    });

                    //Add Color Photos
                    IEnumerable<ModelColorImage> colorPhotos = objModelPage.colorPhotos != null ? objModelPage.colorPhotos.Where(m => !String.IsNullOrEmpty(m.OriginalImagePath)) : null;
                    var colorImages = (colorPhotos != null && colorPhotos.Count() > 0) ? colorPhotos.Select(x => new ColorImageBaseEntity()
                    {
                        HostUrl = x.Host,
                        OriginalImgPath = x.OriginalImagePath,
                        ColorId = x.BikeModelColorId,
                        ImageTitle = x.Name,
                        ImageType = ImageBaseType.ModelColorImage,
                        ImageCategory = x.ImageCategory,
                        Colors = x.ColorCodes.Select(y => y.HexCode)
                    }) : null;
                    if (colorImages != null && colorImages.Count() > 0)
                    {
                        allPhotos.AddRange(colorImages);
                    }

                    //Add Model Gallery Photos
                    if (galleryImages != null && galleryImages.Count() > 0)
                    {
                        objModelPage.Photos = galleryImages;
                        var galleryBikeImages = galleryImages.Select(
                                m => new ColorImageBaseEntity()
                                {
                                    HostUrl = m.HostUrl,
                                    OriginalImgPath = m.OriginalImgPath,
                                    ImageCategory = m.ImageCategory,
                                    ImageTitle = m.ImageTitle,
                                    ImageType = ImageBaseType.ModelGallaryImage
                                });
                        if (galleryBikeImages != null)
                        {
                            allPhotos.AddRange(galleryBikeImages);
                        }
                    }

                    objModelPage.AllPhotos = allPhotos;
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, string.Format("Bikewale.BAL.BikeData.BikeModels.CreateAllPhotoList() : ModelId => {0}", modelId));
            }
        }

        /// <summary>
        /// Created by : Sajal Gupta on 24-02-2017
        /// Description : Get model color photo gallery data from cache.
        /// </summary>
        public IEnumerable<ModelColorImage> GetModelColorPhotos(U modelId)
        {
            IEnumerable<ModelColorImage> objColorImages = null;
            try
            {
                objColorImages = _modelCacheRepository.GetModelColorPhotos(modelId);
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, string.Format("Bikewale.BAL.BikeData.BikeModels.Getmodelcolorphotos ==> ModelId: {0}", modelId));
            }
            return objColorImages;
        }


        /// <summary>
        /// Created By : Aditi Srivastava on 9 Mar 2017
        /// Summary    : Return list of popular scooters
        /// </summary>
        public IEnumerable<MostPopularBikesBase> GetMostPopularScooters(uint topCount, uint? cityId)
        {
            IEnumerable<MostPopularBikesBase> popularScooters = null;
            try
            {
                popularScooters = _modelCacheRepository.GetMostPopularScooters(topCount, cityId);
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, string.Format("BikeModels.GetMostPopularScooters() => topCount {0}, cityId: {1}", topCount, cityId));
            }
            return popularScooters;
        }

        /// <summary>
        /// Created By :- Subodh Jain 10 March 2017
        /// Summary :- Populate Compare ScootersList
        /// </summary>
        public IEnumerable<MostPopularBikesBase> GetMostPopularScooters(uint makeId)
        {
            return _modelCacheRepository.GetMostPopularScooters(makeId);
        }

        /// <summary>
        /// Implemented by  :   Sumit Kate on 24 Mar 2017
        /// Description     :   Returns GetMostPopularBikes based on following params
        /// </summary>
        /// <param name="requestType">request type</param>
        /// <param name="topCount">top count</param>
        /// <param name="makeId">make id</param>
        /// <param name="cityId">cityid</param>
        /// <returns></returns>
        public IEnumerable<MostPopularBikesBase> GetMostPopularBikes(EnumBikeType requestType, uint topCount, uint makeId, uint cityId)
        {
            IEnumerable<MostPopularBikesBase> bikes = null;
            try
            {
                switch (requestType)
                {
                    case EnumBikeType.Scooters:
                        if (makeId > 0 & cityId > 0)
                            bikes = _modelCacheRepository.GetMostPopularScooters(topCount, makeId, cityId);
                        else if (makeId > 0)
                            bikes = _modelCacheRepository.GetMostPopularScooters(topCount, makeId);
                        else
                            bikes = _modelCacheRepository.GetMostPopularScooters(topCount, cityId);
                        break;
                    default:
                        bikes = _modelCacheRepository.GetMostPopularBikesbyMakeCity(topCount, makeId, cityId);
                        break;
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, String.Format("GetMostPopularBikes({0},{1},{2},{3})", requestType, topCount, makeId, cityId));
            }
            return bikes;
        }
    }   // Class
}   // namespace