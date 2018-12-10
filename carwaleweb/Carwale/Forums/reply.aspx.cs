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
using Carwale.UI.Controls;
using Carwale.Notifications;
using Carwale.DAL.CoreDAL;
using Carwale.UI.Common;
using Carwale.DAL.Forums;
using Carwale.BL.Forums;
using Carwale.Entity;
using Carwale.Cache.Forums;
using RabbitMqPublishing;
using System.Collections.Specialized;
using System.Data.Common;
using Carwale.DAL.CoreDAL.MySql;
using MySql.Data.MySqlClient;
using Carwale.Utility;
using AEPLCore.Queue;
using System.Configuration;
using Google.Protobuf;

namespace Carwale.UI.Forums
{
    public class ReplyToThread : Page
    {
        #region Global Variables
        protected HtmlGenericControl divThread;
        protected HtmlGenericControl divForum;
        protected Label lblMessage, lblCaptcha;
        protected Button butSave;
        protected TextBox txtTopic, txtEmail, txtCaptcha;
        protected RichTextEditor rteRT;
        protected Repeater rptThreads;
        protected HtmlTableRow trCustomer, trCaptcha, trAlert;
        protected CheckBox chkEmailAlert;
        protected int IsModerated = 1;

        public string threadId = "", customerId = "";
        public int pageNo;
        #endregion

        #region Properties
        public string ThreadUrl
        {
            get
            {
                if (ViewState["ThreadUrl"] != null)
                    return ViewState["ThreadUrl"].ToString();
                else
                    return "";
            }
            set
            {
                ViewState["ThreadUrl"] = value;
            }
        }

        public string ForumUrl
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

        public string ForumId
        {
            get
            {
                if (ViewState["ForumId"] != null)
                    return ViewState["ForumId"].ToString();
                else
                    return "";
            }
            set { ViewState["ForumId"] = value; }
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

        public string ThreadName
        {
            get
            {
                if (ViewState["ThreadName"] != null)
                    return ViewState["ThreadName"].ToString();
                else
                    return "";
            }
            set { ViewState["ThreadName"] = value; }
        }

        public string StartByEmail
        {
            get
            {
                if (ViewState["StartByEmail"] != null)
                    return ViewState["StartByEmail"].ToString();
                else
                    return "";
            }
            set { ViewState["StartByEmail"] = value; }
        }

        public string StartByName
        {
            get
            {
                if (ViewState["StartByName"] != null)
                    return ViewState["StartByName"].ToString();
                else
                    return "";
            }
            set { ViewState["StartByName"] = value; }
        }

        public bool IsStarterFake
        {
            get
            {
                if (ViewState["IsStarterFake"] != null)
                    return Convert.ToBoolean(ViewState["IsStarterFake"].ToString());
                else
                    return true;
            }
            set { ViewState["IsStarterFake"] = value; }
        }
        #endregion

        #region On Init

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
            if (Request["thread"] != null && Request.QueryString["thread"] != "")//also get the forumId
            {
                threadId = Request.QueryString["thread"];
                if (CommonOpn.CheckId(threadId) == false)//verify the id as passed in the url
                {
                    Response.Redirect("./");//redirect to the default page
                    return;
                }
            }
            else
            {
                Response.Redirect("./");//redirect to the default page
                return;
            }
            if (Request["page"] != null && CommonOpn.CheckId(Request["page"]))
            {
                pageNo = Convert.ToInt32(Request["page"]);
            }
            UserDAL userDetails = new UserDAL();
            if (userDetails.CheckUserHandle(CurrentUser.Id) != false)//Check the handle. if it does not exist then redirect the user to change the handle page
            {
                Response.Redirect("/users/EditUserHandle.aspx?returnUrl=" + HttpUtility.UrlEncode(Request.ServerVariables["HTTP_X_REWRITE_URL"]));
            }
            if (!IsPostBack)
            {
                if (GetForum() == false)
                {
                    Response.Redirect("./");
                    return;
                }
                ShowPreviousPosts();
                if (Request.QueryString["quote"] != null && CommonOpn.CheckId(Request.QueryString["quote"]))//verify the id as passed in the url
                {
                    QuotePost(Request.QueryString["quote"]);
                    Response.Cookies["Forum_MultiQuotes"].Expires = DateTime.Now.AddDays(-1);
                }
                else if (Request.Cookies["Forum_MultiQuotes"] != null && Request.Cookies["Forum_MultiQuotes"].Value.ToString() != "")
                {
                    QuotePost(Request.Cookies["Forum_MultiQuotes"].Value.ToString().Substring(1, Request.Cookies["Forum_MultiQuotes"].Value.ToString().Length - 1));
                    Response.Cookies["Forum_MultiQuotes"].Expires = DateTime.Now.AddDays(-1);
                }
                SaveUserActivity saveAct = new SaveUserActivity();
                saveAct.saveActivity(CurrentUser.Id, "5", ForumId, threadId, CurrentUser.CWC);
            }
            bool checkforum = forumsecuritycheck.Captchacheck(customerId);
            if (!checkforum)
            {
                trCaptcha.Visible = true;
            }
        } // Page_Load
        #endregion

