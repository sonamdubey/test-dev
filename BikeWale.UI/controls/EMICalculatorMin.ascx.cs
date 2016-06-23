using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Bikewale.Controls
{
    public class EMICalculatorMin : System.Web.UI.UserControl
    {
        protected float rateOfInterest { get; set; }
        
        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            rateOfInterest = 12.5f;
        }
    }
}