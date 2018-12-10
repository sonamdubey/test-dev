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
using System.Collections;
using Ajax;
using Carwale.Notifications;
using CarwaleAjax;
using Carwale.DAL.Forums;
using Carwale.UI.Common;
using Carwale.Cache.Forums;

namespace Carwale.UI.Forums
{
    public class StickyThread : Page
    {
        #region Global Variables
        protected Label lblMessage, Thread;
        protected Button butSave, btnDelete;
        protected RadioButton rdbStickyCat, rdbStickyForum;
        protected HtmlGenericControl divCreate, divRemove;
        string threadId = "", strCat = "", type;
        private string customerId = "";

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
            btnDelete.Click += new EventHandler(btnDelete_Click);
        }
        #endregion

        #region Page Load
        void Page_Load(object Sender, EventArgs e)
        {
            // check if user is logged in?
            customerId = CurrentUser.Id;
            if (customerId == "-1")
                Response.Redirect("/forums/");
            // check if user is moderator?
            if (!GetModeratorLoginStatus(customerId))
                Response.Redirect("/forums/");
            if (Request["threadId"] != null && Request.QueryString["threadId"].ToString() != "")
            {
                threadId = Request.QueryString["threadId"].ToString();

                if (CommonOpn.CheckId(threadId) == false)
                {
                    return;
                }
            }
            if (Request["type"] != null && Request.QueryString["type"].ToString() != "")
            {
                type = Request.QueryString["type"].ToString();

                if (CommonOpn.CheckId(threadId) == false)
                {
                    return;
                }
            }
            Ajax.Utility.RegisterTypeForAjax(typeof(AjaxForum));
            if (type == "1")
            {
                divCreate.Visible = true;
                divRemove.Visible = false;
            }
            else if (type == "2")
            {
                divCreate.Visible = false;
                divRemove.Visible = true;
            }
            else
            {
                return;
            }
        } // Page_Load
        #endregion

        #region Save As Sticky
        void butSave_Click(object Sender, EventArgs e)
        {
            try
            {
                string saveId = SaveData("-1");
                Trace.Warn("Save data = " + saveId);
                if (saveId != "-1")
                {
                    lblMessage.Text = "Data Saved Successfully";
                }
                else
                {
                    lblMessage.Text = "You are trying to put duplicate entry.";
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }
        #endregion

        #region Delete

        void btnDelete_Click(object Sender, EventArgs e)
        {
            ThreadsDAL threadDetails = new ThreadsDAL();
            threadDetails.DeleteStickyThreads(Convert.ToInt32(threadId), Convert.ToInt32(customerId));
            lblMessage.Text = "Data Removed Successfully";
        }
        #endregion

        #region Save Data

        string SaveData(string sId)
        {
            ThreadsDAL threadDetails = new ThreadsDAL();
            if (rdbStickyCat.Checked)
            {
                strCat = "1";
            }
            else if (rdbStickyForum.Checked)
            {
                strCat = "2";
            }
            Trace.Warn("threadi d= " + threadId);
            return (threadDetails.InsertStickyThreads(Convert.ToInt32(sId), Convert.ToInt32(threadId), Convert.ToInt32(strCat), Convert.ToInt32(customerId)));
        }
        #endregion

        #region Get Moderator Login Status

        bool GetModeratorLoginStatus(string customerId)
        {
            ForumsCache threadDetails = new ForumsCache();
            return threadDetails.IsModerator(customerId);
        }
        #endregion
    } // class
} // namespace