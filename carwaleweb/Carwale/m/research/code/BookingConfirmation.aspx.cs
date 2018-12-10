using Carwale.BL.PaymentGateway;
using Carwale.Entity.Enum;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace MobileWeb.Research
{
    public class BookingConfirmation : System.Web.UI.Page
    {
        protected HtmlGenericControl divSuccessfull, divNotSuccessfull;
        private string respcd = "";
        public string amount, msg, transId;
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
                respcd = PGCookie.PGRespCode;
                msg = PGCookie.PGMessage;
                transId = PGCookie.PGTransId;
                amount = ConfigurationManager.AppSettings["BookingAmount"].ToString();
                //respcd = "0";
                if (Convert.ToInt16(respcd) == Convert.ToInt16(BillDeskTransactionStatusCode.Successfull))
                {
                    divSuccessfull.Visible = true;
                    divNotSuccessfull.Visible = false;
                }
                else
                {
                    divSuccessfull.Visible = false;
                    divNotSuccessfull.Visible = true;
                }

            }

        }
    }
}