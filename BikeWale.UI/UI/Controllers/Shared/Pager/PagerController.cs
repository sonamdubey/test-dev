using Bikewale.Interfaces.Pager;
using System.Web.Mvc;

namespace Bikewale.Controllers.Shared.Pager
{
    public class PagerController : Controller
    {
        private readonly IPager _objPager = null;
        /// <summary>
        /// Constructor to initialize all the dependencies
        /// </summary>
        /// <param name="_cache"></param>
        /// <param name="_genericBike"></param>
        public PagerController(IPager objPager)
        {
            _objPager = objPager;
        }


        public ActionResult Index()
        {
            return PartialView();
        }

        [Route("pager/")]
        public ActionResult Index(Bikewale.Entities.Pager.PagerEntity objPager)
        {
            Bikewale.Models.Shared.Pager objPagerModel = null;

            if (objPager != null)
            {
                objPagerModel = _objPager.GetPagerControl(objPager);
            }

            return PartialView("~/UI/views/pager/pagercontrol.cshtml", objPagerModel);
        }
    }
}