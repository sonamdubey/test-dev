using Bikewale.Entities.BikeData;
using Bikewale.Interfaces.BikeData;
using Bikewale.Models;
using System.Web.Mvc;

namespace Bikewale.Controllers
{
    /// <summary>
    /// Created by : Sangram Nandkhile on 24 March 2017
    /// Summary: Controller to hold homepage related actions
    /// </summary>
    public class HomePageController : Controller
    {
        private readonly IBikeMakes<BikeMakeEntity, int> _bikeMakes = null;
        public HomePageController(IBikeMakes<BikeMakeEntity, int> bikeMakes)
        {
            _bikeMakes = bikeMakes;
        }
        // GET: HomePage
        [Route("homepage/")]
        public ActionResult Index()
        {
            HomePageVM objData = null;
            HomePageModel obj = new HomePageModel(10, _bikeMakes);

            if (obj.status == Entities.StatusCodes.ContentNotFound)
            {
                return Redirect("/pagenotfound.aspx");
            }
            else if (obj.status == Entities.StatusCodes.RedirectPermanent)
            {
                return RedirectPermanent(obj.redirectUrl);
            }
            else
            {
                objData = obj.GetData();
                return View(objData);
            }
        }

        // GET: HomePage
        [Route("m/homepage/")]
        public ActionResult Index_Mobile()
        {
            HomePageVM objData = null;
            HomePageModel obj = new HomePageModel(10, _bikeMakes);

            if (obj.status == Entities.StatusCodes.ContentNotFound)
            {
                return Redirect("/pagenotfound.aspx");
            }
            else if (obj.status == Entities.StatusCodes.RedirectPermanent)
            {
                return RedirectPermanent(obj.redirectUrl);
            }
            else
            {
                objData = obj.GetData();
                return View(objData);
            }
        }
    }
}
