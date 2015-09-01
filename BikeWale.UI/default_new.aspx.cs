using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Bikewale.controls;

namespace Bikewale
{
    public class default_new : System.Web.UI.Page
    {
        protected News_new ctrlNews;

        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            ctrlNews.TotalRecords = 3;
        }
    }
}