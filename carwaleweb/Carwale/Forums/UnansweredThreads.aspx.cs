using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Carwale.UI.Common;
using Carwale.Notifications;
using Carwale.BL.Forums;
using System.Data.Common;
using Carwale.DAL.CoreDAL.MySql;
using MySql.Data.MySqlClient;

namespace Carwale.UI.Forums
{
    public class UnansweredThreads : Page
    {
        #region Global Variables
        protected HtmlGenericControl divForum, divStrip, divStripTop;
        protected Repeater rptForums, rptPosts;
        private int resultsPerPage = 15; // results per page
        private int maxNoLinks = 15;	//number of links in the strip
        private int pageNo = 1;		//current page number. default 1
        private int total = 0;
        public string totalResults = "", userId = "";

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
        }
        #endregion

        #region Page Load
        void Page_Load(object Sender, EventArgs e)
        {
            if (Request["page"] != null && CommonOpn.CheckId(Request["page"]))
            {
                pageNo = Convert.ToInt32(Request["page"]);
            }
            LoadUnansweredForums();

            if (!Page.IsPostBack)
            {
                SaveUserActivity saveAct = new SaveUserActivity();
                saveAct.saveActivity(CurrentUser.Id, "10", "-1", "-1", CurrentUser.CWC);
            }
        }
        #endregion

        #region Laod Unanswered Forums
        private void LoadUnansweredForums()
        {
            DataSet ds = new DataSet();
            CommonOpn op = new CommonOpn();
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("GetUnAnsweredForumCount_v16_11_7"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, DbConnections.ForumsMySqlReadConnection)) 
                    {
                        if (dr.Read())
                            total = Convert.ToInt32(dr["tot"]);
                    }
                }
                string forum_id = "";

                if (pageNo != 1)
                {
                    int count = ((pageNo - 1) * resultsPerPage);              
                    using (DbCommand cmd = DbFactory.GetDBCommand("GetForumId_v16_11_7"))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DbFactory.GetDbParam("v_Count", DbType.Int32, count));
                        using(IDataReader dr = MySqlDatabase.SelectQuery(cmd,DbConnections.ForumsMySqlReadConnection))
                        {
                            if (dr.Read())
                            {
                                forum_id = dr["Id"].ToString();
                            }
                        }
                    }              
                }
                
                using(DbCommand cmd = DbFactory.GetDBCommand("GetUnAnsweredForumDetails_v16_11_7"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_ForumId", DbType.Int64,!string.IsNullOrEmpty(forum_id) ? forum_id : Convert.DBNull ));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_Count", DbType.Int32, resultsPerPage));
                    ds = MySqlDatabase.SelectAdapterQuery(cmd, DbConnections.ForumsMySqlReadConnection);
                }
                op.BindRepeaterReaderDataSet(ds, rptForums);
            }
            catch (MySqlException err)
            {
                Trace.Warn(err.Message);
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }

            AddStrip();
        }
        #endregion

        #region Add Strip
        void AddStrip()
        {
            //make the strip, based on the total posts, the current page, and the page count
            resultsPerPage = resultsPerPage <= 0 ? 1 : resultsPerPage;
            int pages = (int)Math.Ceiling(Convert.ToDouble(total) / Convert.ToDouble(resultsPerPage));

            //string pageUrl = "Search.aspx?" + searchTerm.Replace(":","=").Replace("~","");
            string pageUrl = "UnansweredThreads.aspx?a=1";

            if (pages > 1)
            {
                divStrip.Visible = true;
                divStripTop.Visible = true;

                //get the slot number
                int slotNo = (int)Math.Ceiling(Convert.ToDouble(pageNo) / Convert.ToDouble(maxNoLinks));

                int startIndex = (slotNo - 1) * maxNoLinks + 1;
                int endIndex = (slotNo - 1) * maxNoLinks + maxNoLinks;
                endIndex = endIndex < pages ? endIndex : pages;
                divStrip.InnerHtml = "Pages : ";

                if (startIndex > maxNoLinks)
                    divStrip.InnerHtml += "<span class='ac'><a href='" + pageUrl + "&page="
                                        + (startIndex - 1).ToString() + "'>«</a></span>";

                for (int i = startIndex; i <= endIndex; i++)
                {
                    if (i != pageNo)
                        divStrip.InnerHtml += "<span class='ac'><a href='" + pageUrl + "&page=" + i.ToString() + "'>"
                                            + i.ToString() + "</a></span>";
                    else
                        divStrip.InnerHtml += "<span class='iac'>" + i.ToString() + "</span>";
                }

                if (endIndex < pages)
                    divStrip.InnerHtml += "<span class='ac'><a href='" + pageUrl + "&page="
                                        + (endIndex + 1).ToString() + "'>»</a></span>";

                // now make an exact replica of footer strip.						
                divStripTop.InnerHtml = divStrip.InnerHtml;
                Trace.Warn(divStrip.InnerHtml);
            }
        }
        #endregion

        #region Get Last Post


        public string GetLastPost(string title, string name, string date, string id, string posts, string startedById, bool openInNewWindow, string url)
        {
            PostsBusinessLogic postData = new PostsBusinessLogic();
            return (postData.GetLastPost(title, name, date, id, posts, startedById, openInNewWindow, url));
        }
        #endregion
    }
}