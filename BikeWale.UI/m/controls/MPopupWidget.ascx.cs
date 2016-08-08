using System;

namespace Bikewale.Mobile.controls
{
    public class MPopupWidget : System.Web.UI.UserControl
    {
        public string ClientIP { get { return Bikewale.Common.CommonOpn.GetClientIP(); } }
        public uint CityId { get; set; }
        public uint AreaId { get; set; }
        public uint ModelId { get; set; }

        protected bool isOperaBrowser = false;


        protected void Page_Load(object sender, EventArgs e)
        {
            DetectOperaBrowser();
        }

        /// <summary>
        /// Created By  : Sushil Kumar on 2nd August 2016
        /// Description : 
        /// </summary>
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