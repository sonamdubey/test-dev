using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Security;

namespace Bikewale.Utility
{
    /// <summary>
    /// Created By : Ashish G. Kamble on 7 Jan 2017
    /// Summary : Class to get the logged in users values
    /// </summary>
    public class OprUser
    {
        /// <summary>
        /// Logged in user opr id
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
        /// Logged in users login id
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
        /// Logged users username
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
