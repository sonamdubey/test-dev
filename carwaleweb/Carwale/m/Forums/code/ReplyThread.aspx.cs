using System;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using MobileWeb.Common;
using MobileWeb.DataLayer;
using MobileWeb.Controls;
using Carwale.Notifications.Logs;
using System.Data.Common;
using Carwale.DAL.CoreDAL.MySql;
using Carwale.Utility;

namespace MobileWeb.Forums
{
	public class ReplyThread : Page
	{
		protected LinkButton btnSave;
		string customerId = "-1";
		protected string threadId;
		protected string topic = "", forumSubCatName = "", forumSubCatId = "", replyingToPosts = "";
        protected TextBox txtReply, txtCapcha;
        protected Label lblQuotedPosts, lblQuotedPostsVisible, errorMsg;
        protected Boolean isModerator, isWaitTimeExceeded, isEmailVerified, isModerationRequired, isCustomerRestricted;
		protected Label lblServerError;
		protected bool _isUserBanned = false;
		protected HtmlGenericControl divReplyForm, divBannedMsg; 
		protected CheckBox chkNotify, chkQuotedPosts;
        protected HiddenField hdnCustomerId, hdnThreadId, hdnThreadTopic, hdnPostCount, hdnLatestPostTime, hdnPostUrl, hdnThreadUrl, hdnForumSubCatName,hdnTopic;
        protected string Forum_MultiQuotes="";
        protected string[] arrQuote;
        protected Label lblCaptchaError;
        protected int isModerated = 1;
        protected string threadUrl = "", postUrl = "";

		protected override void OnInit( EventArgs e )
		{
			InitializeComponent();
		}
		
		void InitializeComponent()
		{
			base.Load += new EventHandler( Page_Load );
			btnSave.Click += new EventHandler( btnSave_Click );
		}
		
		void Page_Load( object Sender, EventArgs e )
		{
			if (!Page.IsPostBack)
			{
				btnSave.Attributes.Add("onclick", "javascript:if(InputValid()==false)return false;");	
				
				if(Request["thread"] != null && Request.QueryString["thread"] != "")
				{
					threadId = Request.QueryString["thread"];
					if(CommonOpn.CheckId(threadId) == false)
					{
                        Response.Redirect("~/m/forums/");
						return;
					}				
				}
				else
				{
                    Response.Redirect("~/m/forums/");
				}
			
				customerId = CurrentUser.Id;
				hdnCustomerId.Value = customerId;
				hdnThreadId.Value = threadId;
				
				if (customerId == "-1")
                    Response.Redirect("~/m/users/auth.aspx?returnUrl=" + HttpUtility.UrlEncode(Request.ServerVariables["HTTP_X_REWRITE_URL"]));
			
				if (CommonOpn.CheckUserHandle(customerId) == false)
                    Response.Redirect("~/m/users/createhandle.aspx?returnUrl=" + HttpUtility.UrlEncode(Request.ServerVariables["HTTP_X_REWRITE_URL"]));	
			
				GetThreadDetails();

                if (Request.QueryString["quote"] != null && Request.QueryString["quote"] != "")
						GetPostsToReplyTo( Request.QueryString["quote"] );
                else if (Request.Cookies["Forum_MultiQuotes"] != null && Request.Cookies["Forum_MultiQuotes"].Value.ToString() != "")
                {
                    arrQuote = Request.Cookies["Forum_MultiQuotes"].Value.Split(':');
                    Forum_MultiQuotes = arrQuote[1];
                    Trace.Warn("Forum_MultiQuotes : " + Forum_MultiQuotes);
                    GetPostsToReplyTo(Forum_MultiQuotes.Substring(1, Forum_MultiQuotes.Length - 1));
                    Response.Cookies["Forum_MultiQuotes"].Expires = DateTime.Now.AddDays(-1);
                }

				if (CommonOpn.IsUserBanned(customerId))
				{
					divReplyForm.Visible = false;
					divBannedMsg.Visible = true;
				}
                GetCustomerHistory();
                //GetCustomerInfo(CurrentUser.Id);//delete it before going live
			}
		}

