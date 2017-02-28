using Bikewale.Common;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.Videos;
using Bikewale.Interfaces.Videos;
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
	public class VideosMobileController :Controller
	{
         private readonly IVideosCacheRepository _videos = null;

         public VideosMobileController(IVideosCacheRepository videos)
        {
            _videos = videos;
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
	}
}