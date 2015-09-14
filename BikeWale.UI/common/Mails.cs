// Mails Class
//
using System;
using System.Web;
using System.Configuration;
using System.Web.Mail;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;

namespace Bikewale.Common 
{
	public class Mails
	{	
		public static void DealerStockBikePurchaseInquiry( string dealerEmail, string dealer, string customerId, string customerComments, int bikeId )
		{
			try
			{
				string email = dealerEmail;
				StringBuilder message = new StringBuilder();
				string subject = "Purchase Request Arrived : Bike #" + bikeId;
				 
				CustomerDetails cd = new CustomerDetails( customerId );
                message.Append("<img align=\"right\" src=\"http://img.carwale.com/bikewaleimg/images/bw-logo.png\" />");
				message.Append( "<h4>Dear " + dealer + ",</h4>" );
				message.Append( "<p>Greetings from bikewale!</p>" );
				message.Append( "<p>bikewale has received a Used Bike Purchase Inquiry for Your Bike #" );
				message.Append( bikeId + "<p><a href=\"http://www.bikewale.com/Used/BikeDetails.aspx?bike=" );
				message.Append( bikeId + "\">Click Here</a> Or copy and paste the following link" );
				message.Append( " to know details of your bike.</p>" );
				message.Append( "<p><a href=\"http://www.bikewale.com/Used/BikeDetails.aspx?bike=" );
				message.Append( bikeId + "\">http://www.bikewale.com/Used/BikeDetails.aspx?bike=" );
				message.Append( bikeId + "</a>" );
				message.Append( "<div style=\"background-color:#B5EDBC;\"><h5>Customer Details:</h5>" );
				message.Append( "<table border=\"0\">" );
				message.Append( "<tr><td>Name</td><td><b>" + cd.Name + "</b></td></tr>" );
				message.Append( "<tr><td>Email</td><td><b>" + cd.Email + "</b></td></tr>" );
				message.Append( "<tr><td>Primary Phone</td><td><b>" + cd.PrimaryPhone + "</b></td></tr>" );
				message.Append( "<tr><td>Address</td><td><b>" + cd.Address + "</b></td></tr>" );
				message.Append( "<tr><td>City</td><td><b>" + cd.City + "</b></td></tr>" );
				message.Append( "</table></div>" );
				
				if ( customerComments != "" )
				{
					message.Append( "<br><p>Customer Comments : " + customerComments + "</p>" );
				}
								
				message.Append( "<br><br>Regards,<br>" );
				message.Append( "Customer Care,<br><b>bikewale</b>" );
								
				HttpContext.Current.Trace.Warn("Sent Mail: " + message.ToString() );
				
				CommonOpn op = new CommonOpn();
				op.SendMail( email, subject, message.ToString(), true );
			}
			catch(Exception err)
			{
				HttpContext.Current.Trace.Warn(err.Message);
				ErrorClass objErr = new ErrorClass(err,HttpContext.Current.Request.ServerVariables["URL"]);
				objErr.SendMail();
			} // catch Exception
		}
		
		public static void CustomerRegistration( string customerId )
		{
			try
			{
				StringBuilder message = new StringBuilder();
				string subject = "BikeWale Registration.";
				 
				CustomerDetails cd = new CustomerDetails( customerId );

                message.Append("<img align=\"right\" src=\"http://img.carwale.com/bikewaleimg/images/bw-logo.png\" />");
				
				message.Append( "<h4>Dear " + cd.Name + ",</h4>" );
				
				message.Append( "<p>Greetings from BikeWale!</p>" ); 
				
				message.Append( "<p>Thank you for visiting BikeWale.com " );
				message.Append( " We are committed to improve your bike trading experience through our services" );
                message.Append( " and are pleased to welcome you in the BikeWale family. </p>");
                message.Append( "<p>With BikeWale, you can now ride");
                message.Append( " your biking passion at full-throttle with reviews, news, comparison, upcoming bikes and more. </p>");
				message.Append( "<p>For future reference your user id and password are as listed below:</p>" );
				
				message.Append( "User ID : " + cd.Email + "<br>" );
				message.Append( "Password : " + cd.Password + "<br>" );

                message.Append("<p>You can change your password by clicking here <a href='http://www.bikewale.com/MyBikewale/changepassword/'>Change Password</a></p>");

                /*
                string cipher = bikewaleSecurity.EncodeVerificationCode( customerId.ToString() );
				
                message.Append( "<p>Please <a target=\"_blank\" href=\"http://www.bikewale.com/users/verifyEmail.aspx?verify=" );
                message.Append( cipher + "\">click here</a>" );
                message.Append( " to activate your account or copy and paste the following link in the browser’s address-bar.</p>" );
                message.Append( "<a target=\"_blank\" href=\"http://www.bikewale.com/users/verifyEmail.aspx?verify=" );
                message.Append( cipher + "\">http://www.bikewale.com/users/verifyEmail.aspx?verify=" + cipher + "</a>" );
                */

                message.Append( "<br>Warm Regards,<br><br>" );
				message.Append( "Customer Care,<br><b>BikeWale</b>" );
				
                //MailerAds ma = new MailerAds();
                //message.Append("<br><br>" + ma.GetAdScript(customerId));
				
				HttpContext.Current.Trace.Warn("Sent Mail: " + message.ToString() );
				
				CommonOpn op = new CommonOpn();
				op.SendMail( cd.Email, subject, message.ToString(), true );
			}
			catch(Exception err)
			{
				HttpContext.Current.Trace.Warn(err.Message);
				ErrorClass objErr = new ErrorClass(err,HttpContext.Current.Request.ServerVariables["URL"]);
				objErr.SendMail();
			} // catch Exception
		}

        /// <summary>
        ///     Written By : Ashish G. Kamble on 29/10/2012
        ///     Summary : Function will send registration mail to the customer with password
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="password">password should not be empty. Must pass user given password or randomly generated password</param>
        public static void CustomerRegistration(string customerId, string password)
        {
            try
            {
                StringBuilder message = new StringBuilder();
                string subject = "BikeWale Registration.";

                CustomerDetails cd = new CustomerDetails(customerId);

                message.Append("<img align=\"right\" src=\"http://img.carwale.com/bikewaleimg/images/bw-logo.png\" />");

                message.Append("<h4>Dear " + cd.Name + ",</h4>");

                message.Append("<p>Greetings from BikeWale!</p>");

                message.Append("<p>Thank you for visiting BikeWale.com ");
                message.Append(" We are committed to improve your bike trading experience through our services");
                message.Append(" and are pleased to welcome you in the BikeWale family. </p>");
                message.Append("<p>With BikeWale, you can now ride");
                message.Append(" your biking passion at full-throttle with reviews, news, comparison, upcoming bikes and more. </p>");
                message.Append("<p>For future reference your user id and password are as listed below:</p>");

                message.Append("User ID : " + cd.Email + "<br>");
                message.Append("Password : " + password + "<br>");

                message.Append("<p>You can change your password by clicking here <a href='http://www.bikewale.com/MyBikewale/changepassword/'>Change Password</a></p>");

                /*
                string cipher = bikewaleSecurity.EncodeVerificationCode( customerId.ToString() );
				
                message.Append( "<p>Please <a target=\"_blank\" href=\"http://www.bikewale.com/users/verifyEmail.aspx?verify=" );
                message.Append( cipher + "\">click here</a>" );
                message.Append( " to activate your account or copy and paste the following link in the browser’s address-bar.</p>" );
                message.Append( "<a target=\"_blank\" href=\"http://www.bikewale.com/users/verifyEmail.aspx?verify=" );
                message.Append( cipher + "\">http://www.bikewale.com/users/verifyEmail.aspx?verify=" + cipher + "</a>" );
                */

                message.Append("<br>Warm Regards,<br><br>");
                message.Append("Customer Care,<br><b>BikeWale</b>");

                //MailerAds ma = new MailerAds();
                //message.Append("<br><br>" + ma.GetAdScript(customerId));

                HttpContext.Current.Trace.Warn("Sent Mail: " + message.ToString());

                CommonOpn op = new CommonOpn();
                op.SendMail(cd.Email, subject, message.ToString(), true);
            }
            catch (Exception err)
            {
                HttpContext.Current.Trace.Warn(err.Message);
                ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            } // catch Exception
        }


