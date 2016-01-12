using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Bikewale.Utility
{
    /// <summary>
    /// Created By  :   Sadhana Upadyay on 29 Dec 2015
    /// Summary     :   
    /// </summary>
    public class CurrentUser
    {
        /// <summary>
        /// Created By  :   Sadhana Upadyay on 29 Dec 2015
        /// Summary     :   
        /// </summary>
        public static void GenerateUniqueCookie()
        {
            HttpCookie bwc = HttpContext.Current.Request.Cookies["BWC"];
            if (bwc == null)
            {
                //create the cwc cookie with a unique random value
                string bwcCookieValue = RandomNoGenerator.GetUniqueKey(25);
                HttpCookie bwcCookie = new HttpCookie("BWC");
                bwcCookie.Value = bwcCookieValue;
                bwcCookie.Expires = DateTime.Now.AddYears(5);
                HttpContext.Current.Response.Cookies.Add(bwcCookie);
            }
        }

        public static long GetCurrentUnixTimeStamp()
        {
            DateTime unixTimeStamp = new DateTime(1970, 1, 1);	//Jan 1 1970
            TimeSpan diff = DateTime.Now - unixTimeStamp;
            return (long)diff.TotalSeconds;
        }
    }
}
