using BikewaleOpr.Interface.AdSlot;
using BikewaleOpr.Models.AdSlot;
using System.Web.Mvc;

namespace BikewaleOpr.Controllers
{
    [Authorize]
    /// <summary>
    /// Created by : Ashutosh Sharma on 30 Oct 2017
    /// Description : Controller for AdSlot page.
    /// </summary>
    /// <returns></returns>
    public class AdSlotController : Controller
    {
        private readonly IAdSlotRepository _AdSlotRepository = null;
        public AdSlotController(IAdSlotRepository AdSlotRepository)
        {
            _AdSlotRepository = AdSlotRepository;
        }

        /// <summary>
        /// Created by : Ashutosh Sharma on 30 Oct 2017
        /// Description : Actions Method for AdSlot page.
        /// </summary>
        /// <returns></returns>
        // GET: AdSlot
        [Route("adslots/")]
        public ActionResult Index()
        {
            AdSlotsPage objAdSlotPage = new AdSlotsPage(_AdSlotRepository);
            return View(objAdSlotPage.GetData());
        }
    }
}