        /// <summary>
        ///     Written By : Ashish G. kamble
        /// </summary>
        /// <param name="email">email id on which password is to be send.</param>
        /// <param name="customerName">Name of the customer</param>
        /// <param name="token">active password token.</param>
        public static void CustomerPasswordRecovery(string email, string customerName, string token)
        {
            try
            {
                StringBuilder message = new StringBuilder();
                string subject = "BikeWale Password Recovery.";

                message.Append("<img align=\"right\" src=\"http://img.carwale.com/bikewaleimg/images/bw-logo.png\" />");

                message.Append("<h4>Dear " + customerName + ",</h4>");

                message.Append("<p>Greetings from BikeWale!</p>");

                message.Append("<p>Thank you for choosing BikeWale.");
                message.Append(" We are committed to improve your bike trading experience through our services.</p>");

                message.Append("<p>This mail is an auto-responder for your request to recover your BikeWale password.");
                message.Append(" Your loginId and password are as mentioned below. </p>");

                message.Append("User ID : " + email + "<br>");
                //message.Append( "Password : " + password + "<br>" );
                message.Append("Please click the below link to reset your password.<br>");
                message.Append("http://www.bikewale.com/users/resetcustomerpassword.aspx?tkn=" + token );
                message.Append("<br>Above link is valid for 24 hours only.<br>");

                message.Append("<p>We request your presence on the portal and look forward to serve your diverse needs.</p>");

                message.Append("<p>For any assistance or suggestions kindly mail us at contact@bikewale.com</p>");

                message.Append("<br>Warm Regards,<br><br>");
                message.Append("Customer Care,<br><b>BikeWale</b>");

                //MailerAds ma = new MailerAds();
                //message.Append("<br><br>" + ma.GetAdScript(CustomerDetails.GetCustomerIdFromEmail(email)));

                HttpContext.Current.Trace.Warn("Sent Mail: " + message.ToString());

                CommonOpn op = new CommonOpn();
                op.SendMail(email, subject, message.ToString(), true);
            }
            catch (Exception err)
            {
                HttpContext.Current.Trace.Warn(err.Message);
                ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            } // catch Exception
        }
		
        //public static void CustomerPasswordRecovery( string email, string customerName, string password )
        //{
        //    try
        //    {
        //        StringBuilder message = new StringBuilder();
        //        string subject = "BikeWale Password Recovery.";

        //        message.Append("<img align=\"right\" src=\"http://img.carwale.com/bikewaleimg/images/bw-logo.png\" />");
				
        //        message.Append( "<h4>Dear " + customerName + ",</h4>" );
				
        //        message.Append( "<p>Greetings from BikeWale!</p>" ); 
				
        //        message.Append( "<p>Thank you for choosing BikeWale." );
        //        message.Append( " We are committed to improve your bike trading experience through our services.</p>" );
				
        //        message.Append( "<p>This mail is an auto-responder for your request to receover your BikeWale password." );
        //        message.Append( " Your loginId and password are as mentioned below. </p>" );
				
        //        message.Append( "User ID : " + email + "<br>" );
        //        message.Append( "Password : " + password + "<br>" );
				
        //        message.Append( "<p>We request your presence on the portal and look forward to serve your diverse needs.</p>" );
				
        //        message.Append( "<p>For any assistance or suggestions kindly mail us at contact@bikewale.com</p>" ); 
				
        //        message.Append( "<br>Warm Regards,<br><br>" );
        //        message.Append( "Customer Care,<br><b>BikeWale</b>" );
				
        //        //MailerAds ma = new MailerAds();
        //        //message.Append("<br><br>" + ma.GetAdScript(CustomerDetails.GetCustomerIdFromEmail(email)));
				
        //        HttpContext.Current.Trace.Warn("Sent Mail: " + message.ToString() );
				
        //        CommonOpn op = new CommonOpn();
        //        op.SendMail( email, subject, message.ToString(), true );
        //    }
        //    catch(Exception err)
        //    {
        //        HttpContext.Current.Trace.Warn(err.Message);
        //        ErrorClass objErr = new ErrorClass(err,HttpContext.Current.Request.ServerVariables["URL"]);
        //        objErr.SendMail();
        //    } // catch Exception
        //}
		
		public static void EmailVerification( string code )
		{
			try
			{
				StringBuilder message = new StringBuilder();
				string subject = "BikeWale Registration Verification.";
				 
				message.Append( "<h4>Dear " + CurrentUser.Name + ",</h4>" );
				
				message.Append( "<p>Greetings from Bikewale!</p>" ); 
				
				message.Append( "<p>Thank you for choosing Bikewale.</p>" );
				
				message.Append( "<p>This mail is an auto-responder for your request to re-send your account activation code.</p>" );
				
				message.Append( "<p>Please <a target=\"_blank\" href=\"http://www.bikewale.com/users/verifyEmail.aspx?verify=" );
				message.Append( code + "\">click here</a>" );
				message.Append( " to activate your account or copy and paste the following link in the browser’s address-bar.</p>" );
				message.Append( "<a target=\"_blank\" href=\"http://www.bikewale.com/users/verifyEmail.aspx?verify=" );
				message.Append( code + "\">http://www.bikewale.com/users/verifyEmail.aspx?verify=" + code + "</a>" );
												
				message.Append( "<p>Once again, thanks you for becoming a member of Bikewale</p>" );
				
				message.Append( "<br>Warm Regards,<br><br>" );
				message.Append( "Customer Bikee,<br><b>BikeWale</b>" );
				
				HttpContext.Current.Trace.Warn("Sent Mail: " + message.ToString() );
				
				CommonOpn op = new CommonOpn();
				op.SendMail( CurrentUser.Email, subject, message.ToString(), true );
			}
			catch(Exception err)
			{
				HttpContext.Current.Trace.Warn(err.Message);
				ErrorClass objErr = new ErrorClass(err,HttpContext.Current.Request.ServerVariables["URL"]);
				objErr.SendMail();
			} // catch Exception
		}
		
