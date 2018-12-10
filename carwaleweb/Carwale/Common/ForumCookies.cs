using System;
using System.Collections.Generic;
using System.Web;

/// <summary>
/// Summary description for ForumCookies
/// </summary>
/// 
namespace Carwale.UI.Common
{
    public class ForumCookies
    {
        public static string ForumUserTrackingCookie
        {
            get
            {
                string val = "";	//default false

                if (HttpContext.Current.Request.Cookies["ForumUserTrackingCookie"] != null &&
                    HttpContext.Current.Request.Cookies["ForumUserTrackingCookie"].Value.ToString() != "")
                {
                    val = HttpContext.Current.Request.Cookies["ForumUserTrackingCookie"].Value.ToString();
                }
                else
                {
                    val = "-1";
                }

                return val;
            }
            set
            {
                HttpCookie objCookie;
                objCookie = new HttpCookie("ForumUserTrackingCookie");
                objCookie.Value = value;
                objCookie.Path = "/forums";
                HttpContext.Current.Response.Cookies.Add(objCookie);
            }
        }
    }//class
}//namespace