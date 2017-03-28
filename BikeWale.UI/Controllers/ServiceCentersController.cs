using Bikewale.Common;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.CMS;
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
        private readonly ICMSCacheContent _articles = null;

        public ServiceCentersController(ICMSCacheContent articles, IUsedBikeDetailsCacheRepository objUsedCache, IBikeMakesCacheRepository<int> bikeMakesCache, IServiceCenterCacheRepository objSCCache)
        {
            _objUsedCache = objUsedCache;
            _bikeMakesCache = bikeMakesCache;
            _objSCCache = objSCCache;
            _articles = articles;
        }

        // GET: ServiceCenters
        public ActionResult Index()
        {
            return View();
        }

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
    }
}