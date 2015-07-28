using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BikeWaleOpr.Common;
using System.Configuration;

namespace BikeWaleOpr
{
    public class Default : Page
    {
        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            Trace.Warn("user id ", CurrentUser.Id);
            if (CurrentUser.Id == "-1")
            {
                Response.Redirect("/users/login.aspx");
            }
        }
    }
}