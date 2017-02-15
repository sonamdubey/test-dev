using System.Web.Mvc;

namespace Bikewale.Controllers.Desktop.NewLaunches
{
    public class NewLaunchesController : Controller
    {
        [Route("newlaunches/")]
        public ActionResult Index()
        {
            return View("~/views/newlaunches/index.cshtml");
        }
    }
}