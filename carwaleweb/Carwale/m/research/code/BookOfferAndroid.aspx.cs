using Carwale.BL.PaymentGateway;
using Carwale.DAL.PaymentGateway;
using Carwale.Entity.Enum;
using Carwale.Entity.Offers;
using Carwale.Entity.PaymentGateway;
using Carwale.Interfaces.PaymentGateway;
using Carwale.Utility;
using Microsoft.Practices.Unity;
using MobileWeb.Common;
using System;
using System.Configuration;
using System.Web;
using System.Web.UI.HtmlControls;

namespace MobileWeb.Research
{
    public class BookOfferAndroid : System.Web.UI.Page
    {
        protected HtmlGenericControl divErrorMsg;
        OfferPGAndroid objOffer;
        protected string errorMsg = "";

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
        }

        void Page_Load(object Sender, EventArgs e)
        {
            if (Request.QueryString["couponId"] != null && Request.QueryString["couponId"] != "")
                PGCookie.CouponId = Request.QueryString["couponId"].ToString();
            if (Request.QueryString["versionId"] != null && Request.QueryString["versionId"] != "")
                PGCookie.VersionId = Request.QueryString["versionId"].ToString();
            if (Request.QueryString["responseId"] != null && Request.QueryString["responseId"] != "")
                PGCookie.ResponseId = Request.QueryString["responseId"].ToString();
            if (Request.QueryString["offerId"] != null && Request.QueryString["offerId"] != "")
                PGCookie.OfferId = Request.QueryString["offerId"].ToString();
            if (Request.QueryString["dealerid"] != null && Request.QueryString["dealerid"] != "")
                PGCookie.DealerId = Request.QueryString["dealerid"].ToString();
            if (Request.QueryString["cityId"] != null && Request.QueryString["cityId"] != "")
            {
                GetPGDetails(PGCookie.ResponseId, Request.QueryString["cityId"].ToString(),PGCookie.OfferId, PGCookie.VersionId);
                BeginTxn("3");
            }
            //else
            //{
            //    divErrorMsg.Visible = true;
            //    errorMsg = "Invalid Details";
            //}

        }
        private void GetPGDetails(string responseId, string cityId,string offerId,string versionId)
        {
            /*
            IUnityContainer container = new UnityContainer();
            container.RegisterType<IOffersRepository, OffersRepository>();

            objOffer = container.Resolve<IOffersRepository>().GetOfferPGDetailsByCouponCode(responseId,cityId,offerId,versionId);*/
           
            Trace.Warn("objoffer " + objOffer.CityName + objOffer.CustEmail + objOffer.ResponseId + objOffer.MobileNo + objOffer.OfferDesc + objOffer.CarName);
        }

        void BeginTxn(string sourceType)
        {
            string transresp = string.Empty;
            PGCookie.CustomerCity = objOffer.CityName;
            PGCookie.PQCarName = objOffer.CarName;
            PGCookie.PQCarImage = objOffer.CarImage;
            PGCookie.PQOfferDesc = Format.RemoveHtmlTags(objOffer.OfferDesc);
            PGCookie.CustomerName = objOffer.CustName;
            PGCookie.CustomerEmail = objOffer.CustEmail;
            PGCookie.CustomerMobile = objOffer.MobileNo;
            var amountReal = CurrentUser.Id == "2178" ? 2 : Convert.ToUInt16(ConfigurationManager.AppSettings["BookingAmount"].ToString());

            if (PGCookie.ResponseId.ToString() != "" && Convert.ToInt32(PGCookie.ResponseId) > 0)
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
                    CustomerName = objOffer.CustName,
                    CustEmail = objOffer.CustEmail,
                    CustMobile = objOffer.MobileNo,
                    CustCity = objOffer.CityName,
                    PlatformId = 74,  //Android
                    ApplicationId = 1, //Carwale
                    RequestToPGUrl = "https://" + HttpContext.Current.Request.ServerVariables["HTTP_HOST"].ToString() + "/new/RedirectToBillDesk.aspx",
                    ReturnUrl = "https://" + HttpContext.Current.Request.ServerVariables["HTTP_HOST"].ToString() + "/new/billdeskresponse.aspx?sourceId=74"
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
                    divErrorMsg.Visible = true;
                    errorMsg = "Transaction Failure";
                }
            }
            else
            {
                divErrorMsg.Visible = true;
                errorMsg = "Invalid Details";
            }
        }
    }
}