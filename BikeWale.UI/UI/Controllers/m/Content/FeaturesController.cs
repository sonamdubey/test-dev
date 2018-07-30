using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Bikewale.Entities.CMS.Articles;
using Bikewale.Entities.CMS.Photos;
using Bikewale.Interfaces.CMS;
using Bikewale.Notifications;

namespace Bikewale.Controllers.Mobile.Content
{
    /// <summary>
    /// Created By : Ashish G. Kamble on 19 Jan 2017
    /// Summary : Contoller have functions related to the features for mobile site
    /// </summary>
    public class FeaturesController : Controller
    {
        private readonly ICMSCacheContent cache = null;

        /// <summary>
        /// Constructor to resolve all the dependencies
        /// </summary>
        /// <param name="_cache"></param>
        public FeaturesController(ICMSCacheContent _cache)
        {
            cache = _cache;
        }

        /// <summary>
        /// Action to get the features list page for the first page
        /// </summary>
        /// <returns></returns>
        [Route("m/features/")]
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Action to get the features list page for the given pageid
        /// </summary>
        /// <param name="pageId"></param>
        /// <returns></returns>
        [Route("m/features/page/{pageId}/")]
        public ActionResult Index(int pageId)
        {
            return View();
        }

        /// <summary>
        /// Action to get the features list page for the given pageid and for a given make
        /// </summary>
        /// <param name="pageId"></param>
        /// <param name="makeId"></param>
        /// <returns></returns>
        [Route("m/features/make/{makeId}/page/{pageId}/")]
        public ActionResult featuresListByMake(int pageId, int makeId)
        {
            return View();
        }

        /// <summary>
        /// Action to get the features list page for the given pageid and for a given model
        /// </summary>
        /// <param name="pageId"></param>
        /// <param name="makeId"></param>
        /// <returns></returns>
        [Route("m/features/model/{modelId}/page/{pageId}/")]
        public ActionResult featuresListByModel(int pageId, int modelId)
        {
            return View();
        }

        /// <summary>
        /// Action to get the features details
        /// </summary>
        /// <param name="basicid"></param>
        /// <returns></returns>
        [Route("m/features/details/{basicid}/")]
        public ActionResult Details(int basicid)
        {
            return View();
        }

        /// <summary>
        /// Action to get the latest features
        /// </summary>
        /// <param name="count">no of features required</param>
        /// <returns></returns>
        [Route("m/features/latest/{count}/")]
        public ActionResult Latest(int count)
        {
            return PartialView();
        }

        /// <summary>
        /// Action to get the latst features for a given make
        /// </summary>
        /// <param name="makeId">make id for which features are required</param>
        /// <param name="count">no of features required</param>
        /// <returns></returns>
        [Route("m/features/make/{makeId}/latest/{count}/")]
        public ActionResult LatestfeaturesByMake(int makeId, int count)
        {
            return PartialView();
        }

        /// <summary>
        /// Action to get the latest features for a given model
        /// </summary>
        /// <param name="modelId">model id for which features are required</param>
        /// <param name="count"></param>
        /// <returns></returns>
        [Route("m/features/model/{modelId}/latest/{count}/")]
        public ActionResult LatestfeaturesByModel(int modelId, int count)
        {
            return PartialView();
        }
    }
}