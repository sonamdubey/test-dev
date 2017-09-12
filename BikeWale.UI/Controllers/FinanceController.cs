using Bikewale.Entities.Dealer;
using Bikewale.Models.Finance;
using System.Web.Mvc;

namespace Bikewale.Controllers
{
    /// <summary>
    /// Controller for Finance modules
    /// </summary>
    /// <author>
    /// Sangram Nandkhile on 08-Sep-2017
    /// 
    /// </author>
    public class FinanceController : Controller
    {
        #region Capital finance

        /// <summary>
        /// index mobile for capital first
        /// Sangram Nandkhile on 08-Sep-2017
        /// </summary>
        /// <returns></returns>
        [Route("m/finance/capitalfirst/")]
        public ActionResult CapitalFirst_Index_Mobile()
        {
            CapitalFirstVM viewModel = new CapitalFirstVM();
            viewModel.ObjLead = new ManufacturerLeadEntity();
            viewModel.objLeadJson = Newtonsoft.Json.JsonConvert.SerializeObject(viewModel.ObjLead);
            return View(viewModel);
        }

    

        /// <summary>
        /// index desktop for capital first
        /// Sangram Nandkhile on 08-Sep-2017
        /// </summary>
        /// <returns></returns>
        [Route("finance/capitalfirst/")]
        public ActionResult CapitalFirst_Index()
        {
            CapitalFirstVM viewModel = new CapitalFirstVM();
            viewModel.ObjLead = new ManufacturerLeadEntity();
            viewModel.objLeadJson = Newtonsoft.Json.JsonConvert.SerializeObject(viewModel.ObjLead);
            return View(viewModel);
        }

        #endregion
    }
}
