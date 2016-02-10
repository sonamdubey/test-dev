using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BikeWaleOpr.Common;
using System.Configuration;

namespace BikewaleOpr.MobileApp
{
    public partial class AppVersioning : System.Web.UI.Page
    {
        protected void Page_Load(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }
    }
}