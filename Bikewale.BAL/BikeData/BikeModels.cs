﻿using Bikewale.BAL.Customer;
using Bikewale.BAL.EditCMS;
using Bikewale.BAL.GrpcFiles;
using Bikewale.BAL.UserReviews.Search;
using Bikewale.Cache.BikeData;
using Bikewale.Cache.CMS;
using Bikewale.Cache.Core;
using Bikewale.DAL.BikeData;
using Bikewale.DAL.Customer;
using Bikewale.DAL.UserReviews;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.CMS;
using Bikewale.Entities.CMS.Articles;
using Bikewale.Entities.CMS.Photos;
using Bikewale.Entities.Customer;
using Bikewale.Entities.GenericBikes;
using Bikewale.Entities.PhotoGallery;
using Bikewale.Entities.UserReviews;
using Bikewale.Entities.Videos;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.CMS;
using Bikewale.Interfaces.Customer;
using Bikewale.Interfaces.EditCMS;
using Bikewale.Interfaces.Pager;
using Bikewale.Interfaces.UserReviews;
using Bikewale.Interfaces.UserReviews.Search;
using Bikewale.Interfaces.Videos;
using Bikewale.Models;
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
namespace Bikewale.BAL.BikeData
{
    /// <summary>
    /// Created By : Ashish G. Kamble on 24 Apr 2014
    /// Summary :     
    /// Modified by : Sanskar Gupta on 22 Jan 2018
    /// Description : Added boolean 'isCityLogicPresent' in 'GetAdPromotedBike' function to separate Newly Launched logic of HomePage and Editorial Pages
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
        private readonly IBikeMaskingCacheRepository<BikeModelEntity, int> _modelMaskingCache = null;
        private readonly IUserReviewsSearch _userReviewsSearch = null;
        private readonly IVideos _videos = null;
        private readonly IUserReviews _userReviews = null;
        private readonly ILog _logger = LogManager.GetLogger(typeof(BikeModels<T, U>));
        private readonly uint _applicationid = Convert.ToUInt32(BWConfiguration.Instance.ApplicationId);
        private static readonly IEnumerable<EnumBikeBodyStyles> _bodyStyles = new List<EnumBikeBodyStyles> { EnumBikeBodyStyles.Scooter, EnumBikeBodyStyles.Street, EnumBikeBodyStyles.Cruiser, EnumBikeBodyStyles.Sports};
        private string _newsContentType;
        /// <summary>
        /// Modified by :   Sumit Kate on 26 Apr 2017
        /// Description :   Register the User Reviews BAL and resolve it
        /// </summary>
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
                container.RegisterType<IUserReviewsSearch, UserReviewsSearch>();
                container.RegisterType<IBikeMakesCacheRepository, BikeMakesCacheRepository>();
                container.RegisterType<IBikeMaskingCacheRepository<BikeModelEntity, int>, BikeModelMaskingCache<BikeModelEntity, int>>();
                container.RegisterType<IBikeModelsCacheRepository<U>, BikeModelsCacheRepository<T, U>>();
                container.RegisterType<IVideos, Bikewale.BAL.Videos.Videos>();
                container.RegisterType<ICustomer<CustomerEntity, UInt32>, Customer<CustomerEntity, UInt32>>();
                container.RegisterType<ICustomerRepository<CustomerEntity, UInt32>, CustomerRepository<CustomerEntity, UInt32>>();
                container.RegisterType<IUserReviews, Bikewale.BAL.UserReviews.UserReviews>();

                modelRepository = container.Resolve<IBikeModelsRepository<T, U>>();
                _objPager = container.Resolve<IPager>();
                _articles = container.Resolve<IArticles>();
                _cacheArticles = container.Resolve<ICMSCacheContent>();
                _modelCacheRepository = container.Resolve<IBikeModelsCacheRepository<U>>();
                _videos = container.Resolve<IVideos>();
                _userReviewCache = container.Resolve<IUserReviewsCache>();
                _userReviews = container.Resolve<IUserReviews>();
                _userReviewsSearch = container.Resolve<IUserReviewsSearch>();
                _modelMaskingCache = container.Resolve<IBikeMaskingCacheRepository<BikeModelEntity, int>>();
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

