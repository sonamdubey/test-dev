using Bikewale.Models;
using System.Web.Mvc;

namespace Bikewale.Controllers
{
    public class PopUpController : Controller
    {
        // GET: PopUp
        [Route("m/popup/")]
        public ActionResult Index(string queryString)
        {
            PoupCityAreaVM objPopupCityAreaVM = new PoupCityAreaVM();
            PopUpCityArea obj = new PopUpCityArea();
            objPopupCityAreaVM= obj.GetData(queryString);

            return View("~/Views/PopUp/PopUpCityArea_Mobile.cshtml", objPopupCityAreaVM);
        }
    }
}