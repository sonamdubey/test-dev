using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Bikewale.Common;

namespace Bikewale.Controls
{
    public class DiscussIt : System.Web.UI.UserControl
    {
        protected Repeater rptDiscussIt;
        protected HtmlGenericControl divThread;
        public string varThreadid;
        public string varType = "article";
        public string ThreadId
        {
            get { return varThreadid; }
            set { varThreadid = value; }
        }

        public string Type
        {
            get { return varType; }
            set { varType = value; }
        }
        protected override void OnInit(EventArgs e)
        {
            base.Load += Page_Load;
        }
       
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (this.ThreadId != null && this.ThreadId != "-1" && this.ThreadId.ToString() != "")
                    FetchPostings();
                else
                    divThread.Visible = false;
            }
        }//pageload
        void FetchPostings()
        {
            string sql = "";
            CommonOpn co = new CommonOpn();

            try
            {
                sql = " SELECT TOP 10 ISNULL(c.Name,'Anonymous') as PostedBy, f.Message, f.MsgDateTime "
                    + " FROM (ForumThreads f With(NoLock) LEFT JOIN Customers c With(NoLock) ON f.CustomerId = c.Id) "
                    + " WHERE f.IsActive=1 AND f.ForumId= @ForumId "
                    + " ORDER BY f.Id DESC ";

                Trace.Warn("sql=" + sql);

                SqlParameter[] param = { new SqlParameter("@ForumId", this.ThreadId) };

                co.BindRepeaterReader(sql, rptDiscussIt, param);
            }
            catch (Exception err)
            {
                ErrorClass.LogError(err, Request.ServerVariables["URL"]);
                
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
    }//class
}//namespace