/*******************************************************************************************************
THIS IS FOR THE DEFAULT PAGE OF THE carwale
*******************************************************************************************************/
using System;
using System.Text;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Configuration;
using Carwale.UI.Controls;
using System.Text.RegularExpressions;
using Carwale.Notifications;
using Carwale.UI.Common;

namespace Carwale.UI.Users
{
    public class ReferToFriend : Page
    {
        protected TextBox txtName, txtEmail, txtFriendEmail;
        protected Button btnSend;
        protected Label lblMessage;
        public bool dataSaved = false;

        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
        }

        void InitializeComponent()
        {
            base.Load += new EventHandler(Page_Load);
            btnSend.Click += new EventHandler(SendMail);
        }

        void Page_Load(object Sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (CurrentUser.Id != "-1")
                {
                    txtName.Text = CurrentUser.Name;
                    txtEmail.Text = CurrentUser.Email;
                }

                ViewState["Link"] = Request.ServerVariables["HTTP_REFERER"];
            }
        } // Page_Load

        void SendMail(object Sender, EventArgs e)
        {
            if (txtName.Text.ToString().Trim() == "")
            {
                lblMessage.Text = "Please Provide Your Name.";
            }
            else if (txtEmail.Text.ToString().Trim() == "")
            {
                lblMessage.Text = "Please Provide Your Email.";
            }
            else if (IsEmail(txtEmail.Text.ToString().Trim()) == false)
            {
                lblMessage.Text = "Please enter a valid email id";
            }
            else if (txtFriendEmail.Text.ToString().Trim() == "")
            {
                lblMessage.Text = "Please Provide Your Friend's Email.";
            }
            else if (txtFriendEmail.Text.Trim().ToString() != "")
            {
                string[] testEmail = txtFriendEmail.Text.Split((",").ToCharArray());
                for (int i = 0; i < testEmail.Length; i++)
                {
                    string strEmail = testEmail[i].ToString().ToLower().Trim();

                    if (IsEmail(strEmail) == false)
                    {
                        lblMessage.Text = "Please enter a valid email id";
                    }
                    else
                    {
                        try
                        {
                            string link = "https://www.carwale.com";

                            if (ViewState["Link"] != null && ViewState["Link"].ToString() != "")
                                link = ViewState["Link"].ToString();
                            dataSaved = true;
                            Mails.ReferToFriend(txtName.Text, txtEmail.Text, strEmail, link);

                        }
                        catch (Exception err)
                        {
                            Trace.Warn(err.Message);
                            ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                            objErr.SendMail();
                        } // catch Exception
                    }
                }
            }
        }


        protected bool IsEmail(string strEmail)
        {
            if (Regex.IsMatch(strEmail, @"^[_a-z0-9-]+(\.[_a-z0-9-]+)*@[a-z0-9-]+(\.[a-z0-9-]+)*(\.([a-z]){2,4})$"))
            {
                return true;
            }
            else
                return false;
        }

    } // class
} // namespace