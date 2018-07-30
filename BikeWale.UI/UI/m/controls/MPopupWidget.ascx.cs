using Bikewale.Entities.Location;
using Bikewale.Utility;
using System;

namespace Bikewale.Mobile.Controls
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
            GlobalCityAreaEntity cityArea = GlobalCityArea.GetGlobalCityArea();
            CityId = cityArea.CityId;
            AreaId = cityArea.AreaId;
        }



        /// <summary>
        /// Created By  : Sushil Kumar on 2nd August 2016
        /// Description : To detect opera mini and uc mini browser
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