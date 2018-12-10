using System;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using MobileWeb.Common;
using MobileWeb.DataLayer;
using MobileWeb.Controls;
using Carwale.Notifications.Logs;
using System.Data.Common;
using Carwale.DAL.CoreDAL.MySql;

namespace MobileWeb.Forums
{
    public class RedirectToThank : Page
    {
     
        private string[] splittedParams;
        protected string postId;
        protected string ch;
        protected string handleExists;
        protected string isSaved = "-2";

        //isSaved=1:newly liked, isSaved=0:already liked, isSaved=-2:default value, isSaved=-3:current user's post
        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
        }

        void InitializeComponent()
        {
            base.Load += new EventHandler(Page_Load);
        }

        void Page_Load(object Sender, EventArgs e)
        {
            splittedParams = Request.QueryString["params"].ToString().Split(',');
            postId = splittedParams[0].ToString();
            ch = splittedParams[1].ToString();
            if (CommonOpn.CheckId(postId) == false || CommonOpn.CheckId(ch) == false)
            {
                return;
            }
            if (ch == "0")
            {
                handleExists = "1";
                if (NotPostedByCurrentUser())
                {
                    SaveToPostThanks(CurrentUser.Id, postId);
                }
            }
            else if (ch == "1")
            {
                handleExists = "0";
                if (DoesHandleExists(CurrentUser.Id))
                {
                    handleExists = "1";
                    if (NotPostedByCurrentUser())
                    {
                        SaveToPostThanks(CurrentUser.Id, postId);                    
                    }
                }
            }
        }

        protected bool NotPostedByCurrentUser()
        {
            bool retVal = true;
            try
            {
                using(DbCommand cmd = DbFactory.GetDBCommand("GetCustomerIdByPostId_v16_11_7"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_PostId", DbType.Int64, postId));
                    using(IDataReader dr = MySqlDatabase.SelectQuery(cmd,DbConnections.ForumsMySqlReadConnection))
                    {
                        if (dr[0].ToString() == CurrentUser.Id)
                        {
                            retVal = false;
                            isSaved = "-3";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            return retVal;
        }


        private bool DoesHandleExists(string cId)
        {
            bool handleExists = false;         
            try
            {
                using(DbCommand cmd = DbFactory.GetDBCommand("GetExistingHandleDetails_v16_11_7"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_userId", DbType.Int64, cId));
                    using(IDataReader dr = MySqlDatabase.SelectQuery(cmd,DbConnections.ForumsMySqlReadConnection))
                    {
                        if (dr.Read())
                          handleExists = true;
                    }
                }
            }
            catch (Exception ex)
            {
                handleExists = false;
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            return handleExists;
        }

        private void SaveToPostThanks(string _customerId, string _postId)
        {                    
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("PostThanksSave_v16_11_7"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_CustomerID", DbType.Int64, _customerId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_PostID", DbType.Int64, _postId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_IsSaved", DbType.Boolean, ParameterDirection.Output));
                    LogLiveSps.LogSpInGrayLog(cmd);
                    cmd.ExecuteNonQuery();
                    if (Convert.ToBoolean(cmd.Parameters["v_IsSaved"].Value))
                        isSaved = "1";
                    else
                        isSaved = "0";
                }        
            }
            catch (Exception ex)
            {
                isSaved = "-1";
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }        
        }
      
    }
}