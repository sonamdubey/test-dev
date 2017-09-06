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
            objPopupCityAreaVM= obj.GetData(queryString);

            return View("~/Views/PopUp/PopUpCityArea_Mobile.cshtml", objPopupCityAreaVM);
        }



        /// <summary>
        /// Author: Sangram Nandkhile on 05 Sep 2017.
        /// Summary: Indexes the lead capture.
        /// </summary>
        [Route("m/popup/leadcapture/")]
        public ActionResult Index_LeadCapture()
        {
            string q = Request.QueryString.ToString();
            PopupLeadCaptureModel objLead = new PopupLeadCaptureModel(q);
            PopupLeadCaptureVM viewModel = objLead.GetData();
            return View(viewModel);
        }

    }
}