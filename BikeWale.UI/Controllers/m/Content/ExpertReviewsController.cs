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
    /// Summary : Contoller have functions related to the expert reviews for mobile site
    /// </summary>
    public class ExpertReviewsController : Controller
    {
        private readonly ICMSCacheContent cache = null;

        /// <summary>
        /// Constructor to resolve all the dependencies
        /// </summary>
        /// <param name="_cache"></param>
        public ExpertReviewsController(ICMSCacheContent _cache)
        {
            cache = _cache;
        }

        /// <summary>
        /// Action to get the expertreviews list page for the first page
        /// </summary>
        /// <returns></returns>
        [Route("m/expertreviews/")]
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Action to get the expertreviews list page for the given pageid
        /// </summary>
        /// <param name="pageId"></param>
        /// <returns></returns>
        [Route("m/expertreviews/page/{pageId}/")]
        public ActionResult Index(int pageId)
        {
            return View();
        }

        /// <summary>
        /// Action to get the expertreviews list page for the given pageid and for a given make
        /// </summary>
        /// <param name="pageId"></param>
        /// <param name="makeId"></param>
        /// <returns></returns>
        [Route("m/expertreviews/make/{makeId}/page/{pageId}/")]
        public ActionResult ExpertReviewsListByMake(int pageId, int makeId)
        {
            return View();
        }

        /// <summary>
        /// Action to get the expertreviews list page for the given pageid and for a given model
        /// </summary>
        /// <param name="pageId"></param>
        /// <param name="makeId"></param>
        /// <returns></returns>
        [Route("m/expertreviews/model/{modelId}/page/{pageId}/")]
        public ActionResult ExpertReviewsListByModel(int pageId, int modelId)
        {
            return View();
        }

        /// <summary>
        /// Action to get the expertreviews details
        /// </summary>
        /// <param name="basicid"></param>
        /// <returns></returns>
        [Route("m/expertreviews/details/{basicid}/")]
        public ActionResult Details(uint basicid)
        {
            return View();
        }

        /// <summary>
        /// Action to get the map expertreviews details page
        /// </summary>
        /// <param name="basicid"></param>
        /// <returns></returns>
        [Route("m/expertreviews/details/{basicid}/amp/")]
        public ActionResult DetailsAMP(uint basicid)
        {
            ArticlePageDetails objExpertReviews = null;

            try
            {
                objExpertReviews = cache.GetArticlesDetails(basicid);

                if (objExpertReviews != null)
                {
                    // set all metatags in the variables
                    ViewBag.Description = objExpertReviews.Description.StripHtml();
                    ViewBag.Canonical = String.Format("{0}/expert-reviews/{1}-{2}.html", Bikewale.Utility.BWConfiguration.Instance.BwHostUrl,objExpertReviews.ArticleUrl, basicid);
                    ViewBag.MobilePageUrl = String.Format("{0}/m/expert-reviews/{1}-{2}.html", Bikewale.Utility.BWConfiguration.Instance.BwHostUrl, objExpertReviews.ArticleUrl, basicid);
                    ViewBag.ArticleSectionTitle = " - BikeWale Expert Reviews";
                    ViewBag.ArticleType = "NewsArticle";
                    ViewBag.Title = objExpertReviews.Title;
                    ViewBag.MainImageUrl = Bikewale.Utility.Image.GetPathToShowImages(objExpertReviews.OriginalImgUrl, objExpertReviews.HostUrl, Bikewale.Utility.ImageSize._640x348);
                    ViewBag.PublishedDate = objExpertReviews.DisplayDate.ToString();
                    ViewBag.LastModified = objExpertReviews.DisplayDate.ToString();
                    ViewBag.PageViews = objExpertReviews.Views;
                    ViewBag.Author = objExpertReviews.AuthorName;
                    ViewBag.VehicleTagsCnt = 0;

                    // code to get the bikes tagged in the article
                    if (objExpertReviews.VehiclTagsList != null)
                    {
                        ViewBag.VehicleTagsList = objExpertReviews.VehiclTagsList.GroupBy(s => s.ModelBase.ModelId).Select(grp => grp.First());
                        ViewBag.VehicleTagsCnt = objExpertReviews.VehiclTagsList.Count();
                    }

                    // code to get article photos
                    IEnumerable<ModelImage> objImg = cache.GetArticlePhotos(Convert.ToInt32(basicid));
                    ViewBag.PhotosCnt = 0;

                    if (objImg != null)
                    {
                        ViewBag.Photos = objImg;
                        ViewBag.PhotosCnt = objImg.Count();
                    }
                }
            }
            catch (Exception ex)
            {

                ErrorClass objErr = new ErrorClass(ex, "m/expertreviews/details/{basicid}/amp/" + basicid);
            }

            return View("~/views/m/content/expertreviews/details_amp.cshtml", objExpertReviews);
        }

        /// <summary>
        /// Action to get the latest expertreviews
        /// </summary>
        /// <param name="count">no of expertreviews required</param>
        /// <returns></returns>
        [Route("m/expertreviews/latest/{count}/")]
        public ActionResult Latest(int count)
        {
            return PartialView();
        }

        /// <summary>
        /// Action to get the latst expertreviews for a given make
        /// </summary>
        /// <param name="makeId">make id for which expertreviews are required</param>
        /// <param name="count">no of expertreviews required</param>
        /// <returns></returns>
        [Route("m/expertreviews/make/{makeId}/latest/{count}/")]
        public ActionResult LatestExpertReviewsByMake(int makeId, int count)
        {
            return PartialView();
        }

        /// <summary>
        /// Action to get the latest expertreviews for a given model
        /// </summary>
        /// <param name="modelId">model id for which expertreviews are required</param>
        /// <param name="count"></param>
        /// <returns></returns>
        [Route("m/expertreviews/model/{modelId}/latest/{count}/")]
        public ActionResult LatestExpertReviewsByModel(int modelId, int count)
        {
            return PartialView();
        }
    }
}