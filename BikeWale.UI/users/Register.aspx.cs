using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Bikewale.Common;
using Bikewale.Controls;

namespace BikWale.Users
{
    public class Registration : System.Web.UI.Page
    {
        protected LoginControl ctlLogin;
        protected RegisterControl ctlRegister;

        protected override void OnInit(EventArgs e)
        {
            base.Load += Page_Load;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            
        }
    }
}