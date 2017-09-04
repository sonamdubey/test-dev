using Bikewale.Models;
using System.Web.Mvc;

namespace Bikewale.Controllers
{
    public class PopUpController : Controller
    {
        // GET: PopUp
        [Route("m/popup/AMP/")]
        public ActionResult Index(string queryString)
        {
            PoupCityAreaVM objPopupCityAreaVM = new PoupCityAreaVM();

            return View("~/Views/PopUp/PopUpCityArea_Mobile.cshtml", objPopupCityAreaVM);
        }
    }
}