		public static void ContactSeller( string sellerEmail, string sellerName, 
										  string customerId, string customerComments, 
										  string profileNo, string bikeName, 
										  string kilometers, string bikeYear, string bikePrice  )
		{
			try
			{
				string email = sellerEmail;
				StringBuilder message = new StringBuilder();
				string subject = "Someone is interested in your bike!";
				 
				CustomerDetails cd = new CustomerDetails( customerId );
				message.Append( "<p>Dear " + sellerName + ",</p>" );
				message.Append( "<p>Congratulations!</p>" );
				
				message.Append( "<p>" + cd.Name + " has shown interest in your bike - " + bikeName + "(#" + profileNo + ") priced at Rs. " + bikePrice + " which is listed for sale at BikeWale</p>" );
								
				message.Append( "<p>" + cd.Name + "'s contact details are mentioned below<br>" );
				message.Append( "Name: " + cd.Name + "<br>" );
				message.Append( "Email: " + cd.Email + "<br>" );
				message.Append( "Contact Number: " + cd.Mobile + "<br>" );
				//message.Append( "Address: " + cd.Address + "<br>" );
				//message.Append( "City: " + cd.City + "</p>" );
				
				message.Append("<p>If you would like to make any changes in your listing, ");
				message.Append("<a href='http://www.bikewale.com/MyBikewale/'>click here to update.</a></p>");
				
				message.Append("<p>Please remove your bike if it has been sold already, so that ever growing number of ");
				message.Append("prospective buyers do not cause inconvenience to you. ");
				message.Append("<a href='http://www.bikewale.com/MyBikewale/'>Click here to remove your bike now.</a></p>");

                message.Append("<p>Alternatively you can also remove your bike by sending SMS 'SOLD' to 56767767.</p>");
				
				message.Append("<p>We are committed to deliver value by bringing genuine buyers for your bike.</p>");
				
				message.Append("<p>We gauge that with the sale of " + bikeName + ", you would be interested buying a new bike. Want to ");
				message.Append("know about a new bike’s price? BikeWale’s <a href='http://www.bikewale.com/pricequote/'>Instant Price Quote</a> is a helpful tool which will ");
				message.Append("enable you to evaluate your purchase options on the price front.</p>");
								
				message.Append( "<br><br>Warm Regards,<br>" );
				message.Append( "Team BikeWale" );
				
				MailerAds ma = new MailerAds();
				message.Append("<br><br>" + ma.GetAdScript(CustomerDetails.GetCustomerIdFromEmail(sellerEmail)));
				
				HttpContext.Current.Trace.Warn("Sent Mail: " + message.ToString() );
				
				CommonOpn op = new CommonOpn();
				op.SendMail( email, subject, message.ToString(), true, cd.Email ); 
			}
			catch(Exception err)
			{
				HttpContext.Current.Trace.Warn(err.Message);
				ErrorClass objErr = new ErrorClass(err,HttpContext.Current.Request.ServerVariables["URL"]);
				objErr.SendMail();
			} // catch Exception
		}
		
		public static void ReferToFriend( string senderName, string senderEmail, string recipientEmail, string link )
		{
			try
			{
				string email = recipientEmail;
				StringBuilder message = new StringBuilder();
				string subject = senderName + " has referred you BikeWale.";

                message.Append("<img align=\"right\" src=\"http://img.carwale.com/bikewaleimg/images/bw-logo.png\" />");
				message.Append( "<h4>Hello,</h4>" );
				message.Append( "<p>Greetings from Bikewale!</p>" );
				message.Append( "<p><b>" + senderName + "(" + senderEmail + ")" + "</b> " );
				message.Append( "has visited Bikewale and found this page very useful.</p>" );
				message.Append( "<p>Please <a target=\"_blank\" href=\"" + link + "\">click</a>" );
				message.Append( " the following link to visit the recommended page.</p>" );
				
				message.Append( "<p><a target=\"_blank\" href=\"" + link + "\">" + link + "</a>" );
							
				message.Append( "<br><br>Regards,<br>" );
				message.Append( "Customer Care,<br><b>bikewale</b>" );
				
				HttpContext.Current.Trace.Warn("Email: " + email );
				HttpContext.Current.Trace.Warn("Sent Mail: " + message.ToString() );
				
				
				CommonOpn op = new CommonOpn();
				op.SendMail( email, subject, message.ToString(), true );
			}
			catch(Exception err)
			{
				HttpContext.Current.Trace.Warn(err.Message);
				ErrorClass objErr = new ErrorClass(err,HttpContext.Current.Request.ServerVariables["URL"]);
				objErr.SendMail();
			} // catch Exception
		}
		
		public static void VisitorFeedback( string senderName, string senderEmail, string feedback, string IP )
		{
			try
			{
				string email = "banwarils@gmail.com";
				StringBuilder message = new StringBuilder();
                string subject = "Visitor has given feedback for Bikewale." + IP;
				 
				message.Append( "<h4>Dear Sir,</h4>" );
				message.Append( "<p>Some visitor has given feedback for bikewale.</p>" );
				message.Append( "<p>Sender Name(Email) IP : <b>" + senderName + "(" + senderEmail + ")" + IP + "</b> " );
				message.Append( "<p><b>Feedback :</b>" + feedback + " </p>" );			
				message.Append( "<br><br>Regards,<br>" );
				message.Append( "Customer Care,<br><b>bikewale</b>" );
				
				HttpContext.Current.Trace.Warn("Email: " + email );
				HttpContext.Current.Trace.Warn("Sent Mail: " + message.ToString() );
				
				CommonOpn op = new CommonOpn();
				op.SendMail( email, subject, message.ToString(), true );
			}
			catch(Exception err)
			{
				HttpContext.Current.Trace.Warn(err.Message);
				ErrorClass objErr = new ErrorClass(err,HttpContext.Current.Request.ServerVariables["URL"]);
				objErr.SendMail();
			} // catch Exception
		}		
		
