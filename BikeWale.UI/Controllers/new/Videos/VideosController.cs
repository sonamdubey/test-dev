using Bikewale.Common;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.Videos;
using Bikewale.Interfaces.Videos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Bikewale.Controllers.Desktop.Videos
{
    public class VideosController : Controller
    {
        private readonly IVideosCacheRepository _videos = null;

        public VideosController(IVideosCacheRepository videos)
        {
            _videos = videos;
        }
        [Route("videos/make/{makeMaskingName}/")]
        public ActionResult Index(string makeMaskingName)
        {
            MakeMaskingResponse makeInfo = new MakeMaskingResponse();
            makeInfo = new MakeHelper().GetMakeByMaskingName(makeMaskingName);

            IEnumerable<BikeVideoModelEntity> objModelVideos = _videos.GetModelVideos(makeInfo.MakeId);

            return View("~/Views/Videos/Makes.cshtml", objModelVideos);
        }
    }
}