using System.Web.Mvc;

namespace BikewaleOpr.Controllers
{
    /// <summary>
    /// Created by : Snehal Dange on 2nd Jan 2018
    /// Descritpion: Controller for Ad Operations(promotion  , monetization) management
    /// </summary>
    [Authorize]
    public class AdOperationController : Controller
    {
        // GET: AdOperation
        public ActionResult Index()
        {
            return View();
        }
    }
}