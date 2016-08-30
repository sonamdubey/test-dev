using System;

namespace Bikewale.Mobile.Controls
{
    public partial class MOnRoadPricequote : System.Web.UI.UserControl
    {
        public string PageId { get; set; }
        public int PQSourceId { get; set; }
        public string ClientIP { get { return Bikewale.Common.CommonOpn.GetClientIP(); } }
        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}