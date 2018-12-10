using Carwale.DTOs.CMS;
using Carwale.Entity;
using Carwale.Entity.CMS;
using Carwale.Interfaces.CMS.Articles;
using Carwale.Interfaces.NewCars;
using Carwale.Notifications;
using Carwale.UI.Filters;
using Carwale.Utility;
using log4net;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.SessionState;

namespace Carwale.UI.Controllers.Editorial
{
    [SessionState(SessionStateBehavior.Disabled)]
    public class NewsDetailsController : Controller
    {
        private readonly IUnityContainer _container;
        private readonly ICMSContent _cmsCacheRepo;
        private static readonly ILog _logger = LogManager.GetLogger(typeof(NewsDetailsController));

        public NewsDetailsController(IUnityContainer container, ICMSContent cmsContentCacheRepo)
        {
            _container = container;
            _cmsCacheRepo = cmsContentCacheRepo;
        }


        [DeviceDetectionFilter, Route("news/details/{id:int:min(1)}"), Route("news/details/{id:int}/{articleMaskingName}")]
        public ActionResult Index(ulong id, string articleMaskingName = null)
        {
            ContentDetailPagesDTO_V2 newsDetailModel = null;
            try
            {

                _container.RegisterInstance<ulong>(id);
                _container.RegisterInstance<string>(string.Format("{0}", (int)CMSContentType.News));
                
                articleMaskingName = articleMaskingName ?? string.Empty;
                string articleUrl = string.Format("/news/{0}", articleMaskingName);

                IServiceAdapterV2 newsDetailAdapter = _container.Resolve<IServiceAdapterV2>("NewsDetails");

                newsDetailModel = newsDetailAdapter.Get<ContentDetailPagesDTO_V2, string>(articleUrl);

                if (newsDetailModel == null)
                {
                    return new HttpStatusCodeResult(500);
                }

                if (newsDetailModel.ArticlePages != null)
                {
                    if (newsDetailModel.IsRedirect)
                    {
                        return RedirectPermanent(newsDetailModel.ArticlePages.ArticleUrl);
                    }

                    ViewBag.Tags = string.Join(",", newsDetailModel.ArticlePages.TagsList.ToArray());
                    ViewBag.Description = SanitizeHTML.RemoveAllHtmlTags(newsDetailModel.ArticlePages.Description);
                    newsDetailModel.BreadcrumbEntitylist = BindBreadCrumb(newsDetailModel.ArticlePages.Title);
                    newsDetailModel.ArticlePages.Description = Format.GetPlainTextFromHTML(newsDetailModel.ArticlePages.Description);
                    if (newsDetailModel.ArticlePages.PageList.IsNotNullOrEmpty())
                        newsDetailModel.ArticleBodyText = Format.GetPlainTextFromHTML(newsDetailModel.ArticlePages.PageList[0].Content);
                    return View("~/Views/Editorial/NewsDetails.cshtml", newsDetailModel);

                }
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "NewsDetailsController.Index()\n Exception : " + ex.Message);
                objErr.LogException();
            }
            return RedirectPermanent("/news/");
        }
        private List<Carwale.Entity.BreadcrumbEntity> BindBreadCrumb(string text)
        {
            List<Carwale.Entity.BreadcrumbEntity> _BreadcrumbEntitylist = new List<Carwale.Entity.BreadcrumbEntity>();
            _BreadcrumbEntitylist.Add(new Entity.BreadcrumbEntity { Title = "Indian Car News", Link = "/news/", Text = "News" });
            _BreadcrumbEntitylist.Add(new Entity.BreadcrumbEntity { Text = text });
            return _BreadcrumbEntitylist;
        }
    }
}