        public IEnumerable<MostPopularBikesBase> GetAdPromotedBike(BikeFilters ObjData, bool isCityLogicPresent)
        {
            IEnumerable<MostPopularBikesBase> objList = null;
            if (isCityLogicPresent)
            {
                if (ObjData.CityId == 0)
                    objList = _modelCacheRepository.GetAdPromotedBikeWithOutCity(ObjData);
                else
                    objList = _modelCacheRepository.GetAdPromotedBike(ObjData);
            }

            else
                objList = _modelCacheRepository.GetAdPromotedBikeWithOutCity(ObjData);

            objList = objList.Where(x => x.StartDate < DateTime.Now && x.EndDate > DateTime.Now);

            return objList;

        }
        public IEnumerable<MostPopularBikesBase> GetAdPromoteBikeFilters(IEnumerable<MostPopularBikesBase> promotedBikes, IEnumerable<MostPopularBikesBase> MostPopularBikes)
        {

            IEnumerable<MostPopularBikesBase> results = promotedBikes.Except(MostPopularBikes.Take(5), new MostPopularBikesBaseComparer());

            if (results.Any())
            {
                var bikes = MostPopularBikes.ToList();
                try
                {
                    var itemToRemove = bikes.SingleOrDefault(r => r.objModel.ModelId == results.ElementAt(0).objModel.ModelId);
                    bikes.Remove(itemToRemove);
                    bikes.Insert(0, results.ElementAt(0));
                    if (results.Count() >= 2)
                    {
                        itemToRemove = bikes.SingleOrDefault(r => r.objModel.ModelId == results.ElementAt(1).objModel.ModelId);
                        bikes.Remove(itemToRemove);
                        bikes.Insert(1, results.ElementAt(1));
                    }
                }
                catch (Exception ex) {
                    ErrorClass.LogError(ex, "Exception : Bikewale.BAL.BikeData.GetAdPromoteBikeFilters");
                }
                MostPopularBikes = bikes;
               
            }
            return MostPopularBikes;
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
                ErrorClass.LogError(ex, "Exception : Bikewale.BAL.BikeData.GetModelPageDetails");
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
                ErrorClass.LogError(ex, string.Format("BikeModelsCacheRepository.GetModelPageDetails() => modelid {0}, versionId: {1}", modelId, versionId));
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
                if (galleryImages != null && galleryImages.Any())
                    modelImages.AddRange(galleryImages);
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Exception : Bikewale.BAL.BikeData.GetModelPhotoGalleryWithMainImage");
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
        /// Created By  : Vivek Singh Tomar on 12th Jan 2018
        /// Descriptio  : Get models with list of images
        /// </summary>
        /// <param name="modelId"></param>
        /// <returns></returns>
        public IEnumerable<ModelImages> GetBikeModelsPhotoGallery(string modelIds, int requiredImageCount)
        {
            IList<ModelImages> images = new List<ModelImages>();
            try
            {

                string contentTypeList = CommonApiOpn.GetContentTypesString(new List<EnumCMSContentType>() { EnumCMSContentType.PhotoGalleries, EnumCMSContentType.RoadTest });

                var _objGrpcmodelsPhotoList = GrpcMethods.GetModelsImages(modelIds, contentTypeList, requiredImageCount);

                if (_objGrpcmodelsPhotoList != null && _objGrpcmodelsPhotoList.LstGrpcModelImaegs.Count > 0)
                {
                    images = GrpcToBikeWaleConvert.ConvertFromGrpcToBikeWale(_objGrpcmodelsPhotoList).ToList();
                }
                AppendModelImages(modelIds, requiredImageCount, ref images);
            }
            catch (Exception err)
            {
                _logger.Error(err.Message, err);
            }
            return images;
        }
        /// <summary>
        /// Created by : Ashutosh Sharma on 11th Jan 2018
        /// Description : Method to get photos of bike models.
        /// </summary>
        /// <param name="modelIds">CSV modelIds for which Photos are to be fetched.</param>
        /// <param name="categoryIds">CSV categoryIds which Photos are to be fetched.</param>
        /// <param name="requiredImageCount">Count of Photos to be fetched for every model.</param>
        /// <returns></returns>
        public IEnumerable<ModelImages> GetBikeModelsPhotos(string modelIds, string categoryIds, int requiredImageCount)
        {
            IList<ModelImages> modelsImages = null;
            try
            {
                modelsImages = new List<ModelImages>();
                var objImages = GrpcMethods.GetModelsImages(modelIds, categoryIds, requiredImageCount);
                if (objImages != null && objImages.LstGrpcModelImaegs != null && objImages.LstGrpcModelImaegs.Count > 0)
                {
                    modelsImages = GrpcToBikeWaleConvert.ConvertFromGrpcToBikeWale(objImages).ToList();
                }
                AppendModelImages(modelIds, requiredImageCount, ref modelsImages);

            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, String.Format("BAL.Images.ImageBL.GetBikeModelsPhotos{0}_categoryIds_{1}_requiredImageCount_{2}", modelIds, categoryIds, requiredImageCount));
            }
            return modelsImages;
        }


