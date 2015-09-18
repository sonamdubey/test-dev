using Carwale.BL.PaymentGateway;
using Carwale.DAL.PaymentGateway;
using Carwale.Entity.Enum;
using Carwale.Entity.PaymentGateway;
using Carwale.Interfaces.PaymentGateway;
using Bikewale.Utility;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net.Http;
using System.Net.Http.Headers;
using Bikewale.Notifications;
using Bikewale.Interfaces.BikeBooking;
using Bikewale.Mobile.PriceQuote;

namespace Bikewale.BikeBooking
{
    public class BillDeskResponse : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //if (Request.Cookies["PGTransId"] != null)
            //{
            //    HttpContext.Current.Request.Cookies.Remove("PGTransId");
            //}
            if (!IsPostBack)
            {
                if (Request.Form.Count > 0)
                {
                    CompleteTransaction();
                }
            }

        }

        private void CompleteTransaction()
        {
            bool isUpdated = false;
            IUnityContainer containerTran = new UnityContainer();
            containerTran.RegisterType<ITransaction, Transaction>()
                        .RegisterType<IPaymentGateway, BillDesk>()
                        .RegisterType<ITransactionRepository, TransactionRepository>()
                        .RegisterType<IPackageRepository, PackageRepository>()
                        .RegisterType<ITransactionValidator, ValidateTransaction>();

            ITransaction completetransaction = containerTran.Resolve<ITransaction>();
            bool transresp = completetransaction.CompleteBillDeskTransaction();
            Trace.Warn("transresp : " + transresp);


            containerTran.RegisterType<IDealerPriceQuote, Bikewale.BAL.BikeBooking.DealerPriceQuote>();
            IDealerPriceQuote objDealer = containerTran.Resolve<IDealerPriceQuote>();
            

            if (Convert.ToInt16(PGCookie.PGRespCode) == Convert.ToInt16(BillDeskTransactionStatusCode.Successfull))
            {
                isUpdated = objDealer.UpdatePQTransactionalDetail(Convert.ToUInt32(PriceQuoteCookie.PQId), Convert.ToUInt32(PGCookie.PGTransId), true, ConfigurationManager.AppSettings["OfferUniqueTransaction"]);
            }
            else
            {
                isUpdated = objDealer.UpdatePQTransactionalDetail(Convert.ToUInt32(PriceQuoteCookie.PQId), Convert.ToUInt32(PGCookie.PGTransId), false, ConfigurationManager.AppSettings["OfferUniqueTransaction"]);
            }
            
            if (Request.QueryString["sourceid"] != null && Request.QueryString["sourceid"] != "")
            {
                if (Request.QueryString["sourceid"].ToString() == "1")
                {
                    if (Convert.ToInt16(PGCookie.PGRespCode) == Convert.ToInt16(BillDeskTransactionStatusCode.Successfull))
                    {
                        HttpContext.Current.Response.Redirect("/pricequote/paymentconfirmation.aspx");
                    }
                    else
                    {
                        HttpContext.Current.Response.Redirect("/pricequote/paymentfailure.aspx");
                    }
                }
                if (Request.QueryString["sourceid"].ToString() == "2")
                {
                    if (Convert.ToInt16(PGCookie.PGRespCode) == Convert.ToInt16(BillDeskTransactionStatusCode.Successfull))
                    {
                        HttpContext.Current.Response.Redirect("/m/pricequote/paymentconfirmation.aspx");
                    }
                    else
                    {
                        HttpContext.Current.Response.Redirect("/m/pricequote/paymentfailure.aspx");
                    }
                }
            }
        }
    }
}
