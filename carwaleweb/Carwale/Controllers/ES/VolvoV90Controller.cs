using Carwale.BL.PaymentGateway;
using Carwale.DAL.PaymentGateway;
using Carwale.Entity.Customers;
using Carwale.Entity.Enum;
using Carwale.Entity.ES;
using Carwale.Entity.Notifications;
using Carwale.Entity.PaymentGateway;
using Carwale.Entity.Template;
using Carwale.Interfaces;
using Carwale.Interfaces.Deals;
using Carwale.Interfaces.ES;
using Carwale.Interfaces.Notifications;
using Carwale.Interfaces.PaymentGateway;
using Carwale.Notifications;
using Carwale.Notifications.Logs;
using Carwale.Notifications.MailTemplates;
using Carwale.UI.ClientBL;
using Carwale.UI.Common;
using Carwale.UI.Filters;
using Carwale.Utility;
using System;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.SessionState;
using AEPLCore.Cache.Interfaces;

namespace Carwale.UI.Controllers.ES
{     
    public class VolvoV90Controller : Controller
    {
        private readonly ITransaction _transaction;
        private readonly IBookingRepository _esInquiry;
        private readonly IDealsNotification _dealsNotification;
        private readonly ISMSNotifications _smsNotification;
        private readonly ICacheManager _cacheMgr;
        private static readonly string[] _bccEmailIds = ((ConfigurationManager.AppSettings["VolvoV90BccMailIds"] ?? "").Split(',')).ToArray();
        private static readonly string _uniqueTransactionId = ConfigurationManager.AppSettings["OfferUniqueTransaction"];             
        public VolvoV90Controller(ITransaction transaction, IBookingRepository esInquiry, IDealsNotification dealsNotification, ISMSNotifications smsNotification, ICacheManager cacheMgr)
        {
            _transaction = transaction;
            _esInquiry = esInquiry;
            _dealsNotification = dealsNotification;
            _smsNotification = smsNotification;
            _cacheMgr = cacheMgr;
        }

        [Route("volvov90specials")]
        public ActionResult Index(bool isApp=false)
        {
            ViewBag.isMobile = false;
            if (DeviceDetectionManager.IsMobile(this.HttpContext))
            {
                ViewBag.isMobile = true;
            }
            ViewBag.isApp = isApp;            

            return View("~/Views/ES/VolvoV90Spotlight.cshtml");
        }

        [Route("v90-cross-country/confirmation/")]
        public ActionResult Confirmation(bool isApp = false)
        {
            ViewBag.isMobile = false;
            if (DeviceDetectionManager.IsMobile(this.HttpContext))
            {
                ViewBag.isMobile = true;
            }
            ViewBag.isApp = isApp;
            var strInquiryId = PGCookie.PGCarId;
            int inquiryId;
            Int32.TryParse(strInquiryId, out inquiryId);

            var customerInquiry = new EsInquiry();
            customerInquiry.Id = inquiryId;
            EsBookingSummary bookingSummary = _esInquiry.GetBookingSummary(inquiryId);            
            if (bookingSummary != null)
            {
                if (bookingSummary.PaymentMode == 2)
                {
                    customerInquiry.IsTransactionCompleted = true;
                    if (!bookingSummary.IsSuccess)
                    {
                        bookingSummary.IsSuccess = true;
                        _esInquiry.SaveEsInquiry(customerInquiry);
                        _esInquiry.GetSetCarCount(bookingSummary.VersionId, bookingSummary.ExteriorColorId, bookingSummary.InteriorColorId, false);
                        _cacheMgr.ExpireCache("booking_version_details_" + bookingSummary.ModelId);
                        SendSms(bookingSummary);
                        SendMailToCustomer(bookingSummary);
                        PushLeadToAutobiz(bookingSummary, inquiryId);
                    }
                }
                else if (bookingSummary.PaymentMode == 4 && (Convert.ToInt16(PGCookie.PGRespCode) == Convert.ToInt16(BillDeskTransactionStatusCode.Successfull)) && bookingSummary.IsTransactionCompleted)
                {
                    customerInquiry.IsTransactionCompleted = true;
                    customerInquiry.TransactionId = String.Format("{0}{1}",_uniqueTransactionId,PGCookie.PGTransId);
                    bookingSummary.TransactionId = customerInquiry.TransactionId; // _uniqueTransactionId + PGCookie.PGTransId;
                    if (!bookingSummary.IsSuccess)
                    {
                        bookingSummary.IsSuccess = true;
                        _esInquiry.SaveEsInquiry(customerInquiry);
                        _esInquiry.GetSetCarCount(bookingSummary.VersionId, bookingSummary.ExteriorColorId, bookingSummary.InteriorColorId, false);
                        _cacheMgr.ExpireCache("booking_version_details_" + bookingSummary.ModelId);
                        SendSms(bookingSummary);
                        SendMailToCustomer(bookingSummary);
                        PushLeadToAutobiz(bookingSummary, inquiryId);
                    }
                }
                if (!bookingSummary.IsSuccess)
                {
                    SendMailToCustomer(bookingSummary);
                    SendSms(bookingSummary);
                }
                return View("~/Views/ES/VolvoV90SpotlightConfirmation.cshtml", bookingSummary);
            }
            else
            {
                return Redirect("/volvo-cars/v90-cross-country/booking/");
            }
            
        }

