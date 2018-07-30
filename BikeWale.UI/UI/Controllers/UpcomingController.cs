using Bikewale.Entities.BikeData;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.BikeData.NewLaunched;
using Bikewale.Interfaces.BikeData.UpComing;
using Bikewale.Interfaces.CMS;
using Bikewale.Models;
using Bikewale.Models.Upcoming;
using System.Web.Mvc;

namespace Bikewale.Controllers
{
    /// <summary>
    /// Created By Sajal Gupta on 10-04-2017
    /// Description : Contolller to fetch upcoming pages.
    /// </summary>
    public class UpcomingController : Controller
    {
        private IUpcoming _upcoming = null;
        private readonly INewBikeLaunchesBL _newLaunches = null;
        private readonly IBikeMakesCacheRepository _bikeMakesCache = null;
        private readonly IBikeModels<BikeModelEntity, int> _bikeModels = null;
        private readonly ICMSCacheContent _objArticles = null;
        public UpcomingController(IUpcoming upcoming, INewBikeLaunchesBL newLaunches, IBikeMakesCacheRepository bikeMakesCache, IBikeModels<BikeModelEntity, int> bikeModels, ICMSCacheContent objArticles)
        {
            _upcoming = upcoming;
            _newLaunches = newLaunches;
            _bikeMakesCache = bikeMakesCache;
            _bikeModels = bikeModels;
            _objArticles = objArticles;
        }

        /// <summary>
        /// Created By Sajal Gupta on 10-04-2017
        /// Description : GEt UpcomingBikes
        /// </summary>       
        [Route("upcomingbikes/")]
        [Bikewale.Filters.DeviceDetection]
        public ActionResult Index(ushort? pageNumber, EnumUpcomingBikesFilter? sort)
        {
            UpcomingPageModel objData = null;
            objData = new UpcomingPageModel(10, pageNumber, 15, _upcoming, _newLaunches, "/upcoming-bikes/", _objArticles);

            if (sort.HasValue)
                objData.SortBy = sort.Value;

            UpcomingPageVM objVM = objData.GetData();
            return View(objVM);
        }

        /// <summary>
        /// Created By Sajal Gupta on 10-04-2017
        /// Description : GEt UpcomingBikes
        /// </summary>          
        [Route("m/upcomingbikes/")]
        public ActionResult Index_Mobile(ushort? pageNumber, EnumUpcomingBikesFilter? sort)
        {
            UpcomingPageModel objData = null;

            objData = new UpcomingPageModel(9, pageNumber, 10, _upcoming, _newLaunches, "/m/upcoming-bikes/", _objArticles);

            if (sort.HasValue)
                objData.SortBy = sort.Value;
            objData.IsMobile = true;
            UpcomingPageVM objVM = objData.GetData();
            return View(objVM);
        }

        /// <summary>
        /// Created By Sajal Gupta on 10-04-2017
        /// Description : GEt UpcomingBikes by make 
        /// </summary>   
        [Route("upcomingbikes/make/{maskingName}/")]
        [Bikewale.Filters.DeviceDetection]
        public ActionResult BikesByMake(string maskingName, ushort? pageNumber, EnumUpcomingBikesFilter? sort)
        {
            string baseUrl = string.Format("/{0}-bikes/upcoming/", maskingName);
            UpcomingByMakePageModel objData = new UpcomingByMakePageModel(maskingName, _upcoming, pageNumber, 15, _bikeModels, baseUrl, _bikeMakesCache);

            if (objData.Status == Entities.StatusCodes.ContentFound)
            {
                if (sort.HasValue)
                    objData.SortBy = sort.Value;

                objData.topbrandCount = 10;
                UpcomingPageVM objVM = objData.GetData();

                if (objVM.TotalBikes > 0)
                    return View(objVM);
                else
                    return HttpNotFound();
            }
            else if (objData.Status == Entities.StatusCodes.RedirectPermanent)
            {
                return Redirect(objData.RedirectUrl);
            }
            else
            {
                return HttpNotFound();
            }
        }

        /// <summary>
        /// Created By Sajal Gupta on 10-04-2017
        /// Description : GEt UpcomingBikes by make 
        /// </summary>   
        [Route("m/upcomingbikes/make/{maskingName}/")]
        public ActionResult BikesByMake_Mobile(string maskingName, ushort? pageNumber, EnumUpcomingBikesFilter? sort)
        {
            string baseUrl = string.Format("/m/{0}-bikes/upcoming/", maskingName);
            UpcomingByMakePageModel objData = new UpcomingByMakePageModel(maskingName, _upcoming, pageNumber, 10, _bikeModels, baseUrl, _bikeMakesCache);

            if (objData.Status == Entities.StatusCodes.ContentFound)
            {
                if (sort.HasValue)
                    objData.SortBy = sort.Value;

                objData.topbrandCount = 9;
                objData.IsMobile = true;
                UpcomingPageVM objVM = objData.GetData();

                if (objVM.TotalBikes > 0)
                    return View(objVM);
                else
                    return HttpNotFound();
            }
            else if (objData.Status == Entities.StatusCodes.RedirectPermanent)
            {
                return Redirect(objData.RedirectUrl);
            }
            else
            {
                return HttpNotFound();

            }
        }
    }
}