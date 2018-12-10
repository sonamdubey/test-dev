using Carwale.BL.Deals;
using Carwale.BL.PaymentGateway;
using Carwale.DTOs.Deals;
using Carwale.Entity;
using Carwale.Entity.Enum;
using Carwale.Entity.PaymentGateway;
using Carwale.Entity.Template;
using Carwale.Interfaces;
using Carwale.Interfaces.Deals;
using Carwale.Notifications;
using Carwale.UI.Common;
using Carwale.Utility;
using System;
using System.Configuration;
using System.Web;
using System.Web.Mvc;


namespace Carwale.UI.Controllers.Deals
{
    public class AdvantageInquiriesController : Controller
    {

        private readonly Carwale.Interfaces.PaymentGateway.ITransaction _transaction;
        private readonly IRepository<DealsInquiryDetail> _dealsRepo;
        private readonly IDeals _carDeals;
        private readonly IDealsNotification _dealsNotification;
        private readonly IDealInquiriesCL _dealInquiriesCL;

       

        public AdvantageInquiriesController(Carwale.Interfaces.PaymentGateway.ITransaction transaction, IRepository<DealsInquiryDetail> dealsRepo,
            IDeals carDeals, IDealsNotification dealsNotification, IDealInquiriesCL dealInquiriesCL)
        {
            _transaction = transaction;
            _dealsRepo = dealsRepo;
            _carDeals = carDeals;
            _dealsNotification = dealsNotification;
            _dealInquiriesCL = dealInquiriesCL;
        }

        // GET: DealInquiries
        public ActionResult Index()
        {
            return View();
        }

        // GET: Thank you screen after payment
        [Route("advantage/booking/confirmation")]
        public ActionResult ThankYou()
        {
            DealsStockDTO dealsStockData = null;
            try
            {
                bool paymentSuccess;
                string respcd = string.Empty;
                string transId, sourceIdForAutobiz = ((int)LeadSourceIdForAutobiz.CarwaleDesktop).ToString();

                respcd = PGCookie.PGRespCode;
                transId = PGCookie.PGTransId;

                if (transId != "-1")
                {
                    paymentSuccess = (respcd != string.Empty && Convert.ToInt16(respcd) == Convert.ToInt16(BillDeskTransactionStatusCode.Successfull));

                    dealsStockData = _dealInquiriesCL.ThankYou(respcd, transId, paymentSuccess, sourceIdForAutobiz);

                    if (dealsStockData == null || dealsStockData.StockCount == 0/* || dealsStockData.DealsData[0].CarCount == 0*/)
                        return HttpNotFound();

                    ViewBag.Success = paymentSuccess;
                    ViewBag.TransactionId = PGCookie.PGTransId;                 
                    PGCookie.PGResponseUrl = string.Empty;
                    if (paymentSuccess)
                        PGCookie.PGTransId = "-1";
                }
                else
                    return HttpNotFound();
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "DealInquiriesController.ThankYou()\n Exception : " + ex.Message);
                objErr.LogException();
            }

            return View("~/Views/Deals/DealInquiries.cshtml", dealsStockData);
        }

        //[HttpPost]
        //[Route("deals/pushLead/{stockId:int}")]
        //public void PushLead(int stockId, int cityId, string sourceId, DealsInquiryDetail dealsInquiry)
        //{
        //    model.DealsInquiryDetails.StockId = stockId;
        //    model.DealsInquiryDetails.CityId = cityId;
        //    model.DealsInquiryDetails.CustomerCity = cityId;
        //    model.DealsInquiryDetails.MasterCityId = CookiesCustomers.MasterCityId;
        //    _dealInquiriesCL.PushLead(stockId, cityId, sourceId, model.DealsInquiryDetails, Convert.ToInt32(Platform.CarwaleDesktop));  // '0' is to signify that its desktop site


        //    SetPDCookie();
        //    string[] url = new string[2];
        //    url = HttpContext.Request.UrlReferrer.ToString().Split('?');
        //    Response.Redirect(url[0] + "booking/" + stockId + "/?" + url[1]);

        //}

        [HttpPost]
        [Route("deals/initpayment/{stockId:int}")]
        public void InitPayment(int stockId, DealsInquiryDetail dealsInquiry)
        {
            if (dealsInquiry.CustomerEmail == null || dealsInquiry.CustomerMobile == null || dealsInquiry.CustomerName == null)
            {
                Response.Redirect("https://" + Request.ServerVariables["HTTP_HOST"].ToString() + "/deals/" + stockId + "/booking");
            }
            int inquiryId = 0;
            string transresp = string.Empty;
            Int32.TryParse(PGCookie.ResponseId.ToString(), out inquiryId);
            PGCookie.PGResponseUrl = "/advantage/booking/confirmation";
            ExpirePDCookie();
            if (inquiryId > 0)
            {
                if (!_dealInquiriesCL.UpdateCustInfo(inquiryId, dealsInquiry))
                {
                    Response.Redirect("https://" + Request.ServerVariables["HTTP_HOST"].ToString() + "/deals/" + stockId + "/booking");
                }
                transresp = _dealInquiriesCL.BeginTransaction(dealsInquiry, inquiryId, Convert.ToInt32(Platform.CarwaleDesktop));
                if (transresp == "Transaction Failure" || transresp == "Invalid information!")
                {
                    Response.Redirect("https://" + Request.ServerVariables["HTTP_HOST"].ToString() + "/deals/" + dealsInquiry.StockId + "/booking");
                }
            }
            else
            {
                Response.Redirect("https://" + Request.ServerVariables["HTTP_HOST"].ToString() + "/deals/" + stockId + "/booking");
            }
        }

        [HttpPost, Route("deals/retrypayment/{stockId:int}")]
        public void RetryPayment(int stockId)
        {
            var dealsInquiry = new DealsInquiryDetail() { StockId = stockId, CustomerName = PGCookie.CustomerName, CustomerMobile = PGCookie.CustomerMobile, CustomerEmail = PGCookie.CustomerEmail, CustomerCity = Convert.ToInt32(PGCookie.CustomerCity) };
            InitPayment(stockId, dealsInquiry);
        }

        private void SetPDCookie()
        {
            HttpCookie isRedirectedFromPD = new HttpCookie("isRedirectedFromPD");
            isRedirectedFromPD.Value = "true";
            isRedirectedFromPD.Expires = DateTime.Now.AddMinutes(30);
            HttpContext.Response.Cookies.Add(isRedirectedFromPD);
        }

        private void ExpirePDCookie()
        {
            if (Request.Cookies["isRedirectedFromPD"] != null)
            {
                HttpCookie myCookie = new HttpCookie("isRedirectedFromPD");
                myCookie.Expires = DateTime.Now.AddDays(-1d);
                Response.Cookies.Add(myCookie);
            }
        }
    }
}