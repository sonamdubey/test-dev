using System;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.Caching;
using WURFL;
using WURFL.Config;
using MobileWeb.DataLayer;
using System.Net.Mail;
using System.Configuration;

namespace MobileWeb
{
    public class Feedback : System.Web.UI.Page
    {

        protected LinkButton btnSubmit;
        private HttpContext objTrace = HttpContext.Current;
        protected TextBox txtName, txtEmail, txtMobile, txtDesc, TextBox1;
        protected HiddenField hdnRdoFeedback;
        bool sendMail = false;
        protected string custName = "", custEmail = "", custMobile = "",feedbackType = "",description = "",visitorUrl = "" ;

        protected override void OnInit( EventArgs e )
        {
	        InitializeComponent();
        }
		
        void InitializeComponent()
        {
	        base.Load += new EventHandler( Page_Load );
            btnSubmit.Click += new EventHandler(btnSubmit_Click);
        }

        void Page_Load(object Sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                btnSubmit.Attributes.Add("onclick", "javascript:if(IsValid()==false)return false;");
                
            }
        }

        /*
            * Author : Supriya K
            * Created Date : 27/8/2013
            * Desc : To send mail on submit button click 
            */
        void btnSubmit_Click(object sender, EventArgs e)
        {
            custName = txtName.Text;
            custEmail = txtEmail.Text;
            custMobile = txtMobile.Text;
            feedbackType = hdnRdoFeedback.Value;
            description = txtDesc.Text;
            visitorUrl = "www.carwale.com" + Request.QueryString["returnUrl"].ToString();
            Trace.Warn("visitorUrl" + visitorUrl);
            SendMail();
            Response.Redirect(Request.QueryString["returnUrl"].ToString());
        }

        public void SendMail()
        {
            string sendError = "";
            //get the value of sendError from web.config file
            try
            {
                sendError = ConfigurationManager.AppSettings["sendError"].ToString();
                if (sendError != "On")
                {
                    sendError = "Off";
                }
            }
            catch (Exception)
            {
                sendError = "Off";
            }

            if (sendError == "On")
                sendMail = true;	//default it is false

            if (sendMail == true)
            {

                try
                {
                    // make sure we use the local SMTP server
                    //SmtpClient client = new SmtpClient("124.153.73.180");//"127.0.0.1";
                    SmtpClient client = new SmtpClient(ConfigurationManager.AppSettings["SMTPSERVER"]);
                    //get the from mail address
                    string localMail = ConfigurationManager.AppSettings["localMail"].ToString();

                    MailAddress from = new MailAddress(localMail, "CarWale.com");

                    //get the to mail id
                    //string email = ConfigurationManager.AppSettings["errorMailTo"].ToString();
                    string email = "mobile@carwale.com";
                    string subject = " CarWale Mobile Feedback ";

                    // Set destinations for the e-mail message.
                    MailAddress to = new MailAddress(email);

                    // create mail message object
                    MailMessage msg = new MailMessage(from, to);

                    // Add Reply-to in the message header.
                    //msg.Headers.Add("Reply-to", "contact@carwale.com");

                    // set some properties
                    msg.IsBodyHtml = true;
                    msg.Priority = MailPriority.High;

                    msg.CC.Add(new MailAddress("neil.andrews@carwale.com"));       //actaul address  
                    //msg.CC.Add(new MailAddress("supriya.k@carwale.com"));           //for testing

                    //prepare the subject
                    msg.Subject = subject;

                    StringBuilder sb = new StringBuilder();

                    sb.Append("<p>Customer Name : " + custName + "</p>");
                    sb.Append("<p>Customer EmailId : " + custEmail + "</p>");
                    sb.Append("<p>Customer Mobile No : " + custMobile + "</p>");
                    sb.Append("<p>Feedback Type : " + feedbackType + "</p>");
                    sb.Append("<p>Description : " + description + "</p>");
                    sb.Append("<p>Visitor Url : " + visitorUrl + "</p>");
                    msg.Body = sb.ToString();

                    //body = " Person Accessing the Page : " + CurrentUser.Email + "\n" + body;                            

                    // Mail Server Configuration. Needed for Rediff Hosting.
                    //msg.Fields["http://schemas.microsoft.com/cdo/configuration/sendusing"] = 1;
                    //msg.Fields["http://schemas.microsoft.com/cdo/configuration/smtpserverpickupdirectory"] = "C:\\inetpub\\mailroot\\pickup";

                    // Send the e-mail
                    client.Send(msg);

                    objTrace.Trace.Warn(msg.From + "," + msg.To + "," + msg.Subject + "," + msg.Body);
                }
                catch (Exception err)
                {
                    objTrace.Trace.Warn("Feedback:SendMail: " + err.Message);

                }
            }
        }
    }
}