		/// <summary>
		/// This Function will be used to send mail to the thread subscribed
		/// </summary>
		public static void NotifyForumSubscribedUsers( string receiverEmail, string receiverName, string replierName, string topic, string threadUrl )
		{
			try
			{
				StringBuilder message = new StringBuilder();
				string subject = "Reply to discussion '" + topic + "'";
				
				message.Append("<p>Dear " + receiverName + ",</p>");
							
				message.Append("<p>Your subscribed discussion <b>" );
				message.Append( topic + "</b> has just been replied by <b>" + replierName + "</b>." );
				
				message.Append("<p>Discussion Link:<br>" );
				message.Append("<a href='" + threadUrl + "'>" + threadUrl + "</a></p>");
				
				message.Append("<p>We thank you for your active participation in BikeWale Forums.</p>" );
				message.Append("<p>Warm Regards,<br>BikeWale Team</p>");
				
				message.Append("<p style='font-size:11px'>This mail was sent to you because " );
				message.Append("you have subscribed to this discussion as 'Instant Email'. " );
				message.Append("If you no longer wish to receive such emails, please visit " );
				message.Append("<a href='http://www.bikewale.com/mybikewale/forums/subscriptions.aspx'>My Subscriptions</a> page to manage your existing subscriptions.<br><br>" );
				message.Append("This is an automated mail. Please do not reply to this mail. For all queries contact "
								+ "<a href='mailto:contact@bikewale.com'>contact@bikewale.com</a></p>");
				
				MailerAds ma = new MailerAds();
				message.Append("<br><br>" + ma.GetAdScript(CustomerDetails.GetCustomerIdFromEmail(receiverEmail)));
				
				HttpContext.Current.Trace.Warn( message.ToString() );
								
				CommonOpn op = new CommonOpn();
				//Trace.Warn( "Email : " + email );
				op.SendMail( receiverEmail, subject, message.ToString(), true );
			}
			catch(Exception err)
			{
				HttpContext.Current.Trace.Warn(err.Message);
				ErrorClass objErr = new ErrorClass(err,HttpContext.Current.Request.ServerVariables["URL"]);
				objErr.SendMail();
			} // catch Exception
		}
		
		
		public static void InquiryArchivedByCustomer( string inquiryType, string customerId, string inquiryId, string status, string feedback)
		{
			try
			{
				string email = "banwarils@gmail.com, bikewale@gmail.com";
				StringBuilder message = new StringBuilder();
				string subject = "Customer has archived his " + inquiryType + " inquiry.";
				 
				message.Append( "<h4>Dear Sir,</h4>" );
				message.Append( "<p>Some customer has archived his " + inquiryType + " inquiry.</p>" );
				message.Append( "<p>Customer Id : <b>" + customerId + "</b></p> " );
				message.Append( "<p>Inquiry Id : <b>" + inquiryId + "</b></p> " );
				message.Append( "<p>Status : <b>" + status + "</b></p> " );
				message.Append( "<p><b>Comments : </b>" + feedback + " </p>" );			
				message.Append( "<br><br>Regards,<br>" );
				message.Append( "Customer Care,<br><b>BikeWale</b>" );
				
				HttpContext.Current.Trace.Warn("Email: " + email );
				HttpContext.Current.Trace.Warn("Sent Mail: " + message.ToString() );
				
				CommonOpn op = new CommonOpn();
				op.SendMail( email, subject, message.ToString(), true );
			}
			catch(Exception err)
			{
				HttpContext.Current.Trace.Warn(err.Message);
				ErrorClass objErr = new ErrorClass(err,HttpContext.Current.Request.ServerVariables["URL"]);
				objErr.SendMail();
			} // catch Exception
		}
		
		/// <summary>
		/// This Function will be used send the purchase request to the dealer
		/// </summary>
		public static void ForwardUsedBikePurchaseInquiry( string email, string name, 
														  string inquiryId, string msg, 
														  string customerId, string comments,
														  string bikeName, string kilometers,
														  string bikeYear, string bikePrice  )
		{
			try
			{
				StringBuilder message = new StringBuilder();
				string subject = "";
				
				if ( inquiryId != "" )
				{
					subject = "Purchase Request Arrived : Bike #" + inquiryId;
				}
				else
				{
					subject = "Used Bike Purchase Inquiry Arrived.";
				}

                message.Append("<img align=\"right\" src=\"http://img.carwale.com/bikewaleimg/images/bw-logo.png\" />");
				message.Append( "<b>Dear " + name + ",</b>" );
				
				message.Append( "<p>Greetings from Bikewale!</p>" );
				
				message.Append( "<p>Please find here following inquiry for Used Bike Purchase. ");
				message.Append( "We are providing you with the details of Customer and " );
				message.Append( "his/her bike preference.</p>");
								
				CustomerDetails cd = new CustomerDetails( customerId );
				
				message.Append( "<div style='background-color:#FFF0E1;padding:5px;'>" );
				
				if ( inquiryId != "" )
				{
					message.Append( "<b>Purchase Inquiry Details</b>" );
					
					message.Append( "<p><a href=\"http://www.bikewale.com/Used/BikeDetails.aspx?bike=" );
					message.Append( inquiryId + "\">Click Here</a> Or copy and paste the following link" );
					message.Append( " to know details of your stock bike.</p>" );
					message.Append( "<p><a href=\"http://www.bikewale.com/Used/BikeDetails.aspx?bike=" );
					message.Append( inquiryId + "\">http://www.bikewale.com/Used/BikeDetails.aspx?bike=" );
					message.Append( inquiryId + "</a></p>" );
					
					message.Append( "<p>Make-Model-Version: " + bikeName + "<br>" );
					message.Append( "Year: " + bikeYear + "<br>" );
					message.Append( "Kilometers: " + kilometers + "<br>" );
					message.Append( "Price: " + bikePrice + "</p>" );
				}
				else
				{
					message.Append( "<p><b>Purchase Inquiry Details</b></p>" );
					
					message.Append( msg );
				}		
				
				message.Append( "</div>" );
						
				message.Append( "<div style=\"background-color:#B5EDBC;\"><h5>Customer Details:</h5>" );
				message.Append( "<table border=\"0\">" );
				message.Append( "<tr><td>Name</td><td><b>" + cd.Name + "</b></td></tr>" );
				message.Append( "<tr><td>Email</td><td><b>" + cd.Email + "</b></td></tr>" );
				message.Append( "<tr><td>Phone(s)</td><td><b>" );		
				message.Append( cd.Phone1 + "," + cd.Phone2 + "," + cd.Mobile );
				message.Append( "</b></td></tr>" );
				message.Append( "<tr><td>Address</td><td><b>" + cd.Address + "</b></td></tr>" );
				message.Append( "<tr><td>City</td><td><b>" + cd.City + "</b></td></tr>" );
				message.Append( "<tr><td>Comments</td><td> " + comments + "</td></tr>" );
				message.Append( "</table></div>" );
								
				message.Append( "<p>We will soon get in touch with customer to have a feedback" );
				message.Append( " on his/her interaction with your organization. We also request" );
				message.Append( " you to update us with status of this inquiry.</p>" );
				
				message.Append( "<br>With Warm Regards,<br><br>" );
				message.Append( "<b>Team Bikewale</b>" );
				
				//HttpContext.Current.Trace.Warn( "Message : " + message.ToString() );
				
				CommonOpn op = new CommonOpn();
				HttpContext.Current.Trace.Warn( "ForwardUsedBikePurchaseInquiry Email : " + email );
				op.SendMail( email, subject, message.ToString(), true );
			}
			catch(Exception err)
			{
				HttpContext.Current.Trace.Warn(err.Message);
				ErrorClass objErr = new ErrorClass(err,HttpContext.Current.Request.ServerVariables["URL"]);
				objErr.SendMail();
			} // catch Exception
		}
		
		
		/// <summary>
		/// This Function will be used send the purchase request to the dealer
		/// </summary>
		public static void ForwardUsedBikePurchaseInquiryNoDetails( string email, string name, 
														  			string inquiryId )
		{
			try
			{
				StringBuilder message = new StringBuilder();
				string subject = "";
				
				if ( inquiryId != "" )
				{
					subject = "Purchase Request Arrived : Bike #" + inquiryId;
				}
				else
				{
					subject = "Used Bike Purchase Inquiry Arrived.";
				}

                message.Append("<img align=\"right\" src=\"http://img.carwale.com/bikewaleimg/images/bw-logo.png\" />");
				message.Append( "<b>Dear " + name + ",</b>" );
				
				message.Append( "<p>Greetings from Bikewale!</p>" );
				
				message.Append( "<p>A new purchase inquiry has arrived for the bike profile number "
								+ "#" + inquiryId + ".<br><br>"
								+ "Contact Pooja at 022-32605878 to know about the inquiry details.</p>");
								
				message.Append( "<br>With Warm Regards,<br><br>" );
				message.Append( "<b>Team Bikewale</b>" );
				
				//HttpContext.Current.Trace.Warn( "Message : " + message.ToString() );
				
				CommonOpn op = new CommonOpn();
				HttpContext.Current.Trace.Warn( "ForwardUsedBikePurchaseInquiry Email : " + email );
				op.SendMail( email, subject, message.ToString(), true );
			}
			catch(Exception err)
			{
				HttpContext.Current.Trace.Warn(err.Message);
				ErrorClass objErr = new ErrorClass(err,HttpContext.Current.Request.ServerVariables["URL"]);
				objErr.SendMail();
			} // catch Exception
		}
		
		
		public static void ContactUs( string senderName, string contactNo, string senderEmail, string mailingAddress, string feedback, string IP )
		{
			try
			{
				string email = mailingAddress ;
				StringBuilder message = new StringBuilder();
				string subject = "Contact Us : BikeWale" ;
				 
				message.Append( "<p>A new visitor wish to contact BikeWale. Customer details are as follows.</p>" );
				message.Append( "<p>Customer Name : <b>" + senderName + "</b> " );
				message.Append( "<p>Phone : <b>" + contactNo + "</b> " );
				message.Append( "<p>E-Mail : <b>" + senderEmail + "</b> " );
				message.Append( "<p>Sender IP : <b>" + IP + "</b> " );
				message.Append( "<p>Feedback :<b>" + feedback + " </b></p>" );		
					
				message.Append( "<br><br>With Warm Regards,<br>" );
				message.Append( "Customer Care,<br><b>bikewale</b>" );
				
				HttpContext.Current.Trace.Warn("Email: " + email );
				HttpContext.Current.Trace.Warn("Sent Mail: " + message.ToString() );
				
				CommonOpn op = new CommonOpn();
				op.SendMail( email, subject, message.ToString(), true );
			}
			catch(Exception err)
			{
				HttpContext.Current.Trace.Warn(err.Message);
				ErrorClass objErr = new ErrorClass(err,HttpContext.Current.Request.ServerVariables["URL"]);
				objErr.SendMail();
			} // catch Exception
		}


