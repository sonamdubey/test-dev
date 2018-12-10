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
using Carwale.DAL.CoreDAL;
using Carwale.UI.Common;
using CarwaleAjax;
using Carwale.DAL.Forums;
using Carwale.Cache.Forums;
using System.Data.Common;
using Carwale.DAL.CoreDAL.MySql;
using MySql.Data.MySqlClient;

namespace Carwale.UI.Forums
{
    public class SplitPost : Page
    {
        #region global variables
        protected Label lblMessage, Thread;
        protected Button butSave, butSaveExisting;
        protected TextBox txtTitle, txtThreadID;
        protected DropDownList cmbCategories;
        protected RadioButton rdbNewThread, rdbExistingThread;
        string threadId = "", Id = "", ForumId = "", strFirstId = "", StrcatNew, StrCustId, StrdateTime;
        public int page = 1;
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
            butSaveExisting.Click += new EventHandler(butSaveExisting_Click);
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

            if ((Request["Id"] != null) && Request.QueryString["Id"].ToString() != "")
            {
                Id = Request.QueryString["Id"].ToString();
            }
            else
            {
                Response.Redirect("/forums/");
            }

            if (Request["ForumId"] != null && Request.QueryString["ForumId"].ToString() != "")
            {
                ForumId = Request.QueryString["ForumId"].ToString();

                if (CommonOpn.CheckId(ForumId) == false)
                {
                    return;
                }
            }

            if (Request["threadId"] != null && Request.QueryString["threadId"].ToString() != "")
            {
                threadId = Request.QueryString["threadId"].ToString();

                if (CommonOpn.CheckId(threadId) == false)
                {
                    return;
                }
            }

            if (Request.QueryString["page"] != null && CommonOpn.CheckId(Request.QueryString["page"]))
            {
                int.TryParse(Request.QueryString["page"], out page);
            }
            else
            {
                Response.Redirect("./");
            }

            Ajax.Utility.RegisterTypeForAjax(typeof(AjaxForum));

            if (!IsPostBack)
            {
                FillCategories();
            }
        } // Page_Load
        #endregion

        #region Button Save click


        void butSave_Click(object Sender, EventArgs e)
        {
            try
            {
                GetFirstId(Id);

                //Insert into database
                string saveId;
                saveId = SaveData("-1");

                if (saveId != "-1")
                {
                    UpdateStats(saveId, cmbCategories.SelectedValue);
                    UpdateStats(threadId, ForumId);
                    lblMessage.Text = "Data Saved Successfully";
                    txtTitle.Text = "";
                    var forumsCache = new ForumsCache();
                    forumsCache.InvalidatePaginationCache(string.Format("Forums-Id-{0}-Page",ForumId),1);
                    forumsCache.InvalidatePaginationCache(string.Format("Forums-Id-{0}-Page", cmbCategories.SelectedValue), 1);
                    forumsCache.InvalidatePaginationCache(string.Format("Forums-Thread-{0}-Page", threadId),page);
                }
                else
                {
                    lblMessage.Text = "You are trying to put duplicate entry.";
                }
            }
            catch (Exception ex)
            {
                Trace.Warn(ex.Message);
                ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }
        #endregion

        #region Buttom Save Existing

        void butSaveExisting_Click(object Sender, EventArgs e)
        {
            try
            {
                if (SaveExistingData())
                {
                    UpdateStats(txtThreadID.Text, StrcatNew);
                    UpdateStats(threadId, ForumId);

                    lblMessage.Text = "Data Saved Successfully";
                    txtThreadID.Text = "";
                    var forumsCache = new ForumsCache();
                    forumsCache.InvalidatePaginationCache(string.Format("Forums-Id-{0}-Page", ForumId), 1);
                    forumsCache.InvalidatePaginationCache(string.Format("Forums-Thread-{0}-Page", txtThreadID.Text), 1);
                    forumsCache.InvalidatePaginationCache(string.Format("Forums-Thread-{0}-Page", threadId), page);
                }
                else
                {
                    lblMessage.Text = "You are trying to put duplicate entry.";
                }

            }
            catch (Exception ex)
            {
                Trace.Warn(ex.Message);
                ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }

        }
        #endregion

        #region Get First Id
        protected void GetFirstId(string ids)
        {
            try
            {
                if (ids != "")
                {
                    strFirstId = ids.Split(',')[0].Trim();
                    //Get customerId,forumId to Insert in Forum
                    GetCustomerDetails(strFirstId);
                }
            }
            catch (Exception ex)
            {
                Trace.Warn(ex.Message);
                ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }
        #endregion

        #region Get Customer Details


        void GetCustomerDetails(string postId)
        {
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("GetForumOwnerId_v16_11_7"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_PostId", DbType.Int64, postId));
                    using(IDataReader dr = MySqlDatabase.SelectQuery(cmd,DbConnections.ForumsMySqlReadConnection))
                    {
                        if (dr.Read())
                        {
                            StrCustId = dr["CustomerId"].ToString();
                            StrdateTime = dr["MsgDateTime"].ToString();
                        }
                    }
                }
            }
            catch (MySqlException err)
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
        #endregion

        #region Save Existing Data

        bool SaveExistingData()
        {
            bool isCompleted = false;
            //Update ForumId
            UpdateForumThreads(txtThreadID.Text);
            //Update Subscriptions
            GetForumId(txtThreadID.Text);
            UpdateSubscriptions();
            isCompleted = true;
            return isCompleted;
        }
        #endregion

        #region Save Data

        string SaveData(string sId)
        {
            string lastSavedId = "-1";
            PostDAL postDL = new PostDAL();
            lastSavedId = postDL.SplitPostSaveData(Convert.ToInt32(sId), Convert.ToInt32(cmbCategories.SelectedValue), txtTitle.Text.Trim().ToString(), Convert.ToInt32(StrCustId), Convert.ToDateTime(StrdateTime));
            //Update ForumId
            UpdateForumThreads(lastSavedId);
            //Update Subscriptions
            UpdateSubscriptions();
            return lastSavedId;
        }
        #endregion

        #region Update Forum Threads


        protected void UpdateForumThreads(string lastSavedId)
        {
            string strId;
            ArrayList arrid = new ArrayList();
            try
            {
                arrid.AddRange(Id.Split(new char[] { ',' }));
                for (int h = 0; h <= arrid.Count - 1; h++)
                {
                    strId = arrid[h].ToString();
                    using(DbCommand cmd = DbFactory.GetDBCommand("UpdateForumAndThread_v16_11_7"))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DbFactory.GetDbParam("v_ForumID", DbType.Int64, lastSavedId));
                        cmd.Parameters.Add(DbFactory.GetDbParam("v_ForumThreadId", DbType.Int64, strId));
                        MySqlDatabase.UpdateQuery(cmd, DbConnections.ForumsMySqlMasterConnection);
                    }
                }
            }
            catch (MySqlException err)
            {
                //catch the sql exception. if it is equal to 2627, then say that it is for duplicate entry 
                Trace.Warn(err.Message);
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            } // catch SqlException
            catch (Exception ex)
            {
                Trace.Warn(ex.Message);
                ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }

        }
        #endregion