        #region  Reply Button Click

        void butSave_Click(object Sender, EventArgs e)
        {
            bool noSpam = false; // if true, it ensures that there is no spamming attempt
            ForumsSecurityChecks forumsecuritycheck = new ForumsSecurityChecks();
            int ForumCustomerCheck = forumsecuritycheck.CheckForumPostEligibility(customerId);
            bool CheckForModeration = forumsecuritycheck.CheckForModeration(customerId);
            if (ForumCustomerCheck != 0 && ForumCustomerCheck != -2)
            {
                if (ForumCustomerCheck == 1)
                {
                    lblMessage.Text = "The forum requires you to wait for 5 minutes before you post again. This restriction is lifted once you have 50 posts. Inconvenience is deeply regretted. ";
                    txtCaptcha.Text = "";
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
                                Response.Cookies["CaptchaImageText"].Expires = DateTime.Now.AddDays(-1);// now expire the cookie
                            }
                        }
                    }
                    if (noSpam)
                    {
                        UserDAL userDetails = new UserDAL();
                        if (userDetails.IsUserBanned(customerId) == false)//now check whether the customer is in the banned list or not.
                        {
                            if (CheckForModeration) // Indicates that posts need to be moderated
                            {
                                IsModerated = 0;
                            }
                            bool CheckForValidHyperlinks = SanitizeHTML.VerifyAllHyperlinks(rteRT.Text.Trim());
                            if(CheckForValidHyperlinks) // Check whether all the hyperlinks are valid or not
                            {
                                string postId = SaveDetails();
                                if (IsModerated == 0)
                                {
                                    Response.Redirect("/forums/PostResponse.aspx");
                                }
                                string msgPage = "/Forums/viewthread.aspx?thread=" + threadId + "&post=" + postId;
                                var forumsCache = new ForumsCache();
                                forumsCache.InvalidatePaginationCache(string.Format("Forums-Thread-{0}-Page", threadId), pageNo);
                                CreateNVC publishToRMQ = new CreateNVC();
                                publishToRMQ.UpdateLuceneIndex(threadId, "1");
                                lblCaptcha.Text = "";
                                if (postId != "-1")
                                {
                                    NotificationsBusinessLogic notifyUsers = new NotificationsBusinessLogic();
                                    UserBusinessLogic cm = new UserBusinessLogic();
                                    var handleData = cm.GetExistingHandleDetails(Convert.ToInt32(CurrentUser.Id));
                                    notifyUsers.NotifySubscribers(msgPage, threadId, this.ThreadName, (handleData != null ? handleData.HandleName : string.Empty), CurrentUser.Id, CurrentUser.Email);//send mails to all the participants
                                    Response.Redirect(msgPage);//redirect it to message page
                                }
                                else
                                    lblMessage.Text = "ERROR:Your post could not be submitted. Please try again.";
                            }
                            else
                            {
                                lblMessage.Text = "ERROR:Post not Submitted. One or more of your links is/are not valid or absolute";
                                txtCaptcha.Text = "";   
                            }
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

        #region Get Forum

        public bool GetForum()
        {
            
            ForumsCache threadDL = new ForumsCache();
            ThreadBasicInfo threadForuminfo = threadDL.GetAllForums(threadId);

            PublishManager rmq = new PublishManager();
            var payload = new NewCarConsumers.ThreadId
            {
                Id = Convert.ToInt32(threadId)
            };

            var message = new QueueMessage
            {
                ModuleName = (ConfigurationManager.AppSettings["NewCarConsumerModuleName"] ?? string.Empty),
                FunctionName = "ForumThreadViewCount",
                Payload = payload.ToByteString(),
                InputParameterName = "ThreadId",
            };
            rmq.PublishMessageToQueue((ConfigurationManager.AppSettings["ThreadViewCountQueue"] ?? "FORUMTHREADVIEWCOUNTMYSQL").ToUpper(), message);

            bool retVal = false, replyStatus = false;
            ForumId = threadForuminfo.ForumId;
            ForumName = threadForuminfo.ForumName;
            ForumDescription = threadForuminfo.ForumDescription;
            ThreadName = threadForuminfo.ThreadName;
            StartByName = threadForuminfo.StartedByName;
            StartByEmail = threadForuminfo.StartedByEmail;
            IsStarterFake = threadForuminfo.IsStarterFake;
            replyStatus = threadForuminfo.replyStatus;
            ThreadUrl = threadForuminfo.ThreadUrl;
            ForumUrl = threadForuminfo.ForumUrl;
            retVal = true;
            if (!replyStatus)
            {
                retVal = false;
            }
            return retVal;
        }
        #endregion

        #region Save Details

        string SaveDetails()
        {
            PostDAL savepost = new PostDAL();
            int alertType = 0;

            if (trAlert.Visible)
                if (chkEmailAlert.Checked)
                    alertType = 2; // instant email subscription
                else
                    alertType = 1; // normal subscription
            else
                alertType = 0; // do not subscribe at all
            string remoteAddr = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"] == null ? null : HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            string clientIp = HttpContext.Current.Request.ServerVariables["HTTP_CLIENT_IP"] == null ? null : HttpContext.Current.Request.ServerVariables["HTTP_CLIENT_IP"];
            return savepost.SavePost(customerId, SanitizeHTML.ToSafeHtml(rteRT.Text.Trim()), alertType, threadId, IsModerated, remoteAddr, clientIp);
        }
        #endregion

        #region Get Title

        public string GetTitle(string value)
        {
            string[] words = value.Split(' ');

            string retVal = "";

            for (int i = 0; i < words.Length; i++)
            {
                if (i == 0)
                {
                    retVal = "<span>" + words[i] + "</span>";
                }
                else
                    retVal += " " + words[i];
            }

            return retVal;
        }
        #endregion

        #region Show Previous Posts

        void ShowPreviousPosts()
        {
            PostDAL postDL = new PostDAL();
            DataSet ds = postDL.ShowPreviousPosts(threadId);
            rptThreads.DataSource = ds.Tables[0];
            rptThreads.DataBind();
            if (rptThreads.Items.Count == 0)
            {
                divThread.Visible = false;
            }
            else
            {
                divThread.Visible = true;
            }

        }
        #endregion

        #region Get Message

        public string GetMessage(string value)
        {
            string post = value;
            post = post.Replace("\n", "<br>");

            string quoteStart = "<div class='quote'>Posted by <b>";
            string quotePostedByEnd = "</b><br>";
            string quoteEnd = "</div>";

            // Identify and replace quotes
            if (post.ToLower().IndexOf("[^^quote=") >= 0)
            {
                Trace.Warn("Quote Found");
                post = post.Replace("[^^quote=", quoteStart);
                post = post.Replace("[^^/quote^^]", quoteEnd);
                post = post.Replace("^^]", quotePostedByEnd);

                Trace.Warn(post);
            }

            return post;
        }
        #endregion

        #region Quote Post
        void QuotePost(string postId)
        {       
            try
            {            
                using (DbCommand cmd = DbFactory.GetDBCommand("GetQuotePostDetails_v16_11_7"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_PostIds", DbType.String, 100, postId));
                    using(IDataReader dr = MySqlDatabase.SelectQuery(cmd,DbConnections.ForumsMySqlReadConnection))
                    {
                        while (dr.Read())
                        {
                            string quotedText = dr["Message"].ToString();

                            if (quotedText.IndexOf("[^^/quote^^]") >= 0)
                                quotedText = quotedText.Substring(quotedText.LastIndexOf("[^^/quote^^]") + 12, quotedText.Length - (quotedText.LastIndexOf("[^^/quote^^]") + 12));

                            rteRT.Text += "[^^quote=" + dr["PostedBy"].ToString()
                                + "^^] "
                                + quotedText + "[^^/quote^^]<br /><br />";
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
        }
        #endregion
    } // class
} // namespace
