// Current User Class
//

using System;
using System.Web;
using System.Data.SqlClient;
using System.Web.Security;
using Carwale.DAL.CoreDAL;
using Carwale.Notifications;
using Carwale.DAL.Customers;
using Carwale.Utility;

namespace Carwale.UI.Common 
{
	public class CurrentUser
	{
        ///<summary>
        /// This Method gets the current user id as logged in. 
        ///if no user is logged in then it returns -1
        ///</summary>
        public static string Id
		{
			get
			{
                return CustomerCookie.GetUserId();
			}
		}
		
		
		public static string Role
		{
			get
			{
				string userRole = "-1";
				
				if(HttpContext.Current.User.Identity.IsAuthenticated == true) 
				{
					FormsIdentity fi = (FormsIdentity)HttpContext.Current.User.Identity;
					FormsAuthenticationTicket ticket = fi.Ticket;

					string strRole 		= ticket.UserData.Split(':')[1].ToString().ToUpper();
					
					if( strRole == "DEALERS")
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
				if(HttpContext.Current.User.Identity.IsAuthenticated == true) 
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
				if(HttpContext.Current.User.Identity.IsAuthenticated == true) 
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
				if(HttpContext.Current.User.Identity.IsAuthenticated == true) 
				{
					FormsIdentity fi = (FormsIdentity)HttpContext.Current.User.Identity;
					FormsAuthenticationTicket ticket = fi.Ticket;
					userName = ticket.UserData.Split(':')[2].ToString();
				}
				if ( userName != "" )
				{
					if ( userName.ToLower() == "false" )
						isVerified = false;
					else
						isVerified = true;
				}
					
				return isVerified;
			}
		}
		
		public static void StartSession( string userName, string userId, string email, bool isEmailVerified )
		{
            HttpContext.Current.Response.Cookies.Add(
                CustomerCookie.StartSession(userName, userId, email, isEmailVerified));
		}
		
		public static void EndSession()
		{
            CustomerCookie.EndSession();
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
    }//class
}//namespace
