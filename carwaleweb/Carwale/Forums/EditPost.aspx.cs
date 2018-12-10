using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Carwale.UI.Controls;
using Carwale.UI.Common;
using Carwale.DAL.Forums;
using Carwale.Cache.Forums;

namespace Carwale.UI.Forums
{
    public class EditPost : Page
    {
        #region Global Variables
        protected Label lblMessage;
        protected Button butSave;
        protected RichTextEditor rteET;
        protected Repeater rptThreads;
        string postId = "";
        public string threadId = "";
        public int page = 1;
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
            // check if user is logged in?
            if (CurrentUser.Id == "-1")
                Response.Redirect("./");
            // check if user is moderator?
            if (!GetModeratorLoginStatus(CurrentUser.Id))
                Response.Redirect("./");
            if (Request.QueryString["postId"] != null && CommonOpn.CheckId(Request.QueryString["postId"]))
            {
                postId = Request.QueryString["postId"];
            }
            else
            {
                Response.Redirect("./");
            }
            if (Request.QueryString["page"] != null && CommonOpn.CheckId(Request.QueryString["page"]))
            {
                int.TryParse(Request.QueryString["page"],out page);
            }
            else
            {
                Response.Redirect("./");
            }
            if (Request["thread"] != null && Request.QueryString["thread"] != "")
            {
                threadId = Request.QueryString["thread"];

                //verify the id as p assed in the url
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
                // Quote the post.
                FillExistingPost(postId);
            }
        } // Page_Load
        #endregion

        #region Save Post Click

        void butSave_Click(object Sender, EventArgs e)
        {
            PostDAL postDetails = new PostDAL();
            bool CheckForValidHyperlinks = SanitizeHTML.VerifyAllHyperlinks(rteET.Text.Trim());
            if(CheckForValidHyperlinks)
            {
                bool postEditStatus = postDetails.EditPostByUser(Convert.ToInt32(postId), SanitizeHTML.ToSafeHtml(rteET.Text.Trim()).ToString(), CurrentUser.Id);
                lblMessage.Text = "Post Updated Successfully!";
                var forumsCache = new ForumsCache();
                forumsCache.InvalidatePaginationCache(string.Format("Forums-Thread-{0}-Page", threadId), page);
            }
            else
            {
                lblMessage.Text = "ERROR:Post not Submitted. One or more of your links is/are not valid or absolute";
            }
            
        }
        #endregion

        #region Fill Existing Post
        void FillExistingPost(string postId)
        {
            PostDAL postDetails = new PostDAL();
            rteET.Text = postDetails.FillExistingPost(postId);
        }
        #endregion

        #region Get Moderator Login Status
        bool GetModeratorLoginStatus(string customerId)
        {
            ForumsCache threadDetails = new ForumsCache();
            return (threadDetails.IsModerator(customerId));
        }
        #endregion
    } // class
} // namespace
