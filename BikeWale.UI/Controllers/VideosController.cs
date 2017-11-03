
using Bikewale.Common;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.PWA.Articles;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.Location;
using Bikewale.Interfaces.PWA.CMS;
using Bikewale.Interfaces.Videos;
using Bikewale.Models;
using Bikewale.Models.Videos;
using Bikewale.PWA.Utils;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Linq;
using Bikewale.Controls;
using Bikewale.Utility;

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
        private readonly IBikeMakes<BikeMakeEntity, int> _bikeMakes = null;
        private readonly IBikeMaskingCacheRepository<BikeModelEntity, int> _objModelCache = null;
        private readonly IBikeMakesCacheRepository<int> _bikeMakesCache = null;
        private readonly ICityCacheRepository _cityCacheRepo = null;
        private readonly IBikeInfo _bikeInfo = null;
        private readonly IPWACMSCacheRepository _renderedArticles = null;
        private readonly IBikeModelsCacheRepository<int> _modelCache = null;

        public VideosController(ICityCacheRepository cityCacheRepo, IBikeInfo bikeInfo, IBikeMakesCacheRepository<int> bikeMakesCache, IVideosCacheRepository videos, IVideos video, IBikeMakes<BikeMakeEntity, int> bikeMakes, IBikeMaskingCacheRepository<BikeModelEntity, int> objModelCache,
            IPWACMSCacheRepository renderedArticles,
             IBikeModelsCacheRepository<int> modelCache)
        {
            _videos = videos;
            _video = video;
            _bikeMakes = bikeMakes;
            _objModelCache = objModelCache;
            _bikeMakesCache = bikeMakesCache;
            _cityCacheRepo = cityCacheRepo;
            _bikeInfo = bikeInfo;
            _renderedArticles = renderedArticles;
            _modelCache = modelCache;
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
                VideosLandingPage modelObj = new VideosLandingPage(_video, _videos, _bikeMakes, _objModelCache);
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
                ErrorClass objErr = new ErrorClass(ex, "ServiceCentersController.Index");
                return Redirect("/pagenotfound.aspx");
            }
        }

        /// <summary>
        /// Created By : Sajal Gupta on 31-03-2017
        /// Descripton : This contoller will fetch data for vidoes landing page desktop
        /// </summary>
        /// <returns></returns>
        [Route("m/videos/Index/")]
        public ActionResult Index_Mobile()
        {
            try
            {
                VideosLandingPage modelObj = new VideosLandingPage(_video, _videos, _bikeMakes, _objModelCache);
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

                VideosLandingPageVM objVM = modelObj.GetData();

                //construct the store for PWA
                
                objVM.Store = ConstructStoreForListPage(objVM);
                var storeJson = JsonConvert.SerializeObject(objVM.Store);

                objVM.ServerRouterWrapper = _renderedArticles.GetVideoListDetails(ConverterUtility.GetSha256Hash(storeJson), objVM.Store.Videos.VideosLanding,
                                "/m/bike-videos/", "root", "ServerRouterWrapper");

                objVM.WindowState = storeJson;

                return View(objVM);
            }
            catch (System.Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "ServiceCentersController.Index_Mobile");
                return Redirect("/pagenotfound.aspx");
            }
        }

        private PwaReduxStore ConstructStoreForListPage(VideosLandingPageVM objVM)
        {
            //construct the store for PWA
            PwaReduxStore store = new PwaReduxStore();

            var allVideos = store.Videos.VideosLanding;
            var topVideos = new PwaVideosLandingPageTopVideos(); ;
            var otherVideos = new PwaVideosLandingPageOtherVideos();


            //set top Videos
            var pwaLandingVideos = new List<PwaBikeVideoEntity>();
            pwaLandingVideos.Add(ConverterUtility.PwaConvert(objVM.LandingFirstVideoData));
            ConverterUtility.PwaCovertAndAppend(pwaLandingVideos, objVM.LandingOtherVideosData);
            topVideos.LandingFirstVideos = pwaLandingVideos;

            var pwaExpertReviews = new PwaVideosBySubcategory();
            var expertReviews = objVM.ExpertReviewsWidgetData;
            if (expertReviews != null)
            {
                pwaExpertReviews.Videos =
                    ConverterUtility.PwaConvert(expertReviews.VideoList.Videos);
                pwaExpertReviews.MoreVideosUrl = @"/expert-reviews-55/";
                pwaExpertReviews.SectionTitle = expertReviews.SectionTitle;
            }
            topVideos.ExpertReviews = pwaExpertReviews;

            //set other Videos


            otherVideos.FirstLook = new PwaVideosBySubcategory();
            if(objVM.FirstLookWidgetData!=null)
            {
                otherVideos.FirstLook.Videos = ConverterUtility.PwaConvert(objVM.FirstLookWidgetData.VideoList.Videos);
            }
                otherVideos.FirstLook.MoreVideosUrl = @"/first-look-61/";
            otherVideos.FirstLook.SectionTitle = "First Look";

            otherVideos.FirstRide = new PwaVideosBySubcategory();
            if (objVM.FirstRideWidgetData != null)
            {
                otherVideos.FirstRide.Videos = ConverterUtility.PwaConvert(objVM.FirstRideWidgetData.VideoList.Videos);
            }
            otherVideos.FirstRide.MoreVideosUrl = @"/first-ride-impressions-57/";
            otherVideos.FirstRide.SectionTitle = "First Ride Impressions";

            otherVideos.LaunchAlert = new PwaVideosBySubcategory();
            if (objVM.LaunchAlertWidgetData != null)
            {
                otherVideos.LaunchAlert.Videos = ConverterUtility.PwaConvert(objVM.LaunchAlertWidgetData.VideoList.Videos);
            }
            otherVideos.LaunchAlert.MoreVideosUrl = @"/launch-alert-59/";
            otherVideos.LaunchAlert.SectionTitle = "Launch Alert";


            otherVideos.Miscellaneous = new PwaVideosBySubcategory();
            if(objVM.MiscellaneousWidgetData!=null)
                otherVideos.Miscellaneous.Videos = ConverterUtility.PwaConvert(objVM.MiscellaneousWidgetData.VideoList.Videos);
            otherVideos.Miscellaneous.MoreVideosUrl = @"/miscellaneous-58/";
            otherVideos.Miscellaneous.SectionTitle = "Miscellaneous";

            otherVideos.MotorSports = new PwaVideosBySubcategory();
            if(objVM.MotorSportsWidgetData!=null)
                otherVideos.MotorSports.Videos = ConverterUtility.PwaConvert(objVM.MotorSportsWidgetData.VideoList.Videos);
            otherVideos.MotorSports.MoreVideosUrl = @"/motorsports-51/";
            otherVideos.MotorSports.SectionTitle = "Motorsports";

            otherVideos.PowerDriftBlockbuster = new PwaVideosBySubcategory();
            if(objVM.PowerDriftBlockbusterWidgetData!=null)
                otherVideos.PowerDriftBlockbuster.Videos = ConverterUtility.PwaConvert(objVM.PowerDriftBlockbusterWidgetData.VideoList.Videos);
            otherVideos.PowerDriftBlockbuster.MoreVideosUrl = @"/powerdrift-uster-62/";
            otherVideos.PowerDriftBlockbuster.SectionTitle = "PowerDrift Blockbuster";

            otherVideos.PowerDriftSpecials = new PwaVideosBySubcategory();
            if(objVM.PowerDriftSpecialsWidgetData!=null)
                otherVideos.PowerDriftSpecials.Videos = ConverterUtility.PwaConvert(objVM.PowerDriftSpecialsWidgetData.VideoList.Videos);
            otherVideos.PowerDriftSpecials.MoreVideosUrl = @"/powerdrift-specials-63/";
            otherVideos.PowerDriftSpecials.SectionTitle = "PowerDrift Specials";

            otherVideos.PowerDriftTopMusic = new PwaVideosBySubcategory();
            if(objVM.PowerDriftTopMusicWidgetData!=null)
                otherVideos.PowerDriftTopMusic.Videos = ConverterUtility.PwaConvert(objVM.PowerDriftTopMusicWidgetData.VideoList.Videos);
            otherVideos.PowerDriftTopMusic.MoreVideosUrl = @"/PowerDrift-top-music-60/";
            otherVideos.PowerDriftTopMusic.SectionTitle = "PowerDrift Top Music";

            PwaBrandsInfo pwaBrands = new PwaBrandsInfo();
            pwaBrands.TopBrands = ConverterUtility.PwaConvert(objVM.Brands.TopBrands);
            pwaBrands.OtherBrands = ConverterUtility.PwaConvert(objVM.Brands.OtherBrands);
            otherVideos.Brands = pwaBrands;


            allVideos.TopVideos = topVideos;
            allVideos.OtherVideos = otherVideos;

            return store;
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
        /// </summary>
        /// <param name="categoryIdList"></param>
        /// <param name="title"></param>
        /// <returns></returns>
        [Route("m/videos/category/{categoryIdList}/title/{title}")]
        public ActionResult CategoryVideos_Mobile(string categoryIdList, string title)
        {
            VideosByCategoryPage objModel = new VideosByCategoryPage(_videos, categoryIdList, title);
            VideosByCategoryPageVM objVM = objModel.GetData(9);

            //construct Store for PWA
            objVM.Store = ConstructStoreForSubCategoryPage(objVM);
            var storeJson = JsonConvert.SerializeObject(objVM.Store);

            objVM.ServerRouterWrapper = _renderedArticles.GetVideoBySubCategoryListDetails(ConverterUtility.GetSha256Hash(storeJson), objVM.Store.Videos.VideosByCategory,
                            string.Format("/m/bike-videos/category/{0}-{1}",title,categoryIdList), "root", "ServerRouterWrapper");

            objVM.WindowState = storeJson;
            return View(objVM);
        }

        private PwaReduxStore ConstructStoreForSubCategoryPage(VideosByCategoryPageVM objVM)
        {
            //construct the store for PWA
            PwaReduxStore store = new PwaReduxStore();

            var subCatVideos = store.Videos.VideosByCategory;
            if (objVM.Videos != null)
            {
                subCatVideos.Videos = ConverterUtility.PwaConvert(objVM.Videos.Videos);
                subCatVideos.SectionTitle = "";
                subCatVideos.MoreVideosUrl = "";
            }                    
            return store;
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
                return Redirect("/pagenotfound.aspx");
            }
            else
            {
                return Redirect("/pagenotfound.aspx");
            }
        }


        /// <summary>
        /// Created by : Sangram Nandkhile on 01 Mar 2017
        /// Summary: Action method for Video details Page
        /// Modified by :   Sumit Kate on 29 Mar 2017
        /// Description :   Videos details view action method
        /// </summary>
        [Route("m/videos/details/{videoId}/")]
        public ActionResult Details_Mobile(uint videoId)
        {
            Bikewale.Models.VideoDetails objModel = new Bikewale.Models.VideoDetails(videoId, _videos);
            if (objModel.Status == Entities.StatusCodes.ContentFound)
            {
                VideoDetailsPageVM objVM = objModel.GetData();

                //construct Store for PWA
                objVM.Store = ConstructStoreForDetailsPage(objVM);
                var storeJson = JsonConvert.SerializeObject(objVM.Store);

                objVM.ServerRouterWrapper = _renderedArticles.GetVideoDetails(ConverterUtility.GetSha256Hash(storeJson), objVM.Store.Videos.VideoDetail,
                                string.Format("/m/bike-videos/{0}-{1}", objVM.Video.VideoTitleUrl, videoId), "root", "ServerRouterWrapper");


                objVM.WindowState = storeJson;

                return View(objVM);
            }
            else if (objModel.Status == Entities.StatusCodes.ContentNotFound)
            {
                return Redirect("/pagenotfound.aspx");
            }
            else
            {
                return Redirect("/pagenotfound.aspx");
            }
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
        
            var relatedInfo = new PwaBikeVideoRelatedInfo();
            relatedInfo.Type = PwaRelatedInfoType.Video;
            relatedInfo.Url = string.Format("api/pwa/similarvideos/{0}/modelid/{1}", videoId, taggedModel);
            relatedInfoList.Add(relatedInfo);

            relatedInfo = new PwaBikeVideoRelatedInfo();
            relatedInfo.Type = PwaRelatedInfoType.Bike;
            relatedInfo.Url = string.Format("api/pwa/popularbodystyle/modelid/{0}/count/9", taggedModel);
            relatedInfoList.Add(relatedInfo);
            videoInfo.RelatedInfoApi = relatedInfoList;


            //similarvideos
            var pwarelatedInfo = videoDetail.RelatedInfo;
            SimilarVideosModel model = new SimilarVideosModel(taggedModel, videoId, _video);
            var similaVideosData = model.GetData();
            var videosList = ConverterUtility.PwaConvert(similaVideosData.Videos);
            var pwaVidList = new PwaBikeVideos();
            pwarelatedInfo.VideoList = pwaVidList;
            if (videosList != null)
                pwaVidList.VideosList = videosList.ToList();
            pwaVidList.CompleteListUrl = similaVideosData.ViewAllLinkUrl;
            pwaVidList.CompleteListUrlAlternateLabel = similaVideosData.ViewAllLinkTitle;
            pwaVidList.Heading = "More related videos";
            pwaVidList.CompleteListUrlLabel = similaVideosData.ViewAllLinkText;

            //bikelist
            
            var cityArea = GlobalCityArea.GetGlobalCityArea();
            IEnumerable<MostPopularBikesBase> objPopularBodyStyle = _modelCache.GetMostPopularBikesByModelBodyStyle((int)taggedModel, 9, objVM.CityId);
            PwaBikeNews outBikeData = new PwaBikeNews();
            outBikeData.BikesList = ConverterUtility.MapMostPopularBikesBaseToPwaBikeDetails(objPopularBodyStyle, cityArea.City);
            outBikeData.Heading = "Popular bikes";
            outBikeData.CompleteListUrl = "/m/best-bikes-in-india/";
            outBikeData.CompleteListUrlAlternateLabel = "Best Bikes in India";
            outBikeData.CompleteListUrlLabel = "View all";
            pwarelatedInfo.BikeList = outBikeData;

            //tagged bike
            BikeInfoWidget bikeInfo = new BikeInfoWidget(_bikeInfo, _cityCacheRepo, taggedModel, objVM.CityId, 3, Bikewale.Entities.GenericBikes.BikeInfoTabType.Videos);
            var bikeInfoVm = bikeInfo.GetData();
            videoDetail.ModelInfo= ConverterUtility.MapGenericBikeInfoToPwaBikeInfo(bikeInfoVm);

            return store;
        }

        /// <summary>
        /// Created by Sajal Gupta on 01-04-2017
        /// Description : Controller for videos make wise page desktop 
        /// </summary>
        /// <param name="makeMaskingName"></param>
        /// <param name="modelMaskingName"></param>
        /// <returns></returns>
        [Filters.DeviceDetection()]
        [Route("videos/make/{makeMaskingName}/model/{modelMaskingName}")]
        public ActionResult Models(string makeMaskingName, string modelMaskingName)
        {
            ModelWiseVideosPage objModel = new ModelWiseVideosPage(makeMaskingName, modelMaskingName, _cityCacheRepo, _bikeInfo, _videos, _bikeMakesCache, _objModelCache);

            if (objModel.makeStatus == Entities.StatusCodes.ContentFound && objModel.modelStatus == Entities.StatusCodes.ContentFound)
            {
                objModel.SimilarBikeWidgetTopCount = 9;
                ModelWiseVideoPageVM objVM = objModel.GetData();
                return View(objVM);
            }
            else if (objModel.makeStatus == Entities.StatusCodes.RedirectPermanent || objModel.modelStatus == Entities.StatusCodes.RedirectPermanent)
            {
                if (objModel.makeStatus == Entities.StatusCodes.RedirectPermanent)
                {
                    return RedirectPermanent(Request.RawUrl.Replace(makeMaskingName, objModel.objMakeResponse.MaskingName));
                }
                else
                {
                    return RedirectPermanent(Request.RawUrl.Replace(makeMaskingName, objModel.objModelResponse.MaskingName));
                }
            }
            else
            {
                return Redirect(CommonOpn.AppPath + "pageNotFound.aspx");
            }
        }

        /// <summary>
        /// Created by Sajal Gupta on 01-04-2017
        /// Description : Controller for videos make wise page mobile 
        /// </summary>
        /// <param name="makeMaskingName"></param>
        /// <param name="modelMaskingName"></param>
        /// <returns></returns>
        [Route("m/videos/make/{makeMaskingName}/model/{modelMaskingName}")]
        public ActionResult Models_Mobile(string makeMaskingName, string modelMaskingName)
        {
            ModelWiseVideosPage objModel = new ModelWiseVideosPage(makeMaskingName, modelMaskingName, _cityCacheRepo, _bikeInfo, _videos, _bikeMakesCache, _objModelCache);
            if (objModel.makeStatus == Entities.StatusCodes.ContentFound && objModel.modelStatus == Entities.StatusCodes.ContentFound)
            {
                objModel.SimilarBikeWidgetTopCount = 9;
                ModelWiseVideoPageVM objVM = objModel.GetData();
                return View(objVM);
            }
            else if (objModel.makeStatus == Entities.StatusCodes.RedirectPermanent || objModel.modelStatus == Entities.StatusCodes.RedirectPermanent)
            {
                if (objModel.makeStatus == Entities.StatusCodes.RedirectPermanent)
                {
                    return RedirectPermanent(Request.RawUrl.Replace(makeMaskingName, objModel.objMakeResponse.MaskingName));
                }
                else
                {
                    return RedirectPermanent(Request.RawUrl.Replace(makeMaskingName, objModel.objModelResponse.MaskingName));
                }
            }
            else
            {
                return Redirect(CommonOpn.AppPath + "pageNotFound.aspx");
            }
        }

        /// <summary>
        /// Created by  :   Aditi Srivastava on 27 Feb 2017
        /// Summary     :   Fetch videos page by masking name
        /// Modified by :   Sumit Kate on 29 Mar 2017
        /// Description :   Make wise video page desktop action method
        /// </summary>
        [Filters.DeviceDetection()]
        [Route("videos/make/{makeMaskingName}/")]
        public ActionResult Makes(string makeMaskingName)
        {
            MakeVideosPage objModel = new MakeVideosPage(makeMaskingName, _videos);
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
                return Redirect("pageNotFound.aspx");
            }
        }

        /// <summary>
        /// Created by : Aditi Srivastava on 27 Feb 2017
        /// Summary    : ActionResult method for make wise videos page
        /// Modified by :   Sumit Kate on 29 Mar 2017
        /// Description :   Make wise video page mobile action method
        /// </summary>
        /// <param name="makeMaskingName"></param>
        /// <returns></returns>
        [Route("m/videos/make/{makeMaskingname}/")]
        public ActionResult Makes_Mobile(string makeMaskingName)
        {
            MakeVideosPage objModel = new MakeVideosPage(makeMaskingName, _videos);
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
                return Redirect("pageNotFound.aspx");
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
                ErrorClass objErr = new ErrorClass(ex, "Bikewale.Controllers.Videos.ScooterVideos");

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

                ErrorClass objErr = new ErrorClass(ex, "Bikewale.Controllers.Videos.ScooterVideos_Mobile");

            }

            return View(objVideosList);
           

        }
    }
}