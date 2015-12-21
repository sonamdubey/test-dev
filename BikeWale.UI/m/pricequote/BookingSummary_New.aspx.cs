using Bikewale.BAL.BikeBooking;
using Bikewale.Common;
using Bikewale.DTO.BookingSummary;
using Bikewale.DTO.PriceQuote.BikeBooking;
using Bikewale.DTO.PriceQuote.DetailedDealerQuotation;
using Bikewale.Entities.BikeBooking;
using Bikewale.Entities.PriceQuote;
using Bikewale.Interfaces.BikeBooking;
using Bikewale.Interfaces.PriceQuote;
using Carwale.BL.PaymentGateway;
using Carwale.DAL.PaymentGateway;
using Carwale.Entity.PaymentGateway;
using Carwale.Interfaces.PaymentGateway;
using Microsoft.Practices.Unity;
using System;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Bikewale.Mobile.PriceQuote
{
    public class BookingSummary_New : System.Web.UI.Page
    {
        protected uint dealerId = 0, versionId = 0, cityId = 0, pqId = 0, areaId = 0, versionPrice = 0, bookingAmount = 0, insuranceAmount = 0;
        protected string clientIP = String.Empty, pageUrl = String.Empty, bikeName = String.Empty, location = String.Empty;
        protected Repeater rptVarients = null, rptVersionColors = null, rptDealerOffers = null, rptPriceBreakup = null;
        protected BikeDealerPriceDetailDTO selectedVarient = null;
        protected DDQDealerDetailBase DealerDetails = null;
        protected bool isOfferAvailable = false, isInsuranceFree = false;
        protected string versionWaitingPeriod = String.Empty, dealerAddress = String.Empty, latitude = "0", longitude = "0";
        protected HtmlInputButton generateNewOTP, deliveryDetailsNextBtn, processOTP;
        protected BookingPageDetailsEntity objBooking = null;
        protected PQCustomerDetail objCustomer = null;
        protected PQ_DealerDetailEntity dealerDetailEntity = null;

        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
            deliveryDetailsNextBtn.ServerClick += new EventHandler(btnMakePayment_click);
            generateNewOTP.ServerClick += new EventHandler(btnMakePayment_click);
            processOTP.ServerClick += new EventHandler(btnMakePayment_click);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            ProcessCookie();
            BindBookingDetails();

        }

        void btnMakePayment_click(object Sender, EventArgs e)
        {
            BeginTransaction("3");
        }

        #region Bind Bike Booking Details
        /// <summary>
        /// Author  : Sushil Kumar 
        /// Created On : 8th December 2015
        /// Summary : Bind booking details
        /// </summary>
        private void BindBookingDetails()
        {
            bool _isContentFound = true;
            try
            {
                FetchBookingDetails();

                if (objBooking != null)
                {
                    if (objBooking.Varients != null)
                    {
                        uint data = Convert.ToUInt32((objBooking.Varients).Where(v => v.MinSpec != null && v.MinSpec.VersionId == versionId).FirstOrDefault().BookingAmount);
                        if (data > 0)
                        {
                            BindVarientDetails();
                        }
                        else
                        {
                            HttpContext.Current.Response.Redirect("http://" + HttpContext.Current.Request.ServerVariables["HTTP_HOST"].ToString() + "/m/pricequote/detaileddealerquotation.aspx", false);
                            HttpContext.Current.ApplicationInstance.CompleteRequest();
                            this.Page.Visible = false;
                            return;
                        }
                    }

                    GetDealerDetails();
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

        #region Fetch Bikebooking details
        /// <summary>
        /// Author  : Sushil Kumar 
        /// Created On : 12th December 2015
        /// Summary : Fetch Bike Booking Details
        /// </summary>
        private void FetchBookingDetails()
        {
            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IDealerPriceQuote, DealerPriceQuote>();
                    IDealerPriceQuote _objDealerPricequote = container.Resolve<IDealerPriceQuote>();

                    //bookingpagedetails fetch
                    objBooking = _objDealerPricequote.FetchBookingPageDetails(cityId, versionId, dealerId);

                    //customer details
                    objCustomer = _objDealerPricequote.GetCustomerDetails(pqId);

                }
            }
            catch (Exception err)
            {
                Trace.Warn(err.Message);
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }

        }
        #endregion

        #endregion

        #region Bind Dealer Details
        /// <summary>
        /// Author  : Sushil Kumar 
        /// Created On : 12th December 2015
        /// Summary : Fetch and bind dealer details
        /// </summary>
        private void GetDealerDetails()
        {
            //fetch dealer details
            FetchDealerDetails();

            if (dealerDetailEntity != null)
            {
                //set location details
                if (dealerDetailEntity.objDealer != null && dealerDetailEntity.objDealer.objCity != null && !String.IsNullOrEmpty(dealerDetailEntity.objDealer.objCity.CityName))
                {
                    if (dealerDetailEntity.objDealer.objArea != null)
                    {
                        if (!String.IsNullOrEmpty(dealerDetailEntity.objDealer.objArea.AreaName))
                            location = String.Format("{0}, {1}", dealerDetailEntity.objDealer.objArea.AreaName, dealerDetailEntity.objDealer.objCity.CityName);

                        latitude = Convert.ToString(dealerDetailEntity.objDealer.objArea.Latitude);
                        longitude = Convert.ToString(dealerDetailEntity.objDealer.objArea.Longitude);
                    }
                    else
                    {
                        location = dealerDetailEntity.objDealer.objCity.CityName;
                    }
                }

                //Dealer Address
                if (dealerDetailEntity.objDealer != null && !String.IsNullOrEmpty(dealerDetailEntity.objDealer.Address))
                {
                    dealerAddress = String.Format("{0}<br/>{1},{2},{3}-{4},{5}.", dealerDetailEntity.objDealer.Name, dealerDetailEntity.objDealer.Address, dealerDetailEntity.objDealer.objArea.AreaName, dealerDetailEntity.objDealer.objCity.CityName, dealerDetailEntity.objDealer.objArea.PinCode, dealerDetailEntity.objDealer.objState.StateName);
                }

                //bind offers provided by dealer
                if (dealerDetailEntity.objDealer != null && dealerDetailEntity.objOffers != null)
                {
                    BindDealerOffers();
                }

            }
            else
            {
                Response.Redirect("/pricequote/quotation.aspx", false);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
                this.Page.Visible = false;
            }

        }


        #region Bind Dealer offers
        /// <summary>
        /// Author  : Sushil Kumar 
        /// Created On : 12th December 2015
        /// Summary : Bind offers available with dealer
        /// </summary>
        private void BindDealerOffers()
        {
            if (dealerDetailEntity.objOffers != null && dealerDetailEntity.objOffers.Count > 0)
            {
                isOfferAvailable = true;
                rptDealerOffers.DataSource = dealerDetailEntity.objOffers;
                rptDealerOffers.DataBind();

            }
        }
        #endregion

        #region Fetch Dealer details
        /// <summary>
        /// Author  : Sushil Kumar 
        /// Created On : 12th December 2015
        /// Summary : Fetch Dealer details
        /// </summary>
        private void FetchDealerDetails()
        {
            //dealer details
            string _apiUrl = String.Format("/api/Dealers/GetDealerDetailsPQ/?versionId={0}&DealerId={1}&CityId={2}", versionId, dealerId, cityId);

            try
            {
                using (Utility.BWHttpClient objClient = new Utility.BWHttpClient())
                {
                    dealerDetailEntity = objClient.GetApiResponseSync<PQ_DealerDetailEntity>(Utility.APIHost.AB, Utility.BWConfiguration.Instance.APIRequestTypeJSON, _apiUrl, dealerDetailEntity);
                }

                if (dealerDetailEntity != null)
                {

                    if (dealerDetailEntity.objQuotation != null)
                    {
                        foreach (var price in dealerDetailEntity.objQuotation.PriceList)
                        {
                            isInsuranceFree = Bikewale.Utility.DealerOfferHelper.HasFreeInsurance(dealerId.ToString(), "", price.CategoryName, price.Price, ref insuranceAmount);
                            if (isInsuranceFree)
                                break;
                        }
                    }

                }
            }
            catch (Exception err)
            {
                Trace.Warn(err.Message);
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }
        #endregion

        #endregion

        #region Bind variants available with dealer
        /// <summary>
        /// Author  : Sushil Kumar 
        /// Created On : 8th December 2015
        /// Summary : Bind Varients available with dealer
        /// </summary>
        private void BindVarientDetails()
        {
            if (versionId > 0 && objBooking != null && objBooking.Varients != null && objBooking.Varients.Count > 0)
            {
                var data = (objBooking.Varients).Where(v => v.BookingAmount > 0);
                rptVarients.DataSource = data;
                rptVarients.DataBind();

                if (objBooking.Varients.FirstOrDefault().Make != null && objBooking.Varients.FirstOrDefault().Model != null)
                {
                    bikeName = String.Format("{0} {1}", objBooking.Varients.FirstOrDefault().Make.MakeName, objBooking.Varients.FirstOrDefault().Model.ModelName);
                }
            }
            else
            {
                Response.Redirect("/pagenotfound.aspx", false);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
                this.Page.Visible = false;
            }
        }
        #endregion 
       

        #region Fetch Customer Details
        /// <summary>
        /// Author  : Sushil Kumar 
        /// Created On : 12th December 2015
        /// Summary : Fetch Customer details fro validating user after make payment
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
        #endregion

        #region Make payment (Transaction Status)
        /// <summary>
        /// Modified By :   Sumit Kate on 18 Nov 2015
        /// Description :   Save the State of the Booking Journey as Described in Task# 107795062
        /// </summary>
        /// <param name="sourceType"></param>
        protected void BeginTransaction(string sourceType)
        {
            string transresp = string.Empty;
            //fetchCustomerDetails();

            if (objCustomer != null && objCustomer.objCustomerBase != null && objCustomer.objCustomerBase.CustomerId > 0)
            {
                var transaction = new TransactionDetails()
                {
                    CustomerID = objCustomer.objCustomerBase.CustomerId,
                    PackageId = (int)Carwale.Entity.Enum.BikeBooking.BikeBooking,
                    ConsumerType = 2,
                    Amount = dealerDetailEntity.objBookingAmt.Amount,
                    ClientIP = CommonOpn.GetClientIP(),
                    UserAgent = HttpContext.Current.Request.ServerVariables["HTTP_USER_AGENT"],
                    PGId = Convert.ToUInt64(PriceQuoteCookie.VersionId),
                    CustomerName = objCustomer.objCustomerBase.CustomerName,
                    CustEmail = objCustomer.objCustomerBase.CustomerEmail,
                    CustMobile = objCustomer.objCustomerBase.CustomerMobile,
                    CustCity = objCustomer.objCustomerBase.cityDetails.CityName,
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
                    HttpContext.Current.Response.Redirect("http://" + HttpContext.Current.Request.ServerVariables["HTTP_HOST"].ToString() + "/m/pricequote/bookingsummary_new.aspx", false);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                    this.Page.Visible = false;
                }
            }
            else
            {
                HttpContext.Current.Response.Redirect("http://" + HttpContext.Current.Request.ServerVariables["HTTP_HOST"].ToString() + "/m/pricequote/bookingsummary_new.aspx", false);
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