        /*Author: Rakesh    
         * Date Created:27/5/2013
         * Description: Check if user is moderator or not (postCount>=50'moderator' else normal user) and give customer history
         */
        private void GetCustomerHistory()
        {
            IDataReader dr = null;
            Forum obj = new Forum();
            try
            {
                obj.GetReader = true;
                obj.GetCustomerHistory(CurrentUser.Id);
                dr = obj.drReader;
                if (dr.Read())
                {
                    hdnPostCount.Value = dr["Posts"].ToString();
                    hdnLatestPostTime.Value = dr["LatestPost"].ToString();
                }
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            finally
            {
                obj.drReader.Close();
            }
        }

        void ReplyToThread()
        {
            Trace.Warn("postUrl : " + hdnPostUrl);
            //if server side validation is true then only proceed
            if (txtReply.Text.Trim() == "")
            {
                lblServerError.Visible = true;
                lblServerError.Text = "(Reply Required)";
            }
            else if (txtReply.Text.Trim().Length > 4000)
            {
                lblServerError.Visible = true;
                lblServerError.Text = "(Max 4000 characters)";
            }
            else
            {
                lblServerError.Visible = false;

                string _reply = "";
                if (chkQuotedPosts.Checked == false)
                    _reply = txtReply.Text.Trim();
                else
                    _reply = lblQuotedPosts.Text.Trim() + txtReply.Text.Trim();

                int alertType = 1;
                if (chkNotify.Checked)
                    alertType = 2;

                string postId = "-1";
                if (!isModerationRequired)
                {             
                    isModerated = 1;
                    postId = ForumsCommon.SavePost(hdnCustomerId.Value, _reply, alertType, hdnThreadId.Value, isModerated);        
                    if (postId != "-1")
                    {                  
                        string msgPage = "/Forums/" + hdnThreadId.Value + "-" + hdnPostUrl.Value + "-" + GetLastPage() + ".html";
                        ForumsCommon.NotifySubscribers(msgPage, hdnThreadId.Value, hdnThreadTopic.Value);
                 
                        Trace.Warn("url : " + "~/m/forums/" + hdnThreadId.Value + "-" + hdnPostUrl.Value + "-p" + GetLastPage() + ".html");
                        Response.Redirect("~/m/forums/" + hdnThreadId.Value + "-" + hdnPostUrl.Value  + "-p" + GetLastPage() + ".html");
                    }
                    else
                    {
                        lblServerError.Visible = true;
                        lblServerError.Text = "(Error occured. Try Again.)";
                    }
                }
                else 
                {
                    isModerated = 0;
                    postId = ForumsCommon.SavePost(hdnCustomerId.Value, _reply, alertType, hdnThreadId.Value, isModerated);
                    Response.Redirect("~/m/forums/ShowInModeration.aspx");
                }
            }
        }

		void btnSave_Click( object Sender, EventArgs e )
		{
            errorMsg.Text = "";
            lblCaptchaError.Text = "";

            GetCustomerInfo(CurrentUser.Id);
            if (!isCustomerRestricted)
            {
                if (isModerator)
                {
                    ReplyToThread();
                }
                else
                {
                    //Trace.Warn("IsEmailVerified" + isEmailVerified);
                    if (isEmailVerified)
                    {
                        if (isWaitTimeExceeded)
                        {
                            string cookieVal = "", captchaText = "";
                            cookieVal = Request.Cookies["CaptchaImageText"].Value;
                            captchaText = CarwaleSecurity.Decrypt(cookieVal, true);
                            if (captchaText == txtCapcha.Text.Trim())
                            {
                                ReplyToThread();
                            }
                            else
                            {
                                //pls enter code correctly
                                lblCaptchaError.Text = "Invalid code.Please try again.";
                            }
                        }
                        else
                        {
                            //Wait for 5 min
                            lblCaptchaError.Text = "Wait for 5 minutes to reply thread";
                        }
                    }
                    else
                    {
                        //send verification mail
                        CommonOpn co = new CommonOpn();
                        co.SendRegMail(CurrentUser.Id);
                        errorMsg.Text = "A verification email has been sent to your registered email address. Please verify your email address in order to continue posting in the forum.";
                    }
                }
            }
            else
            {
                //your carwale membership is banned
                errorMsg.Text = "Your 'CarWale Forum' membership has been suspended. You cannot post in CarWale Forums anymore. If it looks like a mistake to you, please write to banwari@carwale.com.<br><br>";
            }
		}
		
		private string GetLastPage()
		{
			int totalPages = 0;
		    IDataReader dr = null;
			Forum obj = new Forum();
			try
			{
				obj.GetReader = true;
				obj.GetThreadPostCount(hdnThreadId.Value);
				dr = obj.drReader;
				if (dr.Read())
				{
					int totalPosts = 0;
					totalPosts = Convert.ToInt32(dr[0].ToString());
					totalPages = totalPosts / 10;
					if (totalPosts % 10 != 0)
						totalPages++;
				}
			}
			catch(Exception err)
			{
				ErrorClass objErr = new ErrorClass(err,Request.ServerVariables["URL"]);
				objErr.SendMail();
			}
			finally
			{
				obj.drReader.Close();
			}
			return totalPages.ToString();
		}
		
		private void GetThreadDetails()
		{
			IDataReader dr = null;
			Forum obj = new Forum();
			try
			{
				obj.GetReader = true;
				obj.GetThreadDetails(threadId);
				dr = obj.drReader;
				if (dr.Read())
				{
					topic = dr["Topic"].ToString();	
					hdnThreadTopic.Value = topic;
					forumSubCatName = dr["Name"].ToString();
					forumSubCatId = dr["ID"].ToString();
                    hdnThreadUrl.Value = dr["ThreadUrl"].ToString();
                    threadUrl = dr["ThreadUrl"].ToString();
                    hdnPostUrl.Value = dr["PostUrl"].ToString();
                    postUrl = dr["PostUrl"].ToString();
                    hdnForumSubCatName.Value = dr["Name"].ToString();
                    hdnTopic.Value = dr["Topic"].ToString();
				}
			}
			catch(Exception err)
			{
				ErrorClass objErr = new ErrorClass(err,Request.ServerVariables["URL"]);
				objErr.SendMail();
			}
			finally
			{
				obj.drReader.Close();
			}
		}

        /*Author: Rakesh    
         * Date Created: 10/07/2013
         * Description:get customer info to perform operations
         */
        private void GetCustomerInfo(string customerId)
        {
            try
            {
                using(DbCommand cmd =DbFactory.GetDBCommand("GetCustomerInfoForForumSubmit_v16_11_7"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_CutomerId", DbType.Int64, customerId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_IsModerator", DbType.Boolean, ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_IsWaitTimeExceeded", DbType.Boolean, ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_IsEmailVerified", DbType.Boolean, ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_IsModerationRequired", DbType.Boolean, ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_IsCustomerRestricted", DbType.Boolean, ParameterDirection.Output));
                    LogLiveSps.LogSpInGrayLog(cmd);
                    MySqlDatabase.ExecuteNonQuery(cmd, DbConnections.ForumsMySqlReadConnection);
                    isModerator = Convert.ToBoolean(cmd.Parameters["v_IsModerator"].Value);
                    isWaitTimeExceeded = Convert.ToBoolean(cmd.Parameters["v_IsWaitTimeExceeded"].Value);
                    isEmailVerified = Convert.ToBoolean(cmd.Parameters["v_IsEmailVerified"].Value);
                    isModerationRequired = Convert.ToBoolean(cmd.Parameters["v_IsModerationRequired"].Value);
                    isCustomerRestricted = Convert.ToBoolean(cmd.Parameters["v_IsCustomerRestricted"].Value);   
                }
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("Create Thread : " + ex.Message);
            }
        }
		
