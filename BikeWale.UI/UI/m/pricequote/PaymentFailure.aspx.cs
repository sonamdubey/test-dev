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

namespace Bikewale.Mobile.PriceQuote
{
    public class PaymentFailure : System.Web.UI.Page
    {
        protected Button btnTryAgain;
        protected PQCustomerDetail objCustomer = null;
        protected BookingAmountEntity objAmount = null;
        protected string versionId = string.Empty, dealerId = string.Empty, MakeModel = string.Empty;
        protected uint PqId = 0;

        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
            btnTryAgain.Click += new EventHandler(ProcessPayment);
        }

        protected void Page_Load(object sender, EventArgs e)
        {

            PqId = Convert.ToUInt32(PriceQuoteQueryString.PQId);
            dealerId = PriceQuoteQueryString.DealerId;
            versionId = PriceQuoteQueryString.VersionId;

            if (!String.IsNullOrEmpty(dealerId) && Convert.ToUInt32(PriceQuoteQueryString.PQId) > 0 && PGCookie.PGTransId != "-1")
            {
                GetDetailedQuote();
                getCustomerDetails();
                if (objCustomer.IsTransactionCompleted)
                {
                    Response.Redirect("/m/pricequote/paymentconfirmation.aspx?MPQ=" + EncodingDecodingHelper.EncodeTo64(PriceQuoteQueryString.QueryString), false);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                    this.Page.Visible = false;
                }
            }
            else
            {
                Response.Redirect("/m/pricequote/", false);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
                this.Page.Visible = false;
            }
        }

        private void ProcessPayment(object sender, EventArgs e)
        {
            if (objAmount != null)
            {
                if (objAmount.objBookingAmountEntityBase.Amount > 0)
                    BeginTransaction("3");
                else
                {
                    Response.Redirect("/m/pricequote/", false);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                    this.Page.Visible = false;
                }
            }
        }

        /// <summary>
        /// Modified by :   Sumit Kate on 09 Dec 2016
        /// Description :   PG Transaction MySql Migration
        /// </summary>
        /// <param name="sourceType"></param>
        private void BeginTransaction(string sourceType)
        {
            string transresp = string.Empty;

            if (objCustomer.objCustomerBase.CustomerId.ToString() != "" && objCustomer.objCustomerBase.CustomerId > 0)
            {
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
                    PlatformId = 2,  //Desktop
                    ApplicationId = 2, //bikewale

                    RequestToPGUrl = "https://" + HttpContext.Current.Request.ServerVariables["HTTP_HOST"].ToString() + "/bikebooking/RedirectToBillDesk.aspx",

                    //sourceid = 2 to redirect response on mobile site 
                    ReturnUrl = "https://" + HttpContext.Current.Request.ServerVariables["HTTP_HOST"].ToString() + "/bikebooking/billdeskresponse.aspx?sourceId=2&"
                        + "MPQ=" + EncodingDecodingHelper.EncodeTo64(PriceQuoteQueryString.QueryString)
                };
                //PGCookie.PGAmount = transaction.Amount.ToString();
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
                    HttpContext.Current.Response.Redirect("https://" + HttpContext.Current.Request.ServerVariables["HTTP_HOST"].ToString() + "/m/pricequote/bookingsummary.aspx?MPQ=" + EncodingDecodingHelper.EncodeTo64(PriceQuoteQueryString.QueryString));
                }
            }
            else
            {
                HttpContext.Current.Response.Redirect("https://" + HttpContext.Current.Request.ServerVariables["HTTP_HOST"].ToString() + "/m/pricequote/bookingsummary.aspx?MPQ=" + EncodingDecodingHelper.EncodeTo64(PriceQuoteQueryString.QueryString));
            }

        }

        protected void getCustomerDetails()
        {
            using (IUnityContainer container = new UnityContainer())
            {
                container.RegisterType<IDealerPriceQuote, Bikewale.BAL.BikeBooking.DealerPriceQuote>();
                IDealerPriceQuote objDealer = container.Resolve<IDealerPriceQuote>();

                objCustomer = objDealer.GetCustomerDetailsByLeadId(Convert.ToUInt32(PriceQuoteQueryString.LeadId));
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
    }
}