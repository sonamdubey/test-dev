using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Bikewale.Common;

namespace Bikewale.Controls
{
    public class ForumsMin : System.Web.UI.UserControl
    {
        protected Repeater rptForums;
        private string _topRecords;

        public string TopRecords
        {
            get { return _topRecords; }
            set { _topRecords = value; }
        }        

        protected override void OnInit(EventArgs e)
        {
            base.Load += new EventHandler(Page_Load);
        }
        private void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                BindForums();
        }//pageload

        private void BindForums()
        {
            throw new Exception("Method not used/commented");

            //SqlCommand cmd = new SqlCommand("GetForumsMin");
            //cmd.CommandType = CommandType.StoredProcedure;
            //cmd.Parameters.Add("@TopCount", SqlDbType.SmallInt).Value = TopRecords;

            //Database db = new Database();
            //SqlDataReader dr = null;
            //try
            //{
            //    dr = db.SelectQry(cmd);
            //    if (dr.HasRows)
            //    {
            //        rptForums.DataSource = dr;
            //        rptForums.DataBind();
            //    }
            //}
            //catch (SqlException exSql)
            //{
            //    ErrorClass.LogError(exSql, HttpContext.Current.Request.ServerVariables["URL"]);
            //    
            //}
            //catch (Exception ex)
            //{
            //    //Response.Write(ex.Message);
            //    ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"]);
            //    
            //}
            //finally
            //{
            //    dr.Close();
            //    db.CloseConnection();
            //}
        }

        /// <summary>
        /// Retrun Topic name if the topic name lenght is greater than 30 then it should be substring and showing small string for that
        /// </summary>
        /// <param name="Topic"></param>
        /// <returns></returns>
        protected string FormatedTopic(string Topic)
        {
            string result = Topic.Length > 30 ? Topic.Substring(0, Topic.IndexOf(" ", 25)) + "..." : Topic;
            return result;
        }
    }//class
}//namespace