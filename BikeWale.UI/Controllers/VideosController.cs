
using Bikewale.Common;
using Bikewale.Entities.BikeData;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.Location;
using Bikewale.Interfaces.Videos;
using Bikewale.Models;
using Bikewale.Models.Videos;
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
        private readonly IBikeMakes<BikeMakeEntity, int> _bikeMakes = null;
        private readonly IBikeMaskingCacheRepository<BikeModelEntity, int> _objModelCache = null;
        private readonly IBikeMakesCacheRepository<int> _bikeMakesCache = null;
        private readonly ICityCacheRepository _cityCacheRepo = null;
        private readonly IBikeInfo _bikeInfo = null;

        public VideosController(ICityCacheRepository cityCacheRepo, IBikeInfo bikeInfo, IBikeMakesCacheRepository<int> bikeMakesCache, IVideosCacheRepository videos, IVideos video, IBikeMakes<BikeMakeEntity, int> bikeMakes, IBikeMaskingCacheRepository<BikeModelEntity, int> objModelCache)
        {
            _videos = videos;
            _video = video;
            _bikeMakes = bikeMakes;
            _objModelCache = objModelCache;
            _bikeMakesCache = bikeMakesCache;
            _cityCacheRepo = cityCacheRepo;
            _bikeInfo = bikeInfo;
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

                return View(objVM);
            }
            catch (System.Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "ServiceCentersController.Index_Mobile");
                return Redirect("/pagenotfound.aspx");
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
        /// </summary>
        /// <param name="categoryIdList"></param>
        /// <param name="title"></param>
        /// <returns></returns>
        [Route("m/videos/category/{categoryIdList}/title/{title}")]
        public ActionResult CategoryVideos_Mobile(string categoryIdList, string title)
        {
            VideosByCategoryPage objModel = new VideosByCategoryPage(_videos, categoryIdList, title);
            VideosByCategoryPageVM objVM = objModel.GetData(9);
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
            return PartialView("~/views/videos/_SimilarVideos.cshtml", new SimilarVideosModel(modelId, videoId, _video));
        }

        /// <summary>
        /// Created by : Aditi Srivastava 2 March 2017
        /// Summary    : Partial view for similar model videos 
        /// </summary>
        [Route("m/videos/SimilarVideos/")]
        public ActionResult SimilarVideos_Mobile(uint videoId, uint modelId)
        {
            return PartialView("~/views/videos/_SimilarVideos_Mobile.cshtml", new SimilarVideosModel(modelId, videoId, _video));
        }
    }
}