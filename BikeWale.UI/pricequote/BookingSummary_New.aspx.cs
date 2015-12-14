using Bikewale.Common;
using Bikewale.DTO.BookingSummary;
using Bikewale.DTO.PriceQuote.BikeBooking;
using Bikewale.DTO.PriceQuote.DetailedDealerQuotation;
using Bikewale.Entities.BikeBooking;
using Bikewale.Interfaces.BikeBooking;
using Bikewale.Interfaces.PriceQuote;
using Bikewale.Mobile.PriceQuote;
using Carwale.BL.PaymentGateway;
using Carwale.DAL.PaymentGateway;
using Carwale.Entity.PaymentGateway;
using Carwale.Interfaces.PaymentGateway;
using Microsoft.Practices.Unity;
using System;
using System.Configuration;
using System.Web;
using System.Web.UI.WebControls;
using System.Linq;
using System.Web.UI.HtmlControls;

namespace Bikewale.BikeBooking
{
    public class BookingSummary_New : System.Web.UI.Page
    {

        protected PQCustomerDetail objCustomer = null;
        protected uint dealerId = 0, versionId = 0, cityId = 0, pqId = 0, areaId = 0, versionPrice = 0, bookingAmount = 0, insuranceAmount = 0;
        protected string clientIP = String.Empty, pageUrl = String.Empty, bikeName = String.Empty, location = String.Empty;
        protected BookingSummaryBase objBooking = null;
        protected Repeater rptVarients = null, rptVersionColors = null, rptDealerOffers = null, rptPriceBreakup = null;
        protected BikeDealerPriceDetailDTO selectedVarient = null;
        protected DDQDealerDetailBase DealerDetails = null;
        protected bool isOfferAvailable = false, isInsuranceFree = false;
        protected string versionWaitingPeriod = String.Empty, dealerAddress = String.Empty, latitude = "0", longitude = "0";
        protected HtmlInputButton generateNewOTP, deliveryDetailsNextBtn, processOTP;

        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
            deliveryDetailsNextBtn.ServerClick += new EventHandler(btnMakePayment_click);
            generateNewOTP.ServerClick += new EventHandler(btnMakePayment_click);
            processOTP.ServerClick += new EventHandler(btnMakePayment_click);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //device detection
            DeviceDetection dd = new DeviceDetection(Request.ServerVariables["HTTP_X_REWRITE_URL"].ToString());
            dd.DetectDevice();
            ProcessCookie();
            GetVersionNQuotationDetails();

        }

