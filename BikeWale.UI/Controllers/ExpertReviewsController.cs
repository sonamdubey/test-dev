﻿using Bikewale.Entities;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.CMS.Articles;
using Bikewale.Entities.CMS.Photos;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.BikeData.UpComing;
using Bikewale.Interfaces.CMS;
using Bikewale.Interfaces.Location;
using Bikewale.Interfaces.Pager;
using Bikewale.Models;
using Bikewale.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Bikewale.Controllers
{
    /// <summary>
    /// Created By : Ashish G. Kamble on 19 Jan 2017
    /// Summary : Contoller have functions related to the expert reviews for mobile site
    /// </summary>
    public class ExpertReviewsController : Controller
    {
        private readonly ICMSCacheContent _cmsCache = null;
        private readonly IPager _pager = null;
        private readonly IBikeModelsCacheRepository<int> _models = null;
        private readonly IBikeModels<BikeModelEntity, int> _bikeModels = null;
        private readonly IUpcoming _upcoming = null;
        private readonly IBikeInfo _bikeInfo = null;
        private readonly ICityCacheRepository _cityCache = null;
        private readonly IBikeMakesCacheRepository<int> _bikeMakesCacheRepository = null;
        #region Constructor
        public ExpertReviewsController(ICMSCacheContent cmsCache, IPager pager, IBikeModelsCacheRepository<int> models, IBikeModels<BikeModelEntity, int> bikeModels, IUpcoming upcoming, IBikeInfo bikeInfo, ICityCacheRepository cityCache, IBikeMakesCacheRepository<int> bikeMakesCacheRepository)
        {
            _cmsCache = cmsCache;
            _pager = pager;
            _models = models;
            _bikeModels = bikeModels;
            _upcoming = upcoming;
            _bikeInfo = bikeInfo;
            _cityCache = cityCache;
            _bikeMakesCacheRepository = bikeMakesCacheRepository;
        }
        #endregion

        #region Action Methods

        /// <summary>
        /// Created by : Aditi srivastava on 30 Mar 2017
        /// Summmary   : Action method to render news listing page- Desktop
        /// </summary>
        [Route("expertreviews/")]
        [Filters.DeviceDetection()]
        public ActionResult Index()
        {
            ExpertReviewsIndexPage obj = new ExpertReviewsIndexPage(_cmsCache, _pager, _models, _bikeModels, _upcoming);
            if (obj.status == Entities.StatusCodes.ContentNotFound)
            {
                return Redirect("/pagenotfound.aspx");
            }
            else if (obj.status == Entities.StatusCodes.RedirectPermanent)
            {
                return RedirectPermanent(obj.redirectUrl);
            }
            else
            {
                ExpertReviewsIndexPageVM objData = obj.GetData(4);
                if (obj.status == StatusCodes.ContentNotFound)
                    return Redirect("/pagenotfound.aspx");
                else
                    return View(objData);
            }
        }

        /// <summary>
        /// Created by : Aditi srivastava on 27 Mar 2017
        /// Summmary   : Action method to render news listing page - mobile
        /// </summary>
        [Route("m/expertreviews/")]
        public ActionResult Index_Mobile()
        {
            ExpertReviewsIndexPage obj = new ExpertReviewsIndexPage(_cmsCache, _pager, _models, _bikeModels, _upcoming);
            obj.IsMobile = true;
            if (obj.status == Entities.StatusCodes.ContentNotFound)
            {
                return Redirect("/m/pagenotfound.aspx");
            }
            else if (obj.status == Entities.StatusCodes.RedirectPermanent)
            {
                return RedirectPermanent(string.Format("/m{0}", obj.redirectUrl));
            }
            else
            {
                ExpertReviewsIndexPageVM objData = obj.GetData(9);
                if (obj.status == StatusCodes.ContentNotFound)
                    return Redirect("/m/pagenotfound.aspx");
                else
                    return View(objData);
            }
        }

        /// <summary>
        /// Created by : Aditi Srivastava on 31 Mar 2017
        /// Summary    : Action method for expert review details page - Mobile
        /// </summary>
        [Route("m/expertreviews/details/{basicid}/")]
        public ActionResult Detail_Mobile(string basicid)
        {
            ExpertReviewsDetailPage obj = new ExpertReviewsDetailPage(_cmsCache, _models, _bikeModels, _upcoming, _bikeInfo, _cityCache, basicid);
            obj.IsMobile = true;
            if (obj.status == Entities.StatusCodes.ContentNotFound)
            {
                return Redirect("/m/pagenotfound.aspx");
            }
            else if (obj.status == Entities.StatusCodes.RedirectPermanent)
            {
                return RedirectPermanent(string.Format("/m{0}", obj.redirectUrl));
            }
            else
            {
                ExpertReviewsDetailPageVM objData = obj.GetData(9);
                if (obj.status == Entities.StatusCodes.ContentNotFound)
                    return Redirect("/m/pagenotfound.aspx");
                else
                    return View(objData);
            }
        }

        /// <summary>
        /// Created by : Aditi Srivastava on 31 Mar 2017
        /// Summary    : Action method for expert review details page - Desktop
        /// </summary>
        [Route("expertreviews/details/{basicid}/")]
        [Filters.DeviceDetection()]
        public ActionResult Detail(string basicid)
        {
            ExpertReviewsDetailPage obj = new ExpertReviewsDetailPage(_cmsCache, _models, _bikeModels, _upcoming, _bikeInfo, _cityCache, basicid);
            if (obj.status == Entities.StatusCodes.ContentNotFound)
            {
                return Redirect("/pagenotfound.aspx");
            }
            else if (obj.status == Entities.StatusCodes.RedirectPermanent)
            {
                return RedirectPermanent(obj.redirectUrl);
            }
            else
            {
                ExpertReviewsDetailPageVM objData = obj.GetData(3);
                if (obj.status == Entities.StatusCodes.ContentNotFound)
                    return Redirect("/pagenotfound.aspx");
                else
                    return View(objData);
            }
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
                objExpertReviews = _cmsCache.GetArticlesDetails(basicid);

                if (objExpertReviews != null)
                {
                    // set all metatags in the variables
                    ViewBag.Description = objExpertReviews.Description.StripHtml();
                    ViewBag.Canonical = String.Format("{0}/expert-reviews/{1}-{2}.html", Bikewale.Utility.BWConfiguration.Instance.BwHostUrl, objExpertReviews.ArticleUrl, basicid);
                    ViewBag.MobilePageUrl = String.Format("{0}/m/expert-reviews/{1}-{2}.html", Bikewale.Utility.BWConfiguration.Instance.BwHostUrl, objExpertReviews.ArticleUrl, basicid);
                    ViewBag.ArticleSectionTitle = " - BikeWale Expert Reviews";
                    ViewBag.ArticleType = "Article";
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
                    IEnumerable<ModelImage> objImg = _cmsCache.GetArticlePhotos(Convert.ToInt32(basicid));
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
        /// Created by: Vivek Singh Tomar on 17th Aug 2017
        /// Summary: Action method to render expert reviews for scooters
        /// </summary>
        /// <returns></returns>
        [Route("scooter/expertreviews/")]
        [Filters.DeviceDetection()]
        public ActionResult Scooters()
        {
            try
            {

                ScooterExpertReviewsPage obj = new ScooterExpertReviewsPage(_cmsCache, _pager, _models, _bikeModels, _bikeMakesCacheRepository);
                if (obj.status == StatusCodes.ContentNotFound)
                {
                    return Redirect("/pagenotfound.aspx");
                }
                else if (obj.status == StatusCodes.RedirectPermanent)
                {
                    return RedirectPermanent(obj.redirectUrl);
                }
                else
                {
                    ScooterExpertReviewsPageVM objData = obj.GetData();
                    if (obj.status == StatusCodes.ContentNotFound)
                        return Redirect("/pagenotfound.aspx");
                    else
                        return View(objData);
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Bikewale.Controllers.ExpertReviewsController.Scooters");
                return Redirect("/pagenotfound.aspx");
            }
        }

        /// <summary>
        /// Created by: Vivek Singh Tomar on 18th Aug 2017
        /// Summary: Action method to render expert reviews for scooters on mobile
        /// </summary>
        /// <returns></returns>
        [Route("m/scooter/expertreviews/")]
        public ActionResult Scooters_Mobile()
        {
            try
            {
                ScooterExpertReviewsPage obj = new ScooterExpertReviewsPage(_cmsCache, _pager, _models, _bikeModels, _bikeMakesCacheRepository);
                obj.IsMobile = true;
                if (obj.status == StatusCodes.ContentNotFound)
                {
                    return Redirect("/m/pagenotfound.aspx");
                }
                else if (obj.status == StatusCodes.RedirectPermanent)
                {
                    return RedirectPermanent(string.Format("/m{0}", obj.redirectUrl));
                }
                else
                {
                    ScooterExpertReviewsPageVM objData = obj.GetData();
                    if (obj.status == StatusCodes.ContentNotFound)
                        return Redirect("/m/pagenotfound.aspx");
                    else
                        return View(objData);
                }
            }
            catch (Exception ex)
            {

                ErrorClass objErr = new ErrorClass(ex, "Bikewale.Controllers.ExpertReviewsController.Scooters_Mobile");
                return Redirect("/m/pagenotfound.aspx");
            }
        }
        #endregion
    }
}