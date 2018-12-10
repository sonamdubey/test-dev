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
using Carwale.Notifications;
using Carwale.UI.Common;
using Carwale.DAL.CoreDAL;
using Carwale.DAL.Forums;
using Carwale.Notifications.Logs;
using System.Data.Common;
using Carwale.DAL.CoreDAL.MySql;
using MySql.Data.MySqlClient;
using Carwale.Cache.Forums;

namespace Carwale.UI.Forums
{
    public class modReportAbuse : Page
    {
        #region Global Variables
        protected HtmlGenericControl divForum, divMessage;
        protected Repeater rptReport;
        protected Button btnApprove, btnDelete;

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
            btnDelete.Click += new EventHandler(btnDelete_Click);
            btnApprove.Click += new EventHandler(btnApprove_Click);
        }
        #endregion

        #region Page Load
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
        #endregion

        #region Get Moderatoe Login status
        bool GetModeratorLoginStatus(string customerId)
        {
            ForumsCache threadDetails = new ForumsCache();
            return threadDetails.IsModerator(customerId);
        }
        #endregion

        #region Show Abuse Report
        //Bind Data to Grid for display
        void ShowReport()
        {
            ForumsModeratorDAL moderatorDetails = new ForumsModeratorDAL();
            DataSet ds = moderatorDetails.ShowRepotAbuseReport();
            rptReport.DataSource = ds.Tables[0];
            rptReport.DataBind();
        }
        #endregion

        #region Delete Button Click
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
        #endregion

        #region Approve Button Click
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
                        using(DbCommand cmd = DbFactory.GetDBCommand("ApproveAbusedReport_v16_11_7"))
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
        #endregion

        #region Delete Thread
        void DeleteThread(string postId)
        {
            CommonOpn op = new CommonOpn();
            try
            {
                using(DbCommand cmd = DbFactory.GetDBCommand("Forum_UpdateReportAbuse_v16_11_7"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_PostId", DbType.Int64, postId));
                    LogLiveSps.LogSpInGrayLog(cmd);
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
        #endregion

    } // class
} // namespace