		public static void GetCallBack( string senderName, string contactNo, string senderEmail, string address, string city, string feedback, string bikeId )
		{
			try
			{
				string email = "fasih@bikewale.com";
				StringBuilder message = new StringBuilder();
				string subject = "Call back request:" + bikeId ;
				 
				message.Append( "<p>Call back is requested for new bike. Customer details are as follows.</p>" );
				message.Append( "<p>Customer Name : <b>" + senderName + "</b> " );
				message.Append( "<p>Intrested In : <b>" + bikeId + "</b> " );
				message.Append( "<p>Phone : <b>" + contactNo + "</b> " );
				message.Append( "<p>E-Mail : <b>" + senderEmail + "</b> " );
				message.Append( "<p>Address : <b>" + address + "</b> " );
				message.Append( "<p>City : <b>" + city + "</b></p>" );
				
				message.Append( "<p>Feedback :<b>" + feedback + " </b></p>" );		
					
				message.Append( "<br><br>Regards,<br>" );
				message.Append( "Customer Care,<br><b>bikewale</b>" );
				
				HttpContext.Current.Trace.Warn("Email: " + email );
				HttpContext.Current.Trace.Warn("Sent Mail: " + message.ToString() );
				
				CommonOpn op = new CommonOpn();
				op.SendMail( email, subject, message.ToString(), true );
			}
			catch(Exception err)
			{
				HttpContext.Current.Trace.Warn(err.Message);
				ErrorClass objErr = new ErrorClass(err,HttpContext.Current.Request.ServerVariables["URL"]);
				objErr.SendMail();
			} // catch Exception
		}

		/// <summary>
		/// This Function will be used when a Used Bike Purchase inquiry is forwarded
		/// to some finance agency or anyone else. Just u need to mention name and
		/// email of the recipient.
		/// </summary>
		public static void SendSellerDetailsToBuyer(string sellerEmail, string sellerName, 
													string sellerContactNo, string sellerAddress, 			
													string profileNo, string buyerId,
													string bikeName, string kilometers,
													string bikeYear, string bikePrice )
		{
			try
			{
				StringBuilder message = new StringBuilder();
				string subject = "";
				subject = "Seller Details of Bike #" + profileNo;
				
				CustomerDetails cd = new CustomerDetails( buyerId );

                //message.Append( "<img align=\"right\" src=\"http://www.bikewale.com/images/bw-logo.png\" />" );
				message.Append( "Dear " + cd.Name + "," );
				
				message.Append( "<p>You short listed a bike to buy. Congratulations!</p>" );
				
				message.Append( "<p>The bike you have shown interest in is " + bikeName + " (#" + profileNo + "), done " + kilometers + " KM listed for Rs. " + bikePrice + "/-. </p> ");
							
				message.Append( "<p><a href=\"http://www.bikewale.com/Used/BikeDetails.aspx?bike=" );
				message.Append( profileNo + "\">Please click here to view complete details of the bike.</a></p>");
										
				message.Append( "<p>You may contact the seller directly, details as below:<br>" );
				message.Append( "Name: " + sellerName + "<br>" );
				//message.Append( "Email: " + sellerEmail + "<br>" );
				message.Append( "Phone(s): " + sellerContactNo + "<br>" );
				message.Append( "Address: " + sellerAddress + "</p>" );
				
				//if bike belongs to Mumbai & NCR
				//if(CommonOpn.CheckForMumbai(profileNo))
				//{
					//message.Append( "<p>To get finance on this bike call Kshitij at 9821651116</p>");
				//}
				
                //message.Append( "<p>Would you like to know how much you should be paying for this bike? ");
                //message.Append( "bikewale’s <a href='http://bikewale.com/Mybikewale/BikeValuation/ValuationRequest.aspx'>");
                //message.Append( "Used Bike Valuation</a> could help you with it. Simply feed in the details of the bike and you will get close-to-accurate prices for the same.</p>" );
				
				message.Append( "<p>Feel free to contact for any other assistance.</p>" );
				
				message.Append( "<br>Warm Regards,<br><br>" );
				message.Append( "Team bikewale" );
				
				//HttpContext.Current.Trace.Warn( "Message : " + message.ToString() );
				
                //MailerAds ma = new MailerAds();
                //message.Append("<br><br>" + ma.GetAdScript(buyerId));
				
				CommonOpn op = new CommonOpn();
				//HttpContext.Current.Trace.Warn( "SendSellerDetailsToBuyer Email : " + email );
				op.SendMail( cd.Email, subject, message.ToString(), true );
			}
			catch(Exception err)
			{
				HttpContext.Current.Trace.Warn(err.Message);
				ErrorClass objErr = new ErrorClass(err,HttpContext.Current.Request.ServerVariables["URL"]);
				objErr.SendMail();
			} // catch Exception
		}
		
		
		/// <summary>
		/// Send news letter to the customers. the body of the news letter is passed
		///	and the subject is passed
		/// </summary>
		public static void SendNewsLetter(string body, string subject, string startId, string endId)
		{
			CommonOpn op = new CommonOpn();
			Database db = new Database();
			string sql = "";
			
			
			try
			{
				sql = " Select cu.id, cu.Name, cu.email, ck.CustomerKey From "
                    + " Customers as cu, CustomerSecurityKey as ck With(NoLock) Where "
					+ " ReceiveNewsletters = 1 and ck.CustomerId = cu.id and cu.isfake = 0 ";
				
				if(startId != "")
					sql += " AND CU.ID >= @startId";
					
				if(endId != "")
					sql += " AND CU.ID <= @endId";
					
				sql += " ORDER BY ID ";
				
				SqlParameter [] param ={new SqlParameter("@startId", startId), new SqlParameter("@endId", endId)};
				DataSet ds = db.SelectAdaptQry(sql, param);
				
				foreach(DataRow row in ds.Tables[0].Rows)
				{
					StringBuilder message = new StringBuilder();
					
					string custId	= row["Id"].ToString();
					string custName = row["Name"].ToString();
					string custMail = row["email"].ToString();
					string custSKey = row["CustomerKey"].ToString();
					
					message.Append("Dear " + custName + ",");
					
					message.Append(body);
					
					//add the disclaimer message
					message.Append("<span style=\"font-size: 9px;\">This email was sent to"
									+ " <a href=\"mailto:" + custMail + "\">" 
									+ custMail + "</a>. To stop receiving this newsletter click"
									+ " <a href=\"http://www.bikewale.com/Newsletter/Unsubscribe.aspx\">here</a>.</span>");
					
					op.SendNewsletterMail(custMail, subject, message.ToString(), true);
										
					HttpContext.Current.Trace.Warn("Newsletter Sent to : " + custId + " : " + custName + " : " + custMail);
				}
			}
			catch(Exception err)
			{
				HttpContext.Current.Trace.Warn(err.Message);
				ErrorClass objErr = new ErrorClass(err,HttpContext.Current.Request.ServerVariables["URL"]);
				objErr.SendMail();
			} // catch Exception
		}
		
