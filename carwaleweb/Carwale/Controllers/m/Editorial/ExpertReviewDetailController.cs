using Carwale.BL.CMS;
using Carwale.DTOs.CMS;
using Carwale.Entity;
using Carwale.Entity.CMS;
using Carwale.Entity.CMS.Articles;
using Carwale.Entity.CMS.URIs;
using Carwale.Interfaces.CMS.Articles;
using Carwale.Interfaces.NewCars;
using Carwale.Notifications;
using Carwale.Utility;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.SessionState;

namespace Carwale.UI.Controllers.m.Editorial
{
    [SessionState(SessionStateBehavior.Disabled)]
    public class ExpertReviewDetailController : Controller
    {
        private readonly IUnityContainer _container;

        public ExpertReviewDetailController(IUnityContainer container)
        {
            _container = container;
        }

        [Route("m/expertreviewdetails/{basicId}"), Route("m/expertreviewdetails/{basicId}/{articleMaskingName}"), Route("m/expertreviewdetails/{basicId}/{makeName}/{maskingName}"), Route("m/expertreviewdetails/{basicId}/{articleMaskingName}/{makeName}/{maskingName}")]
        public ActionResult Index(ulong basicId, bool isRelatedArticle = false, string articleMaskingName = null, string makeName = null, string maskingName = null)
        {
            ViewBag.isRelatedArticle = isRelatedArticle;
            try
            {
                _container.RegisterInstance<ulong>(basicId);
                _container.RegisterInstance<string>(string.Format("{0},{1}", (int)CMSContentType.ComparisonTests, (int)CMSContentType.RoadTest));
                _container.RegisterInstance<bool>(true);

                articleMaskingName = articleMaskingName ?? string.Empty;
                string articleUrl = string.Empty;

                if (!string.IsNullOrEmpty(makeName) && !string.IsNullOrEmpty(maskingName))
                {
                    if (!string.IsNullOrEmpty(articleMaskingName))
                        articleUrl = string.Format("/{0}-cars/{1}/expert-reviews/{2}", makeName, maskingName, articleMaskingName);
                    else
                        articleUrl = string.Format("/{0}-cars/{1}/expert-reviews", makeName, maskingName);
                }
                else
                    articleUrl = string.Format("/expert-reviews/{0}", articleMaskingName);

                IServiceAdapterV2 expertReviewAdapter = _container.Resolve<IServiceAdapterV2>("ExpertReviewDetailsMobile");
                var expertReviewModel = expertReviewAdapter.Get<ContentDetailPagesDTO_V1, Tuple<string, bool>>(new Tuple<string, bool>(articleUrl, isRelatedArticle));
                if (expertReviewModel.ArticlePages == null) {
                    return new HttpStatusCodeResult(500);
                }

                if (expertReviewModel.IsRedirect)
                {
                    return RedirectPermanent("/m" + expertReviewModel.ArticlePages.ArticleUrl);
                }

                expertReviewModel.ArticlePages.Description = Format.GetPlainTextFromHTML(expertReviewModel.ArticlePages.Description);
                if (expertReviewModel.ArticlePages != null)
                {
                    if (isRelatedArticle)
                        return PartialView("~/views/shared/m/editorial/_ContentDetailExpertReview.cshtml", expertReviewModel);
                    else
                        return View("~/views/m/editorial/expertreviewdetail.cshtml", expertReviewModel);
                }
            }
            catch (Exception ex) {
                ExceptionHandler objErr = new ExceptionHandler(ex, "ExpertReviewDetailController.Index()\n Exception : " + ex.Message);
                objErr.LogException();               
            }
            return Redirect("/m/expert-reviews/");
        }

        [Route("m/emptyResult")]
        public string EmptyResult()
        {
            return string.Empty;
        }
    }
}