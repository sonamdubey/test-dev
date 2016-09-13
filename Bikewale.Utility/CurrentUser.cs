using System;
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
            HttpCookie _cwv = HttpContext.Current.Request.Cookies["_cwv"];
            if (bwc == null)
            {
                //create the cwc cookie with a unique random value
                string bwcCookieValue = RandomNoGenerator.GetUniqueKey(25);

                Cookie bwcCookie = new Cookie("BWC");
                bwcCookie.Value = bwcCookieValue;
                bwcCookie.Expires = DateTime.Now.AddYears(2);
                CookieManager.Add(bwcCookie);

                //also add the cookie for cwv, to identify the session or the visit and also the number of times this visitor has come.
                //Since this has been implemented later, hence add value for visit count only when cwc is being initialized
                //the format for _cwv cookie is : cwcValue.random_key_for_cwv.visit_start_timestamp.visit_last_timestamp.visit_count
                string bwvCookieValue = RandomNoGenerator.GetUniqueKey(10); //add 10 digit unique key for the cwv

                Cookie bwvCookie = new Cookie("_cwv");

                long currServerTimeStamp = GetCurrentUnixTimeStamp();
                bwvCookie.Value = bwcCookieValue + "." + bwvCookieValue + "." + currServerTimeStamp + "." + currServerTimeStamp + "." + currServerTimeStamp + ".1" + ".r";
                bwvCookie.Expires = DateTime.Now.AddYears(2);
                CookieManager.Add(bwvCookie);
            }
            else if (bwc != null && _cwv == null)
            {
                //also add the cookie for cwv, to identify the session or the visit and also the number of times this visitor has come.
                //Since this has been implemented later, hence add value for visit count only when cwc is being initialized
                //the format for _cwv cookie is : cwcValue.random_key_for_cwv.visit_start_timestamp.visit_last_timestamp.visit_count
                string bwvCookieValue = RandomNoGenerator.GetUniqueKey(10); //add 10 digit unique key for the cwv

                Cookie bwvCookie = new Cookie("_cwv");

                long currServerTimeStamp = GetCurrentUnixTimeStamp();
                bwvCookie.Value = bwc.Value + "." + bwvCookieValue + "." + currServerTimeStamp + "." + currServerTimeStamp + "." + currServerTimeStamp + ".1" + ".r";
                bwvCookie.Expires = DateTime.Now.AddYears(2);
                CookieManager.Add(bwvCookie);

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
