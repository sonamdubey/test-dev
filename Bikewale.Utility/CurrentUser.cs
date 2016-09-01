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
    }
}
