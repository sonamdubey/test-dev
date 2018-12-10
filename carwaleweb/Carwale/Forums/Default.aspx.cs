using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Carwale.UI.Common;
using Carwale.Notifications;
using Carwale.BL.Forums;
using Carwale.Cache.Forums;
using MySql.Data.MySqlClient;

namespace Carwale.UI.Forums
{
    public class Default : Page
    {
        #region Global Variables
        protected HtmlGenericControl spnError;
        protected Repeater rptParent;
        public DataSet dsForums = new DataSet();
        public int serial = 0;
        public string discussions = "", posts = "", contributors = "", inboxTotal = "";
        public string total = "", guests = "", members = "", mostUsers = "", mostUsersDate = "";
        DataSet ds = new DataSet();
        DataSet dsViews = new DataSet();
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
        }
        #endregion

        #region Page Load

        void Page_Load(object Sender, EventArgs e)
        {
            // call the last login management function.
            UserBusinessLogic userDetails = new UserBusinessLogic();
            string loginCookie = HttpContext.Current.Request.Cookies["ForumLastLogin"] == null ? null : HttpContext.Current.Request.Cookies["ForumLastLogin"].ToString();
            userDetails.ManageLastLogin(CurrentUser.Id, loginCookie);     
            if (!IsPostBack)
            {          
                DeviceDetection dd = new DeviceDetection(Request.ServerVariables["HTTP_X_REWRITE_URL"].ToString());
                dd.DetectDevice();
                /*	Code added by Ashish Ambokar ends here */
                var forumCache = new ForumsCache();
                ds = forumCache.GetAllForums();
                LoadCategoryViews();
                FillRepeaters();        
                SaveUserActivity saveAct = new SaveUserActivity();
                saveAct.saveActivity(CurrentUser.Id, "1", "-1", "-1", CurrentUser.CWC);                    
            }
        } // Page_Load
        #endregion


        #region Load Category Views

        private void LoadCategoryViews()
        {
            var forumsCache = new ForumsCache();
            dsViews = forumsCache.LoadCategoryViews();
        }
        #endregion

        #region Get Category Views
        protected string GetCategoryViews(string categoryId)
        {
            if (dsViews != null)
            {
                DataRow[] dRow = dsViews.Tables[0].Select("CategoryId=" + categoryId);
                if (dRow.Length > 0)
                    return "(" + dRow[0]["NoOfViews"].ToString() + " Viewing)";
                else
                    return "";
            }
            else
            {
                return "";
            }
        }
        #endregion

        #region Fill Repeaters

        /// <summary>
        /// FillRepaters() is used to fill thread titles in each subcategory
        /// Modified By: Prashant Vishe 
        /// On:9 Sept 2012
        /// </summary>
        void FillRepeaters()
        {

            try
            {
                rptParent.DataSource = ds.Tables[1];
                rptParent.DataBind();
            }
            catch (MySqlException err)
            {
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            } // catch Exception
        }
        #endregion

        #region Get Child

        /// <summary>
        /// GetChild() is used to get information regarding subcategory news
        /// Modified By: Prashant Vishe
        /// On: 9 Sept 2012
        /// </summary>
        /// <returns>dataset containing following details. </returns>
        public DataSet GetChild(string id)
        {
            DataSet dsnew = new DataSet();
            DataTable dt = dsnew.Tables.Add();
            DataRow dr;

            dt.Columns.Add("SubCatId", typeof(string));
            dt.Columns.Add("SubCatName", typeof(string));
            dt.Columns.Add("Description", typeof(string));
            dt.Columns.Add("LastThreadId", typeof(string));
            dt.Columns.Add("LastThread", typeof(string));
            dt.Columns.Add("LastPostedBy", typeof(string));
            dt.Columns.Add("LastPostDate", typeof(string));
            dt.Columns.Add("LastPostedById", typeof(string));
            dt.Columns.Add("Threads", typeof(string));
            dt.Columns.Add("Posts", typeof(string));
            dt.Columns.Add("Handle", typeof(string));
            dt.Columns.Add("Url", typeof(string));
            dt.Columns.Add("SubUrl", typeof(string));

            DataRow[] rows = ds.Tables[0].Select(" ForumCategoryId = " + id);
            try
            {
                foreach (DataRow row in rows)
                {
                    dr = dt.NewRow();

                    dr["SubCatId"] = row["ID"];
                    Trace.Warn("sub Cat Id = " + row["ID"].ToString());
                    dr["SubCatName"] = row["ForumSubCategory"];
                    Trace.Warn("sub Cat Name = " + row["ForumSubCategory"].ToString());
                    dr["Description"] = row["FSC_Description"];
                    dr["LastThreadId"] = row["TopicId"];
                    dr["LastThread"] = row["Topic"];
                    dr["LastPostedBy"] = row["LastPostBy"];
                    dr["LastPostDate"] = row["LastPostDate"];
                    dr["LastPostedById"] = row["LastPostById"];
                    dr["Threads"] = row["Threads"];
                    dr["Posts"] = row["Posts"];
                    dr["Handle"] = row["Handlename"];
                    dr["Url"] = row["Url"];
                    Trace.Warn("Url : " + row["Url"].ToString());
                    dr["SubUrl"] = row["SubUrl"];
                    Trace.Warn("SubUrl = " + row["SubUrl"].ToString());
                    dt.Rows.Add(dr);
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            return dsnew;
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

        #region Get Last Post
        public string GetLastPost(string threadId, string threadTitle, string postedBy, string postDate, string postedById, string url)
        {
            PostsBusinessLogic postsBL = new PostsBusinessLogic();
            return postsBL.GetLastPost(threadId, threadTitle, postedBy, postDate, postedById, url);
        }
        #endregion

    } // class
} // namespace