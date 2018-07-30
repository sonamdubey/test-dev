using System;

namespace Bikewale.Controls
{
    /// <summary>
    /// Created By : Ashish G. Kamble on 3 Sept 2015
    /// Summary : Class have function to login, logout and register the user.
    /// </summary>
    public class LoginControlNew : System.Web.UI.UserControl
    {
        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }
    }
}