using System;
using System.Linq;

namespace Bikewale.Mobile.controls
{
    public class MPopupWidget : System.Web.UI.UserControl
    {
        public string ClientIP { get { return Bikewale.Common.CommonOpn.GetClientIP(); } }
        public uint CityId { get; set; }
        public uint AreaId { get; set; }
        public uint ModelId { get; set; }

        protected bool isOperaBrowser = false;
        protected bool isCitySelected = false;
        protected bool isAreaSelected = false;


        protected void Page_Load(object sender, EventArgs e)
        {
            DetectOperaBrowser();
            CheckCityCookie();
        }

        #region Set User Location from cookie
        /// <summary>
        /// Created By : Sushil Kumar on 4th August 2016
        /// Description : To set user location from the location cookie,if not obtained from customer object 
        /// </summary>
        private void CheckCityCookie()
        {
            try
            {
                var cookies = this.Context.Request.Cookies;
                if (cookies.AllKeys.Contains("location"))
                {
                    string cookieLocation = cookies["location"].Value;
                    if (!String.IsNullOrEmpty(cookieLocation) && cookieLocation.IndexOf('_') != -1)
                    {
                        string[] locArray = cookieLocation.Split('_');

                        if (Convert.ToUInt16(locArray[0]) > 0)
                        {
                            CityId = Convert.ToUInt32(locArray[0]);
                            if (locArray.Length > 3)
                                AreaId = Convert.ToUInt32(locArray[2]);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, String.Format("{0} {1}", Request.ServerVariables["URL"], "CheckCityCookie"));
                objErr.SendMail();
            }
        }
        #endregion

        /// <summary>
        /// Created By  : Sushil Kumar on 2nd August 2016
        /// Description : To detect opera min and uc min browser
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