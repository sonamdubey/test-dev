
using Bikewale.Common;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.PWA.Articles;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.Location;
using Bikewale.Interfaces.PWA.CMS;
using Bikewale.Interfaces.Videos;
using Bikewale.Models;
using Bikewale.Models.Videos;
using Bikewale.PWA.Entities.Images;
using Bikewale.PWA.Utils;
using Bikewale.Utility;
using log4net;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Bikewale.Controllers.Desktop.Videos
{
    /// <summary>
    /// Created by : Aditi Srivastava on 27 Feb 2017
    /// Summary    : Desktop Controller for make specific video page
    /// </summary>
    public class VideosController : Controller
    {
        private readonly IVideosCacheRepository _videos = null;
        private readonly IVideos _video = null;
        private readonly IBikeSeriesCacheRepository _seriesCache = null;
        private readonly IBikeMaskingCacheRepository<BikeModelEntity, int> _objModelCache = null;
        private readonly IBikeMakesCacheRepository _bikeMakesCache = null;
        private readonly ICityCacheRepository _cityCacheRepo = null;
        private readonly IBikeInfo _bikeInfo = null;
        private readonly IPWACMSCacheRepository _renderedArticles = null;
        private readonly IBikeModelsCacheRepository<int> _modelCache = null;
        private readonly IBikeSeries _series = null;
        private readonly IBikeModels<BikeModelEntity, int> _models;
        private readonly IBikeVersions<BikeVersionEntity, uint> _objVersion;


        static ILog _logger = LogManager.GetLogger("Pwa-Logger-VideoController");

        public VideosController(ICityCacheRepository cityCacheRepo, IBikeInfo bikeInfo, IBikeMakesCacheRepository bikeMakesCache, IVideosCacheRepository videos, IVideos video,
            IBikeMaskingCacheRepository<BikeModelEntity, int> objModelCache, IPWACMSCacheRepository renderedArticles, IBikeModelsCacheRepository<int> modelCache,
            IBikeSeriesCacheRepository seriesCache, IBikeSeries series, IBikeModels<BikeModelEntity, int> models, IBikeVersions<BikeVersionEntity, uint> objBikeVersions)
        {
            _videos = videos;
            _video = video;
            _objModelCache = objModelCache;
            _bikeMakesCache = bikeMakesCache;
            _cityCacheRepo = cityCacheRepo;
            _bikeInfo = bikeInfo;
            _renderedArticles = renderedArticles;
            _modelCache = modelCache;
            _seriesCache = seriesCache;
            _models = models;
            _series = series;
            _objVersion = objBikeVersions;
        }

        /// <summary>
        /// Created By : Sajal Gupta on 31-03-2017
        /// Descripton : This contoller will fetch data for vidoes landing page desktop
        /// </summary>
        /// <returns></returns>
        [Filters.DeviceDetection()]
        [Route("videos/Index/")]
        public ActionResult Index()
        {
            try
            {
                VideosLandingPage modelObj = new VideosLandingPage(_video, _videos, _bikeMakesCache, _objModelCache, _models);
                modelObj.LandingVideosTopCount = 5;
                modelObj.ExpertReviewsTopCount = 2;
                modelObj.FirstRideWidgetTopCount = 6;
                modelObj.LaunchAlertWidgetTopCount = 6;
                modelObj.FirstLookWidgetTopCount = 6;
                modelObj.PowerDriftBlockbusterWidgetTopCount = 6;
                modelObj.MotorSportsWidgetTopCount = 6;
                modelObj.PowerDriftSpecialsWidgetTopCount = 6;
                modelObj.PowerDriftTopMusicWidgetTopCount = 6;
                modelObj.MiscellaneousWidgetTopCount = 6;
                modelObj.BrandWidgetTopCount = 10;

                VideosLandingPageVM objVM = modelObj.GetData();

                return View(objVM);
            }
            catch (System.Exception ex)
            {
                ErrorClass.LogError(ex, "VideosController.Index");
                return HttpNotFound();
            }
        }
        /// <summary>
        /// Created By : Sajal Gupta on 31-03-2017
        /// Modified by: Prasad Gawde for PWA
        /// Descripton : This contoller will fetch data for vidoes landing page desktop
        /// </summary>
        /// <returns></returns>
        [Route("m/videos/Index/")]
        public ActionResult Index_Mobile_Pwa()
        {
            try
            {
                VideosLandingPage modelObj = new VideosLandingPage(_video, _videos, _bikeMakesCache, _objModelCache, _models);
                modelObj.LandingVideosTopCount = 5;
                modelObj.ExpertReviewsTopCount = 2;
                modelObj.FirstRideWidgetTopCount = 6;
                modelObj.LaunchAlertWidgetTopCount = 6;
                modelObj.FirstLookWidgetTopCount = 6;
                modelObj.PowerDriftBlockbusterWidgetTopCount = 6;
                modelObj.MotorSportsWidgetTopCount = 6;
                modelObj.PowerDriftSpecialsWidgetTopCount = 6;
                modelObj.PowerDriftTopMusicWidgetTopCount = 6;
                modelObj.MiscellaneousWidgetTopCount = 6;
                modelObj.BrandWidgetTopCount = 6;
                modelObj.IsMobile = true;
                VideosLandingPageVM objVM = modelObj.GetData();

                //construct the store for PWA
                try
                {
                    objVM.Store = ConstructStoreForListPage(objVM);
                    var storeJson = JsonConvert.SerializeObject(objVM.Store);

                    objVM.ServerRouterWrapper = _renderedArticles.GetVideoListDetails(PwaCmsHelper.GetSha256Hash(storeJson), objVM.Store.Videos.VideosLanding,
                                    "/m/bike-videos/", "root", "ServerRouterWrapper");

                    objVM.WindowState = storeJson;
                }
                catch (Exception ex)
                {
                    _logger.Error(ex);
                    return View("~/Views/Videos/Index_Mobile.cshtml", objVM);
                }
                return View(objVM);
            }
            catch (System.Exception ex)
            {
                ErrorClass.LogError(ex, "ServiceCentersController.Index_Mobile");
                return HttpNotFound();
            }
        }
        /// <summary>
        /// Created by Sajal Gupta on 01-04-2017 to fetch data fr category wise desktop videos page.
        /// </summary>
        /// <param name="categoryIdList"></param>
        /// <param name="title"></param>
        /// <returns></returns>
        [Route("videos/category/{categoryIdList}/title/{title}")]
        public ActionResult CategoryVideos(string categoryIdList, string title)
        {
            VideosByCategoryPage objModel = new VideosByCategoryPage(_videos, categoryIdList, title);
            VideosByCategoryPageVM objVM = objModel.GetData(9);
            return View(objVM);
        }

        /// <summary>
        /// Created by Sajal Gupta on 01-04-2017 to fetch data fr category wise mobile videos page.
        /// Modified by: Prasad Gawde for PWA
        /// </summary>
        /// <param name="categoryIdList"></param>
        /// <param name="title"></param>
        /// <returns></returns>
        [Route("m/videos/category/{categoryIdList}/title/{title}")]
        public ActionResult CategoryVideos_Mobile_Pwa(string categoryIdList, string title)
        {
            VideosByCategoryPage objModel = new VideosByCategoryPage(_videos, categoryIdList, title);
            objModel.IsMobile = true;
            VideosByCategoryPageVM objVM = objModel.GetData(9);

            //construct Store for PWA
            try
            {
                objVM.Store = ConstructStoreForSubCategoryPage(objVM);
                var storeJson = JsonConvert.SerializeObject(objVM.Store);

                objVM.ServerRouterWrapper = _renderedArticles.GetVideoBySubCategoryListDetails(PwaCmsHelper.GetSha256Hash(storeJson), objVM.Store.Videos.VideosByCategory,
                                string.Format("/m/bike-videos/category/{0}-{1}", title, categoryIdList), "root", "ServerRouterWrapper");

                objVM.WindowState = storeJson;
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return View("~/Views/Videos/CategoryVideos_Mobile.cshtml", objVM);
            }
            return View(objVM);
        }

        /// <summary>
        /// Created by : Sangram Nandkhile on 01 Mar 2017
        /// Summary: Action method for Video details Page
        /// Modified by :   Sumit Kate on 29 Mar 2017
        /// Description :   Videos details view action method
        /// </summary>
        [Filters.DeviceDetection()]
        [Route("videos/details/{videoId}/")]
        public ActionResult Details(uint videoId)
        {
            Bikewale.Models.VideoDetails objModel = new Bikewale.Models.VideoDetails(videoId, _videos);
            if (objModel.Status == Entities.StatusCodes.ContentFound)
            {
                VideoDetailsPageVM objVM = objModel.GetData();
                return View(objVM);
            }
            else if (objModel.Status == Entities.StatusCodes.ContentNotFound)
            {
                return HttpNotFound();
            }
            else
            {
                return HttpNotFound();
            }
        }
        /// <summary>
        /// Created by : Sangram Nandkhile on 01 Mar 2017
        /// Modified by: Prasad Gawde for PWA
        /// Summary: Action method for Video details Page
        /// Modified by :   Sumit Kate on 29 Mar 2017
        /// Description :   Videos details view action method
        /// </summary>
        [Route("m/videos/details/{videoId}/")]
        public ActionResult Details_Mobile_Pwa(uint videoId)
        {
            Bikewale.Models.VideoDetails objModel = new Bikewale.Models.VideoDetails(videoId, _videos);
            if (objModel.Status == Entities.StatusCodes.ContentFound)
            {
                objModel.IsMobile = true;
                VideoDetailsPageVM objVM = objModel.GetData();

                //construct Store for PWA
                try
                {
                    objVM.Store = ConstructStoreForDetailsPage(objVM);
                    var storeJson = JsonConvert.SerializeObject(objVM.Store);

                    objVM.ServerRouterWrapper = _renderedArticles.GetVideoDetails(PwaCmsHelper.GetSha256Hash(storeJson), objVM.Store.Videos.VideoDetail,
                                    string.Format("/m/bike-videos/{0}-{1}", objVM.Video.VideoTitleUrl, videoId), "root", "ServerRouterWrapper");

                    objVM.WindowState = storeJson;
                }
                catch (Exception ex)
                {
                    _logger.Error(ex);
                    return View("~/Views/Videos/Details_Mobile.cshtml", objVM);
                }
                return View(objVM);
            }
            else if (objModel.Status == Entities.StatusCodes.ContentNotFound)
            {
                return HttpNotFound();
            }
            else
            {
                return HttpNotFound();
            }
        }

        /// <summary>
        /// Created by Sajal Gupta on 01-04-2017
        /// Description : Controller for videos make wise page desktop 
        /// Modified by : Vivek Singh Tomar on 12th Dec 2017
        /// Description : Consider model status while redirecting
        /// </summary>
        /// <param name="makeMaskingName"></param>
        /// <param name="modelMaskingName"></param>
        /// <returns></returns>
        [Filters.DeviceDetection()]
        [Route("videos/make/{makeMaskingName}/model/{modelMaskingName}")]
        public ActionResult Models(string makeMaskingName, string modelMaskingName)
        {
            ModelWiseVideosPage objModel = new ModelWiseVideosPage(makeMaskingName, modelMaskingName, _cityCacheRepo, _bikeInfo, _videos, _bikeMakesCache, _objModelCache, _seriesCache, _series, _models, _objVersion, _modelCache);

            if (objModel.makeStatus == Entities.StatusCodes.RedirectPermanent || objModel.modelStatus == Entities.StatusCodes.RedirectPermanent)
            {
                objModel.redirectUrl = Request.RawUrl;
                if (objModel.makeStatus == Entities.StatusCodes.RedirectPermanent)
                {
                    objModel.redirectUrl = objModel.redirectUrl.Replace(makeMaskingName, objModel.newMakeMasking);
                }

                if (objModel.modelStatus == Entities.StatusCodes.RedirectPermanent)
                {
                    objModel.redirectUrl = objModel.redirectUrl.Replace(modelMaskingName, objModel.newModelMasking);
                }

                return RedirectPermanent(objModel.redirectUrl);
            }
            else if (objModel.makeStatus == Entities.StatusCodes.ContentNotFound || objModel.modelStatus == Entities.StatusCodes.ContentNotFound)
            {
                return HttpNotFound();
            }
            else
            {
                if (!objModel.objMaskingResponse.IsSeriesPageCreated)
                {
                    objModel.SimilarBikeWidgetTopCount = 9;
                    ModelWiseVideoPageVM objVM = objModel.GetData();
                    return View(objVM);
                }
                else
                {

                    ModelWiseVideoPageVM objVM = objModel.GetDataSeries();
                    return View(objVM);

                }
            }
        }

        /// <summary>
        /// Created by Sajal Gupta on 01-04-2017
        /// Description : Controller for videos make wise page mobile 
        /// Modified by : Vivek Singh Tomar on 12th Dec 2017
        /// Description : Consider model status while redirecting
        /// </summary>
        /// <param name="makeMaskingName"></param>
        /// <param name="modelMaskingName"></param>
        /// <returns></returns>
        [Route("m/videos/make/{makeMaskingName}/model/{modelMaskingName}")]
        public ActionResult Models_Mobile(string makeMaskingName, string modelMaskingName)
        {
            ModelWiseVideosPage objModel = new ModelWiseVideosPage(makeMaskingName, modelMaskingName, _cityCacheRepo, _bikeInfo, _videos, _bikeMakesCache, _objModelCache, _seriesCache, _series, _models, _objVersion, _modelCache);

            objModel.IsMobile = true;
            if (objModel.makeStatus == Entities.StatusCodes.RedirectPermanent || objModel.modelStatus == Entities.StatusCodes.RedirectPermanent)
            {
                objModel.redirectUrl = Request.RawUrl;
                if (objModel.makeStatus == Entities.StatusCodes.RedirectPermanent)
                {
                    objModel.redirectUrl = objModel.redirectUrl.Replace(makeMaskingName, objModel.newMakeMasking);
                }

                if (objModel.modelStatus == Entities.StatusCodes.RedirectPermanent)
                {
                    objModel.redirectUrl = objModel.redirectUrl.Replace(modelMaskingName, objModel.newModelMasking);
                }

                return RedirectPermanent(objModel.redirectUrl);
            }
            else if (objModel.makeStatus == Entities.StatusCodes.ContentNotFound || objModel.modelStatus == Entities.StatusCodes.ContentNotFound)
            {
                return HttpNotFound();
            }
            else
            {
                if (!objModel.objMaskingResponse.IsSeriesPageCreated)
                {
                    objModel.SimilarBikeWidgetTopCount = 9;
                    ModelWiseVideoPageVM objVM = objModel.GetData();
                    return View(objVM);
                }
                else
                {

                    ModelWiseVideoPageVM objVM = objModel.GetDataSeries();
                    return View(objVM);

                }
            }
        }

        /// <summary>
        /// Created by  :   Aditi Srivastava on 27 Feb 2017
        /// Summary     :   Fetch videos page by masking name
        /// Modified by :   Sumit Kate on 29 Mar 2017
        /// Description :   Make wise video page desktop action method
        /// Modified by :   Pratibha Verma on 8 Feb 2018
        /// Description :   Added parameter in MakeVideosPage constructor for Linkage of Image Page from Video
        /// </summary>
        [Filters.DeviceDetection()]
        [Route("videos/make/{makeMaskingName}/")]
        public ActionResult Makes(string makeMaskingName)
        {
            MakeVideosPage objModel = new MakeVideosPage(makeMaskingName, _videos, _models);
            if (objModel.Status == Entities.StatusCodes.ContentFound)
            {
                return View(objModel.GetData());
            }
            else if (objModel.Status == Entities.StatusCodes.RedirectPermanent)
            {
                return RedirectPermanent(objModel.RedirectUrl);
            }
            else
            {
                return HttpNotFound();
            }
        }

        /// <summary>
        /// Created by : Aditi Srivastava on 27 Feb 2017
        /// Summary    : ActionResult method for make wise videos page
        /// Modified by :   Sumit Kate on 29 Mar 2017
        /// Description :   Make wise video page mobile action method
        /// Modified by :   Pratibha Verma on 8 Feb 2018
        /// Description :   Added parameter in MakeVideosPage constructor for Linkage of Image Page from Video
        /// </summary>
        /// <param name="makeMaskingName"></param>
        /// <returns></returns>
        [Route("m/videos/make/{makeMaskingname}/")]
        public ActionResult Makes_Mobile(string makeMaskingName)
        {
            MakeVideosPage objModel = new MakeVideosPage(makeMaskingName, _videos, _models);

            if (objModel.Status == Entities.StatusCodes.ContentFound)
            {
                objModel.IsMobile = true;
                return View(objModel.GetData());
            }
            else if (objModel.Status == Entities.StatusCodes.RedirectPermanent)
            {
                return RedirectPermanent(objModel.RedirectUrl);
            }
            else
            {
                return HttpNotFound();
            }
        }

        /// <summary>
        /// Created by : Sangram Nandkhile on 01 Mar 2017
        /// Summary: Partial view to show Similar model bike videos
        /// </summary>
        [Route("videos/SimilarVideos/")]
        public ActionResult SimilarVideos(uint videoId, uint modelId)
        {
            SimilarVideosModel model = new SimilarVideosModel(modelId, videoId, _video);
            return PartialView("~/views/videos/_SimilarVideos.cshtml", model.GetData());
        }

        /// <summary>
        /// Created by : Aditi Srivastava 2 March 2017
        /// Summary    : Partial view for similar model videos 
        /// </summary>
        [Route("m/videos/SimilarVideos/")]
        public ActionResult SimilarVideos_Mobile(uint videoId, uint modelId)
        {
            SimilarVideosModel model = new SimilarVideosModel(modelId, videoId, _video);
            return PartialView("~/views/videos/_SimilarVideos_Mobile.cshtml", model.GetData());
        }
        /// <summary>
        /// Created by : Ashutosh Sharma on 17-Aug-2017
        /// Description : Action method for scooter videos for desktop .
        /// </summary>
        [Filters.DeviceDetection()]
        [Route("scooters/videos/")]
        public ActionResult ScooterVideos()
        {
            ScooterVideosVM objVideosList = null;
            try
            {
                ScooterVideos objVideosModel = new ScooterVideos(_video);
                objVideosList = objVideosModel.GetData();
            }
            catch (System.Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.Controllers.Videos.ScooterVideos");

            }


            return View(objVideosList);


        }
        /// <summary>
        /// Created by : Ashutosh Sharma on 17-Aug-2017
        /// Description : Action method for scooter videos for desktop .
        /// </summary>
        [Route("m/scooters/videos/")]
        public ActionResult ScooterVideos_Mobile()
        {

            ScooterVideosVM objVideosList = null;
            try
            {

                ScooterVideos objVideosModel = new ScooterVideos(_video);

                objVideosModel.IsMobile = true;

                objVideosList = objVideosModel.GetData();

            }
            catch (System.Exception ex)
            {

                ErrorClass.LogError(ex, "Bikewale.Controllers.Videos.ScooterVideos_Mobile");

            }

            return View(objVideosList);


        }

        #region Construct PWA Redux Store
        /// <summary>
        /// Modified by : Ashutosh Sharma on 14 Feb 2018
        /// Description : Added code to bind popular bike models images to redux store.
        /// </summary>
        /// <param name="objVM"></param>
        /// <returns></returns>
        private PwaReduxStore ConstructStoreForListPage(VideosLandingPageVM objVM)
        {
            //construct the store for PWA
            PwaReduxStore store = new PwaReduxStore();

            var allVideos = store.Videos.VideosLanding;
            var topVideos = new PwaVideosLandingPageTopVideos(); ;
            var otherVideos = new PwaVideosLandingPageOtherVideos();
            var popularBikeImages = new PwaPopularBikeImagesListData();

            //set top Videos
            var pwaLandingVideos = new List<PwaBikeVideoEntity>();
            pwaLandingVideos.Add(ConverterUtility.PwaConvert(objVM.LandingFirstVideoData));
            ConverterUtility.PwaCovertAndAppend(pwaLandingVideos, objVM.LandingOtherVideosData);
            topVideos.LandingFirstVideos = pwaLandingVideos;

            if (objVM.ExpertReviewsWidgetData != null)
            {
                topVideos.ExpertReviews = PwaCmsHelper.SetPwaSubCategoryVideos(objVM.ExpertReviewsWidgetData.VideoList.Videos, 55, true);
            }
            //set other Videos            
            if (objVM.FirstLookWidgetData != null)
                otherVideos.FirstLook = PwaCmsHelper.SetPwaSubCategoryVideos(objVM.FirstLookWidgetData.VideoList.Videos, 61);

            if (objVM.FirstRideWidgetData != null)
                otherVideos.FirstRide = PwaCmsHelper.SetPwaSubCategoryVideos(objVM.FirstRideWidgetData.VideoList.Videos, 57);

            if (objVM.LaunchAlertWidgetData != null)
                otherVideos.LaunchAlert = PwaCmsHelper.SetPwaSubCategoryVideos(objVM.LaunchAlertWidgetData.VideoList.Videos, 59);

            if (objVM.MiscellaneousWidgetData != null)
                otherVideos.Miscellaneous = PwaCmsHelper.SetPwaSubCategoryVideos(objVM.MiscellaneousWidgetData.VideoList.Videos, 58);

            if (objVM.MotorSportsWidgetData != null)
                otherVideos.MotorSports = PwaCmsHelper.SetPwaSubCategoryVideos(objVM.MotorSportsWidgetData.VideoList.Videos, 51);

            if (objVM.PowerDriftBlockbusterWidgetData != null)
                otherVideos.PowerDriftBlockbuster = PwaCmsHelper.SetPwaSubCategoryVideos(objVM.PowerDriftBlockbusterWidgetData.VideoList.Videos, 62);

            if (objVM.PowerDriftSpecialsWidgetData != null)
                otherVideos.PowerDriftSpecials = PwaCmsHelper.SetPwaSubCategoryVideos(objVM.PowerDriftSpecialsWidgetData.VideoList.Videos, 63);

            if (objVM.PowerDriftTopMusicWidgetData != null)
                otherVideos.PowerDriftTopMusic = PwaCmsHelper.SetPwaSubCategoryVideos(objVM.PowerDriftTopMusicWidgetData.VideoList.Videos, 60);

            PwaBrandsInfo pwaBrands = new PwaBrandsInfo();
            pwaBrands.TopBrands = ConverterUtility.PwaConvert(objVM.Brands.TopBrands);
            pwaBrands.OtherBrands = ConverterUtility.PwaConvert(objVM.Brands.OtherBrands);
            otherVideos.Brands = pwaBrands;

            if (objVM.PopularSportsBikesWidget != null)
            {
                popularBikeImages.BikeImagesList = ConverterUtility.PwaConvert(objVM.PopularSportsBikesWidget.ModelList);
            }

            allVideos.TopVideos = topVideos;
            allVideos.OtherVideos = otherVideos;
            store.Widgets.BikeImagesCarouselReducer.PopularBikeImagesListData = popularBikeImages;

            return store;
        }

        private PwaReduxStore ConstructStoreForDetailsPage(VideoDetailsPageVM objVM)
        {
            //construct the store for PWA
            PwaReduxStore store = new PwaReduxStore();
            uint videoId = objVM.Video.BasicId;
            var videoDetail = store.Videos.VideoDetail;
            uint taggedModel = objVM.TaggedModelId;
            var videoInfo = videoDetail.VideoInfo;

            videoInfo.VideoInfo = ConverterUtility.PwaConvert(objVM.Video, true);

            var relatedInfoList = new List<PwaBikeVideoRelatedInfo>();

            var modelMaskingName = objVM.Video.MaskingName;

            if (taggedModel != 0) //add related videos only if the model is tagged
            {
                relatedInfoList.Add(new PwaBikeVideoRelatedInfo(PwaRelatedInfoType.Video, string.Format("api/pwa/similarvideos/{0}/modelid/{1}", videoId, taggedModel)));
            }
            else
                relatedInfoList.Add(null);
            //    relatedInfoList.Add(new PwaBikeVideoRelatedInfo(PwaRelatedInfoType.Video, string.Empty));

            relatedInfoList.Add(new PwaBikeVideoRelatedInfo(PwaRelatedInfoType.Bike, string.Format("api/pwa/popularbodystyle/modelid/{0}/count/9", taggedModel)));

            videoInfo.RelatedInfoApi = relatedInfoList;


            //similarvideos
            var pwarelatedInfo = videoDetail.RelatedInfo;

            if (taggedModel > 0) //no simialr videos if model is not tagged
            {
                SimilarVideosModel model = new SimilarVideosModel(taggedModel, videoId, _video);
                var similaVideosData = model.GetData();

                var pwaVidList = new PwaBikeVideos();

                if (similaVideosData != null && similaVideosData.Videos != null)
                {
                    pwarelatedInfo.VideoList = pwaVidList;
                    var videosList = ConverterUtility.PwaConvert(similaVideosData.Videos);
                    if (videosList != null)
                        pwaVidList.VideosList = videosList.ToList();
                    pwaVidList.CompleteListUrl = similaVideosData.ViewAllLinkUrl;
                    pwaVidList.CompleteListUrlAlternateLabel = similaVideosData.ViewAllLinkTitle;
                    pwaVidList.Heading = "More related videos";
                    pwaVidList.CompleteListUrlLabel = similaVideosData.ViewAllLinkText;
                }
            }
            else
                pwarelatedInfo.VideoList = null;

            //bikelist            
            var cityArea = GlobalCityArea.GetGlobalCityArea();
            IEnumerable<MostPopularBikesBase> objPopularBodyStyle = _modelCache.GetMostPopularBikesByModelBodyStyle((int)taggedModel, 9, objVM.CityId);
            PwaBikeNews outBikeData = new PwaBikeNews();
            if (objPopularBodyStyle != null)
                outBikeData.BikesList = ConverterUtility.MapMostPopularBikesBaseToPwaBikeDetails(objPopularBodyStyle);
            outBikeData.Heading = "Popular bikes";
            outBikeData.CompleteListUrl = "/m/best-bikes-in-india/";
            outBikeData.CompleteListUrlAlternateLabel = "Best Bikes in India";
            outBikeData.CompleteListUrlLabel = "View all";
            pwarelatedInfo.BikeList = outBikeData;

            //tagged bike
            if (taggedModel != 0)
            {
                BikeInfoWidget bikeInfo = new BikeInfoWidget(_bikeInfo, _cityCacheRepo, taggedModel, objVM.CityId, 3, Bikewale.Entities.GenericBikes.BikeInfoTabType.Videos);
                var bikeInfoVm = bikeInfo.GetData();
                videoDetail.ModelInfo = ConverterUtility.MapGenericBikeInfoToPwaBikeInfo(bikeInfoVm);
            }
            return store;
        }

        private PwaReduxStore ConstructStoreForSubCategoryPage(VideosByCategoryPageVM objVM)
        {
            //construct the store for PWA
            PwaReduxStore store = new PwaReduxStore();

            var subCatVideos = store.Videos.VideosByCategory;
            if (objVM.Videos != null)
            {
                subCatVideos.Videos = ConverterUtility.PwaConvert(objVM.Videos.Videos);
                subCatVideos.SectionTitle = objVM.TitleName;
                subCatVideos.MoreVideosUrl = string.Empty;
            }
            return store;
        }

        #endregion
    }
}