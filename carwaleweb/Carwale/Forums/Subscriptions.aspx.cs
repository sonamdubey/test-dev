using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Carwale.UI.Common;
using Carwale.DAL.Forums;
using Carwale.BL.Forums;

namespace Carwale.UI.Forums
{
    public class Subscriptions : Page
    {
        #region Global Variables
        protected HtmlGenericControl divForum, divMessage;
        protected Repeater rptForums, rptThreads;
        protected RadioButton optTopics, optAll;
        protected DropDownList cmbAlertType;
        protected Button btnUnsubscribe, btnUpdateSubscription;
        private string threadId = "", customerId = "";
        #endregion

        #region On Init
        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
        }
        #endregion+

        #region Initialize Component
        void InitializeComponent()
        {
            base.Load += new EventHandler(Page_Load);
            btnUpdateSubscription.Click += new EventHandler(btnUpdateSubscription_Click);
            btnUnsubscribe.Click += new EventHandler(btnUnsubscribe_Click);
        }
        #endregion

        #region Page Load
        void Page_Load(object Sender, EventArgs e)
        {
            if (Request["id"] != null && CommonOpn.CheckId(Request["Id"]))// someone has requested forum subscription
            {
                threadId = Request["id"];
            }
            string returnUrl = "subscriptions.aspx";
            if (threadId != "")
                returnUrl += "?id=" + threadId;
            customerId = CurrentUser.Id;
            if (customerId == "-1")
            {
                Response.Redirect("/users/login.aspx?ReturnUrl=" + returnUrl);
            }
            if (!IsPostBack)
            {
                ShowSubscriptions();
                SaveUserActivity saveAct = new SaveUserActivity();
                saveAct.saveActivity(CurrentUser.Id, "6", "-1", "-1", CurrentUser.CWC);
            }
        } // Page_Load
        #endregion

        #region Update Subscription - Butoon

        void btnUpdateSubscription_Click(object sender, EventArgs e)
        {
            ManageSubscriptions("changeType");
            divMessage.InnerHtml = "Notification mode updated successfully.";
            divMessage.Visible = true;
        }

        #endregion

        #region Unsubscribe Button

        void btnUnsubscribe_Click(object sender, EventArgs e)
        {
            ManageSubscriptions("unsubscribe");
            divMessage.InnerHtml = "Selected threads unsubscribed successfully.";
            divMessage.Visible = true;
        }
        #endregion

        #region Manage Subscriptions
        /// <summary>
        /// This method manages subsciption based on the action type. Action Type may be any of : Change Type Or unsubscribe.
        /// </summary>
        /// <param name="actionType"></param>
        void ManageSubscriptions(string actionType)
        {
            Label lblId;
            CheckBox chk;
            for (int i = 0; i < rptForums.Items.Count; i++)
            {
                lblId = (Label)rptForums.Items[i].FindControl("lblId");
                chk = (CheckBox)rptForums.Items[i].FindControl("chkThread");
                if (chk.Checked)
                {
                    ForumSubscriptionsDAL subscriptionDetails = new ForumSubscriptionsDAL();
                    subscriptionDetails.ManageSubscriptions(actionType, Convert.ToInt32(cmbAlertType.SelectedValue), Convert.ToInt32(customerId), Convert.ToInt32(lblId.Text));
                }

            }
            ShowSubscriptions();
            cmbAlertType.SelectedIndex = 0;
        }
        #endregion

        #region Show Subscriptions
        /// <summary>
        /// this function will list all the subsctiptions of a particular user.
        /// </summary>
        void ShowSubscriptions()
        {
            DataSet ds = null;
            CommonOpn op = new CommonOpn();
            ForumSubscriptionsDAL subscriptionDetails = new ForumSubscriptionsDAL();
            ds = subscriptionDetails.ShowSubscriptions(customerId);
            rptForums.DataSource = ds;
            rptForums.DataBind();
            if (rptForums.Items.Count == 0)
            {
                divForum.Visible = false;
                divMessage.InnerHtml = "You haven't subscribed to any discussion!";
                divMessage.Visible = true;
            }
            else
            {
                divForum.Visible = true;
            }
        }

        #endregion

        #region Get Last Post
        /// <summary>
        /// Gets the last post in the subscription.
        /// </summary>
        /// <param name="title"></param>
        /// <param name="name"></param>
        /// <param name="date"></param>
        /// <param name="id"></param>
        /// <param name="posts"></param>
        /// <param name="startedById"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        protected string GetLastPost(string title, string name, string date, string id, string posts, string startedById, string url)
        {
            PostsBusinessLogic postDetails = new PostsBusinessLogic();
            return (postDetails.GetLastPost(title, name, date, id, posts, startedById, url));
        }
        #endregion

        #region Get Last Post Thread
        /// <summary>
        /// Gets the Last Post Thread for the subscription.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="date"></param>
        /// <param name="lastPostById"></param>
        /// <returns></returns>
        protected string GetLastPostThread(string name, string date, string lastPostById)
        {
            PostDAL postDetails = new PostDAL();
            return (postDetails.GetLastPostThread(name, date, lastPostById));
        }
        #endregion

    } // class
} // namespace