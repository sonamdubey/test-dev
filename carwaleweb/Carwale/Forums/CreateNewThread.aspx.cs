/*******************************************************************************************************
IN THIS CLASS THE NEW MEMBEERS WHO HAVE REQUESTED FOR REGISTRATION ARE SHOWN
*******************************************************************************************************/
using System;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Carwale.UI.Common;
using Carwale.UI.Controls;
using System.Text.RegularExpressions;
using RabbitMqPublishing;
using System.Configuration;
using System.Collections.Specialized;
using Carwale.Notifications;
using Carwale.DAL.CoreDAL;
using Carwale.DAL.Forums;
using Carwale.BL.Forums;
using Carwale.Cache.Forums;
using AEPLCore.Cache;
using System.Data.Common;
using Carwale.DAL.CoreDAL.MySql;
using MySql.Data.MySqlClient;
using Carwale.Utility;

namespace Carwale.UI.Forums
{
    public class CreateNewThread : Page
    {
        #region Global Variables
        protected HtmlGenericControl divForum;
        protected Label lblMessage, lblCaptcha, lblResult;
        protected Button butSave;
        protected TextBox txtTopic, txtEmail, txtCaptcha;
        protected RichTextEditor rteNT;
        protected HtmlTableRow trCustomer, trCaptcha, trAlert;
        protected CheckBox chkEmailAlert;
        protected int IsModerated = 1;
        protected string url = "-1";
        public string forumId = "", customerId = "";
        public const int _noOfRefreshPage = 1;
        #endregion

        #region Properties
        public string forumUrl
        {
            get
            {
                if (ViewState["ForumUrl"] != null)
                    return ViewState["ForumUrl"].ToString();
                else
                    return "";
            }
            set { ViewState["ForumUrl"] = value; }
        }

        public string ForumName
        {
            get
            {
                if (ViewState["ForumName"] != null)
                    return ViewState["ForumName"].ToString();
                else
                    return "";
            }
            set { ViewState["ForumName"] = value; }
        }

        public string ForumDescription
        {
            get
            {
                if (ViewState["ForumDescription"] != null)
                    return ViewState["ForumDescription"].ToString();
                else
                    return "";
            }
            set { ViewState["ForumDescription"] = value; }
        }
        #endregion

        #region OnInit

        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
        }
        #endregion

        #region Initialize Component

        void InitializeComponent()
        {
            base.Load += new EventHandler(Page_Load);
            butSave.Click += new EventHandler(butSave_Click);
        }
        #endregion

        #region Page Load
        void Page_Load(object Sender, EventArgs e)
        {
            customerId = CurrentUser.Id;
            ForumsSecurityChecks forumsecuritycheck = new ForumsSecurityChecks();
            //if the customer is already registered then dont ask information for the 
            //customer name and location, else ask the name, email and the location
            if (customerId != "-1")
            {
                trCustomer.Visible = false;
                trCaptcha.Visible = false;
                trAlert.Visible = true;
            }
            else
            {

                Response.Redirect("/users/login.aspx?ReturnUrl=" + HttpUtility.UrlEncode(Request.ServerVariables["HTTP_X_REWRITE_URL"]));
            }

            //also get the forumId
            if (Request["forum"] != null && Request.QueryString["forum"] != "")
            {
                forumId = Request.QueryString["forum"];

                //verify the id as passed in the url
                if (CommonOpn.CheckId(forumId) == false)
                {
                    //redirect to the default page
                    Response.Redirect("default.aspx");
                    return;
                }
            }
            else
            {
                //redirect to the default page
                Response.Redirect("default.aspx");
                return;
            }
            UserDAL userDetails = new UserDAL();
            if (userDetails.CheckUserHandle(CurrentUser.Id) != false)
            {
                Response.Redirect("/users/EditUserHandle.aspx?returnUrl=" + HttpUtility.UrlEncode(Request.ServerVariables["HTTP_X_REWRITE_URL"]));
            }


            if (!IsPostBack)
            {
                ForumName = GetForum();
                SaveUserActivity saveAct = new SaveUserActivity();
                saveAct.saveActivity(CurrentUser.Id, "3", forumId, "-1", CurrentUser.CWC);
            }

            bool checkforum = forumsecuritycheck.Captchacheck(customerId);
            if (!checkforum)
            {
                trCaptcha.Visible = true;
            }

        } // Page_Load

        #endregion

        #region Button Save Click

