using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Bikewale.Common;

namespace Bikewale.MyBikewale
{
    /// <summary>
    ///     Created By : Ashish G. Kamble on 9/3/2012
    /// </summary>
    public class Default : Page
    {

        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                if (CurrentUser.Id != "-1")
                {
                    Response.Redirect("/mybikewale/mylisting.aspx");
                }
            }
        }

    }   // End of class
}   // End of namespace