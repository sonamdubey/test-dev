using Bikewale.Entities.BikeData;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.BikeData.NewLaunched;
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
        private readonly IBikeModelsCacheRepository<int> _bikeModels = null;
        private readonly INewBikeLaunchesBL _newLaunches = null;

        public HomePageController(IBikeMakes<BikeMakeEntity, int> bikeMakes, IBikeModelsCacheRepository<int> bikeModels, INewBikeLaunchesBL newLaunches)
        {
            _bikeMakes = bikeMakes;
            _bikeModels = bikeModels;
            _newLaunches = newLaunches;
        }
        // GET: HomePage
        [Route("homepage/")]
        public ActionResult Index()
        {
            HomePageVM objData = null;
            HomePageModel obj = new HomePageModel(10, 9, _bikeMakes, _newLaunches);

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
            HomePageModel obj = new HomePageModel(10, 9, _bikeMakes, _newLaunches);

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
