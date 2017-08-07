
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
    /// Created by : Ashutosh Sharma on 05/08/2017
    /// Discription : Controller for Manage 
    /// </summary>
    public class NewBikeDealerEMIController : Controller
    {
        private IDealers _dealer = null;
        public NewBikeDealerEMIController(IDealers dealer)
        {
            _dealer = dealer;
        }
        
        [Route("newbikebooking/ManageDealerLoanAmounts/")]
        public ActionResult Index(uint? dealerId)
        {
            EMI loanAmount = null;
            try
            {
                loanAmount = new EMI();
                loanAmount = _dealer.GetDealerLoanAmounts(dealerId??0);
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, string.Format("BikewaleOpr.Controllers.NewBikeDealerEMIController_{0}",dealerId));
            }
            return View(loanAmount);
        }

        [HttpPost]
        public ActionResult Submit(uint? dealerId, EMI emi)
        {
            _dealer.SaveDealerEMI(dealerId??0, emi.MinDownPayment, emi.MaxDownPayment, emi.MinTenure, emi.MaxTenure
                            , emi.MinRateOfInterest, emi.MaxRateOfInterest, emi.MinLoanToValue, emi.MaxLoanToValue
                            , emi.LoanProvider, emi.ProcessingFee, emi.Id, Convert.ToUInt32(CurrentUser.Id));
            return RedirectToAction("Index", new { dealerId = dealerId });
;        }
    }
}