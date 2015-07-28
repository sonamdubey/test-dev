using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Bikewale.Common;

namespace Bikewale.Controls
{
    public partial class TipsAdvicesMin : System.Web.UI.UserControl
    {
        protected Repeater rptTipsAdvices;
        protected HtmlGenericControl divControl;
        private string _topRecords = "5";

        public string TopRecords
        {
            get { return _topRecords; }
            set { _topRecords = value; }
        }        

        protected override void OnInit(EventArgs e)
        {
            base.Load += new EventHandler(Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                FetchRoadTests();
        }

       
        private void FetchRoadTests()
        {
            // Bind DataSet to Repeater
            using (DataTable dtR = GetLatestRoadTests())
            {
                if (dtR.Rows.Count > 0)
                {
                    divControl.Attributes.Remove("class");
                    rptTipsAdvices.DataSource = dtR;
                    rptTipsAdvices.DataBind();
                }      
                else
                    divControl.Attributes.Add("class", "hide");
            }
        }

        private DataTable GetLatestRoadTests()
        {
            DataTable dt = new DataTable();
            using (SqlCommand cmd = new SqlCommand("GetTipsAndAdvices"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@TopCount", SqlDbType.SmallInt).Value = TopRecords;
                cmd.Parameters.Add("@Category", SqlDbType.SmallInt).Value = "5"; // 5 category id for tips and advices.
                try
                {
                    Database db = new Database();
                    dt = db.SelectAdaptQry(cmd).Tables[0];
                }
                catch (SqlException exSql)
                {
                    ErrorClass objErr = new ErrorClass(exSql, HttpContext.Current.Request.ServerVariables["URL"]);
                    objErr.SendMail();
                }
                catch (Exception ex)
                {
                    ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                    objErr.SendMail();
                }
            }

            return dt;
        }
    }//class
}//namespace