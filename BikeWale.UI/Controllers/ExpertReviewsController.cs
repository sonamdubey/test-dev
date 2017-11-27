﻿using System;
using System.Web.Mvc;
using Bikewale.Entities;
using Bikewale.Entities.BikeData;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.BikeData.UpComing;
using Bikewale.Interfaces.CMS;
using Bikewale.Interfaces.Location;
using Bikewale.Interfaces.Pager;
using Bikewale.Models;
using Bikewale.Notifications;

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
        private readonly IBikeMaskingCacheRepository<BikeModelEntity, int> _bikeMasking = null;
        private readonly IBikeModels<BikeModelEntity, int> _bikeModels = null;
        private readonly IUpcoming _upcoming = null;
        private readonly IBikeInfo _bikeInfo = null;
        private readonly ICityCacheRepository _cityCache = null;
        private readonly IBikeMakesCacheRepository _bikeMakesCacheRepository = null;
        private readonly IBikeVersionCacheRepository<BikeVersionEntity, uint> _objBikeVersionsCache = null;
        private readonly IBikeSeriesCacheRepository _seriesCache;
        private readonly IBikeSeries _series;
        #region Constructor
        public ExpertReviewsController(ICMSCacheContent cmsCache, IPager pager, IBikeModelsCacheRepository<int> models, IBikeModels<BikeModelEntity, int> bikeModels, IUpcoming upcoming, IBikeInfo bikeInfo, ICityCacheRepository cityCache, IBikeMakesCacheRepository bikeMakesCacheRepository, IBikeVersionCacheRepository<BikeVersionEntity, uint> objBikeVersionsCache, IBikeMaskingCacheRepository<BikeModelEntity, int> bikeMasking, IBikeSeriesCacheRepository seriesCache, IBikeSeries series)
        {
            _cmsCache = cmsCache;
            _pager = pager;
            _models = models;
            _bikeModels = bikeModels;
            _upcoming = upcoming;
            _bikeInfo = bikeInfo;
            _cityCache = cityCache;
            _bikeMakesCacheRepository = bikeMakesCacheRepository;
            _objBikeVersionsCache = objBikeVersionsCache;
            _bikeMasking = bikeMasking;
            _seriesCache = seriesCache;
            _series = series;
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
            ExpertReviewsIndexPage obj = new ExpertReviewsIndexPage(_cmsCache, _pager, _models, _bikeModels, _upcoming, _bikeMakesCacheRepository, _objBikeVersionsCache, _seriesCache, _series);
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
            ExpertReviewsIndexPage obj = new ExpertReviewsIndexPage(_cmsCache, _pager, _models, _bikeModels, _upcoming, _bikeMakesCacheRepository, _objBikeVersionsCache, _seriesCache, _series);
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
            ExpertReviewsDetailPage obj = new ExpertReviewsDetailPage(_cmsCache, _models, _bikeModels, _upcoming, _bikeInfo, _cityCache, _bikeMakesCacheRepository, _objBikeVersionsCache, _bikeMasking, basicid);
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
                obj.IsMobile = true;
                obj.RefControllerContext = ControllerContext;
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
            ExpertReviewsDetailPage obj = new ExpertReviewsDetailPage(_cmsCache, _models, _bikeModels, _upcoming, _bikeInfo, _cityCache, _bikeMakesCacheRepository, _objBikeVersionsCache, _bikeMasking, basicid);
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
                obj.RefControllerContext = ControllerContext;
                ExpertReviewsDetailPageVM objData = obj.GetData(3);
                if (obj.status == Entities.StatusCodes.ContentNotFound)
                    return Redirect("/pagenotfound.aspx");
                else
                    return View(objData);
            }
        }

        /// <summary>
        /// Action to get the map expertreviews details page
        /// Modified by: Vivek Singh Tomar on 31st Aug 2017
        /// Summary: Removed use of viewbag with VM
        /// </summary>
        /// <param name="basicid"></param>
        /// <returns></returns>
        [Route("m/expertreviews/details/{basicid}/amp/")]
        public ActionResult DetailsAMP(string basicid)
        {
            ExpertReviewsDetailPageVM objData = null;

            ExpertReviewsDetailPage obj = new ExpertReviewsDetailPage(_cmsCache, _models, _bikeModels, _upcoming, _bikeInfo, _cityCache, _bikeMakesCacheRepository, _objBikeVersionsCache, _bikeMasking, basicid);
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
                obj.IsAMPPage = true;
                obj.RefControllerContext = ControllerContext;
                objData = obj.GetData(9);
                if (obj.status == StatusCodes.ContentNotFound)
                    return Redirect("/pagenotfound.aspx");
                else
                    return View("~/views/m/content/expertreviews/details_amp.cshtml", objData);
            }
        }

        [Route("expertreviews/list/")]
        public ActionResult ExpertReviewsListByModel(uint makeId, uint modelId, uint topCount)
        {
            RecentExpertReviewsVM objData = null;

            RecentExpertReviews obj = new RecentExpertReviews(topCount, makeId, modelId, _cmsCache);
            objData = obj.GetData();
            objData.Title = "You may also like expert's reviews";

            return View("~/views/ExpertReviews/_ExpertReviewHorizontalListByModel.cshtml", objData);
        }

        [Route("m/expertreviews/list/")]
        public ActionResult ExpertReviewsListByModel_Mobile(uint makeId, uint modelId, uint topCount)
        {
            RecentExpertReviewsVM objData = null;

            RecentExpertReviews obj = new RecentExpertReviews(topCount, makeId, modelId, _cmsCache);
            objData = obj.GetData();
            objData.Title = "You may also like expert's reviews";

            return View("~/views/ExpertReviews/_ExpertReviewHorizontalListByModel_Mobile.cshtml", objData);
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