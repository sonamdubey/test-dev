using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.HtmlControls;
using Bikewale.Common;
using Bikewale.Forums.Common;
using Bikewale.Community.PMS;
///
/// This page is for default page for forums
/// Inherit from carwale forum default page .done some changes according to the bikeWale.
///
namespace Bikewale.Forums
{
    public class Default : System.Web.UI.Page
    {
        protected HtmlGenericControl spnError;
        protected Repeater rptParent, rptTopContributors, rptCurrentTopContributors;
        public DataSet dsForums = new DataSet();
        public int serial = 0;
        public string discussions = "", posts = "", contributors = "", inboxTotal = "";
        public string total = "", guests = "", members = "", mostUsers = "", mostUsersDate = "";

        DataSet dsViews = new DataSet();
        protected Repeater rptMembers;

        protected override void OnInit(EventArgs e)
        {
            base.Load += new EventHandler(Page_Load);
        }
        private void Page_Load(object sender, EventArgs e)
        {

            // call the last login management function.
            //ForumsCommon.ManageLastLogin();

            //CommonMessage cm = new CommonMessage();
            //inboxTotal = cm.GetUnreadMessageCount();

            if (!IsPostBack)
            {
                /*
                    Code created By : Ashish Ambokar
                    Created Date : 26/6/2012
                    Note : Device detection code is now placed in separate clas DeviceDetection. This class has method DetectDevice which is used for device detection
                */

                // for time being its not required as per asish amboker (26/7/2012)
                //DeviceDetection dd = new DeviceDetection(Request.ServerVariables["HTTP_X_REWRITE_URL"].ToString());
                //dd.DetectDevice();
                /*	Code added by Ashish Ambokar ends here */

                LoadCategoryViews();                
                FillRepeaters();              
                FillContributors();             
                GetForumStats();             
                UserTracking ut = new UserTracking();
                ut.SaveActivity(CurrentUser.Id.ToString(), "1", "-1", "-1");               
                GetUsersCount();             
                LoadActiveMembers();              
            }	
        }

        private void LoadActiveMembers()
        {
            string sql = " SELECT DISTINCT UP.HandleName "
                       + " FROM ForumUserTracking FUT, UserProfile UP With(NoLock) "
                       + " WHERE "
                       + " FUT.UserID = UP.UserId"
                       + " AND DATEDIFF(MINUTE, FUT.ActivityDateTime, getdate()) < 60 AND FUT.UserId <> -1  ";

            CommonOpn op = new CommonOpn();
            try
            {
                op.BindRepeaterReader(sql, rptMembers);
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "LoadCurrentlyActiveUsers");
                objErr.SendMail();
            }
        }

        private void GetUsersCount()
        {
            UserTracking ut = new UserTracking();
            string usersCount = ut.GetUsersCount();
            if (usersCount != "")
            {
                string[] splittedUsersCount = usersCount.Split('|');
                total = splittedUsersCount[0];
                guests = splittedUsersCount[1];
                members = splittedUsersCount[2];
                mostUsers = splittedUsersCount[3];
                mostUsersDate = splittedUsersCount[4];
            }
        }

        private void LoadCategoryViews()
        {
            try
            {
                UserTracking ut = new UserTracking();
                dsViews = ut.LoadCategoryViews();
            }
            catch (Exception ex)
            {
                dsViews = null;
                ErrorClass objErr = new ErrorClass(ex, "LoadCategoryViews");
                objErr.SendMail();
            }
        }

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

        void FillRepeaters()
        {
            CommonOpn op = new CommonOpn();
            string sql;
            Database db = new Database();

            try
            {  
                sql = " SELECT FC.ID, FC.ForumCategoryId, FC.Name, FC.Description, F.ID AS TopicId, F.Topic, "
                    + " IsNull(C.Name, 'anonymous') AS CustomerName, F.StartDateTime, IsNull(CT.Name, 'anonymous') LastPostBy, "
                    + " IsNull(CT.Id, '0') LastPostById, FT.MsgDateTime AS LastPostDate, FC.Threads AS Threads, "
                    + " FC.Posts AS Posts ,ISNULL((SELECT HandleName FROM UserProfile UP WHERE UP.UserId = CT.Id),'anonymous')  AS HandleName"
                    + " FROM ((((ForumSubCategories AS FC With(NoLock) LEFT JOIN ForumThreads AS FT With(NoLock) ON FT.ID = FC.LastPostId) "
                    + " LEFT JOIN Forums AS F With(NoLock) ON F.ID = FT.ForumId) LEFT JOIN Customers AS C With(NoLock) ON C.ID = F.CustomerId) "
                    + " LEFt JOIN Customers AS CT With(NoLock) ON CT.ID = FT.CustomerId) WHERE FC.IsActive = 1 ORDER BY FC.Name ";

                dsForums = db.SelectAdaptQry(sql);

                Trace.Warn("SQl Forums: " + sql);

                sql = " SELECT ID, Name, Description FROM ForumCategories With(NoLock) WHERE IsActive = 1 ORDER BY Name ";
                op.BindRepeaterReader(sql, rptParent);


            }
            catch (SqlException err)
            {
                Trace.Warn(err.Message);
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            } // catch Exception
        }


