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
using Bikewale.Community.PMS;

namespace Bikewale.Forums
{
    public class Threads : System.Web.UI.Page
    {
        protected HtmlGenericControl divForum, divStrip, divStripTop;
        protected Label lblMessage;
        protected Repeater rptForums, rptStickyForums;

        private int maxNoLinks = 15;	//number of links in the strip
        private int pageCount = 2;	//number of threads on each page
        protected int pageNo = 1;		//default page number is 1

        private int total = 0;

        public string forumId = "", inboxTotal = "";

        DataSet dsViews = new DataSet();

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

        protected override void OnInit(EventArgs e)
        {
            base.Load += new EventHandler(Page_Load);
        }
        private void Page_Load(object sender, EventArgs e)
        {
            // call the last login management function.
            ForumsCommon.ManageLastLogin();

            //also get the forumId
            if (Request["forum"] != null && Request.QueryString["forum"] != "")
            {
                forumId = Request.QueryString["forum"];

                //verify the id as passed in the url
                if (CommonOpn.CheckId(forumId) == false)
                {
                    //redirect to the default page
                    Response.Redirect("default.aspx");
                    return;
                }
            }
            else
            {
                //redirect to the default page
                Response.Redirect("default.aspx");
                return;
            }

            /*
                Code created By : Ashish Ambokar
                Created Date : 26/6/2012
                Note : Device detection code is now placed in separate clas DeviceDetection. This class has method DetectDevice which is used for device detection
           
            DeviceDetection dd = new DeviceDetection("/forums/viewforum-" + forumId + ".html");
            dd.DetectDevice();
             */
            /*	Code added by Ashish Ambokar ends here */

            if (Request["page"] != null && CommonOpn.CheckId(Request["page"]))
            {
                pageNo = Convert.ToInt32(Request["page"]);
            }

            Trace.Warn("Forum: " + forumId + " page: " + pageNo);

            CommonMessage cm = new CommonMessage();
            inboxTotal = cm.GetUnreadMessageCount();

            if (!IsPostBack)
            {
                FillRepeaters();
                FillStickyThreads();

                ForumName = GetForum();

                if (ForumName == "")
                {
                    Response.Redirect("default.aspx");
                }

                UserTracking ut = new UserTracking();
                ut.SaveActivity(CurrentUser.Id.ToString(), "2", forumId, "-1");
            }
        }
        void FillRepeaters()
        {
            CommonOpn op = new CommonOpn();
            string sql;
            Database db = new Database();
            SqlDataReader dr = null;

            try
            {
                //get the total number of questions
                sql = " SELECT Count(ID) as tot "
                    + " FROM Forums With(NoLock) "
                    + " WHERE IsActive = 1 AND ForumSubCategoryId = @ForumId ";

                SqlParameter[] param = { new SqlParameter("@ForumId", forumId) };

                dr = db.SelectQry(sql, param);

                if (dr.Read())
                {
                    total = Convert.ToInt32(dr["tot"]);
                }



                Trace.Warn("Total : " + total);
                //string sqlViews;

                if (pageNo == 1)
                {
                    //for the first page get the first pagecount number of results and display them

                    sql = " SELECT Top " + pageCount.ToString() + " F.ID AS TopicId, IsNull(Views,0) AS Reads, "
                        + " F.Topic, IsNull(C.Name, 'anonymous') AS CustomerName, F.StartDateTime, "
                        + " IsNull(F.Posts, 0) AS Replies, "
                        + " FT.MsgDateTime AS LastPostTime, IsNull(CP.Name, 'anonymous') AS LastPostBy, "
                        + " IsNull(C.Id, '0') StartedById, IsNull(CP.Id,'0') LastPostedById, "
                        + " ISNULL((SELECT HandleName FROM UserProfile UP With(NoLock) WHERE UP.UserId = C.id),'anonymous')  AS HandleName,"
                        + " ISNULL((SELECT HandleName FROM UserProfile UP With(NoLock) WHERE UP.UserId = CP.Id),'anonymous')  AS PostHandleName "
                        + " FROM (((Forums AS F With(NoLock) LEFT JOIN ForumThreads AS FT With(NoLock) ON FT.ID = F.LastPostId) "
                        + " LEFT JOIN Customers AS CP With(NoLock) ON CP.ID = FT.CustomerId) "
                        + " LEFT JOIN Customers C With(NoLock) ON C.ID = F.CustomerId ) "
                        + " WHERE F.ForumSubCategoryId = @ForumId AND "
                        + " F.IsActive = 1 ORDER BY FT.MsgDateTime Desc ";

                }
                else
                {
                    //for the other pages get the last id of the
                    //earlier page

                    sql = " Select MsgDateTime From (SELECT TOP 1 * FROM "
                        + " (SELECT TOP " + ((pageNo - 1) * pageCount) + " "
                        + " FT.MsgDateTime FROM (Forums AS F LEFT JOIN ForumThreads AS FT With(NoLock) "
                        + " ON FT.ID = F.LastPostId) WHERE F.ForumSubCategoryId = @ForumId "
                        + " Order By MsgDateTime Desc) AS lastdates Order By MsgDateTime Asc) "
                        + " AS LastMsgDate ";

                    Trace.Warn(sql);

                    string lastMsgDate = "";

                    dr = db.SelectQry(sql, param);

                    if (dr.Read())
                    {
                        lastMsgDate = dr["MsgDateTime"].ToString();
                    }

                    dr.Close();
                    db.CloseConnection();

                    sql = " SELECT Top " + pageCount.ToString() + " F.ID AS TopicId, IsNull(Views,0) AS Reads, "
                        + " F.Topic, IsNull(C.Name, 'anonymous') AS CustomerName, F.StartDateTime, "
                        + " IsNull(F.Posts, 0) AS Replies, "
                        + " FT.MsgDateTime AS LastPostTime, IsNull(CP.Name, 'anonymous') AS LastPostBy, "
                        + " IsNull(C.Id, '0') StartedById, IsNull(CP.Id,'0') LastPostedById, "
                        + " ISNULL((SELECT HandleName FROM UserProfile UP With(NoLock) WHERE UP.UserId = C.id),'anonymous')  AS HandleName,"
                        + " ISNULL((SELECT HandleName FROM UserProfile UP With(NoLock) WHERE UP.UserId = CP.Id),'anonymous')  AS PostHandleName "
                        + " FROM (((Forums AS F With(NoLock) LEFT JOIN ForumThreads AS FT With(NoLock) ON FT.ID = F.LastPostId) "
                        + " LEFT JOIN Customers AS CP With(NoLock) ON CP.ID = FT.CustomerId) "
                        + " LEFT JOIN Customers C With(NoLock) ON C.ID = F.CustomerId ) "
                        + " WHERE F.ForumSubCategoryId = @ForumId AND "
                        + " F.IsActive = 1 AND FT.MsgDateTime < '" + lastMsgDate + "'"
                        + " ORDER BY FT.MsgDateTime Desc ";

                }

                Trace.Warn("Showing now page no : " + pageNo);

                Trace.Warn("viewforums: " + sql);

                //ProcessViews(sqlViews);

                op.BindRepeaterReader(sql, rptForums, param);

                if (rptForums.Items.Count == 0)
                {
                    divForum.Visible = false;
                    //lblMessage.Text = "<p>No threads in this forum. <a href='CreateThreads.aspx?forum=" + forumId + "'>Be the first to create one.</a></p>";
                    lblMessage.Text = "<p>No threads in this forum.</p>";
                }
                else
                {
                    divForum.Visible = true;

                    // add pages strip.
                    AddStrip();
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

        private void LoadThreadViews(string _threadIds)
        {
            try
            {
                UserTracking ut = new UserTracking();
                dsViews = ut.LoadThreadViews(_threadIds);
            }
            catch (Exception ex)
            {
                dsViews = null;
                ErrorClass objErr = new ErrorClass(ex, "LoadThreadViews");
                objErr.SendMail();
            }
        }

        protected string GetThreadViews(string threadId)
        {
            if (dsViews != null)
            {
                DataRow[] dRow = dsViews.Tables[0].Select("ThreadId=" + threadId);
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

        private void ProcessViews(string _sql)
        {
            SqlDataReader dr = null;
            Database db = new Database();
            SqlParameter[] paramViews = { new SqlParameter("@ForumId", forumId) };
            string _threadIds = "";
            try
            {
                dr = db.SelectQry(_sql, paramViews);
                while (dr.Read())
                {
                    if (_threadIds == "")
                        _threadIds = dr["TopicId"].ToString();
                    else
                        _threadIds = _threadIds + "," + dr["TopicId"].ToString();
                }

                if (_threadIds != "")
                    LoadThreadViews(_threadIds);
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            finally
            {
                dr.Close();
                db.CloseConnection();
            }
        }


        void FillStickyThreads()
        {
            CommonOpn op = new CommonOpn();
            string sql;
            Database db = new Database();

            try
            {
                sql = " SELECT FST.ID as ID, FST.ThreadId as TopicId, FST.CatID, IsNull(Views,0) AS Reads, "
                        + " F.Topic, IsNull(C.Name, 'anonymous') AS CustomerName, F.StartDateTime, "
                        + " IsNull(F.Posts, 0) AS Replies, "
                        + " FT.MsgDateTime AS LastPostTime, IsNull(CP.Name, 'anonymous') AS LastPostBy, "
                        + " IsNull(C.Id, '0') StartedById, IsNull(CP.Id,'0') LastPostedById, "
                        + " ISNULL((SELECT HandleName FROM UserProfile UP With(NoLock) WHERE UP.UserId = C.id),'anonymous')  AS HandleName,"
                        + " ISNULL((SELECT HandleName FROM UserProfile UP With(NoLock) WHERE UP.UserId = CP.Id),'anonymous')  AS PostHandleName "
                        + " FROM Forum_StickyThreads FST , Forums AS F With(NoLock) LEFT JOIN ForumThreads AS FT With(NoLock) ON FT.ID = F.LastPostId "
                        + " LEFT JOIN Customers AS CP With(NoLock) ON CP.ID = FT.CustomerId "
                        + " LEFT JOIN Customers C With(NoLock) ON C.ID = F.CustomerId "
                        + " WHERE FST.ThreadId = F.ID AND F.IsActive = 1 AND FST.CatID = 2 "

                        + " UNION ALL "

                        + " SELECT FST.ID as ID, FST.ThreadId as TopicId, FST.CatID, IsNull(Views,0) AS Reads, "
                        + " F.Topic, IsNull(C.Name, 'anonymous') AS CustomerName, F.StartDateTime, "
                        + " IsNull(F.Posts, 0) AS Replies, "
                        + " FT.MsgDateTime AS LastPostTime, IsNull(CP.Name, 'anonymous') AS LastPostBy, "
                        + " IsNull(C.Id, '0') StartedById, IsNull(CP.Id,'0') LastPostedById, "
                        + " ISNULL((SELECT HandleName FROM UserProfile UP With(NoLock) WHERE UP.UserId = C.id),'anonymous')  AS HandleName,"
                        + " ISNULL((SELECT HandleName FROM UserProfile UP With(NoLock) WHERE UP.UserId = CP.Id),'anonymous')  AS PostHandleName "
                        + " FROM Forum_StickyThreads FST , Forums AS F With(NoLock) LEFT JOIN ForumThreads AS FT With(NoLock) ON FT.ID = F.LastPostId "
                        + " LEFT JOIN Customers AS CP With(NoLock) ON CP.ID = FT.CustomerId "
                        + " LEFT JOIN Customers C With(NoLock) ON C.ID = F.CustomerId  "
                        + " WHERE FST.ThreadId = F.ID AND F.ForumSubCategoryId = @ForumId AND "
                        + " FST.CatID = 1 AND F.IsActive = 1 ORDER BY FST.CatID DESC , FT.MsgDateTime Desc ";


                Trace.Warn("Sticky Threads: " + sql);

                SqlParameter[] param = { new SqlParameter("@ForumId", forumId) };

                op.BindRepeaterReader(sql, rptStickyForums, param);

                if (rptStickyForums.Items.Count == 0)
                {
                    rptStickyForums.Visible = false;
                }
                else
                {
                    rptStickyForums.Visible = true;
                }
            }
            catch (SqlException err)
            {
                Trace.Warn(err.Message);
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            } // catch Exception
        }

        public string GetForum()
        {
            string forum = "";
            Database db = new Database();
            SqlDataReader dr= null;

            string sql = "";

            sql = " SELECT Name, Description FROM ForumSubCategories With(NoLock) WHERE IsActive = 1 AND ID = @ForumId ";
            try
            {
                SqlParameter[] param = { new SqlParameter("@ForumId", forumId) };

                dr = db.SelectQry(sql, param);

                if (dr.Read())
                {
                    forum = dr["Name"].ToString();

                    ForumDescription = dr["Description"].ToString();
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

            return forum;
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

        public string GetDate(string value)
        {
            if (value != "")
                return Convert.ToDateTime(value).ToString("dd-MMM,yyyy");
            else
                return "N/A";
        }

        void AddStrip()
        {
            //make the strip, based on the total posts, the current page, and the page count
            pageCount = pageCount <= 0 ? 1 : pageCount;
            int pages = (int)Math.Ceiling(Convert.ToDouble(total) / Convert.ToDouble(pageCount));

            string pageUrl = "viewforum-" + forumId;

            //string pageUrl = "threads.aspx?forum=" + forumId;

            if (pages > 1)
            {
                divStrip.Visible = true;
                divStripTop.Visible = true;

                //get the slot number
                int slotNo = (int)Math.Ceiling(Convert.ToDouble(pageNo) / Convert.ToDouble(maxNoLinks));

                int startIndex = (slotNo - 1) * maxNoLinks + 1;
                int endIndex = (slotNo - 1) * maxNoLinks + maxNoLinks;
                endIndex = endIndex < pages ? endIndex : pages;

                Trace.Warn("slotNo : " + slotNo.ToString());
                Trace.Warn("startIndex : " + startIndex.ToString());
                Trace.Warn("endIndex : " + endIndex.ToString());

                divStrip.InnerHtml = "Pages : ";

                if (startIndex > maxNoLinks)
                    divStrip.InnerHtml += "<span class='ac'><a href='" + pageUrl + "-p"
                                        + (startIndex - 1).ToString() + ".html'>«</a></span>";
                    //divStrip.InnerHtml += "<span style='padding:3px'><a href='" + pageUrl + "&page="
                    //                    + (startIndex - 1).ToString() + "'>«</a></span>";

                for (int i = startIndex; i <= endIndex; i++)
                {
                    if (i != pageNo)
                        divStrip.InnerHtml += "<span class='ac'><a href='" + pageUrl + "-p" + i.ToString() + ".html'>"
                                            + i.ToString() + "</a></span>";

                        //divStrip.InnerHtml += "<span style='padding:3px'><a href='" + pageUrl + "&page=" + i.ToString() + "'>"
                        //                    + i.ToString() + "</a></span>";
                    else
                        divStrip.InnerHtml += "<span style='padding:3px'>" + i.ToString() + "</span>";
                }

                if (endIndex < pages)
                    divStrip.InnerHtml += "<span class='ac'><a href='" + pageUrl + "-p"
                                        + (endIndex + 1).ToString() + ".html'>»</a></span>";

                    //divStrip.InnerHtml += "<span style='padding:3px'><a href='" + pageUrl + "&page="
                    //                    + (endIndex + 1).ToString() + "'>»</a></span>";

                // now make an exact replica of footer strip.						
                divStripTop.InnerHtml = divStrip.InnerHtml;
                Trace.Warn(divStrip.InnerHtml);
            }
        }	
    }//class
}//namespace