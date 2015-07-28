using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BikWale.Users
{
    public class Login : System.Web.UI.Page
    {
        protected override void OnInit(EventArgs e)
        {
            base.Load += Page_Load;
        }

        private void Page_Load(object sender, EventArgs e)
        {

        }
    }
}