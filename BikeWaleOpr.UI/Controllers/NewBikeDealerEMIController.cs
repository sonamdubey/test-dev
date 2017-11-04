
using BikewaleOpr.Entities;
using BikewaleOpr.Interface;
using BikewaleOpr.Interface.Location;
using BikewaleOpr.Models.DealerEMI;
using BikeWaleOpr.Common;
using System;
using System.Web.Mvc;

namespace BikewaleOpr.Controllers
{
    /// <summary>
    /// Created by : Ashutosh Sharma on 05-Aug-2017
    /// Description : Controller for Manage EMI page.
    /// </summary>
    [Authorize]
    public class NewBikeDealerEMIController : Controller
    {
        private IDealers _dealer = null;
        private readonly ILocation _location = null;

        public NewBikeDealerEMIController(IDealers dealer, ILocation locationObject)
        {
            _dealer = dealer;
            _location = locationObject;
        }

        /// <summary>
        /// Created by : Ashutosh Sharma on 05-Aug-2017
        /// Description : Action method for default view of Manage EMI page.
        /// </summary>
        /// <param name="dealerId"></param>
        /// <returns></returns>
        [HttpGet, Route("dealers/{dealerId}/emi/")]
        public ActionResult Index(uint dealerId, uint? cityId, uint? makeId, string dealerName = null, ushort isAdded = 0)
        {
            DealerEMIModel dealerEmiModel = new DealerEMIModel(_location, _dealer);
            DealerEMIVM dealerEmiPageInfo = null;

            try
            {
                dealerEmiPageInfo = dealerEmiModel.GetDealerEmiInfo(dealerId, cityId.Value, makeId.Value, dealerName);
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("NewBikeDealerEMIController.Index_{0}", dealerId));
            }
            return View(dealerEmiPageInfo);
        }

        /// <summary>
        /// Created by : Ashutosh Sharma on 05/08/2017
        /// Description : Action method to submit Manage EMI form.
        /// </summary>
        /// <param name="dealerId"></param>
        /// <param name="emi"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Submit(uint dealerId, EMI emi, uint cityId, uint makeId, string dealerName, ushort isAdded = 0)
        {
            
                _dealer.SaveDealerEMI(dealerId, emi.MinDownPayment, emi.MaxDownPayment, emi.MinTenure, emi.MaxTenure
                                    , emi.MinRateOfInterest, emi.MaxRateOfInterest, emi.MinLoanToValue, emi.MaxLoanToValue
                                    , emi.LoanProvider, emi.ProcessingFee, emi.Id, Convert.ToUInt32(CurrentUser.Id));

            return Redirect(
                string.Format("/dealers/{0}/emi/?cityId={1}&makeId={2}&dealerName={3}&isAdded={4}", dealerId, cityId, makeId, dealerName, isAdded));
        }
    }
}
