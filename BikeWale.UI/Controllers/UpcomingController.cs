using Bikewale.Interfaces.BikeData.NewLaunched;
using Bikewale.Entities.BikeData;
using Bikewale.Filters;
using Bikewale.Interfaces.BikeData.UpComing;
using Bikewale.Models;
using System.Web.Mvc;

namespace Bikewale.Controllers
{
    public class UpcomingController : Controller
    {
        private IUpcoming _upcoming = null;
        private readonly INewBikeLaunchesBL _newLaunches = null;
        public UpcomingController(IUpcoming upcoming, INewBikeLaunchesBL newLaunches)
        {
            _upcoming = upcoming;
            _newLaunches = newLaunches;
        }
        // GET: UpcomingBikes
        [Route("upcomingbikes/")]
        [DeviceDetection]
        public ActionResult Index(ushort? pageNumber)
        {
            UpcomingPageModel objData = new UpcomingPageModel(10, _upcoming, _newLaunches);
            if (pageNumber.HasValue)
            {
                objData = new UpcomingPageModel(_upcoming, (ushort)pageNumber, _newLaunches);
            }
            else
            {
                objData = new UpcomingPageModel(_upcoming, 1, _newLaunches);
            }
            objData.Filters = new UpcomingBikesListInputEntity();
            objData.Filters.StartIndex = 0;
            objData.Filters.EndIndex = 15;
            objData.SortBy = EnumUpcomingBikesFilter.LaunchDateSooner;
            objData.BaseUrl = "/upcoming-bikes/";
            objData.PageSize = 15;
            UpcomingPageVM objVM = objData.GetData();
            return View(objVM);
        }

        // GET: UpcomingBikes
        [Route("m/upcomingbikes/")]
        public ActionResult Index_Mobile()
        {
            ModelBase m = new ModelBase();
            return View(m);
        }

        // GET: UpcomingBikes by Make
        [Route("m/upcomingbikes/make/")]
        public ActionResult UpcomingBikesByMake_Mobile()
        {
            ModelBase m = new ModelBase();
            return View(m);
        }
    }
}