using System;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Carwale.Notifications;
using Carwale.DAL.CoreDAL;
using Carwale.UI.Common;
using Carwale.DAL.Forums;
using System.Data.Common;
using Carwale.DAL.CoreDAL.MySql;
using MySql.Data.MySqlClient;
using Carwale.Cache.Forums;

namespace Carwale.UI.Community.Mods
{
    public class NewsComments : System.Web.UI.Page
    {
        protected Repeater rptNewsComments;
        DataSet ds = new DataSet();
        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
        }

        void InitializeComponent()
        {
            base.Load += new EventHandler(Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (CurrentUser.Id == "-1")
            {
                Response.Redirect("/users/login.aspx?ReturnUrl=" + HttpUtility.UrlEncode(Request.ServerVariables["HTTP_X_REWRITE_URL"]));
            }

            ForumsCache threadInfo = new ForumsCache();
            bool isModerator = threadInfo.IsModerator(CurrentUser.Id);
            if (!(isModerator))
            {
                UrlRewrite.Return404();
            }

            if (!IsPostBack)
            {
                LoadNewsComments();
            }
        }

        private void LoadNewsComments()
        {          
            try
            {          
                using(DbCommand cmd = DbFactory.GetDBCommand("GetNewsComments_v16_11_7"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    ds = MySqlDatabase.SelectAdapterQuery(cmd, DbConnections.EditCmsMySqlReadConnection);
                }
                if (ds != null && ds.Tables.Count > 0)
                {
                    DataTable dt = new DataTable();
                    dt = ds.Tables[0].DefaultView.ToTable(true, new string[] { "NewsId", "NewsTitle", "Url" });
                    rptNewsComments.DataSource = dt;
                    rptNewsComments.DataBind();
                }
            }
            catch (MySqlException err)
            {
                Trace.Warn("Sql Error: " + err.Message + err.Source);
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            catch (Exception err)
            {
                Trace.Warn("Error: " + err.Message + err.Source);
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }

        protected DataTable GetComments(string newsId)
        {
            DataTable dtComments = new DataTable();
            DataColumn column;
            DataRow row;

            DataRow[] drRows = ds.Tables[0].Select("NewsId='" + newsId + "'");

            column = new DataColumn();
            column.DataType = System.Type.GetType("System.String");
            column.ColumnName = "CommentId";
            column.ReadOnly = true;

            dtComments.Columns.Add(column);

            column = new DataColumn();
            column.DataType = System.Type.GetType("System.String");
            column.ColumnName = "UserName";
            column.ReadOnly = true;

            dtComments.Columns.Add(column);

            column = new DataColumn();
            column.DataType = System.Type.GetType("System.String");
            column.ColumnName = "Email";
            column.ReadOnly = true;

            dtComments.Columns.Add(column);

            column = new DataColumn();
            column.DataType = System.Type.GetType("System.String");
            column.ColumnName = "CommentDateTime";
            column.ReadOnly = true;

            dtComments.Columns.Add(column);

            column = new DataColumn();
            column.DataType = System.Type.GetType("System.String");
            column.ColumnName = "Comment";
            column.ReadOnly = true;

            dtComments.Columns.Add(column);

            for (int i = 0; i < drRows.Length; ++i)
            {
                row = dtComments.NewRow();
                row["CommentId"] = drRows[i]["CommentId"].ToString();
                row["UserName"] = drRows[i]["UserName"].ToString();
                row["Email"] = drRows[i]["Email"].ToString();
                row["CommentDateTime"] = drRows[i]["CommentDateTime"].ToString();
                row["Comment"] = drRows[i]["Comment"].ToString();
                dtComments.Rows.Add(row);
            }

            return dtComments;
        }
    }
}