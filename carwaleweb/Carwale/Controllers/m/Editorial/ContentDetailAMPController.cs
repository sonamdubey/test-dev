using Carwale.DTOs.CMS;
using Carwale.Entity.CMS;
using Carwale.Entity.CMS.Articles;
using Carwale.Entity.CMS.URIs;
using Carwale.Interfaces.CMS.Articles;
using Carwale.Interfaces.PriceQuote;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Carwale.Utility;
using System.Text.RegularExpressions;
using Carwale.Notifications;
using Carwale.BL.GrpcFiles;
using Grpc.CMS;

namespace Carwale.UI.Controllers.m.Editorial
{
    public class ContentDetailAMPController : Controller
    {
        private readonly ICMSContent _cmsRepo;
        private readonly IPrices _versionPrices;

        public ContentDetailAMPController(ICMSContent cmsRepo, IPrices versionPrices)
        {
            _cmsRepo = cmsRepo;
            _versionPrices = versionPrices;
        }

        [Route("m/contentdetailsamp/{basicId}/amp"), Route("m/contentdetailsamp/{basicId:int}/{articleMaskingName}/amp")]
        public ActionResult Index(ulong basicId, string articleMaskingName = null)
        {
            articleMaskingName = articleMaskingName ?? string.Empty;
            string articleUrl = string.Format("/news/{0}", articleMaskingName);
            ulong articleBasicId = (ulong)GrpcToCarwaleConvert.ConvertFromGrpcToCarwale(GrpcMethods.GetBasicIdFromArticleUrl(articleUrl + (basicId > 0 ? "-" + basicId : string.Empty) + "/"));

            if (articleBasicId <= 0)
            {
                articleUrl = GrpcToCarwaleConvert.ConvertFromGrpcToCarwale(GrpcMethods.GetArticleUrlFromBasicId(basicId));
                return RedirectPermanent("/m" + articleUrl + "amp/");
            }
            else
                basicId = articleBasicId;

            var articleContentURI = new ArticleContentURI()
            {
                BasicId = basicId
            };

            var contentDetailPagesDTO = new ContentDetailAmpDTO()
            {
                Article = _cmsRepo.GetContentDetails(articleContentURI)
            };

            if (contentDetailPagesDTO.Article == null)
                return new HttpStatusCodeResult(500);

            if (contentDetailPagesDTO.Article.Content != null)
            {
                try
                {
                    contentDetailPagesDTO.Article.Content = contentDetailPagesDTO.Article.Content.ConvertToAmpContent();
                }
                catch (Exception err)
                {
                    ExceptionHandler objErr = new ExceptionHandler(err, "ContentDetailAMPController : Index : Convert Content to amp");
                    objErr.LogException();
                }

                SetViewBagDataForSEO(contentDetailPagesDTO.Article);
                var relatedNews = new List<ArticleSummary>();

                if (contentDetailPagesDTO.Article.TagsList.Count > 0)
                {
                    var articleRelatedURI = new ArticleRelatedURI()
                    {
                        ApplicationId = Convert.ToUInt16(CMSAppId.Carwale),
                        BasicId = (uint)basicId,
                        ContentTypes = "1",
                        TotalRecords = 6
                    };

                    List<ArticleSummary> relatedArticles = _cmsRepo.GetRelatedArticles(articleRelatedURI);

                    if (relatedArticles != null && relatedArticles.Count > 0)
                    {
                        relatedArticles.RemoveAll((item) => item.BasicId == basicId);

                        relatedArticles = relatedArticles.Take(articleRelatedURI.TotalRecords).ToList();

                        foreach (var news in relatedArticles)
                        {
                            try
                            {
                                news.Description = news.Description.ConvertToAmpContent();
                            }
                            catch (Exception err)
                            {
                                ExceptionHandler objErr = new ExceptionHandler(err, "ContentDetailAMPController : Index : Convert GetRelatedArticles to amp");
                                objErr.LogException();
                            }
                            relatedNews.Add(news);
                        }
                    }

                    contentDetailPagesDTO.RelatedArticles = relatedNews;
                }

                return View("~/views/m/amp/NewsDetail.cshtml", contentDetailPagesDTO);
            }

            return RedirectToAction("index", "news");
        }

        private void SetViewBagDataForSEO(ArticleDetails article)
        {
            ViewBag.NewsUrl = article.ArticleUrl;
            ViewBag.Title = article.Title;
            ViewBag.MainImageUrl = Carwale.Utility.ImageSizes.CreateImageUrl(article.HostUrl, Carwale.Utility.ImageSizes._566X318, article.OriginalImgUrl);
            ViewBag.Displaydate = article.DisplayDate.ToString("yyyy-MM-ddTHH:mm:sszzz");
            ViewBag.Author = article.AuthorName;
            ViewBag.Description = article.Description;
            ViewBag.BridgeStoneUrl = System.Configuration.ConfigurationManager.AppSettings["BridgestoneUrl"];
        }
    }
}