using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Bikewale.Common;
using Bikewale.Forums.Common;
using Bikewale.Controls;

namespace Bikewale.Forums
{
    public class Reply : System.Web.UI.Page
    {

        protected HtmlGenericControl divThread;
        protected HtmlGenericControl divForum;
        protected Label lblMessage, lblCaptcha;
        protected Button butSave;
        protected TextBox txtTopic, txtEmail, txtCaptcha;
        protected RichTextEditor rteRT;
        protected Repeater rptThreads;
        protected HtmlTableRow trCustomer, trCaptcha, trAlert;
        protected CheckBox chkEmailAlert;

        public string threadId = "", customerId = "";

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

        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
        }

        void InitializeComponent()
        {
            base.Load += new EventHandler(Page_Load);
            butSave.Click += new EventHandler(butSave_Click);
        }

        void Page_Load(object Sender, EventArgs e)
        {
            customerId = CurrentUser.Id;
            // Modified By :Lucky Rathore on 12 July 2016.
            Form.Action = Request.RawUrl;
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
                Response.Redirect("/forums/notauthorized.aspx?returnUrl=" + HttpUtility.UrlEncode(Request.ServerVariables["HTTP_X_ORIGINAL_URL"]));
            }

            //Trace.Warn(HttpUtility.UrlEncode("http://server/users/commonlogin.aspx?returnUrl=" + Request.ServerVariables["HTTP_X_ORIGINAL_URL"]));

            //also get the forumId
            if (Request["thread"] != null && Request.QueryString["thread"] != "")
            {
                threadId = Request.QueryString["thread"];

                //verify the id as passed in the url
                if (CommonOpn.CheckId(threadId) == false)
                {
                    //redirect to the default page
                    Response.Redirect("./");
                    return;
                }
            }
            else
            {
                //redirect to the default page
                Response.Redirect("./");
                return;
            }
          
