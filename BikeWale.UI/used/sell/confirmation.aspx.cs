using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Bikewale.Common;

namespace Bikewale.Used
{
    public class Confirmation : System.Web.UI.Page
    {
        public string inquiryId = "-1";

        protected HtmlGenericControl msg_listing;

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
            if (!IsPostBack)
            {
                inquiryId = CookiesCustomers.SellInquiryId;

                if ((CurrentUser.Id == "-1" && !CommonOpn.CheckId(CookiesCustomers.SellInquiryId)))
                {
                    Response.Redirect("aboutcar.aspx");
                }
            }
        }

    }   // End of class
}   // End of namespace