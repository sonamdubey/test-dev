using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Bikewale.BAL.EditCMS;
using Bikewale.Cache.CMS;
using Bikewale.Cache.Core;
using Bikewale.Entities.CMS.Articles;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.CMS;
using Bikewale.Interfaces.EditCMS;
using Bikewale.Notifications;
using Microsoft.Practices.Unity;
using Bikewale.Utility;

namespace Bikewale.Controllers.Mobile.Content.News
{    
    /// <summary>
    /// Created By : Ashish G. Kamble on 2 Jan 2017
    /// Controller related to news for the mobile website
    /// </summary>
    public class NewsController : Controller
    {
        private readonly ICMSCacheContent cache = null;

        /// <summary>
        /// Constructor to resolve all the dependencies
        /// </summary>
        /// <param name="_cache"></param>
        public NewsController(ICMSCacheContent _cache)
        {
            cache = _cache;
        }

        /// <summary>
        /// Action to get the news list page for the first page
        /// </summary>
        /// <returns></returns>
        [Route("m/news/")]
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Action to get the news list page for the given pageid
        /// </summary>
        /// <param name="pageId"></param>
        /// <returns></returns>
        [Route("m/news/page/{pageId}/")]
        public ActionResult Index(int pageId)
        {
            return View();
        }

        /// <summary>
        /// Action to get the news list page for the given pageid and for a given make
        /// </summary>
        /// <param name="pageId"></param>
        /// <param name="makeId"></param>
        /// <returns></returns>
        [Route("m/news/page/{pageId}/make/{makeId}/")]
        public ActionResult NewsListByMake(int pageId, int makeId)
        {
            return View();
        }

        /// <summary>
        /// Action to get the news list page for the given pageid and for a given model
        /// </summary>
        /// <param name="pageId"></param>
        /// <param name="makeId"></param>
        /// <returns></returns>
        [Route("m/news/page/{pageId}/model/{modelId}/")]
        public ActionResult NewsListByModel(int pageId, int modelId)
        {
            return View();
        }

        /// <summary>
        /// Action to get the news details
        /// </summary>
        /// <param name="basicid"></param>
        /// <returns></returns>
        [Route("m/news/details/{basicid}/")]
        public ActionResult Details(int basicid)
        {
            return View();
        }

        /// <summary>
        /// Action to get the map news details page
        /// </summary>
        /// <param name="basicid"></param>
        /// <returns></returns>
        [Route("m/news/details/{basicid}/amp/")]
        public ActionResult DetailsAMP(int basicid)
        {
            try
            {
                ArticleDetails objNews = cache.GetNewsDetails(Convert.ToUInt32(basicid));

                if (objNews != null && objNews.Content != null)
                {
                    ViewBag.Title = objNews.Title;
                    ViewBag.Author = objNews.AuthorName;
                    ViewBag.PublishedDate = objNews.DisplayDate.ToString();
                    ViewBag.LastModified = objNews.DisplayDate.ToString();
                    ViewBag.PageViews = objNews.Views;
                    ViewBag.Canonical = String.Format("{0}/news/{1}-{2}.html", Bikewale.Utility.BWConfiguration.Instance.BwHostUrl, basicid, objNews.ArticleUrl);
                    ViewBag.MobilePageUrl = String.Format("{0}/m/news/{1}-{2}.html", Bikewale.Utility.BWConfiguration.Instance.BwHostUrl, basicid, objNews.ArticleUrl);
                    ViewBag.MainImageUrl = Bikewale.Utility.Image.GetPathToShowImages(objNews.OriginalImgUrl, objNews.HostUrl, Bikewale.Utility.ImageSize._640x348);
                    ViewBag.Description = objNews.Description;

                    //Convert article content to the amp content
                    ViewBag.NewsContent = objNews.Content.ConvertToAmpContent();

                    if (!String.IsNullOrEmpty(objNews.NextArticle.ArticleUrl))
                    {
                        ViewBag.NextPageTitle = objNews.NextArticle.Title;
                        ViewBag.NextPageUrl = String.Format("{0}/m/news/{1}-{2}.html", Bikewale.Utility.BWConfiguration.Instance.BwHostUrl, objNews.NextArticle.BasicId, objNews.NextArticle.ArticleUrl);
                    }

                    if (!String.IsNullOrEmpty(objNews.PrevArticle.ArticleUrl))
                    {
                        ViewBag.PrevPageTitle = objNews.PrevArticle.Title;
                        ViewBag.PrevPageUrl = String.Format("{0}/m/news/{1}-{2}.html", Bikewale.Utility.BWConfiguration.Instance.BwHostUrl, objNews.PrevArticle.BasicId, objNews.PrevArticle.ArticleUrl);
                    }
                }                                    
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, "m/news/details/{basicid}/amp/" + basicid);                
            }

            return View("~/views/m/content/news/details_amp.cshtml");
        }

        /// <summary>
        /// Action to get the latest news
        /// </summary>
        /// <param name="count">no of news required</param>
        /// <returns></returns>
        [Route("m/news/latest/{count}/")]
        public ActionResult Latest(int count)
        {
            return PartialView();
        }

        /// <summary>
        /// Action to get the latst news for a given make
        /// </summary>
        /// <param name="makeId">make id for which news are required</param>
        /// <param name="count">no of news required</param>
        /// <returns></returns>
        [Route("m/news/latest/{count}/make/{makeId}/")]
        public ActionResult LatestNewsByMake(int makeId, int count)
        {
            return PartialView();
        }

        /// <summary>
        /// Action to get the latest news for a given model
        /// </summary>
        /// <param name="modelId">model id for which news are required</param>
        /// <param name="count"></param>
        /// <returns></returns>
        [Route("m/news/latest/{count}/model/{modelId}/")]
        public ActionResult LatestnewsByModel(int modelId, int count)
        {
            return PartialView();
        }
    }
}