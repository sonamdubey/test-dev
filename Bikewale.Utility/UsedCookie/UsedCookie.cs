
using RabbitMqPublishing.Common;
using System;
using System.Collections.Generic;
using System.Web;
namespace Bikewale.Utility.UsedCookie
{
    /// <summary>
    /// Created By : Subodh Jain on 2 jan 2017 
    /// Description : Used cookie for pages
    /// </summary>
    public class UsedCookie
    {
        private const string NAME = "Used";
        private const string NAME_BrandIndia = "BrandIndia", NAME_BrandCity = "BrandCity", NAME_ModelIndia = "ModelIndia", NAME_UsedCity = "UsedCity";
        public static bool BrandIndia
        {
            get
            {
                if (HttpContext.Current.Request.Cookies[NAME] != null && !String.IsNullOrEmpty(HttpContext.Current.Request.Cookies[NAME][NAME_BrandIndia]))
                {
                    return HttpContext.Current.Request.Cookies[NAME][NAME_BrandIndia] == "1";
                }
                return false;
            }
            set
            {
                HttpContext.Current.Response.Cookies[NAME][NAME_BrandIndia] = Convert.ToString(value);
            }
        }
        public static bool BrandCity
        {
            get
            {
                if (HttpContext.Current.Request.Cookies[NAME] != null && !String.IsNullOrEmpty(HttpContext.Current.Request.Cookies[NAME][NAME_BrandCity]))
                {
                    return HttpContext.Current.Request.Cookies[NAME][NAME_BrandCity] == "1";
                }
                return false;
            }
            set
            {
                HttpContext.Current.Response.Cookies[NAME][NAME_BrandCity] = Convert.ToString(value);
            }
        }
        public static bool ModelIndia
        {
            get
            {
                if (HttpContext.Current.Request.Cookies[NAME] != null && !String.IsNullOrEmpty(HttpContext.Current.Request.Cookies[NAME][NAME_ModelIndia]))
                {
                    return HttpContext.Current.Request.Cookies[NAME][NAME_ModelIndia] == "1";
                }
                return false;
            }
            set
            {
                HttpContext.Current.Response.Cookies[NAME][NAME_ModelIndia] = Convert.ToString(value);
            }
        }
        public static bool UsedCity
        {
            get
            {
                if (HttpContext.Current.Request.Cookies[NAME] != null && !String.IsNullOrEmpty(HttpContext.Current.Request.Cookies[NAME][NAME_UsedCity]))
                {
                    return HttpContext.Current.Request.Cookies[NAME][NAME_UsedCity] == "1";
                }
                return false;
            }
            set
            {
                HttpContext.Current.Response.Cookies[NAME][NAME_UsedCity] = Convert.ToString(value);
            }
        }

        public static bool IsCookieExists { get { return (HttpContext.Current.Request.Cookies[NAME] != null ? true : false); } }
        /// <summary>
        /// Created By : Subodh Jain on 2 jan 2017 
        /// Description : Set cookie for pages
        /// </summary>
        public static void SetUsedCookie()
        {
            try
            {
                if (!IsCookieExists)
                {

                    Cookie usedCookie = new Cookie(NAME);
                    usedCookie.Values.Add(new KeyValuePair<string, string>(NAME_BrandIndia, "1"));
                    usedCookie.Values.Add(new KeyValuePair<string, string>(NAME_BrandCity, "1"));
                    usedCookie.Values.Add(new KeyValuePair<string, string>(NAME_ModelIndia, "1"));
                    usedCookie.Values.Add(new KeyValuePair<string, string>(NAME_UsedCity, "1"));
                    CookieManager.Add(usedCookie);
                }
            }
            catch (Exception ex)
            {

                ErrorClass objErr = new ErrorClass(ex, "UsedCookie.SetUsedCookie");
            }
        }
    }
}