        [HttpPost, Route("es/booking/volvo-v90/")]
        public void InitPayment(EsInquiry customerInquiry)
        {
            string _confirmationUrl = "/volvo-cars/v90-cross-country/confirmation/";
            string _landingUrl = "/volvo-cars/v90-cross-country/booking/";
            var blockingAmount = Convert.ToUInt16(ConfigurationManager.AppSettings["VolvoV90BookingAmount"].ToString());

            if (customerInquiry.IsApp)
            {
                _confirmationUrl += "?isapp=true";
                _landingUrl += "?isapp=true";
            }

            var customer = _esInquiry.GetEsCustomer(customerInquiry.CustomerId);
            if (customerInquiry.CustomerId == -1 || customer == null || customer.Name != PGCookie.CustomerName || customer.Email != PGCookie.CustomerEmail || customer.Mobile != PGCookie.CustomerMobile)
                Response.Redirect(_landingUrl); //change it with spotlight page url

            int inquiryId = 0;
            string transresp = string.Empty;
            // 4 online, 2 cheque 
            customerInquiry.BookingAmount = blockingAmount;
            customerInquiry.IsTransactionCompleted = false;
            inquiryId = _esInquiry.SaveEsInquiry(customerInquiry);  //create es inquiry
            PGCookie.ResponseId = inquiryId.ToString();                         
            PGCookie.PGResponseUrl = _confirmationUrl;
            PGCookie.PGCarId = inquiryId.ToString();
            ExpirePDCookie();

            if(inquiryId > 0 && customerInquiry.PaymentType == 2)
            {               

                Response.Redirect(_confirmationUrl);
            }
            else if (inquiryId > 0 && customerInquiry.PaymentType == 4)
            {
                var transaction = new TransactionDetails()
                {
                    CustomerID = Convert.ToUInt64(inquiryId),
                    PackageId = (int)OfferPackage.ESVolvoV90Booking,
                    ConsumerType = 0,  // consumerType is 1 for dealer and 0 for Indivisual
                    Amount = blockingAmount,
                    ClientIP = HttpContext.Request.ServerVariables["REMOTE_ADDR"],
                    UserAgent = HttpContext.Request.ServerVariables["HTTP_USER_AGENT"],
                    PGId = Convert.ToUInt64(inquiryId),
                    CustomerName = PGCookie.CustomerName,
                    CustEmail = PGCookie.CustomerEmail,
                    CustMobile = PGCookie.CustomerMobile,
                    CustCity = PGCookie.CustomerCity,
                    PlatformId = customerInquiry.PlatformId,
                    ApplicationId = 1, //Carwale
                    RequestToPGUrl = "https://" + HttpContext.Request.ServerVariables["HTTP_HOST"] + "/new/RedirectToBillDesk.aspx",
                    ReturnUrl = "https://" + HttpContext.Request.ServerVariables["HTTP_HOST"] + "/new/billdeskresponse.aspx?sourceId=1",
                    SourceId = 3   //BillDesk
                };
                PGCookie.PGAmount = transaction.Amount.ToString();
                PGCookie.PGCarId = transaction.PGId.ToString();
                PGCookie.PGPkgId = transaction.PackageId.ToString();
                PGCookie.PGRespCode = "";
                PGCookie.PGMessage = "";

                transresp = _transaction.BeginTransaction(transaction);

                if (transresp == "Transaction Failure" || transresp == "Invalid information!")
                {
                    Response.Redirect("https://" + Request.ServerVariables["HTTP_HOST"].ToString() + _landingUrl);
                }
                Response.Redirect("https://" + Request.ServerVariables["HTTP_HOST"].ToString() + _landingUrl);
            }
            else
            {
                Response.Redirect("https://" + Request.ServerVariables["HTTP_HOST"].ToString() + _landingUrl);
            }
            Response.Redirect("https://" + Request.ServerVariables["HTTP_HOST"].ToString() + _landingUrl);
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

        [Route("es/booking/volvo-v90/sendmail/")]
        public void SendMailToCustomer(EsBookingSummary bookingSummary)
        {
            EmailEntity email = new EmailEntity();
            if (bookingSummary.IsSuccess && bookingSummary.PaymentMode == 4)
            {
                email = new ESBookingReceiptTemplate().GetOnlineBookingSuccessTemplate(bookingSummary);
            }
            else if(!bookingSummary.IsSuccess && bookingSummary.PaymentMode == 4)
            {
                email = new ESBookingReceiptTemplate().GetOnlineBookingFailureTemplate(bookingSummary);
            }
            else if(bookingSummary.PaymentMode == 2)
            {
                email = new ESBookingReceiptTemplate().GetChequeBookingSuccessTemplate(bookingSummary);
            }
            if(!string.IsNullOrEmpty(email.Body) && !string.IsNullOrEmpty(email.Subject) && !string.IsNullOrEmpty(email.Email))
                new Email().SendMail(email.Email, email.Subject, email.Body, null, null, _bccEmailIds);
        }

        public void SendSms(EsBookingSummary bookingSummary)
        {
            SMS sms = null;
            string message ="";
            if (bookingSummary.IsSuccess)
                message = "Congratulations! You've successfully submitted a booking request for Volvo V90 Cross Country. We'll send you a confirmation e-mail to confirm the booking.";            
            else
                message = "Sorry! Your request to book the Volvo V90 Cross Country was not completed. We'll call you back within 24 hrs to help you complete the booking.";
            sms = new SMS()
            {
                Message = message,
                Mobile = bookingSummary.CustomerMobile,
                ReturnedMsg = "",
                Status = true,
                SMSType = (int)SMSType.CustomSMS,
                PageUrl = ""
            };
            _smsNotification.ProcessSMS(sms);
        }

        public object CreateAutobizInquiry(EsBookingSummary esInquiryDetails, int inquiryId)
        {
            string paymentMode = esInquiryDetails.PaymentMode == 2 ? "Cheque" : "Online";
            string transactionStatus = esInquiryDetails.IsSuccess ? "Successful" : "Failure";

            object esInquiry = new
            {
                InquiryId = inquiryId,
                CustomerName = esInquiryDetails.CustomerName,
                CustomerEmail = esInquiryDetails.CustomerEmail,
                CustomerMobile = esInquiryDetails.CustomerMobile,
                CustomerAddress = esInquiryDetails.Address,
                VersionId = esInquiryDetails.VersionId,
                InqDate = esInquiryDetails.InquiryDate,
                Comments = string.Format("Exterior Color: {0} , Interior Color: {1} , Payment Type: {2} , Transaction Status: {3} , Transaction Reference Id: {4}", esInquiryDetails.ExteriorColor, esInquiryDetails.InteriorColor, paymentMode, transactionStatus, esInquiryDetails.TransactionId),
                BranchId = 25299,
                IsPaymentSuccess = esInquiryDetails.IsSuccess,
                CityId = esInquiryDetails.CityId,
                InquirySourceId = 1
            };
            return esInquiry;
        }

        private void PushLeadToAutobiz(EsBookingSummary bookingSummary, int inquiryId)
        {
            object esInquiry = CreateAutobizInquiry(bookingSummary, inquiryId);
            string resContent = "-1";
            string apiUrl = ConfigurationManager.AppSettings["DealsApiHostUrl"];
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(apiUrl);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    using (HttpResponseMessage response = client.PostAsJsonAsync("webapi/NewCarInquiries/Post/", esInquiry).Result)
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            resContent = response.Content.ReadAsStringAsync().Result;
                            if (!string.IsNullOrWhiteSpace(resContent))
                            {
                                resContent = resContent.Replace("\\", "").Trim(new char[1] { '"' });
                                PushAutobizBookingInquiry(bookingSummary, resContent);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }

        public object CreateAutobizBookingInquiry(string resContent, DateTime inquiryDate, int bookingAmount)
        {
            object esBookingInquiry = new
            {
                InquiryId = resContent,
                BookingDate = inquiryDate,
                PaymentAmount = bookingAmount,
                BranchId = 25299,
            };
            return esBookingInquiry;
        }

        private void PushAutobizBookingInquiry(EsBookingSummary bookingSummary, string resContent)
        {
            object esBookingInquiry = CreateAutobizBookingInquiry(resContent, bookingSummary.InquiryDate, bookingSummary.BookingAmount);
            string apiUrl = ConfigurationManager.AppSettings["DealsApiHostUrl"];
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(apiUrl);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    using (HttpResponseMessage response = client.PostAsJsonAsync("webapi/booking/", esBookingInquiry).Result)
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            resContent = response.Content.ReadAsStringAsync().Result;                            
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }

        [Route("v90-cross-country/termsconditions/")]
        public ActionResult Terms()
        {
            ViewBag.isMobile = false;
            if (DeviceDetectionManager.IsMobile(this.HttpContext))
            {
                ViewBag.isMobile = true;
            }
            return View("~/Views/ES/VolvoV90SpotlightTermsConditions.cshtml");
        }
    }
}