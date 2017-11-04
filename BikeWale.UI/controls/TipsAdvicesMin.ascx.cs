using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Bikewale.Common;
using System.Data.Common;
using MySql.CoreDAL;

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
            DataTable dt = default(DataTable);
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("gettipsandadvices"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_topcount", DbType.Int16, TopRecords));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_category", DbType.Int16, "5")); // 5 category id for tips and advices.

                    using (DataSet ds = MySqlDatabase.SelectAdapterQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
                        {
                            dt = ds.Tables[0];
                        }
                    }

                }
            }
            catch (SqlException exSql)
            {
                ErrorClass.LogError(exSql, HttpContext.Current.Request.ServerVariables["URL"]);
                
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                
            }
            return dt;
        }
    }//class
}//namespace