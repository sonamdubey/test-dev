using System.Web.Mvc;

namespace Bikewale.Controllers
{
    public class PriceController : Controller
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Route("pricequote/")]
        public ActionResult Index()
        {
            return View();
        }

        [Route("pricequote/dealer/")]
        public ActionResult DealerPriceQuoteIndex()
        {
            return View();
        }
    }
}