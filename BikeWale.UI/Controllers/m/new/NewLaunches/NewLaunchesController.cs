using System.Web.Mvc;

namespace Bikewale.Controllers.Mobile.NewLaunches
{
    public class NewLaunchesController : Controller
    {
        [Route("m/newlaunches/")]
        public ActionResult Index()
        {
            return View("~/views/m/newlaunches/index.cshtml");
        }
    }
}