        void FillContributors()
        {
            CommonOpn op = new CommonOpn();
            string sql;
            Database db = new Database();

            try
            {              

                sql = " SELECT TOP 5 Posts, CustomerId, ISNULL(U.HandleName,'anonymous') AS Name "
                    + " FROM AP_ForumTopPosts AF With(NoLock) LEFT JOIN UserProfile U With(NoLock) ON U.UserId = AF.CustomerId"
                    + " WHERE PostType = 1 ORDER BY Posts DESC";

                op.BindRepeaterReader(sql, rptTopContributors);

                sql = " SELECT TOP 5 Posts, CustomerId, ISNULL(U.HandleName,'anonymous') AS Name FROM AP_ForumTopPosts AP With(NoLock) "
                    + " LEFT JOIN UserProfile U With(NoLock) ON U.UserId = AP.CustomerId	"
                    + " WHERE PostType = 2 ORDER BY Posts DESC";

                Trace.Warn(sql);

                op.BindRepeaterReader(sql, rptCurrentTopContributors);
            }
            catch (SqlException err)
            {
                Trace.Warn(err.Message);
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            } // catch Exception
        }

        public DataSet GetChild(string id)
        {
            DataSet ds = new DataSet();
            DataTable dt = ds.Tables.Add();
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

            DataRow[] rows = dsForums.Tables[0].Select(" ForumCategoryId = " + id);

            foreach (DataRow row in rows)
            {
                dr = dt.NewRow();

                dr["SubCatId"] = row["ID"];
                dr["SubCatName"] = row["Name"];
                dr["Description"] = row["Description"];
                dr["LastThreadId"] = row["TopicId"];
                dr["LastThread"] = row["Topic"];
                dr["LastPostedBy"] = row["LastPostBy"];
                dr["LastPostDate"] = row["LastPostDate"];
                dr["LastPostedById"] = row["LastPostById"];
                dr["Threads"] = row["Threads"];
                dr["Posts"] = row["Posts"];
                dr["Handle"] = row["handlename"];

                dt.Rows.Add(dr);
            }

            return ds;
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

        public string GetLastPost(string threadId, string threadTitle, string postedBy, string postDate, string postedById)
        {
            string lastPost = "";

            if (postDate.Length > 0)
            {
                lastPost = "<a href='viewthread-" + threadId + ".html'>" + threadTitle + "</a>";

                //lastPost += "<br>by <a title=\"View " + postedBy + "'s complete profile\" class='startBy' href='/Users/Profile-" + CarwaleSecurity.EncryptUserId( long.Parse( postedById ) ) + ".html'>" + postedBy + "</a>";
                if (postedBy != "anonymous")
                    lastPost += "<br>by <a target='_blank' title=\"View " + postedBy + "'s complete profile\" class='startBy' href='/community/members/" + postedBy + ".html'>" + postedBy + "</a>";
                else
                    lastPost += "<br>by <span class='startBy'>" + postedBy + "</span>";

                lastPost += ", <span class='startBy'>" + Convert.ToDateTime(postDate).ToString("dd-MMM-yy hh:mm tt") + "</span>";
            }

            return lastPost;
        }

        void GetForumStats()
        {
            string sql;
            Database db = new Database();
            SqlDataReader dr = null;

            try
            {
                //get the list of subcategories
                sql = " SELECT COUNT(F.Id) AS Discussions, "
                    + " (SELECT COUNT(Id) FROM Customers With(NoLock) WHERE Id IN (SELECT CustomerId FROM ForumThreads) ) AS Contributors, "
                    + " (SELECT COUNT(Id) FROM ForumThreads With(NoLock) ) AS Posts "
                    + " FROM Forums F With(NoLock) ";

                Trace.Warn(sql);

                dr = db.SelectQry(sql);

                if (dr.Read())
                {
                    discussions = CommonOpn.FormatNumeric(dr["Discussions"].ToString());
                    posts = CommonOpn.FormatNumeric(dr["Posts"].ToString());
                    contributors = CommonOpn.FormatNumeric(dr["Contributors"].ToString());
                }
            }
            catch (Exception err)
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