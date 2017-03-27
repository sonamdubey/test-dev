using Bikewale.Entities.BikeData;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.BikeData.NewLaunched;
using Bikewale.Interfaces.Location;
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
        private readonly IBikeModels<BikeModelEntity, int> _bikeModels = null;
        private readonly INewBikeLaunchesBL _newLaunches = null;
        private readonly ICityCacheRepository _usedBikeCities = null;

        public HomePageController(IBikeMakes<BikeMakeEntity, int> bikeMakes, INewBikeLaunchesBL newLaunches, IBikeModels<BikeModelEntity, int> bikeModels, ICityCacheRepository usedBikeCities)
        {
            _bikeMakes = bikeMakes;
            _bikeModels = bikeModels;
            _newLaunches = newLaunches;
            _usedBikeCities = usedBikeCities;
        }
        // GET: HomePage
        [Route("homepage/")]
        public ActionResult Index()
        {
            HomePageVM objData = null;
            uint cityId = 0;
            HomePageModel obj = new HomePageModel(cityId, 10, 9, _bikeMakes, _newLaunches, _bikeModels, _usedBikeCities);

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
            uint cityId = 0;
            HomePageModel obj = new HomePageModel(cityId, 10, 9, _bikeMakes, _newLaunches, _bikeModels, _usedBikeCities);

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