            if (!IsPostBack)
            {
                if (GetForum() == false)
                {
                    Response.Redirect("./");
                    return;
                }

                ShowPreviousPosts();
                //verify the id as passed in the url
                if (Request.QueryString["quote"] != null && CommonOpn.CheckId(Request.QueryString["quote"]))
                {
                    QuotePost(Request.QueryString["quote"]);
                    Response.Cookies["Forum_MultiQuotes"].Expires = DateTime.Now.AddDays(-1);
                }
                else if (Request.Cookies["Forum_MultiQuotes"] != null && Request.Cookies["Forum_MultiQuotes"].Value.ToString() != "")
                {
                    QuotePost(Request.Cookies["Forum_MultiQuotes"].Value.ToString().Substring(1, Request.Cookies["Forum_MultiQuotes"].Value.ToString().Length - 1));
                    Response.Cookies["Forum_MultiQuotes"].Expires = DateTime.Now.AddDays(-1);
                }

                UserTracking ut = new UserTracking();
                ut.SaveActivity(CurrentUser.Id.ToString(), "5", ForumId, threadId);
            }
        } // Page_Load

        void butSave_Click(object Sender, EventArgs e)
        {
            bool noSpam = false; // if true, it ensures that there is no spamming attempt

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
                    if (txtCaptcha.Text.Trim() == Request.Cookies["CaptchaImageText"].Value)
                    {
                        noSpam = true;

                        // now expire the cookie
                        Response.Cookies["CaptchaImageText"].Expires = DateTime.Now.AddDays(-1);
                    }
                }
            }

            if (noSpam)
            {
                //now check whether the customer is in the banned list or not.
                if (ForumsCommon.IsUserBanned(customerId) == false)
                {
                    string postId = SaveDetails();
                    string msgPage = "/forums/viewthread-" + threadId + ".html#post" + postId;

                    lblCaptcha.Text = "";

                    //Mails.NotifyForumSubscribedUsers( "rajeev@bikewale.com", "Rajeev", "Some Name", "threadName", msgPage);

                    if (postId != "-1")
                    {
                        //send mails to all the participants
                        ForumsCommon.NotifySubscribers(msgPage, threadId, this.ThreadName);

                        //redirect it to message page
                        Response.Redirect(msgPage);
                    }
                    else
                        lblMessage.Text = "ERROR:Your post could not be submitted. Please try again.";
                }
                else
                {
                    lblMessage.Text = "Your 'BikeWale Forum' membership has been suspended. You cannot post in BikeWale Forums anymore. If it looks like a mistake to you, please write to banwari@bikewale.com.<br><br>";
                }

            }
            else
            {
                txtCaptcha.Text = "";
                lblCaptcha.Text = "Invalid code. Please try again.";
            }
        }

        public bool GetForum()
        {
            bool retVal = false, replyStatus = false;
            Database db = new Database();
            SqlDataReader dr = null;

            string sql = "";

            sql = " SELECT FC.ID, FC.Name, FC.Description, F.Topic, IsNull(C.IsFake,0) AS IsStarterFake, "
                + " F.ReplyStatus, IsNull(C.Name,'') AS StartByName, IsNull(C.email,'') AS StartByEmail, "
                + " F.StartDateTime AS StartedOn FROM ForumSubCategories AS FC, "
                + " Forums AS F With(NoLock) LEFT JOIN Customers AS C With(NoLock) ON C.ID = F.CustomerId "
                + " WHERE F.ID = @ThreadId AND F.IsActive = 1 AND "
                + " FC.ID = F.ForumSubCategoryId AND FC.IsActive = 1";

            Trace.Warn(sql);

            try
            {
                SqlCommand cmd = new SqlCommand(sql);

                cmd.Parameters.Add("@ThreadId", SqlDbType.BigInt).Value = threadId;

                dr = db.SelectQry(cmd);

                if (dr.Read())
                {
                    ForumId = dr["ID"].ToString();
                    ForumName = dr["Name"].ToString();
                    ForumDescription = dr["Description"].ToString();
                    ThreadName = dr["Topic"].ToString();

                    StartByName = dr["StartByName"].ToString();
                    StartByEmail = dr["StartByEmail"].ToString();
                    IsStarterFake = Convert.ToBoolean(dr["IsStarterFake"].ToString());
                    replyStatus = Convert.ToBoolean(dr["ReplyStatus"].ToString());

                    retVal = true;
                }



                if (!replyStatus)
                {
                    retVal = false;
                }
            }
            catch (SqlException err)
            {
                Trace.Warn(err.Message);
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();

                retVal = false;
            } // catch Exception
            finally
            {
                if(dr != null)
                    dr.Close();
                db.CloseConnection();
            }

            return retVal;
        }


        string SaveDetails()
        {
            int alertType = 0;

            if (trAlert.Visible)
                if (chkEmailAlert.Checked)
                    alertType = 2; // instant email subscription
                else
                    alertType = 1; // normal subscription
            else
                alertType = 0; // do not subscribe at all

            return ForumsCommon.SavePost(customerId, SanitizeHTML.ToSafeHtml(rteRT.Text.Trim()), alertType, threadId);
        }


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

        void ShowPreviousPosts()
        {
            CommonOpn op = new CommonOpn();
            string sql;
            Database db = new Database();

            try
            {
                sql = " SELECT TOP 10 FT.ID, IsNull(U.HandleName,'anonymous') AS PostedBy, FT.Message, FT.MsgDateTime "
                    + " FROM ForumThreads AS FT With(NoLock) LEFT JOIN UserProfile AS U With(NoLock) ON U.UserId = FT.CustomerId"
                    + " WHERE FT.ForumId = @ForumId AND FT.IsActive = 1 "
                    + " ORDER BY FT.MsgDateTime DESC ";


                SqlParameter[] param = { new SqlParameter("@ForumId", threadId) };

                op.BindRepeaterReader(sql, rptThreads, param);

                if (rptThreads.Items.Count == 0)
                {
                    divThread.Visible = false;
                }
                else
                {
                    divThread.Visible = true;
                }
            }
            catch (SqlException err)
            {
                Trace.Warn(err.Message);
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            } // catch Exception
        }

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

        void QuotePost(string postId)
        {
            string sql;
            Database db = new Database();
            SqlDataReader dr = null;
            try
            {
                SqlCommand cmd = new SqlCommand();

                sql = " SELECT IsNull(U.HandleName,'anonymous') AS PostedBy, Message "
                    + " FROM ForumThreads AS FT With(NoLock) LEFT JOIN UserProfile AS U With(NoLock) ON U.UserId = FT.CustomerId "
                    + " WHERE FT.Id IN (" + db.GetInClauseValue(postId, "Id", cmd) + ") AND FT.IsActive = 1 ORDER BY FT.Id";
                //+ " WHERE FT.Id = @PostId AND FT.IsActive = 1 ";

                //SqlParameter [] param = {new SqlParameter("@PostId", postId)};
                cmd.CommandText = sql;

                dr = db.SelectQry(cmd);
                rteRT.Text = "";

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
            catch (SqlException err)
            {
                Trace.Warn(err.Message);
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            } // catch Exception
            finally
            {
                if(dr != null)
                    dr.Close();

                db.CloseConnection();
            }

        }

    }//class
}//namespace