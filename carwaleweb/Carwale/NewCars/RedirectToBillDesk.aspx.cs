using Carwale.Utility;
using System;
using System.Configuration;

namespace Carwale.UI.NewCars
{
    public class RedirectToBillDesk : System.Web.UI.Page
    {
        protected string msg = string.Empty;
        protected int submit = 0;
        protected static readonly string paymentGatewayUrl = ConfigurationManager.AppSettings["PaymentGatewayUrlBillDesk"];
        
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
            if (Request.QueryString["msg"] != null && Request.QueryString["msg"] != "")
            {
                msg = CarwaleSecurity.Decrypt(Request.QueryString["msg"].ToString());
                submit = 1;
            }           
        }
    }
}