        void butSave_Click(object Sender, EventArgs e)
        {
            ForumsSecurityChecks forumsecuritycheck = new ForumsSecurityChecks();
            bool noSpam = false; // if true, it ensures that there is no spamming attempt
            int ForumCustomerCheck = forumsecuritycheck.CheckForumPostEligibility(customerId);
            bool CheckForModeration = forumsecuritycheck.CheckForModeration(customerId); // Check if user posts need to be moderated.
            // The following test condition means that the customer is a verified customer
            if (ForumCustomerCheck != 0 && ForumCustomerCheck != -2)
            {
                if (ForumCustomerCheck == 1)
                {
                    lblMessage.Text = "The forum requires you to wait for 5 minutes before you post again. This restriction is lifted once you have 50 posts. Inconvenience is deeply regretted. ";
                    txtCaptcha.Text = "";
                    noSpam = false;
                }
                else
                {
                    // check if user is logged in. 
                    // If he is logged in then there is no chance of spamming.
                    if (!trCaptcha.Visible)
                    {
                        noSpam = true;
                    }
                    else
                    {
                        // or else try matching captcha image text and user input
                        if (Request.Cookies["CaptchaImageText"] != null)
                        {
                            if (txtCaptcha.Text.Trim() == CarwaleSecurity.Decrypt(Request.Cookies["CaptchaImageText"].Value, true))
                            {
                                noSpam = true;
                                // now expire the cookie
                                Response.Cookies["CaptchaImageText"].Expires = DateTime.Now.AddDays(-1);
                            }
                        }
                    }
                    if (noSpam)
                    {
                        UserDAL userDetails = new UserDAL();
                        //now check whether the customer is in the banned list or not.
                        if (userDetails.IsUserBanned(customerId) == false)
                        {
                            if (CheckForModeration) // Indicates that posts need to be moderated
                            {
                                IsModerated = 0;
                            }

                            string threadId = SaveDetails();
                            var forumsCache = new ForumsCache();
                            forumsCache.InvalidatePaginationCache(string.Format("Forums-Id-{0}-Page", forumId), _noOfRefreshPage);
                            if (IsModerated == 0)
                            {
                                Response.Redirect("/forums/PostResponse.aspx");
                            }
                            CreateNVC publishToRMQ = new CreateNVC();
                            publishToRMQ.UpdateLuceneIndex(threadId, "1");

                            string msgPage = "/forums/" + threadId + "-" + url + ".html";

                            lblCaptcha.Text = "";

                            if (threadId != "-1")
                            {
                                Response.Redirect(msgPage);
                            }
                            else
                                lblMessage.Text = "ERROR:Your entry could not be submitted. Please try again.";
                        }
                        else
                        {
                            lblMessage.Text = "Your 'CarWale Forum' membership has been suspended. You cannot post in CarWale Forums anymore. If it looks like a mistake to you, please write to banwari@carwale.com.<br><br>";
                        }
                    }
                    else
                    {
                        txtCaptcha.Text = "";
                        lblCaptcha.Text = "Invalid code. Please try again.";
                    }

                }

            }
            // Ask user to verify email first and only then can he post in the forum.
            else if (ForumCustomerCheck == 0)
            {
                lblMessage.Text = "A verification email has been sent to your registered email address. Please verify your email address in order to continue posting in the forum.";
                Mails.CustomerRegistration(customerId);
            }

            else if (ForumCustomerCheck == -2)
            {
                lblMessage.Text = "Your 'CarWale Forum' membership has been suspended. You cannot post in CarWale Forums anymore. If it looks like a mistake to you, please write to banwari@carwale.com.<br><br>";
            }
        }

        #endregion

        #region Get Forum Details

        public string GetForum()
        {
            string forum = "";
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("GetForumSubCategoryById_v16_11_7"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_ForumId", DbType.Int64, forumId));
                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, DbConnections.ForumsMySqlReadConnection))
                    {
                        if (dr.Read())
                        {
                            forum = dr["Name"].ToString();
                            ForumDescription = dr["Description"].ToString();
                            forumUrl = dr["Url"].ToString();
                        }
                    }
                }
            }
            catch (MySqlException err)
            {
                Trace.Warn(err.Message);
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            } // catch Exception

            return forum;
        }

        #endregion

        #region Save Thread Details

        string SaveDetails()
        {
            Trace.Warn("save detils reached");
            string threadId = "-1";
            ThreadsDAL createthread = new ThreadsDAL();
            int alertType = 0;
            if (trAlert.Visible)
                if (chkEmailAlert.Checked) alertType = 2; // instant email subscription
                else alertType = 1; // normal subscription
            else alertType = 0; // do not subscribe at all
            //Topic worked on to get a url
            Trace.Warn("save detils reached");
            string title = txtTopic.Text.Trim();
            string tmp = Regex.Replace(title, @"[^a-zA-Z 0-9]", "");//replaces all characters other than a character or a number with a space.
            string iurl = Regex.Replace(tmp, @"\s+", " ");// Replace multiple spaces with a single space
            url = iurl.Trim().Replace(" ", "-").ToLower();//replace space with -.
            Trace.Warn("save detils reached" + url);
            string remoteAddr = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"] == null ? null : HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            string clientIp = HttpContext.Current.Request.ServerVariables["HTTP_CLIENT_IP"] == null ? null : HttpContext.Current.Request.ServerVariables["HTTP_CLIENT_IP"];
            threadId = createthread.CreateNewThread(
                            customerId,
                            SanitizeHTML.ToSafeHtml(rteNT.Text.Trim()),
                            alertType,
                            forumId,
                            txtTopic.Text.Trim(),
                            IsModerated, remoteAddr, clientIp);

            return threadId;
        }

        #endregion

    } // class
} // namespace