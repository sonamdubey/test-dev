using System;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Text.RegularExpressions;
using System.Net.Mail;
using MobileWeb.DataLayer;
using System.Globalization;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using Carwale.DAL.Customers;
using Carwale.Notifications;

namespace MobileWeb.Common 
{
	public class CommonOpn
	{
		private HttpContext objTrace = HttpContext.Current;						
		
		//this function expires the cookie for the needing of the contact information
		public void ExpireNeedContactInformation()
		{
			HttpContext.Current.Response.Cookies["NeedContactInformation"].Expires = DateTime.Now.AddYears( -1 );		
		}
		
        /// <summary>
        /// modified by sachin bharti 22/9/15
        /// purpose : remove 0 from regular expression
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
		public static bool CheckId( string input )
		{
			bool retVal = false;
			try
			{
				//check with the regular expression
                if (input != "0" && Regex.IsMatch(input, @"^[0-9]+$") == true)
				{
					//check its length
					if(input.Length <=9)
					{
						retVal = true;
					}
					else
					{
						retVal = false;
					}
				}
				else
				{
					retVal = false;
				}
			}
			catch(Exception err)
			{
				HttpContext.Current.Trace.Warn(err.Message);
				retVal = false;
			}
			
			return retVal;
		} // CheckId
		
		public static bool CheckUserHandle( string userId )
		{
			bool retVal = false;
			IDataReader dr = null;
            MobileWeb.DataLayer.Users obj = new MobileWeb.DataLayer.Users();
			try
			{
				obj.GetReader = true;
				obj.CheckUserHandle(userId);
				dr = obj.drReader;
				if (dr.Read())
					retVal = true;
			}
			catch(Exception err)
			{
				ErrorClass objErr = new ErrorClass(err,HttpContext.Current.Request.ServerVariables["URL"]);
				objErr.SendMail();
			}
			finally
			{
				obj.drReader.Close();
			}
			return retVal;
		}
		
		/// <summary>
       /// To return standard values of model ratings
       /// </summary>
       /// <param name="ratings"></param>
       /// <returns>image class name(string)</returns>
       public static string GetRateImage(double ratings)
       {
           try
           {
               int absRating = Convert.ToInt32(Math.Floor(ratings));
               return string.Format("{0}-rating",Enum.GetName(typeof(Carwale.Entity.Enum.StarRatings), (int)((ratings>absRating?absRating+0.5:absRating)*10)));
           }
           catch (Exception ex)
           {
               ExceptionHandler objErr = new ExceptionHandler(ex, "CommonOpn.GetRateImage()\n Exception : " + ex.Message);
               objErr.LogException();
           }
           return string.Format("{0}-rating",Carwale.Entity.Enum.StarRatings.empty.ToString());
       }

		public static bool HandleAvailable( string handleName )
		{
			bool retVal = true;
			SqlDataReader dr = null;
            MobileWeb.DataLayer.Users obj = new MobileWeb.DataLayer.Users();
			try
			{
				obj.GetReader = true;
				obj.HandleAvailable(handleName);
				dr = obj.dr;

				
				if (dr.Read())
				{
					retVal = false;
				}
			}
			catch(Exception err)
			{
				//HttpContext.Current.Response.Write(err.Message + "<br><br>" + err.StackTrace);
				retVal = false;
				ErrorClass objErr = new ErrorClass(err,HttpContext.Current.Request.ServerVariables["URL"]);
				objErr.SendMail();
			}
			finally
			{
				obj.dr.Close();
			}
			return retVal;
		}
		
		public static bool IsUserBanned( string customerId )
		{
			bool retVal = false;
			IDataReader dr = null;
			Forum obj = new Forum();
			try
			{
				obj.GetReader = true;
				obj.IsUserBanned(customerId);
				dr = obj.drReader;
				
				if (dr.Read())
				{
					retVal = true;
				}
			}
			catch(Exception err)
			{
				ErrorClass objErr = new ErrorClass(err,HttpContext.Current.Request.ServerVariables["URL"]);
				objErr.SendMail();
			}
			finally
			{
				obj.drReader.Close();
			}
			return retVal;
		}
		
