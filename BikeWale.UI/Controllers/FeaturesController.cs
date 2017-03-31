using Bikewale.CoreDAL;
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
        private ICMSCacheContent _Cache = null;
        private IPager _objPager = null;


        public FeaturesController(ICMSCacheContent Cache, IPager objPager)
        {
            _Cache = Cache;
            _objPager = objPager;
        }

        /// <summary>
        /// Created by :- Subodh Jain on 31 March 2017
        /// Summary :- Index Method for Features news section
        /// </summary>
        /// <returns></returns>
        [Route("content/features/{pageNumber}")]
        [Filters.DeviceDetection()]
        public ActionResult Index(ushort? pageNumber)
        {
            try
            {
                IndexPage objIndexPage = new IndexPage(_Cache, _objPager);
                if (objIndexPage != null)
                {
                    IndexFeatureVM objFeatureIndex = new IndexFeatureVM();
                    return View(objFeatureIndex);
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
        [Route("m/content/features/{pageNumber}")]
        public ActionResult Index_Mobile(ushort? pageNumber)
        {
            return View();
        }

        public ActionResult Details()
        {
            return View();
        }

        public ActionResult Details_Mobile()
        {
            return View();
        }
    }
}