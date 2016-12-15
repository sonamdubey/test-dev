using System;

namespace Bikewale.Controls
{
    public class ServiceCentersByBrand : System.Web.UI.UserControl
    {
        public string staticUrl1 = System.Configuration.ConfigurationManager.AppSettings["staticUrl"];
        public string staticFileVersion1 = System.Configuration.ConfigurationManager.AppSettings["staticFileVersion"];
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