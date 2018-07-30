using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Bikewale.Common;

///
///Created By : Umesh Ojha on 8/8/2012
///
namespace Bikewale.Controls
{    
    public class UpcomingBikeHome : System.Web.UI.UserControl
    {
        protected Repeater rptUpcomingBikes;

        private string _topRecords;

        public string TopRecords
        {
            get { return _topRecords; }
            set { _topRecords = value; }
        }

        private string _width = "grid_2";

        public string ControlWidth
        {
            get { return _width; }
            set { _width = value; }
        } 
        protected override void OnInit(EventArgs e)
        {
            base.Load += new EventHandler(Page_Load);
        }

        private void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                FetchUpcomingBikes();
        }

        private void FetchUpcomingBikes()
        {
            throw new Exception("Method not used/commented");

            //SqlCommand cmd = new SqlCommand("GetUpcomingBikeMin");
            //cmd.CommandType = CommandType.StoredProcedure;
            //cmd.Parameters.Add("@TopCount", SqlDbType.SmallInt).Value = TopRecords;
            //cmd.Parameters.Add("@ControlWidth", SqlDbType.VarChar, 10).Value = ControlWidth;

            //Database db = new Database();
            //SqlDataReader dr = null;
            //try
            //{
            //    dr = db.SelectQry(cmd);
            //    if (dr.HasRows)
            //    {
            //        rptUpcomingBikes.DataSource = dr;
            //        rptUpcomingBikes.DataBind();
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
            //    if(dr != null)
            //        dr.Close();
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
            string result = Topic.Length > 125 ? Topic.Substring(0, Topic.IndexOf(" ", 110)) + "..." : Topic;
            return result;
        }
    }
}