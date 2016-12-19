using Bikewale.Notifications;
using System;

namespace Bikewale.Controls
{
    public class DealersByBrand : System.Web.UI.UserControl
    {
        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "DealersByBrand.Page_Load()");
                objErr.SendMail();
            }
        }
    }
}