using Bikewale.BAL;
using Bikewale.Common;
using Bikewale.Entities.BikeData;
using Bikewale.Interfaces.Videos;
using System.Web.Mvc;

namespace Bikewale.Controllers.Desktop.Videos
{
    public class VideosController : Controller
    {
        private readonly IVideosCacheRepository _videos = null;
        private readonly IVideos _video = null;


        public VideosController(IVideosCacheRepository videos, IVideos video)
        {
            _videos = videos;
            _video = video;
        }
        [Route("videos/make/{makeMaskingName}/")]
        public ActionResult Index(string makeMaskingName)
        {
            MakeMaskingResponse makeInfo = new MakeMaskingResponse();
            makeInfo = new MakeHelper().GetMakeByMaskingName(makeMaskingName);
            //IEnumerable<BikeVideoModelEntity> objModelVideos = _videos.GetModelVideos(makeInfo.MakeId);
            return View("~/Views/Videos/Makes.cshtml", null);
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

            return View("~/Views/Videos/Details.cshtml");
        }

        /// <summary>
        /// Created by : Sangram Nandkhile on 01 Mar 2017
        /// Summary: Partial view to show Similar model bike videos
        /// </summary>
        [Route("videos/SimilarVideos/")]
        public ActionResult SimilarVideos(uint videoId)
        {
            var similarModelsVideos = _video.GetSimilarModelsVideos(videoId, 9);
            return PartialView("~/views/shared/_SimilarVideo.cshtml", similarModelsVideos);
        }
    }
}


