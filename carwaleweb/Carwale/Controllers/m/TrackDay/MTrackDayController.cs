using AutoMapper;
using Carwale.DTOs.CMS;
using Carwale.Entity.CMS.URIs;
using Carwale.Interfaces.CMS.Articles;
using Carwale.Interfaces.CMS.Photos;
using Carwale.UI.ClientBL;
using Carwale.UI.Common;
using Carwale.UI.Filters;
using Carwale.Utility;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Mvc;

namespace Carwale.UI.Controllers.m.TrackDay
{
    public class TrackDayController : Controller
    {
        private readonly ICMSContent _cmsCacheRepo;
        private readonly IPhotos _cmsPhotosCacheRepo;

        public TrackDayController(ICMSContent cmsRepo, IPhotos cmsPhotosCacheRepo)
        {
            _cmsCacheRepo = cmsRepo;
            _cmsPhotosCacheRepo = cmsPhotosCacheRepo;
        }

        [Route("m/trackday2016/")]
        public ActionResult Index(bool isApp = false)
        {
            if (DeviceDetectionManager.IsMobile(this.HttpContext))
            {
                ViewBag.isMobile = true;
            }
            ViewBag.IsApp = isApp;
            return View("~/Views/m/TrackDay/MTrackDayLandingPage.cshtml");
        }
        [Route("m/trackday2016/details/{basicId}")]
        public ActionResult TrackDayDetails(ulong basicId, bool isApp = false)
        {
            if (DeviceDetectionManager.IsMobile(this.HttpContext))
            {
                ViewBag.isMobile = true;
            }
            ViewBag.IsApp = isApp;

            var articleContentURI = new ArticleContentURI()
            {
                BasicId = basicId
            };
            var articlePhotoURI = new ArticlePhotoUri()
            {
                basicId = basicId
            };
            var articleByCatURI = new ArticleByCatURI()
            {
                ApplicationId = 1,
                CategoryIdList = "23",
                StartIndex = 1,
                EndIndex = 10
            };

            var contentDetailPagesDTO = new ContentDetailDTO()
            {
                Article = _cmsCacheRepo.GetContentDetails(articleContentURI),
            };

            if (contentDetailPagesDTO.Article == null)
                return new HttpStatusCodeResult(500);

            ViewBag.ArticleImages = _cmsPhotosCacheRepo.GetArticlePhotos(articlePhotoURI);
            ViewBag.RelatedArticles = _cmsCacheRepo.GetContentListByCategory(articleByCatURI);

            if (contentDetailPagesDTO.Article.Content != null)
            {
                return View("~/Views/m/TrackDay/MTrackDayDetailsPage.cshtml", contentDetailPagesDTO);
            }

            return Redirect("/m/trackday/");

            
        }

        
    }
}