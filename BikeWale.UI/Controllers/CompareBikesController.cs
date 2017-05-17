using Bikewale.CoreDAL;
using Bikewale.Interfaces.CMS;
using Bikewale.Interfaces.Compare;
using Bikewale.Models;
using System.Web.Mvc;

namespace Bikewale.Controllers
{
    /// <summary>
    /// Created By :- Subodh Jain 10 May 2017
    /// Summary :- Compare Bike controller
    /// </summary>
    public class CompareBikesController : Controller
    {

        private readonly IBikeCompareCacheRepository _cachedCompare = null;
        private readonly ICMSCacheContent _compareTest = null;

        public CompareBikesController(IBikeCompareCacheRepository cachedCompare, ICMSCacheContent compareTest)
        {
            _cachedCompare = cachedCompare;
            _compareTest = compareTest;

        }

        // GET: CompareBikes
        [Route("compare/")]
        public ActionResult Index()
        {
            CompareIndex objCompare = new CompareIndex(_cachedCompare, _compareTest);

            if (objCompare != null)
            {
                CompareVM CompareVM = null;

                CompareVM = objCompare.GetData();
                return View(CompareVM);
            }
            else
            {
                return Redirect(CommonOpn.AppPath + "pageNotFound.aspx");
            }
        }
        // GET: CompareBikes
        [Route("m/compare/")]
        public ActionResult Index_Mobile()
        {
            CompareIndex objCompare = new CompareIndex(_cachedCompare, _compareTest);

            if (objCompare != null)
            {
                CompareVM CompareVM = null;

                CompareVM = objCompare.GetData();
                return View(CompareVM);
            }
            else
            {
                return Redirect(CommonOpn.AppPath + "pageNotFound.aspx");
            }
        }

        // GET: CompareBikes Details
        [Route("compare/details/")]
        public ActionResult CompareBikeDetails()
        {
            ModelBase m = new ModelBase();
            return View(m);
        }
    }
}