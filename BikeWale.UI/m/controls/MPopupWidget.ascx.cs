using System;

namespace Bikewale.Mobile.controls
{
    public partial class MPopupWidget : System.Web.UI.UserControl
    {
        public string ClientIP { get { return Bikewale.Common.CommonOpn.GetClientIP(); } }
        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}