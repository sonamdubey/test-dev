using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Carwale.BL.PaymentGateway;
using Carwale.DAL.PaymentGateway;
using Carwale.Entity.Enum;
using Carwale.Entity.PaymentGateway;
using Carwale.Interfaces.PaymentGateway;
//using Vspl.Common;
using Microsoft.Practices.Unity;
using System.Configuration;
using MobileWeb.Common;

namespace MobileWeb.Research
{
    public class BookCar : System.Web.UI.Page
    {
        protected Button btnMakePayment;
        
        string ClientIP
        {
            get
            {
                if (HttpContext.Current.Request.ServerVariables["HTTP_CLIENT_IP"] != null)
                    return HttpContext.Current.Request.ServerVariables["HTTP_CLIENT_IP"];
                else
                    return "";

            }
        }


        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
        }

        void InitializeComponent()
        {
            base.Load += new EventHandler(Page_Load);
            this.btnMakePayment.Click += new EventHandler(btnMakePayment_Click);
        }

        void Page_Load(object Sender, EventArgs e)
        {
            
        }

        void btnMakePayment_Click(object Sender, EventArgs e)
        {
            BeginTxn("3");       //SourceId = 4 for Bill Desk PG from mobile
        }

        void BeginTxn(string sourceType)
        {
            string transresp = string.Empty;
            var amountReal = CurrentUser.Id == "2178" ? 2 : Convert.ToUInt16(ConfigurationManager.AppSettings["BookingAmount"].ToString());
            Trace.Warn("ResponseId : " + PGCookie.ResponseId);
            if (PGCookie.ResponseId != "" && Convert.ToInt32(PGCookie.ResponseId) > 0)
            {
                var transaction = new TransactionDetails()
                {
                    CustomerID = Convert.ToUInt64(PGCookie.ResponseId),
                    PackageId = (int)OfferPackage.OfferPackage,
                    ConsumerType = 1,
                    Amount = amountReal,
                    ClientIP = ClientIP,
                    UserAgent = HttpContext.Current.Request.ServerVariables["HTTP_USER_AGENT"],
                    PGId = Convert.ToUInt64(PGCookie.ResponseId),
                    CustomerName = PGCookie.CustomerName,
                    CustEmail = PGCookie.CustomerEmail,
                    CustMobile = PGCookie.CustomerMobile,
                    CustCity = PGCookie.CustomerCity,
                    PlatformId = 43,  //Mobile
                    ApplicationId = 1, //Carwale
                    RequestToPGUrl = "https://" + HttpContext.Current.Request.ServerVariables["HTTP_HOST"].ToString() + "/new/RedirectToBillDesk.aspx",
                    ReturnUrl = "https://" + HttpContext.Current.Request.ServerVariables["HTTP_HOST"].ToString() + "/new/billdeskresponse.aspx?sourceId=43"
                };
                PGCookie.PGAmount = transaction.Amount.ToString();
                PGCookie.PGCarId = transaction.PGId.ToString();
                PGCookie.PGPkgId = transaction.PackageId.ToString();
                PGCookie.PGRespCode = "";
                PGCookie.PGMessage = "";

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
                    HttpContext.Current.Response.Redirect("https://" + HttpContext.Current.Request.ServerVariables["HTTP_HOST"].ToString() + "/research/offercustomerinfo.aspx");
                    Trace.Warn("fail");
                }
            }
            else
            {
                HttpContext.Current.Response.Redirect("https://" + HttpContext.Current.Request.ServerVariables["HTTP_HOST"].ToString() + "/research/offercustomerinfo.aspx");
                Trace.Warn("fail");
            }
        }        
    }
}