		/// <summary>
		/// This Function will be used send the purchase request to the dealer
		/// </summary>
		public static void ForwardNewBikeShowroomInquiry( string email, string name )
		{
			try
			{
				StringBuilder message = new StringBuilder();
				string subject = "";

				subject = "Used Bike Purchase Inquiry Arrived.";

                message.Append("<img align=\"right\" src=\"http://img.carwale.com/bikewaleimg/images/bw-logo.png\" />");
				message.Append( "<b>Dear " + name + ",</b>" );
				
				message.Append( "<p>Greetings from bikewale!</p>" );
				
				message.Append( "<p>A new purchase inquiry has arrived. <br><br>"
								+ "Contact us at 022-32605878 to know about the inquiry details.</p>");
								
				message.Append( "<br>With Warm Regards,<br><br>" );
				message.Append( "<b>Team bikewale</b>" );
				
				//HttpContext.Current.Trace.Warn( "Message : " + message.ToString() );
				
				CommonOpn op = new CommonOpn();
				HttpContext.Current.Trace.Warn( "ForwardUsedBikePurchaseInquiry Email : " + email );
				op.SendMail( email, subject, message.ToString(), true );
			}
			catch(Exception err)
			{
				HttpContext.Current.Trace.Warn(err.Message);
				ErrorClass objErr = new ErrorClass(err,HttpContext.Current.Request.ServerVariables["URL"]);
				objErr.SendMail();
			} // catch Exception
		}
		
		
		/*send mail to the seller than some buyer wants to purchase its bike.
			ask him to come to bikewale.com to view the inquiry
		*/
		public static void ContactSellerWithNoBuyerDetails( string sellerEmail, string sellerName, 
										  string url, string profileNo, string bikeName,
										  string kilometers, string bikeYear, string bikePrice  )
		{
			try
			{
				string email = sellerEmail;
				StringBuilder message = new StringBuilder();
				string subject = "Someone is interested in your bike!";
				
				message.Append( "<p>Dear " + sellerName + ",</p>" );
				message.Append( "<p>Greetings from bikewale!</p>" );
				message.Append( "<p>Some buyer has viewed your bike (#" + profileNo + ") at bikewale and sent you a message.</p>" );
				
				message.Append( "<p>To view the buyer's details <a href='http://www.bikewale.com/Mybikewale/'>"
								+ "click here</a>.<br> Or copy and paste the following link in your brwoser: <br>"
								+ "http://www.bikewale.com/Mybikewale/ </p>" );
				
				message.Append( "<p>Bike details as follows: <br><br>Make-Model-Version: " + bikeName + "<br>" );
				message.Append( "Year: " + bikeYear + "<br>" );
				message.Append( "Kilometers: " + kilometers + "<br>" );
				message.Append( "Price: " + bikePrice + "</p>" );
				
				message.Append( "<p><a href=\"http://www.bikewale.com/Used/BikeDetails.aspx?bike=" );
				message.Append( profileNo + "\">Click Here</a> here to view complete details of the above bike<br>" );
				message.Append( "Or copy and paste the link " );
				message.Append( "<a href=\"http://www.bikewale.com/Used/BikeDetails.aspx?bike=" );
				message.Append( profileNo + "\">http://www.bikewale.com/Used/BikeDetails.aspx?bike=" );
				message.Append( profileNo + "</a></p>" );
				
				
								
				message.Append( "<br><br>Warm Regards,<br>" );
				message.Append( "Customer Care,<br>bikewale" );
				
				MailerAds ma = new MailerAds();
				message.Append("<br><br>" + ma.GetAdScript(CustomerDetails.GetCustomerIdFromEmail(sellerEmail)));
				
				HttpContext.Current.Trace.Warn("Sent Mail: " + message.ToString() );
				
				CommonOpn op = new CommonOpn();
				op.SendMail( email, subject, message.ToString(), true); 
			}
			catch(Exception err)
			{
				HttpContext.Current.Trace.Warn(err.Message);
				ErrorClass objErr = new ErrorClass(err,HttpContext.Current.Request.ServerVariables["URL"]);
				objErr.SendMail();
			} // catch Exception
			
			
		}
		
		/// <summary>
		/// This Function will be used to send mail to the questioner in bikewale Answers
		/// </summary>
		public static void CANewAnswer( string questionerEmail, string questionerName, string question, string questionUrl )
		{
			try
			{
				StringBuilder message = new StringBuilder();
				string subject = "New Answer to your question at bikewale Answers";
				
				message.Append("<table width=\"100%\" border=\"0\" "
								+ "style=\"border:1px solid #FDA8D7;background-color:#FFE8EB;font-size:12px;font-family:Verdana, Arial, Helvetica, sans-serif;\">"
								+ "<tr><td><b>Dear " + questionerName + ",</b></td></tr>");
				
				
				message.Append( "<tr><td><p>Greetings from <a href='http://www.bikewale.com'>bikewale!</a></p>" );
				
				message.Append("<p>An answer is posted for your question <a href='" + questionUrl + "'>" + question + "</a>.</p>");
				
				message.Append("<p><a href='" + questionUrl + "'>Click here</a> to see the answer to your question.</p>");
				
				message.Append("<p>We thank you for your active participation at bikewale Answers.<br><br>"
								+ "With Warm Regards,<br><br><b>Team bikewale</b></p>");
				
				message.Append("<p style='font-size:11px'>Disclaimer:<br>"
								+ "This is an automated mail. Please do not reply to this mail. For all queries contact<br>"
								+ "<a href='mailto:contact@bikewale.com'>contact@bikewale.com</a></p>");
				
				message.Append("</td></tr></table>");
								
				CommonOpn op = new CommonOpn();
				//Trace.Warn( "Email : " + email );
				op.SendMail( questionerEmail, subject, message.ToString(), true );
			}
			catch(Exception err)
			{
				HttpContext.Current.Trace.Warn(err.Message);
				ErrorClass objErr = new ErrorClass(err,HttpContext.Current.Request.ServerVariables["URL"]);
				objErr.SendMail();
			} // catch Exception
		}
		
