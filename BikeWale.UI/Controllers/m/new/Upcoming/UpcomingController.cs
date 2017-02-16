using Bikewale.Entities.BikeData;
using Bikewale.Interfaces.BikeData.UpComing;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Bikewale.Controllers.Mobile.Upcoming
{
    /// <summary>
    /// Created By :- Subodh Jain 16 Feb 2017
    /// Summary :- Desktop upcoming controller
    /// </summary>
    public class UpcomingController : Controller
    {
        private readonly IUpcoming _upcoming = null;

        public UpcomingController(IUpcoming upcoming)
        {
            _upcoming = upcoming;
        }
        [Route("m/upcoming/")]
        public ActionResult Index(int? makeId)
        {
            UpcomingBikesListInputEntity objFiler = null;

            if (makeId.HasValue && makeId.Value > 0)
                objFiler.MakeId = makeId.Value;
            IEnumerable<UpcomingBikeEntity> objUpcomingBikes = _upcoming.GetModels(objFiler, EnumUpcomingBikesFilter.Default);
            return View("~/Views/m/Shared/_UpcomingBikes.cshtml", objUpcomingBikes);
        }
    }
}