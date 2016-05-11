using System;

namespace Bikewale.Controls
{
    public class OnRoadPricequote : System.Web.UI.UserControl
    {
        public string PageId { get; set; }
        public int PQSourceId { get; set; }
        public string ClientIP { get { return Bikewale.Common.CommonOpn.GetClientIP(); } }
        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }



    }
}