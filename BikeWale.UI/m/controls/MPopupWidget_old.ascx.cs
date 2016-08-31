using System;

namespace Bikewale.Mobile.controls
{
    public class MPopupWidgetOld : System.Web.UI.UserControl
    {
        public string ClientIP { get { return Bikewale.Common.CommonOpn.GetClientIP(); } }
        public bool CityId { get; set; }
        public bool AreaId { get; set; }
        public bool ModelId { get; set; }

        protected bool isOperaBrowser = false;
        protected bool isCitySelected = false;
        protected bool isAreaSelected = false;


        protected void Page_Load(object sender, EventArgs e)
        {
            DetectOperaBrowser();
        }

        private void DetectOperaBrowser()
        {
            System.Web.HttpBrowserCapabilities browserDetection = Request.Browser;
            if (browserDetection != null)
            {
                string browserType = browserDetection.Type;
                string browser = browserDetection.Browser;
                string browserVersion = browserDetection.Version;

                if (!string.IsNullOrEmpty(browserType) && browserType.ToLower().Contains("mini"))
                    isOperaBrowser = true;
            }

        }
    }
}