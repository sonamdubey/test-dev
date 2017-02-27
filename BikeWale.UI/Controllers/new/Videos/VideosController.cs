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
        /// <summary>
        /// Created by : Aditi Srivastava on 27 Feb 2017
        /// Summary    : Desktop Controller for make video page
        /// </summary>
        public class VideosMobileController : Controller
        {
            private readonly IVideosCacheRepository _videos = null;

            public VideosMobileController(IVideosCacheRepository videos)
            {
                _videos = videos;
            }
            [Route("videos/make/{maskingname}/")]
            public ActionResult Index(string makeMaskingName)
            {
                MakeMaskingResponse makeInfo = new MakeMaskingResponse();
                makeInfo = new MakeHelper().GetMakeByMaskingName(makeMaskingName);

                IEnumerable<BikeVideoModelEntity> objModelVideos = _videos.GetModelVideos(makeInfo.MakeId);

                return View("~/Views/Videos/Makes.cshtml", objModelVideos);
            }
        }
	}
}