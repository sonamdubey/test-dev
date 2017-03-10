using Bikewale.BAL.MVC.UI;
using Bikewale.Interfaces.BikeData;
using Bikewale.Models.Shared;
using System.Web.Mvc;


namespace Bikewale.Controllers.Desktop.Scooters
{
    public class ScootersController : Controller
    {
        public readonly IBikeMakesCacheRepository<int> _IScooterCache;
        public ScootersController(IBikeMakesCacheRepository<int> IScooter)
        {
            _IScooterCache = IScooter;
        }
        // GET: Scooters
        [Route("scooters/")]
        public ActionResult Index()
        {
            return View("~/views/scooters/index.cshtml");
        }

        [Route("scooters/make/")]
        public ActionResult BikesByMake()
        {
            return View("~/views/scooters/bikesbymake.cshtml");
        }

        [Route("scooters/brands/")]
        public ActionResult Brands()
        {
            ScooterBrands scooters = new ScooterBrands();
            BrandWidget brands = scooters.GetScooterBrands(_IScooterCache, 10);
            return View("~/views/shared/_brands.cshtml", brands);
        }

        [Route("m/scooters/brands/")]
        public ActionResult BrandsMobile()
        {
            ScooterBrands scooters = new ScooterBrands();
            BrandWidget brands = scooters.GetScooterBrands(_IScooterCache, 6);
            return View("~/views/m/shared/_brands.cshtml", brands);
        }
    }
}