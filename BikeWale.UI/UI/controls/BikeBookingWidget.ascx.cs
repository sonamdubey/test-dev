using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Bikewale.Common;

namespace Bikewale.Controls
{
    /// <summary>
    /// Created By : Ashish G. Kamble
    /// </summary>
    public class BikeBookingWidget : System.Web.UI.UserControl
    {
        //protected DropDownList ddlMake;

        public string MakeId { get; set; }
        public string SeriesId { get; set; }
        public string Make { get; set; }
        public string Series { get; set; }
        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                //BindMakes();
                //MakeId = "1";
            }
        }

    }   // class
}   // namespace