		private void GetPostsToReplyTo(string postId)
		{
			IDataReader dr = null;
			Forum obj = new Forum();
			try
			{
				obj.GetReader = true;
				obj.GetPostsToReplyTo(postId);
				dr = obj.drReader;
				
				replyingToPosts = "";	
				while ( dr.Read() )
				{
                    Trace.Warn("msg : " + dr["Message"].ToString());
					string quotedText = dr["Message"].ToString();

					if ( quotedText.IndexOf( "[^^/quote^^]" ) >= 0 )
						quotedText = quotedText.Substring( quotedText.LastIndexOf( "[^^/quote^^]" ) + 12, quotedText.Length - ( quotedText.LastIndexOf( "[^^/quote^^]" ) + 12 ) );
					
					replyingToPosts += "[^^quote=" + dr["PostedBy"].ToString()
								  + "^^] " 
								  + quotedText + "[^^/quote^^]<br /><br />";
				}
				lblQuotedPosts.Text = replyingToPosts;
				lblQuotedPostsVisible.Text = replyingToPosts;
			}
			catch(Exception err)
			{
				ErrorClass objErr = new ErrorClass(err,Request.ServerVariables["URL"]);
				objErr.SendMail();
			}
			finally
			{
				obj.drReader.Close();
			}
		}
	}
}	
