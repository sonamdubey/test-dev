// Current User Class
//

using System;
using System.Web;
using System.Web.Security;

namespace BikeWaleOpr.Common
{
    public class CurrentUser
	{	  	
		public static string Id
		{
			get
			{
				string userId = "-1";
				if(HttpContext.Current.User.Identity.IsAuthenticated) 
				{
					FormsIdentity fi = (FormsIdentity)HttpContext.Current.User.Identity;
					FormsAuthenticationTicket ticket = fi.Ticket;
					userId = ticket.UserData.Split(':')[0];
				}

                return userId;
			}
		}
		
		
		public static string LoginId
		{
			get
			{
				string userId = "-1";
				if(HttpContext.Current.User.Identity.IsAuthenticated == true) 
				{
					FormsIdentity fi = (FormsIdentity)HttpContext.Current.User.Identity;
					FormsAuthenticationTicket ticket = fi.Ticket;
					userId = ticket.UserData.Split(':')[2].ToString();
				}
				
				return userId;
			}
		}
		
		
		public static string UserName
		{
			get
			{
				string userId = "";
				if(HttpContext.Current.User.Identity.IsAuthenticated == true) 
				{
					FormsIdentity fi = (FormsIdentity)HttpContext.Current.User.Identity;
					FormsAuthenticationTicket ticket = fi.Ticket;
					userId = ticket.UserData.Split(':')[1].ToString();
				}
				
				return userId;
			}
		}
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
		
		public static void StartSession( string userName, string userId, string loginId )
		{
			//create a ticket and add it to the cookie
			FormsAuthenticationTicket ticket;
			//now add the id and the role to the ticket, concat the id and role, separated by ',' 
			ticket = new FormsAuthenticationTicket(
						1, 
						userName, 
						DateTime.Now, 
						DateTime.Now.AddHours(12), 
						false,
						userId + ":" + userName + ":" + loginId  
					);
									
			//add the ticket into the cookie
			HttpCookie objCookie;
			objCookie = new HttpCookie(".ASPXAUTH");
			objCookie.Value = FormsAuthentication.Encrypt(ticket);
			HttpContext.Current.Response.Cookies.Add(objCookie);
		}
		
		public static void EndSession()
		{
			FormsAuthentication.SignOut();
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
    }//class
}//namespace