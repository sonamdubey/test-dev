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
using System.Web.UI;
using System.IO;
using Carwale.Notifications;
using Carwale.UI.Common;
using Carwale.DAL.CoreDAL;
using Carwale.DAL.Customers;
using Carwale.Entity;
using Carwale.Interfaces;

namespace Carwale.UI.Common
{
    public class Mails
    {
        public static void CustomerRegistration(string customerId)
        {
            try
            {
                StringBuilder message = new StringBuilder();
                string subject = "CarWale Registration.";

                CustomerDetails cd = new CustomerDetails(customerId);

                //message.Append( "<img align=\"right\" src=\"https://www.carwale.com/images/logo.gif\" />" );

                message.Append("<h4>Dear " + cd.Name + ",</h4>");

                message.Append("<p>Greetings from Carwale!</p>");

                message.Append("<p>Thank you for choosing Carwale. ");
                message.Append(" We are committed to making your car buying and selling process simpler.</p>");

                //message.Append("<p>For future reference your user id and password will be as listed below:</p>");
                message.Append("<p>For future reference your user id will be as listed below:</p>");

                message.Append("User ID : " + cd.Email + "<br>");
                //message.Append( "Password : " + cd.Password + "<br>" );

                message.Append("<p>You can change your password by visiting at <a href='https://www.carwale.com/MyCarwale/'>www.CarWale.com/MyCarwale/</a></p>");

                //Ravi Uncommented

                //  string cipher =    CarwaleSecurity.EncodeVerificationCode(customerId.ToString());

                string cipher = Utils.Utils.EncryptTripleDES(customerId.ToString());

                HttpContext.Current.Trace.Warn("cipher" + cipher);

                message.Append("<p>Please <a target=\"_blank\" href=\"https://www.carwale.com/users/verifyEmail.aspx?verify=");
                message.Append(cipher + "\">click here</a>");
                message.Append(" to activate your account or copy and paste the following link in the browser’s address-bar.</p>");
                message.Append("<a target=\"_blank\" href=\"https://www.carwale.com/users/verifyEmail.aspx?verify=");
                message.Append(cipher + "\">https://www.carwale.com/users/verifyEmail.aspx?verify=" + cipher + "</a>");

                //Ravi Uncommented


                message.Append("<br>Warm Regards,<br><br>");
                message.Append("Customer Care,<br><b>CarWale</b>");

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
        
        public static void ReferToFriend(string senderName, string senderEmail, string recipientEmail, string link)
        {
            try
            {
                string email = recipientEmail;
                StringBuilder message = new StringBuilder();
                string subject = senderName + " has referred you CarWale.";

                message.Append("<img align=\"right\" src=\"https://www.carwale.com/images/logo.gif\" />");
                message.Append("<h4>Hello,</h4>");
                message.Append("<p>Greetings from Carwale!</p>");
                message.Append("<p><b>" + senderName + "(" + senderEmail + ")" + "</b> ");
                message.Append("has visited Carwale and found this page very useful.</p>");
                message.Append("<p>Please <a target=\"_blank\" href=\"" + link + "\">click</a>");
                message.Append(" the following link to visit the recommended page.</p>");

                message.Append("<p><a target=\"_blank\" href=\"" + link + "\">" + link + "</a>");

                message.Append("<br><br>Regards,<br>");
                message.Append("Customer Care,<br><b>CarWale</b>");

                HttpContext.Current.Trace.Warn("Email: " + email);
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

        public static void InquiryArchivedByCustomer(string inquiryType, string customerId, string inquiryId, string status, string feedback)
        {
            try
            {
                string email = "banwarils@gmail.com, carwale@gmail.com";
                StringBuilder message = new StringBuilder();
                string subject = "Customer has archived his " + inquiryType + " inquiry.";

                message.Append("<h4>Dear Sir,</h4>");
                message.Append("<p>Some customer has archived his " + inquiryType + " inquiry.</p>");
                message.Append("<p>Customer Id : <b>" + customerId + "</b></p> ");
                message.Append("<p>Inquiry Id : <b>" + inquiryId + "</b></p> ");
                message.Append("<p>Status : <b>" + status + "</b></p> ");
                message.Append("<p><b>Comments : </b>" + feedback + " </p>");
                message.Append("<br><br>Regards,<br>");
                message.Append("Customer Care,<br><b>CarWale</b>");

                HttpContext.Current.Trace.Warn("Email: " + email);
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
        
        public static void NewcarDealerOfferInquiry(string dealerEmail, string dealer, string name, string phone, string userEmailId, string modelName)
        {
            try
            {
                string email = dealerEmail;
                StringBuilder message = new StringBuilder();
                string subject = "Offer Request for : Car #" + modelName;

                message.Append("<img align=\"right\" src=\"https://www.carwale.com/images/logo.gif\" />");
                message.Append("<h4>Dear " + dealer + ",</h4>");
                message.Append("<p>Greetings from Carwale!</p>");
                message.Append("<p>CarWale has received a Offer Inquiry for Your Car #" + modelName + "</p>");
                message.Append("<div style=\"background-color:#B5EDBC;\"><h5>Customer Details:</h5>");
                message.Append("<table border=\"0\">");
                message.Append("<tr><td>Name</td><td><b>" + name + "</b></td></tr>");
                message.Append("<tr><td>Primary Phone</td><td><b>" + phone + "</b></td></tr>");
                message.Append("<tr><td>EmailId</td><td><b>" + userEmailId + "</b></td></tr>");
                message.Append("</table></div>");

                message.Append("<br><br>Regards,<br>");
                message.Append("Customer Care,<br><b>CarWale</b>");

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

        public static void UsedCarDealerInquiry(string dealerEmail, string dealer, string name, string phone, string date)
        {
            try
            {
                string email = dealerEmail;
                StringBuilder message = new StringBuilder();
                string subject = "User Interested to know Details of Available Used Cars";

                message.Append("<img align=\"right\" src=\"https://www.carwale.com/images/logo.gif\" />");
                message.Append("<h4>Dear " + dealer + ",</h4>");
                message.Append("<p>Greetings from Carwale!</p>");
                message.Append("<p>CarWale has received a request from user to enquire about Available Cars</p>");
                message.Append("<div style=\"background-color:#B5EDBC;\"><h5>Customer Details:</h5>");
                message.Append("<table border=\"0\">");
                message.Append("<tr><td>Name</td><td><b>" + name + "</b></td></tr>");
                message.Append("<tr><td>Primary Phone</td><td><b>" + phone + "</b></td></tr>");
                message.Append("</table></div>");

                message.Append("<br><br>Regards,<br>");
                message.Append("Customer Care,<br><b>CarWale</b>");

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
    }//class
}//namespace