		/// <summary>
		/// This Function will be used to send mail to the questioner in bikewale Answers
		/// </summary>
		public static void CANewQuestion( string expertEmail, string expertName, string question, string questionUrl )
		{
			try
			{
				StringBuilder message = new StringBuilder();
				string subject = "New Question is Asked at bikewale Answers";
				
				message.Append("<table width=\"100%\" border=\"0\" "
								+ "style=\"border:1px solid #FDA8D7;background-color:#FFE8EB;font-size:12px;font-family:Verdana, Arial, Helvetica, sans-serif;\">"
								+ "<tr><td><b>Dear " + expertName + ",</b></td></tr>");
				
				message.Append( "<tr><td><p>Greetings from <a href='http://www.bikewale.com'>bikewale!</a></p>" );
				
				message.Append("<p>A new question, <a href='" + questionUrl + "'>" + question + "</a> is asked at bikewale Answers.</p>");
				
				message.Append("<p><a href='" + questionUrl + "'>Click here</a> to view and answer the question.</p>");
				
				message.Append("<p>This email is sent to you because you have registered as a 'bikewale Answers Expert' with us. "
								+ "We thank you for your active participation at bikewale Answers.<br><br>"
								+ "With Warm Regards,<br><br><b>Team bikewale</b></p>");
				
				message.Append("<p style='font-size:11px'>Disclaimer:<br>"
								+ "Change your preferences for receiving mails from bikewale Answers "
								+ " <a href='http://www.bikewale.com/mybikewale/answers/preferences.aspx'>here</a>. </p>");
				
				message.Append("</td></tr></table>");
								
				CommonOpn op = new CommonOpn();
				//Trace.Warn( "Email : " + email );
				op.SendMail( expertEmail, subject, message.ToString(), true );
			}
			catch(Exception err)
			{
				HttpContext.Current.Trace.Warn(err.Message);
				ErrorClass objErr = new ErrorClass(err,HttpContext.Current.Request.ServerVariables["URL"]);
				objErr.SendMail();
			} // catch Exception
		}
		
		/// <summary>
		/// This Function will be used to send mail to the reviewer in bikewale User Reviews
		/// </summary>
		public static void CustomerReviewComment( string reviewerEmail, string reviewerName, string title, string reviewUrl )
		{
			try
			{
				StringBuilder message = new StringBuilder();
				string subject = "A comment is posted on your review - bikewale";
				
				message.Append("<table width=\"100%\" border=\"0\" "
								+ "style=\"border:1px solid #FDA8D7;background-color:#FFE8EB;font-size:12px;font-family:Verdana, Arial, Helvetica, sans-serif;\">"
								+ "<tr><td><b>Dear " + reviewerName + ",</b></td></tr>");
				
				
				message.Append( "<tr><td><p>Greetings from <a href='http://www.bikewale.com'>bikewale!</a></p>" );
				
				message.Append("<p>A comment is posted on your review <a href='" + reviewUrl + "'>" + title + "</a>.</p>");
				
				message.Append("<p><a href='" + reviewUrl + "'>Click here</a> to read the comment on your review.</p>");
				
				message.Append("<p>We thank you for writing review for bikewale.<br><br>"
								+ "With Warm Regards,<br><br><b>Team bikewale</b></p>");
				
				message.Append("<p style='font-size:11px'>Disclaimer:<br>"
								+ "This is an automated mail. Please do not reply to this mail. For all queries contact<br>"
								+ "<a href='mailto:contact@bikewale.com'>contact@bikewale.com</a></p>");
				
				message.Append("</td></tr></table>");
				
				
				MailerAds ma = new MailerAds();
				message.Append("<br><br>" + ma.GetAdScript(CustomerDetails.GetCustomerIdFromEmail(reviewerEmail)));
								
				CommonOpn op = new CommonOpn();
				//Trace.Warn( "Email : " + email );
				op.SendMail( reviewerEmail, subject, message.ToString(), true );
			}
			catch(Exception err)
			{
				HttpContext.Current.Trace.Warn(err.Message);
				ErrorClass objErr = new ErrorClass(err,HttpContext.Current.Request.ServerVariables["URL"]);
				objErr.SendMail();
			} // catch Exception
			
		}	
			/// <summary>
		/// This Function will be used to send mail to the reviewer in bikewale User Reviews
		/// </summary>
		public static void PerformanceReportPerformance(string inqId )
		{
			try
			{
				StringBuilder message = new StringBuilder();
				string subject = "Customer comes after reading Performance Report";
				
				message.Append("<p>For bike profile number #" + inqId + "</p>");
								
				CommonOpn op = new CommonOpn();
				//Trace.Warn( "Email : " + email );
				op.SendMail( "rajeevmantu@gmail.com", subject, message.ToString(), true );
			}
			catch(Exception err)
			{
				HttpContext.Current.Trace.Warn(err.Message);
				ErrorClass objErr = new ErrorClass(err,HttpContext.Current.Request.ServerVariables["URL"]);
				objErr.SendMail();
			} // catch Exception
		}
		
