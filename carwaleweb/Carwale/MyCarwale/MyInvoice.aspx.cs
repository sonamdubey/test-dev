/*******************************************************************************************************
IN THIS CLASS THE NEW MEMBEERS WHO HAVE REQUESTED FOR REGISTRATION ARE SHOWN
*******************************************************************************************************/
using System;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Carwale.UI.Controls;
using Carwale.Notifications;
using Carwale.DAL.CoreDAL;
using Carwale.UI.Common;
using Microsoft.Practices.Unity;
using Carwale.Interfaces.PaymentGateway;
using Carwale.DAL.PaymentGateway;
using Carwale.Entity.Classified;

namespace Carwale.UI.MyCarwale
{
    public class MyInvoice : Page
    {
        protected Repeater rptPayments;
        public string customerId;
        public int serial = 0;
        public string invoiceId = "";
        public string cprID, consumerName, consumerEmail, consumerContactNo,
                        consumerAddress, paymentMode, paymentModeDetails, amount,
                        packageName, packageDetails, entryDateTime;

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
            // check for login.
            if (CurrentUser.Id == "-1")
                Response.Redirect("/users/login.aspx?returnUrl=/MyCarwale/MyPayments.aspx");

            customerId = CurrentUser.Id;

            if (Request["inv"] != null && Request.QueryString["inv"] != "")
            {
                if (CommonOpn.CheckId(Request.QueryString["inv"]) == false)
                    Response.Redirect("MyPayments.aspx");

                invoiceId = Request.QueryString["inv"];
            }
            else
                Response.Redirect("MyPayments.aspx");


            GetData();

        } // Page_Load


        void GetData()
        {
            bool found = false;
            try
            {
                //get the list of subcategories
                if(!string.IsNullOrEmpty(customerId) && !string.IsNullOrEmpty(invoiceId))
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IPackageRepository, PackageRepository>();
                    IPackageRepository pckgRepo = container.Resolve<IPackageRepository>();

                    InvoiceDetails invoiceDetails = null;

                    invoiceDetails = pckgRepo.GetInvoiceDetails(Convert.ToInt32(customerId), Convert.ToInt32(invoiceId));
                    
                    found = true;

                    cprID = invoiceDetails.CprID;
                    consumerName = invoiceDetails.ConsumerName;
                    consumerEmail = invoiceDetails.ConsumerEmail;
                    consumerContactNo = invoiceDetails.ConsumerContactNo;
                    consumerAddress = invoiceDetails.ConsumerAddress;
                    paymentMode = invoiceDetails.PaymentMode;
                    paymentModeDetails = invoiceDetails.PaymentModeDetails;
                    amount = invoiceDetails.Amount;
                    packageName = invoiceDetails.PackageName;
                    packageDetails = invoiceDetails.PackageDetails;
                    entryDateTime = Convert.ToDateTime(invoiceDetails.EntryDateTime).ToString("dd-MMM,yyyy");
                }
            }
            catch (Exception err)
            {
                Trace.Warn(err.Message);
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            } // catch Exception
            
            if (found == false)
                Response.Redirect("MyPayments.aspx");
        }


    } // class
} // namespace