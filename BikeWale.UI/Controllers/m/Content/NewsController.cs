using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Bikewale.BAL.EditCMS;
using Bikewale.Cache.CMS;
using Bikewale.Cache.Core;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.CMS;
using Bikewale.Interfaces.EditCMS;
using Bikewale.Notifications;
using Microsoft.Practices.Unity;
using Bikewale.Utility;
using Bikewale.Entities.CMS.Articles;

namespace Bikewale.Controllers.Mobile.Content
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
        [Route("m/news/make/{makeId}/page/{pageId}/")]
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
        [Route("m/news/model/{modelId}/page/{pageId}/")]
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
        [Route("m/news/make/{makeId}/latest/{count}/")]
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
        [Route("m/news/model/{modelId}/latest/{count}/")]
        public ActionResult LatestnewsByModel(int modelId, int count)
        {
            return PartialView();
        }
    }
}