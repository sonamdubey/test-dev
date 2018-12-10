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
using Carwale.UI.Common;
using Carwale.DAL.Forums;
using Carwale.Cache.Forums;

namespace Carwale.UI.Forums
{
    public class MoveThread : Page
    {
        #region Global Variables
        protected Label lblMessage, Thread;
        protected Button butSave;
        protected TextBox txtTitle;
        protected DropDownList cmbCategories;
        string threadId = "";
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
                Response.Redirect("/forums/");
            // check if user is moderator?
            if (!GetModeratorLoginStatus(CurrentUser.Id))
                Response.Redirect("/forums/");
            if (Request.QueryString["threadId"] != null && CommonOpn.CheckId(Request.QueryString["threadId"]))
            {
                threadId = Request.QueryString["threadId"];
            }
            else
            {
                Response.Redirect("/forums/");
            }

            if (!IsPostBack)
            {
                FillCategories();
                // Quote the post.
                FillExisting(threadId);
            }
        } // Page_Load
        #endregion

        #region Move Thread Button Click
        void butSave_Click(object Sender, EventArgs e)
        {
            var postDetails = new PostDAL();
            postDetails.MovePost(txtTitle.Text.Trim().ToString(), Convert.ToInt32(cmbCategories.SelectedValue), Convert.ToInt32(threadId));
            lblMessage.Text = "Post Updated Successfully!";
            var forumsCache = new ForumsCache();
            forumsCache.InvalidatePaginationCache(string.Format("Forums-Id-{0}-Page",ViewState["oldforumsId"]), 1);
            forumsCache.InvalidatePaginationCache(string.Format("Forums-Id-{0}-Page", cmbCategories.SelectedValue), 1);
        }
        #endregion

        #region Fill Categories
        void FillCategories()
        {
            DataSet ds = null;
            var threadDetails = new ThreadsDAL();
            ds = threadDetails.FillCategories();
            cmbCategories.DataSource = ds.Tables[0];
            cmbCategories.DataTextField = "Category";
            cmbCategories.DataValueField = "Id";
            cmbCategories.DataBind();
        }
        #endregion

        #region Fill Existing Details
        void FillExisting(string postId)
        {
            DataSet ds = null;
            var threadDetails = new ThreadsDAL();
            ds = threadDetails.FillExistingCategories(threadId);
            txtTitle.Text = ds.Tables[0].Rows[0][0].ToString();
            ViewState["oldforumsId"] = ds.Tables[0].Rows[0][1].ToString();
            cmbCategories.SelectedIndex = cmbCategories.Items.IndexOf(cmbCategories.Items.FindByValue(ds.Tables[0].Rows[0][1].ToString()));	// Fill Car Month.		
        }
        #endregion

        #region Get Moderator Login Status
        bool GetModeratorLoginStatus(string customerId)
        {
            var threadDetails = new ForumsCache();
            return threadDetails.IsModerator(customerId);
        }
        #endregion
    } // class
} // namespace