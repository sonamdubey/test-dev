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
        private readonly IUpcoming _upcoming = null;

        public DUpcomingController(IUpcoming upcoming)
        {
            _upcoming = upcoming;
        }
        [Route("upcoming/")]
        public ActionResult Index(UpcomingBikesListInputEntity objFilters, EnumUpcomingBikesFilter sortBy)
        {

            IEnumerable<UpcomingBikeEntity> objUpcomingBikes = _upcoming.GetModels(objFilters, sortBy);

            return View("~/Views/Shared/_UpcomingBikes.cshtml", objUpcomingBikes);
        }
    }
}