using System;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Carwale.UI.Common;
using Carwale.UI.Controls;
using Carwale.Notifications;
using Carwale.DAL.CoreDAL;
using Carwale.BL.Forums;
using System.Data.Common;
using Carwale.DAL.CoreDAL.MySql;
using MySql.Data.MySqlClient;
using Carwale.DAL.Forums;
using Carwale.Utility;
using Carwale.Lucene.Forums;

namespace Carwale.UI.Forums
{
    public class Search : Page
    {
        #region Global Variables
        protected HtmlGenericControl divForum, divStrip, divStripTop;
        public string inboxTotal = "";
        protected string queryString = null;
        protected string sortBy = "relevant";
        protected Label lblMessage;
        protected Repeater rptForums, rptPosts;
        protected TextBox txtSearch;
        protected Button btnSearch;
        protected RadioButton optTopics, optAll;
        UserBusinessLogic userDetails = new UserBusinessLogic();
        ForumsDAL forumDetails = new ForumsDAL();
        private int resultsPerPage = 15; // results per page
        private int maxNoLinks = 15;	//number of links in the strip
        private int pageNo = 1;		//current page number. default 1
        private int total = 0;
        public string totalResults = "", userId = "";
        protected AddPageListToLink pagestrip;
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
            btnSearch.Click += new EventHandler(btnSearch_Click);
        }


        #endregion

        #region Page Load

        void Page_Load(object Sender, EventArgs e)
        {
            // call the last login management function.
            string loginCookie = HttpContext.Current.Request.Cookies["ForumLastLogin"] == null ? null : HttpContext.Current.Request.Cookies["ForumLastLogin"].ToString();
            userDetails.ManageLastLogin(CurrentUser.Id, loginCookie);
            UserDAL userdal = new UserDAL();          
            if (!IsPostBack)
            {
                pagestrip.Visible = false;
                divStrip.Visible = false;
                bool startSearch = false;
                if (Request["s"] != null)
                {
                    Carwale.UI.Common.UrlRewrite.Return404();
                    txtSearch.Text = Request["s"];
                    string searchQuery = txtSearch.Text.Trim();
                    queryString = searchQuery.Replace(":", "=").Replace("~", "");
                    startSearch = true;
                    SaveUserActivity saveAct = new SaveUserActivity();
                    saveAct.saveActivity(CurrentUser.Id, "9", "-1", "-1", CurrentUser.CWC);
                }
                if (Request["get"] != null)
                {
                    if (Request["get"].ToLower() == "new" && CurrentUser.Id != "-1")
                    {
                        txtSearch.Text = "get:new";
                        SaveUserActivity saveAct = new SaveUserActivity();
                        saveAct.saveActivity(CurrentUser.Id, "7", "-1", "-1", CurrentUser.CWC);
                    }
                    else
                    {
                        txtSearch.Text = "get:today";
                        SaveUserActivity saveAct = new SaveUserActivity();
                        saveAct.saveActivity(CurrentUser.Id, "8", "-1", "-1", CurrentUser.CWC);
                    }

                    startSearch = true;
                }
                if (Request["threadsBy"] != null)
                {
                    try
                    {
                        string userId = CarwaleSecurity.DecryptUserId(Convert.ToInt64(Request["threadsBy"]));
                        txtSearch.Text = "threadsBy:" + Request["threadsBy"];
                        startSearch = true;
                    }
                    catch (Exception err)
                    {
                        startSearch = false;
                        ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                        objErr.SendMail();
                    }
                }
                if (Request["postsBy"] != null)
                {
                    try
                    {
                        string userId = CarwaleSecurity.DecryptUserId(Convert.ToInt64(Request["postsBy"]));
                        txtSearch.Text = "postsBy:" + Request["postsBy"];
                        startSearch = true;
                    }
                    catch (Exception err)
                    {
                        startSearch = false;
                        ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                        objErr.SendMail();
                    }
                }
                if (Request["page"] != null && CommonOpn.CheckId(Request["page"]))
                {

                    pageNo = Convert.ToInt32(Request["page"]);
                    if (pageNo <= 0)
                    {
                        pageNo = 1;
                    }
                }
                //this stores the field by which the search result needs to be sorted.
                if (Request["sort"] != null)
                {
                    Trace.Warn("query string sort" + Request.QueryString["sort"].ToString());
                    sortBy = Request["sort"].ToString();

                }
                // if search criteria is set, start searching
                if (startSearch) SearchForums();
                if (totalResults == "")
                {
                    SaveUserActivity saveAct = new SaveUserActivity();
                    saveAct.saveActivity(CurrentUser.Id, "9", "-1", "-1", CurrentUser.CWC);
                }
            }
        } // Page_Load
        #endregion
  
