using System.Web.Mvc;
using Bikewale.Models;
using Bikewale.Filters;

namespace Bikewale.Controllers
{
    public class AdTestController : Controller
    {
        // GET: AdTest
        [Route("bwadtest/"),DeviceDetection]
        public ActionResult Index()
        {
			ModelBase objBase = new ModelBase();			

			return View(objBase);
        }

        [Route("m/bwadtest/")]
        public ActionResult Index_Mobile()
        {
            ModelBase objBase = new ModelBase();

            return View(objBase);
        }
    }
}