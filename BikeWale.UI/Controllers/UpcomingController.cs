using Bikewale.Common;
using Bikewale.Entities.BikeData;
using Bikewale.Interfaces.BikeData.NewLaunched;
using Bikewale.Interfaces.BikeData.UpComing;
using Bikewale.Models;
using Bikewale.Models.Upcoming;
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
        [Bikewale.Filters.DeviceDetection]
        public ActionResult Index(ushort? pageNumber)
        {
            UpcomingPageModel objData = null;
            if (pageNumber.HasValue)
            {
                objData = new UpcomingPageModel(_upcoming, (ushort)pageNumber, _newLaunches);
            }
            else
            {
                objData = new UpcomingPageModel(_upcoming, 1, _newLaunches);
            }
            UpcomingPageVM objVM = objData.GetData();
            return View(objVM);
        }

        // GET: UpcomingBikes
        [Route("m/upcomingbikes/")]
        public ActionResult Index_Mobile(ushort? pageNumber)
        {
            UpcomingPageModel objData = null;
            if (pageNumber.HasValue)
            {
                objData = new UpcomingPageModel(_upcoming, (ushort)pageNumber, _newLaunches);
            }
            else
            {
                objData = new UpcomingPageModel(_upcoming, 1, _newLaunches);
            }
            objData.Filters = new UpcomingBikesListInputEntity();
            objData.Filters.PageSize = 10;
            objData.SortBy = EnumUpcomingBikesFilter.LaunchDateSooner;
            objData.BaseUrl = "/upcoming-bikes/";
            objData.PageSize = 10;
            objData.topbrandCount = 10;
            UpcomingPageVM objVM = objData.GetData();
            return View(objVM);
        }

        // GET: UpcomingBikes by Make
        [Route("upcomingbikes/make/{maskingName}")]
        [Bikewale.Filters.DeviceDetection]
        public ActionResult BikesByMake(string maskingName, ushort? pageNumber)
        {
            UpcomingByMakePageModel objData = null;
            if (pageNumber.HasValue)
            {
                objData = new UpcomingByMakePageModel(maskingName, _upcoming, (ushort)pageNumber, _newLaunches);
            }
            else
            {
                objData = new UpcomingByMakePageModel(maskingName, _upcoming, 1, _newLaunches);
            }

            if (objData.Status == Entities.StatusCodes.ContentFound)
            {
                objData.Filters = new UpcomingBikesListInputEntity();
                objData.Filters.PageSize = 15;
                objData.Filters.MakeId = (int)objData.MakeId;
                objData.SortBy = EnumUpcomingBikesFilter.LaunchDateSooner;
                objData.BaseUrl = "/upcoming-bikes/";
                objData.PageSize = 15;
                objData.topbrandCount = 10;
                UpcomingPageVM objVM = objData.GetData();
                if (objVM.TotalBikes > 0)
                    return View(objVM);
                else
                    return Redirect(CommonOpn.AppPath + "pageNotFound.aspx");
            }
            else if (objData.Status == Entities.StatusCodes.RedirectPermanent)
            {
                return Redirect(CommonOpn.AppPath + "pageNotFound.aspx");
            }
            else
            {
                return Redirect("pageNotFound.aspx");
            }
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