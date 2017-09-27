using Bikewale.Models.BikeModels;
using System.Web.Mvc;

namespace Bikewale.Controllers
{
    public class ReviewController : Controller
    {
        // GET: Review
        [Route("m/review/"), Filters.DeviceDetection]
        public ActionResult Index_Mobile()
        {
            ModelPageVM obj = new ModelPageVM();
            return View(obj);
        }
    }
}