using Bikewale.Common;
using Bikewale.Entities.BikeData;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.BikeData.NewLaunched;
using Bikewale.Interfaces.BikeData.UpComing;
using Bikewale.Interfaces.CMS;
using Bikewale.Interfaces.Dealer;
using Bikewale.Interfaces.Location;
using Bikewale.Interfaces.ServiceCenter;
using Bikewale.Interfaces.Used;
using Bikewale.Models.ServiceCenters;
using Bikewale.ServiceCenters;
using System.Web.Mvc;

namespace Bikewale.Controllers
{
    /// <summary>
    /// Created by Sajal Gupta on 23-03-2017  
    /// </summary>
    public class ServiceCentersController : Controller
    {
        #region Private variables
        private readonly IBikeMakesCacheRepository _bikeMakes = null;
        private readonly IServiceCenterCacheRepository _objSCCache = null;
        private readonly IUsedBikeDetailsCacheRepository _objUsedCache = null;
        private readonly IBikeModels<BikeModelEntity, int> _bikeModels = null;
        private readonly IDealerCacheRepository _objDealerCache = null;
        private readonly ICMSCacheContent _articles = null;
        private readonly IServiceCenter _objSC = null;
        private readonly INewBikeLaunchesBL _newLaunches = null;
        private readonly IUpcoming _upcoming = null;
        private readonly ICityCacheRepository _ICityCache = null;
        #endregion

        #region Constructor
        public ServiceCentersController(ICityCacheRepository ICityCache, IUpcoming upcoming, INewBikeLaunchesBL newLaunches, IDealerCacheRepository objDealerCache, IBikeModels<BikeModelEntity, int> bikeModels, ICMSCacheContent articles, IUsedBikeDetailsCacheRepository objUsedCache, IBikeMakesCacheRepository bikeMakesCache, IServiceCenterCacheRepository objSCCache, IServiceCenter objSC)
        {
            _objUsedCache = objUsedCache;
            _bikeMakes = bikeMakesCache;
            _objSCCache = objSCCache;
            _bikeModels = bikeModels;
            _articles = articles;
            _objSC = objSC;
            _objDealerCache = objDealerCache;
            _upcoming = upcoming;
            _newLaunches = newLaunches;
            _ICityCache = ICityCache;
        }
        #endregion


        /// <summary>
        /// Created by Sajal Gupta on 30-03-2017
        /// This action method will fetch data for service center landing page desktop.
        /// </summary>
        /// <returns></returns>
        [Filters.DeviceDetection()]
        [Route("servicecenter/Index/")]
        public ActionResult Index()
        {
            try
            {
                ServiceCenterLandingPage modelObj = new ServiceCenterLandingPage(_ICityCache, _objUsedCache, _upcoming, _newLaunches, _bikeModels, _articles, _bikeMakes);
                if (modelObj != null)
                {
                    modelObj.BrandWidgetTopCount = 10;
                    modelObj.BikeCareRecordsCount = 3;
                    modelObj.PopularBikeWidgetTopCount = 9;
                    modelObj.NewLaunchedBikesWidgtData = 9;
                    modelObj.UpcomingBikesWidgetData = 9;
                    modelObj.UsedBikeModelWidgetTopCount = 9;
                    ServiceCenterLandingPageVM pageVM = modelObj.GetData();
                    return View(pageVM);
                }
                else
                {
                    return Redirect(CommonOpn.AppPath + "pageNotFound.aspx");
                }
            }
            catch (System.Exception ex)
            {
                ErrorClass.LogError(ex, "ServiceCentersController.Index");
                return Redirect(CommonOpn.AppPath + "pageNotFound.aspx");
            }
        }

