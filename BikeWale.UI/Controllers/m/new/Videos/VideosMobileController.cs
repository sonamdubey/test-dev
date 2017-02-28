using Bikewale.Common;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.Videos;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.Videos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Bikewale.Controllers.Mobile.Videos
{
    /// <summary>
    /// Created by : Aditi Srivastava on 27 Feb 2017
    /// Summary    : Mobile Controller for make video page
    /// </summary>
	public class VideosMobileController :Controller
	{
         private readonly IVideosCacheRepository _videos = null;
         private readonly IBikeModelsCacheRepository<int> _modelCache = null;


         public VideosMobileController(IVideosCacheRepository videos, IBikeModelsCacheRepository<int> modelCache)
        {
            _videos = videos;
            _modelCache = modelCache;
        }
        [Route("m/videos/make/{makeMaskingname}/")]
        public ActionResult Index(string makeMaskingName)
        {
            MakeMaskingResponse makeInfo = new MakeMaskingResponse();
            makeInfo = new MakeHelper().GetMakeByMaskingName(makeMaskingName);

            ViewBag.MakeName = "";
            IEnumerable<BikeVideoModelEntity> objModelVideos = _videos.GetModelVideos(makeInfo.MakeId);
            if (objModelVideos != null)
            {
                ViewBag.MakeName = objModelVideos.FirstOrDefault().objMake.MakeName;
            }
            int TotalItems = 9;
            IEnumerable<MostPopularBikesBase> objPopularBikes = _modelCache.GetMostPopularBikes(TotalItems,(int)makeInfo.MakeId);
            ViewBag.PopularBikes = objPopularBikes;
            return View("~/Views/m/Videos/Makes.cshtml", objModelVideos);
        }
	}
}