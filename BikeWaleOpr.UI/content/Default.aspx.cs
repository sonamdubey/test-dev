using System;
using System.Web.UI;

namespace BikeWaleOpr.Content
{
    public class Default : Page
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