        #region Search Button Click
        void btnSearch_Click(object sender, EventArgs e)
        {
            string searchTerm = txtSearch.Text.Trim();

            if (searchTerm.ToLower().IndexOf("postsby") >= 0 || searchTerm.ToLower().IndexOf("threadsby") >= 0)
                Response.Redirect("search.aspx?" + searchTerm.Replace(":", "=").Replace("~", ""));
            else if (searchTerm.ToLower().IndexOf("get:new") >= 0 || searchTerm.ToLower().IndexOf("get:today") >= 0)
                Response.Redirect("search.aspx?" + searchTerm.Replace(":", "=").Replace("~", ""));
            else
                Response.Redirect("search.aspx?s=" + searchTerm.Replace("~", "") + "&sort=relevant");
        }
        #endregion

        #region Search Forums
        void SearchForums()
        {
            string searchTerm = txtSearch.Text.Trim();
            string searchValue = "", displayText = "", searchType = "";
            if (searchTerm.ToLower().IndexOf("postsby:") >= 0)
            {
                userId = searchTerm.Substring(8, searchTerm.Length - 8);
                userId = CarwaleSecurity.DecryptUserId(Convert.ToInt64(userId));
                searchValue = userId;
                searchType = "PostsBy";
                displayText = "All posts by " + GetUserName(userId);
            }
            else if (searchTerm.ToLower().IndexOf("threadsby:") >= 0)
            {
                userId = searchTerm.Substring(10, searchTerm.Length - 10);
                userId = CarwaleSecurity.DecryptUserId(Convert.ToInt64(userId));
                searchValue = userId;
                searchType = "ThreadsBy";
                displayText = "All discussions started by " + GetUserName(userId);
            }
            else if (searchTerm.ToLower().Trim() == "get:new")
            {
                searchValue = Request.Cookies["ForumLastLogin"].Value;
                searchType = "ByDate";
                displayText = "All posts since last login";
            }
            else if (searchTerm.ToLower().Trim() == "get:today")
            {
                searchValue = DateTime.Today.AddDays(-1).ToString();
                searchType = "ByDate";
                displayText = "All posts in last 24 hours";
            }
            else
            {
                searchValue = searchTerm;
                searchTerm = "s=" + searchTerm;
                searchType = "Keyword";
                displayText = searchValue;
            }
            if (searchType == "ThreadsBy" || searchType == "PostsBy" || searchTerm.Length > 2)
            {
                LogAndProcessSearchData(searchValue, searchType, searchTerm);
            }
            else
            {
                lblMessage.Text = "Search string should be at least three (3) characters long.";
            }
        }
        #endregion

        #region Log and Process Search Data

        void LogAndProcessSearchData(string searchTermToSearch, string searchType, string searchTerm)
        {
            ForumSearchDAL searchForums = new ForumSearchDAL();
            DataTable dt = searchForums.SearchForums(searchTermToSearch, searchType, pageNo, resultsPerPage, sortBy, out total);
            if (total > 0)
            {
                lblMessage.Text = "<b>" + total + "</b> result(s) found.<b>";
                int pages = (int)Math.Ceiling(Convert.ToDouble(total) / Convert.ToDouble(resultsPerPage));
                if (pages > 1)
                {
                    pagestrip.Visible = true;
                    divStrip.Visible = true;
                }
                ShowResults(dt, searchType, searchTerm);
            }
            else
            {

                lblMessage.Text = "<b>No result(s) found.<b>";
            }

        }
        #endregion


