﻿using Bikewale.Common;
using Bikewale.Entities.BikeBooking;
using Bikewale.Entities.BikeBooking;
using Bikewale.Interfaces.BikeBooking;
using Carwale.BL.PaymentGateway;
using Carwale.DAL.PaymentGateway;
using Carwale.Entity.Enum;
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

namespace Bikewale.Mobile.PriceQuote
{
    public class PaymentFailure : System.Web.UI.Page
    {
        protected Button btnTryAgain;
        protected PQCustomerDetail objCustomer = null;
        protected BookingAmountEntity objAmount = null;
        protected string versionId = string.Empty, dealerId = string.Empty, MakeModel =string.Empty;
        protected uint PqId = 0;

        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
            btnTryAgain.Click += new EventHandler(ProcessPayment);
        }

        protected void Page_Load(object sender, EventArgs e)
        {

            PqId = Convert.ToUInt32(PriceQuoteCookie.PQId);
            dealerId = PriceQuoteCookie.DealerId;
            versionId = PriceQuoteCookie.VersionId;

            if (!String.IsNullOrEmpty(dealerId) && Convert.ToUInt32(PriceQuoteCookie.PQId) > 0 && PGCookie.PGTransId != "-1")
            {
                GetDetailedQuote();
                getCustomerDetails();
                if (objCustomer.IsTransactionCompleted)
                    Response.Redirect("/m/pricequote/paymentconfirmation.aspx", true);
            }
            else
                Response.Redirect("/m/pricequote/", true);
        }

        private void ProcessPayment(object sender, EventArgs e)
        {
            if (objAmount != null)
            {
                if (objAmount.objBookingAmountEntityBase.Amount > 0)
                    BeginTransaction("3");
                else
                    Response.Redirect("/m/pricequote/", true);
            }
        }

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
                    PGId = Convert.ToUInt64(PriceQuoteCookie.VersionId),
                    CustomerName = objCustomer.objCustomerBase.CustomerName,
                    CustEmail = objCustomer.objCustomerBase.CustomerEmail,
                    CustMobile = objCustomer.objCustomerBase.CustomerMobile,
                    CustCity = objCustomer.objCustomerBase.cityDetails.CityName,
                    PlatformId = 2,  //Desktop
                    ApplicationId = 2, //bikewale

                    RequestToPGUrl = "http://" + HttpContext.Current.Request.ServerVariables["HTTP_HOST"].ToString() + "/bikebooking/RedirectToBillDesk.aspx",

                    //sourceid = 2 to redirect response on mobile site 
                    ReturnUrl = "http://" + HttpContext.Current.Request.ServerVariables["HTTP_HOST"].ToString() + "/bikebooking/billdeskresponse.aspx?sourceId=2"
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
                    HttpContext.Current.Response.Redirect("http://" + HttpContext.Current.Request.ServerVariables["HTTP_HOST"].ToString() + "/m/pricequote/bookingsummary.aspx");
                    Trace.Warn("fail");
                }
            }
            else
            {
                HttpContext.Current.Response.Redirect("http://" + HttpContext.Current.Request.ServerVariables["HTTP_HOST"].ToString() + "/m/pricequote/bookingsummary.aspx");
                Trace.Warn("fail");
            }

        }

        protected void getCustomerDetails()
        {
            using (IUnityContainer container = new UnityContainer())
            {
                container.RegisterType<IDealerPriceQuote, Bikewale.BAL.BikeBooking.DealerPriceQuote>();
                IDealerPriceQuote objDealer = container.Resolve<IDealerPriceQuote>();

                objCustomer = objDealer.GetCustomerDetails(Convert.ToUInt32(PriceQuoteCookie.PQId));
            }
        }

        /// <summary>
        /// Written By : Ashwini Todkar on 15 Dec 2014
        /// Summary    : Method to get dealer price quote, offers, facilities, contact details 
        /// </summary>
        private void GetDetailedQuote()
        {
            bool _isContentFound = true;
            try
            {
                string _abHostUrl = ConfigurationManager.AppSettings["ABApiHostUrl"];
                string _requestType = "application/json";

                string _apiUrl = "/api/dealers/getdealerbookingamount/?versionId=" + PriceQuoteCookie.VersionId + "&DealerId=" + PriceQuoteCookie.DealerId;
                // Send HTTP GET requests 

                objAmount = BWHttpClient.GetApiResponseSync<BookingAmountEntity>(_abHostUrl, _requestType, _apiUrl, objAmount);

                if (objAmount != null)
                    MakeModel = objAmount.objMake.MakeName + " " + objAmount.objModel.ModelName;
            }
            catch (Exception err)
            {
                Trace.Warn(err.Message);
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            finally
            {
                if (!_isContentFound)
                    Response.Redirect("/m/pagenotfound.aspx", true);
            }
        }
    }
}