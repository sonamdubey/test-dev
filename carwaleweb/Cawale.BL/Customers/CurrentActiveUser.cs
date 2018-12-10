using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Security;

namespace Carwale.BL.Customers
{
    public class CurrentActiveUser
    {

        ///<summary>
        /// This Method gets the current user id as logged in. 
        ///if no user is logged in then it returns -1
        ///</summary>
        public static string Id
        {
            get
            {
                string userId = "-1";

                if (HttpContext.Current.User.Identity.IsAuthenticated == true)
                {
                    FormsIdentity fi = (FormsIdentity)HttpContext.Current.User.Identity;
                    FormsAuthenticationTicket ticket = fi.Ticket;

                    string strRole = ticket.UserData.Split(':')[1].ToString().ToUpper();
                    string strUserId = ticket.UserData.Split(':')[0].ToString();

                    userId = ticket.UserData.Split(':')[0].ToString();
                }

                return userId;
            }
        }


        public static string Role
        {
            get
            {
                string userRole = "-1";

                if (HttpContext.Current.User.Identity.IsAuthenticated == true)
                {
                    FormsIdentity fi = (FormsIdentity)HttpContext.Current.User.Identity;
                    FormsAuthenticationTicket ticket = fi.Ticket;

                    string strRole = ticket.UserData.Split(':')[1].ToString().ToUpper();

                    if (strRole == "DEALERS")
                        userRole = "DEALER";
                    else
                        userRole = "INDIVIDUAL";
                }

                return userRole;
            }
        }


        ///<summary>
        /// This Method gets the current user email as logged in. 
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

        ///<summary>
        /// This Method gets the current user name as logged in. 
        ///if no user is logged in then it returns ""
        ///</summary>
        public static string Name
        {
            get
            {
                string userName = "";
                if (HttpContext.Current.User.Identity.IsAuthenticated == true)
                {
                    userName = HttpContext.Current.User.Identity.Name;
                }

                return userName;
            }
        }

        ///<summary>
        /// This Method gets the current user name as logged in. 
        ///if no user is logged in then it returns ""
        ///</summary>
        public static bool EmailVerified
        {
            get
            {
                bool isVerified = false;
                string userName = "";
                if (HttpContext.Current.User.Identity.IsAuthenticated == true)
                {
                    FormsIdentity fi = (FormsIdentity)HttpContext.Current.User.Identity;
                    FormsAuthenticationTicket ticket = fi.Ticket;
                    userName = ticket.UserData.Split(':')[2].ToString();
                }
                if (userName != "")
                {
                    if (userName.ToLower() == "false")
                        isVerified = false;
                    else
                        isVerified = true;
                }

                return isVerified;
            }
        }

        public static void StartSession(string userName, string userId, string email, bool isEmailVerified)
        {
            //create a ticket and add it to the cookie
            FormsAuthenticationTicket ticket;
            //now add the id and the role to the ticket, concat the id and role, separated by ',' 
            ticket = new FormsAuthenticationTicket(
                        1,
                        userName,
                        DateTime.Now,
                        DateTime.Now.AddDays(365),
                        false,
                        userId + ":" + email + ":" + isEmailVerified.ToString()
                    );

            //add the ticket into the cookie
            HttpCookie objCookie;
            objCookie = new HttpCookie(".ASPXAUTH");
            objCookie.Value = FormsAuthentication.Encrypt(ticket);
            HttpContext.Current.Response.Cookies.Add(objCookie);

            // delete auction cookie if not exists
            // it will be reassign
            DeleteAuctionCookie();
        }

        public static void EndSession()
        {
            FormsAuthentication.SignOut();

            //also clear the cookie for the contact information
            ExpireNeedContactInformation();

            // delete auction cookie if not exists
            // it will be reassign
            DeleteAuctionCookie();
        }

        static void DeleteAuctionCookie()
        {
            //Delete auction cookies if exists
            if (HttpContext.Current.Request.Cookies["CookieBidderId"] != null)
            {
                HttpContext.Current.Response.Cookies["CookieBidderId"].Expires = DateTime.Now.AddYears(-1);
            }
        }

        public static string CWC
        {
            get
            {
                if (HttpContext.Current.Request.Cookies["CWC"] != null && HttpContext.Current.Request.Cookies["CWC"].Value.ToString() != "")
                {
                    return HttpContext.Current.Request.Cookies["CWC"].Value.ToString();
                }
                else return "-1";
            }
        }
        public static void ExpireNeedContactInformation()
        {
            HttpContext.Current.Response.Cookies["NeedContactInformation"].Expires = DateTime.Now.AddYears(-1);
        }
    }//class
}
