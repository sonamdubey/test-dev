using System;
using System.Linq;

namespace Bikewale.Mobile.Controls
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
                if (cookies.AllKeys.Contains("location") && !String.IsNullOrEmpty(cookies["location"].Value))
                {
                    string cookieLocation = cookies["location"].Value.Replace('-', ' ');
                    if (!String.IsNullOrEmpty(cookieLocation) && cookieLocation.IndexOf('_') != -1)
                    {
                        string[] locArray = cookieLocation.Split('_');
                        if (locArray != null && locArray.Length > 0)
                        {
                            uint _cityId;
                            if (UInt32.TryParse(locArray[0], out _cityId) && _cityId > 0)
                            {
                                CityId = _cityId;
                                uint _areaId;
                                if (locArray.Length > 3 && UInt32.TryParse(locArray[2], out _areaId) && _areaId > 0)
                                    AreaId = _areaId;
                            }
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