        /// <summary>
        /// Created by Sajal Gupta on 30-03-2017
        /// This action method will fetch data for service center landing page mobile.
        /// </summary>
        /// <returns></returns>
        [Route("m/servicecenter/Index/")]
        public ActionResult Index_Mobile()
        {
            try
            {
                ServiceCenterLandingPage modelObj = new ServiceCenterLandingPage(_ICityCache, _objUsedCache, _upcoming, _newLaunches, _bikeModels, _articles, _bikeMakes);
                if (modelObj != null)
                {
                    modelObj.BrandWidgetTopCount = 6;
                    modelObj.BikeCareRecordsCount = 3;
                    modelObj.PopularBikeWidgetTopCount = 9;
                    modelObj.NewLaunchedBikesWidgtData = 9;
                    modelObj.UpcomingBikesWidgetData = 9;
                    modelObj.UsedBikeModelWidgetTopCount = 9;
                    modelObj.IsMobile = true;
                    ServiceCenterLandingPageVM pageVM = modelObj.GetData();
                    return View(pageVM);
                }
                else
                {
                    return Redirect(CommonOpn.AppPath + "pageNotFound.aspx");
                }
            }
            catch (System.Exception ex)
            {
                ErrorClass.LogError(ex, "ServiceCentersController.Index_Mobile");
                return Redirect(CommonOpn.AppPath + "pageNotFound.aspx");
            }
        }

        /// <summary>
        /// Created by Sajal Gupta on 28-03-2017
        /// This action method will fetch details for service centers in india page Desktop
        /// </summary>
        /// <param name="makeMaskingName"></param>
        /// <returns></returns>
        [Filters.DeviceDetection()]
        [Route("servicecentersinindia/make/{makeMaskingName}")]
        public ActionResult ServiceCentersInIndia(string makeMaskingName)
        {
            try
            {
                ServiceCenterIndiaPage modelObj = new ServiceCenterIndiaPage(_articles, _objUsedCache, _bikeMakes, _objSCCache, makeMaskingName);

                if (modelObj != null)
                {
                    if (modelObj.status == Entities.StatusCodes.ContentFound)
                    {
                        ServiceCenterIndiaPageVM objVM = modelObj.GetData();
                        return View(objVM);
                    }
                    else if (modelObj.status == Entities.StatusCodes.RedirectTemporary)
                    {
                        return Redirect(modelObj.redirectUrl);
                    }
                    else if (modelObj.status == Entities.StatusCodes.RedirectPermanent)
                    {
                        return RedirectPermanent(Request.RawUrl.Replace(makeMaskingName, modelObj.objResponse.MaskingName));
                    }
                    else
                    {
                        return Redirect(CommonOpn.AppPath + "pageNotFound.aspx");
                    }
                }
                else
                {
                    return Redirect(CommonOpn.AppPath + "pageNotFound.aspx");
                }
            }
            catch (System.Exception ex)
            {

                ErrorClass.LogError(ex, "ServiceCentersController.ServiceCentersInIndia");
                return Redirect(CommonOpn.AppPath + "pageNotFound.aspx");
            }
        }

        /// <summary>
        /// Created by Sajal Gupta on 28-03-2017
        /// This action method will fetch details for service centers in india page Mobile
        /// </summary>
        /// <param name="makeMaskingName"></param>
        /// <returns></returns>
        [Route("m/servicecentersinindia/make/{makeMaskingName}")]
        public ActionResult ServiceCentersInIndia_Mobile(string makeMaskingName)
        {
            try
            {
                ServiceCenterIndiaPage modelObj = new ServiceCenterIndiaPage(_articles, _objUsedCache, _bikeMakes, _objSCCache, makeMaskingName);

                if (modelObj != null)
                {
                    if (modelObj.status == Entities.StatusCodes.ContentFound)
                    {
                        modelObj.isMobile = true;
                        ServiceCenterIndiaPageVM objVM = modelObj.GetData();
                        return View(objVM);
                    }
                    else if (modelObj.status == Entities.StatusCodes.RedirectTemporary)
                    {
                        return Redirect(string.Format("/m{0}", modelObj.redirectUrl));
                    }
                    else if (modelObj.status == Entities.StatusCodes.RedirectPermanent)
                    {
                        return RedirectPermanent(Request.RawUrl.Replace(makeMaskingName, modelObj.objResponse.MaskingName));
                    }
                    else
                    {
                        return Redirect(CommonOpn.AppPath + "pageNotFound.aspx");
                    }
                }
                else
                {
                    return Redirect(CommonOpn.AppPath + "pageNotFound.aspx");
                }
            }
            catch (System.Exception ex)
            {

                ErrorClass.LogError(ex, "ServiceCentersController.ServiceCentersInIndia_Mobile");
                return Redirect(CommonOpn.AppPath + "pageNotFound.aspx");
            }
        }


