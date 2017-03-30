using Bikewale.Common;
using Bikewale.Entities.BikeData;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.CMS;
using Bikewale.Interfaces.Dealer;
using Bikewale.Interfaces.ServiceCenter;
using Bikewale.Interfaces.Used;
using Bikewale.Models.ServiceCenters;
using System.Web.Mvc;

namespace Bikewale.Controllers
{
    /// <summary>
    /// Created by Sajal Gupta on 23-03-2017  
    /// </summary>
    public class ServiceCentersController : Controller
    {
        private readonly IBikeMakesCacheRepository<int> _bikeMakesCache = null;
        private readonly IServiceCenterCacheRepository _objSCCache = null;
        private readonly IUsedBikeDetailsCacheRepository _objUsedCache = null;
        private readonly IBikeModels<BikeModelEntity, int> _bikeModels = null;
        private readonly IDealerCacheRepository _objDealerCache = null;
        private readonly ICMSCacheContent _articles = null;
        private readonly IServiceCenter _objSC = null;

        public ServiceCentersController(IDealerCacheRepository objDealerCache, IBikeModels<BikeModelEntity, int> bikeModels, ICMSCacheContent articles, IUsedBikeDetailsCacheRepository objUsedCache, IBikeMakesCacheRepository<int> bikeMakesCache, IServiceCenterCacheRepository objSCCache, IServiceCenter objSC)
        {
            _objUsedCache = objUsedCache;
            _bikeMakesCache = bikeMakesCache;
            _objSCCache = objSCCache;
            _bikeModels = bikeModels;
            _articles = articles;
            _objSC = objSC;
            _objDealerCache = objDealerCache;
        }

        // GET: ServiceCenters
        public ActionResult Index()
        {
            return View();
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
                ServiceCenterIndiaPage modelObj = new ServiceCenterIndiaPage(_articles, _objUsedCache, _bikeMakesCache, _objSCCache, makeMaskingName);

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

                ErrorClass objErr = new ErrorClass(ex, "ServiceCentersController.ServiceCentersInIndia");
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
                ServiceCenterIndiaPage modelObj = new ServiceCenterIndiaPage(_articles, _objUsedCache, _bikeMakesCache, _objSCCache, makeMaskingName);

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

                ErrorClass objErr = new ErrorClass(ex, "ServiceCentersController.ServiceCentersInIndia_Mobile");
                return Redirect(CommonOpn.AppPath + "pageNotFound.aspx");
            }
        }

        [Filters.DeviceDetection()]
        [Route("servicecentersincity/make/{makeMaskingName}/city/{cityMaskingName}")]
        public ActionResult ServiceCentersInCity(string makeMaskingName, string cityMaskingName)
        {
            try
            {
                ServiceCenterCityPage modelObj = new ServiceCenterCityPage(_objDealerCache, _objUsedCache, _bikeModels, _objSCCache, _objSC, _bikeMakesCache, cityMaskingName, makeMaskingName);

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

                ErrorClass objErr = new ErrorClass(ex, "ServiceCentersController.ServiceCentersInCity");
                return Redirect(CommonOpn.AppPath + "pageNotFound.aspx");
            }
        }

        [Route("m/servicecentersincity/make/{makeMaskingName}/city/{cityMaskingName}")]
        public ActionResult ServiceCentersInCity_Mobile(string makeMaskingName, string cityMaskingName)
        {
            try
            {
                ServiceCenterCityPage modelObj = new ServiceCenterCityPage(_objDealerCache, _objUsedCache, _bikeModels, _objSCCache, _objSC, _bikeMakesCache, cityMaskingName, makeMaskingName);

                if (modelObj != null)
                {
                    if (modelObj.status == Entities.StatusCodes.ContentFound)
                    {
                        modelObj.NearByCitiesWidgetTopCount = 9;
                        modelObj.UsedBikeWidgetTopCount = 9;
                        modelObj.BikeShowroomWidgetTopCount = 9;
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

                ErrorClass objErr = new ErrorClass(ex, "ServiceCentersController.ServiceCentersInCity");
                return Redirect(CommonOpn.AppPath + "pageNotFound.aspx");
            }
        }

        [Filters.DeviceDetection()]
        [Route("servicecenterdetail/make/{makeMaskingName}/servicecenterid/{serviceCenterId}")]
        public ActionResult ServiceCenterDetail(string makeMaskingName, uint serviceCenterId)
        {
            try
            {
                ServiceCenterDetailsPage modelObj = new ServiceCenterDetailsPage(_bikeModels, _objUsedCache, _objDealerCache, _objSC, _bikeMakesCache, makeMaskingName, serviceCenterId);

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

                ErrorClass objErr = new ErrorClass(ex, "DealerShowroomController.DealerDetails");
                return Redirect(CommonOpn.AppPath + "pageNotFound.aspx");
            }

        }

        [Route("m/servicecenterdetail/make/{makeMaskingName}/servicecenterid/{serviceCenterId}")]
        public ActionResult ServiceCenterDetail_Mobile(string makeMaskingName, uint serviceCenterId)
        {
            try
            {
                ServiceCenterDetailsPage modelObj = new ServiceCenterDetailsPage(_bikeModels, _objUsedCache, _objDealerCache, _objSC, _bikeMakesCache, makeMaskingName, serviceCenterId);

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

                ErrorClass objErr = new ErrorClass(ex, "DealerShowroomController.DealerDetails");
                return Redirect(CommonOpn.AppPath + "pageNotFound.aspx");
            }

        }
    }
}