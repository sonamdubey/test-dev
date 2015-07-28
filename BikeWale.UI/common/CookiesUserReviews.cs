using System;
using System.Collections.Generic;
using System.Web;

/// <summary>
/// Summary description for CookiesUserReviews
/// </summary>

namespace Bikewale.Common
{
    public class CookiesUserReviews
    {
        public static string URHelpful
        {
            get
            {
                string val = "";

                if (HttpContext.Current.Request.Cookies["URHelpful"] != null &&
                    HttpContext.Current.Request.Cookies["URHelpful"].Value.ToString() != "")
                {
                    val = HttpContext.Current.Request.Cookies["URHelpful"].Value.ToString();
                }
                else
                    val = "";

                return val;
            }
            set
            {
                HttpCookie objCookie;
                objCookie = new HttpCookie("URHelpful");
                objCookie.Value = value;
                HttpContext.Current.Response.Cookies.Add(objCookie);
            }
        }
    }
}