        /// <summary>
        /// Created by Sajal Gupta on 30-03-2017
        /// This action method will fetch data for service center in city page desktop.
        /// </summary>
        /// <returns></returns>
        [Filters.DeviceDetection()]
        [Route("servicecentersincity/make/{makeMaskingName}/city/{cityMaskingName}")]
        public ActionResult ServiceCentersInCity(string makeMaskingName, string cityMaskingName)
        {
            try
            {
                ServiceCenterCityPage modelObj = new ServiceCenterCityPage(_objDealerCache, _objUsedCache, _bikeModels, _objSCCache, _objSC, _bikeMakes, cityMaskingName, makeMaskingName);

                if (modelObj != null)
                {
                    if (modelObj.status == Entities.StatusCodes.ContentFound)
                    {
                        modelObj.NearByCitiesWidgetTopCount = 9;
                        modelObj.UsedBikeWidgetTopCount = 9;
                        modelObj.BikeShowroomWidgetTopCount = 3;
                        ServiceCenterCityPageVM objPage = modelObj.GetData();
                        return View(objPage);
                    }
                    else if (modelObj.status == Entities.StatusCodes.RedirectPermanent)
                    {
                        return RedirectPermanent(Request.RawUrl.Replace(makeMaskingName, modelObj.objResponse.MaskingName));
                    }
                    else
                    {
                        return Redirect(CommonOpn.AppPath + "pageNotFound.aspx");
                    }
                }
                else
                {
                    return Redirect(CommonOpn.AppPath + "pageNotFound.aspx");
                }
            }
            catch (System.Exception ex)
            {

                ErrorClass.LogError(ex, "ServiceCentersController.ServiceCentersInCity");
                return Redirect(CommonOpn.AppPath + "pageNotFound.aspx");
            }
        }

        /// <summary>
        /// Created by Sajal Gupta on 30-03-2017
        /// This action method will fetch data for service center in city page desktop.
        /// </summary>
        /// <returns></returns>
        [Route("m/servicecentersincity/make/{makeMaskingName}/city/{cityMaskingName}")]
        public ActionResult ServiceCentersInCity_Mobile(string makeMaskingName, string cityMaskingName)
        {
            try
            {
                ServiceCenterCityPage modelObj = new ServiceCenterCityPage(_objDealerCache, _objUsedCache, _bikeModels, _objSCCache, _objSC, _bikeMakes, cityMaskingName, makeMaskingName);

                if (modelObj != null)
                {
                    if (modelObj.status == Entities.StatusCodes.ContentFound)
                    {
                        modelObj.NearByCitiesWidgetTopCount = 9;
                        modelObj.UsedBikeWidgetTopCount = 9;
                        modelObj.BikeShowroomWidgetTopCount = 9;
                        modelObj.IsMobile = true;
                        ServiceCenterCityPageVM objPage = modelObj.GetData();
                        return View(objPage);
                    }
                    else if (modelObj.status == Entities.StatusCodes.RedirectPermanent)
                    {
                        return RedirectPermanent(Request.RawUrl.Replace(makeMaskingName, modelObj.objResponse.MaskingName));
                    }
                    else
                    {
                        return Redirect(CommonOpn.AppPath + "pageNotFound.aspx");
                    }
                }
                else
                {
                    return Redirect(CommonOpn.AppPath + "pageNotFound.aspx");
                }
            }
            catch (System.Exception ex)
            {

                ErrorClass.LogError(ex, "ServiceCentersController.ServiceCentersInCity_Mobile");
                return Redirect(CommonOpn.AppPath + "pageNotFound.aspx");
            }
        }

