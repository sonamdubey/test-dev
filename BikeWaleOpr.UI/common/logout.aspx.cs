using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BikeWaleOpr.Common;
using System.Configuration;

namespace BikeWaleOpr
{
    public class Logout : Page
    {
        protected override void OnInit(EventArgs e)
        { 
            this.Load += new EventHandler(Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                bool logout = false;               
                
                if (Request["logout"] != null && Request.QueryString["logout"] == "logout")
                {
                    logout = true;
                    Trace.Warn("logout");
                }

                if (logout == true)
                {
                    CurrentUser.EndSession();
                    BikeWaleAuthentication.ClearAllCookiesValues();
                    //Response.Redirect(ConfigurationManager.AppSettings["redirectUrl"]);
                    Response.Redirect("/users/login.aspx");
                }

            }
        }
    }
}