		public static void MailToPaidCustomer(string customerName, string mobile, string eMail, string profileId, string versionId)
		{
			try
			{
				HttpContext.Current.Trace.Warn("Sending Mail XML");
				StringBuilder message = new StringBuilder();
				string subject = "Your bike display is live on bikewale";
				
				message.Append("<table width=600 border=0 cellspacing=0 cellpadding=0>");
  				message.Append("<tr><td align=center><table width=557 border=0 align=center cellpadding=0 cellspacing=0>");
				message.Append("<tr><td height=16></td></tr><tr><td><a href=http://www.bikewale.com/?ltsrc=17128 target=_blank>");
				message.Append("<img src=http://img.bikewale.com/cw-common/logo.jpg border=0></a></td>");
				message.Append("</tr><tr><td height=18></td></tr><tr><td style='font-family:Tahoma; font-size:12px; color:#070707;'>");
				
				message.Append("Dear " + customerName + ",");
				message.Append("<br><br>Thank you for choosing <a href=http://www.bikewale.com/?ltsrc=17128 target=_blank style='color:#034fb6;'>www.bikewale.com</a>");
				message.Append(" to sell your bike.");
				message.Append("</td></tr><tr><td height=7></td></tr><tr><td style='font-family:Tahoma; font-size:12px; color:#070707;'></td></tr><tr><td>");
				message.Append("<table width=100% border=0 cellspacing=0 cellpadding=0 style='border:1px solid #f2f2f2;'>");
				message.Append("<tr><td height=10></td><td height=10></td></tr><tr><td width=46% align=center valign=top>");
				message.Append("<table width=90% border=0 align=center cellpadding=0 cellspacing=0>");
				message.Append("<tr><td height=65>&nbsp;</td></tr><tr><td>");
				message.Append("<img src=http://img.bikewale.com/bikes/" + versionId + "b.jpg border=0></td></tr>");
				message.Append("<tr><td>&nbsp;</td></tr><tr><td align=center>");
				message.Append("<img src=http://img.bikewale.com/Mailer/PaidList/mark_img.jpg width=58 height=48 border=0></td>");
				message.Append("</tr><tr><td align=center style='font-family:Arial, Helvetica, sans-serif; font-size:20px; font-weight:bold; color:#16a607;'>");
				message.Append("Your bike advertisement <br>has been published.");
				message.Append("</td></tr><tr><td height=22>&nbsp;</td></tr><tr><td align=center>");
				message.Append("<a href='http://www.bikewale.com/used/BikeDetails.aspx?bike=" + profileId + "&ltsrc=17128'>");
				message.Append("<img src=http://img.bikewale.com/Mailer/PaidList/view_btn.jpg width=151 height=25 border=0></a></td></tr>");
				message.Append("<tr><td height=56>&nbsp;</td></tr><tr>");
				message.Append("<td style='font-family:Arial, Helvetica, sans-serif; font-size:11px; color:#9d9d9d; text-align:justify;'>");
				message.Append("<em><strong>Tip:</strong> Your username and password has been sent to you separately via email. After you login,");
				message.Append("you can make changes to the advertisement, upload photos and even remove your ad (once your bike is sold).</em>");
				message.Append("</td></tr><tr><td>&nbsp;</td></tr></table></td>");
				message.Append("<td width=54% align=center valign=top background=http://img.bikewale.com/Mailer/PaidList/right_bg.gif style='background-image:url(http://img.bikewale.com/Mailer/PaidList/right_bg.gif); background-repeat:no-repeat;'>");
				message.Append("<table width=90% border=0 align=center cellpadding=0 cellspacing=0>");
				message.Append("<tr><td height=32 colspan=3 valign=bottom style='font-family:Arial, Helvetica, sans-serif; font-size:18px; font-weight:bold; color:#034fb6;'>CAR SELLING TIPS</td>");
				message.Append("</tr><tr>");
				message.Append("<td width=27% height=98 align=center valign=middle style='font-family:Arial, Helvetica, sans-serif; font-size:72px; color:#adadad; font-weight:bold;'>1</td>");
				message.Append("<td width=65% height=98 align=left valign=middle style='font-family:Arial, Helvetica, sans-serif; font-size:11px; color:#000000;'>Upload good quality photos of your bike's interior and exterior - this increases the number of people interested in your bike.</td>");
				message.Append("<td width=8% height=98 align=left valign=middle style='font-family:Arial, Helvetica, sans-serif; font-size:11px; color:#000000;'>&nbsp;</td>");
				message.Append("</tr><tr>");
				message.Append("<td height=60 align=center valign=middle style='font-family:Arial, Helvetica, sans-serif; font-size:72px; color:#adadad; font-weight:bold;'>2</td>");
				message.Append("<td height=60 align=left valign=middle style='font-family:Arial, Helvetica, sans-serif; font-size:11px; color:#000000;'>Please take the time to respond to all enquiries promptly - you never know who could be the next owner of your bike.</td>");
				message.Append("<td height=60 align=left valign=middle style='font-family:Arial, Helvetica, sans-serif; font-size:11px; color:#000000;'>&nbsp;</td>");
				message.Append("</tr><tr>");
				message.Append("<td height=90 align=center valign=middle style='font-family:Arial, Helvetica, sans-serif; font-size:72px; color:#adadad; font-weight:bold;'>3</td>");
				message.Append("<td height=90 align=left valign=middle style='font-family:Arial, Helvetica, sans-serif; font-size:11px; color:#000000;'>Invite prospective buyers to inspect and test drive the bike personally.</td>");
				message.Append("<td height=90 align=left valign=middle style='font-family:Arial, Helvetica, sans-serif; font-size:11px; color:#000000;'>&nbsp;</td>");
				message.Append("</tr><tr>");
				message.Append("<td height=90 align=center valign=middle style='font-family:Arial, Helvetica, sans-serif; font-size:72px; color:#adadad; font-weight:bold;'>4</td>");
				message.Append("<td height=90 align=left valign=middle style='font-family:Arial, Helvetica, sans-serif; font-size:11px; color:#000000;'>Ensure the bike is clean from both outside and inside before you show it to a prospective buyer.</td>");
				message.Append("<td height=90 align=left valign=middle style='font-family:Arial, Helvetica, sans-serif; font-size:11px; color:#000000;'>&nbsp;</td>");
				message.Append("</tr><tr>");
				message.Append("<td height=90 align=center valign=middle style='font-family:Arial, Helvetica, sans-serif; font-size:72px; color:#adadad; font-weight:bold;'>5</td>");
				message.Append("<td height=90 align=left valign=middle style='font-family:Arial, Helvetica, sans-serif; font-size:11px; color:#000000;'>Keep the Registration Certificate and insurance documents ready for prospective buyers to inspect.</td>");
				message.Append("<td height=90 align=left valign=middle style='font-family:Arial, Helvetica, sans-serif; font-size:11px; color:#000000;'>&nbsp;</td>");
				message.Append("</tr></table></td></tr></table></td></tr><tr><td height=16>&nbsp;</td></tr><tr>");
				message.Append("<td height=51 align=left valign=middle bgcolor=#e7f8ff style='border:1px solid #d4e8f1; font-family:Tahoma; font-size:12px; font-weight:bold; color:#5b5b5b; padding-left:20px;'>You will soon start receiving responses via email and SMS (at your mobile number " + mobile + ") from people interested in buying your bike.</td>");
				message.Append("</tr><tr><td>&nbsp;</td></tr><tr><td style='font-family:Tahoma; font-size:12px; color:#000000;'>");
				message.Append("Should you need any help, please feel free to write to us at <a href='mailto:contact@bikewale.com' target=_blank style='color:#034fb6;'>contact@bikewale.com</a> or SMS ‘Sell’ to 56767767 and we will respond within 1 working day.<br>");
				message.Append("<br>Thank you once again.<br><br>Regards,<br>Team bikewale</td></tr><tr><td>&nbsp;</td></tr><tr>");
				message.Append("<td height=25 align=left bgcolor=#efeeee style='font-family:Arial, Helvetica, sans-serif; font-size:11px; color:#777777; padding-left:10px;'>P.S. Don&rsquo;t forget to remove your advertisement from the site once your bike is sold.</td>");
				message.Append("</tr><tr><td height=10></td></tr></table></td></tr></table>");
				
				HttpContext.Current.Trace.Warn("Sent Mail: " + message.ToString() );
				
				//Send Mail
				CommonOpn op = new CommonOpn();
				op.SendMail(eMail, subject, message.ToString(), true); 
			}
			catch(Exception err)
			{
				HttpContext.Current.Trace.Warn(err.Message);
				ErrorClass objErr = new ErrorClass(err,HttpContext.Current.Request.ServerVariables["URL"]);
				objErr.SendMail();
			} // catch Exception
		}
		
    }//class
}//namespace