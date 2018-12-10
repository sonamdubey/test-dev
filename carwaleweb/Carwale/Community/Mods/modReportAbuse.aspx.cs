using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Carwale.Notifications;
using Carwale.UI.Common;
using Carwale.DAL.CoreDAL;
using Carwale.DAL.Forums;
using Carwale.Notifications.Logs;
using System.Data.Common;
using Carwale.DAL.CoreDAL.MySql;
using MySql.Data.MySqlClient;
using Carwale.Cache.Forums;

namespace Carwale.UI.Community.Mods
{
    public class modReportAbuse : Page
    {
        protected HtmlGenericControl divForum, divMessage;
        protected Repeater rptReport;
        protected Button btnApprove, btnDelete;

        private string customerId = "";

        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
        }

        void InitializeComponent()
        {
            base.Load += new EventHandler(Page_Load);
            btnDelete.Click += new EventHandler(btnDelete_Click);
            btnApprove.Click += new EventHandler(btnApprove_Click);
        }

        void Page_Load(object Sender, EventArgs e)
        {
            // someone has requested forum subscription
            customerId = CurrentUser.Id;

            if (!GetModeratorLoginStatus(customerId))
                Response.Redirect("default.aspx");

            if (!IsPostBack)
            {
                ShowReport();
            }
        } // Page_Load

        bool GetModeratorLoginStatus(string customerId)
        {
            ForumsCache threadInfo = new ForumsCache();
            return threadInfo.IsModerator(CurrentUser.Id);

        }

        //Bind Data to Grid for display
        void ShowReport()
        {         
            CommonOpn op = new CommonOpn();
            DataSet ds = new DataSet();
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("GetReportedPosts_v16_11_7"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    ds = MySqlDatabase.SelectAdapterQuery(cmd, DbConnections.ForumsMySqlReadConnection);
                }
                op.BindRepeaterReaderDataSet(ds, rptReport);
            }
            catch (Exception err)
            {
                Trace.Warn(err.Message);
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }

        void btnDelete_Click(object sender, EventArgs e)
        {
            Label lblId;
            CheckBox chk;
            for (int i = 0; i < rptReport.Items.Count; i++)
            {
                lblId = (Label)rptReport.Items[i].FindControl("lblId");
                chk = (CheckBox)rptReport.Items[i].FindControl("chkID");

                if (chk.Checked)
                {
                    DeleteThread(lblId.Text);
                }
            }

            ShowReport();

            divMessage.InnerHtml = "Report deleted successfully";
            divMessage.Visible = true;
        }

        void btnApprove_Click(object sender, EventArgs e)
        {
            Label lblReportId;
            CheckBox chk;
            for (int i = 0; i < rptReport.Items.Count; i++)
            {
                lblReportId = (Label)rptReport.Items[i].FindControl("lblReportId");
                chk = (CheckBox)rptReport.Items[i].FindControl("chkID");

                if (chk.Checked)
                {
                    try
                    {                    
                        using(DbCommand cmd = DbFactory.GetDBCommand("ApproveReportedPostByAbuseId_v16_11_7"))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add(DbFactory.GetDbParam("v_AbuseId", DbType.Int64, lblReportId.Text));
                            MySqlDatabase.UpdateQuery(cmd, DbConnections.ForumsMySqlMasterConnection);
                        }
                    }
                    catch (MySqlException err)
                    {
                        Trace.Warn(err.Message);
                        ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                        objErr.SendMail();
                    } // catch Exception
                }
            }

            ShowReport();

            divMessage.InnerHtml = "Report approved successfully";
            divMessage.Visible = true;
        }


        void DeleteThread(string postId)
        {      
            CommonOpn op = new CommonOpn();
            try
            {
                using(DbCommand cmd = DbFactory.GetDBCommand("Forum_UpdateReportAbuse_v16_11_7"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_PostId", DbType.Int64, postId));
                    MySqlDatabase.ExecuteNonQuery(cmd, DbConnections.ForumsMySqlMasterConnection);

                }          
            }
            catch (MySqlException err)
            {
                Trace.Warn(err.Message);
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            } // catch Exception       
        }

    } // class
} // namespace