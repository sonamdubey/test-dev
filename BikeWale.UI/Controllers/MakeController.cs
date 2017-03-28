using Bikewale.Interfaces.Dealer;
using Bikewale.Models;
using System.Web.Mvc;

namespace Bikewale.Controllers
{
    /// <summary>
    /// </summary>
    /// <seealso cref="System.Web.Mvc.Controller" />
    /// <author>
    /// Created by: Sangram Nandkhile on 27-Mar-2017
    /// Summary: Controller which holds actions for Make
    /// </author>
    public class MakeController : Controller
    {
        private readonly IDealerCacheRepository _dealerServiceCenters = null;
        public MakeController(IDealerCacheRepository dealerServiceCenters)
        {
            _dealerServiceCenters = dealerServiceCenters;
        }
        // GET: Makes
        [Route("make/")]
        public ActionResult Index()
        {
            uint makeId = 7;
            MakePageModel obj = new MakePageModel(makeId, 9, _dealerServiceCenters);
            MakePageVM objData = new MakePageVM();

            if (obj.Status == Entities.StatusCodes.ContentNotFound)
            {
                return Redirect("/pagenotfound.aspx");
            }
            else if (obj.Status == Entities.StatusCodes.RedirectPermanent)
            {
                return RedirectPermanent(obj.RedirectUrl);
            }
            else
            {
                objData = obj.GetData();
            }
            return View(objData);
        }
    }
}