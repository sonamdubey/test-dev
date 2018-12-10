using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Carwale.UI.Common;
using Carwale.Notifications;
using System.Data.Common;
using Carwale.DAL.CoreDAL.MySql;
using MySql.Data.MySqlClient;

namespace Carwale.UI.Controls
{
    public class DiscussIt : UserControl
    {
        protected Repeater rptDiscussIt;
        protected HtmlGenericControl divThread;
        public string varThreadid;
        public string varType = "article";
        public string _ThreadUrl = "";

        protected override void OnInit(EventArgs e)
        {
            InitializeComponents();
        }

        public string ThreadId
        {
            get { return varThreadid; }
            set { varThreadid = value; }
        }

        public string ThreadUrl
        {
            get { return _ThreadUrl; }
            set { _ThreadUrl = value; }
        }

        public string Type
        {
            get { return varType; }
            set { varType = value; }
        }

        void InitializeComponents()
        {
            this.Load += new EventHandler(this.Page_Load);
        }

        void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (this.ThreadId != null && this.ThreadId != "-1" && this.ThreadId.ToString() != "")
                    FetchPostings();
                else
                    divThread.Visible = false;
            }
        } // Page_Load

        void FetchPostings()
        {        
            CommonOpn co = new CommonOpn();
            DataSet ds = new DataSet();
            try
            {                           
                using (DbCommand cmd = DbFactory.GetDBCommand("FetchPosting_v16_11_7"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_ForumId", DbType.Int64, this.ThreadId));
                    ds = MySqlDatabase.SelectAdapterQuery(cmd, DbConnections.ForumsMySqlReadConnection);
                }
                co.BindRepeaterReaderDataSet(ds, rptDiscussIt);
            }
            catch (MySqlException err)
            {
                HttpContext.Current.Trace.Warn("FetchPostings Sql EX : ", err.Message);
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            catch (Exception err)
            {
                HttpContext.Current.Trace.Warn("FetchPostings EX : ", err.Message);
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        } // FetchPostings

        public string FormatMessage(string message)
        {
            string msg = "";

            if (message.IndexOf("[^^/quote^^]") > 0)
            {
                Trace.Warn("index : " + message + ", " + message.LastIndexOf("[^^/quote^^]") + ", " + message.Length);
                msg = message.Substring(message.LastIndexOf("[^^/quote^^]") + 12, message.Length - message.LastIndexOf("[^^/quote^^]") - 12);
            }
            else msg = message;

            return msg;
        }
    } // Class
} // Namespace
