using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Bikewale.Common;
using Bikewale.Ajax;
using Bikewale.Community.PMS;
using Bikewale.Controls;
using Bikewale.Forums.Common;

namespace Bikewale.Forums
{
    public class ViewThreads : System.Web.UI.Page
    {
        protected HtmlGenericControl divThread, divStrip, divStripTop, divNoReplyMessage, divReplyLink, divQuickReply;
        protected Label lblMessage, lblBrk;
        protected Repeater rptThreads;
        protected LinkButton lnkDeletePost, lnkDeleteThread, lnkCloseThread, lnkMergePost;
        protected Panel pnlModeratorTools;
        protected RichTextEditor rteQR;
        protected Button butSave;
        protected CheckBox chkEmailAlert;
        protected HyperLink hyplnk, hyplnkRemoveSticky, hyplnkCreateSticky;
        protected string postedByUserIds = "", concatenatedPostIds = "";
        public string threadId = "", customerId = "", inboxTotal = "";
        public int serial = 0;
        public bool modLogin = false, flagLogin = false;

        private int maxNoLinks = 25;	// number of links in the strip
        private int postsPerPage = 10;	// number of posts on each page
        protected int pageNo = 1;		// default page number is 1

        protected int total = 0;

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

        public string StartedBy
        {
            get
            {
                if (ViewState["StartedBy"] != null)
                    return ViewState["StartedBy"].ToString();
                else
                    return "";
            }
            set { ViewState["StartedBy"] = value; }
        }

