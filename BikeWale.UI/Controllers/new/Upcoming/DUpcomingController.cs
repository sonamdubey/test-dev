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
        public ActionResult Index(int? makeId)
        {
            UpcomingBikesListInputEntity objFiler = null;

            if (makeId.HasValue && makeId.Value > 0)
                objFiler.MakeId = makeId.Value;
            IEnumerable<UpcomingBikeEntity> objUpcomingBikes = _upcoming.GetModels(objFiler, EnumUpcomingBikesFilter.Default);

            return View("~/Views/Shared/_UpcomingBikes.cshtml", objUpcomingBikes);
        }
    }
}