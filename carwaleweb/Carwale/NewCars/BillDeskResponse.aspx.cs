using Carwale.BL.PaymentGateway;
using Carwale.DAL.PaymentGateway;
using Carwale.Entity.Enum;
using Carwale.Entity.PaymentGateway;
using Carwale.Interfaces.PaymentGateway;
using Carwale.Utility;
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
using Carwale.Notifications;
using Carwale.Interfaces.Classified.SellCar;
using Carwale.DAL.Classified.SellCar;

namespace Carwale.UI.NewCars
{
    public class BillDeskResponse : System.Web.UI.Page
    {
        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
        }

        void InitializeComponent()
        {
            base.Load += new EventHandler(Page_Load);
        }
        
        protected void Page_Load(object sender, EventArgs e)
        {
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
            IUnityContainer containerTran = new UnityContainer();
            containerTran.RegisterType<ITransaction, Transaction>()
                        .RegisterType<IPaymentGateway, BillDesk>()
                        .RegisterType<ITransactionRepository, TransactionRepository>()
                        .RegisterType<IPackageRepository, PackageRepository>()
                        .RegisterType<ITransactionValidator, ValidateTransaction>()
                        .RegisterType<ISellCarRepository, SellCarRepository>();

            ITransaction completetransaction = containerTran.Resolve<ITransaction>();
            bool transresp = completetransaction.CompleteBillDeskTransaction();
            Trace.Warn("transresp : " + transresp);

            if (!String.IsNullOrEmpty(PGCookie.PGResponseUrl))
                Response.Redirect(PGCookie.PGResponseUrl);

            if (Convert.ToInt16(PGCookie.PGRespCode) == Convert.ToInt16(BillDeskTransactionStatusCode.Successfull))
            {
                PaymentSucc(PGCookie.CouponId, PGCookie.ResponseId, PGCookie.VersionId, PGCookie.PGTransId);
            }
            

            if (Request.QueryString["sourceid"] != null && Request.QueryString["sourceid"] != "")
            {
                if (Request.QueryString["sourceid"].ToString() == "1")
                    HttpContext.Current.Response.Redirect("/new/BillDeskResponseSucc.aspx");
                if (Request.QueryString["sourceid"].ToString() == "43" || Request.QueryString["sourceid"].ToString() == "74")
                    HttpContext.Current.Response.Redirect("/m/research/BookingConfirmation.aspx");
            }
        }

        private void PaymentSucc(string couponId, string responseId, string versionId, string bookNo)
        {
            HttpResponseMessage _response = null;
            try
            {
                using (var client = new HttpClient())
                {
                    string objResponse = string.Empty;
                    string hostUrl = "https://" + HttpContext.Current.Request.ServerVariables["HTTP_HOST"].ToString() + "/";
                    string requestType = "application/json";
                    string apiUrl = "webapi/offers/paymentSuccess?couponid=" + couponId + "&responseid=" + responseId + "&versionid=" + versionId + "&bookno=" + bookNo;
                    string response = string.Empty;

                    client.BaseAddress = new Uri(hostUrl);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(requestType));
                    client.DefaultRequestHeaders.Add("CWK", "KYpLANI09l53DuSN7UVQ304Xnks=");
                    client.DefaultRequestHeaders.Add("SourceId", "74");

                    _response = client.GetAsync(apiUrl).Result;
                    _response.EnsureSuccessStatusCode(); //Throw if not a success code.                    

                    if (_response.IsSuccessStatusCode)
                    {
                        if (_response.StatusCode == System.Net.HttpStatusCode.OK) //Check 200 OK Status        
                            objResponse = _response.Content.ReadAsStringAsync().Result;
                    }
                }
            }
            catch (Exception err)
            {
                ExceptionHandler objErr = new ExceptionHandler(err, "BillDeskResponse.PaymentSucc()" + _response.StatusCode);
                objErr.LogException();
            }
        }


    }
}
