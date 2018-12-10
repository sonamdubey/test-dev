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

namespace Carwale.UI.MyCarwale
{
    public class MyPayments : Page
    {
        protected Repeater rptPayments;
        public string customerId;
        public int serial = 0;

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
            if (!IsPostBack)
            {
                FillRepeaters();
            }
        }


        void FillRepeaters()
        {
            try
            {
                // op.BindRepeaterReader(sql, rptPayments, param);
                if (!string.IsNullOrEmpty(customerId))
                {
                    using (IUnityContainer container = new UnityContainer())
                    {
                        container.RegisterType<IPackageRepository, PackageRepository>();
                        IPackageRepository pckgRepo = container.Resolve<IPackageRepository>();
                        rptPayments.DataSource = pckgRepo.GetPaymentsDetails(Convert.ToInt32(customerId));
                        rptPayments.DataBind();
                    }
                }

            }
            catch (SqlException err)
            {
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }
    }
}