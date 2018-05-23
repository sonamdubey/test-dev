using Bikewale.Common;
using Bikewale.Entities.BikeBooking;
using Bikewale.Interfaces.BikeBooking;
using Bikewale.Utility;
using Carwale.BL.PaymentGateway;
using Carwale.DAL.Classified.SellCar;
using Carwale.DAL.PaymentGateway;
using Carwale.Entity.PaymentGateway;
using Carwale.Interfaces.Classified.SellCar;
using Carwale.Interfaces.PaymentGateway;
using Microsoft.Practices.Unity;
using System;
using System.Web;
using System.Web.UI.WebControls;

namespace Bikewale.PriceQuote
{
    public class PaymentFailure : System.Web.UI.Page
    {
        protected Button btnMakePayment;
        protected PQCustomerDetail objCustomer;
        protected BookingAmountEntity objAmount = null;
        protected string MakeModel = string.Empty;

        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
            btnMakePayment.Click += new EventHandler(btnMakePayment_click);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            // Modified By :Lucky Rathore on 12 July 2016.
            Form.Action = Request.RawUrl;
            //code for device detection added by Ashwini Todkar
            // Modified By :Ashish Kamble on 5 Feb 2016
            string originalUrl = Request.ServerVariables["HTTP_X_ORIGINAL_URL"];
            if (String.IsNullOrEmpty(originalUrl))
                originalUrl = Request.ServerVariables["URL"];

            DeviceDetection dd = new DeviceDetection(originalUrl);
            dd.DetectDevice();

            if (!String.IsNullOrEmpty(PriceQuoteQueryString.DealerId) && Convert.ToUInt32(PriceQuoteQueryString.PQId) > 0 && PGCookie.PGTransId != "-1")
            {
                GetDetailedQuote();
                getCustomerDetails();
                if (objCustomer.IsTransactionCompleted)
                {
                    Response.Redirect("/pricequote/paymentconfirmation.aspx?MPQ=" + EncodingDecodingHelper.EncodeTo64(PriceQuoteQueryString.QueryString), false);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                    this.Page.Visible = false;
                }
            }
            else
            {
                //PushBikeBookingFailure();
                Response.Redirect("/pricequote/", false);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
                this.Page.Visible = false;
            }
        }

        // <summary>
        /// created By : Sadhana Upadhyay on 11 Nov 2014
        /// Summary : Get Customer Details 
        /// </summary>
        protected void getCustomerDetails()
        {
            using (IUnityContainer container = new UnityContainer())
            {
                container.RegisterType<IDealerPriceQuote, Bikewale.BAL.BikeBooking.DealerPriceQuote>();
                IDealerPriceQuote objDealer = container.Resolve<IDealerPriceQuote>();

                objCustomer = objDealer.GetCustomerDetails(Convert.ToUInt32(PriceQuoteQueryString.PQId));
            }
        }

        /// <summary>
        /// Written By : Ashwini Todkar on 15 Dec 2014
        /// Summary    : PopulateWhere to get dealer price quote, offers, facilities, contact details 
        /// </summary>
        private void GetDetailedQuote()
        {
            bool _isContentFound = true;
            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<Bikewale.Interfaces.AutoBiz.IDealers, Bikewale.DAL.AutoBiz.DealersRepository>();
                    Bikewale.Interfaces.AutoBiz.IDealers objDealer = container.Resolve<Bikewale.DAL.AutoBiz.DealersRepository>();
                    objAmount = objDealer.GetDealerBookingAmount(Convert.ToUInt32(PriceQuoteQueryString.VersionId), Convert.ToUInt32(PriceQuoteQueryString.DealerId));
                }


