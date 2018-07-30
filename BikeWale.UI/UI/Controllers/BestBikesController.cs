using System.Web.Mvc;

namespace Bikewale.Controllers
{

    /// <summary>
    /// </summary>
    /// <seealso cref="System.Web.Mvc.Controller" />
    /// <author>
    /// Created by: Sangram Nandkhile on 24-Mar-2017
    /// Summary: Controller which holds actions for 
    /// </author>
    public class BestBikesController : Controller
    {
        // GET: BestBikes
        public ActionResult Index()
        {
            return View();
        }
    }
}