using System;
using System.Text;
using System.Data;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using Carwale.UI.Common;
using System.Collections;
using Ajax;
using AjaxPro;
using Carwale.DAL.CoreDAL;
using Carwale.Notifications;
using Carwale.DAL.Forums;
using Carwale.BL.Forums;
using Carwale.Notifications.Logs;
using System.Data.Common;
using Carwale.DAL.CoreDAL.MySql;
using MySql.Data.MySqlClient;
using Carwale.Cache.Forums;

namespace CarwaleAjax
{
    public class AjaxCommunityModerate
    {

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.None)]
        public string Moderate(string DataStringApprove, string DataStringBan, string ModId, string DataStringDeactivate)
        {
            string retVal = "";

            ForumsCache threadInfo = new ForumsCache();
            bool isModerator = threadInfo.IsModerator(CurrentUser.Id);
            if (isModerator)
            {

                DataTable Approvedt = new DataTable();
                Approvedt.Columns.Add("ThreadID");
                Approvedt.Columns.Add("CustomerID");
                Approvedt.Columns.Add("ForumID");
                DataTable Bandt = Approvedt.Clone();
                DataTable Deactivatedt = Approvedt.Clone();
                CreateNVC publishToRMQ = new CreateNVC();
                foreach (String str in DataStringApprove.Split('#'))
                {
                    if (!String.IsNullOrEmpty(str))
                    {
                        string[] str1 = str.Split('/');
                        Approvedt.Rows.Add(str1[0], str1[1], str1[2]);
                        publishToRMQ.UpdateLuceneIndex(str1[2], "1");
                    }
                }
                foreach (String str in DataStringBan.Split('#'))
                {
                    if (!String.IsNullOrEmpty(str))
                    {
                        string[] str1 = str.Split('/');
                        Bandt.Rows.Add(str1[0], str1[1], str1[2]);
                    }
                }
                foreach (String str in DataStringDeactivate.Split('#'))
                {
                    if (!String.IsNullOrEmpty(str))
                    {
                        string[] str1 = str.Split('/');
                        Deactivatedt.Rows.Add(str1[0], str1[1], str1[2]);
                    }
                }          
                try
                {
                    using (DbCommand cmd = DbFactory.GetDBCommand("ModerateApprove_v16_11_7"))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DbFactory.GetDbParam("v_approvelist", DbType.String, 8000,MySqlDatabaseUtility.ConvertDataTableToString(Approvedt)));
                        cmd.Parameters.Add(DbFactory.GetDbParam("v_modid", DbType.String, 5000, ModId));
                        MySqlDatabase.ExecuteNonQuery(cmd, DbConnections.ForumsMySqlMasterConnection);
                    }
                    using (DbCommand cmd = DbFactory.GetDBCommand("ModerateBan_v16_11_7"))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DbFactory.GetDbParam("v_banlist", DbType.String, 8000, MySqlDatabaseUtility.ConvertDataTableToString(Bandt)));
                        cmd.Parameters.Add(DbFactory.GetDbParam("v_modid", DbType.String, 5000, ModId));
                        MySqlDatabase.ExecuteNonQuery(cmd, DbConnections.ForumsMySqlMasterConnection);
                    }

                    using (DbCommand cmd = DbFactory.GetDBCommand("ModDeactivatePost_v16_11_7"))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DbFactory.GetDbParam("v_deactivatelist", DbType.String, 8000, MySqlDatabaseUtility.ConvertDataTableToString(Bandt)));
                        cmd.Parameters.Add(DbFactory.GetDbParam("v_modid", DbType.String, 5000, ModId));
                        MySqlDatabase.ExecuteNonQuery(cmd, DbConnections.ForumsMySqlMasterConnection);
                    }
                    retVal = "success!";                      
                }
                catch (MySqlException ex)
                {
                    retVal = "MySQL: " + ex.Message;
                    ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                    objErr.SendMail();

                }
                catch (Exception ex)
                {
                    retVal = "ex: " + ex.Message;
                    ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                    objErr.SendMail();

                }            
            }
            else
                retVal = "Access Denied!!";

            return retVal;
        }

    }
}