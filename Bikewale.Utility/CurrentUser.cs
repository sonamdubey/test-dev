using System;
using System.Web;
using System.Web.Security;

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

        ///<summary>
        /// This PopulateWhere gets the current user email as logged in. 
        ///if no user is logged in then it returns ""
        ///</summary>
        public static string Email
        {
            get
            {
                string email = "";
                if (HttpContext.Current.User.Identity.IsAuthenticated == true)
                {
                    FormsIdentity fi = (FormsIdentity)HttpContext.Current.User.Identity;
                    FormsAuthenticationTicket ticket = fi.Ticket;
                    email = ticket.UserData.Split(':')[1].ToString();
                }

                return email;
            }
        }

        /// <summary>
        /// Created By : Sadhana Upadhyay on 1st April 2014
        /// Summary : Function to get Client IP Address
        /// </summary>
        /// <returns></returns>
        public static string GetClientIP()
        {
            //string clientIp = HttpContext.Current.Request.ServerVariables["HTTP_CLIENT_IP"] == null ? DBNull.Value.ToString() : HttpContext.Current.Request.ServerVariables["HTTP_CLIENT_IP"];
            //return clientIp;
            string[] serVars = { "HTTP_CLIENT_IP", "HTTP_X_FORWARDED_FOR", "HTTP_X_FORWARDED", "HTTP_X_CLUSTER_CLIENT_IP", "HTTP_FORWARDED_FOR", "HTTP_FORWARDED", "REMOTE_ADDR" };
            string clientIp = string.Empty;
            foreach (string serverVariable in serVars)
            {
                clientIp = HttpContext.Current.Request.ServerVariables[serverVariable] == null ? DBNull.Value.ToString() : HttpContext.Current.Request.ServerVariables[serverVariable];
                if (!String.IsNullOrEmpty(clientIp))
                {
                    if (serverVariable == "HTTP_X_FORWARDED_FOR")
                    {
                        if (!string.IsNullOrEmpty(clientIp))
                        {
                            string[] ipRange = clientIp.Split(',');
                            if (ipRange != null)
                            {
                                clientIp = ipRange[ipRange.Length - 1];
                            }
                        }
                    }
                    break;
                }
            }
            //string clientIp = HttpContext.Current.Request.ServerVariables["HTTP_CLIENT_IP"] == null ? DBNull.Value.ToString() : HttpContext.Current.Request.ServerVariables["HTTP_CLIENT_IP"];

            return clientIp;
        }
        /// <summary>
        /// Created By : Sushil Kumar on 14th December 2017
        /// Description : Create a random number which is assign to user for ab testing
        /// </summary>
        public static void SetBikewaleABTestingUser()
        {
            Random r = new Random((int)DateTime.Now.Ticks);
            BWCookies.SetCookie("_bwtest", 365, Convert.ToString(r.Next(1, 101)));
        }
    }
}
