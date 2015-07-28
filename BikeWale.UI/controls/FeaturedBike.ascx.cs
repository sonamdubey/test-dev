using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Bikewale.Common;

///
///created by umesh for home page featured bike
///
namespace Bikewale.Controls
{
    public class FeaturedBike : System.Web.UI.UserControl
    {
        protected Repeater rptFeaturedBike;

        private string _topRecords = "5";

        public string TopRecords
        {
            get { return _topRecords; }
            set { _topRecords = value; }
        }

        private string _width = "grid_2";

        public string ControlWidth
        {
            get { return _width ; }
            set { _width = value; }
        }

        private string _imageWidth = "136px;";

        public string ImageWidth
        {
            get { return _imageWidth; }
            set { _imageWidth = value; }
        }

        protected override void OnInit(EventArgs e)
        {
            base.Load += new EventHandler(Page_Load);
        }
        private void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {  
                FetchFeaturedBikes();                
            }
        }       

        private void FetchFeaturedBikes()
        {
            SqlCommand cmd = new SqlCommand("GetFeaturedBikeMin");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@TopCount", SqlDbType.SmallInt).Value = TopRecords;
            cmd.Parameters.Add("@ControlWidth", SqlDbType.VarChar,10).Value = ControlWidth;

            Database db = new Database();
            SqlDataReader dr = null;
            try
            {
                dr = db.SelectQry(cmd);
                if (dr.HasRows)
                {
                    rptFeaturedBike.DataSource = dr;
                    rptFeaturedBike.DataBind();
                }              
            }
            catch (SqlException exSql)
            {
                ErrorClass objErr = new ErrorClass(exSql, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            catch (Exception ex)
            {
                //Response.Write(ex.Message);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            finally
            {
                if (dr != null)
                {
                    dr.Close();
                }                
                db.CloseConnection();
            }
        }

        /// <summary>
        /// Retrun Topic name if the topic name lenght is greater than 30 then it should be substring and showing small string for that
        /// </summary>
        /// <param name="Topic"></param>
        /// <returns></returns>
        protected string FormatedTopic(string Topic)
        {
            Trace.Warn("Topic.Length : ", Topic.Length.ToString());
            string result = Topic.Length > 100 ? Topic.Substring(0, Topic.IndexOf(" ", 90)) + "..." : Topic;
            Trace.Warn("topic liength : ", result.Length.ToString());
            return result;
        }
    }//class
}//namespace