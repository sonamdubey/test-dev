using Carwale.Interfaces.SponsoredCar;
using Carwale.UI.ClientBL;
using Carwale.UI.Common;
using Carwale.UI.Filters;
using System.Web.Mvc;

namespace Carwale.UI.Controllers.ES
{
    public class SponsoredCampaignController : Controller
    {
        private readonly ISponsoredCarBL _iSponsoredCampaign;
        public SponsoredCampaignController(ISponsoredCarBL sponsoredCampaign)
        {
            _iSponsoredCampaign = sponsoredCampaign;
        }

        [Carwale.UI.Common.OutputCacheAttr("platform")]
        public ActionResult GetDoodle(int platform)
        {
            var doodleData = _iSponsoredCampaign.GetDoodle(platform);
            if (doodleData != null)
            {
                return PartialView("~/Views/ES/Doodle.cshtml", doodleData);
            }
            return new EmptyResult();
        }
    }
}