using System;
using System.Web;

namespace Bikewale.Used
{
    /// <summary>
    /// Created By : Ashish G. Kamble on 13 Dec 2012
    /// Summary : Class to manage cookies related to used bikes.
    /// </summary>
    public static class ClassifiedCookies
    {
        /// <summary>
        ///     Cookie for the search criteria selected by customer.
        /// </summary>
        ///Modified By : Ashwini Todkar on 16 April 2014 
        ///Summary     : if cookie not exist then returns empty string
        public static string UsedSearchQueryString
        {
            get
            {
                string val = "";	//default false
                
                if (HttpContext.Current.Request.Cookies["_USQueryString"] != null && HttpContext.Current.Request.Cookies["_USQueryString"].Value.ToString() != "")
                {
                    val = HttpContext.Current.Request.Cookies["_USQueryString"].Value.ToString();
                }

                return val;
            }
            set
            {
                HttpCookie objCookie;
                objCookie = new HttpCookie("_USQueryString");
                objCookie.Value = value;               
                HttpContext.Current.Response.Cookies.Add(objCookie);
            }
        }
    }   // End of class
}   // End of namespace