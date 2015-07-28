using Carwale.Utility;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace Bikewale.BikeBooking
{
    public class RedirectToBillDesk : System.Web.UI.Page
    {
        protected string msg = string.Empty;
        public int submit = 0;

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
                msg =CarwaleSecurity.Decrypt(Request.QueryString["msg"].ToString());
                submit = 1;
            }
            Trace.Warn("msg" + msg);
        }
    }
}
