using System;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Bikewale.Common;
using System.Web.Security;
using System.Web.UI.HtmlControls;

namespace Bikewale.Common 
{
	public class CustomersLogin
	{
		// Individual Login
		public bool DoLogin(string loginId, string passwdEnter, bool rememberMe )
		{	
			bool retVal = false;			
			string userId = "", name = "";						
			
			try
			{
		        // Check whether given password and password in db matches or not.
                RegisterCustomer objCust = new RegisterCustomer();
                Customers objCustomers = objCust.IsValidPassword(passwdEnter, loginId);

				//check the password is valid or not
                if (!String.IsNullOrEmpty(objCustomers.Id))
				{
                    userId = objCustomers.Id;
                    name = objCustomers.Name;

					CurrentUser.StartSession( name, userId, loginId);
						
					// if visitor intends to remain login forever
					if ( rememberMe )
					{
						string credentials = "";
						// create credentials like in the following format
						// userId~userName~Email~Password~isEmailVerified 
                        credentials = HttpUtility.UrlEncode(BikewaleSecurity.EncryptUsingSymmetric("rememberme", userId)) + "~"
                                    + HttpUtility.UrlEncode(BikewaleSecurity.EncryptUsingSymmetric("rememberme", name)) + "~"
                                    + HttpUtility.UrlEncode(BikewaleSecurity.EncryptUsingSymmetric("rememberme", loginId)) + "~";
							
						HttpCookie rememberCookie = new HttpCookie( "RememberMe" );
						rememberCookie.Value = credentials;
						rememberCookie.Expires = DateTime.Now.AddYears(1);
						HttpContext.Current.Response.Cookies.Add( rememberCookie );
					}
								
					retVal = true;
				}
				else
				{
					retVal = false;	
				}
			}
			catch(Exception err)
			{
				HttpContext.Current.Trace.Warn(err.Message);
				ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"]);
				objErr.SendMail();
			} // catch Exception

            return retVal;
		}
		
		/* 
			Dealer login
			This function will return true if dealer login successfull else will return false;
			Added in this class on 16-May-09 By Satish Sharma			
		*/
		public bool DoDealerLogin(string loginId, string passwdEnter, bool rememberMe)
		{
            throw new Exception("Method not used/commented");

            //bool retVal = false;
			
            //SqlConnection con;
            //SqlCommand cmd;
            //SqlParameter prm;
            //Database db = new Database();
						
            //string conStr = db.GetConString();			
            //con = new SqlConnection( conStr );
			
            //try
            //{
            //    HttpContext.Current.Trace.Warn( "Submitting Data" );
            //    cmd = new SqlCommand("CHECKLOGIN", con);
            //    cmd.CommandType = CommandType.StoredProcedure;
			
            //    prm = cmd.Parameters.Add("@LOGINID", SqlDbType.VarChar, 30);
            //    prm.Value = loginId ;
				
            //    prm = cmd.Parameters.Add("@PASSWD", SqlDbType.VarChar, 20);
            //    prm.Direction = ParameterDirection.Output;
				
            //    prm = cmd.Parameters.Add("@USERROLE", SqlDbType.VarChar, 20);
            //    prm.Direction = ParameterDirection.Output;
								
            //    prm = cmd.Parameters.Add("@USERID", SqlDbType.BigInt);
            //    prm.Direction = ParameterDirection.Output;
				
               //  Bikewale.Notifications.LogLiveSps.LogSpInGrayLog(cmd);						
            //    prm = cmd.Parameters.Add("@ORGANIZATION", SqlDbType.VarChar, 50);
            //    prm.Direction = ParameterDirection.Output;
												
            //    con.Open();
            //    //run the command
            //    cmd.ExecuteNonQuery();
				
            //    string passwdReturn = cmd.Parameters[1].Value.ToString();
            //    string userRole = cmd.Parameters[2].Value.ToString();		
            //    string userId = cmd.Parameters[3].Value.ToString();
            //    string organization = cmd.Parameters[4].Value.ToString();								
								
            //    if(userId == "-1")
            //    {
            //        retVal = false;	
            //    }
            //    else
            //    {
            //        //check the password
            //        if(passwdEnter == passwdReturn)
            //        {
						
						
            //            //clear all the cookies
            //            ClearAllCookiesValues();
						
            //            //create a ticket and add it to the cookie
            //            FormsAuthenticationTicket ticket;
            //            //now add the id and the role to the ticket, concat the id and role, separated by ',' 
            //            ticket = new FormsAuthenticationTicket(1, loginId, DateTime.Now, DateTime.Now.AddHours(10), false,userId + ":" + userRole.ToUpper() + ":" + organization);
																		
						
            //            //add the ticket into the cookie
            //            HttpCookie objCookie;
            //            objCookie = new HttpCookie(".ASPXAUTH");
            //            objCookie.Value = FormsAuthentication.Encrypt(ticket);
            //            HttpContext.Current.Response.Cookies.Add(objCookie);
								
            //            retVal = true;	
            //        }
            //        else
            //        {
            //            retVal = false;	
            //        }
            //    }
            //}
            //catch(SqlException err)
            //{
            //    //catch the sql exception. if it is equal to 2627, then say that it is for duplicate entry 
            //    HttpContext.Current.Trace.Warn(err.Message);
            //    ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"]);
            //    objErr.SendMail();
			
				
            //} // catch SqlException
            //catch(Exception err)
            //{
            //    HttpContext.Current.Trace.Warn(err.Message);
            //    ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"]);
            //    objErr.SendMail();
            //} // catch Exception
            //finally
            //{
            //    //close the connection	
            //    if(con.State == ConnectionState.Open)
            //    {
            //        con.Close();
            //    }
            //}
		
            //return retVal;
		}
		
		private void ClearAllCookiesValues()
		{
			//this function clears all the cookies values, and is to be updated 
			//in case new cookies are added
			int i;
			int limit = HttpContext.Current.Request.Cookies.Count - 1;
			for(i = 0;i <= limit ;i++ )
			{
				HttpCookie aCookie = HttpContext.Current.Request.Cookies[i];
				aCookie.Expires = DateTime.Now.AddDays(-1);
				HttpContext.Current.Response.Cookies.Add(aCookie);
			}
		}
	}
}