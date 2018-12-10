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
using Carwale.UI.Common;
using Carwale.DAL.Forums;
using Carwale.Entity;

namespace Carwale.UI.Forums
{
    public class EditPostByUser : Page
    {
        #region Global Variables
        protected Label lblMessage;
        protected Button butSave, butCancel;
        protected RichTextEditor rteET;
        protected Repeater rptThreads;
        string postId = "";
        public string threadId = "";
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
            butCancel.Click += new EventHandler(butCancel_Click);
        }
        #endregion

        #region Page Load
        void Page_Load(object Sender, EventArgs e)
        {
            // check if user is logged in?
            if (CurrentUser.Id == "-1")
                Response.Redirect("/forums/");

            if (Request.QueryString["post"] != null && CommonOpn.CheckId(Request.QueryString["post"]))
            {
                postId = Request.QueryString["post"];
            }
            else
            {
                Response.Redirect("/forums/");
            }

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

            // check if user is correct ?
            if (!GetLoginStatus(CurrentUser.Id, postId))
            {
                Response.Redirect(threadId + "-" + ThreadUrl + ".html");
            }

            if (!IsPostBack)
            {
                if (GetForum() == false)
                {
                    Response.Redirect("./");
                    return;
                }
                FillExistingPost(postId);
            }
        } // Page_Load
        #endregion

        #region Cancel Button Click

        void butCancel_Click(object Sender, EventArgs e)
        {
            Response.Redirect("/Forums/viewthread.aspx?thread=" + threadId + "&post=" + postId);
        }
        #endregion

        #region Save Button Click

        void butSave_Click(object Sender, EventArgs e)
        {
            UserDAL userDetails = new UserDAL();
            //first check whether the customer is in the banned list or not.
            if (userDetails.IsUserBanned(CurrentUser.Id) == false)
            {
                PostDAL postDetails = new PostDAL();
                bool postEditStatus = postDetails.EditPostByUser(Convert.ToInt32(postId), SanitizeHTML.ToSafeHtml(rteET.Text.Trim()).ToString(), CurrentUser.Id);
                lblMessage.Text = "Post Updated Successfully!";
                Response.Redirect("/Forums/viewthread.aspx?thread=" + threadId + "&post=" + postId);
            }
            else
            {
                lblMessage.Text = "Your 'CarWale Forum' membership has been suspended. You cannot post in CarWale Forums anymore. If it looks like a mistake to you, please write to banwari@carwale.com.<br><br>";
            }

        }
        #endregion

        #region Get Forum Details
        public bool GetForum()
        {
            bool retVal = false;
            try
            {
                ThreadsDAL threadDetails = new ThreadsDAL();
                ThreadBasicInfo ThreadInfo = threadDetails.GetForumForUserPostEdit(threadId);
                ForumId = ThreadInfo.ForumId;
                ForumName = ThreadInfo.ForumName;
                ThreadName = ThreadInfo.ThreadName;
                ThreadUrl = ThreadInfo.ThreadUrl;
                ForumUrl = ThreadInfo.ForumUrl;
                retVal = true;
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
                retVal = false;
            } // catch Exception
            return retVal;
        }
        #endregion

        #region Fill Existing Post

        void FillExistingPost(string postId)
        {
            PostDAL postDetails = new PostDAL();
            rteET.Text = postDetails.FillExistingPost(postId);
        }
        #endregion

        #region Get Login Status

        bool GetLoginStatus(string userId, string postId)
        {
            UserDAL userDetails = new UserDAL();
            return (userDetails.GetLoginStatus(userId, postId));
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
    } // class
} // namespace