		public void SendMail(string email, string subject, string body, bool htmlType)
		{
			try
            {
                SmtpClient client = new SmtpClient(ConfigurationManager.AppSettings["SMTPSERVER"]);//"127.0.0.1";
                string localMail = ConfigurationManager.AppSettings["localMail"].ToString();
                MailAddress from = new MailAddress(localMail, "CarWale.com");
                MailAddress to = new MailAddress(email);
                MailMessage msg = new MailMessage(from, to);
                msg.Headers.Add( "Reply-to", "contact@carwale.com" );
                msg.IsBodyHtml = true;
                msg.Priority = MailPriority.High;
                msg.Subject = subject;                           
                msg.Body = body;
                client.Send(msg);
            }
            catch(Exception err)
            {
                ErrorClass objErr = new ErrorClass(err,"SendMail in CommonOpn");
                objErr.SendMail();
            }			
			
		}
		
		public static string ParseLandlineNumber(string input)
		{
			//get only the numeric data
			char [] chars = input.ToCharArray();
			string raw = "";
						
			for(int i = 0; i < chars.Length; i++) 
			{
				if(Regex.IsMatch(chars[i].ToString(), @"^[0-9]$") == true || chars[i].ToString() == "-")
				{
					raw += chars[i].ToString();
				}
			}
			
			//if the number is less than 10
			if(raw.Length < 6)
				return "";
						
			//get the last 10 characters if it is greater than 10
			if(raw.Length > 11)
				raw = raw.Substring(raw.Length - 11, 11);
				
			return raw;
		}
		
		public static string ParseMobileNumber(string input)
		{
			//get only the numeric data
			char [] chars = input.ToCharArray();
			string raw = "";
						
			for(int i = 0; i < chars.Length; i++) 
			{
				if(Regex.IsMatch(chars[i].ToString(), @"^[0-9]$") == true)
				{
					raw += chars[i].ToString();
				}
			}
			
			//if the number is less than 10
			if(raw.Length < 10)
				return "";
			
			//get the last 10 characters if it is greater than 10
			if(raw.Length > 10)
				raw = raw.Substring(raw.Length - 10, 10);
			
				
			return raw;
		}
		
		public static string FormatNumeric( string numberToFormat ) 
		{
			string formatted = "";
			int breakPoint = 3;
            if (!string.IsNullOrEmpty(numberToFormat))
            {
                for (int i = numberToFormat.Length - 1; i >= 0; i--)
                {
                    formatted = numberToFormat[i].ToString() + formatted;
                    if ((numberToFormat.Length - i) == breakPoint && numberToFormat.Length > breakPoint)
                    {
                        HttpContext.Current.Trace.Warn(formatted);
                        formatted = "," + formatted;
                        breakPoint += 2;
                    }
                }
            }
			
			return formatted;
		}
			
		public static string FormatSpecial( string url )
		{
			string reg = @"[^/\-0-9a-zA-Z\s]*"; // everything except a-z, 0-9, / and -
	
			url = Regex.Replace(url, reg, "", RegexOptions.IgnoreCase);
					
			return url.Replace(" ","").Replace("-","").Replace("/","").ToLower();
		}

        public static string RemoveAnchorTag(string str)
        {
            str = Regex.Replace(str, @"<a [^>]+>(.*?)<\/a>", "$1");
            return str;
        }

        public void SendRegMail(string customerId)
        {
            try
            {
                StringBuilder message = new StringBuilder();
                string subject = "CarWale Registration.";

                CustomerDetails cd = new CustomerDetails(customerId);
               
                message.Append("<h4>Dear " + cd.Name + ",</h4>");

                message.Append("<p>Greetings from Carwale!</p>");

                message.Append("<p>Thank you for choosing Carwale. ");
                message.Append(" We are committed to making your car buying and selling process simpler.</p>");


                //Author:Rakesh
                //added: 08/07/2013
                string cipher = Utils.Utils.EncryptTripleDES(customerId.ToString());

                HttpContext.Current.Trace.Warn("cipher" + cipher);

                message.Append("<p>Please <a target=\"_blank\" href=\"https://www.carwale.com/users/verifyEmail.aspx?verify=");
                message.Append(cipher + "\">click here</a>");
                message.Append(" to activate your account or copy and paste the following link in the browser’s address-bar.</p>");
                message.Append("<a target=\"_blank\" href=\"https://www.carwale.com/users/verifyEmail.aspx?verify=");
                message.Append(cipher + "\">https://www.carwale.com/users/verifyEmail.aspx?verify=" + cipher + "</a>");

                //08/07/2013

                message.Append("<br>Warm Regards,<br><br>");
                message.Append("Customer Care,<br><b>CarWale</b>");

                SendMail(cd.Email, subject, message.ToString(), true);
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            } // catch Exception
        }       

    }
}	