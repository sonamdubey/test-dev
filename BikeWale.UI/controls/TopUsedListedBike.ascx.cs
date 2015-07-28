using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Bikewale.Common;

namespace Bikewale.Controls
{
    public class TopUsedListedBike : System.Web.UI.UserControl
    {
        protected HtmlContainerControl noCarsMessage, topUsedCarItems;

        protected Repeater rptListings;
        private string _topRecords;

        public string TopRecords
        {
            get { return _topRecords; }
            set { _topRecords = value; }
        }

        bool _DisplayTwoColumn = false;
        public bool DisplayTwoColumn
        {
            get { return _DisplayTwoColumn; }
            set { _DisplayTwoColumn = value; }
        }

        protected override void OnInit(EventArgs e)
        {
            base.Load += new EventHandler(Page_Load);
        }

        private void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                BindListedBike();
        }//pageload

        private void BindListedBike()
        {
            SqlCommand cmd = new SqlCommand("GeUsedBikeListings");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@TopCount", SqlDbType.SmallInt).Value = TopRecords;

            Database db = new Database();
            SqlDataReader dr = null;
            try
            {
                dr = db.SelectQry(cmd);
                if (dr.HasRows)
                {
                    rptListings.DataSource = dr;
                    rptListings.DataBind();
                }
                else
                {
                    topUsedCarItems.Visible = false;
                    noCarsMessage.Visible = true;
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
                if(dr != null)
                    dr.Close();
                db.CloseConnection();
            }
        }      
    }//class
}//namespace