        /// <summary>
        /// Gets the bike models photos.
        /// </summary>
        /// <param name="modelIds">The model ids.</param>
        /// <param name="categoryIds">The category ids.</param>
        /// <param name="requiredImageCount">The required image count.</param>
        /// <param name="pager">The pager.</param>
        /// <returns></returns>
        public ModelImageWrapper GetBikeModelsPhotos(string modelIds, string categoryIds, int requiredImageCount, ImagePager pager)
        {

            ModelImageWrapper imageWrapper = null;
            try
            {
                IEnumerable<ModelImages> modelsImages = null;
                modelsImages = GetBikeModelsPhotos(modelIds, categoryIds, requiredImageCount);

                if (modelsImages != null && modelsImages.Any())
                {
                    imageWrapper = new ModelImageWrapper();
                    imageWrapper.Models = modelsImages;
                    imageWrapper = SetNextPrevUrl(imageWrapper, pager);
                    if (imageWrapper.Models != null)
                        imageWrapper.RecordCount = imageWrapper.Models.Count();
                }
                return imageWrapper;
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, String.Format("BAL.Images.ImageBL.GetBikeModelsPhotos_modelIds_{0}_categoryIds_{1}_requiredImageCount_{2}", modelIds, categoryIds, requiredImageCount));
            }
            return null;
        }
        /// <summary>
        /// Sets the next previous URL.
        /// </summary>
        /// <param name="imageWrapper">The image wrapper.</param>
        /// <param name="pager">The pager.</param>
        /// <returns></returns>
        private ModelImageWrapper SetNextPrevUrl(ModelImageWrapper imageWrapper, ImagePager pager)
        {
            string controllerurl = "/api/images/pages/";

            pager.PageNo = (pager.PageNo == 0) ? 1 : pager.PageNo;
            if (pager.PageNo == pager.TotalPages)
                imageWrapper.NextPageUrl = string.Empty;
            else
            {
                //string apiUrlStrforNext = GetApiUrl(objFilters, 1);
                imageWrapper.NextPageUrl = string.Format("{0}{1}?pagesize={2}", controllerurl, pager.PageNo + 1, pager.PageSize);
            }

            if (pager.PageNo == 1 || pager.PageNo == 0)
                imageWrapper.PrevPageUrl = string.Empty;
            else
            {

                imageWrapper.PrevPageUrl = string.Format("{0}{1}?pagesize={2}", controllerurl, pager.PageNo - 1, pager.PageSize);
            }
            return imageWrapper;

        }
        /// <summary>
        /// Created by  :   Sumit Kate on 15 Jan 2018
        /// Description :   Appends model image and model color photos to image lists
        /// </summary>
        /// <param name="modelIds"></param>
        /// <param name="requiredImageCount"></param>
        /// <param name="modelsImages"></param>
        private void AppendModelImages(string modelIds, int requiredImageCount, ref IList<ModelImages> modelsImages)
        {
            if (!String.IsNullOrEmpty(modelIds) && requiredImageCount > 0 && modelsImages != null)
            {
                try
                {
                    ICollection<BikeModelColorImageEntity> colorImages = null;
                    var modelIdsArray = Array.ConvertAll(modelIds.Split(','), int.Parse);

                    var missingModelIds = modelIdsArray.Except(modelsImages.Select(m => m.ModelId));
                    if (missingModelIds != null && missingModelIds.Count() > 0)
                    {
                        colorImages = _modelCacheRepository.GetModelImages(modelIds);
                        var images = colorImages.GroupBy(m => m.Model.ModelId);
                        foreach (var img in modelsImages)
                        {
                            var cmsImages = img.ModelImage.ToList();
                            var image = images.Where(m => m.Key == img.ModelId).FirstOrDefault();
                            if (image != null && image.Any())
                            {
                                img.RecordCount += image.Count();
                                cmsImages.AddRange(ConvertToModelImages(image));
                                img.ModelImage = cmsImages.Take(requiredImageCount);
                            }
                        }

                        var missingModelImages = colorImages.Where(m => missingModelIds.Contains(m.Model.ModelId));
                        var missingImages = missingModelImages.GroupBy(m => m.Model.ModelId);
                        foreach (var image in missingImages)
                        {
                            var firstImg = image.First();
                            var img = new ModelImages()
                            {
                                ModelId = firstImg.Model.ModelId,
                                ModelBase = firstImg.Model,
                                MakeBase = new BikeMakeEntityBase() { MakeId = firstImg.Make.MakeId, MakeName = firstImg.Make.MakeName, MaskingName = firstImg.Make.MakeMaskingName },
                                RecordCount = image.Count(),
                                BikeName = String.Format("{0} {1}", firstImg.Make.MakeName, firstImg.Model.ModelName),
                                ModelImage = ConvertToModelImages(image).Take(requiredImageCount)
                            };
                            modelsImages.Add(img);
                        }
                    }
                    modelsImages = modelsImages.OrderBy(m => Array.IndexOf(modelIdsArray, m.ModelId)).ToList();
                }
                catch (Exception ex)
                {
                    ErrorClass.LogError(ex, "Error in AppendModelImages");
                }
            }
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 15 Jan 2018
        /// Description :   Convert To ModelImages
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        private IEnumerable<ModelImage> ConvertToModelImages(IGrouping<int, BikeModelColorImageEntity> image)
        {
            List<ModelImage> images = new List<ModelImage>();
            try
            {

                if (image != null && image.Count() > 0)
                {
                    images = new List<ModelImage>();
                    foreach (var img in image)
                    {
                        images.Add(new ModelImage()
                        {
                            HostUrl = img.HostUrl,
                            OriginalImgPath = img.OriginalImagePath,
                            MakeBase = new BikeMakeEntityBase() { MakeId = img.Make.MakeId, MakeName = img.Make.MakeName, MaskingName = img.Make.MakeMaskingName },
                            ModelBase = img.Model
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "ConvertToModelImages");
            }
            return images;
        }
        //// The delegate must have the same signature as the method
        //// it will call asynchronously.
        //public delegate IEnumerable<ModelImage> AsyncMethodCaller(U modelId);

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
        public IEnumerable<UpcomingBikeEntity> GetUpcomingBikesList(EnumUpcomingBikesFilter sortBy, int pageSize, int? makeId = null, int? modelId = null, int? curPageNo = null)
        {
            IEnumerable<UpcomingBikeEntity> objUpcoming = null;
            try
            {

                objUpcoming = _modelCacheRepository.GetUpcomingBikesList(sortBy, pageSize, makeId, modelId, curPageNo);
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass.LogError(ex, "Exception : Bikewale.BAL.BikeData.GetUpcomingBikesList");
            }

            return objUpcoming;
        }

        /// <summary>
        /// Modified by :   Sumit Kate on 26 Apr 2017
        /// Description :   Use BAL to get the old User reviews for App
        /// Modified by : Pratibha Verma on 25the January
        /// Description : Added AutoExpo2018 in news category
        /// </summary>
        /// <param name="modelId"></param>
        /// <returns></returns>
        public BikeModelContent GetRecentModelArticles(U modelId)
        {
            BikeModelContent objModelArticles = new BikeModelContent();
            IEnumerable<ReviewEntity> objReview = null;
            IEnumerable<ArticleSummary> objRecentNews = null;
            IEnumerable<ArticleSummary> objExpertReview = null;
            IEnumerable<BikeVideoEntity> objVideos = null;

            try
            {
                List<EnumCMSContentType> categoryList = new List<EnumCMSContentType>();
                categoryList.Add(EnumCMSContentType.News);
                categoryList.Add(EnumCMSContentType.AutoExpo2018);
                _newsContentType = CommonApiOpn.GetContentTypesString(categoryList);
                var reviewTask = Task.Factory.StartNew(() => objReview = _userReviews.GetUserReviews(1, 2, Convert.ToUInt32(modelId), 0, FilterBy.MostRecent).ReviewList);
                var newsTask = Task.Factory.StartNew(() => objRecentNews = _cacheArticles.GetMostRecentArticlesByIdList(_newsContentType, 2, 0, Convert.ToUInt32(modelId)));
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
                ErrorClass.LogError(ex, string.Format("Bikewale.BAL.BikeData.BikeModels.GetRecentModelArticles({0})", modelId));
            }

            return objModelArticles;
        }


        /// <summary>
        /// Created by :   Sushil Kumar on 6th Sep 2017
        /// Description :  Added feature to get user reviews as per new approach 
        /// </summary>
        /// <param name="modelId"></param>
        /// <returns></returns>
        public Bikewale.Entities.BikeData.v2.BikeModelContent GetRecentModelArticlesv2(U modelId)
        {
            Entities.BikeData.v2.BikeModelContent objModelArticles = new Entities.BikeData.v2.BikeModelContent();
            IEnumerable<ArticleSummary> objRecentNews = null;
            IEnumerable<ArticleSummary> objExpertReview = null;
            IEnumerable<BikeVideoEntity> objVideos = null;
            Bikewale.Entities.UserReviews.Search.SearchResult userReviews = null;

            try
            {
                Bikewale.Entities.UserReviews.Search.InputFilters filters = new Bikewale.Entities.UserReviews.Search.InputFilters()
                {
                    Model = modelId.ToString(),
                    SO = 1,
                    PN = 1,
                    PS = 3,
                    Reviews = true
                };

                var reviewTask = Task.Factory.StartNew(() => userReviews = _userReviewsSearch.GetUserReviewsList(filters));
                var newsTask = Task.Factory.StartNew(() => objRecentNews = _cacheArticles.GetMostRecentArticlesByIdList(Convert.ToString((int)EnumCMSContentType.News), 2, 0, Convert.ToUInt32(modelId)));
                var expReviewTask = Task.Factory.StartNew(() => objExpertReview = _cacheArticles.GetMostRecentArticlesByIdList(Convert.ToString((int)EnumCMSContentType.RoadTest), 2, 0, Convert.ToUInt32(modelId)));
                var videosTask = Task.Factory.StartNew(() => objVideos = GetVideosByModelIdViaGrpc(Convert.ToInt32(modelId)));

                //calling tasks asynchronously, this will wait untill all tasks are completed
                Task.WaitAll(reviewTask, newsTask, expReviewTask, videosTask);

                if (userReviews != null && userReviews.Result != null)
                {
                    objModelArticles.ReviewDetails = userReviews.Result;
                }
                objModelArticles.News = objRecentNews;
                objModelArticles.ExpertReviews = objExpertReview;
                objModelArticles.Videos = objVideos;

            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("Bikewale.BAL.BikeData.BikeModels.GetRecentModelArticles({0})", modelId));
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
        /// Modified By : sushil Kumar on 4th Oct 2017
        /// Description : Remove unused call to modeldetails 
        ///                 Now only call two function for model details and model color photos
        ///                 Moved galleryimages call and videos call by model into this specified function
        ///                 Added task for all other calls after model details is fetched from the cache
        /// </summary>
        /// <param name="modelId"></param>
        /// <returns></returns>
        public ModelPhotoGalleryEntity GetPhotoGalleryData(U modelId)
        {
            ModelPhotoGalleryEntity objModelPhotoGalleryData = null;
            try
            {
                BikeModelPageEntity modelPage = new BikeModelPageEntity();
                modelPage.ModelDetails = _modelMaskingCache.GetById(Convert.ToInt32(modelId));

                if (modelPage.ModelDetails != null)
                {
                    objModelPhotoGalleryData = new ModelPhotoGalleryEntity();
                    objModelPhotoGalleryData.ObjModelEntity = modelPage.ModelDetails;

                    var colorPhotosTask = Task.Factory.StartNew(() => modelPage.colorPhotos = _modelCacheRepository.GetModelColorPhotos(modelId));
                    var galleryTask = Task.Factory.StartNew(() => modelPage.Photos = GetBikeModelPhotoGallery(modelId));
                    var videosTask = Task.Factory.StartNew(() => objModelPhotoGalleryData.VideosList = _videos.GetVideosByMakeModel(1, 50, (uint)objModelPhotoGalleryData.ObjModelEntity.MakeBase.MakeId, Convert.ToUInt32(modelId)));

                    Task.WaitAll(colorPhotosTask, galleryTask, videosTask);

                    objModelPhotoGalleryData.ImageList = CreateAllBikePhotosList(modelPage.ModelDetails, modelPage.Photos, modelPage.colorPhotos);
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("Bikewale.BAL.BikeData.BikeModels.GetPhotoGalleryData : modelId{0}", modelId));
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

                if (modelPhotos != null && modelPhotos.Any())
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

                    var colorImages = (colorPhotos != null && colorPhotos.Any()) ? colorPhotos.Select(x => new ColorImageBaseEntity()
                    {
                        HostUrl = x.Host,
                        OriginalImgPath = x.OriginalImagePath,
                        ColorId = x.BikeModelColorId,
                        ImageTitle = x.Name,
                        ImageId = x.Id,
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
                        ImageId = modelImage.ImageId,
                        ImageType = ImageBaseType.ModelGallaryImage,
                        ImageCategory = modelImage.ImageCategory
                    });

                    //Add Color Photos
                    if (colorImages != null && colorImages.Any())
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
                ErrorClass.LogError(ex, string.Format("Bikewale.BAL.BikeData.BikeModels.CreateAllPhotoList() : ModelId => {0}", modelId));
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
                    IEnumerable<ModelImage> galleryImages = null;
                    galleryImages = GetBikeModelPhotoGallery(modelId);


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
                    var colorImages = colorPhotos.Select(x => new ColorImageBaseEntity()
                    {
                        HostUrl = x.Host,
                        OriginalImgPath = x.OriginalImagePath,
                        ColorId = x.BikeModelColorId,
                        ImageTitle = x.Name,
                        ImageId = x.Id,
                        ImageType = ImageBaseType.ModelColorImage,
                        ImageCategory = x.ImageCategory,
                        Colors = x.ColorCodes.Select(y => y.HexCode)
                    });

                    if (colorImages != null && colorImages.Any())
                    {
                        allPhotos.AddRange(colorImages);
                    }

                    //// Wait for the WaitHandle to become signaled.
                    //result.AsyncWaitHandle.WaitOne();

                    //// Perform additional processing here.
                    //// Call EndInvoke to retrieve the results.
                    //galleryImages = caller.EndInvoke(result);

                    //// Close the wait handle.
                    //result.AsyncWaitHandle.Close();

                    //Add Model Gallery Photos
                    if (galleryImages != null && galleryImages.Any())
                    {
                        objModelPage.Photos = galleryImages;
                        var galleryBikeImages = galleryImages.Select(
                                m => new ColorImageBaseEntity()
                                {
                                    HostUrl = m.HostUrl,
                                    OriginalImgPath = m.OriginalImgPath,
                                    ImageCategory = m.ImageCategory,
                                    ImageTitle = m.ImageTitle,
                                    ImageId = m.ImageId,
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
                ErrorClass.LogError(ex, string.Format("Bikewale.BAL.BikeData.BikeModels.CreateAllPhotoList() : ModelId => {0}", modelId));
            }
        }








        /// <summary>
        /// Created by: Sushil Kumar on 4th Oct 2017
        /// Summary: To club all model images which includes modelimage,colorimages and gallery images
        /// </summary>
        /// <param name="objModelPage"></param>
        /// <returns></returns>
        private IEnumerable<ColorImageBaseEntity> CreateAllBikePhotosList(BikeModelEntity modelDetails, IEnumerable<ModelImage> galleryImages, IEnumerable<ModelColorImage> colorPhotos)
        {
            List<ColorImageBaseEntity> allPhotos = null;
            try
            {

                if (modelDetails != null && !String.IsNullOrEmpty(modelDetails.HostUrl) && !String.IsNullOrEmpty(modelDetails.OriginalImagePath))
                {
                    allPhotos = new List<ColorImageBaseEntity>();

                    var imageDesc = String.Format("{0} Model Image", modelDetails.ModelName);
                    //Add Model Image
                    allPhotos.Add(new ColorImageBaseEntity()
                    {
                        HostUrl = modelDetails.HostUrl,
                        OriginalImgPath = modelDetails.OriginalImagePath,
                        ImageCategory = "Model Image",
                        ImageTitle = imageDesc,
                        ImageType = ImageBaseType.ModelImage
                    });


                    if (colorPhotos != null)
                    {
                        var colorImages = colorPhotos.Select(x => new ColorImageBaseEntity()
                        {
                            HostUrl = x.Host,
                            OriginalImgPath = x.OriginalImagePath,
                            ColorId = x.BikeModelColorId,
                            ImageTitle = x.Name,
                            ImageId = x.Id,
                            ImageType = ImageBaseType.ModelColorImage,
                            ImageCategory = x.ImageCategory,
                            Colors = x.ColorCodes.Select(y => y.HexCode)
                        }).Where(m => !String.IsNullOrEmpty(m.OriginalImgPath));

                        if (colorImages != null && colorImages.Any())
                        {
                            allPhotos.AddRange(colorImages);
                        }
                    }

                    //Add Model Gallery Photos
                    if (galleryImages != null && galleryImages.Any())
                    {
                        var galleryBikeImages = galleryImages.Select(
                                m => new ColorImageBaseEntity()
                                {
                                    HostUrl = m.HostUrl,
                                    OriginalImgPath = m.OriginalImgPath,
                                    ImageCategory = m.ImageCategory,
                                    ImageTitle = m.ImageTitle,
                                    ImageId = m.ImageId,
                                    ImageType = ImageBaseType.ModelGallaryImage
                                });
                        if (galleryBikeImages != null)
                        {
                            allPhotos.AddRange(galleryBikeImages);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("Bikewale.BAL.BikeData.BikeModels.CreateAllBikePhotosList() : ModelId => {0}", (modelDetails != null ? modelDetails.ModelId : 0)));
            }

            return allPhotos;
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
                ErrorClass.LogError(ex, string.Format("Bikewale.BAL.BikeData.BikeModels.Getmodelcolorphotos ==> ModelId: {0}", modelId));
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
                ErrorClass.LogError(ex, string.Format("BikeModels.GetMostPopularScooters() => topCount {0}, cityId: {1}", topCount, cityId));
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
                            bikes = _modelCacheRepository.GetMostPopularScooters(makeId);
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
                ErrorClass.LogError(ex, String.Format("GetMostPopularBikes({0},{1},{2},{3})", requestType, topCount, makeId, cityId));
            }
            return bikes;
        }

        /// <summary>
        /// Created By:Snehal Dange on 3rd Nov 2017
        /// Descrption: Get mileage details and similar bikes by mileage 
        /// </summary>
        /// <param name="modelId"></param>
        /// <returns></returns>
        public BikeMileageEntity GetMileageDetails(uint modelId)
        {
            BikeMileageEntity mileageWidgetObj = null;
            try
            {
                if (modelId > 0)
                {
                    BikeMileageEntity obj = null;

                    obj = _modelCacheRepository.GetMileageDetails();
                    if (obj != null && obj.Bikes != null)
                    {

                        BikeWithMileageInfo currentModel = null;
                        ICollection<BikeWithMileageInfo> bikeList = null;
                        float tolerance = 0;
                        mileageWidgetObj = new BikeMileageEntity();
                        if (obj.Bikes != null)
                        {
                            currentModel = obj.Bikes.FirstOrDefault(m => m.Model.ModelId == modelId);
                        }
                        if (currentModel != null)
                        {
                            mileageWidgetObj.BodyStyleMileage = obj.BodyStyleMileage.Where(m => m.BodyStyleId == currentModel.BodyStyleId);
                            if (currentModel.Rank <= 3)
                            {
                                tolerance = ((currentModel.MileageByUserReviews) / 10);
                            }
                            if (obj.Bikes != null)
                            {
                                bikeList = obj.Bikes.Where(m => m.BodyStyleId == currentModel.BodyStyleId).ToList();
                            }
                            IList<BikeWithMileageInfo> mileageList = new List<BikeWithMileageInfo>();
                            if (bikeList != null)
                            {
                                byte i = 0;
                                foreach (var listObj in bikeList)
                                {
                                    if (listObj != null && listObj.Model != null && (listObj.Model.ModelId != modelId))
                                    {
                                        if ((listObj.Rank < currentModel.Rank) || (currentModel.Rank <= 3 && (listObj.MileageByUserReviews >= (currentModel.MileageByUserReviews - tolerance))))
                                        {
                                            mileageList.Add(listObj);
                                            if (++i == 9)
                                            {
                                                break;
                                            }
                                        }

                                    }

                                }
                            }

                            mileageList.Add(currentModel);
                            mileageWidgetObj.Bikes = mileageList;
                        }

                    }


                }
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass.LogError(ex, String.Format("BikeModels.GetMileageDetails()_ModelId: {0}", modelId));
            }
            return mileageWidgetObj;
        }

        /// <summary>
        /// Created by  : Vivek Singh Tomar on 28th Nov 2017
        /// Description : Get series by model id
        /// </summary>
        /// <param name="modelId"></param>
        /// <returns></returns>
        public BikeSeriesEntityBase GetSeriesByModelId(uint modelId)
        {
            BikeSeriesEntityBase objSeries = null;
            try
            {
                if (modelId > 0)
                {
                    objSeries = _modelCacheRepository.GetSeriesByModelId(modelId);
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("Bikewale.BAL.BikeData.BikeModels.GetSeriesByModelId modelId = {0}", modelId));
            }
            return objSeries;
        }

        /// <summary>
        /// Created By  : Rajan Chauhan on 29 Jan 2018
        /// Description : Get All ModelIds with BodyStyle of particular make 
        /// </summary>
        /// <param name="makeId"></param>
        /// <param name="bodyStyle"></param>
        /// <returns></returns>
        public IEnumerable<ModelIdWithBodyStyle> GetModelIdsForImages(uint makeId, EnumBikeBodyStyles bodyStyle)
        {
            IEnumerable<ModelIdWithBodyStyle> modelIdsWithBodyStyle = null;
            try
            {
                var objData = _modelCacheRepository.GetModelIdsForImages();
                if (objData != null)
                {
                    modelIdsWithBodyStyle = objData.Where(g => (g.MakeId == makeId || makeId == 0) && (bodyStyle.Equals(g.BodyStyle) || bodyStyle.Equals(EnumBikeBodyStyles.AllBikes)));
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.BAL.BikeData.BikeModels.GetModelIdsForImages");
            }
            return modelIdsWithBodyStyle;
        }

        /// <summary>
        /// Created by  : Vivek Singh Tomar on 11th Jan 2018
        /// Description : Get model ids with body style with required filters
        /// Functionality : list return will [startIndex, endIndex] i.e. inclusive, indexing starts from 1
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ModelIdWithBodyStyle> GetModelIdsForImages(uint makeId, EnumBikeBodyStyles bodyStyle, uint startIndex, uint endIndex)
        {
            IEnumerable<ModelIdWithBodyStyle> modelIdsWithBodyStyle = null;
            try
            {
                if (startIndex > 0 && (startIndex <= endIndex))
                {
                    var objData = _modelCacheRepository.GetModelIdsForImages();
                    if (objData != null)
                    {
                        modelIdsWithBodyStyle = objData.Where(g => (g.MakeId == makeId || makeId == 0) && (bodyStyle.Equals(g.BodyStyle) || bodyStyle.Equals(EnumBikeBodyStyles.AllBikes)));
                        if (modelIdsWithBodyStyle != null)
                        {
                            modelIdsWithBodyStyle = modelIdsWithBodyStyle.Skip(Convert.ToInt32(startIndex - 1)).Take(Convert.ToInt32(endIndex - startIndex + 1));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.BAL.BikeData.BikeModels.GetModelIdsForImages");
            }
            return modelIdsWithBodyStyle;
        }
        /// <summary>
        /// Created by  : Vivek Singh Tomar on 11th Jan 2018
        /// Description : Get model ids with body style with required filters
        /// Functionality : list return will [startIndex, endIndex] i.e. inclusive, indexing starts from 1
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ModelIdWithBodyStyle> GetModelIdsForImages(uint makeId, EnumBikeBodyStyles bodyStyle, ref ImagePager pager)
        {
            IEnumerable<ModelIdWithBodyStyle> modelIdsWithBodyStyle = null;
            try
            {
                if (pager.StartIndex > 0 && (pager.StartIndex <= pager.EndIndex))
                {
                    var objData = _modelCacheRepository.GetModelIdsForImages();
                    if (objData != null)
                    {
                        modelIdsWithBodyStyle = objData.Where(g => (g.MakeId == makeId || makeId == 0) && (bodyStyle.Equals(g.BodyStyle) || bodyStyle.Equals(EnumBikeBodyStyles.AllBikes)));
                        if (modelIdsWithBodyStyle != null)
                        {
                            pager.TotalResults = modelIdsWithBodyStyle.Count();
                            pager.TotalPages = (int)Math.Ceiling((double)pager.TotalResults / (double)pager.PageSize); //  / pager.PageSize;
                            modelIdsWithBodyStyle = modelIdsWithBodyStyle.Skip(pager.StartIndex - 1).Take(pager.EndIndex - pager.StartIndex + 1);
                            pager.CurrentSetResults = modelIdsWithBodyStyle.Count();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.BAL.BikeData.BikeModels.GetModelIdsForImages");
            }
            return modelIdsWithBodyStyle;
        }
        /// <summary>
        /// Created By  : Rajan Chauhan on 30 Jan 2017
        /// Description : Creation of lookup array for make images page
        /// </summary>
        /// <param name="makeId"></param>
        /// <returns></returns>
        public IDictionary<EnumBikeBodyStyles, IEnumerable<uint>> GetModelsWithBodyStyleLookupArray(uint makeId)
        {
            IDictionary<EnumBikeBodyStyles, IEnumerable<uint>> LookupArray = new Dictionary<EnumBikeBodyStyles,IEnumerable<uint>>();
            try
            {
                var objData = GetModelIdsForImages(makeId, EnumBikeBodyStyles.AllBikes);
                if (objData != null)
                {
                    IEnumerable<ModelIdWithBodyStyle> modelIdWithBodyStyle = null;
                    IEnumerable<uint> modelIds = null;
                    foreach (EnumBikeBodyStyles bodyStyle in _bodyStyles)
                    {
                        modelIdWithBodyStyle = objData.Where(g => (bodyStyle.Equals(g.BodyStyle)));
                        if(modelIdWithBodyStyle != null && modelIdWithBodyStyle.Any())
                        {
                            modelIds = modelIdWithBodyStyle.Select(g => g.ModelId);
                            LookupArray.Add(bodyStyle, modelIds);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("Bikewale.BAL.BikeData.BikeModels.GetModelsWithBodyStyleLookupArray : GetModelsWithBodyStyleLookupArray({0})", makeId));
            }
            return LookupArray;
        }
        private class MostPopularBikesBaseComparer : IEqualityComparer<MostPopularBikesBase>
        {

            public bool Equals(MostPopularBikesBase x, MostPopularBikesBase y)
            {


                if (Object.ReferenceEquals(x, y)) return true;


                if (Object.ReferenceEquals(x, null) || Object.ReferenceEquals(y, null))
                    return false;


                return x.objModel.ModelId == y.objModel.ModelId;
            }


            public int GetHashCode(MostPopularBikesBase product)
            {

                if (Object.ReferenceEquals(product, null)) return 0;


                int hashProductName = product.objModel.ModelName == null ? 0 : product.objModel.ModelName.GetHashCode();

                int hashProductCode = product.objModel.ModelId.GetHashCode();
                return hashProductName ^ hashProductCode;
            }

        }


    }   // Class
}   // namespace