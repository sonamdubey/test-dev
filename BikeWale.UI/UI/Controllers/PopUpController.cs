using Bikewale.Models;
using Bikewale.Models.PopUp;
using System.Web.Mvc;

namespace Bikewale.Controllers
{
    public class PopUpController : Controller
    {
        // GET: PopUp
        [Route("m/popup/")]
        public ActionResult Index(string queryString)
        {
            PoupCityAreaVM objPopupCityAreaVM = new PoupCityAreaVM();
            PopUpCityArea obj = new PopUpCityArea();
            objPopupCityAreaVM = obj.GetData(queryString);

            return View("~/UI/Views/PopUp/PopUpCityArea_Mobile.cshtml", objPopupCityAreaVM);
        }



        /// <summary>
        /// Author: Sangram Nandkhile on 05 Sep 2017.
        /// Summary: Indexes the lead capture.
        /// </summary>
        [Route("m/popup/leadcapture/")]
        public ActionResult Index_LeadCapture(string q, ushort platformId = 0)
        {
            PopupLeadCaptureModel objLead = new PopupLeadCaptureModel(q);
            PopupLeadCaptureVM viewModel = objLead.GetData();
            viewModel.LeadCapture.IsAmp = platformId == 4;
            viewModel.LeadCapture.IsApp = platformId == 3;
            viewModel.LeadCapture.PlatformId = (ushort)(platformId > 0 ? platformId : 2);
            return View(viewModel);
        }

    }
}