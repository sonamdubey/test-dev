using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace BikeWaleOpr.MVC.UI.common
{
    /// <summary>
    /// Written By : Ashish G. Kamble on 22 Dec 2016
    /// Summary : Class to get the logged in user information from the user cookie
    /// </summary>
    public static class CurrentUser
    {
        /// <summary>
        /// Property to get the logged in user's id
        /// </summary>
        public static string Id
        {
            get
            {
                string userId = "-1";
                if (HttpContext.Current.User.Identity.IsAuthenticated == true)
                {
                    FormsIdentity fi = (FormsIdentity)HttpContext.Current.User.Identity;
                    FormsAuthenticationTicket ticket = fi.Ticket;
                    userId = ticket.UserData.Split(':')[0].ToString();
                }

                return userId;
            }
        }

        /// <summary>
        /// Property to get the logged in user's login id
        /// </summary>
        public static string LoginId
        {
            get
            {
                string userId = "-1";
                if (HttpContext.Current.User.Identity.IsAuthenticated == true)
                {
                    FormsIdentity fi = (FormsIdentity)HttpContext.Current.User.Identity;
                    FormsAuthenticationTicket ticket = fi.Ticket;
                    userId = ticket.UserData.Split(':')[1].ToString();
                }

                return userId;
            }
        }

        /// <summary>
        /// Property to get the logged in user's name
        /// </summary>
        public static string UserName
        {
            get
            {
                string userId = "";
                if (HttpContext.Current.User.Identity.IsAuthenticated == true)
                {
                    FormsIdentity fi = (FormsIdentity)HttpContext.Current.User.Identity;
                    FormsAuthenticationTicket ticket = fi.Ticket;
                    userId = ticket.UserData.Split(':')[2].ToString();
                }

                return userId;
            }
        }
    }
}