        public string StartedOn
        {
            get
            {
                if (ViewState["StartedOn"] != null && ViewState["StartedOn"].ToString() != "")
                    return Convert.ToDateTime(ViewState["StartedOn"]).ToString("dd-MMM,yyyy HH:mm");
                else
                    return "";
            }
            set { ViewState["StartedOn"] = value; }
        }
        protected override void OnInit(EventArgs e)
        {
            base.Load += new EventHandler(Page_Load);
            lnkDeletePost.Click += new EventHandler(DeletePost);
            lnkDeleteThread.Click += new EventHandler(DeleteThread);
            lnkCloseThread.Click += new EventHandler(CloseThread);
            lnkMergePost.Click += new EventHandler(MergePost);
            butSave.Click += new EventHandler(butSave_Click);
        }
        private void Page_Load(object sender, EventArgs e)
        {
            // call the last login management function.
            ForumsCommon.ManageLastLogin();

            customerId = CurrentUser.Id;

            if (customerId != "-1") // show quick reply box if user is logged in.
                divQuickReply.Visible = true;

            if (customerId != "-1")
                flagLogin = true;
            else
                flagLogin = false;

            //check if moderator is logged-in?
            modLogin = GetModeratorLoginStatus(customerId);

            if (modLogin == true)
                pnlModeratorTools.Visible = true;

            //also get the threadId
            if (Request["thread"] != null && Request.QueryString["thread"] != "")
            {
                threadId = Request.QueryString["thread"];

                //verify the id as passed in the url
                if (CommonOpn.CheckId(threadId) == false)
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

            if (Request["page"] != null && CommonOpn.CheckId(Request["page"]))
            {
                pageNo = Convert.ToInt32(Request["page"]);
            }

            if (Request["post"] != null && CommonOpn.CheckId(Request["post"]))
            {
                int post = Convert.ToInt32(Request["post"]);

                // find out post no and redirect user accordingly.
                FindPost(post);
            }

            //Check the handle. if it does not exist then redirect the user to change the handle page
            if (ForumsCommon.CheckUserHandle(CurrentUser.Id) != false)
            {
                divQuickReply.Visible = false;
            }

           // Utility.RegisterTypeForAjax(typeof(AjaxForum));

            CommonMessage cm = new CommonMessage();
            inboxTotal = cm.GetUnreadMessageCount();

            if (!IsPostBack)
            {
                Trace.Warn("Loading page for the first time..");
                if (GetForum() == false)
                {
                    Response.Redirect("default.aspx");
                    return;
                }

                if (customerId != "")
                {

                    bool Stickyflag = chkStickyThreads(threadId, customerId);

                    if (Stickyflag == true)
                    {
                        hyplnkRemoveSticky.Visible = true;
                        hyplnkRemoveSticky.Text = "Remove Sticky";
                        hyplnkCreateSticky.Visible = false;
                    }
                    else
                    {
                        hyplnkCreateSticky.Visible = true;
                        hyplnkCreateSticky.Text = "Create Sticky";
                        hyplnkRemoveSticky.Visible = false;

                    }


                    AjaxForum aj = new AjaxForum();

                    bool flag = aj.GetSubscribeLink(threadId, customerId);

                    //if (flag == true)
                    //{
                    //    hyplnk.Visible = false;
                    //    lblBrk.Visible = false;

                    //}
                    //else
                    //{
                    //    hyplnk.Visible = true;
                    //    hyplnk.Text = "<B>Subscribe to this thread</B>";
                    //    lblBrk.Visible = true;
                    //}
                }
                //else
                //{
                //    //hyplnk.Visible = false;
                //    //lblBrk.Visible = false;
                //}


                FillRepeaters();

                UserTracking ut = new UserTracking();
                ut.SaveActivity(CurrentUser.Id.ToString(), "4", ForumId, threadId);
            }
            else
            {
                Trace.Warn("Loading page after postback..");
            }

            if (Request.Cookies["Forum_MultiQuotes"] == null)
            {
                HttpCookie objCookie;
                objCookie = new HttpCookie("Forum_MultiQuotes");
                objCookie.Value = "";
                objCookie.HttpOnly = false;
                HttpContext.Current.Response.Cookies.Add(objCookie);
            }
        }

        protected bool chkStickyThreads(string threadId, string customerId)
        {
            string sql = "";
            Database db = new Database();
            SqlDataReader dr = null;
            bool retVal = false;

            try
            {
                sql = " SELECT ID FROM Forum_StickyThreads With(NoLock) WHERE CreatedBy = @CreatedBy AND ThreadId = @ThreadId ";

                Trace.Warn("sql cust : " + sql);

                SqlParameter[] param = 
					{
						new SqlParameter("@CreatedBy", customerId),
						new SqlParameter("@ThreadId ", threadId)
					};

                dr = db.SelectQry(sql, param);
                if (dr.HasRows == true)
                {
                    retVal = true;
                }
            }
            catch (SqlException err)
            {
                Trace.Warn(err.Message);
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            } // catch Exception
            catch (Exception ex)
            {
                Trace.Warn(ex.Message);
                ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            finally
            {
                if(dr != null)
                    dr.Close();

                db.CloseConnection();
            }
            return retVal;
        }

        void DeletePost(object sender, EventArgs e)
        {
            if (Request.Form["__EVENTARGUMENT"] != null && Request.Form["__EVENTARGUMENT"].ToString() != "")
            {
                Trace.Warn("__EVENTARGUMENT : " + Request.Form["__EVENTARGUMENT"].ToString());

                Database db = new Database();

                string sql = "";

                sql = " UPDATE ForumThreads SET IsActive=0 WHERE ID = @ThreadId ";

                try
                {
                    SqlParameter[] param = { new SqlParameter("@ThreadId", Request.Form["__EVENTARGUMENT"].ToString()) };

                    db.UpdateQry(sql, param);

                    FillRepeaters();
                }
                catch (SqlException err)
                {
                    Trace.Warn(err.Message);
                    ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                    objErr.SendMail();
                } // catch Exception
            }
        }

        void MergePost(object sender, EventArgs e)
        {
            try
            {
                Label lblId;
                string strMergeId, strMessage, segments = "";
                for (int i = 0; i < rptThreads.Items.Count; i++)
                {
                    HtmlInputCheckBox chkIds = (HtmlInputCheckBox)rptThreads.Items[i].FindControl("chkItem");
                    lblId = (Label)rptThreads.Items[i].FindControl("lblId");
                    if (chkIds.Checked)
                    {
                        segments += lblId.Text + ","; //All the messages r appended
                    }
                }

                strMergeId = segments.Split(',')[0].Trim();
                Trace.Warn("FirstMerge ID : " + strMergeId);

                segments = segments.Substring(0, segments.LastIndexOf(',')).ToString();
                Trace.Warn("All Merge IDs : " + segments);

                if (CheckCustomerId(segments) == true)
                {
                    strMessage = AppendMessagesForMerge(segments);
                    Trace.Warn(strMessage);

                    // Update FirstId with the appended messages
                    string sql = "", sql1 = "";
                    Database db = new Database();

                    sql = " UPDATE ForumThreads SET Message = @Message "
                        + " WHERE ID = @MergeId ";

                    Trace.Warn(sql);

                    SqlParameter[] param = 
						{
							new SqlParameter("@Message", strMessage.Trim()),
							new SqlParameter("@MergeId", strMergeId)
						};

                    db.UpdateQry(sql, param);

                    Trace.Warn("Updated....... ");

                    ArrayList arrid = new ArrayList();
                    arrid.AddRange(segments.Split(new char[] { ',' }));
                    for (int h = 1; h <= arrid.Count - 1; h++)
                    {
                        string Id = arrid[h].ToString();
                        Trace.Warn("In array : " + Id);
                        sql1 = " UPDATE ForumThreads SET IsActive=0 WHERE ID = @ThreadId ";

                        SqlParameter[] param1 = { new SqlParameter("@ThreadId", Id) };

                        db.UpdateQry(sql1, param1);
                    }

                    UpdateStats(threadId, ForumId);
                    FillRepeaters();

                }
                else
                {
                    lblMessage.Text = "Cannot Merge Posts of different Customers";
                }
            }
            catch (SqlException err)
            {
                Trace.Warn(err.Message);
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            } // catch Exception
            catch (Exception err)
            {
                Trace.Warn(err.Message);
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            } // catch Exception	
        }

        protected bool CheckCustomerId(string ids)
        {
            string sql = "";
            Database db = new Database();
            DataSet dsCustomerId = new DataSet();

            bool retVal = false;

            try
            {
                SqlParameter[] param1 = null;

                sql = " SELECT DISTINCT CustomerId FROM ForumThreads With(NoLock) WHERE ID IN (" + db.GetInClauseValue(ids, "ThreadId", out param1) + ")";
                Trace.Warn("sql cust : " + sql);
                dsCustomerId = db.SelectAdaptQry(sql, param1);

                // To check post selected are of same customerId
                if (dsCustomerId.Tables[0].Rows.Count == 1)
                    retVal = true;

            }
            catch (SqlException err)
            {
                Trace.Warn(err.Message);
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            } // catch Exception
            catch (Exception ex)
            {
                Trace.Warn(ex.Message);
                ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            return retVal;
        }

        protected string AppendMessagesForMerge(string ids)
        {
            string strId = "", strMessage = "", sql = "";
            Database db = new Database();
            SqlDataReader dr = null;

            ArrayList arrid = new ArrayList();
            try
            {
                arrid.AddRange(ids.Split(new char[] { ',' }));

                for (int h = 0; h <= arrid.Count - 1; h++)
                {
                    strId = arrid[h].ToString();

                    sql = " SELECT Message FROM ForumThreads With(NoLock) WHERE ID = @ThreadId ";

                    SqlParameter[] param = { new SqlParameter("@ThreadId", strId) };

                    dr = db.SelectQry(sql, param);
                    if (dr.Read())
                    {
                        strMessage = strMessage + "</BR>" + dr["Message"].ToString();
                    }
                }
            }
            catch (SqlException err)
            {
                Trace.Warn(err.Message);
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            } // catch Exception
            catch (Exception ex)
            {
                Trace.Warn(ex.Message);
                ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            finally
            {
                if(dr != null)
                    dr.Close();

                db.CloseConnection();
            }

            return strMessage;
        }

        protected bool UpdateStats(string ForumId, string SubCategoryId)
        {
            bool returnVal = false;

            SqlConnection con;
            SqlCommand cmd;
            SqlParameter prm;
            Database db = new Database();
            CommonOpn op = new CommonOpn();

            string conStr = db.GetConString();
            con = new SqlConnection(conStr);

            try
            {
                cmd = new SqlCommand("Forum_UpdateStats", con);
                cmd.CommandType = CommandType.StoredProcedure;

                prm = cmd.Parameters.Add("@ForumId", SqlDbType.BigInt);
                prm.Value = ForumId;

                prm = cmd.Parameters.Add("@SubCategoryId", SqlDbType.BigInt);
                prm.Value = SubCategoryId;

                con.Open();
                //run the command
                cmd.ExecuteNonQuery();

                returnVal = true;
            }
            catch (SqlException err)
            {
                Trace.Warn(err.Message);
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            } // catch Exception
            catch (Exception ex)
            {
                Trace.Warn(ex.Message);
                ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            finally
            {
                //close the connection	
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
            return returnVal;
        }

        void DeleteThread(object sender, EventArgs e)
        {
            Database db = new Database();

            string sql = "";

            try
            {
                sql = " UPDATE Forums SET IsActive=0 WHERE ID = @ThreadId ";

                SqlParameter[] param = { new SqlParameter("@ThreadId", threadId) };

                db.UpdateQry(sql, param);
            }
            catch (SqlException err)
            {
                Trace.Warn(err.Message);
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            } // catch Exception
            finally
            {
                Response.Redirect("default.aspx");
            }
        }

        void CloseThread(object sender, EventArgs e)
        {
            Database db = new Database();

            string sql = "";

            try
            {
                sql = " UPDATE Forums SET ReplyStatus=0 WHERE ID = @ThreadId ";

                SqlParameter[] param = { new SqlParameter("@ThreadId", threadId) };

                db.UpdateQry(sql, param);

                GetForum();
            }
            catch (SqlException err)
            {
                Trace.Warn(err.Message);
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            } // catch Exception
        }

        public bool GetForum()
        {
            bool retVal = false;
            Database db = new Database();
            SqlDataReader dr = null;
            bool replyStatus = true;

            string sql = "";

            try
            {
                //update the viewd count by 1
                sql = " UPDATE Forums SET Views = IsNull(Views, 0) + 1 WHERE ID = @ThreadId ";
                SqlParameter[] param = { new SqlParameter("@ThreadId", threadId) };
                db.UpdateQry(sql, param);

                // get forum details	
                sql = " SELECT FC.ID, FC.Name, FC.Description, F.Topic, F.ReplyStatus, "
                    + " F.StartDateTime AS StartedOn FROM ForumSubCategories AS FC, "
                    + " Forums AS F With(NoLock) WHERE F.ID = @ThreadId AND F.IsActive = 1 AND "
                    + " FC.ID = F.ForumSubCategoryId AND FC.IsActive = 1 ";

                dr = db.SelectQry(sql, param);

                if (dr.Read())
                {
                    ForumId = dr["ID"].ToString();
                    ForumName = dr["Name"].ToString();
                    ForumDescription = dr["Description"].ToString();
                    ThreadName = dr["Topic"].ToString();
                    //StartedBy 		= dr["StartedBy"].ToString();
                    StartedOn = dr["StartedOn"].ToString();

                    replyStatus = Convert.ToBoolean(dr["ReplyStatus"].ToString());

                    retVal = true;
                }

                if (!replyStatus)
                {
                    divNoReplyMessage.InnerHtml = "<b>This discussion is closed for new replies.</b>";
                    divReplyLink.Visible = false;
                    if (!modLogin) divQuickReply.Visible = false; // hide quick reply box as well.
                }
            }
            catch (SqlException err)
            {
                Trace.Warn(err.Message);
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();

                retVal = false;
            } // catch Exception
            finally
            {
                if(dr != null)
                    dr.Close();

                db.CloseConnection();
            }
    
            return retVal;
        }

        void FillRepeaters()
        {
            CommonOpn op = new CommonOpn();
            string sql;
            Database db = new Database();
            SqlDataReader dr;

            try
            {
                //get the total number of posts
                sql = " SELECT Count(ID) as tot "
                    + " FROM ForumThreads With(NoLock) "
                    + " WHERE IsActive = 1 AND ForumId = @ThreadId ";

                SqlParameter[] param = { new SqlParameter("@ThreadId", threadId) };

                dr = db.SelectQry(sql, param);

                if (dr.Read())
                {
                    total = Convert.ToInt32(dr["tot"]);
                }

                dr.Close();
                db.CloseConnection();

                Trace.Warn("Total : " + total);

                if (pageNo == 1)
                {
                    //get the list of posts
                    sql = " SELECT TOP " + postsPerPage.ToString() + " FT.ID, IsNull(C.Name,'anonymous') AS PostedBy,"
                        + " IsNull(C.Id, 0) AS PostedById, Ci.Name AS City, FT.Message, FT.MsgDateTime, FT.LastUpdatedTime,"
                        + " IsNull( C1.Name,'N/A') AS UpdatedBy, "
                        + " IsNull((SELECT AvtarPhoto FROM UserProfile With(NoLock) WHERE IsAvtarApproved=1 AND UserId=FT.CustomerId), '') Avtar, "
                        + " IsNull((SELECT Signature FROM UserProfile With(NoLock) WHERE UserId=FT.CustomerId), '') Signature, "
                        + " (SELECT COUNT(FT1.ID) FROM ForumThreads FT1, Forums F With(NoLock) WHERE F.Id=FT1.ForumId "
                        + " AND FT1.CustomerId=FT.CustomerId AND FT1.IsActive=1 AND F.IsActive=1) Posts, "
                        + " (SELECT Role FROM BikeWaleRoles FR, ForumCustomerRoles FCR With(NoLock) WHERE FCR.RoleId=FR.ID AND CustomerId=FT.CustomerId) Role, "
                        + " (SELECT CustomerId FROM BikeWaleGuys With(NoLock) WHERE CustomerId=FT.CustomerId) BikewaleGuy, "
                        + " IsNull(FB.CustomerId, -1) AS BannedCust, "
                        + " ISNULL((SELECT HandleName FROM UserProfile UP With(NoLock) WHERE UP.UserId = FT.CustomerId),'anonymous') AS HandleName,"
                        + " ISNULL((SELECT HandleName FROM UserProfile U With(NoLock) WHERE U.UserId = C1.id),'anonymous')  AS LastUpdatedHandle "
                        + " ,ISNULL((SELECT ThanksReceived FROM UserProfile UP1 With(NoLock) WHERE UP1.UserId = FT.CustomerId),0) AS ThanksReceived"
                        + " ,ISNULL((SELECT CONVERT(VARCHAR,JoiningDate,106) FROM UserProfile UP2 With(NoLock) WHERE UP2.UserId = FT.CustomerId),0) AS JoiningDate"
                        + " FROM (((( ForumThreads AS FT With(NoLock) LEFT JOIN Customers AS C With(NoLock) ON C.ID=FT.CustomerId )"
                        + " LEFT JOIN Cities Ci With(NoLock) ON C.CityId=Ci.Id ) "
                        + " LEFT JOIN Customers AS C1 With(NoLock) ON C1.ID=FT.UpdatedBy ) "
                        + " LEFT JOIN Forum_BannedList AS FB With(NoLock) ON FB.CustomerId = C.ID) "
                        + " WHERE FT.ForumId= @ThreadId AND "
                        + " FT.IsActive = 1 ORDER BY FT.MsgDateTime ";
                }
                else
                {
                    serial = (pageNo - 1) * postsPerPage;
                    //for the other pages get the last id of the
                    //earlier page

                    sql = " Select CONVERT(VARCHAR,MsgDateTime,9) MsgDateTime From (SELECT TOP 1 * FROM "
                        + " (SELECT TOP " + ((pageNo - 1) * postsPerPage) + " "
                        + " FT.MsgDateTime FROM ForumThreads AS FT With(NoLock) "
                        + " WHERE IsActive = 1 AND ForumId = @ThreadId "
                        + " Order By MsgDateTime) AS lastdates Order By MsgDateTime DESC) "
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

                    //get the list of posts
                    sql = " SELECT TOP " + postsPerPage.ToString() + " FT.ID, IsNull(C.Name,'anonymous') AS PostedBy, "
                        + " IsNull(C.Id, 0) AS PostedById, Ci.Name AS City, FT.Message, FT.MsgDateTime, FT.LastUpdatedTime, "
                        + " IsNull( C1.Name,'N/A') AS UpdatedBy, "
                        + " IsNull((SELECT AvtarPhoto FROM UserProfile With(NoLock) WHERE IsAvtarApproved=1 AND UserId=FT.CustomerId), '') Avtar, "
                        + " IsNull((SELECT Signature FROM UserProfile With(NoLock) WHERE UserId=FT.CustomerId), '') Signature, "
                        + " (SELECT COUNT(FT1.ID) FROM ForumThreads FT1, Forums F With(NoLock) WHERE F.Id=FT1.ForumId "
                        + " AND FT1.CustomerId=FT.CustomerId AND FT1.IsActive=1 AND F.IsActive=1) Posts, "
                        + " (SELECT Role FROM BikeWaleRoles FR, ForumCustomerRoles FCR With(NoLock) WHERE FCR.RoleId=FR.ID AND CustomerId=FT.CustomerId) Role, "
                        + " (SELECT CustomerId FROM BikeWaleGuys With(NoLock) WHERE CustomerId=FT.CustomerId) BikewaleGuy, "
                        + " IsNull(FB.CustomerId, -1) AS BannedCust, "
                        + " ISNULL((SELECT HandleName FROM UserProfile UP With(NoLock) WHERE UP.UserId = C.id),'anonymous') AS HandleName,"
                        + " ISNULL((SELECT HandleName FROM UserProfile U With(NoLock) WHERE U.UserId = C1.id),'anonymous')  AS LastUpdatedHandle "
                        + " ,ISNULL((SELECT ThanksReceived FROM UserProfile UP1 With(NoLock) WHERE UP1.UserId = FT.CustomerId),0) AS ThanksReceived"
                        + " ,ISNULL((SELECT CONVERT(VARCHAR,JoiningDate,106) FROM UserProfile UP2 With(NoLock) WHERE UP2.UserId = FT.CustomerId),0) AS JoiningDate"
                        + " FROM (((( ForumThreads AS FT With(NoLock) LEFT JOIN Customers AS C With(NoLock) ON C.ID=FT.CustomerId )"
                        + " LEFT JOIN Customers AS C1 With(NoLock) ON C1.ID=FT.UpdatedBy ) "
                        + " LEFT JOIN Cities Ci With(NoLock) ON C.CityId=Ci.Id ) "
                        + " LEFT JOIN Forum_BannedList AS FB With(NoLock) ON FB.CustomerId = C.ID) "
                        + " WHERE FT.ForumId= @ThreadId AND FT.MsgDateTime > '" + lastMsgDate + "'"
                        + " AND FT.IsActive = 1 ORDER BY FT.MsgDateTime ";
                }

                Trace.Warn("viewThread1: " + sql);

                op.BindRepeaterReader(sql, rptThreads, param);

                if (rptThreads.Items.Count == 0)
                {
                    divThread.Visible = false;
                    lblMessage.Text = "<p>No posts in this forum.</p>";
                }
                else
                {
                    divThread.Visible = true;

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
        }


        public string GetTitle(string value)
        {
            string[] words = value.Split(' ');

            string retVal = "";

            for (int i = 0; i < words.Length; i++)
            {
                //if (i == 0)
                //{
                //    retVal = "<span>" + words[i] + "</span>";
                //}
                //else
                    retVal += " " + words[i];
            }

            return retVal;
        }

        public string GetMessage(string value)
        {
            string post = value;
            //post = post.Replace("\n", "<br>");

            string quoteStart = "<div class='quote'>Posted by <b>";
            string quotePostedByEnd = "</b><br>";
            string quoteEnd = "</div>";

            // Identify and replace quotes
            if (post.ToLower().IndexOf("[^^quote=") >= 0)
            {
                Trace.Warn("Quote Found");
                post = post.Replace("[^^quote=", quoteStart);
                post = post.Replace("[^^/quote^^]", quoteEnd);
                post = post.Replace("^^]", quotePostedByEnd);

                Trace.Warn(post);
            }

            if (Request["h"] != null && Request["h"] != "")
            {
                post = post.ToLower().Replace(Request["h"], "<span style='background:#FFFF00;color:#000000;'>" + Request["h"] + "</span>");
            }

            //add smileys
            post = post.Replace(":)", "<img class='inobg' src='" + ImagingFunctions.GetRootImagePath() + "/images/forums/smileys/smile.gif' border='0' align='absmiddle' />");
            post = post.Replace(":(", "<img class='inobg' src='" + ImagingFunctions.GetRootImagePath() + "/images/forums/smileys/sad.gif' border='0' align='absmiddle' />");
            post = post.Replace(":D", "<img class='inobg' src='" + ImagingFunctions.GetRootImagePath() + "/images/forums/smileys/biggrin.gif' border='0' align='absmiddle' />");
            post = post.Replace(":-)", "<img class='inobg' src='" + ImagingFunctions.GetRootImagePath() + "/images/forums/smileys/smile.gif' border='0' align='absmiddle' />");
            post = post.Replace(":-(", "<img class='inobg' src='" + ImagingFunctions.GetRootImagePath() + "/images/forums/smileys/sad.gif' border='0' align='absmiddle' />");
            post = post.Replace(":-D", "<img class='inobg' src='" + ImagingFunctions.GetRootImagePath() + "/images/forums/smileys/biggrin.gif' border='0' align='absmiddle' />");

            post = post.Replace("<p>&nbsp;</p>", ""); // remove empty paragraphs.

            return post;
        }

        public string GetSignature(string value)
        {
            if (value != "")
                return "<img src=\"" + ImagingFunctions.GetRootImagePath() + "/images/forums/line.jpg\" align=\"absmiddle\" /><BR><span style=\"color: #777777\"> " + value + "</span>";
            else
                return "";
        }

        public string GetDateTime(string value)
        {
            if (value != "")
                return Convert.ToDateTime(value).ToString("dd-MMM, yyyy hh:mm tt");
            else
                return "N/A";
        }

        bool GetModeratorLoginStatus(string customerId)
        {
            ForumsCommon fc = new ForumsCommon();
            return fc.GetModeratorLoginStatus(customerId);
        }

        // will tell if a post can be edited or not
        public bool IsPostEditable(string msgDateTime, string postedBy)
        {
            bool logStatus = false;

            if (CurrentUser.Id == postedBy && Convert.ToDateTime(msgDateTime).AddMinutes(10) >= DateTime.Now)
                logStatus = true;
            else
                logStatus = false;

            return logStatus;
        }

        void AddStrip()
        {
            //make the strip, based on the total questions, the current page, and the page count
            postsPerPage = postsPerPage <= 0 ? 1 : postsPerPage;
            int pages = (int)Math.Ceiling(Convert.ToDouble(total) / Convert.ToDouble(postsPerPage));

            string pageUrl = "ViewThread-" + threadId;

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

                for (int i = startIndex; i <= endIndex; i++)
                {
                    if (i != pageNo)
                        divStrip.InnerHtml += "<span class='ac'><a href='" + pageUrl + "-p" + i.ToString() + ".html'>"
                                            + i.ToString() + "</a></span>";
                    else
                        divStrip.InnerHtml += "<span class='iac'>" + i.ToString() + "</span>";
                }

                if (endIndex < pages)
                    divStrip.InnerHtml += "<span class='ac'><a href='" + pageUrl + "-p"
                                        + (endIndex + 1).ToString() + ".html'>»</a></span>";

                // now make an exact replica of footer strip.						
                divStripTop.InnerHtml = divStrip.InnerHtml;
            }
        }

        public string GetUserTitle(string role, string posts, string bannedCust)
        {
            string title = "";

            if (bannedCust != "-1")
            {
                title = "[BANNED]";
            }
            else if (role != "")
            {
                //title = "<img src='" + CarWale.CommonCW.ImagingFunctions.GetRootImagePath() + "/images/forums/" + role + ".jpg' align='absmiddle' title='" + role + "' />";
                //Above line is commented and replaced as follow
                title = "<div><div style=\"height: 16px;margin: auto;width: 16px;float:left;\" title='" + role + "' ><img src='" + ImagingFunctions.GetRootImagePath() + "/images/forums/moderatorforum.gif' /></div><div style='float:left;margin-left:5px;width:100px;'>Moderator</div><div style='clear:both;'></div></div>";
            }
            else
            {
                int noOfPosts = int.Parse(posts);

                if (noOfPosts < 26)
                    title = "New Arrival<br>";
                else if (noOfPosts < 51)
                    title = "Driven<br>";
                else if (noOfPosts < 101)
                    title = "Road-tested<br>";
                else if (noOfPosts < 251)
                    title = "Long-termer<br>";
                else if (noOfPosts < 501)
                    title = "Beloved<br>";
                else if (noOfPosts < 1001)
                    title = "Best-seller<br>";
                else title = "Legend<br>";
            }

            return title;
        }

        public string SendPrivateMessage(string postedById, string handle)
        {
            string retLink = "";

            if (customerId != "" && customerId != "-1" && postedById != CurrentUser.Id && postedById != "0")
            {
                //retLink = "<a id='#' target='_blank' href='/community/pms/compose.aspx?username=" + handle + "'><img align='absmiddle' title='Send a private message to this member' style='cursor:pointer' border='0' src='" + ImagingFunctions.GetRootImagePath() + "/images/icons/sendmessage.gif'"
                //        + " alt='PM' ></img></a>";
            }

            return retLink;
        }

        // function to figure out given post no's page.
        void FindPost(int post)
        {
            CommonOpn op = new CommonOpn();
            string sql;
            Database db = new Database();
            SqlDataReader dr = null;
            int oldPosts = 0;

            try
            {
                //get the total number of posts
                sql = " SELECT Count(ID) AS OldPosts "
                    + " FROM ForumThreads With(NoLock) "
                    + " WHERE IsActive=1 AND ForumId = @ThreadId "
                    + " AND MsgDateTime < (SELECT MsgDateTime FROM ForumThreads With(NoLock) WHERE Id = @ForumThreadId)";

                SqlParameter[] param = 
						{
							new SqlParameter("@ThreadId", threadId),
							new SqlParameter("@ForumThreadId", post)
						};

                dr = db.SelectQry(sql, param);

                Trace.Warn(sql);

                if (dr.Read())
                {
                    oldPosts = Convert.ToInt32(dr["OldPosts"]);
                    Trace.Warn("Old Posts : " + oldPosts);
                }

                pageNo = (oldPosts - (oldPosts % postsPerPage)) / postsPerPage + 1;

                Trace.Warn("Extracted Page No : " + pageNo);
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

            if (pageNo > 1)
                Response.Redirect("viewthread-" + threadId + "-p" + pageNo + ".html#post" + post);
            else
                Response.Redirect("viewthread-" + threadId + ".html#post" + post);
        }

        // Quick Reply
        void butSave_Click(object Sender, EventArgs e)
        {
            if (flagLogin == true)
            {
                //check whether the user is in the banned list or not
                if (ForumsCommon.IsUserBanned(customerId) == false)
                {
                    int alertType = 0;

                    if (chkEmailAlert.Checked)
                        alertType = 2; // instant email subscription
                    else
                        alertType = 1; // normal subscription

                    // post save with 100 alert type. It means no action is to be taken on alerts. Leave them intact.
                    string postId = ForumsCommon.SavePost(CurrentUser.Id, SanitizeHTML.ToSafeHtml(rteQR.Text), alertType, threadId);
                    Trace.Warn("rteQR.Text: " + rteQR.Text);
                    Trace.Warn(postId);
                    string discussionUrl = "/Forums/viewthread.aspx?thread=" + threadId + "&post=" + postId;

                    if (postId != "-1")
                    {
                        //send mails to all the participants
                        ForumsCommon.NotifySubscribers(discussionUrl, threadId, this.ThreadName);

                        Trace.Warn("Data is saved and now redirecting... : " + discussionUrl);
                        //redirect it to message page
                        Response.Redirect(discussionUrl);
                    }
                    else
                        lblMessage.Text = "Some error occured. Post could not be submitted. Please try again.";
                }
                else
                {
                    lblMessage.Text = "Your 'BikeWale Forum' membership has been suspended. You cannot post in BikeWale Forums anymore. If it looks like a mistake to you, please write to banwari@Bikewale.com.<br><br>";
                }
            }
            else
            {
                Response.Redirect("/Forums/notauthorized.aspx?returnUrl=" + HttpUtility.UrlEncode(Request.ServerVariables["HTTP_X_ORIGINAL_URL"]));
            }

        }

        protected string ConcatenatePostedByUserIds(string _postedbyid)
        {
            if (postedByUserIds == "")
            {
                postedByUserIds = "," + _postedbyid + ",";
            }
            else
            {
                if (postedByUserIds.IndexOf("," + _postedbyid + ",") == -1)
                    postedByUserIds = postedByUserIds + _postedbyid + ",";
            }
            return "";
        }

        protected string GetPostedByUserIds()
        {
            return postedByUserIds;
        }

        protected string ConcatenatePostIds(string _postid)
        {
            if (concatenatedPostIds == "")
            {
                concatenatedPostIds = _postid;
            }
            else
            {
                concatenatedPostIds = concatenatedPostIds + "," + _postid;
            }
            return "";
        }

        protected string CheckOwnerForVisibility(string _postedById)
        {
            if (_postedById == CurrentUser.Id)
                return "style='display:none;'";
            else
                return "style='display:display;'";
        }
    }
}