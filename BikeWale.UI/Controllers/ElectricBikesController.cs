using System.Web.Mvc;

namespace Bikewale.Controllers
{
    public class ElectricBikesController : Controller
    {
        // GET: ElectricBikes
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Index_Mobile()
        {
            return View();
        }

    }
}