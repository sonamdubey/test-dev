using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using MobileWeb.Common;
using MobileWeb.DataLayer;
using System.Text.RegularExpressions;
using Carwale.Notifications.Logs;
using System.Data.Common;
using Carwale.DAL.CoreDAL.MySql;
using Carwale.Utility;

namespace MobileWeb.Forums
{
	public class CreateThread : Page
	{
		protected LinkButton btnSave;
        protected Panel pnlError;
        protected Label lblCaptchaError, errorMsg;
        protected TextBox txtTitle, txtDesc, txtCapcha;
		protected CheckBox chkAlert;
	    private string customerId;
        protected string forumId;
        protected Boolean isModerator, isWaitTimeExceeded, isEmailVerified, isModerationRequired, isCustomerRestricted;
		protected string subCatName;
		protected HtmlGenericControl divCreateForm, divBannedMsg;
        protected HiddenField hdnForumId, hdnLatestPostTime, hdnPostCount, hdnSubCatName, hdnThreadUrl;
        protected int isModerated = 1;
        protected string url = "",threadUrl = "";
       // private double diffenceOfTime;
		protected override void OnInit( EventArgs e )
		{
			InitializeComponent();
		}
		
		void InitializeComponent()
		{
			base.Load += new EventHandler( Page_Load );
			btnSave.Click += new EventHandler( butSave_Click );
		}
		
		void Page_Load( object Sender, EventArgs e )
		{
            if (!Page.IsPostBack)
            { 
                btnSave.Attributes.Add("onclick", "javascript:if(Validate()==false)return false;");

                if (Request["forum"] != null && Request.QueryString["forum"] != "")
                {
                    forumId = Request.QueryString["forum"];
                    Trace.Warn("forumID " + forumId);
                    hdnForumId.Value = forumId;

                    if (CommonOpn.CheckId(forumId) == false)
                    {
                        Response.Redirect("~/m/forums/");
                        return;
                    }
                }
                else
                {
                    Response.Redirect("~/m/forums/");
                    return;
                }

                customerId = CurrentUser.Id;

                if (customerId == "-1")
                    Response.Redirect("~/m/users/auth.aspx?returnUrl=" + HttpUtility.UrlEncode(Request.ServerVariables["HTTP_X_REWRITE_URL"]));

                if (CommonOpn.CheckUserHandle(customerId) == false)
                    Response.Redirect("~/m/users/createhandle.aspx?returnUrl=" + HttpUtility.UrlEncode(Request.ServerVariables["HTTP_X_REWRITE_URL"]));

                GetSubCategoryDetails();

                if (CommonOpn.IsUserBanned(customerId))
                {
                    divCreateForm.Visible = false;
                    divBannedMsg.Visible = true;
                }

                GetCustomerHistory();
            }
		}
		
		private void GetSubCategoryDetails()
		{
			IDataReader dr = null;
			Forum obj = new Forum();
			try
			{
				obj.GetReader = true;
				obj.GetSubCategoryDetails(hdnForumId.Value);
				dr = obj.drReader;
				if (dr.Read())
				{
					subCatName = dr["Name"].ToString();
                    hdnSubCatName.Value = dr["Name"].ToString();
                    threadUrl = dr["Url"].ToString();
                    hdnThreadUrl.Value = dr["Url"].ToString();
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
         * Date Created:23/5/2013
         * Description: Check if user is moderator or not (postCount>=50'moderator' else normal user)
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

        /*Author: Rakesh    
         * Date Modified: 04/07/2013
         * Description: Create new thread and set value of isModerator depevnding on whether moderation is required or not
         */

        void CreateNewThread()
        {
            if (!isModerationRequired)
            {
                //redirect to show thread
                isModerated = 1;
                string threadId = SaveDetails();
                Response.Redirect("~/m/forums/" + threadId + "-" + url +  ".html");
            }
            else
            {
                //your thread is waiting for moderation
                isModerated = 0;
                string threadId = SaveDetails();
                Response.Redirect("~/m/forums/ShowInModeration.aspx");
                //lblCaptchaError.Text = "Your post is in moderation. Inconvinience is deeply regreted. ";
            }
        }

        /*Author: Rakesh    
         * Date Modified: 08/07/2013
         * Description: Perform operations on submit button click and save thread
         */
		void butSave_Click( object Sender, EventArgs e )
		{
            //GetCustomerHistory();
            errorMsg.Text = "";
            lblCaptchaError.Text = "";

            GetCustomerInfo(CurrentUser.Id);
            if (!isCustomerRestricted)
            {
                if (isModerator)
                {
                    CreateNewThread();
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
                                CreateNewThread();
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
                            lblCaptchaError.Text = "Wait for 5 minutes to create new thread";
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

        /*Author: Rakesh    
         * Date Created: 05/07/2013
         * Description:get customer info to perform operations
         */
        private void GetCustomerInfo(string customerId)
        {
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("GetCustomerInfoForForumSubmit_v16_11_7"))
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

        /*Author: Rakesh    
         * Date Modified: 04/07/2013
         * Description: passes parameter isModerated
         */
        string SaveDetails()
		{
			string threadId = "-1";
			int alertType;
			
			if ( chkAlert.Checked ) 
				alertType = 2; // instant email subscription
			else 
				alertType = 1; // normal subscription

            string title = txtTitle.Text.Trim();
            string tmp = Regex.Replace(title, @"[^a-zA-Z 0-9]", "");//replaces all characters other than a character or a number with a space.
            string iurl = Regex.Replace(tmp, @"\s+", " ");// Replace multiple spaces with a single space
            url = iurl.Trim().Replace(" ", "-").ToLower();//replace space with -.
			threadId = ForumsCommon.CreateNewThread( 
							CurrentUser.Id, 
							txtDesc.Text.Trim(),
							alertType,
							hdnForumId.Value,
							txtTitle.Text.Trim(),
                            isModerated);
					
			return threadId;
		}

	}
}
