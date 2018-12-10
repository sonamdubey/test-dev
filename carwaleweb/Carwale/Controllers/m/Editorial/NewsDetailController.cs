using AutoMapper;
using Carwale.BL.CMS;
using Carwale.BL.GrpcFiles;
using Carwale.DTOs.CMS;
using Carwale.Entity.CMS;
using Carwale.Entity.CMS.Articles;
using Carwale.Entity.CMS.URIs;
using Carwale.Interfaces.CMS.Articles;
using Carwale.Interfaces.PriceQuote;
using Carwale.Notifications;
using Grpc.CMS;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.SessionState;

namespace Carwale.UI.Controllers.m.Editorial
{
    [SessionState(SessionStateBehavior.Disabled)]
    public class NewsDetailController : Controller
    {
        private readonly ICMSContent _cmsRepo;
        private readonly IPrices _versionPrices;

        public NewsDetailController(ICMSContent cmsRepo, IPrices versionPrices)
        {
            _cmsRepo = cmsRepo;
            _versionPrices = versionPrices;
        }

        [Route("m/newsdetail/{basicId}"), Route("m/newsdetail/{basicId:int}/{articleMaskingName}")]
        public ActionResult Index(ulong basicId, bool isRelatedArticle = false, string articleMaskingName = null)
        {
            try
            {
                ViewBag.isRelatedArticle = isRelatedArticle;
               
                if (!isRelatedArticle)
                {
                    
                    articleMaskingName = articleMaskingName ?? string.Empty;
                    string articleUrl = string.Format("/news/{0}", articleMaskingName);
                    ulong articleBasicId = (ulong)GrpcToCarwaleConvert.ConvertFromGrpcToCarwale(GrpcMethods.GetBasicIdFromArticleUrl(articleUrl + (basicId > 0 ? "-" + basicId : string.Empty) + "/"));

                    if (articleBasicId <= 0)
                    {
                        articleUrl = GrpcToCarwaleConvert.ConvertFromGrpcToCarwale(GrpcMethods.GetArticleUrlFromBasicId(basicId));
                        return RedirectPermanent("/m" + articleUrl);
                    }
                    else
                        basicId = articleBasicId;
                }

                var articleContentURI = new ArticleContentURI()
                {
                    BasicId = basicId
                };
                var contentDetailPagesDTO = new ContentDetailDTO()
                {
                    Article = _cmsRepo.GetContentDetails(articleContentURI),
                    RelatedArticles = Mapper.Map<List<Carwale.Entity.CMS.Articles.RelatedArticles>, List<Carwale.DTOs.CMS.Articles.RelatedArticlesDTO>>(
                                                                                                   _cmsRepo.GetRelatedContent(Convert.ToInt32(basicId)))
                };
                if (contentDetailPagesDTO.Article == null)
                {
                    return new HttpStatusCodeResult(500);
                }

                contentDetailPagesDTO.DescriptionText = GetPlainTextFromHTML(contentDetailPagesDTO.Article.Description);
                if (contentDetailPagesDTO.Article.Content != null)
                {
                    if (isRelatedArticle)
                    {
                        return PartialView("~/views/shared/m/editorial/_ContentDetail.cshtml", contentDetailPagesDTO);
                    }
                    else
                    {
                        contentDetailPagesDTO.ArticleBodyText = GetPlainTextFromHTML(contentDetailPagesDTO.Article.Content);
                        return View("~/views/m/editorial/newsdetail.cshtml", contentDetailPagesDTO);
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "msite NewsDetailsController.Index()\n Exception : " + ex.Message);
                objErr.LogException();
            }
            return RedirectPermanent("/m/news/");
        }

        private string GetPlainTextFromHTML(string html)
        {
            if (string.IsNullOrWhiteSpace(html))
                return string.Empty;
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);
            string text = Regex.Replace(doc.DocumentNode.InnerText, @"\s+", " ").Trim().Replace("&nbsp;", " ");
            return HttpUtility.HtmlDecode(text);
        }

        /// <summary>
        /// author : sachin bharti on 25/11/15
        /// purpose : get distinct tagged models
        /// </summary>
        /// <param name="vehiclTagsList"></param>
        private void FindTagModels(List<VehicleTag> vehiclTagsList)
        {
            var modelIds = CMSCommon.GetDistinctModels(vehiclTagsList);

            //if a single model tagged only then get on road price
            if (modelIds.Count() == 1)
            {
                ViewBag.ModelId = modelIds.First();
                GetOnRoadPrice(ViewBag.ModelId);
            }
        }

        /// <summary>
        /// author : sachin bharti on 25/11/15
        /// </summary>
        /// <param name="modelId"></param>
        private void GetOnRoadPrice(int modelId)
        {
            var modelPrice = _versionPrices.GetOnRoadPrice(modelId, int.Parse(System.Configuration.ConfigurationManager.AppSettings["DefaultCityId"]));
            ViewBag.CarPrice = modelPrice != null ? (Carwale.UI.Common.FormatPrice.FormatFullPrice(modelPrice.MinPrice.ToString() == "0" ? "" : modelPrice.MinPrice.ToString())) : null;
        }
    }
}