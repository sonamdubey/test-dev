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

        public VideosController(IVideosCacheRepository videos, IVideos video)
        {
            _videos = videos;
            _video = video;
        }

        /// <summary>
        /// Created by : Aditi Srivastava on 27 Feb 2017
        /// Summary    : Fetch videos page by masking name
        /// </summary>
        /// 
        [Route("videos/make/{makeMaskingName}/")]
        public ActionResult Index(string makeMaskingName)
        {
            IEnumerable<BikeVideoModelEntity> objModelVideos = null;
            MakeMaskingResponse makeInfo = null;
            makeInfo = new MakeHelper().GetMakeByMaskingName(makeMaskingName);
            if (makeInfo.StatusCode == 200)
            {
                ViewBag.CityId = GlobalCityArea.GetGlobalCityArea().CityId;
                ViewBag.TopCount = 9;
                ViewBag.MakeId = makeInfo.MakeId;
                ViewBag.makeMaskingName = makeMaskingName;
                ViewBag.MakeName = "";
                objModelVideos = _videos.GetModelVideos(makeInfo.MakeId);
                if (objModelVideos != null && objModelVideos.Count() > 0 && objModelVideos.FirstOrDefault().objMake != null)
                {
                    ViewBag.MakeName = objModelVideos.FirstOrDefault().objMake.MakeName;
                }
                ViewBag.Title = string.Format("{0} Bike Videos - BikeWale", ViewBag.MakeName);
                ViewBag.Description = string.Format("Check latest {0} bikes videos, watch BikeWale expert's take on {0} bikes - features, performance, price, fuel economy, handling and more.", ViewBag.MakeName);
                ViewBag.Keywords = string.Format("{0},{0} bikes,{0} videos", ViewBag.MakeName);
                ViewBag.canonical = string.Format("https://www.bikewale.com/{0}-bikes/videos/", ViewBag.makeMaskingName);
                ViewBag.alternate = string.Format("https://www.bikewale.com/m/{0}-bikes/videos/", ViewBag.makeMaskingName);
                ViewBag.Ad_300x250BTF = false;
                ViewBag.Ad_300x250 = false;
                return View("~/Views/Videos/Makes.cshtml", objModelVideos);
            }
            else if(makeInfo.StatusCode==301)
            {
                return Index(makeInfo.MaskingName);
            }
            else
            {
                return Redirect(CommonOpn.AppPath + "pageNotFound.aspx");
            }
        }

        /// <summary>
        /// Created by : Sangram Nandkhile on 01 Mar 2017
        /// Summary: Action method for Video details Page
        /// </summary>
        [Route("videos/details/{videoId}/")]
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
            ViewBag.CityId = GlobalCityArea.GetGlobalCityArea().CityId;

            ViewBag.TopCount = 9;
            return View("~/Views/Videos/Details.cshtml");
        }

        /// <summary>
        /// Created by : Sangram Nandkhile on 01 Mar 2017
        /// Summary: Partial view to show Similar model bike videos
        /// </summary>
        [Route("videos/SimilarVideos/")]
        public ActionResult SimilarVideos(uint videoId, uint modelId)
        {
            SimilarModelsModel similarVideosModel = new SimilarModelsModel();
            similarVideosModel.Videos = _video.GetSimilarModelsVideos(videoId,modelId, 9);
            similarVideosModel.ModelId = modelId;
            return PartialView("~/views/shared/_SimilarVideo.cshtml", similarVideosModel);
        }
    }
}