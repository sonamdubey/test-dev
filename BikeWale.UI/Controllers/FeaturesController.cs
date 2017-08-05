using Bikewale.Common;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.CMS.Articles;
using Bikewale.Entities.CMS.Photos;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.BikeData.UpComing;
using Bikewale.Interfaces.CMS;
using Bikewale.Interfaces.Pager;
using Bikewale.Models;
using Bikewale.Models.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Bikewale.Controllers
{
    public class FeaturesController : Controller
    {
        private readonly ICMSCacheContent _Cache = null;
        private readonly IPager _objPager = null;
        private readonly IUpcoming _upcoming = null;
        private readonly IBikeModels<BikeModelEntity, int> _bikeModels = null;
        private readonly IBikeModelsCacheRepository<int> _models = null;

        public FeaturesController(ICMSCacheContent Cache, IPager objPager, IUpcoming upcoming, IBikeModels<BikeModelEntity, int> bikeModels, IBikeModelsCacheRepository<int> models)
        {
            _Cache = Cache;
            _objPager = objPager;
            _upcoming = upcoming;
            _bikeModels = bikeModels;
            _models = models;
        }

        /// <summary>
        /// Created by :- Subodh Jain on 31 March 2017
        /// Summary :- Index Method for Features news section
        /// </summary>
        /// <returns></returns>
        [Route("features/")]
        [Filters.DeviceDetection()]
        public ActionResult Index()
        {
            IndexPage obj = new IndexPage(_Cache, _objPager, _upcoming, _bikeModels);
            if (obj.status == Entities.StatusCodes.ContentNotFound)
            {
                return Redirect("/pagenotfound.aspx");
            }
            else
            {
                IndexFeatureVM objData = new IndexFeatureVM();
                objData = obj.GetData(4);
                if (obj.status == Entities.StatusCodes.ContentFound)
                    return View(objData);
                else
                    return Redirect("/pageNotFound.aspx");
            }
        }
        /// <summary>
        /// Created by :- Subodh Jain on 31 March 2017
        /// Summary :- Index Method for Features news section Mobile
        /// </summary>
        /// <returns></returns>
        [Route("m/features/")]
        public ActionResult Index_Mobile()
        {
            IndexPage obj = new IndexPage(_Cache, _objPager, _upcoming, _bikeModels);
            obj.IsMobile = true;
            if (obj.status == Entities.StatusCodes.ContentNotFound)
            {
                return Redirect("/m/pagenotfound.aspx");
            }
            else
            {
                IndexFeatureVM objData = new IndexFeatureVM();
                objData = obj.GetData(9);
                if (obj.status == Entities.StatusCodes.ContentFound)
                    return View(objData);
                else
                    return Redirect("/m/pageNotFound.aspx");
            }
        }

        /// <summary>
        ///Created By:- Subodh Jain 31 March 2017
        ///Summary :- Detail Page Feature Desktop
        /// </summary>
        /// <returns></returns>
        [Route("features/detail/{basicid}/")]
        [Filters.DeviceDetection()]
        public ActionResult Detail(string basicId)
        {
            DetailPage objDetail = new DetailPage(_Cache, _upcoming, _bikeModels, _models, basicId);
            if (objDetail.status == Entities.StatusCodes.ContentNotFound)
            {
                return Redirect("/pagenotfound.aspx");
            }
            else if (objDetail.status == Entities.StatusCodes.RedirectPermanent)
            {
                return RedirectPermanent(objDetail.redirectUrl);
            }
            else
            {
                DetailFeatureVM objData = objDetail.GetData(4);
                if (objDetail.status == Entities.StatusCodes.ContentNotFound)
                    return Redirect("/pagenotfound.aspx");
                else
                    return View(objData);
            }

        }
        /// <summary>
        ///Created By:- Subodh Jain 31 March 2017
        ///Summary :- Detail Page Feature Mobile
        /// </summary>
        /// <returns></returns>
        [Route("m/features/detail/{basicid}/")]
        public ActionResult Detail_Mobile(string basicId)
        {
            DetailPage objDetail = new DetailPage(_Cache, _upcoming, _bikeModels, _models, basicId);
            if (objDetail.status == Entities.StatusCodes.ContentNotFound)
            {
                return Redirect("/pagenotfound.aspx");
            }
            else if (objDetail.status == Entities.StatusCodes.RedirectPermanent)
            {
                return Redirect(string.Format("/m{0}", objDetail.redirectUrl));
            }
            else
            {
                DetailFeatureVM objData = objDetail.GetData(9);
                if (objDetail.status == Entities.StatusCodes.ContentNotFound)
                    return Redirect("/m/pageNotFound.aspx");
                else
                    return View(objData);
            }
        }

        /// <summary>
        /// Action to get the map features details page
        /// </summary>
        /// <param name="basicid"></param>
        /// <returns></returns>
        [Route("m/features/details/{basicid}/amp/")]
        public ActionResult DetailsAMP(uint basicid)
        {
            ArticlePageDetails objFeatures = null;

            try
            {
                objFeatures = _Cache.GetArticlesDetails(basicid);

                if (objFeatures != null)
                {
                    // set all metatags in the variables
                    ViewBag.Description = objFeatures.Description.StripHtml();
                    ViewBag.Canonical = String.Format("{0}/features/{1}-{2}/", Bikewale.Utility.BWConfiguration.Instance.BwHostUrl, objFeatures.ArticleUrl, basicid);
                    ViewBag.MobilePageUrl = String.Format("{0}/m/features/{1}-{2}/", Bikewale.Utility.BWConfiguration.Instance.BwHostUrl, objFeatures.ArticleUrl, basicid);
                    ViewBag.ArticleSectionTitle = " - BikeWale Features";
                    ViewBag.ArticleType = "Article";
                    ViewBag.Title = objFeatures.Title;
                    ViewBag.MainImageUrl = Bikewale.Utility.Image.GetPathToShowImages(objFeatures.OriginalImgUrl, objFeatures.HostUrl, Bikewale.Utility.ImageSize._640x348);
                    ViewBag.PublishedDate = objFeatures.DisplayDate.ToString();
                    ViewBag.LastModified = objFeatures.DisplayDate.ToString();
                    ViewBag.PageViews = objFeatures.Views;
                    ViewBag.Author = objFeatures.AuthorName;
                    ViewBag.VehicleTagsCnt = 0;

                    // code to get the bikes tagged in the article
                    if (objFeatures.VehiclTagsList != null)
                    {
                        ViewBag.VehicleTagsList = objFeatures.VehiclTagsList.GroupBy(s => s.ModelBase.ModelId).Select(grp => grp.First());
                        ViewBag.VehicleTagsCnt = objFeatures.VehiclTagsList.Count();
                    }

                    // code to get article photos
                    IEnumerable<ModelImage> objImg = _Cache.GetArticlePhotos(Convert.ToInt32(basicid));
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
                ErrorClass objErr = new ErrorClass(ex, "m/features/details/{basicid}/amp/" + basicid);
            }

            return View("~/views/m/content/features/details_amp.cshtml", objFeatures);
        }
    }
}