        private void GetVersionNQuotationDetails()
        {
            bool _isContentFound = true;
            try
            {
                //sets the base URI for HTTP requests
                string _abHostUrl = ConfigurationManager.AppSettings["bwHostUrl"];
                string _requestType = "application/json";

                string _apiUrl = String.Format("api/BookingSummary?pqId={0}&versionId={1}&dealerId={2}&cityId={3}", pqId, versionId, dealerId, cityId);
                // Send HTTP GET requests 

                objBooking = BWHttpClient.GetApiResponseSync<BookingSummaryBase>(_abHostUrl, _requestType, _apiUrl, objBooking);

                if (objBooking != null && objBooking.DealerQuotation != null && objBooking.Varients != null)
                {
                    if (objBooking.DealerQuotation.objBookingAmt == null || (objBooking.DealerQuotation.objBookingAmt != null && objBooking.DealerQuotation.objBookingAmt.Amount < 1))
                    {
                        HttpContext.Current.Response.Redirect("http://" + HttpContext.Current.Request.ServerVariables["HTTP_HOST"].ToString() + "/pricequote/detaileddealerquotation.aspx", false);
                        HttpContext.Current.ApplicationInstance.CompleteRequest();
                        this.Page.Visible = false;
                        return;
                    }

                    if (objBooking.Varients != null)
                    {
                        uint data = Convert.ToUInt32((objBooking.Varients).Where(v => v.MinSpec!=null && v.MinSpec.VersionId == versionId).FirstOrDefault().BookingAmount);
                        if (data > 0)
                        {
                           BindVarientDetails();
                        }
                        else
                        {
                            HttpContext.Current.Response.Redirect("http://" + HttpContext.Current.Request.ServerVariables["HTTP_HOST"].ToString() + "/pricequote/detaileddealerquotation.aspx", false);
                            HttpContext.Current.ApplicationInstance.CompleteRequest();
                            this.Page.Visible = false;
                            return;
                        }
                       
                    }

                    if (objBooking.DealerQuotation != null)
                    {
                        GetDealerDetails();
                    }
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
        private void GetDealerDetails()
        {
            if (objBooking != null && objBooking.DealerQuotation != null)
            {
                DealerDetails = objBooking.DealerQuotation;
                //location details
                if (DealerDetails.objDealer != null && DealerDetails.objDealer.objCity != null && !String.IsNullOrEmpty(DealerDetails.objDealer.objCity.CityName))
                {
                    if (DealerDetails.objDealer.objArea != null)
                    {
                        if (!String.IsNullOrEmpty(DealerDetails.objDealer.objArea.AreaName))
                            location = String.Format("{0}, {1}", DealerDetails.objDealer.objArea.AreaName, DealerDetails.objDealer.objCity.CityName);

                        latitude = Convert.ToString(DealerDetails.objDealer.objArea.Latitude);
                        longitude = Convert.ToString(DealerDetails.objDealer.objArea.Longitude);
                    }
                    else
                    {
                        location = DealerDetails.objDealer.objCity.CityName;
                    }
                }

                //Dealer Address
                if (DealerDetails.objDealer != null && !String.IsNullOrEmpty(DealerDetails.objDealer.Address))
                {
                    dealerAddress = String.Format("{0},{1},{2}-{3},{4}.", DealerDetails.objDealer.Address, DealerDetails.objDealer.objArea.AreaName, DealerDetails.objDealer.objCity.CityName, DealerDetails.objDealer.objArea.PinCode, DealerDetails.objDealer.objState.StateName);
                }

                //bind offers provided by dealer
                if (DealerDetails.objDealer != null && DealerDetails.objOffers != null)
                {
                    if (DealerDetails.objOffers != null && DealerDetails.objOffers.Count > 0)
                    {
                        isOfferAvailable = true;
                        rptDealerOffers.DataSource = DealerDetails.objOffers;
                        rptDealerOffers.DataBind();

                    }
                    insuranceAmount = DealerDetails.InsuranceAmount;
                    isInsuranceFree = DealerDetails.IsInsuranceFree;
                }

            }
        }

        private void BindVarientDetails()
        {
            if (versionId > 0 && objBooking != null && objBooking.Varients != null && objBooking.Varients.Count > 0)
            {
                var data = (objBooking.Varients).Where(v => v.BookingAmount > 0);
                rptVarients.DataSource = data;
                rptVarients.DataBind();
            }
        }


        void btnMakePayment_click(object Sender, EventArgs e)
        {
            BeginTransaction("3");
        }


        /// <summary>
        /// 
        /// </summary>
        protected void fetchCustomerDetails()
        {
            using (IUnityContainer container = new UnityContainer())
            {
                container.RegisterType<IDealerPriceQuote, Bikewale.BAL.BikeBooking.DealerPriceQuote>();
                IDealerPriceQuote objDealer = container.Resolve<IDealerPriceQuote>();

                objCustomer = objDealer.GetCustomerDetails(Convert.ToUInt32(PriceQuoteCookie.PQId));
            }
        }

        #region Make payment (Transaction Status)
        /// <summary>
        /// Modified By :   Sumit Kate on 18 Nov 2015
        /// Description :   Save the State of the Booking Journey as Described in Task# 107795062
        /// </summary>
        /// <param name="sourceType"></param>
        protected void BeginTransaction(string sourceType)
        {
            string transresp = string.Empty;
            fetchCustomerDetails();

            if (objCustomer != null && objCustomer.objCustomerBase != null && objCustomer.objCustomerBase.CustomerId > 0)
            {
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

                IPriceQuote _objPriceQuote = null;
                container.RegisterType<IPriceQuote, BAL.PriceQuote.PriceQuote>();
                _objPriceQuote = container.Resolve<IPriceQuote>();
                _objPriceQuote.SaveBookingState(Convert.ToUInt32(PriceQuoteCookie.PQId), Entities.PriceQuote.PriceQuoteStates.InitiatedPayment);

                ITransaction begintrans = container.Resolve<ITransaction>();
                transresp = begintrans.BeginTransaction(transaction);
                Trace.Warn("transresp : " + transresp);

                if (transresp == "Transaction Failure" || transresp == "Invalid information!")
                {
                    HttpContext.Current.Response.Redirect("http://" + HttpContext.Current.Request.ServerVariables["HTTP_HOST"].ToString() + "/pricequote/bookingsummary_new.aspx", false);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                    this.Page.Visible = false;
                }
            }
            else
            {
                HttpContext.Current.Response.Redirect("http://" + HttpContext.Current.Request.ServerVariables["HTTP_HOST"].ToString() + "/pricequote/bookingsummary_new.aspx", false);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
                this.Page.Visible = false;
            }
        }
        #endregion

        #region Private Method to process cookie
        /// <summary>
        /// Checks for the valid PQ Cookie
        /// </summary>
        private void ProcessCookie()
        {
            if (PriceQuoteCookie.IsPQCoockieExist())
            {
                if (UInt32.TryParse(PriceQuoteCookie.PQId, out pqId) && UInt32.TryParse(PriceQuoteCookie.DealerId, out dealerId) && UInt32.TryParse(PriceQuoteCookie.VersionId, out versionId))
                {
                    cityId = Convert.ToUInt32(PriceQuoteCookie.CityId);
                    areaId = Convert.ToUInt32(PriceQuoteCookie.AreaId);

                    if (dealerId > 0)
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