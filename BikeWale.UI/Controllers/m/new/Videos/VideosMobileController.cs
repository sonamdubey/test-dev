using Bikewale.BAL;
using Bikewale.Common;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.Videos;
using Bikewale.Interfaces.Videos;
using Bikewale.Models.Mobile.Videos;
using Bikewale.Utility;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Bikewale.Controllers.Mobile.Videos
{
    /// <summary>
    /// Created by : Aditi Srivastava on 27 Feb 2017
    /// Summary    : Mobile Controller for make specific video page
    /// </summary>
    public class VideosMobileController : Controller
    {
        private readonly IVideosCacheRepository _videos = null;
        private readonly IVideos _video = null;

        public VideosMobileController(IVideosCacheRepository videos, IVideos video)
        {
            _videos = videos;
            _video = video;
        }

        [Route("m/videos/make/{makeMaskingname}/")]
        public ActionResult Index(string makeMaskingName)
        {
            MakeMaskingResponse makeInfo = new MakeMaskingResponse();
            makeInfo = new MakeHelper().GetMakeByMaskingName(makeMaskingName);
            ViewBag.CityId = GlobalCityArea.GetGlobalCityArea().CityId;
            ViewBag.TopCount = 9;
            ViewBag.MakeId = makeInfo.MakeId;
            ViewBag.makeMaskingName = makeMaskingName;
            ViewBag.MakeName = "";
            IEnumerable<BikeVideoModelEntity> objModelVideos = _videos.GetModelVideos(makeInfo.MakeId);
            if (objModelVideos != null && objModelVideos.Count() > 0 && objModelVideos.FirstOrDefault().objMake != null)
            {
                ViewBag.MakeName = objModelVideos.FirstOrDefault().objMake.MakeName;
            }
            ViewBag.Title = string.Format("{0} Bike Videos - BikeWale", ViewBag.MakeName);
            ViewBag.Description = string.Format("Check latest {0} bikes videos, watch BikeWale expert's take on {0} bikes - features, performance, price, fuel economy, handling and more.", ViewBag.MakeName);
            ViewBag.Keywords = string.Format("{0},{0} bikes,{0} videos", ViewBag.MakeName);
            ViewBag.canonical = string.Format("https://www.bikewale.com/{0}-bikes/videos/", ViewBag.makeMaskingName);
            ViewBag.Ad_320x50 = true;
            ViewBag.Ad_Bot_320x50 = true;
            return View("~/Views/m/Videos/Makes.cshtml", objModelVideos);
        }
        [Route("m/videos/details/{videoId}/")]
        public ActionResult Index(uint videoId)
        {
            VideoDetailsHelper helper = new VideoDetailsHelper(videoId, _videos);
            var details = helper.GetDetails();
            ViewBag.DetailsPage = details;

            ViewBag.Description = details.PageMetas.Description;
            ViewBag.Title = details.PageMetas.Title;
            ViewBag.Keywords = details.PageMetas.Keywords;
            ViewBag.canonical = details.PageMetas.CanonicalUrl;
            ViewBag.alternate = details.PageMetas.AlternateUrl;

            ModelMaskingResponse modelInfo = new ModelMaskingResponse();
            modelInfo = new ModelHelper().GetModelDataByMasking(details.VideoEntity.MaskingName);
            ViewBag.ModelId = modelInfo.ModelId;
            if (GlobalCityArea.GetGlobalCityArea().CityId != 0)
                ViewBag.CityId = GlobalCityArea.GetGlobalCityArea().CityId;
            else
                ViewBag.CityId = Configuration.GetDefaultCityId;
            ViewBag.TopCount = 9;
            return View("~/Views/m/Videos/Details.cshtml");
        }

        /// <summary>
        /// Created by : Aditi Srivastava 2 March 2017
        /// Summary    : Partial view for similar model videos 
        /// </summary>
        [Route("m/videos/SimilarVideos/")]
        public ActionResult SimilarVideos(uint videoId, uint modelId)
        {
            SimilarModelsModel similarVideosModel = new SimilarModelsModel();
            similarVideosModel.Videos = _video.GetSimilarModelsVideos(videoId, 9);
            similarVideosModel.ModelId = modelId;
            return PartialView("~/views/m/shared/_SimilarVideo.cshtml", similarVideosModel);
        }
    }
}