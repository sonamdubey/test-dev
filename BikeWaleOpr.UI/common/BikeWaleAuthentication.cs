using System;
using System.Web;
using System.Web.Security;

/// <summary>
/// Summary description for BikeWaleLogin
/// </summary>

namespace BikeWaleOpr
{
    public static class BikeWaleAuthentication
    {
        public static string GetOprUserId()
        {
            string oprId = "-1";

            if (HttpContext.Current.User.Identity.IsAuthenticated == true)
            {
                FormsIdentity fi = (FormsIdentity)HttpContext.Current.User.Identity;
                FormsAuthenticationTicket ticket = fi.Ticket;

                oprId = ticket.UserData;
                oprId = oprId.Split(':')[0];
            }
            return oprId;
        }

        //  Modified By : Ashwini Todkar on 19th dec 2013

        public static bool CreateAuthCookies(string oprId, string userName)
        {
            //check the password
            //clear all the cookies
            ClearAllCookiesValues();

            //create a ticket and add it to the cookie
            FormsAuthenticationTicket ticket;
            //now add the id and the role to the ticket, concat the id and role, separated by ',' 
            //ticket = new FormsAuthenticationTicket(1, oprId, DateTime.Now, DateTime.Now.AddHours(9), false, oprId);
            ticket = new FormsAuthenticationTicket(1, oprId, DateTime.Now, DateTime.Now.AddHours(9), false, oprId + ":" + userName);
            

            //add the ticket into the cookie
            HttpCookie objCookie;
            objCookie = new HttpCookie("_bikewaleOpr");
            objCookie.Value = FormsAuthentication.Encrypt(ticket);
            HttpContext.Current.Response.Cookies.Add(objCookie);

            return true;
        }

        public static void ClearAllCookiesValues()
        {
            //this function clears all the cookies values, and is to be updated 
            //in case new cookies are added
            int i;
            int limit = HttpContext.Current.Request.Cookies.Count - 1;
            for (i = 0; i <= limit; i++)
            {
                HttpCookie aCookie = HttpContext.Current.Request.Cookies[i];
                aCookie.Expires = DateTime.Now.AddDays(-1);
                HttpContext.Current.Response.Cookies.Add(aCookie);
            }
        }
    }
}