                if (objAmount != null)
                    MakeModel = objAmount.objMake.MakeName + " " + objAmount.objModel.ModelName;
            }
            catch (Exception err)
            {
                Trace.Warn(err.Message);
                Bikewale.Notifications.ErrorClass.LogError(err, Request.ServerVariables["URL"]);

            }
            finally
            {
                if (!_isContentFound)
                {
                    UrlRewrite.Return404();
                }
            }
        }

        void btnMakePayment_click(object Sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(PriceQuoteQueryString.DealerId) && Convert.ToUInt32(PriceQuoteQueryString.PQId) > 0)
            {


                if (objAmount != null)
                {
                    if (objAmount.objBookingAmountEntityBase.Amount > 0)

                        BeginTransaction("3");
                    else
                    {
                        Response.Redirect("/pricequote/", false);
                        HttpContext.Current.ApplicationInstance.CompleteRequest();
                        this.Page.Visible = false;
                    }
                }
            }
        }

        /// <summary>
        /// Modified by :   Sumit Kate on 09 Dec 2016
        /// Description :   PG Transaction MySql Migration
        /// </summary>
        /// <param name="sourceType"></param>
        protected void BeginTransaction(string sourceType)
        {
            string transresp = string.Empty;

            if (objCustomer.objCustomerBase.CustomerId.ToString() != "" && objCustomer.objCustomerBase.CustomerId > 0)
            {
                Trace.Warn("Inside begin tarns" + objCustomer.objCustomerBase.CustomerId.ToString());
                var transaction = new TransactionDetails()
                {
                    CustomerID = objCustomer.objCustomerBase.CustomerId,
                    PackageId = (int)Carwale.Entity.Enum.BikeBooking.BikeBooking,
                    ConsumerType = 2,
                    Amount = objAmount.objBookingAmountEntityBase.Amount,
                    ClientIP = CommonOpn.GetClientIP(),
                    UserAgent = HttpContext.Current.Request.ServerVariables["HTTP_USER_AGENT"],
                    PGId = Convert.ToUInt64(PriceQuoteQueryString.VersionId),
                    CustomerName = objCustomer.objCustomerBase.CustomerName,
                    CustEmail = objCustomer.objCustomerBase.CustomerEmail,
                    CustMobile = objCustomer.objCustomerBase.CustomerMobile,
                    CustCity = objCustomer.objCustomerBase.cityDetails.CityName,
                    PlatformId = 1,  //Desktop
                    ApplicationId = 2, //bikewale
                    RequestToPGUrl = "https://" + HttpContext.Current.Request.ServerVariables["HTTP_HOST"].ToString() + "/bikebooking/RedirectToBillDesk.aspx",
                    ReturnUrl = "https://" + HttpContext.Current.Request.ServerVariables["HTTP_HOST"].ToString() + "/bikebooking/billdeskresponse.aspx?sourceId=1&"
                        + "MPQ=" + EncodingDecodingHelper.EncodeTo64(PriceQuoteQueryString.QueryString)
                };

                PGCookie.PGCarId = transaction.PGId.ToString();

                //Modified By : Sadhana Upadhyay on 22 Jan 2016 
                //Added Logic to save Bike Booking Cookie 
                BikeBookingCookie.SaveBBCookie(PriceQuoteQueryString.CityId, PriceQuoteQueryString.PQId, PriceQuoteQueryString.AreaId,
                    PriceQuoteQueryString.VersionId, PriceQuoteQueryString.DealerId);

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
                    HttpContext.Current.Response.Redirect("https://" + HttpContext.Current.Request.ServerVariables["HTTP_HOST"].ToString() + "/pricequote/bookingsummary_new.aspx?MPQ=" + EncodingDecodingHelper.EncodeTo64(PriceQuoteQueryString.QueryString));
                    Trace.Warn("fail");
                }
            }
            else
            {
                HttpContext.Current.Response.Redirect("https://" + HttpContext.Current.Request.ServerVariables["HTTP_HOST"].ToString() + "/pricequote/bookingsummary_new.aspx?MPQ=" + EncodingDecodingHelper.EncodeTo64(PriceQuoteQueryString.QueryString));
                Trace.Warn("fail");
            }
        }
    }
}