using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Bikewale
{
    public partial class test : System.Web.UI.Page
    {        
        protected void Page_Load(object sender, EventArgs e)
        {

            if (HttpContext.Current.Session == null)
            {
                Trace.Warn("session is disabled");
            }
            else {
                Trace.Warn("session is enabled", HttpContext.Current.Session.Mode.ToString());
            }
        }
    }
}