        #region Get Forum Id

        protected void GetForumId(string threadId)
        {
           // Database db = new Database();
           // SqlDataReader dr;
            string sql = "";

            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("GetForumSubCategory_v16_11_7"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_ThreadId", DbType.Int64, threadId));
                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, DbConnections.ForumsMySqlReadConnection))
                    {
                        if (dr.Read())
                        {
                            Trace.Warn(sql);
                            StrcatNew = dr["ForumSubCategoryId"].ToString();
                        }
                    }
                }
            }
            catch (MySqlException err)
            {
                //catch the sql exception. if it is equal to 2627, then say that it is for duplicate entry 
                Trace.Warn(err.Message);
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            } // catch SqlException
            catch (Exception ex)
            {
                Trace.Warn(ex.Message);
                ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }
        #endregion

        #region Update Subscriptions
        protected void UpdateSubscriptions()
        {
            ArrayList arrid = new ArrayList();
            string StrForumId1 = "", StrCustId1 = "";
            string strId;
            try
            {
                arrid.AddRange(Id.Split(new char[] { ',' }));
                for (int h = 0; h <= arrid.Count - 1; h++)
                {
                    strId = arrid[h].ToString();
                    using (DbCommand cmd = DbFactory.GetDBCommand("UpdateSubscription_v16_11_7"))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DbFactory.GetDbParam("v_ForumThreadId", DbType.Int64, strId));
                        using(IDataReader dr = MySqlDatabase.SelectQuery(cmd,DbConnections.ForumsMySqlMasterConnection))
                        {
                            if (dr.Read())
                            {                          
                                StrForumId1 = dr["ForumId"].ToString();
                                StrCustId1 = dr["CustomerId"].ToString();
                            }
                        }
                    }
                // Insert into Subscriptions
                    AddSubscription(StrCustId1, StrForumId1);
                }
            }
            catch (MySqlException err)
            {
                //catch the sql exception. if it is equal to 2627, then say that it is for duplicate entry 
                Trace.Warn(err.Message);
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            } // catch SqlException
            catch (Exception ex)
            {
                Trace.Warn(ex.Message);
                ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }
        #endregion

        #region Add Subscriptions

        protected bool AddSubscription(string custID, string subId)
        {
            ForumSubscriptionsDAL forumSubscriptions = new ForumSubscriptionsDAL();
            return forumSubscriptions.AddSubscription(custID, subId);
        }
        #endregion

        #region Update Stats

        protected bool UpdateStats(string ForumId, string SubCategoryId)
        {
            ThreadsDAL threadDL = new ThreadsDAL();
            return threadDL.UpdateStats(ForumId, SubCategoryId);
        }
        #endregion

        #region Fill Categories

        void FillCategories()
        {
            ThreadsDAL threadDetails = new ThreadsDAL();
            DataSet ds = threadDetails.FillCategories();
            try
            {
                if (ds != null && ds.Tables.Count > 0)
                {
                    cmbCategories.DataSource = ds.Tables[0];
                    cmbCategories.DataTextField = "Category";
                    cmbCategories.DataValueField = "Id";
                    cmbCategories.DataBind();
                    cmbCategories.Items.Insert(0, new ListItem("--Select--", "0"));
                }
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            } // catch Exception
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