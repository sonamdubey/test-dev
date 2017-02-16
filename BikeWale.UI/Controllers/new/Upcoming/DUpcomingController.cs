using Bikewale.Entities.BikeData;
using Bikewale.Interfaces.BikeData.UpComing;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Bikewale.Controllers.Desktop.Upcoming
{
    /// <summary>
    /// Created By :- Subodh Jain 16 Feb 2017
    /// Summary :- Desktop upcoming controller
    /// </summary>
    public class DUpcomingController : Controller
    {
        private readonly IUpcomingBL _upcoming = null;

        public DUpcomingController(IUpcomingBL upcoming)
        {
            _upcoming = upcoming;
        }
        [Route("upcoming/")]
        public ActionResult Index(int? makeId)
        {
            IEnumerable<UpcomingBikeEntity> objUpcomingBikes = _upcoming.GetUpComingBike(makeId);
            return View("~/Views/Shared/_UpcomingBikes.cshtml", objUpcomingBikes);
        }
    }
}