        #region Show Results

        void ShowResults(DataTable dt, string searchType, string searchTerm)
        {
            CommonOpn op = new CommonOpn();
            if (searchType.ToLower() == "postsby") // if posts by user
            {
                rptPosts.DataSource = dt;
                rptPosts.DataBind();
                rptForums.Visible = false;
            }
            else
            {

                rptForums.DataSource = dt;
                rptForums.DataBind();
                rptPosts.Visible = false;
            }
            AddStrip(searchTerm); // add pages strip.
        }
        #endregion

        public string GetDate(string value)
        {
            if (value != "")
                return Convert.ToDateTime(value).ToString("dd-MMM,yyyy");
            else
                return "N/A";
        }

        void AddStrip(string searchTerm)
        {
            //make the strip, based on the total posts, the current page, and the page count
            resultsPerPage = resultsPerPage <= 0 ? 1 : resultsPerPage;
            string pageUrl = null;
            int pages = (int)Math.Ceiling(Convert.ToDouble(total) / Convert.ToDouble(resultsPerPage));
            if (sortBy == null)
                pageUrl = "Search.aspx?" + searchTerm.Replace(":", "=").Replace("~", "");
            else
                pageUrl = "Search.aspx?" + searchTerm.Replace(":", "=").Replace("~", "") + "&sort=" + sortBy;
            pagestrip.CurrentPageNumber = pageNo;
            pagestrip.PageSize = resultsPerPage;
            pagestrip.PagesPerSlot = maxNoLinks;
            pagestrip.TotalRecordCount = total;
            pagestrip.BaseUrl = pageUrl;
            pagestrip.GenerateStrip();
        }

        public string FormatMessage(string message)
        {
            string msg = "";

            if (message.IndexOf("[^^/quote^^]") > 0)
            {
                msg = message.Substring(message.LastIndexOf("[^^/quote^^]") + 12, message.Length - message.LastIndexOf("[^^/quote^^]") - 12);
            }
            else msg = message;

            if (msg.Length > 100) msg = msg.Substring(0, 100) + "...";

            if (msg.LastIndexOf('<') > 0) msg = msg.Substring(0, msg.LastIndexOf('<'));

            return msg;
        }

        string GetUserName(string userId)
        {
            string userName = "";
            try
            {          
                using(DbCommand cmd = DbFactory.GetDBCommand("GetCustomerNameById_v16_11_7"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_UserId", DbType.Int64, userId));
                    using(IDataReader dr = MySqlDatabase.SelectQuery(cmd,DbConnections.CarDataMySqlReadConnection))
                    {
                        if (dr.Read())
                        {
                            userName = dr["NAME"].ToString();
                        }
                    }
                }
            }
            catch (MySqlException err)
            {
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            } // catch Exception        
            return userName;
        }

        string GetHandleName(string userId)
        {
            string userName = "";      
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("GetCustomerDetailsByHandleName_v16_11_7"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_HandleName", DbType.String, 100, userId));
                    using(IDataReader dr = MySqlDatabase.SelectQuery(cmd,DbConnections.CarDataMySqlReadConnection))
                    {
                        if (dr.Read())
                        {
                            userName = dr["HandleName"].ToString();
                        }
                    }
                }
            }
            catch (MySqlException err)
            {
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            } // catch Exception      
            return userName;
        }

        #region Get Last Post


        public string GetLastPost(string title, string name, string date, string id, string posts, string startedById, bool openInNewWindow, string url)
        {
            PostsBusinessLogic postData = new PostsBusinessLogic();
            return (postData.GetLastPost(title, name, date, id, posts, startedById, openInNewWindow, url));
        }
        #endregion

        #region Get Last Post Thread

        public string GetLastPostThread(string name, string date, string lastPostById)
        {
            PostDAL postData = new PostDAL();
            return (postData.GetLastPostThread(name, date, lastPostById));
        }
        #endregion
    } // class
} // namespace