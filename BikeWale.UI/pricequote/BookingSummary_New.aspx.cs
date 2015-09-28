using Bikewale.Common;
using Bikewale.DTO.BookingSummary;
using Bikewale.DTO.PriceQuote.BikeBooking;
using Bikewale.DTO.PriceQuote.CustomerDetails;
using Bikewale.Entities.BikeBooking;
using Bikewale.Interfaces.BikeBooking;
using Bikewale.Mobile.PriceQuote;
using Bikewale.Notifications;
using Carwale.BL.PaymentGateway;
using Carwale.DAL.PaymentGateway;
using Carwale.Entity.PaymentGateway;
using Carwale.Interfaces.PaymentGateway;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Bikewale.BikeBooking
{
    public class BookingSummary_New : System.Web.UI.Page
    {
        protected string pageName, description, bikeName, keywords, title;
        protected Button btnMakePayment;
        protected string dealerId, versionId, cityId, pqId, clientIP, pageUrl, areaId, color;
        protected BookingPageDetailsDTO objBookingPageDetailsDTO = null;
        protected BookingSummaryBase objBooking = null;
        protected PQCustomerDetail objCustomer = null;
        bool isDealerNotified = false;
        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
            btnMakePayment.Click += new EventHandler(btnMakePayment_click);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //device detection
            DeviceDetection dd = new DeviceDetection(Request.ServerVariables["HTTP_X_REWRITE_URL"].ToString());
            dd.DetectDevice();
            ProcessCookie();
            GetDetailedQuote();

        }

        void btnMakePayment_click(object Sender, EventArgs e)
        {
            BeginTransaction("3");
        }

        private void GetDetailedQuote()
        {
            bool _isContentFound = true;
            try
            {
                //sets the base URI for HTTP requests
                string _abHostUrl = ConfigurationManager.AppSettings["bwHostUrl"];
                string _requestType = "application/json";

                string _apiUrl = String.Format("api/BookingSummary?pqId={0}&versionId={1}&dealerId={2}&cityId={3}", PriceQuoteCookie.PQId, PriceQuoteCookie.VersionId, PriceQuoteCookie.DealerId, PriceQuoteCookie.CityId);
                // Send HTTP GET requests 

                objBooking = BWHttpClient.GetApiResponseSync<BookingSummaryBase>(_abHostUrl, _requestType, _apiUrl, objBooking);

                if (objBooking != null && objBooking.DealerQuotation != null && objBooking.Customer != null)
                {
                    if (objBooking.DealerQuotation.objBookingAmt == null || (objBooking.DealerQuotation.objBookingAmt != null && objBooking.DealerQuotation.objBookingAmt.Amount == 0))
                    {
                        HttpContext.Current.Response.Redirect("http://" + HttpContext.Current.Request.ServerVariables["HTTP_HOST"].ToString() + "/pricequote/dealerPricequote.aspx",true);
                    }
                    bikeName = String.Format("{0} {1}", objBooking.DealerQuotation.objQuotation.objMake.MakeName, objBooking.DealerQuotation.objQuotation.objModel.ModelName);
                    getCustomerDetails();
                }
                else
                {
                    _isContentFound = false;
                }
            }
            catch (Exception err)
            {
                Trace.Warn(err.Message);
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            finally
            {
                if (!_isContentFound)
                {
                    Response.Redirect("/pagenotfound.aspx", false);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                    this.Page.Visible = false;
                }
            }
        }
        protected void getCustomerDetails()
        {
            using (IUnityContainer container = new UnityContainer())
            {
                container.RegisterType<IDealerPriceQuote, Bikewale.BAL.BikeBooking.DealerPriceQuote>();
                IDealerPriceQuote objDealer = container.Resolve<IDealerPriceQuote>();

                objCustomer = objDealer.GetCustomerDetails(Convert.ToUInt32(PriceQuoteCookie.PQId));
                if (objCustomer.objColor != null)
                    color = objCustomer.objColor.ColorName;
            }
        }

        protected void BeginTransaction(string sourceType)
        {
            string transresp = string.Empty;

            if (objCustomer != null && objCustomer.objCustomerBase != null && objCustomer.objCustomerBase.CustomerId > 0)
            {
                Trace.Warn("Inside begin tarns" + objCustomer.objCustomerBase.CustomerId.ToString());
                var transaction = new TransactionDetails()
                {
                    CustomerID = objCustomer.objCustomerBase.CustomerId,
                    PackageId = (int)Carwale.Entity.Enum.BikeBooking.BikeBooking,
                    ConsumerType = 2,
                    Amount = objBooking.DealerQuotation.objBookingAmt.Amount,
                    ClientIP = CommonOpn.GetClientIP(),
                    UserAgent = HttpContext.Current.Request.ServerVariables["HTTP_USER_AGENT"],
                    PGId = Convert.ToUInt64(PriceQuoteCookie.VersionId),
                    CustomerName = objCustomer.objCustomerBase.CustomerName,
                    CustEmail = objCustomer.objCustomerBase.CustomerEmail,
                    CustMobile = objCustomer.objCustomerBase.CustomerMobile,
                    CustCity = objBooking.Customer.objCustomerBase.CustomerName,
                    PlatformId = 1,  //Desktop
                    ApplicationId = 2, //Carwale
                    RequestToPGUrl = "http://" + HttpContext.Current.Request.ServerVariables["HTTP_HOST"].ToString() + "/bikebooking/RedirectToBillDesk.aspx",
                    ReturnUrl = "http://" + HttpContext.Current.Request.ServerVariables["HTTP_HOST"].ToString() + "/bikebooking/billdeskresponse.aspx?sourceId=1"
                };
                //PGCookie.PGAmount = transaction.Amount.ToString();
                PGCookie.PGCarId = transaction.PGId.ToString();
                //PGCookie.PGPkgId = transaction.PackageId.ToString();
                //PGCookie.PGRespCode = "";
                //PGCookie.PGMessage = "";

                IUnityContainer container = new UnityContainer();
                container.RegisterType<ITransaction, Transaction>()
                .RegisterType<ITransactionRepository, TransactionRepository>()
                .RegisterType<IPackageRepository, PackageRepository>()
                .RegisterType<ITransactionValidator, ValidateTransaction>();

                if (sourceType == "3")
                {
                    container.RegisterType<IPaymentGateway, BillDesk>();
                    transaction.SourceId = Convert.ToInt16(sourceType);
                }

                ITransaction begintrans = container.Resolve<ITransaction>();
                transresp = begintrans.BeginTransaction(transaction);
                Trace.Warn("transresp : " + transresp);

                if (transresp == "Transaction Failure" || transresp == "Invalid information!")
                {
                    HttpContext.Current.Response.Redirect("http://" + HttpContext.Current.Request.ServerVariables["HTTP_HOST"].ToString() + "/pricequote/bookingsummary.aspx");
                    Trace.Warn("fail");
                }
            }
            else
            {
                HttpContext.Current.Response.Redirect("http://" + HttpContext.Current.Request.ServerVariables["HTTP_HOST"].ToString() + "/pricequote/bookingsummary.aspx");
                Trace.Warn("fail");
            }
        }

        #region Private Method
        /// <summary>
        /// Checks for the valid PQ Cookie
        /// </summary>
        private void ProcessCookie()
        {
            if (PriceQuoteCookie.IsPQCoockieExist())
            {
                dealerId = !String.IsNullOrEmpty(PriceQuoteCookie.DealerId) ? PriceQuoteCookie.DealerId : "0";
                versionId = PriceQuoteCookie.VersionId;
                cityId = PriceQuoteCookie.CityId;
                pqId = PriceQuoteCookie.PQId;
                areaId = PriceQuoteCookie.AreaId;
                if (Convert.ToUInt32(dealerId) > 0)
                {
                    clientIP = Bikewale.Common.CommonOpn.GetClientIP();
                    pageUrl = Request.ServerVariables["URL"];
                }
                else
                {
                    Response.Redirect("/", false);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                    this.Page.Visible = false;
                }
            }
            else
            {
                Response.Redirect("/", false);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
                this.Page.Visible = false;
            }
        }
        #endregion
    }
}