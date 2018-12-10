using AutoMapper;
using Carwale.DTOs.CMS;
using Carwale.DTOs.CMS.Articles;
using Carwale.Entity.CMS.URIs;
using Carwale.Interfaces.CMS.Articles;
using Carwale.Notifications;
using Carwale.Notifications.Logs;
using Carwale.Service;
using Carwale.UI.Filters;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Carwale.UI.Controllers.Editorial
{
    public class RelatedArticleController : Controller
    {
        private readonly IUnityContainer _container;
        private readonly ICMSContent _cmsCacheRepo;

        public RelatedArticleController(IUnityContainer container, ICMSContent cmsContentCacheRepo)
        {
            _container = container;
            _cmsCacheRepo = cmsContentCacheRepo;
        }

        [DeviceDetectionFilter, Route("relatedArticle/details")]
        public ActionResult Index(int basicId)
        {
            try
            {
                var articleURI = new ArticleContentURI()
                {
                    BasicId = (ulong)basicId
                };

                var content = Mapper.Map<Carwale.Entity.CMS.Articles.ArticlePageDetails, ArticlePageDetails>(_cmsCacheRepo.GetContentPages(articleURI));

                if (content == null)
                {
                    ModelState.AddModelError("", "Item not found");
                    return View(String.Empty);
                }
                content.BasicId = (ulong)basicId;
                return PartialView("~/Views/Shared/Editorial/RelatedArticle.cshtml", content);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            ModelState.AddModelError("", "Item not found");
            return View(String.Empty);
        }
    }
}