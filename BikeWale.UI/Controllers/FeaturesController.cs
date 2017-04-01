using Bikewale.CoreDAL;
using Bikewale.Entities.BikeData;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.BikeData.UpComing;
using Bikewale.Interfaces.CMS;
using Bikewale.Interfaces.Pager;
using Bikewale.Models;
using Bikewale.Models.Features;
using Bikewale.Notifications;
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
        public ActionResult Index(ushort? pageNumber)
        {
            try
            {
                IndexPage objIndexPage = new IndexPage(_Cache, _objPager, _upcoming, _bikeModels);
                if (objIndexPage != null)
                {
                    IndexFeatureVM objFeatureIndex = new IndexFeatureVM();
                    objIndexPage.TopCount = 4;
                    if (pageNumber.HasValue && pageNumber.Value > 0)
                        objIndexPage.CurPageNo = pageNumber.Value;
                    objFeatureIndex = objIndexPage.GetData();
                    if (objIndexPage.status == Entities.StatusCodes.ContentFound)
                        return View(objFeatureIndex);
                    else
                        return Redirect(CommonOpn.AppPath + "pageNotFound.aspx");
                }
                else
                {
                    return Redirect(CommonOpn.AppPath + "pageNotFound.aspx");
                }
            }
            catch (System.Exception ex)
            {

                ErrorClass objErr = new ErrorClass(ex, "FeaturesController.Index");
                return Redirect(CommonOpn.AppPath + "pageNotFound.aspx");
            }


        }
        /// <summary>
        /// Created by :- Subodh Jain on 31 March 2017
        /// Summary :- Index Method for Features news section Mobile
        /// </summary>
        /// <returns></returns>
        [Route("m/features/")]
        public ActionResult Index_Mobile(ushort? pageNumber)
        {
            try
            {
                IndexPage objIndexPage = new IndexPage(_Cache, _objPager, _upcoming, _bikeModels);
                if (objIndexPage != null)
                {
                    IndexFeatureVM objFeatureIndex = new IndexFeatureVM();
                    objIndexPage.TopCount = 9;
                    if (pageNumber.HasValue && pageNumber.Value > 0)
                        objIndexPage.CurPageNo = pageNumber.Value;
                    objFeatureIndex = objIndexPage.GetData();
                    if (objIndexPage.status == Entities.StatusCodes.ContentFound)
                        return View(objFeatureIndex);
                    else
                        return Redirect(CommonOpn.AppPath + "pageNotFound.aspx");
                }
                else
                {
                    return Redirect(CommonOpn.AppPath + "pageNotFound.aspx");
                }
            }
            catch (System.Exception ex)
            {

                ErrorClass objErr = new ErrorClass(ex, "FeaturesController.Index");
                return Redirect(CommonOpn.AppPath + "pageNotFound.aspx");
            }
        }

        /// <summary>
        ///Created By:- Subodh Jain 31 March 2017
        ///Summary :- Detail Page Feature Desktop
        /// </summary>
        /// <returns></returns>
        [Route("content/features/details/")]
        [Filters.DeviceDetection()]
        public ActionResult Detail(uint? basicId)
        {
            try
            {
                uint _basicId = basicId.HasValue ? basicId.Value : 0;
                DetailPage objDetail = new DetailPage(_basicId, _Cache, _upcoming, _bikeModels, _models);
                if (objDetail != null)
                {
                    DetailFeatureVM objDetailsVM = new DetailFeatureVM();
                    objDetail.TopCount = 4;
                    objDetailsVM = objDetail.GetData();
                    if (objDetail.status == Entities.StatusCodes.ContentFound)
                        return View(objDetailsVM);
                    else if (objDetail.status == Entities.StatusCodes.RedirectPermanent)
                    {

                        string redirectUrl = string.Format("/features/{0}-{1}/", objDetailsVM.objFeature.Title, _basicId);
                        return Redirect(redirectUrl);
                    }
                    else
                        return Redirect(CommonOpn.AppPath + "pageNotFound.aspx");

                }
                else
                {

                    return Redirect(CommonOpn.AppPath + "pageNotFound.aspx");
                }
            }
            catch (System.Exception ex)
            {

                ErrorClass objErr = new ErrorClass(ex, "FeaturesController.Index");
                return Redirect(CommonOpn.AppPath + "pageNotFound.aspx");
            }



        }
        /// <summary>
        ///Created By:- Subodh Jain 31 March 2017
        ///Summary :- Detail Page Feature Mobile
        /// </summary>
        /// <returns></returns>
        [Route("m/content/features/details/")]
        public ActionResult Detail_Mobile(uint? basicId)
        {
            try
            {
                uint _basicId = basicId.HasValue ? basicId.Value : 0;
                DetailPage objDetail = new DetailPage(_basicId, _Cache, _upcoming, _bikeModels, _models);
                if (objDetail != null)
                {
                    DetailFeatureVM objDetailsVM = new DetailFeatureVM();
                    objDetail.TopCount = 9;
                    objDetailsVM = objDetail.GetData();
                    if (objDetail.status == Entities.StatusCodes.ContentFound)
                        return View(objDetailsVM);
                    else if (objDetail.status == Entities.StatusCodes.RedirectPermanent && objDetailsVM != null && objDetailsVM.objFeature != null)
                    {
                        string redirectUrl = string.Format("/m/features/{0}-{1}/", objDetailsVM.objFeature.Title, _basicId);
                        return Redirect(redirectUrl);
                    }
                    else
                        return Redirect(CommonOpn.AppPath + "pageNotFound.aspx");

                }
                else
                {

                    return Redirect(CommonOpn.AppPath + "pageNotFound.aspx");
                }
            }
            catch (System.Exception ex)
            {

                ErrorClass objErr = new ErrorClass(ex, "FeaturesController.Index");
                return Redirect(CommonOpn.AppPath + "pageNotFound.aspx");
            }
        }
    }
}