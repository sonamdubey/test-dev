using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Carwale.Notifications;
using Carwale.UI.Common;
using Carwale.DAL.Forums;
using Carwale.Cache.Forums;

namespace Carwale.UI.Community.Mods
{
    public class Default : Page
    {
        protected Button btnContinue, btnCustomerToBan, btnCancelCustomerToBan, btnRemoveBan, btnReactivateForum, btnReactivateForumThread, btnReactivateForNewPosts;
        protected TextBox txtHandleToBan, txtForumId, txtForumThreadId;
        protected Label lblHandleToBanError, lblCustomerToBan, lblCustomerIdToBan, lblForumMessage, lblForumThreadMessage;
        ForumsModeratorDAL modsDal = new ForumsModeratorDAL();
        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
        }

        void InitializeComponent()
        {
            base.Load += new EventHandler(Page_Load);
            btnContinue.Click += new EventHandler(btnContinue_Click);
            btnCustomerToBan.Click += new EventHandler(btnCustomerToBan_Click);
            btnRemoveBan.Click += new EventHandler(btnRemoveBan_Click);
            btnReactivateForum.Click += new EventHandler(btnReactivateForum_Click);
            btnReactivateForNewPosts.Click += new EventHandler(btnReactivateForNewPosts_Click);
            btnReactivateForumThread.Click += new EventHandler(btnReactivateForumThread_Click);
        }

        void Page_Load(object Sender, EventArgs e)
        {
            if (CurrentUser.Id == "-1")
            {
                Response.Redirect("/users/login.aspx?ReturnUrl=" + HttpUtility.UrlEncode(Request.ServerVariables["HTTP_X_REWRITE_URL"]));
            }
            ForumsCache threadInfo = new ForumsCache();
            bool isModerator = threadInfo.IsModerator(CurrentUser.Id);
            if (!(isModerator))
            {
			UrlRewrite.Return404();
            }

            btnCancelCustomerToBan.Attributes.Add("onclick", "CanceCustomerToBan(); return false;");
            btnReactivateForum.Attributes.Add("onclick", "if (ForumReactivateConfirmation() == false) return false;");
            btnReactivateForNewPosts.Attributes.Add("onclick", "if (ForumReactivateForNewPostsConfirmation() == false) return false;");
            btnReactivateForumThread.Attributes.Add("onclick", "if (ForumThreadReactivateConfirmation() == false) return false;");
        }

        void btnContinue_Click(object sender, EventArgs e)
        {     
            DataSet ds = new DataSet();             
            try
            {
                ds = modsDal.GetCustomerDetailsByHandleName(txtHandleToBan.Text.Trim());         
                if(ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {                
                    lblCustomerToBan.Text = "Member you selected is : " + ds.Tables[0].Rows[0]["Customer"].ToString();               
                    lblCustomerIdToBan.Text = ds.Tables[0].Rows[0]["CustomerId"].ToString();
                    lblCustomerToBan.Visible = true;
                    btnCancelCustomerToBan.Visible = true;
                    lblHandleToBanError.Visible = false;
                    txtHandleToBan.Enabled = false;
                    btnContinue.Enabled = false;               
                    GetAction(ds.Tables[0].Rows[0]["CustomerId"].ToString());
                }
                else
                {
                    lblHandleToBanError.Text = "Handle name you entered does not exists";
                    lblCustomerIdToBan.Text = "-1";
                    lblHandleToBanError.Visible = true;
                    btnCustomerToBan.Visible = false;
                    btnRemoveBan.Visible = false;
                    btnCancelCustomerToBan.Visible = false;
                    lblCustomerToBan.Visible = false;
                    txtHandleToBan.Enabled = true;
                    btnContinue.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }     
        }

        private void GetAction(string customerId)
        {
            DataSet ds = new DataSet();        
            try
            {          
                ds = modsDal.GetAction(customerId);         
                if(ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    btnCustomerToBan.Visible = false;
                    btnRemoveBan.Visible = true;
                }
                else
                {
                    btnCustomerToBan.Visible = true;
                    btnRemoveBan.Visible = false;
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }         
        }

        void btnCustomerToBan_Click(object sender, EventArgs e)
        {          
            try
            {
                bool status = modsDal.BanCustomerById(lblCustomerIdToBan.Text, CurrentUser.Id);
                if (status)
                {
                    txtHandleToBan.Text = "";
                    lblHandleToBanError.Text = "Successfully banned";
                    lblHandleToBanError.Visible = true;
                }
                else
                {
                    lblHandleToBanError.Text = "This participant is already banned";
                    lblHandleToBanError.Visible = true;
                }

                txtHandleToBan.Enabled = true;
                btnContinue.Enabled = true;
                lblCustomerToBan.Visible = false;
                btnCustomerToBan.Visible = false;
                btnCancelCustomerToBan.Visible = false;
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }
        void btnRemoveBan_Click(object sender, EventArgs e)
        {
            try
            {           
                modsDal.RemoveBanById(lblCustomerIdToBan.Text);
                lblHandleToBanError.Text = "Successfully removed from banned list";
                lblHandleToBanError.Visible = true;
                txtHandleToBan.Text = "";
                txtHandleToBan.Enabled = true;
                btnContinue.Enabled = true;
                lblCustomerToBan.Visible = false;
                btnCustomerToBan.Visible = false;
                btnRemoveBan.Visible = false;
                btnCancelCustomerToBan.Visible = false;
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }

        void btnReactivateForum_Click(object sender, EventArgs e)
        {      
            try
            {          
                modsDal.ReActivateForumUser(txtForumId.Text.Trim());
                modsDal.LogModAction(CurrentUser.Id, "", "Restored-Forum", txtForumId.Text.Trim());
                txtForumId.Text = "";
                lblForumMessage.Text = "Thread successfully restored";
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }      
        }

        void btnReactivateForNewPosts_Click(object sender, EventArgs e)
        {  
            try
            {           
                modsDal.ActivateForNewsPost(txtForumId.Text.Trim());
                modsDal.LogModAction(CurrentUser.Id, "", "Reopened-Forum", txtForumId.Text.Trim());
                txtForumId.Text = "";
                lblForumMessage.Text = "Thread successfully reopened for new posts";
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }

        void btnReactivateForumThread_Click(object sender, EventArgs e)
        {      
            try
            {                       
                modsDal.ActivateForumThread(txtForumThreadId.Text.Trim());
                modsDal.LogModAction(CurrentUser.Id, txtForumThreadId.Text.Trim(), "Reactivate-Thread", "");
                txtForumThreadId.Text = "";
                lblForumThreadMessage.Text = "Post successfully restored";

            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }       
        }
    }
}