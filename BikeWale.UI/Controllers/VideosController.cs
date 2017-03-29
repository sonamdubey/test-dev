using Bikewale.Common;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.Videos;
using Bikewale.Interfaces.Videos;
using Bikewale.Models;
using Bikewale.Models.Mobile.Videos;
using Bikewale.Utility;
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

        public VideosController(IVideosCacheRepository videos, IVideos video)
        {
            _videos = videos;
            _video = video;
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
        /// Created by : Aditi Srivastava on 27 Feb 2017
        /// Summary    : Fetch videos page by masking name
        /// </summary>
        [Route("videos/make/{makeMaskingName}/")]
        public ActionResult Makes(string makeMaskingName)
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
            else if (makeInfo.StatusCode == 301)
            {
                return RedirectPermanent(Request.RawUrl.Replace(makeMaskingName, makeInfo.MaskingName));
            }
            else
            {
                return Redirect(CommonOpn.AppPath + "pageNotFound.aspx");
            }
        }

        /// <summary>
        /// Created by : Aditi Srivastava on 27 Feb 2017
        /// Summary    : ActionResult method for make wise videos page
        /// </summary>
        /// <param name="makeMaskingName"></param>
        /// <returns></returns>
        [Route("m/videos/make/{makeMaskingname}/")]
        public ActionResult Makes_Mobile(string makeMaskingName)
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
                ViewBag.Ad_320x50 = true;
                ViewBag.Ad_Bot_320x50 = true;
                return View("~/Views/m/Videos/Makes.cshtml", objModelVideos);
            }
            else if (makeInfo.StatusCode == 301)
            {
                return RedirectPermanent(Request.RawUrl.Replace(makeMaskingName, makeInfo.MaskingName));
            }
            else
            {
                return Redirect(CommonOpn.AppPath + "pageNotFound.aspx");
            }
        }

        /// <summary>
        /// Created by : Sangram Nandkhile on 01 Mar 2017
        /// Summary: Partial view to show Similar model bike videos
        /// </summary>
        [Route("videos/SimilarVideos/")]
        public ActionResult SimilarVideos(uint videoId, uint modelId)
        {
            SimilarModelsModel similarVideosModel = new SimilarModelsModel();
            similarVideosModel.Videos = _video.GetSimilarModelsVideos(videoId, modelId, 9);
            similarVideosModel.ModelId = modelId;
            if (modelId > 0)
            {
                BikeModelEntity objModel = new ModelHelper().GetModelDataById(modelId);

                if (objModel != null)
                {
                    similarVideosModel.ViewAllLinkText = "View all";
                    similarVideosModel.ViewAllLinkUrl = String.Format("/{0}-bikes/{1}/videos/", objModel.MakeBase.MaskingName, objModel.MaskingName);
                    similarVideosModel.ViewAllLinkTitle = String.Format("{0} {1} Videos", objModel.MakeBase.MakeName, objModel.ModelName);
                }
            }
            return PartialView("~/views/shared/_SimilarVideo.cshtml", similarVideosModel);
        }

        /// <summary>
        /// Created by : Aditi Srivastava 2 March 2017
        /// Summary    : Partial view for similar model videos 
        /// </summary>
        [Route("m/videos/SimilarVideos/")]
        public ActionResult SimilarVideos_Mobile(uint videoId, uint modelId)
        {
            SimilarModelsModel similarVideosModel = new SimilarModelsModel();
            similarVideosModel.Videos = _video.GetSimilarModelsVideos(videoId, modelId, 9);
            similarVideosModel.ModelId = modelId;
            if (modelId > 0)
            {
                BikeModelEntity objModel = new ModelHelper().GetModelDataById(modelId);
                if (objModel != null)
                {
                    similarVideosModel.ViewAllLinkText = "View all";
                    similarVideosModel.ViewAllLinkUrl = string.Format("/m/{0}-bikes/{1}/videos/", objModel.MakeBase.MaskingName, objModel.MaskingName);
                    similarVideosModel.ViewAllLinkTitle = string.Format("{0} {1} Videos", objModel.MakeBase.MakeName, objModel.ModelName);
                }
            }
            return PartialView("~/views/m/shared/_SimilarVideo.cshtml", similarVideosModel);
        }
    }
}