        /// <summary>
        /// Created by Sajal Gupta on 30-03-2017
        /// This action method will fetch data for service center details page desktop.
        /// </summary>
        /// <returns></returns>
        [Filters.DeviceDetection()]
        [Route("servicecenterdetail/make/{makeMaskingName}/servicecenterid/{serviceCenterId}")]
        public ActionResult ServiceCenterDetail(string makeMaskingName, uint serviceCenterId)
        {
            try
            {
                ServiceCenterDetailsPage modelObj = new ServiceCenterDetailsPage(_bikeModels, _objUsedCache, _objDealerCache, _objSC, _bikeMakes, makeMaskingName, serviceCenterId);

                if (modelObj != null)
                {
                    if (modelObj.status == Entities.StatusCodes.ContentFound)
                    {
                        modelObj.PopularBikeWidgetTopCount = 9;
                        modelObj.UsedBikeWidgetTopCount = 9;
                        modelObj.BikeShowroomWidgetTopCount = 3;
                        modelObj.OtherServiceCenterWidgetTopCount = 3;
                        ServiceCenterDetailsPageVM objPage = modelObj.GetData();
                        return View(objPage);
                    }
                    else if (modelObj.status == Entities.StatusCodes.RedirectPermanent)
                    {
                        return RedirectPermanent(Request.RawUrl.Replace(makeMaskingName, modelObj.objResponse.MaskingName));
                    }
                    else
                    {
                        return Redirect(CommonOpn.AppPath + "pageNotFound.aspx");
                    }
                }
                else
                {
                    return Redirect(CommonOpn.AppPath + "pageNotFound.aspx");
                }
            }
            catch (System.Exception ex)
            {

                ErrorClass.LogError(ex, "ServiceCentersController.ServiceCenterDetail");
                return Redirect(CommonOpn.AppPath + "pageNotFound.aspx");
            }

        }

        /// <summary>
        /// Created by Sajal Gupta on 30-03-2017
        /// This action method will fetch data for service center details page mobile.
        /// </summary>
        /// <returns></returns>
        [Route("m/servicecenterdetail/make/{makeMaskingName}/servicecenterid/{serviceCenterId}")]
        public ActionResult ServiceCenterDetail_Mobile(string makeMaskingName, uint serviceCenterId)
        {
            try
            {
                ServiceCenterDetailsPage modelObj = new ServiceCenterDetailsPage(_bikeModels, _objUsedCache, _objDealerCache, _objSC, _bikeMakes, makeMaskingName, serviceCenterId);

                if (modelObj != null)
                {
                    if (modelObj.status == Entities.StatusCodes.ContentFound)
                    {
                        modelObj.PopularBikeWidgetTopCount = 9;
                        modelObj.UsedBikeWidgetTopCount = 9;
                        modelObj.BikeShowroomWidgetTopCount = 6;
                        modelObj.OtherServiceCenterWidgetTopCount = 6;
                        ServiceCenterDetailsPageVM objPage = modelObj.GetData();
                        return View(objPage);
                    }
                    else if (modelObj.status == Entities.StatusCodes.RedirectPermanent)
                    {
                        return RedirectPermanent(Request.RawUrl.Replace(makeMaskingName, modelObj.objResponse.MaskingName));
                    }
                    else
                    {
                        return Redirect(CommonOpn.AppPath + "pageNotFound.aspx");
                    }
                }
                else
                {
                    return Redirect(CommonOpn.AppPath + "pageNotFound.aspx");
                }
            }
            catch (System.Exception ex)
            {

                ErrorClass.LogError(ex, "ServiceCentersController.ServiceCenterDetail_Mobile");
                return Redirect(CommonOpn.AppPath + "pageNotFound.aspx");
            }

        }
    }
}