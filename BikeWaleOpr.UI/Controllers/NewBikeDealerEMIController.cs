
using BikewaleOpr.Entities;
using BikewaleOpr.Interface;
using BikeWaleOpr.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BikewaleOpr.Controllers
{
    /// <summary>
    /// Created by : Ashutosh Sharma on 05-Aug-2017
    /// Description : Controller for Manage EMI page.
    /// </summary>
    public class NewBikeDealerEMIController : Controller
    {
        private IDealers _dealer = null;
        public NewBikeDealerEMIController(IDealers dealer)
        {
            _dealer = dealer;
        }

        /// <summary>
        /// Created by : Ashutosh Sharma on 05-Aug-2017
        /// Description : Action method for default view of Manage EMI page.
        /// </summary>
        /// <param name="dealerId"></param>
        /// <returns></returns>
        [Route("newbikebooking/ManageDealerLoanAmounts/{dealerId}")]
        public ActionResult Index(uint dealerId)
        {
            EMI loanAmount = null;
            try
            {
                loanAmount = new EMI();
                loanAmount = _dealer.GetDealerLoanAmounts(dealerId);
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, string.Format("NewBikeDealerEMIController_{0}",dealerId));
            }
            return View(loanAmount);
        }

        /// <summary>
        /// Created by : Ashutosh Sharma on 05/08/2017
        /// Description : Action method to submit Manage EMI form.
        /// </summary>
        /// <param name="dealerId"></param>
        /// <param name="emi"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Submit(uint dealerId, EMI emi)
        {
            _dealer.SaveDealerEMI(dealerId, emi.MinDownPayment, emi.MaxDownPayment, emi.MinTenure, emi.MaxTenure
                            , emi.MinRateOfInterest, emi.MaxRateOfInterest, emi.MinLoanToValue, emi.MaxLoanToValue
                            , emi.LoanProvider, emi.ProcessingFee, emi.Id, Convert.ToUInt32(CurrentUser.Id));
            return RedirectToAction("Index", new { dealerId = dealerId });
;        }
    }
}