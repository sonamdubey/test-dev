using Bikewale.Common;
using Bikewale.DTO.PriceQuote.BikeBooking;
using Bikewale.DTO.PriceQuote.DetailedDealerQuotation;
using Bikewale.Entities.BikeBooking;
using Bikewale.Entities.PriceQuote;
using Bikewale.Interfaces.BikeBooking;
using Bikewale.Interfaces.PriceQuote;
using Bikewale.Utility;
using Carwale.BL.PaymentGateway;
using Carwale.DAL.Classified.SellCar;
using Carwale.DAL.PaymentGateway;
using Carwale.Entity.PaymentGateway;
using Carwale.Interfaces.Classified.SellCar;
using Carwale.Interfaces.PaymentGateway;
using Microsoft.Practices.Unity;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Bikewale.BikeBooking
{
    /// <summary>
    /// Modified by :   Sumit Kate on 19 Jan 2016
    /// Description :   Added Users Testimonial Control
    /// Modified By : Sushil Kumar on 20th April 2016
    /// Description : Remove ctrlUsersTestimonials section as testimonials are removed
    /// </summary>
    public class BookingSummary_New : System.Web.UI.Page
    {
        protected uint dealerId = 0, versionId = 0, cityId = 0, areaId = 0, versionPrice = 0, bookingAmount = 0, insuranceAmount = 0, leadId = 0;
        protected string clientIP = String.Empty, pageUrl = String.Empty, bikeName = String.Empty, location = String.Empty, pqId = String.Empty;
        protected Repeater rptVarients = null, rptVersionColors = null, rptDealerOffers = null, rptPriceBreakup = null, rptDealerFinalOffers = null;
        protected BikeDealerPriceDetailDTO selectedVarient = null;
        protected DDQDealerDetailBase DealerDetails = null;
        protected bool isOfferAvailable = false, isInsuranceFree = false;
        protected string versionWaitingPeriod = String.Empty, dealerAddress = String.Empty, latitude = "0", longitude = "0", jsonBikeVarients = String.Empty;
        protected HtmlInputButton generateNewOTP, deliveryDetailsNextBtn, processOTP;
        protected BookingPageDetailsEntity objBooking = null;
        protected PQCustomerDetail objCustomer = null;
        protected PQ_DealerDetailEntity dealerDetailEntity = null;
        protected string bikesData = string.Empty, discountedPriceList = string.Empty;

        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
            deliveryDetailsNextBtn.ServerClick += new EventHandler(btnMakePayment_click);

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
        /// Summary : Get version details and quotation details
        ///           Also get dealer details from autobiz
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
                        var _data = (objBooking.Varients).FirstOrDefault(v => v.MinSpec != null && v.MinSpec.VersionId == versionId);
                        if (_data != null && _data.BookingAmount > 0)
                        {
                            BindVarientDetails();
                        }
                        else
                        {
                            HttpContext.Current.Response.Redirect("/", false);
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
                ErrorClass.LogError(err, Request.ServerVariables["URL"]);

            }
            finally
            {
                if (!_isContentFound)
                {
                    UrlRewrite.Return404();
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
                    container.RegisterType<IDealerPriceQuote, Bikewale.BAL.BikeBooking.DealerPriceQuote>();
                    IDealerPriceQuote _objDealerPricequote = container.Resolve<IDealerPriceQuote>();

                    //bookingpagedetails fetch
                    objBooking = _objDealerPricequote.FetchBookingPageDetails(cityId, versionId, dealerId);

                    //customer details
                    objCustomer = _objDealerPricequote.GetCustomerDetailsByLeadId(leadId);

                }
            }
            catch (Exception err)
            {
                Trace.Warn(err.Message);
                ErrorClass.LogError(err, Request.ServerVariables["URL"]);

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
                    dealerAddress = String.Format("{0}<br/>{1},{2},{3}-{4},{5}.", dealerDetailEntity.objDealer.Organization, dealerDetailEntity.objDealer.Address, dealerDetailEntity.objDealer.objArea.AreaName, dealerDetailEntity.objDealer.objCity.CityName, dealerDetailEntity.objDealer.objArea.PinCode, dealerDetailEntity.objDealer.objState.StateName);
                }

                //bind offers provided by dealer
                if (dealerDetailEntity.objDealer != null && dealerDetailEntity.objOffers != null)
                {
                    BindDealerOffers();
                }

            }
            else
            {
                Response.Redirect("/pricequote/quotation.aspx?MPQ=" + EncodingDecodingHelper.EncodeTo64(PriceQuoteQueryString.QueryString), false);
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

                rptDealerFinalOffers.DataSource = dealerDetailEntity.objOffers;
                rptDealerFinalOffers.DataBind();


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

            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<Bikewale.Interfaces.AutoBiz.IDealers, Bikewale.DAL.AutoBiz.DealersRepository>();
                    Bikewale.Interfaces.AutoBiz.IDealers objDealer = container.Resolve<Bikewale.DAL.AutoBiz.DealersRepository>();
                    PQParameterEntity objParam = new PQParameterEntity();
                    objParam.CityId = cityId;
                    objParam.DealerId = dealerId;
                    objParam.VersionId = versionId;
                    dealerDetailEntity = objDealer.GetDealerDetailsPQ(objParam);
                }

                if (dealerDetailEntity != null)
                {

                    if (dealerDetailEntity.objQuotation != null)
                    {
                        discountedPriceList = JsonConvert.SerializeObject(dealerDetailEntity.objQuotation.discountedPriceList);

                        foreach (var price in dealerDetailEntity.objQuotation.PriceList)
                        {
                            isInsuranceFree = Bikewale.Utility.DealerOfferHelper.HasFreeInsurance(dealerId.ToString(), "", price.CategoryName, price.Price, ref insuranceAmount);
                            if (isInsuranceFree)
                                break;
                        }

                        if (dealerDetailEntity.objOffers != null && dealerDetailEntity.objOffers.Count > 0)
                            dealerDetailEntity.objQuotation.discountedPriceList = OfferHelper.ReturnDiscountPriceList(dealerDetailEntity.objOffers, dealerDetailEntity.objQuotation.PriceList);
                    }
                }
            }
            catch (Exception err)
            {
                Trace.Warn(err.Message);
                ErrorClass.LogError(err, Request.ServerVariables["URL"]);

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

                jsonBikeVarients = EncodingDecodingHelper.EncodeTo64(JsonConvert.SerializeObject(objBooking.Varients));
                bikesData = JsonConvert.SerializeObject(objBooking.Varients);

                if (objBooking.Varients.FirstOrDefault().Make != null && objBooking.Varients.FirstOrDefault().Model != null)
                {
                    bikeName = String.Format("{0} {1}", objBooking.Varients.FirstOrDefault().Make.MakeName, objBooking.Varients.FirstOrDefault().Model.ModelName);
                }

            }
            else
            {
                UrlRewrite.Return404();
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

                objCustomer = objDealer.GetCustomerDetailsByLeadId(leadId);
            }
        }
        #endregion

        #region Make payment (Transaction Status)
        /// <summary>
        /// Modified By :   Sumit Kate on 18 Nov 2015
        /// Description :   Save the State of the Booking Journey as Described in Task# 107795062
        /// Modified by :   Sumit Kate on 09 Dec 2016
        /// Description :   PG Transaction MySql Migration
        /// </summary>
        /// <param name="sourceType"></param>
        protected void BeginTransaction(string sourceType)
        {
            string transresp = string.Empty;
            //fetchCustomerDetails();
            TransactionDetails transaction = null;
            try
            {
                if (objCustomer != null && objCustomer.objCustomerBase != null && objCustomer.objCustomerBase.CustomerId > 0)
                {
                    transaction = new TransactionDetails()
                    {
                        CustomerID = objCustomer.objCustomerBase.CustomerId,
                        PackageId = (int)Carwale.Entity.Enum.BikeBooking.BikeBooking,
                        ConsumerType = 2,
                        Amount = dealerDetailEntity.objBookingAmt.Amount,
                        ClientIP = CommonOpn.GetClientIP(),
                        UserAgent = HttpContext.Current.Request.ServerVariables["HTTP_USER_AGENT"],
                        PGId = Convert.ToUInt64(PriceQuoteQueryString.VersionId),
                        CustomerName = objCustomer.objCustomerBase.CustomerName,
                        CustEmail = objCustomer.objCustomerBase.CustomerEmail,
                        CustMobile = objCustomer.objCustomerBase.CustomerMobile,
                        CustCity = objCustomer.objCustomerBase.cityDetails.CityName,
                        PlatformId = 1,  //Desktop
                        ApplicationId = 2, //Carwale
                        RequestToPGUrl = string.Format("{0}/UI/bikebooking/RedirectToBillDesk.aspx", BWConfiguration.Instance.BwHostUrl),
                        ReturnUrl = string.Format("{0}/UI/bikebooking/billdeskresponse.aspx?sourceId=1&MPQ={1}", BWConfiguration.Instance.BwHostUrl, EncodingDecodingHelper.EncodeTo64(PriceQuoteQueryString.QueryString))
                    };
                    //PGCookie.PGAmount = transaction.Amount.ToString();

                    //Modified By : Sadhana Upadhyay on 22 Jan 2016 
                    //Added Logic to save Bike Booking Cookie 
                    BikeBookingCookie.SaveBBCookie(PriceQuoteQueryString.CityId, PriceQuoteQueryString.PQId, PriceQuoteQueryString.AreaId,
                        PriceQuoteQueryString.VersionId, PriceQuoteQueryString.DealerId);

                    PGCookie.PGCarId = transaction.PGId.ToString();

                    IUnityContainer container = new UnityContainer();
                    container.RegisterType<ITransaction, Transaction>()
                    .RegisterType<ISellCarRepository, SellCarRepository>()
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
                        HttpContext.Current.Response.Redirect(String.Format("{0}/pricequote/bookingsummary_new.aspx?MPQ={1}", BWConfiguration.Instance.BwHostUrlForJs, EncodingDecodingHelper.EncodeTo64(PriceQuoteQueryString.QueryString)), false);
                        HttpContext.Current.ApplicationInstance.CompleteRequest();
                        this.Page.Visible = false;
                    }
                }
                else
                {
                    HttpContext.Current.Response.Redirect(String.Format("{0}/pricequote/bookingsummary_new.aspx?MPQ={1}", BWConfiguration.Instance.BwHostUrlForJs, EncodingDecodingHelper.EncodeTo64(PriceQuoteQueryString.QueryString)), false);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                    this.Page.Visible = false;
                }
            }
            catch (ThreadAbortException)
            {
                //Do nothing for thread abort exception
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass.LogError(ex, "BookingSummary_New.BeginTransaction : " + PriceQuoteQueryString.QueryString + " " + Newtonsoft.Json.JsonConvert.SerializeObject(transaction));
            }
        }
        #endregion

        #region Private Method to process cookie
        /// <summary>
        /// Checks for the valid PQ Cookie
        /// </summary>
        private void ProcessCookie()
        {

            try
            {
                if (PriceQuoteQueryString.IsPQQueryStringExists())
                {
					pqId = PriceQuoteQueryString.PQId;
                    if (!String.IsNullOrEmpty(pqId) && UInt32.TryParse(PriceQuoteQueryString.DealerId, out dealerId) && UInt32.TryParse(PriceQuoteQueryString.VersionId, out versionId))
                    {
						leadId = Convert.ToUInt32(PriceQuoteQueryString.LeadId);
                        cityId = Convert.ToUInt32(PriceQuoteQueryString.CityId);
                        areaId = Convert.ToUInt32(PriceQuoteQueryString.AreaId);

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
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass.LogError(ex, "BookingSummary_New.ProcessCookie : " + Request.Url.PathAndQuery);
            }
        }
        #endregion
    }
}

